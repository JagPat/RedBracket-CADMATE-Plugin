using System;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using Gssoft.Gscad.ApplicationServices;
using Gssoft.Gscad.DatabaseServices;
using Gssoft.Gscad.EditorInput; 

namespace RBAutocadPlugIn
{
    public class SaveCommand : Command
    {
        private List<String> _AllDrawing = new List<String>();
        public List<String> AllDrawing
        {
            get { return this._AllDrawing; }
            set { this._AllDrawing = value; }
        }
        private List<String> drawingList = new List<String>();

        public List<String> Drawings
        {
            get { return this.drawingList; }
            set { this.drawingList = value; }
        }
        private  String filePath ;

        public  String FilePath
        {
            get { return this.filePath; }
            set { this.filePath = value; }
        }
        private List<String> newDrawings = new List<String>();

        public List<String> NewDrawings
        {
            get { return this.newDrawings; }
            set { this.newDrawings = value; }        
        }

        private System.Data.DataTable drawingInfo;
        public System.Data.DataTable DrawingInfo
        {
            get { return this.drawingInfo; }
            set { this.drawingInfo = value; }
        }

        private String filePreFix;

        public String FilePreFix
        {
            get { return this.filePreFix; }
            set { this.filePreFix = value; }
        }
    
        private String projectID;
        public String ProjectID
        {
            get { return this.projectID; }
            set { this.projectID = value; }
        }

        public void Execute(object parameter)
        {
            try
            {
                var doc = Gssoft.Gscad.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                if (doc == null) return;

                var db = doc.Database;
                var ed = doc.Editor;

                // Start a transaction
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    try
                    {
                        // Save the current document
                        doc.Database.SaveAs(doc.Name, true, DwgVersion.Current, doc.Database.SecurityParameters);
                        
                        // Update the drawing list if needed
                        if (!Drawings.Contains(doc.Name))
                        {
                            Drawings.Add(doc.Name);
                            NewDrawings.Add(doc.Name);
                        }
                        
                        // Update the UI if a CADRibbon instance was passed as parameter
                        if (parameter is CADRibbon ribbon)
                        {
                            ribbon.ShowMessage("Drawing saved successfully.", "Save Complete", System.Windows.Forms.MessageBoxIcon.Information);
                        }
                        
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        tr.Abort();
                        throw new Exception($"Failed to save drawing: {ex.Message}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
   
    }
