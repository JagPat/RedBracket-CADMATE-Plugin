using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

using AdvancedDataGridView;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;

namespace AutocadPlugIn
{
    public partial class OpenDocument : Form
    {
       // Connect.Connector cn = new Connect.Connector();
        private System.ComponentModel.IContainer components = null;


        public OpenDocument()
        {
          // acadApp.ShowAlertDialog("1");
           InitializeComponent();
        }
        private void OpenDocument_Load(object sender, System.EventArgs e)
		{
           // Check.DefaultCellStyle.NullValue = null;
           
            // load image strip
           // acadApp.ShowAlertDialog("12");
          this.imageStrip.ImageSize = new System.Drawing.Size(16, 16);
          // acadApp.ShowAlertDialog("12_1");
          this.imageStrip.TransparentColor = System.Drawing.Color.Magenta;
          //acadApp.ShowAlertDialog("12_2");
           this.imageStrip.ImageSize = new Size(16, 16);
         //  acadApp.ShowAlertDialog("12_3");
          // this.imageStrip.Images.AddStrip(AutocadPlugIn.Properties.Resources.newGroupPostIconStrip1);
        //   acadApp.ShowAlertDialog("12_4");
            treeGridView1.ImageList = imageStrip;
          //  acadApp.ShowAlertDialog("12_5");
            // attachment header cell
         //   this.Check.HeaderCell = new AttachmentColumnHeader(imageStrip.Images[2]);

            
    }

        private void OpenDocument_Shown(object sender, EventArgs e)
        {
            Font boldFont = new Font(treeGridView1.DefaultCellStyle.Font, FontStyle.Bold);

           // acadApp.ShowAlertDialog("13");

            TreeGridNode node = treeGridView1.Nodes.Add(false, "Class Room", "DR-2586", "CAD Drawing", "A");
            node.ImageIndex = 0;
            node.DefaultCellStyle.Font = boldFont;

  
            node = node.Nodes.Add(false, "Chair Block", "BK-619", "CAD Block", "B");
            node.ImageIndex = 1;

            node = treeGridView1.Nodes.Add(false, "Class Room", "DR-2586", "CAD Drawing", "A");
            node.ImageIndex = 0;
            node.DefaultCellStyle.Font = boldFont;

     
            node = node.Nodes.Add(false, "Chair Block", "BK-619", "CAD Block", "B");
            node.ImageIndex = 1;

          
            
        }

        internal class AttachmentColumnHeader : DataGridViewColumnHeaderCell
        {
            public Image _image;
            public AttachmentColumnHeader(Image img)
                : base()
            {
                this._image = img;
            }
            protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                graphics.DrawImage(_image, cellBounds.X + 4, cellBounds.Y + 2);
            }
            protected override object GetValue(int rowIndex)
            {
                return null;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
         /*   dataGridView1 = (System.Windows.Forms.DataGridView)treeGridView1;
            int numberOfRow = this.dataGridView1.Rows.Count;
            for (int i = 0; i < numberOfRow; i++)
            {
                
               acadApp.ShowAlertDialog(Convert.ToString(dataGridView1.Rows[i].GetHashCode()));
            }
           */
            IEnumerator<TreeGridNode> it = this.treeGridView1.Nodes.GetEnumerator();
            while (it.MoveNext())
            {
                MessageBox.Show(Convert.ToString(it.GetHashCode()));
               // int index = it.Current.RowIndex;
                //int index = it.Current.DataGridView.Rows.GetHashCode();
                //bool index = it.Current.Expand();
                //bool index = 
                int index;
                if (it.Current.HasChildren)
                {
                     index = it.Current.HasChildren.GetHashCode();
                }
                else
                     index = it.Current.GetHashCode();
                MessageBox.Show(Convert.ToString(index));
            }
           /* for ( TreeGridNode it = (TreeGridNode)treenodes.GetEnumerator(); ; )
            {
                while (it.HasChildren)
                {
                    MessageBox.Show(Convert.ToString(it.GetHashCode()));
                }
            }*/
            /*int nodeCount = treeGridView1.Nodes.Count;
            MessageBox.Show(Convert.ToString(nodeCount));
            int j = 0;
            while (j < nodeCount)
            {
                
            }*/
            
        }

        private void treeGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                dataGridView1 = (System.Windows.Forms.DataGridView)treeGridView1;
                dataGridView1.Rows[e.RowIndex].Cells[0].Value = (bool)treeGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue;
                string val = treeGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                
                MessageBox.Show(Convert.ToString(e.RowIndex));
                MessageBox.Show(Convert.ToString(dataGridView1.Rows[e.RowIndex].GetHashCode()));
                MessageBox.Show(Convert.ToString(treeGridView1.SelectedRows[0].GetHashCode()));

                
                MessageBox.Show(val); 
            }

        }

	}
	
}
