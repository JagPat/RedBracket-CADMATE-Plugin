using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutocadPlugIn
{
    public partial class frmProgressBar : Form
    {
        public frmProgressBar()
        {
            InitializeComponent(); this.FormBorderStyle = FormBorderStyle.None;
            pnlTop.BackColor = pnlRight.BackColor = pnlLeft.BackColor = pnlBottom.BackColor =Color.FromArgb(23,110,110);
            
        }

        private void frmProgressBar_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch(Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        private void lblStatus_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lblStatus.Refresh();
                pbProcess.Refresh();
                this.Refresh();
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }
    }
}
