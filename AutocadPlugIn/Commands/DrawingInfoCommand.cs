using System;
using System.Windows.Forms;
using RBAutocadPlugIn.UI_Forms;

namespace RBAutocadPlugIn.Commands
{
    public class DrawingInfoCommand : Command
    {
        public void Execute(object parameter)
        {
            try
            {
                using (var drawingInfoForm = new frmDrawingInfo())
                {
                    drawingInfoForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to show Drawing Info: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
