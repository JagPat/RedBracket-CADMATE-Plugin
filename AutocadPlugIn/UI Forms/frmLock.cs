using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
 
using AdvancedDataGridView;
using System.IO;
 
using System.Collections;

namespace AutocadPlugIn.UI_Forms
{
    public partial class frmLock : Form
    {
        public ArrayList drawings = new ArrayList();
        ICADManager cadManager = new AutoCADManager();
        public frmLock()
        {
            InitializeComponent();
        }

        private void frmLock_Load(object sender, EventArgs e)
        {
            try
            {
                this.imageStrip.ImageSize = new System.Drawing.Size(17, 17);
                this.imageStrip.TransparentColor = System.Drawing.Color.Magenta;
                this.imageStrip.ImageSize = new Size(17, 17);
                this.imageStrip.Images.AddStrip(Properties.Resources.LockImageStrip1);

                LockTree.ImageList = imageStrip;

                System.Data.DataTable dtTreeGridData = new System.Data.DataTable();


                int totalCount = LockTree.Nodes.Count;

                int counter = 0;
                for (int i = 0; i < totalCount; i++)
                {
                    TreeGridNode TreeNode1 = LockTree.Nodes.ElementAt(0);
                    this.ClearTreeView(TreeNode1);
                    LockTree.Nodes.Remove(TreeNode1);
                }

                dtTreeGridData = cadManager.GetExternalRefreces();

                TreeGridNode node = new TreeGridNode();

                LockCommand lockStatusCmd = new LockCommand();
                lockStatusCmd.DrawingInfo = dtTreeGridData;
                LockController lockStatusCon = new LockController();

                if (lockStatusCon.errorString != null)
                {
                    MessageBox.Show(lockStatusCon.errorString);
                    return;
                }
                dtTreeGridData = lockStatusCon.getLockStatus(lockStatusCmd);

                foreach (DataRow rw in dtTreeGridData.Rows)
                {

                    if (counter == 0)
                    {

                        node = LockTree.Nodes.Add(false, rw["drawingName"], rw["drawingnumber"], rw["classification"], rw["revision"], rw["drawingid"], rw["lockstatus"], rw["lockby"], rw["projectname"], rw["projectid"], rw["drawingstate"]);

                        if (rw["lockstatus"].ToString() == "1")
                        {
                            node.ImageIndex = 0;
                            node.Cells[0].ReadOnly = true;
                        }
                        else if (rw["lockstatus"].ToString() == "2")
                        {
                            node.ImageIndex = 1;
                            node.Cells[0].ReadOnly = true;
                        }

                        node.Expand();
                        counter = 1;
                    }
                    else
                    {
                        TreeGridNode node1 = node.Nodes.Add(false, rw["DrawingName"], rw["drawingnumber"], rw["classification"], rw["revision"], rw["drawingid"], rw["lockstatus"], rw["lockby"], rw["projectname"], rw["projectid"], rw["drawingstate"]);

                        if (rw["lockstatus"].ToString() == "1")
                        {
                            node1.ImageIndex = 0;
                            node1.Cells[0].ReadOnly = true;
                        }
                        else if (rw["lockstatus"].ToString() == "2")
                        {
                            node1.ImageIndex = 1;
                            node1.Cells[0].ReadOnly = true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
                return;
            }
           // MessageBox.Show("Hi.");
        }

        private void LockBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (drawings.Count != 0)
                {
                    //cadManager.LockActiveDocument();
                    if (!LockDrawings(drawings))
                    {


                        MessageBox.Show("Succesfully Locked");

                        //CADRibbon ribbon = new CADRibbon();
                        //ribbon.browseDEnable = true;
                        //ribbon.createDEnable = true;
                        //ribbon.browseBEnable = true;
                        //ribbon.createBEnable = true;
                        //ribbon.LockEnable = false;
                        //ribbon.SaveEnable = true;
                        //ribbon.UnlockEnable = true;
                        //ribbon.DrawingInfoEnable = true;
                        //ribbon.MyRibbon();

                        this.Hide();
                    }
                }
                else
                {
                    MessageBox.Show("Please Select drawing first");
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
                return;
            }
        }
        public bool LockDrawings(ArrayList drawingIDs)
        {
            bool IsError = false;
            LockCommand lockCmd = new LockCommand();
            lockCmd.DrawingIds = drawingIDs;

            LockController lockCnt = new LockController();
            lockCnt.Execute(lockCmd);
            if (lockCnt.errorString != null)
            {
                MessageBox.Show(lockCnt.errorString);
                IsError = true;
                return IsError;
            }
            return IsError;
        }
        private System.Windows.Forms.DataGridView dataGridView1;
        private void LockTree_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                try
                {
                    dataGridView1 = (System.Windows.Forms.DataGridView)LockTree;
                    TreeGridNode TreeNode1 = (TreeGridNode)dataGridView1.Rows[e.RowIndex];

                    TreeNode1.Expand();
                    String id = (string)TreeNode1.Cells[5].Value;

                    if ((bool)TreeNode1.Cells[0].ReadOnly)
                    {
                        if ((String)TreeNode1.Cells[6].Value == "1")
                            MessageBox.Show("Drawing " + (String)TreeNode1.Cells[1].Value + " is already Locked by you");
                        else if ((String)TreeNode1.Cells[6].Value == "2")
                            MessageBox.Show("Drawing " + (String)TreeNode1.Cells[1].Value + " is already Locked by: " + (String)TreeNode1.Cells[7].Value);
                    }
                    else if ((bool)TreeNode1.Cells[0].EditedFormattedValue)
                    {
                        drawings.Add(id);
                    }
                    else
                    {
                        drawings.Remove(id);
                    }
                    this.ExpandNodeForCheck(TreeNode1);
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
                    return;
                }
            }
        }
        private void ExpandNodeForCheck(TreeGridNode CurrentNode)
        {
            string id = null;
            try
            {

                for (int nodeCount = 0; nodeCount < CurrentNode.Nodes.Count(); nodeCount++)
                {
                    if (CurrentNode.Nodes.ElementAt(nodeCount).Nodes.Count() == 0)
                    {
                        if (!CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].ReadOnly)
                        {
                            CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].Value = CurrentNode.Cells[0].EditedFormattedValue;

                            id = (string)CurrentNode.Nodes.ElementAt(nodeCount).Cells[5].Value;

                            if ((bool)CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].Value)
                                drawings.Add(id);
                            else
                                drawings.Remove(id);
                        }
                    }
                    else
                    {
                        TreeGridNode node1 = CurrentNode.Nodes.ElementAt(nodeCount);
                        CurrentNode.Nodes.ElementAt(nodeCount).Expand();

                        if (!CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].ReadOnly)
                        {
                            CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].Value = CurrentNode.Cells[0].EditedFormattedValue;

                            id = (string)CurrentNode.Nodes.ElementAt(nodeCount).Cells[5].Value;

                            if ((bool)CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].Value)
                                drawings.Add(id);
                            else
                                drawings.Remove(id);
                            this.ExpandNodeForCheck(node1);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
                return;
            }


        }
        private void ClearTreeView(TreeGridNode CurrentNode)
        {
            try
            {
                for (int nodeCount = 0; nodeCount < CurrentNode.Nodes.Count; nodeCount++)
                {
                    if (CurrentNode.Nodes.ElementAt(nodeCount).Nodes.Count() == 0)
                    {
                        LockTree.Nodes.Remove(CurrentNode.Nodes.ElementAt(nodeCount));
                    }
                    else
                    {
                        TreeGridNode node1 = CurrentNode.Nodes.ElementAt(nodeCount);
                        this.ClearTreeView(node1);
                        LockTree.Nodes.Remove(node1);
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
                return;
            }

        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
