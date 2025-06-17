using System;

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AdvancedDataGridView;
using System.IO;
using Newtonsoft.Json;
using static RBAutocadPlugIn.Helper;
using static RBAutocadPlugIn.ShowMessage;
using static RBAutocadPlugIn.Properties.Resources;
namespace RBAutocadPlugIn.UI_Forms
{
    public partial class frmSave_Active_Drawings : Form
    {

        public Hashtable htAllDrawing = new Hashtable();
        private System.Data.DataTable dtTreeGridData = new System.Data.DataTable();
        RBConnector objRBC = new RBConnector();
        //  Stack<List<clsFolderSearchReasult>> StackFolderSearchReasult = new Stack<List<clsFolderSearchReasult>>();
        List<System.Data.DataTable> lstdtLayoutInfo = new List<System.Data.DataTable>();
        public bool LoadFlag = false;
        bool IsParentNew = false;
        bool IsChildOld = false;
        bool IsParentNewChildOld = false;
        bool IsSaveAs = false;
        public List<String> AllDrawing = new List<String>();
        public List<String> UnqDrawing = new List<String>();
        public List<object> objTag = new List<Object>() { new DataTable(), new Stack<List<clsFolderSearchReasult>>() };
        public frmSave_Active_Drawings(ref bool OpenForm, bool IsSaveAs = false)
        {

            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.IsSaveAs = IsSaveAs;

            object obj = Autodesk.AutoCAD.ApplicationServices.Application.GetSystemVariable("DBMOD");
            if (System.Convert.ToInt16(obj) != 0)
            {
                OpenForm = false;
                ValMess("Please save all documents to process further.");
                return;
            }
            // cadManager.SaveActiveDrawing();

        }


        private void Save_Active_Drawings_Load(object sender, EventArgs e)
        {
            // do not write any code here
        }
        public bool FormLoad()
        {
            LoadFlag = true;
            try
            {

                //  cadManager.SaveAllOpenDoc();
                Cursor.Current = Cursors.WaitCursor;
                this.BackColor = FormBGColor;
                CADDescription.BackColor = submit.BackColor = cancel.BackColor = savetreeGrid.BackgroundColor = FormBGColor;

                savetreeGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);
                this.imageStrip.ImageSize = new Size(17, 17);
                this.imageStrip.TransparentColor = Color.Magenta;
                this.imageStrip.ImageSize = new Size(17, 17);
                this.imageStrip.Images.AddStrip(LockImageStrip1);
                savetreeGrid.ImageList = imageStrip;
                int totalCount = savetreeGrid.Nodes.Count;
                int counter = 0;


                //objCadMgr.SaveActiveDrawing();
                TreeGridNode node = null;
                for (int i = 0; i < totalCount; i++)
                {
                    TreeGridNode TreeNode1 = savetreeGrid.Nodes.ElementAt(0);
                    this.ClearTreeView(TreeNode1);
                    savetreeGrid.Nodes.Remove(TreeNode1);
                }

                dtTreeGridData = cadManager.GetDrawingExternalRefreces1(IsSaveAs);
                if (dtTreeGridData == null)
                {
                    return false;
                }

                string FoldID = "", FoldPath = "", ProJNmNo = "", OldDWGNo = "";
                for (int i = 0; i < dtTreeGridData.Rows.Count; i++)
                {
                    if (Convert.ToString(dtTreeGridData.Rows[i]["DrawingId"]).Trim().Length == 0 || IsSaveAs)
                    {
                        dtTreeGridData.Rows[i]["drawingstate"] = FirstStatusName;


                    }
                    OldDWGNo = Convert.ToString(dtTreeGridData.Rows[i]["drawingnumber"]);

                    if (Convert.ToString(dtTreeGridData.Rows[i]["isroot"]).ToLower() == "true" &&
                        Convert.ToString(dtTreeGridData.Rows[i]["DrawingId"]).Trim().Length == 0)
                    {
                        IsParentNew = true;
                    }

                    //   MessageBox.Show("" + dtTreeGridData.Rows[i]);

                    if (Convert.ToString(dtTreeGridData.Rows[i]["isroot"]).ToLower() == "true" &&
                        Convert.ToString(dtTreeGridData.Rows[i]["DrawingId"]).Trim().Length > 0)
                    {
                        FoldID = Convert.ToString(dtTreeGridData.Rows[i]["folderid"]);
                        FoldPath = Convert.ToString(dtTreeGridData.Rows[i]["folderpath"]);
                        // MessageBox.Show("" + dtTreeGridData.Rows[i]["ProjectNameNo"]);
                        ProJNmNo = Convert.ToString(dtTreeGridData.Rows[i]["ProjectNameNo"]);
                        //   MessageBox.Show(""+ProJNmNo);
                    }

                    if (!IsParentNew && Convert.ToString(dtTreeGridData.Rows[i]["isroot"]).ToLower() == "false" &&
                        Convert.ToString(dtTreeGridData.Rows[i]["DrawingId"]).Trim().Length == 0)
                    {
                        dtTreeGridData.Rows[i]["folderid"] = FoldID;
                        dtTreeGridData.Rows[i]["folderpath"] = FoldPath;
                        dtTreeGridData.Rows[i]["ProjectNameNo"] = ProJNmNo;
                    }

                    if (IsParentNew && Convert.ToString(dtTreeGridData.Rows[i]["isroot"]).ToLower() == "false" &&
                        Convert.ToString(dtTreeGridData.Rows[i]["DrawingId"]).Trim().Length > 0 &&
                        !IsChildOld)
                    {
                        IsParentNewChildOld = IsChildOld = true;
                    }

                    if (IsSaveAs && Convert.ToString(dtTreeGridData.Rows[i]["isroot"]).ToLower() == "true")
                    {
                        dtTreeGridData.Rows[i]["DrawingId"] = "";
                        dtTreeGridData.Rows[i]["folderid"] = "";
                        dtTreeGridData.Rows[i]["folderpath"] = "";
                        dtTreeGridData.Rows[i]["ProjectNameNo"] = "";
                        dtTreeGridData.Rows[i]["revision"] = "";

                    }
                    //if (IsParentNew && Convert.ToString(dtTreeGridData.Rows[i]["isroot"]).ToLower() == "false" &&
                    //    Convert.ToString(dtTreeGridData.Rows[i]["DrawingId"]).Trim().Length == 0)
                    //{
                    //    IsParentNew = true;
                    //}





                    dtTreeGridData.Rows[i]["sourceid"] = "true";
                    if (Convert.ToString(dtTreeGridData.Rows[i]["DrawingId"]).Trim().Length > 0)
                    {
                        decimal CurrentVersion = Convert.ToDecimal(dtTreeGridData.Rows[i]["Revision"]);
                        try
                        {


                            decimal LatestVersion = Convert.ToDecimal(GetLatestVersion(Convert.ToString(dtTreeGridData.Rows[i]["DrawingNumber"])));

                            if (LatestVersion == 0)
                            {
                                if (!IsSaveAs)
                                {
                                    if (InfoYNMess("Following file no longer exist on server. do you want to update it as new ?\n" + Convert.ToString(dtTreeGridData.Rows[i]["filepath"]).Trim()) == DialogResult.No || IsSaveAs)
                                    {
                                        return false;
                                    }
                                    else
                                    {

                                        dtTreeGridData.Rows[i]["DrawingId"] = "";
                                        dtTreeGridData.Rows[i]["folderid"] = "";
                                        dtTreeGridData.Rows[i]["folderpath"] = "";
                                        //dtTreeGridData.Rows[i]["ProjectNameNo"] = "";
                                        dtTreeGridData.Rows[i]["revision"] = "";
                                    }
                                }
                                else
                                {

                                    dtTreeGridData.Rows[i]["DrawingId"] = "";
                                    dtTreeGridData.Rows[i]["folderid"] = "";
                                    dtTreeGridData.Rows[i]["folderpath"] = "";
                                    //dtTreeGridData.Rows[i]["ProjectNameNo"] = "";
                                    dtTreeGridData.Rows[i]["revision"] = "";
                                }

                            }
                            else if (CurrentVersion < LatestVersion)
                            {
                                if (InfoYNMess("Newer version file\n" + Convert.ToString(dtTreeGridData.Rows[i]["DrawingName"]) + "\n is already available on server, Are you sure you want to upload ?") == DialogResult.No)
                                {
                                    dtTreeGridData.Rows[i]["sourceid"] = "false";
                                    if (Convert.ToString(dtTreeGridData.Rows[i]["isroot"]).ToLower() == "true")
                                    {
                                        this.Close();
                                        return false;
                                    }
                                }

                            }
                        }
                        catch { }
                    }


                }


                SaveCommand lockStatusCmd = new SaveCommand();
                lockStatusCmd.DrawingInfo = dtTreeGridData;

                SaveController lockStatusCon = new SaveController();
                dtTreeGridData = lockStatusCon.getLockStatus(lockStatusCmd);
                if (lockStatusCon.errorString != null)
                {
                    ErrorMess(lockStatusCon.errorString);
                    this.Close();
                    return false;
                }

                #region CreateTreeGrid


                FIllCMB(cadtype, objRBC.GetFIleType(), "name", "id", true);
                FIllCMB(State, objRBC.GetFIleStatus(), "statusname", "id", true, IsSortByDisplayMember: false);
                FIllCMB(ProjectName, objRBC.GetProjectDetail(), "PNAMENO", "id", true, "My Files");





                foreach (DataRow rw in dtTreeGridData.Rows)
                {
                    if (Convert.ToString(rw["sourceid"]) == "false")
                    {
                        continue;
                    }


                    DataTable dtLayoutInfo = GetdtLayoutInfo(rw);

                    if (rw["isroot"].ToString() == "true")
                    {
                        if (rw["drawingid"].ToString() != "")//update
                        {
                            ProjectName.ReadOnly = true;
                            BtnBrowseFolder.Visible = false;
                            FolderPath.Width += BtnBrowseFolder.Width;
                        }
                        else//save
                        {
                            BtnBrowseFolder.Visible = true;
                        }
                        //string LIStatus = Convert.ToString(rw["DrawingId"]).Trim().Length == 0 && !IsSaveAs? "Empty" : "Filled";
                        string LIStatus = IsSaveAs ? dtLayoutInfo == null || dtLayoutInfo.Rows.Count == 0 ? "Empty" : "Filled" : Convert.ToString(rw["DrawingId"]).Trim().Length == 0 ? "Empty" : "Filled";
                        if (LIStatus == "Empty")
                        {
                            dtLayoutInfo.Rows.Clear();
                        }
                        //    MessageBox.Show(""+rw["projectnameno"]);
                        node = savetreeGrid.Nodes.Add(LIStatus, true, rw["drawingName"].ToString(), rw["drawingnumber"].ToString(), rw["Classification"].ToString(),
                           rw["drawingstate"].ToString(),
                            rw["drawingid"].ToString(), rw["filepath"].ToString(), rw["type"].ToString(), rw["lockstatus"].ToString(), rw["sourceid"].ToString()
                            , rw["isroot"].ToString(), rw["Layouts"], rw["projectnameno"], rw["revision"].ToString()
                            , rw["candelete"]
                            , rw["isowner"]
                            , rw["hasviewpermission"]
                            , rw["isactfilelatest"]
                            , rw["iseditable"]
                            , rw["caneditstatus"]
                            , rw["hasstatusclosed"]
                            , rw["isletest"]
                            , rw["prefix"]
                            , rw["projectnameno"]
                            , rw["folderpath"]
                            , rw["folderid"]
                             , rw["folderid"]
                             , rw["IsNewXref"]
                             , "", "", "", "" //project,status,type id,ProjNo
                            , rw["PK"]
                            , rw["FK"]
                             , rw["projectname"]
                              , rw["folderid"]
                              , rw["drawingstate"].ToString()
                              , rw["drawingstate"].ToString()
                              , rw["drawingnumber"].ToString()
                            );
                        //node.Cells["Check"].ReadOnly = Convert.ToString(rw["DrawingId"]).Trim().Length == 0 || IsSaveAs ? true : false;
                        node.Cells["Check"].ReadOnly = true;

                        CADDescription.Text = Convert.ToString(rw["versiondesc"]);
                        //   MessageBox.Show(rw["drawingid"].ToString());
                        if (rw["drawingid"].ToString() == "")
                        {
                            //   MessageBox.Show("1");
                            node.Cells["projectname"].Value = "";
                            node.Cells["projectname"].ReadOnly = false;
                            node.Cells["drawingnumber"].Value = "AutoFill";
                        }
                        else
                        {
                            //  MessageBox.Show("2");
                            node.Cells["projectname"].ReadOnly = true;
                            node.Cells["State"].ReadOnly = !Convert.ToBoolean(rw["caneditstatus"]);
                        }

                        if (rw["lockstatus"].ToString() == "1")
                            node.ImageIndex = 0;
                        else if (rw["lockstatus"].ToString() == "2")
                        {
                            node.ImageIndex = 1;
                            node.Cells["Check"].ReadOnly = true;
                        }
                        node.Expand();



                        if (rw["drawingid"].ToString() == "")
                        {
                            for (int i = 0; i < dtLayoutInfo.Rows.Count; i++)
                            {
                                if (Convert.ToString(dtLayoutInfo.Rows[i]["IsFile"]) == "1")
                                {
                                    dtLayoutInfo.Rows[i]["ChangeVersion"] = "False";
                                }
                                else
                                {
                                    dtLayoutInfo.Rows[i]["ChangeVersion"] = "True";
                                }
                                dtLayoutInfo.Rows[i]["FileID1"] = "";
                                dtLayoutInfo.Rows[i]["LayoutID"] = "";
                            }
                        }

                        objTag = new List<Object>() { new DataTable(), new Stack<List<clsFolderSearchReasult>>() };

                        node.Tag = objTag;
                        AssigndtLayoutInfoToTag(node, dtLayoutInfo);
                        AddNode(dtTreeGridData, rw, node, OldDWGNo);
                    }












                }
                #endregion CreateTreeGrid
                bool IsRevisionVisible = false;
                foreach (var item in savetreeGrid.Nodes)
                {
                    if (Convert.ToString(item.Cells["revision"].Value).Trim().Length > 0)
                    {
                        IsRevisionVisible = true;
                    }

                    foreach (var item1 in item.Nodes)
                    {
                        if (Convert.ToString(item.Cells["revision"].Value).Trim().Length > 0)
                        {
                            IsRevisionVisible = true;
                        }
                    }
                }
                if (IsRevisionVisible)
                {
                    revision.Visible = true;
                }
                else
                {
                    revision.Visible = false;
                    FolderPath.Width += revision.Width;
                }

            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
                LoadFlag = false;
                Cursor.Current = Cursors.Default;
                return false;
            }
            LoadFlag = false;
            Cursor.Current = Cursors.Default;
            return true;
        }

        public void AddNode(DataTable dtDrawing, DataRow rw1, TreeGridNode node = null, string OldDWGNo = null)
        {
            try
            {
                DataTable dtChild = RowFilter(RowFilter(dtDrawing, "PK", "FK", "<>"), "FK", Convert.ToString(rw1["PK"]));
                foreach (DataRow rw in dtChild.Rows)
                {
                    System.Data.DataTable dtLayoutInfo = GetdtLayoutInfo(rw);
                    string LIStatus = Convert.ToString(rw["DrawingId"]).Trim().Length == 0 ? "Empty" : "Filled";
                    //  as per suggested by email dated  sep 7 2018 by mahesh saraswti
                    TreeGridNode node1 = node.Nodes.Add(LIStatus, /*Convert.ToString(rw["DrawingId"]).Trim().Length == 0 || IsSaveAs ? true :*/ false, rw["drawingName"].ToString(), rw["drawingnumber"].ToString(), rw["Classification"].ToString(),
                          rw["drawingstate"].ToString(), rw["drawingid"].ToString(), rw["filepath"].ToString(), rw["type"].ToString(),
                           rw["lockstatus"].ToString(), rw["sourceid"].ToString(), rw["isroot"].ToString(), rw["Layouts"], rw["projectnameno"]
                               , rw["revision"].ToString(), rw["candelete"]
                           , rw["isowner"]
                           , rw["hasviewpermission"]
                           , rw["isactfilelatest"]
                           , rw["iseditable"]
                           , rw["caneditstatus"]
                           , rw["hasstatusclosed"]
                           , rw["isletest"]
                           , rw["prefix"]
                           , rw["projectnameno"]
                            , rw["folderpath"]
                            , rw["folderid"]
                             , rw["folderid"]
                              , rw["IsNewXref"]
                              , "", "", "", "" //project,status,type id,ProjNo 
                            , rw["PK"]
                            , rw["FK"]
                             , rw["projectname"]
                              , rw["folderid"]
                              , rw["drawingstate"].ToString()
                              , rw["drawingstate"].ToString()
                              , rw["drawingnumber"].ToString()
                              );


                    node1.Cells["projectname"].ReadOnly = true;
                    if (rw["drawingid"].ToString() == "")
                    {
                        if (IsParentNew)
                        {
                            //  MessageBox.Show("parent " + IsParentNew);
                            node1.Cells["projectname"].Value = "";
                            node1.Cells["FolderPath"].Value = "";
                            node1.Cells["FolderID"].Value = "";
                        }


                        node1.Cells["drawingnumber"].Value = "AutoFill";
                    }
                    else
                    {
                        node.Cells["State"].ReadOnly = !Convert.ToBoolean(rw["caneditstatus"]);
                    }

                    if (rw["lockstatus"].ToString() == "1")
                    {
                        node1.ImageIndex = 0;
                    }

                    else if (rw["lockstatus"].ToString() == "2")
                    {
                        node1.ImageIndex = 1;
                    }
                    node1.Expand();

                    objTag = new List<Object>() { new DataTable(), new Stack<List<clsFolderSearchReasult>>() };

                    node1.Tag = objTag;
                    AssigndtLayoutInfoToTag(node1, dtLayoutInfo);

                    AddNode(dtTreeGridData, rw, node1, OldDWGNo);
                    node1.Expand();                 //accourding to email on sep 7 2018
                    node1.Cells["Check"].ReadOnly = /*Convert.ToString(rw["DrawingId"]).Trim().Length == 0 || */IsSaveAs ? true : false;
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }

        public DataTable GetdtLayoutInfo(DataRow rw)
        {
            System.Data.DataTable dtLayoutInfo = new System.Data.DataTable();
            try
            {
                #region LayoutInfo
                string LayoutInfo1 = Convert.ToString(rw["layoutinfo"]);
                List<LayoutInfo> objLI = JsonConvert.DeserializeObject<List<LayoutInfo>>(LayoutInfo1);


                dtLayoutInfo.Columns.Add("ChangeVersion");
                dtLayoutInfo.Columns.Add("FileLayoutName");
                dtLayoutInfo.Columns.Add("FileID1");
                dtLayoutInfo.Columns.Add("LayoutID");
                dtLayoutInfo.Columns.Add("LayoutType");
                dtLayoutInfo.Columns.Add("LayoutStatus");
                dtLayoutInfo.Columns.Add("Version");
                dtLayoutInfo.Columns.Add("Description");
                dtLayoutInfo.Columns.Add("IsFile");
                dtLayoutInfo.Columns.Add("TypeID");
                dtLayoutInfo.Columns.Add("StatusID");
                dtLayoutInfo.Columns.Add("ACLayoutID");
                dtLayoutInfo.Columns.Add("LayoutName1");

                dtLayoutInfo.Columns.Add("LayoutNo");
                dtLayoutInfo.Columns.Add("CreatedBy");
                dtLayoutInfo.Columns.Add("CreatedOn");
                dtLayoutInfo.Columns.Add("UpdatedBy");
                dtLayoutInfo.Columns.Add("UpdatedOn");
                dtLayoutInfo.Columns.Add("LayoutStatusOld");
                if (objLI != null)
                {
                    foreach (LayoutInfo obj in objLI)
                    {
                        DataRow dr = dtLayoutInfo.NewRow();
                        dr["ChangeVersion"] = false;
                        dr["FileLayoutName"] = obj.name;
                        dr["FileID1"] = rw["drawingid"];
                        dr["LayoutID"] = obj.id;
                        dr["LayoutType"] = obj.typename;
                        if (IsSaveAs)
                            dr["LayoutStatus"] = FirstStatusName;
                        else
                            dr["LayoutStatus"] = obj.statusname;
                        dr["Version"] = obj.versionNo;
                        dr["Description"] = obj.description;
                        dr["IsFile"] = "0";
                        dr["TypeID"] = obj.typeId;

                        if (IsSaveAs)
                            dr["StatusID"] = FirstStatusID;
                        else
                            dr["StatusID"] = obj.statusId;
                        dr["ACLayoutID"] = obj.layoutId == null ? string.Empty : obj.layoutId;
                        dr["LayoutName1"] = obj.name;

                        dr["LayoutNo"] = obj.fileNo;
                        dr["CreatedBy"] = obj.createdby;
                        dr["CreatedOn"] = obj.created0n;
                        dr["UpdatedBy"] = obj.updatedby;
                        dr["UpdatedOn"] = obj.updatedon;
                        if (IsSaveAs)
                            dr["LayoutStatusOld"] = FirstStatusName;
                        else
                            dr["LayoutStatusOld"] = obj.statusname;
                        dtLayoutInfo.Rows.Add(dr);
                    }
                }

                #endregion
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
            return dtLayoutInfo;
        }

        private void ClearTreeView(TreeGridNode CurrentNode)
        {
            try
            {
                for (int nodeCount = 0; nodeCount < CurrentNode.Nodes.Count; nodeCount++)
                {
                    if (CurrentNode.Nodes.ElementAt(nodeCount).Nodes.Count() == 0)
                    {
                        savetreeGrid.Nodes.Remove(CurrentNode.Nodes.ElementAt(nodeCount));
                    }
                    else
                    {
                        TreeGridNode node1 = CurrentNode.Nodes.ElementAt(nodeCount);
                        this.ClearTreeView(node1);
                        savetreeGrid.Nodes.Remove(node1);
                    }
                }
            }
            catch (System.Exception ex)
            {
                ErrorMess("Exception Occur: " + ex.Message);
                return;
            }

        }

        private void submit_Click(object sender, EventArgs e)
        {
            CheckFileInfoFlag = true;
            try
            {

                submit.Focus();
                List<TreeGridNode> selectedTreeGridNodes = new List<TreeGridNode>();
                this.Cursor = Cursors.WaitCursor;
                bool Is_Delete = false;
                bool Is_Save = false;
                int PBValue = 7;

                //return;
                //To check howmany file is selected.
                foreach (TreeGridNode treeGridNode in savetreeGrid.Nodes)
                {
                    if ((bool)treeGridNode.Cells["Check"].FormattedValue)
                    {
                        selectedTreeGridNodes.Add(treeGridNode);
                        PBValue++; PBValue++; PBValue++;
                        GetSellectedNode(treeGridNode, ref PBValue, 3);
                    }
                }


                if (selectedTreeGridNodes.Count < 1)
                {
                    ValMess("Please select at least one file to save.");
                    this.Cursor = Cursors.Default;
                    CheckFileInfoFlag = false;
                    return;
                }
                #region LayoutTable Settlement
                LayoutInfoSettlement(selectedTreeGridNodes[0]);

                #endregion
                //To check whether any file with same name exist or not
                #region Duplication Check
                bool IsDuplicationCheck = true;
                if (IsDuplicationCheck)
                {
                    if (Name_Length_Duplication_Check(selectedTreeGridNodes[0]))
                    {
                        this.Cursor = Cursors.Default; CheckFileInfoFlag = true;
                        return;
                    }

                }
                #endregion

                GetProgressBar(PBValue, "File Save in Progress...", "");
                IncrementProgressBar(1, "Checking file existance on server.");

                #region Check for File existance in server
                if (!CheckFileExistance1(selectedTreeGridNodes[0]) || !CheckFileExistance(selectedTreeGridNodes[0]))
                {
                    CloseProgressBar(); Cursor.Current = Cursors.Default; CheckFileInfoFlag = true;
                    return;
                }
                #endregion



                SaveController objController = new SaveController();

                SaveCommand objCmd = new SaveCommand();
                lstdtLayoutInfo = new List<System.Data.DataTable>();

                htAllDrawing.Clear();
                AllDrawing.Clear();
                if (!GenFileInfoDT(selectedTreeGridNodes[0]) || !GenFileInfo(selectedTreeGridNodes[0]))
                {
                    ShowMessage.ErrorMess("File extension not found, please check.");
                    this.Cursor = Cursors.Default; CheckFileInfoFlag = true;
                    return;
                }


                foreach (var key in htAllDrawing.Keys)
                {
                    UnqDrawing.Add(Convert.ToString(htAllDrawing[key]));
                }

                objCmd.AllDrawing = AllDrawing;



                // to ask user want to delete file after save or not.
                if (InfoYNMess("Would you like to delete local file/files" + Environment.NewLine + " after saving it to RedBracket ?") == DialogResult.Yes)
                {
                    PBValue++;
                    Is_Delete = true;
                }
                // CloseProgressBar();
                //return;


                // to iterate selected file
                foreach (TreeGridNode currentTreeGrdiNode in selectedTreeGridNodes)
                {

                    if (Convert.ToString(currentTreeGrdiNode.Cells["isEditable"].Value).Length > 3)
                    {
                        if (!Convert.ToBoolean(Convert.ToString(currentTreeGrdiNode.Cells["isEditable"].Value).ToLower()))
                        {
                            InfoMess("You don't have edit permission for this file.");
                            CloseProgressBar();
                            this.Cursor = Cursors.Default; CheckFileInfoFlag = true;
                            return;
                        }
                    }
                    String FilePath = objCmd.FilePath = Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value);









                    IncrementProgressBar(1, "Saving file properties on server.");
                    //Helper.HideProgressBar();
                    // save properties in RB
                    Is_Save = objController.ExecuteSave(objCmd, false, lstdtLayoutInfo);



                    if (Is_Save)
                    {
                        // CloseProgressBar();


                        #region Add Title Block 
                        if (TitleBlockFlag)
                        {
                            IncrementProgressBar(1, "Updating Title block Attributes.");
                            DataTable dtFileInfo = objController.dtDrawingProperty.Copy();
                            foreach (PLMObject item in objController.plmObjs)
                            {
                                bool IsDocOpend = cadManager.OpenDocSilently(item.FilePath);


                                cadManager.AddingAttributeToABlock(item.FilePath, DrawingAttributes, item.ObjectName, false);
                                DataTable dtLayoutInfo = item.dtLayoutInfo.Copy();
                                foreach (DataRow dr in dtLayoutInfo.Rows)
                                {
                                    if (Convert.ToString(dr["IsFile"]) == "0")
                                        cadManager.AddingAttributeToABlock(item.FilePath, LayoutAttributes, Convert.ToString(dr["FileLayoutName"]), true);
                                }
                                if (IsDocOpend)
                                {
                                    cadManager.CloseDocSilently(item.FilePath);
                                }
                            }
                        }
                        #endregion

                        IncrementProgressBar(1, "Updating new properties to local file.");
                        // CloseProgressBar();

                        // Update document info into document for future referance 
                        if (objController.dtDrawingProperty.Rows.Count > 0)
                        {
                            cadManager.UpdateExRefInfo(objCmd.FilePath, objController.dtDrawingProperty);
                        }



                        #region Close Files
                        IncrementProgressBar(1, "Closing open files.");
                        cadManager.ChecknCloseOpenedDoc(FilePath);
                        CloseFile(currentTreeGrdiNode);

                        #endregion

                        #region Updating Layout Name
                        IncrementProgressBar(1, "Updating layout names in file.");
                        foreach (PLMObject objplmo in objController.plmObjs)
                        {
                            // CloseProgressBar();
                            int Count = -1;

                            foreach (DataRow dr in objplmo.dtLayoutInfo.Rows)
                            {
                                Count++;
                                if (Convert.ToString(dr["IsFile"]) == "1")
                                {
                                    continue;
                                }
                                if (Convert.ToString(dr["LayoutID"]).Trim().Length == 0)
                                {
                                    string Sufix = "_" + string.Format("{0:00}", Count) + "_" + objplmo.ObjectNumber;
                                    string NewLayoutName = Convert.ToString(dr["LayoutName1"]).Trim();
                                    cadManager.renamelayoutName(objplmo.FilePath, Convert.ToString(dr["FileLayoutName"]).Trim().Replace(Sufix, ""), NewLayoutName);
                                }
                            }

                        }
                        #endregion
                        IncrementProgressBar(1, "Updating file names.");
                        // CloseProgressBar();

                        cadManager.UpdateExRefFileName(objCmd.FilePath, objCmd.FilePath, ref objController.plmObjs);

                        IncrementProgressBar(1, "Uploading file to server.");


                        // save File in RB
                        Is_Save = objController.ExecuteSave(objCmd, true);
                    }

                    if (Is_Save && IsSaveAs)
                    {
                        IncrementProgressBar(1, "Deleting old files.");
                        File.Delete(objController.plmObjs[0].Oldfilepath);
                        DeleteFile(currentTreeGrdiNode, objController.plmObjs, true);
                    }
                    if (!Is_Save)
                    {
                        DeleteFileFromDB(objController.plmObjs);
                    }
                    // To delete file
                    if (Is_Delete && Is_Save)
                    {
                        IncrementProgressBar(1, "Deleting local files.");
                        File.Delete(objController.plmObjs[0].Oldfilepath);
                        File.Delete(objController.plmObjs[0].FilePath);
                        DeleteFile(currentTreeGrdiNode, objController.plmObjs, false);


                    }
                }
                IncrementProgressBar(PBValue, "");
                this.Cursor = Cursors.Default;
                if (Is_Save)
                {
                    CloseProgressBar();
                    InfoMess("Save operation successfully completed.");
                    this.Close(); CheckFileInfoFlag = true;
                    return;
                }
                else
                {
                    CloseProgressBar();
                    ErrorMessUD("Save operation unsuccessfully completed."); CheckFileInfoFlag = true;
                    return;
                }

            }
            catch (Exception ex)
            {
                CloseProgressBar();
                ErrorMess(ex.Message);
                this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
            CheckFileInfoFlag = true;
        }


        public bool Name_Length_Duplication_Check(TreeGridNode ParentNode)
        {

            try
            {
                if (Name_Length_Check(ParentNode))
                {
                    return true;
                }
                if (Name_Duplication_Check(ParentNode))
                {
                    return true;
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message); return true;
            }
            return false;
        }
        public bool Name_Length_Check(TreeGridNode ParentNode)
        {

            try
            {
                bool RetVal = FileName_LayoutNameLengthValidation(ParentNode);
                if (RetVal)
                    return RetVal;
                foreach (TreeGridNode childNode in ParentNode.Nodes)
                {
                    if ((bool)childNode.Cells["Check"].FormattedValue)
                    {
                        RetVal = FileName_LayoutNameLengthValidation(childNode);

                        if (RetVal)
                            return RetVal;
                        else
                        {
                            RetVal = Name_Length_Check1(childNode);
                            if (RetVal)
                                return RetVal;
                        }
                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message); return true;
            }
            return false;
        }
        public bool Name_Length_Check1(TreeGridNode ParentNode)
        {

            try
            {
                bool RetVal = false;

                foreach (TreeGridNode childNode in ParentNode.Nodes)
                {
                    if ((bool)childNode.Cells["Check"].FormattedValue)
                    {

                        RetVal = FileName_LayoutNameLengthValidation(childNode);
                        if (RetVal)
                            return RetVal;
                        else
                        {
                            RetVal = Name_Length_Check1(childNode);
                            if (RetVal)
                                return RetVal;
                        }


                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message); return true;
            }
            return false;
        }
        public bool FileName_LayoutNameLengthValidation(TreeGridNode currentTreeGrdiNode)
        {
            try
            {
                System.Data.DataTable dtLayoutInfo = GetdtLayoutInfoFromTag(currentTreeGrdiNode);
                //File name and layout out name length validation
                for (int i = 0; i < dtLayoutInfo.Rows.Count; i++)
                {
                    string FN = RemovePreFixFromFileName(Path.GetFileName(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim()), Convert.ToString(currentTreeGrdiNode.Cells["prefix"].Value).Trim());
                    string LN = Convert.ToString(dtLayoutInfo.Rows[i]["FileLayoutName"]);

                    if (Convert.ToString(dtLayoutInfo.Rows[i]["IsFile"]) == "1")
                    {
                        if (FN.Length < FileLayoutNameLength)
                        {

                        }
                        else
                        {
                            ErrorMessUD(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim() + "\n File name length exceeds maximum limit.");
                            this.Cursor = Cursors.Default;
                            return true;
                        }
                    }
                    else
                    {
                        if (FileNameCheckForSpecialCharacter(LN,true))
                        {
                            if (LN.Length < FileLayoutNameLength)
                            {

                            }
                            else
                            {
                                ErrorMessUD(LN + "\n Layout name length exceeds maximum limit. of file " + Environment.NewLine + FN);
                                this.Cursor = Cursors.Default;
                                return true;
                            }
                        }
                        else
                        {
                            this.Cursor = Cursors.Default;
                            return true;
                        }
                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message); return true;
            }
            return false;
        }
        public bool Name_Duplication_Check(TreeGridNode ParentNode)
        {

            try
            {
                bool RetVal = FileNameDuplicationCheck(ParentNode);
                if (RetVal)
                    return RetVal;
                foreach (TreeGridNode childNode in ParentNode.Nodes)
                {
                    if ((bool)childNode.Cells["Check"].FormattedValue)
                    {
                        RetVal = FileNameDuplicationCheck(childNode);
                        if (RetVal)
                            return RetVal;
                        else
                        {
                            RetVal = Name_Duplication_Check1(childNode);
                            if (RetVal)
                                return RetVal;
                        }

                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message); return true;
            }
            return false;
        }
        public bool Name_Duplication_Check1(TreeGridNode ParentNode)
        {

            try
            {
                bool RetVal = false;

                foreach (TreeGridNode childNode in ParentNode.Nodes)
                {
                    if ((bool)childNode.Cells["Check"].FormattedValue)
                    {

                        RetVal = FileNameDuplicationCheck(childNode);
                        if (RetVal)
                            return RetVal;
                        else
                        {
                            RetVal = Name_Duplication_Check1(childNode);
                            if (RetVal)
                                return RetVal;
                        }
                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message); return true;
            }
            return false;
        }
        public void SaveFiles(TreeGridNode ParentNode)
        {
            try
            {
                foreach (TreeGridNode childNode in ParentNode.Nodes)
                {
                    if ((bool)childNode.Cells["Check"].FormattedValue)
                    {


                        SaveFiles(childNode);
                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }
        public string GetProjectID(TreeGridNode ParentNode)
        {
            string ProjectID = "";
            string ProjectNameNo = "";
            string ProjectName = "";
            string ProjectNo = "";
            try
            {
                GetProjectDetail(ParentNode, ref ProjectID, ref ProjectNameNo, ref ProjectName, ref ProjectNo);
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
            return ProjectID;
        }
        public string GetProjectNo(TreeGridNode ParentNode)
        {
            string ProjectID = "";
            string ProjectNameNo = "";
            string ProjectName = "";
            string ProjectNo = "";
            try
            {
                GetProjectDetail(ParentNode, ref ProjectID, ref ProjectNameNo, ref ProjectName, ref ProjectNo);
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
            return ProjectNo;
        }
        public string GetProjectName(TreeGridNode ParentNode)
        {
            string ProjectID = "";
            string ProjectNameNo = "";
            string ProjectName = "";
            string ProjectNo = "";
            try
            {
                GetProjectDetail(ParentNode, ref ProjectID, ref ProjectNameNo, ref ProjectName, ref ProjectNo);
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
            return ProjectName;
        }
        public string GetProjectNameNo(TreeGridNode ParentNode)
        {
            string ProjectID = "";
            string ProjectNameNo = "";
            string ProjectName = "";
            string ProjectNo = "";
            try
            {
                GetProjectDetail(ParentNode, ref ProjectID, ref ProjectNameNo, ref ProjectName, ref ProjectNo);
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
            return ProjectNameNo;
        }
        public void GetProjectDetail(TreeGridNode selectedTreeNode, ref string ProjectID, ref string ProjectNameNo, ref string ProjectName, ref string ProjectNo)
        {
            try
            {
                try
                {

                    ProjectID = Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value));
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];
                    ProjectNameNo = FindValueInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value)), "PNAMENO");
                    ProjectName = FindValueInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value)), "name");
                    ProjectNo = FindValueInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value)), "number");

                }
                catch
                {
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];

                    ProjectID = FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                    ProjectNameNo = FindIDInCMB((System.Data.DataTable)c.DataSource, "PNAMENO", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                    ProjectName = FindIDInCMB((System.Data.DataTable)c.DataSource, "name", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                    ProjectNo = FindIDInCMB((System.Data.DataTable)c.DataSource, "number", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }
        public string GetFileStatus(TreeGridNode selectedTreeNode, bool IsOld = false)
        {
            string FileStatus = "";
            try
            {
                string CN = "State";
                if (IsOld)
                    CN = "OldState";
                DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["State"];
                try
                {
                    decimal d = Convert.ToDecimal(selectedTreeNode.Cells[CN].Value);
                    //FileStatus = Convert.ToString(selectedTreeNode.Cells["State"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["State"].Value));
                    FileStatus = FindValueInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells[CN].Value), "statusname");
                }
                catch
                {
                    //FileStatus =  FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["State"].Value), "statusname");
                    //FileStatus =  FindIDInCMB((System.Data.DataTable)c.DataSource, "statusname", Convert.ToString(selectedTreeNode.Cells[CN].Value), "statusname");
                    FileStatus = Convert.ToString(selectedTreeNode.Cells[CN].Value);
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
            return FileStatus;
        }

        public DataTable GetdtLayoutInfoFromTag(TreeGridNode selectedTreeNode)
        {
            try
            {
                objTag = new List<Object>() { new DataTable(), new Stack<List<clsFolderSearchReasult>>() };
                objTag = (List<object>)selectedTreeNode.Tag;
                DataTable dtLayoutInfo = new DataTable();
                dtLayoutInfo = (DataTable)objTag[0];
                return dtLayoutInfo;
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
            return null;
        }
        public Stack<List<clsFolderSearchReasult>> GetStackFolderSearchReasultFromTag(TreeGridNode selectedTreeNode)
        {
            try
            {
                objTag = new List<Object>() { new DataTable(), new Stack<List<clsFolderSearchReasult>>() };
                objTag = (List<object>)selectedTreeNode.Tag;
                Stack<List<clsFolderSearchReasult>> StackFolderSearchReasult = new Stack<List<clsFolderSearchReasult>>();
                StackFolderSearchReasult = (Stack<List<clsFolderSearchReasult>>)objTag[1];
                return StackFolderSearchReasult;
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
            return null;
        }
        public void AssignStackFolderSearchReasultToTag(TreeGridNode selectedTreeNode, Stack<List<clsFolderSearchReasult>> StackFolderSearchReasult)
        {
            try
            {
                objTag = new List<Object>() { new DataTable(), new Stack<List<clsFolderSearchReasult>>() };
                objTag = (List<object>)selectedTreeNode.Tag;
                objTag[1] = StackFolderSearchReasult;
                selectedTreeNode.Tag = objTag;
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }

        }
        public void AssigndtLayoutInfoToTag(TreeGridNode selectedTreeNode, DataTable dtLayoutInfo)
        {
            try
            {
                objTag = new List<Object>() { new DataTable(), new Stack<List<clsFolderSearchReasult>>() };
                objTag = (List<object>)selectedTreeNode.Tag;
                objTag[0] = dtLayoutInfo;
                selectedTreeNode.Tag = objTag;
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }

        }
        public void LayoutInfoSettlement(TreeGridNode ParentNode)
        {
            try
            {
                foreach (TreeGridNode ChildNode in ParentNode.Nodes)
                {
                    if ((bool)ChildNode.Cells["Check"].FormattedValue)
                    {

                        if (IsParentNewChildOld || IsSaveAs)
                        {
                            if (Convert.ToString(ChildNode.Cells["drawingID"].Value).Trim().Length > 0)
                            {
                                string PN = Convert.ToString(ChildNode.Cells["ProjectName"].Value).Trim();
                                string OPN = Convert.ToString(ChildNode.Cells["OldProject"].Value).Trim();
                                if (PN == OPN)
                                {
                                    ChildNode.Cells["FolderID"].Value = ChildNode.Cells["OldFolderID"].Value;
                                    ChildNode.Cells["FolderPath"].Value = "";
                                }
                                else
                                {
                                    ChildNode.Cells["drawingID"].Value = "";
                                    System.Data.DataTable dtLayoutInfo = GetdtLayoutInfoFromTag(ChildNode);

                                    for (int i = 0; i < dtLayoutInfo.Rows.Count; i++)
                                    {
                                        if (Convert.ToString(dtLayoutInfo.Rows[i]["IsFile"]) == "1")
                                        {
                                            dtLayoutInfo.Rows[i]["ChangeVersion"] = "False";
                                        }
                                        else
                                        {
                                            dtLayoutInfo.Rows[i]["ChangeVersion"] = "True";
                                        }
                                        dtLayoutInfo.Rows[i]["FileID1"] = "";
                                        dtLayoutInfo.Rows[i]["LayoutID"] = "";
                                    }


                                    AssigndtLayoutInfoToTag(ChildNode, dtLayoutInfo);
                                }
                            }
                            LayoutInfoSettlement(ChildNode);
                        }
                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }
        public bool FileNameDuplicationCheck(TreeGridNode currentTreeGrdiNode)
        {
            try
            {
                string MyProjectId = GetProjectID(currentTreeGrdiNode);

                MyProjectId = MyProjectId == "-1" ? string.Empty : MyProjectId;
                if (Convert.ToString(currentTreeGrdiNode.Cells["drawingID"].Value).Trim().Length == 0)
                {
                    if (File.Exists(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim()))
                    {
                        string FN = RemovePreFixFromFileName(Path.GetFileName(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim()), Convert.ToString(currentTreeGrdiNode.Cells["prefix"].Value).Trim());

                        if (FileNameCheckForSpecialCharacter(FN))
                        {
                            if (!objRBC.CheckFileExistance(MyProjectId, FN))
                            {
                                this.Cursor = Cursors.Default;
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }


                    }
                    else
                    {
                        ErrorMessUD(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim() + "\n File does not exists.");
                        this.Cursor = Cursors.Default;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception E)
            {
                ErrorMess(E.Message); return true;
            }
        }

        public void DeleteFile(TreeGridNode ParentNode, List<PLMObject> plmObjs, bool IsSaveAs)
        {
            try
            {
                foreach (TreeGridNode childNode in ParentNode.Nodes)
                {
                    if ((bool)childNode.Cells["Check"].FormattedValue)
                    {

                        foreach (PLMObject obj in plmObjs)
                        {
                            if (obj.Oldfilepath == Convert.ToString(childNode.Cells["filepath"].Value))
                            {
                                if (IsSaveAs)
                                {
                                    File.Delete(obj.Oldfilepath);
                                }
                                else
                                {
                                    File.Delete(obj.Oldfilepath);
                                    childNode.Cells["filepath"].Value = obj.FilePath;
                                    File.Delete(obj.FilePath);
                                }


                                break;
                            }
                        }
                        File.Delete(Convert.ToString(childNode.Cells["filepath"].Value));

                        DeleteFile(childNode, plmObjs, IsSaveAs);
                    }
                }

            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }
        public void DeleteFileFromDB(List<PLMObject> plmObjs)
        {
            try
            {


                foreach (PLMObject obj in plmObjs)
                {
                    if (obj.PK.Length > 0)
                    {

                        if (!objRBC.DeleteFileNLayoutDetail(obj.PK))
                            break;
                    }
                }




            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }
        public void CloseFile(TreeGridNode ParentNode)
        {
            try
            {
                foreach (TreeGridNode childNode in ParentNode.Nodes)
                {
                    if ((bool)childNode.Cells["Check"].FormattedValue)
                    {

                        cadManager.ChecknCloseOpenedDoc(Convert.ToString(childNode.Cells["filepath"].Value));

                        CloseFile(childNode);
                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void savetreeGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }
            TreeGridNode selectedTreeNode = (TreeGridNode)savetreeGrid.Rows[e.RowIndex];

            try
            {
                if (e.ColumnIndex == LayoutInfoStatus.Index)
                {
                    FillLayoutInfoFromUser(selectedTreeNode);
                }

                if (e.ColumnIndex == Check.Index)
                {



                    #region "Lock Drawings"
                    if (selectedTreeNode.Cells["lockstatus"].Value.ToString() == "0")
                    {

                        if (!objRBC.LockObject(new List<PLMObject> { new PLMObject() { ObjectId = Convert.ToString(selectedTreeNode.Cells["drawingID"].Value) } }))
                        {
                            selectedTreeNode.ImageIndex = 0;
                            selectedTreeNode.Cells["lockstatus"].Value = "1";
                        }
                    }
                    if (selectedTreeNode.Cells["lockstatus"].Value.ToString() == "2")
                    {
                        selectedTreeNode.Cells["Check"].Value = false;
                        savetreeGrid.RefreshEdit();
                        ErrorMessUD("This drawing is locked by other user.");

                        return;
                    }
                    #endregion

                    ChangeNodeValue(savetreeGrid.Nodes[0], selectedTreeNode);
                    if ((bool)selectedTreeNode.Cells["check"].EditedFormattedValue)
                    {


                        if (Convert.ToString(selectedTreeNode.Cells["drawingid"].Value).Trim().Length > 0)
                        {

                            if (!Convert.ToBoolean(selectedTreeNode.Cells["isletest"].Value))
                            {
                                if (ValMessYN("This file is not latest file, Do you want to upload it anyway ?.") == DialogResult.Yes)
                                {

                                }
                                else
                                {
                                    selectedTreeNode.Cells["check"].Value = false; savetreeGrid.RefreshEdit();
                                    return;
                                }
                            }
                            if (Convert.ToBoolean(selectedTreeNode.Cells["hasStatusClosed"].Value))
                            {
                                if (ValMessYN("This file is closed, Do you want to make new revision ?.") == DialogResult.Yes)
                                {

                                }
                                else
                                {
                                    selectedTreeNode.Cells["check"].Value = false; savetreeGrid.RefreshEdit();
                                    return;
                                }
                            }

                            if (Convert.ToBoolean(selectedTreeNode.Cells["isowner"].Value))
                            {
                            }
                            else
                            {
                                if (!Convert.ToBoolean(selectedTreeNode.Cells["isEditable"].Value))
                                {
                                    InfoMess("This file is not editable. so you can not save it.");
                                    selectedTreeNode.Cells["check"].Value = false;
                                    savetreeGrid.RefreshEdit();
                                    return;
                                }
                            }


                        }
                        //if (Convert.ToString(selectedTreeNode.Cells["isroot"].Value) == "1")
                        if (!selectedTreeNode.Cells["check"].ReadOnly)
                        {
                            FillLayoutInfoFromUser(selectedTreeNode);
                        }



                    }
                }
                if (e.ColumnIndex == BtnBrowseFolder.Index)
                {
                    //if (Convert.ToString(selectedTreeNode.Cells["isroot"].Value) == "true")
                    {
                        string MyProjectId = "", FolderID = "", ProjectName = "", FolderPath = "";
                        ProjectName = GetProjectName(selectedTreeNode);
                        MyProjectId = GetProjectID(selectedTreeNode);


                        FolderID = Convert.ToString(selectedTreeNode.Cells["FolderID"].Value).Trim();
                        FolderPath = Convert.ToString(selectedTreeNode.Cells["FolderPath"].Value).Trim();
                        MyProjectId = MyProjectId == "0" || MyProjectId == "-1" ? "0" : MyProjectId;
                        FolderID = FolderID == string.Empty || FolderID == "0" || FolderID == "-1" ? "0" : FolderID;
                        ProjectName = ProjectName == string.Empty ? "My Files" : ProjectName;
                        Stack<List<clsFolderSearchReasult>> StackFolderSearchReasult1 = GetStackFolderSearchReasultFromTag(selectedTreeNode);
                        Stack<List<clsFolderSearchReasult>> StackFolderSearchReasult = CopyStack(StackFolderSearchReasult1);
                        if (StackFolderSearchReasult.Count == 0)
                        {
                            List<clsFolderSearchReasult> objFolderSearchResult1 = objRBC.SearchFolder(MyProjectId, "0");

                            List<clsFolderSearchReasult> objFolderSearchResult = new List<clsFolderSearchReasult>(); ;
                            clsFolderSearchReasult obj = new clsFolderSearchReasult() { id = "-2", name = ProjectName, childFolderSize = Convert.ToInt16(objFolderSearchResult1.Count), companyId = "0" };
                            objFolderSearchResult.Add(obj);
                            StackFolderSearchReasult.Clear();
                            StackFolderSearchReasult.Push(objFolderSearchResult);


                        }

                        Stack<List<clsFolderSearchReasult>> TStackFolderSearchReasult = new Stack<List<clsFolderSearchReasult>>();
                        TStackFolderSearchReasult = CopyStack(StackFolderSearchReasult);


                        frmFolderSelection objFolderSelection = new frmFolderSelection(FolderID, MyProjectId, StackFolderSearchReasult, FolderPath);
                        objFolderSelection.ShowDialog();
                        if (objFolderSelection.IsSelect)
                        {


                            selectedTreeNode.Cells["FolderID"].Value = objFolderSelection.FolderID;
                            selectedTreeNode.Cells["FolderPath"].Value = objFolderSelection.FolderPath;
                            if (Convert.ToString(selectedTreeNode.Cells["isroot"].FormattedValue) == "true")
                            {
                                LoadFlag = true;
                                AssignFolderPath(savetreeGrid.Nodes[0], selectedTreeNode, objFolderSelection);
                                AssignStackFolderSearchReasultToTag(selectedTreeNode, StackFolderSearchReasult);
                                LoadFlag = false;
                            }
                            else
                            {
                                AssignStackFolderSearchReasultToTag(selectedTreeNode, StackFolderSearchReasult);
                            }
                            selectedTreeNode.Cells["oldFolderID1"].Value = objFolderSelection.FolderID;




                        }
                        else
                        {

                            AssignStackFolderSearchReasultToTag(selectedTreeNode, TStackFolderSearchReasult);
                        }
                    }
                }
                if (e.RowIndex == State.Index)
                {

                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }

        }
        public void FillLayoutInfoFromUser(TreeGridNode selectedTreeNode, bool IsShow = true)
        {
            try
            {


                string MyProjectId = GetProjectID(selectedTreeNode); ;


                MyProjectId = MyProjectId == "-1" ? string.Empty : MyProjectId;
                System.Data.DataTable dtLayoutInfo = GetdtLayoutInfoFromTag(selectedTreeNode) == null ? new System.Data.DataTable() : GetdtLayoutInfoFromTag(selectedTreeNode);
                frmLayoutVersionUpdate objfrm = new frmLayoutVersionUpdate(
                    Convert.ToString(selectedTreeNode.Cells["drawingid"].Value),
                     Convert.ToString(selectedTreeNode.Cells["drawing"].Value),
                      Convert.ToString(selectedTreeNode.Cells["cadtype"].FormattedValue),
                       Convert.ToString(selectedTreeNode.Cells["State"].FormattedValue),
                        Convert.ToString(selectedTreeNode.Cells["revision"].Value),
                        MyProjectId,
                        Convert.ToString(selectedTreeNode.Cells["filepath"].Value),
                        Convert.ToString(selectedTreeNode.Cells["DrawingNumberOld"].Value),
                        Convert.ToString(selectedTreeNode.Cells["DrawingNumber"].Value));
                objfrm.dtLayoutInfo = dtLayoutInfo.Copy();
                if (IsShow)
                {
                    objfrm.ShowDialog();
                }
                else
                {
                    objfrm.frmLayoutVersionUpdate_Load(null, null);
                    objfrm.btnSave_Click(null, null);
                }
                if (objfrm.dtLayoutInfo.Rows.Count > 0)
                {
                    if (Convert.ToString(selectedTreeNode.Cells["isroot"].FormattedValue) == "true")
                    {
                        AssigndtLayoutInfoToTag(selectedTreeNode, objfrm.dtLayoutInfo);
                    }
                    else
                    {
                        LoadFlag = true;
                        ChangeNodeTagValue(savetreeGrid.Nodes[0], selectedTreeNode, objfrm.dtLayoutInfo);
                        LoadFlag = false;
                    }
                    CheckStatusChange(selectedTreeNode, true);
                    selectedTreeNode.Cells["LayoutInfoStatus"].Value = "Filled";



                }
                else
                {
                    selectedTreeNode.Cells["LayoutInfoStatus"].Value = "Empty";
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }
        public void AssignFolderPath(TreeGridNode ParentNode, TreeGridNode CurrentNode, frmFolderSelection objFolderSelection)
        {
            try
            {
                foreach (TreeGridNode ChildNode in ParentNode.Nodes)
                {
                    if (Convert.ToString(ChildNode.Cells["oldFolderID1"].Value) == string.Empty || Convert.ToString(ChildNode.Cells["FolderID"].Value) == Convert.ToString(CurrentNode.Cells["oldFolderID1"].Value))
                    {
                        ChildNode.Cells["FolderID"].Value = objFolderSelection.FolderID;
                        ChildNode.Cells["FolderPath"].Value = objFolderSelection.FolderPath;
                        AssignStackFolderSearchReasultToTag(ChildNode, objFolderSelection.StackFolderSearchReasult);
                    }
                    AssignFolderPath(ChildNode, CurrentNode, objFolderSelection);
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }
        public void ChangeNodeTagValue(TreeGridNode ParentNode, TreeGridNode CurrentNode, DataTable dtLayoutInfo)
        {
            try
            {
                foreach (TreeGridNode ChildNode in ParentNode.Nodes)
                {
                    if (Convert.ToString(ChildNode.Cells["filepath"].Value) == Convert.ToString(CurrentNode.Cells["filepath"].Value))
                    {
                        ChildNode.Cells["Check"].Value = true;
                        AssigndtLayoutInfoToTag(ChildNode, dtLayoutInfo);
                    }
                    ChangeNodeTagValue(ChildNode, CurrentNode, dtLayoutInfo);
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }
        public void ChangeNodeValue(TreeGridNode ParentNode, TreeGridNode CurrentNode)
        {
            try
            {
                if (LoadFlag)
                {
                    return;
                }
                foreach (TreeGridNode ChildNode in ParentNode.Nodes)
                {
                    if (Convert.ToString(ChildNode.Cells["filepath"].Value) == Convert.ToString(CurrentNode.Cells["filepath"].Value) && ChildNode != CurrentNode)
                    {
                        ChildNode.Cells["cadtype"].Value = CurrentNode.Cells["cadtype"].Value;
                        ChildNode.Cells["State"].Value = CurrentNode.Cells["State"].Value;
                        ChildNode.Cells["Check"].Value =!Convert.ToBoolean(CurrentNode.Cells["Check"].Value);
                        ChildNode.Cells["FolderID"].Value = CurrentNode.Cells["FolderID"].Value;
                        ChildNode.Cells["FolderPath"].Value = CurrentNode.Cells["FolderPath"].Value;
                        LoadFlag = true;
                        ChildNode.Cells["OldFolderID1"].Value = CurrentNode.Cells["OldFolderID1"].Value;
                        LoadFlag = false;
                        //ChildNode.Cells["OldState"].Value = CurrentNode.Cells["OldState"].Value;
                        AssignStackFolderSearchReasultToTag(ChildNode, GetStackFolderSearchReasultFromTag(CurrentNode));
                    }
                    ChangeNodeValue(ChildNode, CurrentNode);
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }
        public bool GenFileInfo(TreeGridNode tgnParent)
        {
            try
            {

                foreach (TreeGridNode tgnChild in tgnParent.Nodes)
                {
                    if ((bool)tgnChild.Cells["Check"].Value)
                    {
                        if (GenFileInfoDT(tgnChild))
                            GenFileInfo(tgnChild);
                        else
                            return false;
                    }
                }
                return true;
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
                return false;
            }
        }
        public bool CheckFileExistance(TreeGridNode tgnParent)
        {
            try
            {

                foreach (TreeGridNode tgnChild in tgnParent.Nodes)
                {
                    if ((bool)tgnChild.Cells["Check"].Value)
                    {
                        if (!CheckFileExistance1(tgnChild))
                        {
                            return false;
                        }
                        else
                        {
                            if (!CheckFileExistance(tgnChild))
                            {
                                return false;
                            }
                        }

                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message); return false;
            }
            return true;
        }
        public bool CheckFileExistance1(TreeGridNode CurrentNode)
        {
            try
            {
                string ObjectId = Convert.ToString(CurrentNode.Cells["drawingID"].Value);
                string FP = Convert.ToString(CurrentNode.Cells["filepath"].Value);
                if (ObjectId.Trim().Length > 0)
                {
                    if (!objRBC.CheckFileExistance(ObjectId))
                    {
                        if (InfoYNMess("Following file no longer exist on server. do you want to update it as new ?") == DialogResult.No)
                        {
                            return false;
                        }
                        else
                        {
                            CurrentNode.Cells["drawingID"].Value = "";
                        }
                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message); return false;
            }
            return true;
        }
        public bool GenFileInfoDT(TreeGridNode selectedTreeNode)
        {
            try
            {
                #region "Concate  infromation with ';' for Drawing"
                String id = (String)selectedTreeNode.Cells["drawingid"].Value;
                String ItemType = (String)selectedTreeNode.Cells["itemtype"].Value;
                String FilePath = (String)selectedTreeNode.Cells["filepath"].Value;
                String MyProjectName = selectedTreeNode.Cells["projectname"].FormattedValue.ToString();
                String MyProjectId = selectedTreeNode.Cells["projectname"].Value.ToString();
                String MyProjectNo = "";
                string PreFix = Convert.ToString(selectedTreeNode.Cells["prefix"].Value);
                string FileTypeID = "";
                string FileStatusID = "";
                string FileStatus = GetFileStatus(selectedTreeNode);
                string FileType = "";

                System.Data.DataTable dtLayoutInfo = GetdtLayoutInfoFromTag(selectedTreeNode);

                if (dtLayoutInfo == null)
                {
                    dtLayoutInfo = new System.Data.DataTable();
                }
                if (dtLayoutInfo.Rows.Count == 0 || IsSaveAs)
                {
                    FillLayoutInfoFromUser(selectedTreeNode, false);
                    dtLayoutInfo = GetdtLayoutInfoFromTag(selectedTreeNode);
                }
                else
                {
                    selectedTreeNode.Cells["LayoutInfoStatus"].Value = "Filled";
                }

                lstdtLayoutInfo.Add(dtLayoutInfo);
                try
                {

                    MyProjectId = Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value));
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];
                    MyProjectNo = FindValueInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value)), "number");
                    MyProjectName = FindValueInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value)), "name");
                }
                catch
                {
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];

                    //c.Value =  FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "name");
                    //MyProjectId = Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value));
                    MyProjectId = FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                    MyProjectNo = FindIDInCMB((System.Data.DataTable)c.DataSource, "number", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                    MyProjectName = FindIDInCMB((System.Data.DataTable)c.DataSource, "name", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                }

                try
                {
                    //FileTypeID = Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["cadtype"].Value));
                    FileTypeID = Convert.ToString(selectedTreeNode.Cells["cadtype"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["cadtype"].Value));
                    FileType = Convert.ToString(selectedTreeNode.Cells["cadtype"].FormattedValue) == string.Empty ? "" : Convert.ToString(selectedTreeNode.Cells["cadtype"].FormattedValue);
                }
                catch
                {
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["cadtype"];
                    FileType = Convert.ToString(c.Value);
                    // c.Value =  FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["cadtype"].Value), "name");
                    //FileTypeID = Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["cadtype"].Value));
                    FileTypeID = FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["cadtype"].Value), "name");
                }
                try
                {
                    //FileStatusID = Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["State"].Value));
                    FileStatusID = Convert.ToString(selectedTreeNode.Cells["State"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["State"].Value));
                }
                catch
                {
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["State"];
                    //c.Value =  FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["State"].Value), "statusname");
                    //FileTypeID = Convert.ToString(selectedTreeNode.Cells["cadtype"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["cadtype"].Value));
                    FileStatusID = FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["State"].Value), "statusname");
                }

                if (MyProjectId == "0" || MyProjectId == "-1")
                {
                    MyProjectId = "";
                }
                if (FileTypeID == "0" || FileTypeID == "-1")
                {
                    FileTypeID = "";
                }
                if (FileStatusID == "0" || FileStatusID == "-1")
                {
                    FileStatusID = "";
                }

                selectedTreeNode.Cells["ProjectID"].Value = MyProjectId;
                selectedTreeNode.Cells["StatusID"].Value = FileStatusID;
                selectedTreeNode.Cells["TypeID"].Value = FileTypeID;
                selectedTreeNode.Cells["ProjNo"].Value = MyProjectNo;
                selectedTreeNode.Cells["ProjName"].Value = MyProjectName;
                selectedTreeNode.Cells["FileStatus"].Value = FileStatus;

                #endregion

                String DI = "";
                for (int i = 0; i < selectedTreeNode.Cells.Count; i++)
                {
                    DI += (Convert.ToString(selectedTreeNode.Cells[i].Value) + ";");
                }
                DI += (CADDescription.Text + ";");
                AllDrawing.Add(DI);
                try
                {
                    if (Path.GetExtension(Convert.ToString(selectedTreeNode.Cells["filepath"].Value)).Length > 0)
                    {
                        htAllDrawing.Add(selectedTreeNode.Cells["filepath"].Value, DI);
                    }
                    else
                    {
                        return false;
                    }

                }
                catch
                {

                }

                return true;
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
                return false;
            }
        }

        public void AssignLayoutInfo(TreeGridNode CurrentNode)
        {
            try
            {
                AssignLayoutInfo1(CurrentNode);
                foreach (TreeGridNode Child in CurrentNode.Nodes)
                {
                    AssignLayoutInfo1(Child);
                    AssignLayoutInfo(Child);
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }
        public void AssignLayoutInfo1(TreeGridNode CurrentNode)
        {
            try
            {
                foreach (TreeGridNode Child in CurrentNode.Nodes)
                {
                    System.Data.DataTable dtLayoutInfo = GetdtLayoutInfoFromTag(Child);

                    if (dtLayoutInfo == null)
                    {
                        dtLayoutInfo = new System.Data.DataTable();
                    }
                    if (dtLayoutInfo.Rows.Count == 0)
                    {
                        FillLayoutInfoFromUser(Child, false);
                        dtLayoutInfo = GetdtLayoutInfoFromTag(Child);
                    }
                    else
                    {
                        Child.Cells["LayoutInfoStatus"].Value = "Filled";
                    }

                    lstdtLayoutInfo.Add(dtLayoutInfo);
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }
        private void Save_Active_Drawings_Resize(object sender, EventArgs e)
        {
            //int f_height=this.Height;
            //int f_width = this.Width;
            //submit.Location = new Point((f_width / 2)-100, f_height - 85);
            //cancel.Location = new Point((f_width / 2)+100, f_height - 85);
            //Comments.Location = new Point(20, f_height - 88);
            //CADDescription.Location = new Point(100, f_height - 88);
        }

        private void savetreeGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void savetreeGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //if (savetreeGrid.IsCurrentCellDirty)
            //{
            //    savetreeGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //}

            if (savetreeGrid.CurrentCell is DataGridViewComboBoxCell)
            {
                savetreeGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
                //savetreeGrid.EndEdit();
            }
        }

        private void savetreeGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 5)
                {
                    TreeGridNode selectedTreeNode = (TreeGridNode)savetreeGrid.Rows[e.RowIndex];
                    if ((bool)selectedTreeNode.Cells["check"].EditedFormattedValue)
                    {
                        if (Convert.ToString(selectedTreeNode.Cells["canEditStatus"].Value).Trim().Length > 3)
                        {
                            if (!Convert.ToBoolean(selectedTreeNode.Cells["canEditStatus"].Value))
                            {
                                InfoMess("You dont have permission to change status of this file.");
                            }
                        }

                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }

        private void savetreeGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0 || LoadFlag)
                {
                    return;
                }
                TreeGridNode selectedTreeNode = (TreeGridNode)savetreeGrid.Rows[e.RowIndex];
                if (e.ColumnIndex == ProjectName.Index && Convert.ToString(selectedTreeNode.Cells["isroot"].Value) == "true")
                {
                    string MyProjectId = GetProjectID(selectedTreeNode), ProjectName = GetProjectName(selectedTreeNode), ProjectNameNo = GetProjectNameNo(selectedTreeNode);



                    selectedTreeNode.Cells["FolderPath"].Value = "";
                    selectedTreeNode.Cells["FolderID"].Value = "0";
                    selectedTreeNode.Cells["OldFolderID1"].Value = "0";
                    ProjectName = ProjectName == string.Empty ? "My Files" : ProjectName;
                    ProjectNameNo = ProjectNameNo == string.Empty ? "My Files" : ProjectNameNo;

                    MyProjectId = MyProjectId == "-1" ? "0" : MyProjectId;
                    AssignValuetoGridCell(selectedTreeNode, "FolderID", "0");
                    AssignValuetoGridCell(selectedTreeNode, "FolderPath", "");
                    AssignValuetoGridCell(selectedTreeNode, "projectname", ProjectNameNo);
                    List<clsFolderSearchReasult> objFolderSearchResult1 = objRBC.SearchFolder(MyProjectId, "0");
                    List<clsFolderSearchReasult> objFolderSearchResult = new List<clsFolderSearchReasult>(); ;
                    clsFolderSearchReasult obj = new clsFolderSearchReasult() { id = "-2", name = ProjectName, childFolderSize = Convert.ToInt16(objFolderSearchResult1.Count), companyId = "0" };
                    objFolderSearchResult.Add(obj);

                    Stack<List<clsFolderSearchReasult>> StackFolderSearchReasult = GetStackFolderSearchReasultFromTag(selectedTreeNode);
                    StackFolderSearchReasult.Clear();
                    StackFolderSearchReasult.Push(objFolderSearchResult);
                    AssignStackFolderSearchReasultToTag(selectedTreeNode, StackFolderSearchReasult);

                }
                if ((e.ColumnIndex == cadtype.Index || e.ColumnIndex == State.Index || e.ColumnIndex == OldFolderID1.Index) /*&& Convert.ToString(selectedTreeNode.Cells["isroot"].Value) == "false"*/)
                {
                    bool IsValueChanged = true;

                    if (e.ColumnIndex == State.Index)
                    {
                        if (LoadFlag)
                            return;
                        IsValueChanged = CheckStatusChange(selectedTreeNode);
                    }
                    if (IsValueChanged)
                    {
                        //if (e.ColumnIndex == State.Index)
                        //    selectedTreeNode.Cells["OldState"].Value = GetFileStatus(selectedTreeNode);

                        ChangeNodeValue(savetreeGrid.Nodes[0], selectedTreeNode);
                    }

                }

            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }

        public bool Check_Status_Change(TreeGridNode ParentNode)
        {

            try
            {
                bool RetVal = CheckStatusChange(ParentNode, true);
                if (!RetVal)
                    return !RetVal;
                foreach (TreeGridNode childNode in ParentNode.Nodes)
                {
                    if ((bool)childNode.Cells["Check"].FormattedValue)
                    {


                        RetVal = Check_Status_Change(childNode);
                        if (!RetVal)
                            return !RetVal;


                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message); return false;
            }
            return true;
        }
        //public bool Name_Duplication_Check1(TreeGridNode ParentNode)
        //{

        //    try
        //    {
        //        bool RetVal = false;

        //        foreach (TreeGridNode childNode in ParentNode.Nodes)
        //        {
        //            if ((bool)childNode.Cells["Check"].FormattedValue)
        //            {

        //                RetVal = FileNameDuplicationCheck(childNode);
        //                if (RetVal)
        //                    return RetVal;
        //                else
        //                {
        //                    RetVal = Name_Duplication_Check1(childNode);
        //                    if (RetVal)
        //                        return RetVal;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception E)
        //    {
        //         ErrorMess(E.Message); return true;
        //    }
        //    return false;
        //}

        public bool CheckStatusChange(TreeGridNode selectedTreeNode, bool ShowFileName = false)
        {
            LoadFlag = true;
            try
            {
                bool OldStatusTypeClosed = IsClosedStatus(selectedTreeNode, true);
                bool NewStatusTypeCLosed = IsClosedStatus(selectedTreeNode);
                string FileName = "";

                if (ShowFileName)
                {
                    FileName = Convert.ToString(selectedTreeNode.Cells["drawing"].Value) + Environment.NewLine;
                }



                if (OldStatusTypeClosed && NewStatusTypeCLosed)
                {

                }
                else if (OldStatusTypeClosed && !NewStatusTypeCLosed)
                {
                    if (ValMessYN(FileName + "Changing file status from '" + GetFileStatus(selectedTreeNode, true) + "' to '" + GetFileStatus(selectedTreeNode) + "' will lead to creation of new revision, \n Do you want to proceeds anyway ?") == DialogResult.No)
                    {
                        selectedTreeNode.Cells["State"].Value = selectedTreeNode.Cells["OldState"].Value;
                    }
                    else
                    {
                        LoadFlag = false;
                        return false;
                    }
                }
                if (!OldStatusTypeClosed && NewStatusTypeCLosed)
                {
                    if (!IsAllLayoutClose(selectedTreeNode))
                    {
                        ValMess("You can not change status to '" + GetFileStatus(selectedTreeNode) + "' unless all layouts are in Close state.");


                        selectedTreeNode.Cells["State"].Value = selectedTreeNode.Cells["OldState"].Value;
                        //savetreeGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        //;
                        //savetreeGrid.CurrentCell = selectedTreeNode.Cells["Check"];
                        //savetreeGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        //savetreeGrid.RefreshEdit();
                        //savetreeGrid.ClearSelection();
                        //cancel.Focus();

                        savetreeGrid.Refresh();
                        //Refresh();
                        LoadFlag = false;
                        return false;
                    }
                }
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
            LoadFlag = false;
            return true;
        }
        public bool IsAllLayoutClose(TreeGridNode CurrentNode)
        {
            try
            {
                DataRow[] dtLayoutInfo = GetdtLayoutInfoFromTag(CurrentNode).Select("IsFile='0'");
                DataTable dtStatus = objRBC.GetFIleStatus();
                if (dtStatus != null)
                {
                    foreach (DataRow dr in dtLayoutInfo)
                    {
                        string LayoutStatus = Convert.ToString(dr["LayoutStatus"]);
                        DataRow[] dr1 = dtStatus.Select("statusname = '" + LayoutStatus + "' and IsClosed ='False'");


                        if (dr1.Length > 0)
                        {
                            return false;
                        }




                        //foreach (DataRow dr1 in dtStatus.Rows)
                        //{
                        //    string Status = Convert.ToString(dr1["statusname"]);
                        //    bool IsClose = Convert.ToBoolean(dr1["IsClosed"]);

                        //    if (LayoutStatus == Status && !IsClose)
                        //    {
                        //        return false;
                        //    }
                        //}

                    }
                }

                return true;
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
            return false;
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
                ErrorMess(E.Message);
            }
            return false;
        }
        public void AssignValuetoGridCell(TreeGridNode Parent, string ColumnName, string Value)
        {
            try
            {
                foreach (TreeGridNode Child in Parent.Nodes)
                {
                    Child.Cells[ColumnName].Value = Value;
                    AssignValuetoGridCell(Child, ColumnName, Value);
                }
                //Parent.Cells[ColumnName].Value = Value;
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }

        private void savetreeGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void savetreeGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0 || LoadFlag)
                {
                    return;
                }
                //TreeGridNode selectedTreeNode = (TreeGridNode)savetreeGrid.Rows[e.RowIndex];
                //if ((e.ColumnIndex == cadtype.Index || e.ColumnIndex == State.Index || e.ColumnIndex == OldFolderID1.Index) /*&& Convert.ToString(selectedTreeNode.Cells["isroot"].Value) == "false"*/)
                //{
                //    bool IsValueChanged = true;

                //    if (e.ColumnIndex == State.Index)
                //    {
                //        if (LoadFlag)
                //            return;
                //        LoadFlag = true;
                //        if (IsClosedStatus(selectedTreeNode))
                //        {
                //            if (!IsAllLayoutClose(selectedTreeNode))
                //            {
                //                 ValMess("You can not change status to '" + GetFileStatus(selectedTreeNode) + "' unless all layouts are in Close state.");

                //                //selectedTreeNode.Cells["State"].Value = selectedTreeNode.Cells["OldState"].Value;
                //                //savetreeGrid.CurrentCell = selectedTreeNode.Cells["Check"];
                //                //savetreeGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
                //                //savetreeGrid.RefreshEdit();
                //                //savetreeGrid.ClearSelection();
                //                //cancel.Focus();

                //                // savetreeGrid.Refresh();
                //                //Refresh();
                //                IsValueChanged = false;
                //            }
                //        }
                //        LoadFlag = false;
                //    }
                //    if (IsValueChanged)
                //    {
                //        if (e.ColumnIndex == State.Index)
                //            selectedTreeNode.Cells["OldState"].Value = GetFileStatus(selectedTreeNode);

                //        ChangeNodeValue(savetreeGrid.Nodes[0], selectedTreeNode);
                //    }
                //}
            }
            catch (Exception E)
            {
                ErrorMess(E.Message);
            }
        }

        private void pnlTop_Paint(object sender, PaintEventArgs e)
        {

        }



        /*  private void ExpandNodeForCheck(TreeGridNode CurrentNode)
          {
              try
              {
                  for (int nodeCount = 0; nodeCount < CurrentNode.Nodes.Count(); nodeCount++)
                  {
                      if (CurrentNode.Nodes.ElementAt(nodeCount).Nodes.Count() == 0)
                      {

                          if (CurrentNode.Cells["drawingid"].Value != "")
                          {
                              CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].Value = CurrentNode.Cells[0].EditedFormattedValue;
                              String id = (string)CurrentNode.Nodes.ElementAt(nodeCount).Cells[5].Value;
                              String ItemType = "CAD";
                              String FilePath = (string)CurrentNode.Nodes.ElementAt(nodeCount).Cells[6].Value;
                              String DarawingInfo = id + ";" + ItemType + ";" + FilePath;

                              id = (string)CurrentNode.Nodes.ElementAt(nodeCount).Cells[5].Value;

                              if ((bool)CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].Value)
                                  drawings.Add(DarawingInfo);
                              else
                                  drawings.Remove(DarawingInfo);
                          }
                      }
                      else
                      {
                          TreeGridNode node1 = CurrentNode.Nodes.ElementAt(nodeCount);
                          CurrentNode.Nodes.ElementAt(nodeCount).Expand();
                          CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].Value = CurrentNode.Cells[0].EditedFormattedValue;
                          String id = (string)CurrentNode.Nodes.ElementAt(nodeCount).Cells[5].Value;
                          String ItemType = "CAD";
                          String FilePath = (string)CurrentNode.Nodes.ElementAt(nodeCount).Cells[6].Value;
                          String DarawingInfo = id + ";" + ItemType + ";" + FilePath;

                          id = (string)CurrentNode.Nodes.ElementAt(nodeCount).Cells[5].Value;

                          if ((bool)CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].Value)
                              drawings.Add(DarawingInfo);
                          else
                              drawings.Remove(DarawingInfo);

                          this.ExpandNodeForCheck(node1);

                      }
                  }
              }
              catch (System.Exception ex)
              {
                  System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
                  return;
              }
          }       */
    }
}