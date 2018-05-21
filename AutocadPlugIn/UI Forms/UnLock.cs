using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CADController.Commands;
using AdvancedDataGridView;
using System.IO;
using CADController.Controllers;
using CADController;
namespace AutocadPlugIn.UI_Forms
{
    public partial class UnLock : Form
    {
        public ArrayList drawings = new ArrayList();
        ICADManager cadManager = new AutoCADManager();
       
        public UnLock()
        {
            InitializeComponent();
        }

        private void UnLock_Load(object sender, EventArgs e)
        {
            try
            {
            this.imageStrip.ImageSize = new System.Drawing.Size(17, 17);
            this.imageStrip.TransparentColor = System.Drawing.Color.Magenta;
            this.imageStrip.ImageSize = new Size(17, 17);
            this.imageStrip.Images.AddStrip(Properties.Resources.LockImageStrip1);

            UnLockTree.ImageList = imageStrip;

            System.Data.DataTable dtTreeGridData = new System.Data.DataTable();


            int totalCount = UnLockTree.Nodes.Count;

            int counter = 0;
            for (int i = 0; i < totalCount; i++)
            {
                TreeGridNode TreeNode1 = UnLockTree.Nodes.ElementAt(0);
                this.ClearTreeView(TreeNode1);
                UnLockTree.Nodes.Remove(TreeNode1);
            }
            
            dtTreeGridData = cadManager.GetExternalRefreces();

            TreeGridNode node = new TreeGridNode();

                UnlockCommand lockStatusCmd = new UnlockCommand();
                lockStatusCmd.DrawingInfo = dtTreeGridData;
                UnlockController lockStatusCon = new UnlockController();

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
                    
                    node = UnLockTree.Nodes.Add(false, rw["drawingName"], rw["drawingnumber"], rw["classification"], rw["revision"], rw["drawingid"], rw["lockstatus"], rw["projectname"],rw["projectid"],rw["drawingstate"]);

                    if (rw["lockstatus"].ToString() == "1")
                        node.ImageIndex = 0;
                    else if (rw["lockstatus"].ToString() == "0")
                        node.Cells[0].ReadOnly = true;
                    else if (rw["lockstatus"].ToString() == "2")
                        node.ImageIndex = 1;

                    node.Expand();
                    counter = 1;
                }
                else
                        {
                            TreeGridNode node1 = node.Nodes.Add(false, rw["DrawingName"], rw["drawingnumber"], rw["classification"], rw["revision"], rw["drawingid"], rw["lockstatus"], rw["projectname"], rw["projectid"], rw["drawingstate"]);

                    if (rw["lockstatus"].ToString() == "1")
                        node1.ImageIndex = 0;
                    else if (rw["lockstatus"].ToString() == "0")
                        node1.Cells[0].ReadOnly = true;
                    else if (rw["lockstatus"].ToString() == "2")
                        node1.ImageIndex = 1;
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
                for (int nodeCount = 0; nodeCount < CurrentNode.Nodes.Count ; nodeCount++)
                {
                    if (CurrentNode.Nodes.ElementAt(nodeCount).Nodes.Count() == 0)
{
                        UnLockTree.Nodes.Remove(CurrentNode.Nodes.ElementAt(nodeCount));
        }
                    else
                    {
                        TreeGridNode node1 = CurrentNode.Nodes.ElementAt(nodeCount);
                        this.ClearTreeView(node1);
                        UnLockTree.Nodes.Remove(node1);
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

        private void UnLockBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (drawings.Count != 0)
                {
                    //cadManager.UnLockActiveDocument();

                    UnlockCommand unLockCmd = new UnlockCommand();
                    unLockCmd.DrawingIds = drawings;

                    UnlockController unLockCnt = new UnlockController();
                    unLockCnt.Execute(unLockCmd);

                    if (unLockCnt.errorString != null)
                    {
                        MessageBox.Show(unLockCnt.errorString);
                        return;
                    }
                    

                    MessageBox.Show("Succesfully Unlocked");
                    //CADRibbon ribbon = new CADRibbon();
                    //ribbon.browseDEnable = true;
                    //ribbon.createDEnable = true;
                    //ribbon.browseBEnable = true;
                    //ribbon.createBEnable = true;
                    //ribbon.LockEnable = true;
                    //ribbon.SaveEnable = false;
                    //ribbon.UnlockEnable = false;
                    //ribbon.DrawingInfoEnable = true;
                    //ribbon.MyRibbon();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Please Select drawing first");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Exception Occur: " + ex.Message);
            }
        }

        private void UnLockTree_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                //  MessageBox.Show("expand Node");
                dataGridView1 = (System.Windows.Forms.DataGridView)UnLockTree;
                TreeGridNode TreeNode1 = (TreeGridNode)dataGridView1.Rows[e.RowIndex];

                try
                {
                    TreeNode1.Expand();
                    String id = (string)TreeNode1.Cells[5].Value;
                    if (TreeNode1.Cells[0].ReadOnly)
                    {
                        MessageBox.Show("Drawing " + (string)TreeNode1.Cells[1].Value + "is not locked");
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
                    MessageBox.Show("\nProblem reading/processing \"{0}\": {1}", ex.Message);
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

        private void UnLock_Resize(object sender, EventArgs e)
        {
            int f_height = this.Height;
            int f_width = this.Width;
            UnLockBtn.Location = new Point((f_width / 2)-100, f_height - 85);
            CancelBtn.Location = new Point((f_width / 2) + 100, f_height - 85);
        }
        
    }
}
