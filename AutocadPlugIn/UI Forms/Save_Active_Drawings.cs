using System;
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
using CADController.Configuration;
using RedBracketConnector;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Newtonsoft.Json;
using RestSharp;
namespace AutocadPlugIn.UI_Forms
{
    public partial class Save_Active_Drawings : Form
    {
        public ArrayList drawings = new ArrayList();
        public ArrayList drawingsOpen = new ArrayList();
        public Hashtable htNewDrawings = new Hashtable();
        private System.Data.DataTable dtTreeGridData = new System.Data.DataTable();
        RBConnector objRBC = new RBConnector();

        public Save_Active_Drawings()
        {

            InitializeComponent();
            //ICADManager objCadMgr = CADFactory.getCADManager();
            //objCadMgr.SaveActiveDrawing();

        }

        private void Save_Active_Drawings_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

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

                SaveCommand lockStatusCmd = new SaveCommand();
                lockStatusCmd.DrawingInfo = dtTreeGridData;

                SaveController lockStatusCon = new SaveController();
                dtTreeGridData = lockStatusCon.getLockStatus(lockStatusCmd);
                if (lockStatusCon.errorString != null)
                {
                    MessageBox.Show(lockStatusCon.errorString);
                    this.Close();
                    return;
                }

                #region CreateTreeGrid

              
                Helper.FIllCMB(cadtype, objRBC.GetFIleType(), "name", "id", true);
                Helper.FIllCMB(State, objRBC.GetFIleStatus(), "statusname", "id", true);
                Helper.FIllCMB(ProjectName, objRBC.GetProjectDetail(), "PNAMENO", "id", true, "My Files");

         


            
                foreach (DataRow rw in dtTreeGridData.Rows)
                {

                    DataGridViewComboBoxCell ds = new DataGridViewComboBoxCell();
                    CADIntegrationConfiguration objWordConfig = new CADIntegrationConfiguration(); 


                    if (rw["drawingid"].ToString() != "")
                    { 
                        ProjectName.ReadOnly = true;
                    }
                    else
                    {

                        this.savetreeGrid.Columns["version"].Visible = false;
                    }
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
                            dr["Version"] = obj.versionNo;
                            dr["Description"] = obj.description;
                            dr["IsFile"] = "0";
                            dr["TypeID"] = obj.typeId;
                            dr["StatusID"] = obj.statusId;
                            dr["ACLayoutID"] = obj.layoutId == null ? string.Empty : obj.layoutId;
                            dr["LayoutName1"] = obj.name;
                            dtLayoutInfo.Rows.Add(dr);
                        }
                    }


                    
                    #endregion
                    if (counter == 0 || rw["isroot"].ToString() == "1")
                    {
                        ArrayList cmbData = new ArrayList();
                        cmbData.Add(rw["revision"].ToString());
                        cmbData.Add("Next");
                        ds.DataSource = cmbData;

                        node = savetreeGrid.Nodes.Add(false, rw["drawingName"].ToString(), rw["drawingnumber"].ToString(), rw["Classification"].ToString(), rw["revision"].ToString(),
                            rw["drawingstate"].ToString(), rw["drawingid"].ToString(), rw["filepath"].ToString(), rw["type"].ToString(), rw["lockstatus"].ToString(), rw["sourceid"].ToString()
                            , rw["isroot"].ToString(), rw["Layouts"], rw["projectname"]
                            , rw["candelete"]
                            , rw["isowner"]
                            , rw["hasviewpermission"]
                            , rw["isactfilelatest"]
                            , rw["iseditable"]
                            , rw["caneditstatus"]
                            , rw["hasstatusclosed"]
                            , rw["isletest"]
                            , rw["prefix"]
                            );
                      
                        node.Cells["version"].Value = true;
                        if (rw["drawingid"].ToString() == "")
                        { 
                            node.Cells["projectname"].Value = ""; 
                            node.Cells["version"].Value = true;
                            node.Cells["targetrevision"].ReadOnly = true; 
                            node.Cells["projectname"].ReadOnly = false;
                            node.Cells["drawingnumber"].Value = "AutoFill";
                        }
                        else
                        { 
                            node.Cells["projectname"].ReadOnly = true; 
                            node.Cells["State"].ReadOnly = !Convert.ToBoolean(rw["caneditstatus"]); 
                        }
                        node.Cells["targetrevision"] = ds;
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
                        ArrayList cmbData1 = new ArrayList();
                        cmbData1.Add(rw["revision"].ToString());
                        cmbData1.Add("Next");
                     TreeGridNode     node1 = node.Nodes.Add(false, rw["drawingName"].ToString(), rw["drawingnumber"].ToString(), rw["Classification"].ToString(),
                            rw["revision"].ToString(), rw["drawingstate"].ToString(), rw["drawingid"].ToString(), rw["filepath"].ToString(), rw["type"].ToString(),
                            rw["lockstatus"].ToString(), rw["sourceid"].ToString(), rw["isroot"].ToString(), rw["Layouts"], rw["projectname"]
                                , rw["candelete"]
                            , rw["isowner"]
                            , rw["hasviewpermission"]
                            , rw["isactfilelatest"]
                            , rw["iseditable"]
                            , rw["caneditstatus"]
                            , rw["hasstatusclosed"]
                            , rw["isletest"]
                            , rw["prefix"]);
                        node1.Cells["version"].Value = true;
                        node1.Cells["version"].ReadOnly = true;
                        ds.DataSource = cmbData1;
                        if (rw["drawingid"].ToString() == "")
                        { 
                            node1.Cells["projectname"].Value = "";
                            node.Cells["version"].Value = true; 
                            node1.Cells["projectname"].ReadOnly = true; 
                            node1.Cells["targetrevision"].ReadOnly = true;
                            node1.Cells["drawingnumber"].Value = "AutoFill";
                        }
                        else
                        {  
                            node.Cells["State"].ReadOnly = !Convert.ToBoolean(rw["caneditstatus"]);
                        }
                        node1.Cells["targetrevision"] = ds;
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
            catch(Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
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
                    }
                }

                //To check whether any file with same name exist or not
                #region Duplication Check
                bool IsDuplicationCheck = false;
                if(IsDuplicationCheck)
                {
                    foreach (TreeGridNode currentTreeGrdiNode in selectedTreeGridNodes)
                    {
                        System.Data.DataTable dtLayoutInfo = (System.Data.DataTable)currentTreeGrdiNode.Tag;
                        //File name and layout out name length validation
                        for (int i = 0; i < dtLayoutInfo.Rows.Count; i++)
                        {
                            string FN = Path.GetFileName(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim());
                            string LN = Convert.ToString(dtLayoutInfo.Rows[i]["FileLayoutName"]);
                            if (FN.Length < Helper.FileLayoutNameLength)
                            {
                                
                            }
                            else
                            {
                                ShowMessage.ErrorMess(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim() + "\n File name length exceeds maximum limit.");
                                this.Cursor = Cursors.Default;
                                return;
                            }
                            if (LN.Length < Helper.FileLayoutNameLength)
                            {

                            }
                            else
                            {
                                ShowMessage.ErrorMess(LN + "\n Layout name length exceeds maximum limit. of file "+Environment.NewLine + FN);
                                this.Cursor = Cursors.Default;
                                return;
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
                                string FN = Path.GetFileName(Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value).Trim());
                                
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
                                string FN = Path.GetFileName(Convert.ToString(ChildNode.Cells["filepath"].Value).Trim());
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
                                    string FN = Path.GetFileName(Convert.ToString(ChildNode.Cells["filepath"].Value).Trim());
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
                if (selectedTreeGridNodes.Count < 1)
                {
                    MessageBox.Show("Please select at least one file to save.");
                    this.Cursor = Cursors.Default;
                    return;
                }

                //string checkoutPath = Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath").ToString();
                SaveController objController = new SaveController();
                AutoCADManager objMgr = new AutoCADManager();
                SaveCommand objCmd = new SaveCommand();

                htNewDrawings.Clear();
                drawings.Clear();
                GenFileInfo();
                //return;
                ICollection keys = htNewDrawings.Keys;
                IEnumerator key = keys.GetEnumerator();
                while (key.MoveNext())
                {
                    objCmd.NewDrawings.Add(htNewDrawings[key.Current.ToString()].ToString());
                }

                foreach (String str in drawings)
                {
                    objCmd.Drawings.Add(str);
                }

                // to ask user want to delete file after save or not.
                if (ShowMessage.InfoYNMess("Would you like to delete local file/files" + Environment.NewLine + " after saving it to RedBracket ?") == DialogResult.Yes)
                {
                    Is_Delete = true;
                }


                progressBar1.Value = progressBar1.Minimum = 0;
                progressBar1.Maximum = 5;
                progressBar1.Visible = true;
                // to iterate selected file
                foreach (TreeGridNode currentTreeGrdiNode in selectedTreeGridNodes)
                {

                    if (Convert.ToString(currentTreeGrdiNode.Cells["isEditable"].Value).Length > 3)
                    {
                        if (!Convert.ToBoolean(Convert.ToString(currentTreeGrdiNode.Cells["isEditable"].Value).ToLower()))
                        {
                            progressBar1.Visible = false;
                            progressBar1.Value = progressBar1.Minimum = 0;
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

                        //Update Xref name to wihout prefix 
                        objMgr.UpdateExRefPathInfo1(FilePath);


                        progressBar1.Increment(1); progressBar1.Refresh(); this.Refresh();
                        // save in RB
                        Is_Save = objController.ExecuteSave(objCmd);
                        progressBar1.Increment(1); progressBar1.Refresh(); this.Refresh();

                        if (Is_Save)
                        {
                            //Save layout info
                            #region Save layout info
                            try
                            {
                                System.Data.DataTable dtLayoutInfo = (System.Data.DataTable)currentTreeGrdiNode.Tag;
                                string MyProjectId = "";

                                if (dtLayoutInfo != null)
                                {
                                    try
                                    {

                                        MyProjectId = Convert.ToString(currentTreeGrdiNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(currentTreeGrdiNode.Cells["projectname"].Value));
                                    }
                                    catch
                                    {
                                        DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)currentTreeGrdiNode.Cells["projectname"];

                                        //c.Value = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "name");
                                        //MyProjectId = Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value));
                                        MyProjectId = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(currentTreeGrdiNode.Cells["projectname"].Value), "PNAMENO");
                                    }

                                    MyProjectId = MyProjectId == "0" || MyProjectId == "-1" ? string.Empty : MyProjectId;

                                    objRBC.SaveUpdateLayoutInfo(dtLayoutInfo, MyProjectId, Convert.ToString(objController.dtDrawingProperty.Select("isroot=True")[0]["DrawingId"]));

                                    foreach(TreeGridNode Child in currentTreeGrdiNode.Nodes)
                                    {
                                        dtLayoutInfo = (System.Data.DataTable)Child.Tag;

                                        objRBC.SaveUpdateLayoutInfo(dtLayoutInfo, MyProjectId, Convert.ToString(objController.dtDrawingProperty.Select("DrawingName='"+ Convert.ToString(Child.Cells["drawing"].Value) + "'")[0]["DrawingId"]));

                                    }

                                    //objMgr.SetLayoutOwnerID(dtLayoutInfo, Convert.ToString(objController.dtDrawingProperty.Select("isroot=True")[0]["filepath"]));
                                }

                            }
                            catch
                            {

                            }
                            #endregion

                            
                           

                            #region Update File Properties
                            bool IsdownloadNewFile = false;

                            if (IsdownloadNewFile)
                            {
                                #region Download and open updated file from RB
                                try
                                {
                                    if (!Is_Delete)
                                    {
                                        string MyProjectId = "", MyProjectNo = ""; string ProjectName = "";
                                        try
                                        {

                                            MyProjectId = Convert.ToString(currentTreeGrdiNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(currentTreeGrdiNode.Cells["projectname"].Value));
                                            DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)currentTreeGrdiNode.Cells["projectname"];
                                            MyProjectNo = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "number", Convert.ToString(currentTreeGrdiNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(currentTreeGrdiNode.Cells["projectname"].Value), "id");
                                            ProjectName = Convert.ToString(currentTreeGrdiNode.Cells["projectname"].FormattedValue);
                                        }
                                        catch
                                        {
                                            DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)currentTreeGrdiNode.Cells["projectname"];

                                            MyProjectId = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(currentTreeGrdiNode.Cells["projectname"].Value), "PNAMENO");
                                            MyProjectNo = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "number", Convert.ToString(currentTreeGrdiNode.Cells["projectname"].Value), "PNAMENO");
                                            ProjectName = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "name", Convert.ToString(currentTreeGrdiNode.Cells["projectname"].Value), "PNAMENO");
                                        }
                                        string checkoutPath = RedBracketConnector.Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath").ToString();

                                        if (ProjectName.Trim().Length == 0)
                                        {
                                            ProjectName = "MyFiles";
                                        }
                                        checkoutPath = Path.Combine(checkoutPath, ProjectName);
                                        if (!Directory.Exists(checkoutPath))
                                        {
                                            Directory.CreateDirectory(checkoutPath);
                                        }


                                        if (Convert.ToString(currentTreeGrdiNode.Cells["drawingID"].Value).Length > 0)
                                        {
                                            DownloadOpenDocument(Convert.ToString(currentTreeGrdiNode.Cells["drawingID"].Value), checkoutPath);
                                            File.Delete(objCmd.FilePath);
                                            foreach (TreeGridNode ChildNode in currentTreeGrdiNode.Nodes)
                                            {
                                                if ((bool)ChildNode.Cells[0].FormattedValue)
                                                {
                                                    File.Delete(Convert.ToString(ChildNode.Cells["filepath"].Value));
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DownloadOpenDocument(Convert.ToString(objController.dtDrawingProperty.Select("isroot=True")[0]["DrawingId"]), checkoutPath);
                                        }
                                        try
                                        {
                                            objMgr.SaveActiveDrawing(false);
                                        }
                                        catch { }
                                        progressBar1.Increment(1); progressBar1.Refresh(); this.Refresh();
                                    }
                                }
                                catch (Exception E)
                                {
                                    ShowMessage.ErrorMess(E.Message);
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                                #endregion

                                #region Deleting XrefFile with Old Name
                                try
                                {
                                    foreach (TreeGridNode ChildNode in currentTreeGrdiNode.Nodes)
                                    {
                                        if ((bool)ChildNode.Cells[0].FormattedValue)
                                        {
                                            string Dir = Path.GetDirectoryName(Convert.ToString(ChildNode.Cells["filepath"].Value));

                                            string MyProjectNo = ""; string ProjectName = "";
                                            try
                                            {
                                                string MyProjectId = Convert.ToString(currentTreeGrdiNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(currentTreeGrdiNode.Cells["projectname"].Value));
                                                DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)currentTreeGrdiNode.Cells["projectname"];
                                                MyProjectNo = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "number", Convert.ToString(ChildNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(ChildNode.Cells["projectname"].Value), "id");
                                                ProjectName = Convert.ToString(ChildNode.Cells["projectname"].FormattedValue);
                                            }
                                            catch
                                            {
                                                DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)currentTreeGrdiNode.Cells["projectname"];

                                                MyProjectNo = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "number", Convert.ToString(ChildNode.Cells["projectname"].Value), "PNAMENO");
                                                ProjectName = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "name", Convert.ToString(ChildNode.Cells["projectname"].Value), "PNAMENO");
                                            }
                                            string checkoutPath = RedBracketConnector.Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath").ToString();

                                            if (ProjectName.Trim().Length == 0)
                                            {
                                                ProjectName = "MyFiles";
                                            }
                                            checkoutPath = Path.Combine(checkoutPath, ProjectName);


                                            if (Dir == checkoutPath)
                                                File.Delete(Convert.ToString(ChildNode.Cells["filepath"].Value));
                                        }
                                    }
                                }
                                catch { }
                                #endregion
                            }
                            else
                            {
                                // Update document info into document for future referance 
                                if (objController.dtDrawingProperty.Rows.Count > 0)
                                {
                                    objMgr.UpdateExRefInfo(objCmd.FilePath, objController.dtDrawingProperty);
                                }

                            }
                            #endregion

                            #region  Renaming Parent and XRef Files

                            //update Xref name to NewXref Name with PreFix
                            objMgr.UpdateExRefPathInfo(objCmd.FilePath);

                            #endregion
                        }


                    }
                   





                    progressBar1.Increment(1); progressBar1.Refresh(); this.Refresh();
                  

                    // To delete file
                    if (Is_Delete && File.Exists(objCmd.FilePath))
                    {
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
                        progressBar1.Increment(1); progressBar1.Refresh(); this.Refresh();
                    }
                }
                this.Cursor = Cursors.Default;
                if (Is_Save)
                {
                    progressBar1.Value = progressBar1.Maximum; progressBar1.Refresh(); this.Refresh();
                    ShowMessage.InfoMess("Save operation successfully completed.");
                    this.Close();
                    return;
                }
                else
                {
                    progressBar1.Value = progressBar1.Maximum; progressBar1.Refresh(); this.Refresh();
                    progressBar1.Visible = false;
                    ShowMessage.ErrorMess("Save operation unsuccessfully completed.");
                    return;
                }
                #region Commented Code
                //if (htNewDrawings.Count > 0 || drawings.Count > 0)
                //{
                //    ICADManager objMgr = new AutoCADManager();
                //    SaveCommand objCmd = new SaveCommand();

                //    ICollection keys = htNewDrawings.Keys;
                //    IEnumerator key = keys.GetEnumerator();
                //    while (key.MoveNext())
                //    {
                //        objCmd.NewDrawings.Add(htNewDrawings[key.Current.ToString()].ToString());
                //    }
                //    foreach (String str in drawings)
                //    {
                //        objCmd.Drawings.Add(str);
                //    }
                //    objController.Execute(objCmd);
                //    if (objController.errorString != null)
                //    {
                //        MessageBox.Show(objController.errorString);
                //        this.Cursor = Cursors.Default;
                //        return;
                //    }
                //    Hashtable htDrawingProperty = new Hashtable();
                //    foreach (DataRow row in objController.dtDrawingProperty.Rows)
                //    {      /*int i = 0;
                //            foreach (DataColumn column in objController.dtDrawingProperty.Columns)
                //            {
                //                MessageBox.Show(column.ColumnName.ToString() + "---------->" + row[i].ToString());
                //                i++;
                //            }*/

                //        htDrawingProperty.Add("DrawingId", row["DrawingId"]);
                //        htDrawingProperty.Add("DrawingName", row["DrawingName"]);
                //        htDrawingProperty.Add("Classification", row["Classification"]);
                //        htDrawingProperty.Add("DrawingNumber", row["DrawingNumber"]);
                //        htDrawingProperty.Add("DrawingState", row["DrawingState"]);
                //        htDrawingProperty.Add("Revision", row["Revision"]);
                //        htDrawingProperty.Add("Generation", row["Generation"]);
                //        htDrawingProperty.Add("Type", row["Type"]);
                //        htDrawingProperty.Add("ProjectName", row["ProjectName"]);
                //        htDrawingProperty.Add("ProjectId", row["ProjectId"]);
                //        htDrawingProperty.Add("CreatedOn", row["createdon"]);
                //        htDrawingProperty.Add("CreatedBy", row["createdby"]);
                //        htDrawingProperty.Add("ModifiedOn", row["modifiedon"]);
                //        htDrawingProperty.Add("ModifiedBy", row["modifiedby"]);

                //        if ((bool)row["isroot"])
                //        {
                //            objMgr.OpenActiveDocument(row["filepath"].ToString(), "updtMainDrawing", htDrawingProperty);
                //        }
                //        else
                //        {
                //            objMgr.OpenActiveDocument(row["filepath"].ToString(), "updtXRDrawing", htDrawingProperty);
                //        }
                //        htDrawingProperty.Clear();
                //    }
                //    bool deletefile = false;
                //    if (MessageBox.Show("Would you like to Delete Local file/files after Saving it into Aras?", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                //        deletefile = true;
                //    foreach (DataRow row in objController.dtDrawingProperty.Rows)
                //    {
                //        if ((bool)row["isroot"])
                //        {
                //            objMgr.CloseActiveDocument(row["filepath"].ToString());
                //            if (deletefile)
                //                objMgr.DeleteActiveDocument(row["filepath"].ToString());
                //        }
                //        else
                //        {
                //            if (deletefile)
                //                objMgr.DeleteActiveDocument(row["filepath"].ToString());
                //        }
                //    }
                //    MessageBox.Show("Save operation succesfully completed.");
                //    this.Cursor = Cursors.Default;
                //    this.Close();
                //}
                #endregion
            }
            catch (Exception ex)
            {
                progressBar1.Visible = false;
                ShowMessage.ErrorMess(ex.Message);
                this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
        }
        public void DownloadOpenDocument1(string fileId, string checkoutPath, bool IsParent = false)
        {
            try
            {
                RBConnector objRBC = new RBConnector();
                Hashtable DrawingProperty = new Hashtable();

                byte[] RawBytes = null;
                ResultSearchCriteria Drawing = objRBC.GetSingleFileInfo(fileId, ref RawBytes);

                if (Drawing != null)
                {
                    DrawingProperty.Add("DrawingId", Drawing.id);
                    DrawingProperty.Add("DrawingName", Drawing.name);
                    DrawingProperty.Add("Classification", "");
                    DrawingProperty.Add("FileTypeID", Drawing.type == null ? string.Empty : Drawing.type.name == null ? string.Empty : Drawing.type.name);
                    DrawingProperty.Add("DrawingNumber", Drawing.fileNo);

                    DrawingProperty.Add("DrawingState", Drawing.status == null ? string.Empty : Drawing.status.statusname == null ? string.Empty : Drawing.status.statusname);
                    DrawingProperty.Add("Revision", Drawing.versionno);
                    DrawingProperty.Add("LockStatus", Drawing.filelock);
                    DrawingProperty.Add("Generation", "123");
                    DrawingProperty.Add("Type", Drawing.coreType.id);
                    #region LayoutInfo
                    String LayoutInfos = "";
                    LayoutInfos = Helper.GetLayoutInfo(Drawing.fileLayout);
                    #endregion
                    //DrawingProperty.Add("ProjectName", Drawing.projectname );
                    if (Drawing.projectname != null)
                    {
                        if (Drawing.projectname.Trim().Length == 0)
                        {
                            DrawingProperty.Add("ProjectName", "My Files");
                        }
                        else
                        {
                            DrawingProperty.Add("ProjectName", Drawing.projectname + " (" + Drawing.projectNumber + ")");
                        }
                    }
                    else
                    {
                        DrawingProperty.Add("ProjectName", "My Files");
                    }
                    DrawingProperty.Add("ProjectId", Drawing.projectinfo);
                    DrawingProperty.Add("CreatedOn", Drawing.updatedon);
                    DrawingProperty.Add("CreatedBy", Drawing.createdby);
                    DrawingProperty.Add("ModifiedOn", Drawing.updatedon);
                    DrawingProperty.Add("ModifiedBy", Drawing.updatedby);

                    DrawingProperty.Add("canDelete", Drawing.canDelete);
                    DrawingProperty.Add("isowner", Drawing.isowner);
                    DrawingProperty.Add("hasViewPermission", Drawing.hasViewPermission);
                    DrawingProperty.Add("isActFileLatest", Drawing.isActFileLatest);

                    DrawingProperty.Add("isEditable", Drawing.isEditable);
                    DrawingProperty.Add("canEditStatus", Drawing.canEditStatus);
                    DrawingProperty.Add("hasStatusClosed", Drawing.hasStatusClosed);
                    DrawingProperty.Add("isletest", Drawing.isletest);
                    DrawingProperty.Add("LayoutInfo", LayoutInfos);
                    DrawingProperty.Add("projectno", Drawing.projectNumber == null ? string.Empty : Drawing.projectNumber);

                    string ProjectNo = Drawing.projectNumber == null ? string.Empty : Drawing.projectNumber;


                    string FileType = Drawing.type == null ? string.Empty : Drawing.type.name == null ? string.Empty : Drawing.type.name;









                    string PreFix = "";
                    if (ProjectNo.Trim().Length > 0)
                    {
                        PreFix = ProjectNo + "-";
                    }
                    PreFix = PreFix + Drawing.fileNo + "-";


                    PreFix += Convert.ToString(FileType) == string.Empty ? string.Empty : Convert.ToString(FileType) + "-";

                    PreFix += Convert.ToString(Drawing.versionno) == string.Empty ? string.Empty : Convert.ToString(Drawing.versionno) + "#";


                    DrawingProperty.Add("prefix", PreFix);
                    //DrawingProperty.Add("isroot", true);
                    //DrawingProperty.Add("sourceid","");
                    //DrawingProperty.Add("Layouts","");

                    string filePathName = Path.Combine(checkoutPath, Drawing.name);
                    if (IsParent)
                    {
                        filePathName = Path.Combine(checkoutPath, PreFix + Drawing.name);
                    }

                    using (var binaryWriter = new BinaryWriter(File.Open(filePathName, FileMode.OpenOrCreate)))
                    {
                        binaryWriter.Write(RawBytes);
                    }
                    AutoCADManager objMgr = new AutoCADManager();
                    if (IsParent)
                    {

                        objMgr.UpdateExRefPathInfo(filePathName);
                        objMgr.OpenActiveDocument(filePathName, "View", DrawingProperty);
                    }
                    else
                    {
                        objMgr.SetAttributesXrefFiles(DrawingProperty, filePathName);
                        objMgr.UpdateLayoutAttributeArefFile(DrawingProperty, filePathName);
                    }

                }
                else
                {
                    ShowMessage.ErrorMess("Some error occures while retrieving file.");
                }
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

        }

        public void DownloadOpenDocument(string fileId, string checkoutPath)
        {
            try
            {
                RBConnector objRBC = new RBConnector();
                var XrefFIle = objRBC.GetXrefFIleInfo(fileId);

                foreach (ResultSearchCriteria obj in XrefFIle)
                {
                    DownloadOpenDocument1(obj.id, checkoutPath, false);
                }

                DownloadOpenDocument1(fileId, checkoutPath, true);

            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

        }
        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void savetreeGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // return;
            if (e.ColumnIndex == 0)
            {
                TreeGridNode selectedTreeNode = (TreeGridNode)savetreeGrid.Rows[e.RowIndex];
                //bool flag = false;
                //CADDescription.ReadOnly = false;
                //for (int GridRows = 0; GridRows < savetreeGrid.Rows.Count; GridRows++)
                //{
                //    TreeGridNode MyTreeNode = (TreeGridNode)savetreeGrid.Rows[GridRows];
                //    if ((bool)MyTreeNode.Cells["check"].EditedFormattedValue)
                //    { flag = true; break; }
                //}
                //if (flag)
                //    CADDescription.ReadOnly = true;

                //#region "Concate  infromation with ';' for Drawing presented in ARASInnovator"
                //String id = (String)selectedTreeNode.Cells["drawingid"].Value;
                //String ItemType = (String)selectedTreeNode.Cells["itemtype"].Value;
                //String FilePath = (String)selectedTreeNode.Cells["filepath"].Value;
                //String MyProjectName = selectedTreeNode.Cells["projectname"].FormattedValue.ToString();
                //String MyProjectId = selectedTreeNode.Cells["projectid"].FormattedValue.ToString();
                //Hashtable DrawingData = new Hashtable();
                ////ArasConnector.ArasConnector DrawingDetail = new ArasConnector.ArasConnector();
                //RBConnector DrawingDetail = new RBConnector();
                //DrawingData = DrawingDetail.GetDrawingDetail(id);
                //if (DrawingData.Count < 1)
                //    DrawingData = DrawingDetail.GetDrawingDetail(selectedTreeNode.Cells["drawingid"].Value.ToString());

                //if (selectedTreeNode.Cells["targetrevision"].Value == null)
                //{
                //    selectedTreeNode.Cells["targetrevision"].Value = "";
                //}
                //String DrawingInformation = id + ";" + ItemType + ";" + FilePath + ";" + selectedTreeNode.Cells["isroot"].Value.ToString() + ";" + selectedTreeNode.Cells["targetrevision"].Value.ToString() + ";" + selectedTreeNode.Cells["version"].Value.ToString() + ";" + CADDescription.Text + ";" + selectedTreeNode.Cells["sourceid"].Value.ToString() + ";" + MyProjectName + ";" + MyProjectId + ";" + DrawingData["createdon"].ToString() + ";" + DrawingData["createdby"].ToString() + ";" + DrawingData["modifiedon"].ToString() + ";" + DrawingData["modifiedby"].ToString() + ";" + selectedTreeNode.Cells["Layouts"].Value.ToString();

                //#endregion

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

                //    #region "Drawing is present in redbracket"
                //    if (selectedTreeNode.Cells["drawingid"].Value.ToString() != "")
                //    {
                //        if ((bool)selectedTreeNode.Cells["check"].EditedFormattedValue)
                //        {
                //            selectedTreeNode.Cells["targetrevision"].ReadOnly = true;
                //            drawings.Add(DrawingInformation);
                //            drawingsOpen.Add(DrawingInformation);
                //            savetreeGrid.Columns[17].ReadOnly = true;
                //        }
                //        else
                //        {
                //            selectedTreeNode.Cells["targetrevision"].ReadOnly = false;
                //            drawings.Remove(DrawingInformation);
                //            drawingsOpen.Remove(DrawingInformation);
                //            savetreeGrid.Columns[17].ReadOnly = false;
                //        }
                //    }
                //    #endregion

                //    #region "Add Drawing to redbracket"
                //    //else
                //    //{
                if ((bool)selectedTreeNode.Cells["check"].EditedFormattedValue)
                {
                    //selectedTreeNode.Cells["projectname"].ReadOnly = true;
                    //selectedTreeNode.Cells["realtyname"].ReadOnly = true;
                    //selectedTreeNode.Cells["projectid"].ReadOnly = true;
                    //selectedTreeNode.Cells["realtyid"].ReadOnly = true;

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
            if (e.ColumnIndex == 5)
            {
                //TreeGridNode selectedTreeNode = (TreeGridNode)savetreeGrid.Rows[e.RowIndex];
                //if ((bool)selectedTreeNode.Cells["check"].EditedFormattedValue)
                //{
                //    if (!Convert.ToBoolean(selectedTreeNode.Cells["canEditStatus"].Value))
                //    {
                //        ShowMessage.InfoMess("You dont have permission to change status of this file.");
                //    }
                //}
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

                try
                {

                    MyProjectId = Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value));
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];
                    MyProjectNo = Helper.FindValueInCMB((System.Data.DataTable)c.DataSource, "number", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "id");
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
                //DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];
                //MyProjectId =Convert.ToString(c.Value);
                Hashtable DrawingData = new Hashtable();
                //ArasConnector.ArasConnector DrawingDetail = new ArasConnector.ArasConnector();
                RBConnector DrawingDetail = new RBConnector();
                DrawingData = DrawingDetail.GetDrawingDetail(id);
                if (DrawingData.Count < 1)
                    DrawingData = DrawingDetail.GetDrawingDetail(selectedTreeNode.Cells["drawingid"].Value.ToString());

                if (selectedTreeNode.Cells["targetrevision"].Value == null)
                {
                    selectedTreeNode.Cells["targetrevision"].Value = "";
                }
                String DrawingInformation = id + ";" + ItemType + ";" + FilePath + ";" + selectedTreeNode.Cells["isroot"].Value.ToString() +
                    ";" + selectedTreeNode.Cells["targetrevision"].Value.ToString() + ";" + selectedTreeNode.Cells["version"].Value.ToString() +
                    ";" + CADDescription.Text + ";" + selectedTreeNode.Cells["sourceid"].Value.ToString() + ";" + MyProjectName + ";" + MyProjectId + ";"
                    + DrawingData["createdon"].ToString() + ";" + DrawingData["createdby"].ToString() + ";" + DrawingData["modifiedon"].ToString() + ";"
                    + DrawingData["modifiedby"].ToString() + ";" + selectedTreeNode.Cells["Layouts"].Value.ToString()
                   + ";" + FileStatusID
                     + ";" + FileTypeID + ";" + selectedTreeNode.Cells["revision"].Value.ToString() + ";" + MyProjectNo + ";" + selectedTreeNode.Cells["DrawingNumber"].Value.ToString()
                     + ";" + FileType + ";" + PreFix;


                #endregion

                #region "Drawing is present in redbracket"
                if (selectedTreeNode.Cells["drawingid"].Value.ToString() != "")
                {
                    if ((bool)selectedTreeNode.Cells["check"].EditedFormattedValue)
                    {
                        selectedTreeNode.Cells["targetrevision"].ReadOnly = true;
                        drawings.Add(DrawingInformation);
                        drawingsOpen.Add(DrawingInformation);
                        savetreeGrid.Columns[17].ReadOnly = true;
                    }
                    else
                    {
                        selectedTreeNode.Cells["targetrevision"].ReadOnly = false;
                        drawings.Remove(DrawingInformation);
                        drawingsOpen.Remove(DrawingInformation);
                        savetreeGrid.Columns[17].ReadOnly = false;
                    }
                }
                #endregion

                #region "Add Drawing to redbracket"
                //else
                //{
                if ((bool)selectedTreeNode.Cells["check"].EditedFormattedValue)
                {

                    String[] strarry = new String[5];
                    String DrawingInformation1;
                    String DrawingNameandNumber = selectedTreeNode.Cells["drawing"].Value.ToString();
                    int index = DrawingNameandNumber.IndexOf('.');
                    int length = DrawingNameandNumber.Length;
                    if (index > 0)
                        DrawingNameandNumber = DrawingNameandNumber.Remove(index);
                    DrawingInformation1 = DrawingNameandNumber + ";;" + DrawingNameandNumber + ";" +
                            selectedTreeNode.Cells["filepath"].Value.ToString() + ";" + selectedTreeNode.Cells["sourceid"].Value.ToString() +
                            ";CAD;" + selectedTreeNode.Cells["isroot"].Value.ToString() + ";" + Convert.ToString(selectedTreeNode.Cells["projectname"].Value) + ";"
                            + ""/*Realityname */ + ";" + CADDescription.Text + ";"
                            + selectedTreeNode.Cells["sourceid"].Value.ToString() + ";" + MyProjectName + ";" + MyProjectId + ";" + DrawingData["createdon"].ToString()
                            + ";" + DrawingData["createdby"].ToString() + ";" + DrawingData["modifiedon"].ToString() + ";" + DrawingData["modifiedby"].ToString() + ";"
                            + selectedTreeNode.Cells["Layouts"].Value.ToString()
                      + ";" + FileStatusID
                     + ";" + FileTypeID + ";" + selectedTreeNode.Cells["revision"].Value.ToString() + ";" + MyProjectNo + ";" + selectedTreeNode.Cells["DrawingNumber"].Value.ToString()
                     + ";" + FileType + ";" + PreFix;

                    if (Convert.ToString(selectedTreeNode.Cells["drawingid"].Value).Trim() == String.Empty)
                        htNewDrawings.Add(selectedTreeNode.Cells["drawing"].Value.ToString(), DrawingInformation1);

                    //selectedTreeNode.Cells["drawingnumber"].Value = DrawingNameandNumber;
                    //selectedTreeNode.Cells["drawing"].Value = DrawingNameandNumber;
                }
                else
                {

                }

                #endregion
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