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
using System.Collections;

namespace RBAutocadPlugIn.UI_Forms
{
    public partial class frmLayoutVersionUpdate : Form
    {
        public bool LoadFlag = false;
        public string Fileid = "";
        public string FileName = "";
        public string FileType = "";
        public string FileStatus = "";
        public string FileVersion = "";
        public string ProjectID = "";
        public string FilePath = "";
        public string OldDWGNo = "";

        public DataTable dtLayoutInfo = new DataTable();
        RBConnector objRBC = new RBConnector();
         
        public frmLayoutVersionUpdate(string Fileid = "", string FileName = "", string FileType = "", string FileStatus = "", string FileVersion = "",
            string ProjectID = "", string FilePath = "", string OldDWGNo = "")
        {
            InitializeComponent(); this.FormBorderStyle = FormBorderStyle.None;
            this.Fileid = Fileid;
            this.FileName = FileName;
            this.FileType = FileType;
            this.FileStatus = FileStatus;
            this.FileVersion = FileVersion;
            this.ProjectID = ProjectID;
            this.FilePath = FilePath;
            this.OldDWGNo = OldDWGNo;
            pnlTop.BackColor = pnlRight.BackColor = pnlLeft.BackColor = pnlBottom.BackColor = Helper.clrChildPopupBorderColor;
        }

        public void frmLayoutVersionUpdate_Load(object sender, EventArgs e)
        {
            LoadFlag = true;
            try
            {
                btnCancel.UseVisualStyleBackColor = btnSave.UseVisualStyleBackColor = false;
                btnCancel.BackColor = btnSave.BackColor = this.BackColor = tgvLayouts.BackgroundColor = Helper.FormBGColor;
                tgvLayouts.RowsDefaultCellStyle.BackColor = Helper.FormBGColor;
                Location = new Point(Location.X, Location.Y + 10);
                tgvLayouts.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);
                tgvLayouts.RowsDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Regular);
                Cursor.Current = Cursors.WaitCursor;
                Helper.FIllCMB(LayoutType, objRBC.GetFIleType(), "name", "id", true);
                Helper.FIllCMB(LayoutStatus, objRBC.GetFIleStatus(), "statusname", "id", true, IsSortByDisplayMember: false);

                TreeGridNode node = null;

                node = tgvLayouts.Nodes.Add(false
                                   , FileName
                                     , Fileid
                                   , ""
                                   , FileType
                                   , FileStatus

                                   , ""
                                    , FileVersion
                                   , "1"
                                   , ""
                                   , ""
                                   , ""
                                   , ""
                                     , ""
                                   , ""
                                   , ""
                                     , ""
                                   , ""

                                   );
                node.Expand();
                node.ReadOnly = true;
                Hashtable htLayoutInfo =Helper.cadManager.GetLayoutInfo(FilePath);
                DataTable dtLI = new DataTable();
                dtLI.Columns.Add("Name");
                dtLI.Columns.Add("DrawingNo");
                dtLI.Columns.Add("NO", typeof(decimal));
                dtLI.Columns.Add("ACLayoutID");
                dtLI.Columns.Add("BasicLayoutName");
                foreach (DictionaryEntry key in htLayoutInfo)
                {
                    DataRow dr = dtLI.NewRow();
                    string LayoutName = key.Key.ToString();
                    string LayoutID1 = key.Value.ToString();
                    string DrawingNO = "";
                    dr["Name"] = LayoutName;
                    dr["ACLayoutID"] = LayoutID1;
                    string tempLN = LayoutName;
                    if (LayoutName.Contains("_") && LayoutName.Contains(OldDWGNo))
                    {
                        LayoutName = LayoutName.Substring(0, LayoutName.LastIndexOf("_"));
                        if (LayoutName.Contains("_"))
                        {
                            LayoutName = LayoutName.Substring(0, LayoutName.LastIndexOf("_"));
                        }
                    }

                    dr["BasicLayoutName"] = LayoutName;
                    
                    if (tempLN.Contains("_"))
                    {
                        string DN = tempLN.Substring(tempLN.LastIndexOf("_"));
                        tempLN = tempLN.Replace(DN, "");
                        if (tempLN.Contains("_"))
                        {
                            string LN = tempLN.Substring(tempLN.LastIndexOf("_"));
                            dr["NO"] = LN.Substring(1);
                            LN = LN + DN;
                            DrawingNO = LN.Substring(1);
                            dr["DrawingNo"] = DrawingNO;
                            DataRow[] rw1 = dtLayoutInfo.Copy().Select("FileLayoutName like '%" + LN + "'");
                            if (rw1.Length > 0)
                            {

                            }
                        }
                    }
                    dtLI.Rows.Add(dr);

                }
                dtLI = Helper.SortTable(dtLI, "NO");
                foreach (DataRow dr in dtLI.Rows)

                {
                    string DrawingNO = Convert.ToString(dr["DrawingNo"]).Length > 7 ? Convert.ToString(dr["DrawingNo"]).Substring(2) : "";
                    string LayoutName = Convert.ToString(dr["Name"]);
                    //Helper.CloseProgressBar();
                    bool IsAvailable = false;
                    DataRow rw = dtLayoutInfo.NewRow();
                    foreach (DataRow rw2 in dtLayoutInfo.Rows)
                    {
                        string LayoutNo = Convert.ToString(dr["NO"]);
                        if (Convert.ToString(rw2["IsFile"]) == "1")
                        {
                            continue;
                        }


                        if (LayoutName.Trim().Length == 0)
                        {
                            continue;
                        }


                        //(140688313392624)
                        if (LayoutName == Convert.ToString(rw2["FileLayoutName"]))
                        //if (LayoutID1 == Convert.ToString(rw["ACLayoutID"]))
                        {
                            string tempLN = LayoutName;
                            if (tempLN.Contains("_"))
                            {


                                string DN = tempLN.Substring(tempLN.LastIndexOf("_"));
                                tempLN = tempLN.Replace(DN, "");
                                if (tempLN.Contains("_"))
                                {
                                    string LN = tempLN.Substring(tempLN.LastIndexOf("_"));
                                    if (DrawingNO == OldDWGNo)
                                    {
                                        rw2["LayoutName1"] = LayoutName.Replace("_" + Convert.ToString(dr["NO"]) + "_" + DrawingNO, "");
                                    }
                                }
                            }
                            rw = rw2;

                            IsAvailable = true;
                            break;

                        }

                    }
                    if (!IsAvailable)
                    {
                        string tempLN = LayoutName;
                        if (tempLN.Contains("_"))
                        {
                            string DN = tempLN.Substring(tempLN.LastIndexOf("_"));
                            tempLN = tempLN.Replace(DN, "");
                            if (tempLN.Contains("_"))
                            {
                                string LN = tempLN.Substring(tempLN.LastIndexOf("_"));
                                LN = LN + DN;
                                DataRow[] rw1 = dtLayoutInfo.Copy().Select("FileLayoutName like '%" + LN + "'");
                                if (rw1.Length > 0)
                                {
                                    rw = rw1[0];
                                    rw["ChangeVersion"] = IsAvailable = true;
                                    rw["FileLayoutName"] = rw["LayoutName1"] = LayoutName;

                                    if (DrawingNO != OldDWGNo)
                                    {
                                        dr["Name"] = LayoutName.Replace(LN, "");
                                    }
                                }
                            }
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
                                                    , rw["Description"]
                                                    , rw["Version"]
                                                    , rw["IsFile"]
                                                    , rw["TypeID"]
                                                    , rw["StatusID"]
                                                    , rw["ACLayoutID"]
                                                    , rw["LayoutName1"]
                                                    , rw["LayoutNo"]
                                                    , rw["CreatedBy"]
                                                    , rw["CreatedOn"]
                                                    , rw["UpdatedBy"]
                                                    , rw["UpdatedOn"]
                                                    , rw["LayoutStatusOld"]
                               );
                    }

                }


                {
                    // write code to get layout data from RB and compare them with current layout
                    // if no data available at RB then load default data as follow

                    //DataTable dtLayoutInfoRB = new DataTable();

                    //Hashtable htLayoutInfo = CadManager.GetLayoutInfo();


                    foreach (DataRow dr in dtLI.Rows)
                    {
                        string LayoutName = Convert.ToString(dr["Name"]);
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
                                break;
                            }
                        }

                        if (!IsAvailable)
                        {
                            string tempLN = LayoutName;
                            if (tempLN.Contains("_"))
                            {
                                string DN = tempLN.Substring(tempLN.LastIndexOf("_"));
                                tempLN = tempLN.Replace(DN, "");
                                if (tempLN.Contains("_"))
                                {
                                    string LN = tempLN.Substring(tempLN.LastIndexOf("_"));
                                    LN = LN + DN;
                                    DataRow[] rw1 = dtLayoutInfo.Copy().Select("FileLayoutName like '%" + LN + "'");
                                    if (rw1.Length > 0)
                                    {
                                        IsAvailable = true;
                                    }
                                    string DrawingNO = DN.Substring(1);
                                    if (DrawingNO == OldDWGNo)
                                    {
                                        dr["BasicLayoutName"] = LayoutName.Replace(LN, "");
                                    }
                                }
                            }




                        }


                        if (!IsAvailable)
                        {
                            node = tgvLayouts.Nodes[0].Nodes.Add(true,
                                         LayoutName
                                       , Convert.ToString(tgvLayouts.Nodes[0].Cells["FileID1"].Value)
                                       , ""
                                       , ""
                                       , Helper.FirstStatusName

                                       , "", "0.0"
                                       , "0"
                                       , ""
                                       , Helper.FirstStatusID
                                       , Convert.ToString(dr["ACLayoutID"])// key.Value.ToString()
                                       , Convert.ToString(dr["BasicLayoutName"])
                                     , ""
                                   , ""
                                   , ""
                                     , ""
                                   , ""
                                   , Helper.FirstStatusName
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
            LoadFlag = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void btnSave_Click(object sender, EventArgs e)
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


                        bool IsOldStatusClosed = IsClosedStatus(ChildNode, true);
                        bool IsNewStatusClosed = IsClosedStatus(ChildNode);
                        if (IsOldStatusClosed && IsNewStatusClosed)//Both state are closed
                        {
                            ChildNode.Cells["VersionType"].Value = "Minor";
                        }
                        else if (!IsOldStatusClosed && IsNewStatusClosed)//old is open and new is closed
                        {
                            ChildNode.Cells["VersionType"].Value = "Minor";
                        }
                        else if (IsOldStatusClosed && !IsNewStatusClosed)//old is closed and new is open
                        {
                            ChildNode.Cells["VersionType"].Value = "Major";

                        }
                        else if (!IsOldStatusClosed && !IsNewStatusClosed)//None of them is closed
                        {
                            ChildNode.Cells["VersionType"].Value = "Minor";
                        }


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
                if (e.RowIndex < 0 || e.ColumnIndex < 0 || LoadFlag)
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

                    bool IsOldStatusClosed = IsClosedStatus(selectedTreeNode, true);
                    bool IsNewStatusClosed = IsClosedStatus(selectedTreeNode);
                    if (IsOldStatusClosed && IsNewStatusClosed)//Both state are closed
                    {

                    }
                    else if (!IsOldStatusClosed && IsNewStatusClosed)//old is open and new is closed
                    {

                    }
                    else if (IsOldStatusClosed && !IsNewStatusClosed)//old is closed and new is open
                    {
                        if (ShowMessage.ValMessYN("Changing layout status from '" + GetFileStatus(selectedTreeNode, true) + "' to '" + GetFileStatus(selectedTreeNode) + "' will lead to creation of new revision, \n Do you want to proceeds anyway ?") == DialogResult.No)
                        {
                            selectedTreeNode.Cells["LayoutStatus"].Value = selectedTreeNode.Cells["LayoutStatusOld"].Value;
                        }
                        else
                        {

                        }
                    }
                    else if (!IsOldStatusClosed && !IsNewStatusClosed)//None of them is closed
                    {

                    }

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


        public bool IsClosedStatus(TreeGridNode CurrentNode, bool IsOld = false)
        {
            try
            {
                DataTable dtStatus = objRBC.GetFIleStatus();

                string FileStatus = GetFileStatus(CurrentNode, IsOld);
                if (dtStatus != null)
                {

                    DataRow[] dr1 = dtStatus.Select("statusname = '" + FileStatus + "' and IsClosed ='True'");
                    if (dr1.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return false;
        }

        public string GetFileStatus(TreeGridNode selectedTreeNode, bool IsOld = false)
        {
            string FileStatus = "";
            try
            {
                string CN = "LayoutStatus";
                if (IsOld)
                    CN = "LayoutStatusOld";
                DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["LayoutStatus"];
                try
                {
                    decimal d = Convert.ToDecimal(selectedTreeNode.Cells[CN].Value);
                    //FileStatus = Convert.ToString(selectedTreeNode.Cells["State"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["State"].Value));
                    FileStatus = Helper.FindValueInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells[CN].Value), "statusname");
                }
                catch
                {
                    //FileStatus = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["State"].Value), "statusname");
                    //FileStatus = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "statusname", Convert.ToString(selectedTreeNode.Cells[CN].Value), "statusname");
                    FileStatus = Convert.ToString(selectedTreeNode.Cells[CN].Value);
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return FileStatus;
        }
    }
}
