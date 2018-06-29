using System;
using System.Collections; 
using System.Data;
using System.Drawing; 
using System.Windows.Forms;
using CADController.Commands;
using AdvancedDataGridView; 
using CADController.Controllers;
using CADController;
using RedBracketConnector;
namespace AutocadPlugIn.UI_Forms
{
    public partial class frmLockUnLock : Form
    {
        public ArrayList drawings = new ArrayList();
        ICADManager cadManager = new AutoCADManager();

        public frmLockUnLock()
        {
            InitializeComponent();this.FormBorderStyle = FormBorderStyle.None;
        }

        private void UnLock_Load(object sender, EventArgs e)
        {
            try
            {
                UnLockTree.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);
                UnLockTree.RowsDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Regular);
                this.imageStrip.ImageSize = new System.Drawing.Size(17, 17);
                this.imageStrip.TransparentColor = System.Drawing.Color.Magenta;
                this.imageStrip.ImageSize = new Size(17, 17);
                this.imageStrip.Images.AddStrip(Properties.Resources.LockImageStrip1);

                UnLockTree.ImageList = imageStrip;
                UnLockTree.ImageList.Images.Add(Properties.Resources.Unlock);
                System.Data.DataTable dtTreeGridData = new System.Data.DataTable();


                UnLockTree.Nodes.Clear();

                int counter = 0;
                

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

                        node = UnLockTree.Nodes.Add("", rw["drawingName"], rw["drawingnumber"], rw["classification"], rw["drawingstate"], rw["revision"],
                            rw["projectname"],
                            rw["drawingid"], rw["lockstatus"],   rw["lockby"]);

                        if (rw["lockstatus"].ToString() == "1")
                        {
                            node.ImageIndex = 0;
                            node.Cells[0].Value = "Unlock";
                        }

                        else if (rw["lockstatus"].ToString() == "0")
                        {
                             node.ImageIndex = 2;
                            node.Cells[0].Value = "Lock";
                        }

                        else if (rw["lockstatus"].ToString() == "2")
                        {
                            node.ImageIndex = 1;
                            node.Cells[0].Value = "Lock";
                            node.Cells[0].ReadOnly = true;
                        }


                        node.Expand();
                        counter = 1;
                    }
                    else
                    {
                        TreeGridNode node1 = node.Nodes.Add("", rw["drawingName"], rw["drawingnumber"], rw["classification"], rw["drawingstate"], rw["revision"],
                            rw["projectname"],
                            rw["drawingid"], rw["lockstatus"], rw["lockby"]);
                        if (rw["lockstatus"].ToString() == "1")
                        {
                            node1.ImageIndex = 0;
                            node1.Cells[0].Value = "Unlock";
                        }

                        else if (rw["lockstatus"].ToString() == "0")
                        {
                            node1.ImageIndex = 2;
                            node1.Cells[0].Value = "Lock";
                        }

                        else if (rw["lockstatus"].ToString() == "2")
                        {
                            node1.ImageIndex = 1;
                            node1.Cells[0].Value = "Lock";
                            node1.Cells[0].ReadOnly = true;
                        }
                    }
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return;
            }
        }

        

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    String id = (string)TreeNode1.Cells["DrawingID"].Value;
                    if (TreeNode1.Cells[0].ReadOnly)
                    {
                        MessageBox.Show("Drawing " + (string)TreeNode1.Cells[1].Value + "is locked by +" + (string)TreeNode1.Cells["LockedBy"].Value);
                    }
                    else if (Convert.ToString(TreeNode1.Cells["LockStatus"].Value) == "1")
                    {
                        drawings.Clear();
                        drawings.Add(id);

                       
                        UnlockCommand unLockCmd = new UnlockCommand();
                        unLockCmd.DrawingIds = drawings;

                        UnlockController unLockCnt = new UnlockController();
                        unLockCnt.Execute(unLockCmd);

                        if (unLockCnt.errorString != null)
                        {
                            ShowMessage.ErrorMess(unLockCnt.errorString);
                            return;
                        }
                        TreeNode1.ImageIndex=2;
                        TreeNode1.Cells[0].Value = "Lock";
                        TreeNode1.Cells["LockStatus"].Value = "0";
                        TreeNode1.Cells["LockedBy"].Value = "";
                        ShowMessage.InfoMess("Succesfully Unlocked");
                    }
                    else if (Convert.ToString(TreeNode1.Cells["LockStatus"].Value) == "0")
                    {
                        drawings.Clear();
                        drawings.Add(id);


                        LockCommand LockCmd = new LockCommand();
                        LockCmd.DrawingIds = drawings;

                        LockController  LockCnt = new LockController();
                        LockCnt.Execute(LockCmd);

                        if (LockCnt.errorString != null)
                        {
                            ShowMessage.ErrorMess(LockCnt.errorString);
                            return;
                        }
                        TreeNode1.Cells[0].Value = "Unlock";
                        TreeNode1.Cells["LockStatus"].Value = "1";
                        TreeNode1.Cells["LockedBy"].Value = Helper.UserFullName;
                        TreeNode1.ImageIndex = 0;
                        ShowMessage.InfoMess("Succesfully Locked");
                       
                    }


                    //this.ExpandNodeForCheck(TreeNode1);

                }
                catch (Exception E)
                {
                    ShowMessage.ErrorMess(E.Message);
                    return;
                }
            }
        }

       

        

    }
}
