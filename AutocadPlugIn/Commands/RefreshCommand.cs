using System;
using System.Windows.Forms;
using Gssoft.Gscad.ApplicationServices;
using Gssoft.Gscad.DatabaseServices;
using Gssoft.Gscad.EditorInput;

namespace RBAutocadPlugIn.Commands
{
    public class RefreshCommand : Command
    {
        public void Execute(object parameter)
        {
            try
            {
                var doc = Application.DocumentManager.MdiActiveDocument;
                if (doc == null) return;

                var ed = doc.Editor;
                var db = doc.Database;

                // Start a transaction
                using (var tr = db.TransactionManager.StartTransaction())
                {
                    try
                    {
                        // Regenerate the current view
                        ed.Regen(RegenFlags.ForceViewAndShade);
                        
                        // Update the display
                        ed.UpdateScreen();
                        ed.WriteMessage("\nView refreshed.\n");
                        
                        tr.Commit();
                    }
                    catch (Exception ex)
                    {
                        tr.Abort();
                        throw new Exception($"Failed to refresh view: {ex.Message}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to refresh: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
