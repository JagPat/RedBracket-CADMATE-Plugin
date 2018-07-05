using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms; 

namespace AutocadPlugIn.UI_Forms
{
    public partial class GetDrawinDetail : Form
    {
        public static String DrawingDetail = null;
        
        public GetDrawinDetail()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            frmSave_Active_Drawings objsave = new frmSave_Active_Drawings();
            DrawingDetail = txtDrawingNumber.Text + ";" + cmbType.SelectedValue.ToString() + ";" + cmbType.Text.ToString();
            this.Close();
        }
        private void GetDrawinDetail_Load(object sender, EventArgs e)
        {
            

        }
    }
}
