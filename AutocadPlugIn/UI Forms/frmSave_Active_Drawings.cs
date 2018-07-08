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

using RedBracketConnector;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Newtonsoft.Json;
using RestSharp;
namespace AutocadPlugIn.UI_Forms
{
    public partial class frmSave_Active_Drawings : Form
    {
        public ArrayList drawings = new ArrayList();
        public ArrayList drawingsOpen = new ArrayList();
        public Hashtable htNewDrawings = new Hashtable();
        private System.Data.DataTable dtTreeGridData = new System.Data.DataTable();
        RBConnector objRBC = new RBConnector();
        Stack<List<clsFolderSearchReasult>> StackFolderSearchReasult = new Stack<List<clsFolderSearchReasult>>();
        List<System.Data.DataTable> lstdtLayoutInfo = new List<System.Data.DataTable>();
        public bool LoadFlag = false;
        bool IsParentNew = false;
        bool IsChildOld = false;
        bool IsParentNewChildOld = false;
        bool IsSaveAs = false;
        public List<String> AllDrawing = new List<String>();
        public frmSave_Active_Drawings(bool IsSaveAs = false)
        {

            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.IsSaveAs = IsSaveAs;
            //ICADManager objCadMgr = CADFactory.getCADManager();
            //objCadMgr.SaveActiveDrawing();

        }

        private void Save_Active_Drawings_Load(object sender, EventArgs e)
        {
            LoadFlag = true;
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                savetreeGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);
                this.imageStrip.ImageSize = new System.Drawing.Size(17, 17);
                this.imageStrip.TransparentColor = System.Drawing.Color.Magenta;
                this.imageStrip.ImageSize = new Size(17, 17);
                this.imageStrip.Images.AddStrip(Properties.Resources.LockImageStrip1);
                savetreeGrid.ImageList = imageStrip;
                int totalCount = savetreeGrid.Nodes.Count;
                int counter = 0;

                ICADManager objCadMgr = CADFactory.getCADManager();
                //objCadMgr.SaveActiveDrawing();
                TreeGridNode node = null;
                for (int i = 0; i < totalCount; i++)
                {
                    TreeGridNode TreeNode1 = savetreeGrid.Nodes.ElementAt(0);
                    this.ClearTreeView(TreeNode1);
                    savetreeGrid.Nodes.Remove(TreeNode1);
                }

                dtTreeGridData = objCadMgr.GetExternalRefreces();


                string FoldID = "", FoldPath = "", ProJNmNo = "";
                for (int i = 0; i < dtTreeGridData.Rows.Count; i++)
                {

                    if (Convert.ToString(dtTreeGridData.Rows[i]["isroot"]).ToLower() == "true" &&
                        Convert.ToString(dtTreeGridData.Rows[i]["DrawingId"]).Trim().Length == 0)
                    {
                        IsParentNew = true;
                    }



                    if (Convert.ToString(dtTreeGridData.Rows[i]["isroot"]).ToLower() == "true" &&
                        Convert.ToString(dtTreeGridData.Rows[i]["DrawingId"]).Trim().Length > 0)
                    {
                        FoldID = Convert.ToString(dtTreeGridData.Rows[i]["folderid"]);
                        FoldPath = Convert.ToString(dtTreeGridData.Rows[i]["folderpath"]);
                        ProJNmNo = Convert.ToString(dtTreeGridData.Rows[i]["ProjectNameNo"]);
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

                    //if (IsParentNew && Convert.ToString(dtTreeGridData.Rows[i]["isroot"]).ToLower() == "false" &&
                    //    Convert.ToString(dtTreeGridData.Rows[i]["DrawingId"]).Trim().Length == 0)
                    //{
                    //    IsParentNew = true;
                    //}





                    dtTreeGridData.Rows[i]["sourceid"] = "true";
                    if (Convert.ToString(dtTreeGridData.Rows[i]["DrawingId"]).Trim().Length > 0)
                    {
                        decimal CurrentVersion = Convert.ToDecimal(dtTreeGridData.Rows[i]["Revision"]);
                        decimal LatestVersion = Convert.ToDecimal(Helper.GetLatestVersion(Convert.ToString(dtTreeGridData.Rows[i]["DrawingNumber"])));

                        if (CurrentVersion < LatestVersion)
                        {
                            if (ShowMessage.InfoYNMess("Newer version file\n" + Convert.ToString(dtTreeGridData.Rows[i]["DrawingName"]) + "\n is already available on server, Are you sure you want to upload ?") == DialogResult.No)
                            {
                                dtTreeGridData.Rows[i]["sourceid"] = "false";
                                if (Convert.ToString(dtTreeGridData.Rows[i]["isroot"]).ToLower() == "true")
                                {
                                    this.Close();
                                    return;
                                }
                            }

                        }
                    }
                }


                SaveCommand lockStatusCmd = new SaveCommand();
                lockStatusCmd.DrawingInfo = dtTreeGridData;

                SaveController lockStatusCon = new SaveController();
                dtTreeGridData = lockStatusCon.getLockStatus(lockStatusCmd);
                if (lockStatusCon.errorString != null)
                {
                    ShowMessage.ErrorMess(lockStatusCon.errorString);
                    this.Close();
                    return;
                }

                #region CreateTreeGrid


                Helper.FIllCMB(cadtype, objRBC.GetFIleType(), "name", "id", true);
                Helper.FIllCMB(State, objRBC.GetFIleStatus(), "statusname", "id", true);
                Helper.FIllCMB(ProjectName, objRBC.GetProjectDetail(), "PNAMENO", "id", true, "My Files");





                foreach (DataRow rw in dtTreeGridData.Rows)
                {
                    if (Convert.ToString(rw["sourceid"]) == "false")
                    {
                        continue;
                    }


                    CADIntegrationConfiguration objWordConfig = new CADIntegrationConfiguration();



                    #region LayoutInfo
                    string LayoutInfo1 = Convert.ToString(rw["layoutinfo"]);
                    List<LayoutInfo> objLI = JsonConvert.DeserializeObject<List<LayoutInfo>>(LayoutInfo1);

                    System.Data.DataTable dtLayoutInfo = new System.Data.DataTable();
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
                            dr["LayoutStatus"] = obj.statusname;
                            dr["Version"] = obj.versionno;
                            dr["Description"] = obj.description;
                            dr["IsFile"] = "0";
                            dr["TypeID"] = obj.typeId;
                            dr["StatusID"] = obj.statusId;
                            dr["ACLayoutID"] = obj.layoutId == null ? string.Empty : obj.layoutId;
                            dr["LayoutName1"] = obj.name;

                            dr["LayoutNo"] = obj.fileNo;
                            dr["CreatedBy"] = obj.createdby;
                            dr["CreatedOn"] = obj.createdon;
                            dr["UpdatedBy"] = obj.updatedby;
                            dr["UpdatedOn"] = obj.updatedon;
                            dtLayoutInfo.Rows.Add(dr);
                        }
                    }



                    #endregion
                    if (counter == 0 || rw["isroot"].ToString() == "true")
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


                        node = savetreeGrid.Nodes.Add(false, rw["drawingName"].ToString(), rw["drawingnumber"].ToString(), rw["Classification"].ToString(),
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
                             , "", "", "","" //project,status,type id,ProjNo
                            );


                        if (rw["drawingid"].ToString() == "")
                        {
                            node.Cells["projectname"].Value = "";
                            node.Cells["projectname"].ReadOnly = false;
                            node.Cells["drawingnumber"].Value = "AutoFill";
                        }
                        else
                        {
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
                        counter = 1;
                        node.Tag = dtLayoutInfo;
                    }
                    else
                    {

                        TreeGridNode node1 = node.Nodes.Add(false, rw["drawingName"].ToString(), rw["drawingnumber"].ToString(), rw["Classification"].ToString(),
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
                                  );


                        node1.Cells["projectname"].ReadOnly = true;
                        if (rw["drawingid"].ToString() == "")
                        {
                            if (IsParentNew)
                            {
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

                        node1.Tag = dtLayoutInfo;
                    }


                }
                #endregion CreateTreeGrid

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            LoadFlag = false;
            Cursor.Current = Cursors.Default;
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
                System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
                return;
            }

        }

        private void submit_Click(object sender, EventArgs e)
        {
            try
            {
                submit.Focus();
                List<TreeGridNode> selectedTreeGridNodes = new List<TreeGridNode>();
                this.Cursor = Cursors.WaitCursor;
                bool Is_Delete = false;
                bool Is_Save = false;
                int PBValue = 7;
                #region Commented Code
                //String FilePath = db.OriginalFileName;

                //Test Code
                try
                {
                    //Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                    //DocumentLock doclock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument();
                    //Database db = doc.Database;


                    //    db.ThumbnailBitmap = doc.CapturePreviewImage(100,100);
                    //    Bitmap bm = db.ThumbnailBitmap;

                    //    //db.SaveAs("castial", DwgVersion.Current);
                    //    pictureBox1.Image = bm;
                    //return;

                }
                catch (Exception E)
                {

                }
                #endregion

                //To check howmany file is selected.
                foreach (TreeGridNode treeGridNode in savetreeGrid.Nodes)
                {
                    if ((bool)treeGridNode.Cells[0].FormattedValue)
                    {
                        selectedTreeGridNodes.Add(treeGridNode);
                        PBValue++; PBValue++;
                    }
                }


                if (selectedTreeGridNodes.Count < 1)
                {
                    ShowMessage.ValMess("Please select at least one file to save.");
                    this.Cursor = Cursors.Default;
                    return;
                }

                foreach (TreeGridNode ParentNode in savetreeGrid.Nodes)
                {
                    foreach (TreeGridNode ChildNode in ParentNode.Nodes)
                    {
                        if (IsParentNewChildOld)
                        {
                            if (Convert.ToString(ChildNode.Cells["drawingID"].Value).Trim().Length > 0)
                            {
                                if (Convert.ToString(ChildNode.Cells["ProjectName"].Value) == Convert.ToString(ChildNode.Cells["OldProject"].Value))
                                {
                                    ChildNode.Cells["FolderID"].Value = ChildNode.Cells["OldFolderID"].Value;
                                    ChildNode.Cells["FolderPath"].Value = "";
                                }
                                else
                                {
                                    ChildNode.Cells["drawingID"].Value = "";
                                    System.Data.DataTable dtLayoutInfo = (System.Data.DataTable)ChildNode.Tag;

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
                            }
                        }
                    }
                }
                //To check whether any file with same name exist or not
                #region Duplication Check
                bool IsDuplicationCheck = true;
                if (IsDuplicationCheck)
                {
                    foreach (TreeGridNode currentTreeGrdiNode in selectedTreeGridNodes)
                    {
                        System.Data.DataTable dtLayoutInfo = (System.Data.DataTable)currentTreeGrdiNode.Tag;
                        //File name and layout out name length validation
                        for (int i = 0; i < dtLayoutInfo.Rows.Count; i++)
                        {
                            string FN = Helper.RemovePreFixFromFileName(Path.GetFileName(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim()), Convert.ToString(currentTreeGrdiNode.Cells["prefix"].Value).Trim());
                            string LN = Convert.ToString(dtLayoutInfo.Rows[i]["FileLayoutName"]);

                            if (Convert.ToString(dtLayoutInfo.Rows[i]["IsFile"]) == "1")
                            {
                                if (FN.Length < Helper.FileLayoutNameLength)
                                {

                                }
                                else
                                {
                                    ShowMessage.ErrorMess(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim() + "\n File name length exceeds maximum limit.");
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                            }
                            else
                            {
                                if (LN.Length < Helper.FileLayoutNameLength)
                                {

                                }
                                else
                                {
                                    ShowMessage.ErrorMess(LN + "\n Layout name length exceeds maximum limit. of file " + Environment.NewLine + FN);
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                            }
                        }
                        string MyProjectId = "";
                        try
                        {

                            MyProjectId = Convert.ToString(currentTreeGrdiNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(currentTreeGrdiNode.Cells["projectname"].Value));
                        }
                        catch
                        {
                            DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)currentTreeGrdiNode.Cells["projectname"];

                            MyProjectId = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(currentTreeGrdiNode.Cells["projectname"].Value), "PNAMENO");
                        }
                        MyProjectId = MyProjectId == "-1" ? string.Empty : MyProjectId;
                        if (Convert.ToString(currentTreeGrdiNode.Cells["drawingID"].Value).Trim().Length == 0)
                        {
                            if (File.Exists(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim()))
                            {
                                string FN = Helper.RemovePreFixFromFileName(Path.GetFileName(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim()), Convert.ToString(currentTreeGrdiNode.Cells["prefix"].Value).Trim());

                                if (!objRBC.CheckFileExistance(MyProjectId, FN))
                                {
                                    this.Cursor = Cursors.Default;
                                    return;
                                }


                            }
                            else
                            {
                                ShowMessage.ErrorMess(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim() + "\n File does not exists.");
                                this.Cursor = Cursors.Default;
                                return;
                            }
                        }
                        foreach (TreeGridNode ChildNode in currentTreeGrdiNode.Nodes)
                        {
                            //File name and layout out name length validation
                            dtLayoutInfo = (System.Data.DataTable)ChildNode.Tag;
                            for (int i = 0; i < dtLayoutInfo.Rows.Count; i++)
                            {
                                string FN = Helper.RemovePreFixFromFileName(Path.GetFileName(Convert.ToString(ChildNode.Cells["filepath"].Value).Trim()), Convert.ToString(ChildNode.Cells["prefix"].Value).Trim());
                                string LN = Convert.ToString(dtLayoutInfo.Rows[i]["FileLayoutName"]);
                                if (FN.Length < Helper.FileLayoutNameLength)
                                {

                                }
                                else
                                {
                                    ShowMessage.ErrorMess(FN + "\n File name length exceeds maximum limit.");
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                                if (LN.Length < Helper.FileLayoutNameLength)
                                {

                                }
                                else
                                {
                                    ShowMessage.ErrorMess(LN + "\n Layout name length exceeds maximum limit. of file " + Environment.NewLine + FN);
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                            }
                            MyProjectId = "";
                            try
                            {

                                MyProjectId = Convert.ToString(ChildNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(ChildNode.Cells["projectname"].Value));
                            }
                            catch
                            {
                                DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)currentTreeGrdiNode.Cells["projectname"];

                                MyProjectId = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(ChildNode.Cells["projectname"].Value), "PNAMENO");
                            }
                            MyProjectId = MyProjectId == "-1" ? string.Empty : MyProjectId;
                            if (Convert.ToString(ChildNode.Cells["drawingID"].Value).Trim().Length == 0)
                            {
                                if (File.Exists(Convert.ToString(ChildNode.Cells["filepath"].Value).Trim()))
                                {
                                    string FN = Helper.RemovePreFixFromFileName(Path.GetFileName(Convert.ToString(ChildNode.Cells["filepath"].Value).Trim()), Convert.ToString(ChildNode.Cells["prefix"].Value).Trim());
                                    if (!objRBC.CheckFileExistance(MyProjectId, FN))
                                    {
                                        this.Cursor = Cursors.Default;
                                        return;
                                    }

                                }
                                else
                                {
                                    ShowMessage.ErrorMess(Convert.ToString(ChildNode.Cells["filepath"].Value).Trim() + "\n File does not exists.");
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                            }
                        }
                    }
                }
                #endregion
                //To check whether any file is selected or not



                //string checkoutPath = Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath").ToString();
                SaveController objController = new SaveController();
                AutoCADManager objMgr = new AutoCADManager();
                SaveCommand objCmd = new SaveCommand();

                lstdtLayoutInfo = new List<System.Data.DataTable>();
                //htNewDrawings.Clear();
                //drawings.Clear();
                AllDrawing.Clear();
                GenFileInfo();
                //return;
                //ICollection keys = htNewDrawings.Keys;
                //IEnumerator key = keys.GetEnumerator();
                //while (key.MoveNext())
                //{
                //    objCmd.NewDrawings.Add(htNewDrawings[key.Current.ToString()].ToString());
                //}

                //foreach (String str in drawings)
                //{
                //    objCmd.Drawings.Add(str);
                //}
                objCmd.AllDrawing = AllDrawing;
                // to ask user want to delete file after save or not.
                if (ShowMessage.InfoYNMess("Would you like to delete local file/files" + Environment.NewLine + " after saving it to RedBracket ?") == DialogResult.Yes)
                {
                    PBValue++;
                    Is_Delete = true;
                }



                // to iterate selected file
                foreach (TreeGridNode currentTreeGrdiNode in selectedTreeGridNodes)
                {

                    if (Convert.ToString(currentTreeGrdiNode.Cells["isEditable"].Value).Length > 3)
                    {
                        if (!Convert.ToBoolean(Convert.ToString(currentTreeGrdiNode.Cells["isEditable"].Value).ToLower()))
                        {
                            ShowMessage.InfoMess("You dont have edit permission for this file.");
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    String FilePath = objCmd.FilePath = Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value);

                    try
                    {
                        objMgr.SaveActiveDrawing(false);
                    }
                    catch { }


                    bool IsRenameXref = true;
                    Helper.GetProgressBar(PBValue, "File Save in Progress...", "Closing files to upload.");

                    if (IsRenameXref)
                    {
                        #region Close Files
                        bool IsCloseFile = true;

                        if (IsCloseFile)
                        {
                            objMgr.ChecknCloseOpenedDoc(FilePath);
                            foreach (TreeGridNode ChildNode in currentTreeGrdiNode.Nodes)
                            {
                                if ((bool)ChildNode.Cells[0].FormattedValue)
                                {
                                    objMgr.ChecknCloseOpenedDoc(Convert.ToString(ChildNode.Cells["filepath"].Value));
                                }
                            }
                        }
                        #endregion



                        Helper.IncrementProgressBar(1, "Uploading file properties.");

                        // save properties in RB
                        Is_Save = objController.ExecuteSave(objCmd, false, lstdtLayoutInfo);



                        if (Is_Save)
                        {
                            //Helper.CloseProgressBar();


                            #region Update File Properties 

                            Helper.IncrementProgressBar(1, "Updating new properties to local file.");

                            // Update document info into document for future referance 
                            if (objController.dtDrawingProperty.Rows.Count > 0)
                            {
                                objMgr.UpdateExRefInfo(objCmd.FilePath, objController.dtDrawingProperty);
                            }

                            //Update Xref name to wihout prefix 
                            //objMgr.UpdateExRefPathInfo1(FilePath);


                            #endregion
                            Helper.IncrementProgressBar(1, "Updating file names.");


                            objMgr.UpdateExRefPathInfo2(objCmd.FilePath, ref objController.plmObjs);

                            Helper.IncrementProgressBar(1, "Uploading file to redbracket.");


                            // save File in RB
                            Is_Save = objController.ExecuteSave(objCmd, true);
                            #region  Renaming Parent and XRef Files

                            //update Xref name to NewXref Name with PreFix


                            #endregion



                        }


                    }

                    // To delete file
                    if (Is_Delete && File.Exists(objCmd.FilePath) && Is_Save)
                    {
                        Helper.IncrementProgressBar(1, "Deleting local files.");

                        // Save and close the file before deleting. Otherwise system will not allow you to Delete the file.
                        //objMgr.SaveActiveDrawing(); // Saves the current active drawing.
                        //objMgr.CloseActiveDocument(objCmd.FilePath);    // Close the active document. Specify the file name, other files may be opened.
                        File.Delete(objCmd.FilePath);
                        foreach (TreeGridNode ChildNode in currentTreeGrdiNode.Nodes)
                        {
                            if ((bool)ChildNode.Cells[0].FormattedValue)
                            {
                                File.Delete(Convert.ToString(ChildNode.Cells["filepath"].Value));
                            }
                        }

                    }
                }
                Helper.IncrementProgressBar(PBValue, "");
                this.Cursor = Cursors.Default;
                if (Is_Save)
                {
                    Helper.CloseProgressBar();
                    ShowMessage.InfoMess("Save operation successfully completed.");
                    this.Close();
                    return;
                }
                else
                {
                    Helper.CloseProgressBar();
                    ShowMessage.ErrorMess("Save operation unsuccessfully completed.");
                    return;
                }

            }
            catch (Exception ex)
            {
                Helper.CloseProgressBar();
                ShowMessage.ErrorMess(ex.Message);
                this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
        }



        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void savetreeGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // return;
            TreeGridNode selectedTreeNode = (TreeGridNode)savetreeGrid.Rows[e.RowIndex];
            if (e.ColumnIndex == 0)
            {



                #region "Lock Drawings"
                if (selectedTreeNode.Cells["lockstatus"].Value.ToString() == "0")
                {
                    ArrayList arDrawingIDs = new ArrayList();
                    arDrawingIDs.Add(selectedTreeNode.Cells["drawingID"].Value.ToString());
                    frmLock1 objLock = new frmLock1();
                    if (!objLock.LockDrawings(arDrawingIDs))
                    {
                        selectedTreeNode.ImageIndex = 0;
                        selectedTreeNode.Cells["lockstatus"].Value = "1";
                    }
                }
                if (selectedTreeNode.Cells["lockstatus"].Value.ToString() == "2")
                {
                    selectedTreeNode.Cells["Check"].Value = false;
                    savetreeGrid.RefreshEdit();
                    MessageBox.Show("This drawing is locked by other user.");

                    return;
                }
                #endregion


                if ((bool)selectedTreeNode.Cells["check"].EditedFormattedValue)
                {


                    if (Convert.ToString(selectedTreeNode.Cells["drawingid"].Value).Trim().Length > 0)
                    {

                        if (!Convert.ToBoolean(selectedTreeNode.Cells["isletest"].Value))
                        {
                            ShowMessage.ValMess("This file is not latest file, so you can not save it."); selectedTreeNode.Cells["check"].Value = false; savetreeGrid.RefreshEdit();
                            return;
                        }
                        if (Convert.ToBoolean(selectedTreeNode.Cells["hasStatusClosed"].Value))
                        {
                            ShowMessage.ValMess("This file is closed, so you can not save it."); selectedTreeNode.Cells["check"].Value = false; savetreeGrid.RefreshEdit();
                            return;
                        }

                        if (Convert.ToBoolean(selectedTreeNode.Cells["isowner"].Value))
                        {
                        }
                        else
                        {
                            if (!Convert.ToBoolean(selectedTreeNode.Cells["isEditable"].Value))
                            {
                                ShowMessage.InfoMess("This file is not editable. so you can not save it.");
                                selectedTreeNode.Cells["check"].Value = false;
                                savetreeGrid.RefreshEdit();
                                return;
                            }
                        }


                    }
                    //if (Convert.ToString(selectedTreeNode.Cells["isroot"].Value) == "1")
                    {
                        string MyProjectId = "";
                        try
                        {
                            MyProjectId = Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value));
                        }
                        catch
                        {
                            DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];
                            MyProjectId = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                        }

                        MyProjectId = MyProjectId == "-1" ? string.Empty : MyProjectId;
                        System.Data.DataTable dtLayoutInfo = selectedTreeNode.Tag == null ? new System.Data.DataTable() : (System.Data.DataTable)selectedTreeNode.Tag;
                        frmLayoutVersionUpdate objfrm = new frmLayoutVersionUpdate(
                            Convert.ToString(selectedTreeNode.Cells["drawingid"].Value),
                             Convert.ToString(selectedTreeNode.Cells["drawing"].Value),
                              Convert.ToString(selectedTreeNode.Cells["cadtype"].FormattedValue),
                               Convert.ToString(selectedTreeNode.Cells["State"].FormattedValue),
                                Convert.ToString(selectedTreeNode.Cells["revision"].Value),
                                MyProjectId,
                                Convert.ToString(selectedTreeNode.Cells["filepath"].Value));
                        objfrm.dtLayoutInfo = dtLayoutInfo.Copy();
                        objfrm.ShowDialog();
                        if (objfrm.dtLayoutInfo.Rows.Count > 0)
                        {
                            selectedTreeNode.Tag = objfrm.dtLayoutInfo;
                        }
                    }



                    if (Convert.ToString(selectedTreeNode.Cells["drawingid"].Value) == string.Empty)
                    {
                        selectedTreeNode.Cells["projectname"].ReadOnly = false;
                    }
                    else
                    {
                        selectedTreeNode.Cells["projectname"].ReadOnly = true;
                    }

                }
            }
            if (e.ColumnIndex == BtnBrowseFolder.Index)
            {
                if (Convert.ToString(selectedTreeNode.Cells["isroot"].Value) == "true")
                {
                    string MyProjectId = "", FolderID = "", ProjectName = "", FolderPath = "";

                    try
                    {
                        MyProjectId = Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value));
                        ProjectName = Convert.ToString(selectedTreeNode.Cells["projectname"].FormattedValue);
                    }
                    catch
                    {
                        DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];

                        MyProjectId = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                        ProjectName = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "name", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                    }

                    FolderID = Convert.ToString(selectedTreeNode.Cells["FolderID"].Value).Trim();
                    FolderPath = Convert.ToString(selectedTreeNode.Cells["FolderPath"].Value).Trim();
                    MyProjectId = MyProjectId == "0" || MyProjectId == "-1" ? "0" : MyProjectId;
                    FolderID = FolderID == string.Empty || FolderID == "0" || FolderID == "-1" ? "0" : FolderID;
                    ProjectName = ProjectName == string.Empty ? "My Files" : ProjectName;

                    if (StackFolderSearchReasult.Count == 0)
                    {
                        //List<clsFolderSearchReasult> objFolderSearchResult = objRBC.SearchFolder(MyProjectId, FolderID);

                        List<clsFolderSearchReasult> objFolderSearchResult = new List<clsFolderSearchReasult>(); ;
                        clsFolderSearchReasult obj = new clsFolderSearchReasult() { id = "-2", name = ProjectName, childFolderSize = 0, companyId = "0" };
                        objFolderSearchResult.Add(obj);
                        StackFolderSearchReasult.Clear();
                        StackFolderSearchReasult.Push(objFolderSearchResult);

                        if (objFolderSearchResult == null)
                        {
                            ShowMessage.InfoMess("No folder found inside project " + ProjectName); return;
                        }
                        else
                        {
                            if (StackFolderSearchReasult.Count == 0)
                            {
                                StackFolderSearchReasult.Push(objFolderSearchResult);
                            }


                        }
                    }

                    Stack<List<clsFolderSearchReasult>> TStackFolderSearchReasult = new Stack<List<clsFolderSearchReasult>>();
                    TStackFolderSearchReasult = Helper.CopyStack(StackFolderSearchReasult);


                    frmFolderSelection objFolderSelection = new frmFolderSelection(FolderID, MyProjectId, StackFolderSearchReasult, FolderPath);
                    objFolderSelection.ShowDialog();
                    if (objFolderSelection.IsSelect)
                    {
                        selectedTreeNode.Cells["FolderID"].Value = objFolderSelection.FolderID;
                        selectedTreeNode.Cells["FolderPath"].Value = objFolderSelection.FolderPath;

                        foreach (TreeGridNode Child in selectedTreeNode.Nodes)
                        {
                            Child.Cells["FolderID"].Value = objFolderSelection.FolderID;
                            Child.Cells["FolderPath"].Value = objFolderSelection.FolderPath;
                        }
                    }
                    else
                    {
                        StackFolderSearchReasult = TStackFolderSearchReasult;
                    }
                }
            }
            //#endregion

        }
        public void GenFileInfo()
        {
            try
            {
                foreach (TreeGridNode tgnParent in savetreeGrid.Nodes)
                {

                    foreach (TreeGridNode tgnChild in tgnParent.Nodes)
                    {
                        if ((bool)tgnChild.Cells[0].Value)
                            GenFileInfoDT(tgnChild);
                    }
                    GenFileInfoDT(tgnParent);
                }

                // var v = htNewDrawings;

                //var ht = from ht1 in htNewDrawings.AsQueryable()
                //         orderby ht.IsRoot
                //         select ht.*;

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }
        public void GenFileInfoDT(TreeGridNode selectedTreeNode)
        {
            try
            {
                #region "Concate  infromation with ';' for Drawing presented in ARASInnovator"
                String id = (String)selectedTreeNode.Cells["drawingid"].Value;
                String ItemType = (String)selectedTreeNode.Cells["itemtype"].Value;
                String FilePath = (String)selectedTreeNode.Cells["filepath"].Value;
                String MyProjectName = selectedTreeNode.Cells["projectname"].FormattedValue.ToString();
                String MyProjectId = selectedTreeNode.Cells["projectname"].Value.ToString();
                String MyProjectNo = "";
                string PreFix = Convert.ToString(selectedTreeNode.Cells["prefix"].Value);
                string FileTypeID = "";
                string FileStatusID = "";
                string FileType = "";

                System.Data.DataTable dtLayoutInfo = (System.Data.DataTable)selectedTreeNode.Tag;

                if (dtLayoutInfo == null)
                    dtLayoutInfo = new System.Data.DataTable();
                lstdtLayoutInfo.Add(dtLayoutInfo);
                try
                {

                    MyProjectId = Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value));
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];
                    MyProjectNo = Helper.FindValueInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value)), "number");
                }
                catch
                {
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];

                    //c.Value = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "name");
                    //MyProjectId = Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value));
                    MyProjectId = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                    MyProjectNo = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "number", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
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
                    // c.Value = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["cadtype"].Value), "name");
                    //FileTypeID = Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["cadtype"].Value));
                    FileTypeID = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["cadtype"].Value), "name");
                }
                try
                {
                    //FileStatusID = Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["State"].Value));
                    FileStatusID = Convert.ToString(selectedTreeNode.Cells["State"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["State"].Value));
                }
                catch
                {
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["State"];
                    //c.Value = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["State"].Value), "statusname");
                    //FileTypeID = Convert.ToString(selectedTreeNode.Cells["cadtype"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["cadtype"].Value));
                    FileStatusID = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["State"].Value), "statusname");
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
                
                #endregion

                String DI = "";
                for (int i = 0; i < selectedTreeNode.Cells.Count; i++)
                {
                    DI += (Convert.ToString(selectedTreeNode.Cells[i].Value) + ";");
                }
                DI += (CADDescription.Text + ";");
                AllDrawing.Add(DI);

                //if (selectedTreeNode.Cells["drawingid"].Value.ToString() != "")
                //{
                //    String DrawingInformation = id + ";" + ItemType + ";" + FilePath + ";" + selectedTreeNode.Cells["isroot"].Value.ToString() +
                // ";" + "" + ";" + "" +
                // ";" + CADDescription.Text + ";" + selectedTreeNode.Cells["sourceid"].Value.ToString() + ";" + MyProjectName + ";" + MyProjectId + ";"
                // + "" + ";" + "" + ";" + "" + ";"
                // + "" + ";" + selectedTreeNode.Cells["Layouts"].Value.ToString()
                //+ ";" + FileStatusID
                //  + ";" + FileTypeID + ";" + selectedTreeNode.Cells["revision"].Value.ToString() + ";" + MyProjectNo + ";" + selectedTreeNode.Cells["DrawingNumber"].Value.ToString()
                //  + ";" + FileType + ";" + PreFix + ";" + Convert.ToString(selectedTreeNode.Cells["FolderID"].Value) + ";" + Convert.ToString(selectedTreeNode.Cells["FolderPath"].Value);

                //    drawings.Add(DrawingInformation);
                //}




                //String DrawingInformation1;
                //String DrawingNameandNumber = selectedTreeNode.Cells["drawing"].Value.ToString();



                //DrawingInformation1 = DrawingNameandNumber + ";;" + DrawingNameandNumber + ";" +
                //        selectedTreeNode.Cells["filepath"].Value.ToString() + ";" + selectedTreeNode.Cells["sourceid"].Value.ToString() +
                //        ";CAD;" + selectedTreeNode.Cells["isroot"].Value.ToString() + ";" + Convert.ToString(selectedTreeNode.Cells["projectname"].Value) + ";"
                //        + "" + ";" + CADDescription.Text + ";"
                //        + selectedTreeNode.Cells["sourceid"].Value.ToString() + ";" + MyProjectName + ";" + MyProjectId + ";" + ""
                //        + ";" + "" + ";" + "" + ";" + "" + ";"
                //        + selectedTreeNode.Cells["Layouts"].Value.ToString()
                //  + ";" + FileStatusID
                // + ";" + FileTypeID + ";" + selectedTreeNode.Cells["revision"].Value.ToString() + ";" + MyProjectNo + ";" + selectedTreeNode.Cells["DrawingNumber"].Value.ToString()
                // + ";" + FileType + ";" + PreFix + ";" + Convert.ToString(selectedTreeNode.Cells["FolderID"].Value) + ";" + Convert.ToString(selectedTreeNode.Cells["FolderPath"].Value);

                //if (Convert.ToString(selectedTreeNode.Cells["drawingid"].Value).Trim() == String.Empty)
                //    htNewDrawings.Add(selectedTreeNode.Cells["drawing"].Value.ToString(), DrawingInformation1);




            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        private void savetreeGrid_CellBeginEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 13)
                {
                    //CADIntegrationConfiguration objWordConfig = new CADIntegrationConfiguration();
                    TreeGridNode selectedTreeNode = (TreeGridNode)savetreeGrid.Rows[e.RowIndex];
                    //String sg_projectid = Convert.ToString(selectedTreeNode.Cells["projectname"].Value);

                    foreach (TreeGridNode ChildTreeNode in selectedTreeNode.Nodes)
                    {
                        //ChildTreeNode.Cells["projectname"].Value = Convert.ToString(selectedTreeNode.Cells["projectname"].FormattedValue);

                        DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)ChildTreeNode.Cells["projectname"];

                        c.Value = selectedTreeNode.Cells["projectname"].Value;
                    }
                    #region Commented Code
                    //decimal int_projectid =Convert.ToString(selectedTreeNode.Cells["projectname"].Value).Trim()==string.Empty?0: Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value);
                    ////DataGridViewComboBoxCell c2 = (DataGridViewComboBoxCell);
                    //// c2.Value = Helper.FindValueInCMB((System.Data.DataTable)c2.DataSource, "id", sg_projectid, "number"); ; ;
                    ////c2.Value = int_projectid;
                    //selectedTreeNode.Cells["projectid"].Value = selectedTreeNode.Cells["projectname"].Value;
                    ////selectedTreeNode.Cells["projectid"].Value = sg_projectid;
                    //selectedTreeNode.Cells["realtyname"].Value = "";
                    //selectedTreeNode.Cells["realtyid"].Value = "";
                    //for (int rows = 1; rows < savetreeGrid.Rows.Count; rows++)
                    //{
                    //    TreeGridNode ChildTreeNode = (TreeGridNode)savetreeGrid.Rows[rows];
                    //    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)ChildTreeNode.Cells["projectname"];

                    //    c.Value = selectedTreeNode.Cells["projectid"].Value;
                    //    //c.Value = Helper.FindValueInCMB((System.Data.DataTable)c.DataSource, "id", sg_projectid, "name");
                    //    DataGridViewComboBoxCell c1 = (DataGridViewComboBoxCell)ChildTreeNode.Cells["projectid"];
                    //    c1.Value = selectedTreeNode.Cells["projectid"].Value;
                    //    //c1.Value = Helper.FindValueInCMB((System.Data.DataTable)c1.DataSource, "id", sg_projectid, "number"); ;
                    //    //ChildTreeNode.Cells["projectname"].Value = sg_projectid;
                    //    //ChildTreeNode.Cells["projectid"].Value = sg_projectid;
                    //}
                    //System.Data.DataTable dtRealtyNo = new System.Data.DataTable();
                    //dtRealtyNo.Columns.Add("ProjectId", typeof(string));
                    //dtRealtyNo.Columns.Add("RealtyName", typeof(string));
                    //dtRealtyNo.Columns.Add("RealtyNo", typeof(string));
                    //dtRealtyNo = objWordConfig.GetRealtyEntity();
                    //DataView Realty = new DataView(dtRealtyNo, "ProjectId='" + sg_projectid + "'", "RealtyNo", DataViewRowState.CurrentRows);
                    //Realty.AddNew();

                    //RealtyName.DataSource = Realty;
                    //RealtyName.DisplayMember = "RealtyName";
                    //RealtyName.ValueMember = "RealtyNo";
                    //RealtyId.DataSource = Realty;
                    //RealtyId.DisplayMember = "RealtyNo";
                    //RealtyId.ValueMember = "RealtyNo";

                    //savetreeGrid.Refresh();
                    //this.Refresh();
                    #endregion
                }
                if (e.ColumnIndex == 14)
                {
                    #region Commented Code
                    //TreeGridNode selectedTreeNode = (TreeGridNode)savetreeGrid.Rows[e.RowIndex];

                    //String sg_projectid = Convert.ToString(selectedTreeNode.Cells["projectid"].Value);
                    //DataGridViewComboBoxCell c2 = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectid"];

                    //selectedTreeNode.Cells["projectname"].Value = c2.Value;
                    //for (int rows = 1; rows < savetreeGrid.Rows.Count; rows++)
                    //{
                    //    TreeGridNode ChildTreeNode = (TreeGridNode)savetreeGrid.Rows[rows];
                    //    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)ChildTreeNode.Cells["projectname"];

                    //    c.Value = c2.Value;

                    //    //c.Value = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", sg_projectid, "number");
                    //    DataGridViewComboBoxCell c1 = (DataGridViewComboBoxCell)ChildTreeNode.Cells["projectid"];
                    //    c1.Value = c2.Value;
                    //    //c1.Value = Helper.FindValueInCMB((System.Data.DataTable)c1.DataSource, "id", sg_projectid, "number"); ;
                    //    //ChildTreeNode.Cells["projectname"].Value = sg_projectid;
                    //    //ChildTreeNode.Cells["projectid"].Value = sg_projectid;
                    //}
                    #endregion
                }
                if (e.ColumnIndex == 15)
                {
                    //TreeGridNode selectedTreeNode = (TreeGridNode)savetreeGrid.Rows[e.RowIndex];
                    //String sg_projectid = selectedTreeNode.Cells["realtyname"].Value.ToString();
                    //selectedTreeNode.Cells["realtyid"].Value = sg_projectid;
                }
                if (e.ColumnIndex == 16)
                {
                    //TreeGridNode selectedTreeNode = (TreeGridNode)savetreeGrid.Rows[e.RowIndex];
                    //String sg_projectid = selectedTreeNode.Cells["realtyid"].Value.ToString();
                    //selectedTreeNode.Cells["realtyname"].Value = sg_projectid;
                }
                if (e.ColumnIndex == 18)
                {
                    //TreeGridNode selectedTreeNode = (TreeGridNode)savetreeGrid.Rows[e.RowIndex];
                    //for (int rows = 1; rows < savetreeGrid.Rows.Count; rows++)
                    //{
                    //    TreeGridNode ChildTreeNode = (TreeGridNode)savetreeGrid.Rows[rows];
                    //    ChildTreeNode.Cells["version"].Value = selectedTreeNode.Cells["version"].Value;
                    //}
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
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
            if (savetreeGrid.IsCurrentCellDirty)
            {
                savetreeGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
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
                                ShowMessage.InfoMess("You dont have permission to change status of this file.");
                            }
                        }

                    }
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
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
                    string MyProjectId = "", FolderID = "", ProjectName = "", FolderPath = "", ProjectNameNo = "";

                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];
                    try
                    {
                        MyProjectId = Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value));
                        ProjectName = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "name", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "id");
                        ProjectNameNo = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "PNAMENO", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "id");
                    }
                    catch
                    {
                        ProjectNameNo = Convert.ToString(selectedTreeNode.Cells["projectname"].Value);
                        MyProjectId = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                        ProjectName = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "name", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                    }

                    selectedTreeNode.Cells["FolderPath"].Value = "";
                    selectedTreeNode.Cells["FolderID"].Value = "0";
                    ProjectName = ProjectName == string.Empty ? "My Files" : ProjectName;
                    ProjectNameNo = ProjectNameNo == string.Empty ? "My Files" : ProjectNameNo;

                    foreach (TreeGridNode Child in selectedTreeNode.Nodes)
                    {
                        Child.Cells["FolderID"].Value = "0";
                        Child.Cells["FolderPath"].Value = "";
                        Child.Cells["projectname"].Value = ProjectNameNo;

                    }


                    List<clsFolderSearchReasult> objFolderSearchResult = new List<clsFolderSearchReasult>(); ;
                    clsFolderSearchReasult obj = new clsFolderSearchReasult() { id = "-2", name = ProjectName, childFolderSize = 0, companyId = "0" };
                    objFolderSearchResult.Add(obj);
                    StackFolderSearchReasult.Clear();
                    StackFolderSearchReasult.Push(objFolderSearchResult);

                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
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