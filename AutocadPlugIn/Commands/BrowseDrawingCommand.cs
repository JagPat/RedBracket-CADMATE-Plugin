using System;
using System.Windows.Forms;
using Gssoft.Gscad.ApplicationServices;
using Gssoft.Gscad.DatabaseServices;
using Gssoft.Gscad.EditorInput;
using Gssoft.Gscad.Runtime;
using System.IO;

namespace RBAutocadPlugIn.Commands
{
    public class BrowseDrawingCommand : Command
    {
        public void Execute(object parameter)
        {
            try
            {
                var doc = Application.DocumentManager.MdiActiveDocument;
                if (doc == null) return;

                var ed = doc.Editor;
                var db = doc.Database;

                // Show open file dialog
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Drawing Files (*.dwg)|*.dwg|All Files (*.*)|*.*";
                    openFileDialog.Title = "Open Drawing";
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.Multiselect = false;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = openFileDialog.FileName;

                        // Check if file exists
                        if (!File.Exists(filePath))
                        {
                            ed.WriteMessage("\nFile not found: " + filePath);
                            return;
                        }

                        // Start a transaction
                        using (var tr = db.TransactionManager.StartTransaction())
                        {
                            try
                            {
                                // Close the current document if it's not saved
                                if (!doc.Database.Saved)
                                {
                                    var result = MessageBox.Show(
                                        "Save changes to the current drawing?", 
                                        "Save Changes", 
                                        MessageBoxButtons.YesNoCancel, 
                                        MessageBoxIcon.Question);

                                    if (result == DialogResult.Yes)
                                    {
                                        doc.Database.SaveAs(doc.Name, true, DwgVersion.Current, doc.Database.SecurityParameters);
                                    }
                                    else if (result == DialogResult.Cancel)
                                    {
                                        return;
                                    }
                                }

                                // Open the selected drawing
                                doc.Database.ReadDwgFile(filePath, FileShare.ReadWrite, true, null);
                                doc.Database.RetainOriginalThumbnailBitmap = true;
                                doc.Database.ThumbnailBitmapMode = ThumbnailUpdateMode.Update;

                                // Update the UI if a CADRibbon instance was passed as parameter
                                if (parameter is CADRibbon ribbon)
                                {
                                    ribbon.ShowMessage($"Opened drawing: {Path.GetFileName(filePath)}");
                                }

                                tr.Commit();
                            }
                            catch (Exception ex)
                            {
                                tr.Abort();
                                throw new Exception($"Failed to open drawing: {ex.Message}", ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to browse drawing: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
