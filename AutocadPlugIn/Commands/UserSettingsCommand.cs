using System;
using System.Windows.Forms;
using RBAutocadPlugIn.UI_Forms;

namespace RBAutocadPlugIn.Commands
{
    public class UserSettingsCommand : Command
    {
        public void Execute(object parameter)
        {
            try
            {
                using (var settingsForm = new frmUserSettings())
                {
                    settingsForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to show User Settings: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
