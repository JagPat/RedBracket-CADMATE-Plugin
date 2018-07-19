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
    public partial class frmError : Form
    {
        public frmError(string UserMessage,string TechnicalMessage)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            txtTechnicalMsg.Text = TechnicalMessage;
            lblUserMessage.Text = UserMessage;
            panel1.BackgroundImage = SystemIcons.Error.ToBitmap();
            panel1.BackgroundImageLayout = ImageLayout.Center;
        }

        private void frmError_Load(object sender, EventArgs e)
        {
            
        }

        private void btnShowMessage_Click(object sender, EventArgs e)
        {
            btnShowMessage.Visible = false;

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
