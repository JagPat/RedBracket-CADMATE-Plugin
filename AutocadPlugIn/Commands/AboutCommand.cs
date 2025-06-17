using System;
using System.Windows.Forms;
using RBAutocadPlugIn.UI_Forms;

namespace RBAutocadPlugIn.Commands
{
    public class AboutCommand : Command
    {
        public void Execute(object parameter)
        {
            try
            {
                using (var aboutForm = new frmAbout())
                {
                    aboutForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to show About dialog: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
