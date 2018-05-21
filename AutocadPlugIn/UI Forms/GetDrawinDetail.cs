using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CADController.Configuration;

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
            Save_Active_Drawings objsave = new Save_Active_Drawings();
            DrawingDetail = txtDrawingNumber.Text + ";" + cmbType.SelectedValue.ToString() + ";" + cmbType.Text.ToString();
            this.Close();
        }
        private void GetDrawinDetail_Load(object sender, EventArgs e)
        {
            # region "Load CAD Type in Combobox"
            CADIntegrationConfiguration objWordConfig = new CADIntegrationConfiguration();
            Hashtable htdisplayName = null;
            DataTable dtDocTypes = new DataTable();
            dtDocTypes.Columns.Add("DisplayName", typeof(string));
            dtDocTypes.Columns.Add("ClassificationPath", typeof(string));
            dtDocTypes.Rows.Add("Non", "Non");
            htdisplayName = objWordConfig.GetClassification();
            ICollection keys = htdisplayName.Keys;
            IEnumerator getNames = keys.GetEnumerator();
            while (getNames.MoveNext())
            {
                dtDocTypes.Rows.Add(getNames.Current.ToString(), htdisplayName[getNames.Current].ToString());
            }
            cmbType.DataSource = dtDocTypes;
            cmbType.DisplayMember = "DisplayName";
            cmbType.ValueMember = "ClassificationPath";
            #endregion

        }
    }
}
