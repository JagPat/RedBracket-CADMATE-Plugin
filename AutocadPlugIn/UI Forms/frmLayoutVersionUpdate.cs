using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RedBracketConnector;
using AdvancedDataGridView;
using System.Collections;

namespace AutocadPlugIn.UI_Forms
{
    public partial class frmLayoutVersionUpdate : Form
    {
        public string Fileid = "";
        public string FileName = "";
        public string FileType = "";
        public string FileStatus = "";
        public string FileVersion = "";
        public string ProjectID = "";
        public string FilePath = "";
        public DataTable dtLayoutInfo = new DataTable();
        RBConnector objRBC = new RBConnector();
        AutoCADManager CadManager = new AutoCADManager();
        public frmLayoutVersionUpdate(string Fileid = "", string FileName = "", string FileType = "", string FileStatus = "", string FileVersion = "",
            string ProjectID = "", string FilePath = "")
        {
            InitializeComponent();
            this.Fileid = Fileid;
            this.FileName = FileName;
            this.FileType = FileType;
            this.FileStatus = FileStatus;
            this.FileVersion = FileVersion;
            this.ProjectID = ProjectID;
            this.FilePath = FilePath;
        }

        private void frmLayoutVersionUpdate_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Helper.FIllCMB(LayoutType, objRBC.GetFIleType(), "name", "id", true);
                Helper.FIllCMB(LayoutStatus, objRBC.GetFIleStatus(), "statusname", "id", true);

                TreeGridNode node = null;

                node = tgvLayouts.Nodes.Add(false
                                   , FileName
                                     , Fileid
                                   , ""
                                   , FileType
                                   , FileStatus
                                   , FileVersion
                                   , ""
                                   , "1"
                                   , ""
                                   , ""
                                   , ""
                                   , ""
                                   );
                node.Expand();
                node.ReadOnly = true;
                Hashtable htLayoutInfo = CadManager.GetLayoutInfo(FilePath);
                if (dtLayoutInfo.Rows.Count > 0)
                {


                    foreach (DataRow rw in dtLayoutInfo.Rows)
                    {
                        if (Convert.ToString(rw["IsFile"]) == "1")
                        {
                            continue;
                        }



                        // string Layouts = CadManager.GetLayoutInfo();
                        // var Layout = Layouts.Split('$');

                        bool IsAvailable = false;
                        foreach (DictionaryEntry key in htLayoutInfo)
                        {
                            string LayoutName = key.Key.ToString();
                            string LayoutID1 = key.Value.ToString();
                            if (LayoutName.Trim().Length == 0)
                            {
                                continue;
                            }


                            //(140688313392624)
                            if (LayoutName == Convert.ToString(rw["FileLayoutName"]))
                            //if (LayoutID1 == Convert.ToString(rw["ACLayoutID"]))
                            {
                                IsAvailable = true; break;
                            }
                        }

                        if (IsAvailable)
                        {
                            node = tgvLayouts.Nodes[0].Nodes.Add(Convert.ToBoolean(rw["ChangeVersion"])
                                       , rw["FileLayoutName"]
                                        , rw["FileID1"]
                                   , rw["LayoutID"]
                                   , rw["LayoutType"]
                                   , rw["LayoutStatus"]
                                   , rw["Version"]
                                   , rw["Description"]
                                   , rw["IsFile"]
                                   , rw["TypeID"]
                                   , rw["StatusID"]
                                   , rw["ACLayoutID"]
                                   , rw["LayoutName1"]
                                   );
                        }
                    }
                }
                // else
                {
                    // write code to get layout data from RB and compare them with current layout
                    // if no data available at RB then load default data as follow

                    DataTable dtLayoutInfoRB = new DataTable();

                    //Hashtable htLayoutInfo = CadManager.GetLayoutInfo();


                    foreach (DictionaryEntry key in htLayoutInfo)
                    {
                        string LayoutName = key.Key.ToString();
                        if (LayoutName.Trim().Length == 0)
                        {
                            continue;
                        }
                        bool IsAvailable = false;

                        foreach (TreeGridNode ChildNode in tgvLayouts.Nodes[0].Nodes)
                        {
                            if (LayoutName == Convert.ToString(ChildNode.Cells[1].Value))
                            {
                                IsAvailable = true;
                            }
                        }




                        if (!IsAvailable)
                        {
                            node = tgvLayouts.Nodes[0].Nodes.Add(true,
                                         LayoutName
                                       , Convert.ToString(tgvLayouts.Nodes[0].Cells["FileID1"].Value)
                                       , ""
                                       , ""
                                       , ""
                                       , "0.0"
                                       , ""
                                       , "0"
                                       , ""
                                       , ""
                                       , key.Value.ToString()
                                       , LayoutName
                                       );

                            node.Cells[0].ReadOnly = true;
                        }
                    }


                }
                foreach (TreeGridNode ChildNode in tgvLayouts.Nodes[0].Nodes)
                {
                    ChildNode.Cells["LayoutType"].ReadOnly = ChildNode.Cells["LayoutStatus"].ReadOnly = ChildNode.Cells["Description"].ReadOnly = !(bool)ChildNode.Cells[0].Value;

                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                //foreach(TreeGridNode ParentNode in tgvLayouts.Nodes)
                //{
                //    foreach(TreeGridNode ChildNode in ParentNode.Nodes)
                //    {
                //        if((bool)ChildNode.Cells[0].Value && Convert.ToString(ChildNode.Cells["LayoutID"].Value).Length==0)
                //        {
                //          bool RetVal=  objRBC.CheckLayoutExistance(ProjectID, Convert.ToString(ChildNode.Cells["LayoutName1"].Value), Convert.ToString(ChildNode.Cells["FileID1"].Value));

                //            if(!RetVal)
                //            {
                //                return;
                //            }
                //        }
                //    }
                //}






                DataTable dt = new DataTable();
                foreach (DataGridViewColumn C in tgvLayouts.Columns)
                {
                    dt.Columns.Add(C.Name);
                }
                int count = 0;
                // it is assumed that there will be only one parent node.
                foreach (TreeGridNode ParentNode in tgvLayouts.Nodes)
                {
                    dt.Rows.Add();

                    for (int i = 0; i < ParentNode.Cells.Count; i++)
                    {
                        dt.Rows[count][i] = ParentNode.Cells[i].FormattedValue;
                    }
                    count++;
                    foreach (TreeGridNode ChildNode in ParentNode.Nodes)
                    {
                        dt.Rows.Add();

                        for (int i = 0; i < ChildNode.Cells.Count; i++)
                        {
                            dt.Rows[count][i] = ChildNode.Cells[i].FormattedValue;
                        }
                        count++;
                    }
                }

                dtLayoutInfo = dt.Copy();
                this.Close();
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            Cursor.Current = Cursors.Default;
        }

        private void tgvLayouts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void tgvLayouts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }
                TreeGridNode selectedTreeNode = (TreeGridNode)tgvLayouts.CurrentRow;
                if (e.ColumnIndex == 0)//FileType
                {
                    //LayoutType.ReadOnly = LayoutStatus.ReadOnly = Description.ReadOnly = !(bool)selectedTreeNode.Cells[0].Value;
                    selectedTreeNode.Cells["LayoutType"].ReadOnly = selectedTreeNode.Cells["LayoutStatus"].ReadOnly = selectedTreeNode.Cells["Description"].ReadOnly = !(bool)selectedTreeNode.Cells[0].Value;
                }
                if (e.ColumnIndex == 4)//FileType
                {
                    string FileTypeID = "";
                    try
                    {

                        FileTypeID = Convert.ToString(selectedTreeNode.Cells["LayoutType"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["LayoutType"].Value));
                        FileType = Convert.ToString(selectedTreeNode.Cells["LayoutType"].FormattedValue) == string.Empty ? "" : Convert.ToString(selectedTreeNode.Cells["LayoutType"].FormattedValue);
                    }
                    catch
                    {
                        DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["LayoutType"];
                        FileType = Convert.ToString(c.Value);
                        // c.Value = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["cadtype"].Value), "name");
                        //FileTypeID = Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["cadtype"].Value));
                        FileTypeID = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["LayoutType"].Value), "name");
                    }
                    if (FileTypeID == "0" || FileTypeID == "-1")
                    {
                        FileTypeID = "";
                    }
                    selectedTreeNode.Cells["TypeID"].Value = FileTypeID;
                }
                if (e.ColumnIndex == 5)//FileStatus
                {
                    string FileStatusID = "";
                    try
                    {
                        //FileStatusID = Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["State"].Value));
                        FileStatusID = Convert.ToString(selectedTreeNode.Cells["LayoutStatus"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["LayoutStatus"].Value));
                    }
                    catch
                    {
                        DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["LayoutStatus"];
                        //c.Value = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["State"].Value), "statusname");
                        //FileTypeID = Convert.ToString(selectedTreeNode.Cells["cadtype"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["cadtype"].Value));
                        FileStatusID = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["LayoutStatus"].Value), "statusname");
                    }

                    if (FileStatusID == "0" || FileStatusID == "-1")
                    {
                        FileStatusID = "";
                    }
                    selectedTreeNode.Cells["StatusID"].Value = FileStatusID;
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        private void tgvLayouts_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (tgvLayouts.IsCurrentCellDirty)
            {
                tgvLayouts.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
    }
}
