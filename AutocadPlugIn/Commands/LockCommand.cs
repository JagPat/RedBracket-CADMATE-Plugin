using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using Gssoft.Gscad.ApplicationServices;
using Gssoft.Gscad.DatabaseServices;
using Gssoft.Gscad.EditorInput;

namespace RBAutocadPlugIn
{
    public class LockCommand : Command
    {
        private ArrayList drawingIds;
        public ArrayList DrawingIds
        {
            get { return this.drawingIds; }
            set { this.drawingIds = value; }
        }

        private System.Data.DataTable drawingInfo;
        public System.Data.DataTable DrawingInfo
        {
            get { return this.drawingInfo; }
            set { this.drawingInfo = value; }
        }

        //private String projectname;
        //public String ProjectName
        //{
        //    get { return this.projectname; }
        //    set { this.projectname = value; }
        //}

        //private String drawingType;
        //public String DrawingType
        //{
        //    get { return this.drawingType; }
        //    set { this.drawingType = value; }
        //}

        public void Execute(object parameter)
        {
            var doc = Gssoft.Gscad.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;

            var db = doc.Database;
            var ed = doc.Editor;

            try
            {
                // Check if we have drawing info
                if (drawingInfo == null || drawingInfo.Rows.Count == 0)
                {
                    ed.WriteMessage("\nNo drawing information available for locking.");
                    return;
                }

                // Start a transaction
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    try
                    {
                        // Get the current drawing name
                        string drawingName = Path.GetFileNameWithoutExtension(db.OriginalFileName);
                        
                        // Find the drawing in our drawing info
                        DataRow[] rows = drawingInfo.Select($"DrawingName = '{drawingName}'");
                        if (rows.Length == 0)
                        {
                            ed.WriteMessage($"\nDrawing '{drawingName}' not found in project.");
                            return;
                        }

                        // Check if already locked
                        var drawingRow = rows[0];
                        string lockStatus = drawingRow["LockStatus"].ToString();
                        
                        if (lockStatus == "1")
                        {
                            ed.WriteMessage("\nThis drawing is already locked by you.");
                            return;
                        }
                        else if (lockStatus == "2")
                        {
                            string lockedBy = drawingRow["LockedBy"].ToString();
                            ed.WriteMessage($"\nThis drawing is already locked by another user: {lockedBy}");
                            return;
                        }

                        // Update the lock status in the database
                        // In a real implementation, this would call a service to update the lock status
                        drawingRow["LockStatus"] = "1"; // 1 = Locked by current user
                        drawingRow["LockedBy"] = Environment.UserName;
                        drawingRow["LockedDate"] = DateTime.Now;

                        // Add to locked drawings list if not already present
                        if (drawingIds == null)
                            drawingIds = new ArrayList();
                            
                        if (!drawingIds.Contains(drawingName))
                            drawingIds.Add(drawingName);

                        // Commit the transaction
                        tr.Commit();

                        // Update UI if a CADRibbon instance was passed as parameter
                        if (parameter is CADRibbon ribbon)
                        {
                            ribbon.SetLockStatus(true);
                            ribbon.ShowMessage("Drawing Locked", $"Drawing '{drawingName}' locked successfully.", MessageBoxIcon.Information);
                        }

                        // Log to command line
                        ed.WriteMessage($"\nDrawing '{drawingName}' locked successfully.");
                        
                        // Commit the transaction
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        tr.Abort();
                        throw new Exception($"Failed to lock drawing: {ex.Message}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to lock: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}