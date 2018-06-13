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

            #region filetype
            Helper.FIllCMB(cadtype, objRBC.GetFIleType(), "name", "id", true);

            #endregion filetype
            #region filestatus
            //RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), "/AutocadFiles/fetchFileStatus", DataFormat.Json, null, true, null);
            //var statusInfoList = JsonConvert.DeserializeObject<List<ResultStatusData>>(restResponse.Content);
            //State.DataSource = statusInfoList;
            //State.DisplayMember = "statusname";
            //State.ValueMember = "statusname";
            // objRBC.GetFIleStatus(CDState,   "name", "name", true);
            Helper.FIllCMB(State, objRBC.GetFIleStatus(), "statusname", "id", true);
            #endregion filestatus

            Helper.FIllCMB(ProjectName, objRBC.GetProjectDetail(), "PNAMENO", "id", true,"My Files");

            //ProjectName.DataSource = dtProjectNo;
            //ProjectName.DisplayMember = "ProjectName";
            //ProjectName.ValueMember = "ProjectId";
            //  Helper.FIllCMB(ProjectId, objRBC.GetProjectDetail(), "number", "id", true);


            //ProjectId.DataSource = dtProjectNo;
            //ProjectId.DisplayMember = "ProjectNo";
            //ProjectId.ValueMember = "ProjectId";
            foreach (DataRow rw in dtTreeGridData.Rows)
            {

                DataGridViewComboBoxCell ds = new DataGridViewComboBoxCell();
                CADIntegrationConfiguration objWordConfig = new CADIntegrationConfiguration();
                //System.Data.DataTable dtProjectNo = new System.Data.DataTable();
                // dtProjectNo.Columns.Add("ProjectId", typeof(string));
                // dtProjectNo.Columns.Add("ProjectName", typeof(string));
                // dtProjectNo.Columns.Add("ProjectNo", typeof(string));
                // dtProjectNo = objWordConfig.GetProject();
                // dtProjectNo.Rows.Add("", "Non", "Non");


                if (rw["drawingid"].ToString() != "")
                {
                    //this.savetreeGrid.Columns["realtyid"].Visible = false;
                    //this.savetreeGrid.Columns["realtyname"].Visible = false;
                    //ProjectId.ReadOnly = true;
                    ProjectName.ReadOnly = true;
                }
                else
                {

                    this.savetreeGrid.Columns["version"].Visible = false;
                }

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


                    //DataGridViewComboBoxCell C=(DataGridViewComboBoxCell) node.Cells["cadtype"]   ;
                    node.Cells["version"].Value = true;
                    if (rw["drawingid"].ToString() == "")
                    {
                        //RealtyName.ReadOnly = false;
                        //RealtyId.ReadOnly = false;
                        //node.Cells["projectid"].Value = "";
                        node.Cells["projectname"].Value = "";
                        //node.Cells["realtyid"].Value = "";
                        //node.Cells["realtyname"].Value = "";
                        node.Cells["version"].Value = true;
                        node.Cells["targetrevision"].ReadOnly = true;
                        //node.Cells["projectid"].ReadOnly = false;
                        node.Cells["projectname"].ReadOnly = false;
                        node.Cells["drawingnumber"].Value = "AutoFill";
                    }
                    else
                    {
                        //foreach (DataRow rr in dtProjectNo.Rows)
                        //{
                        //    if (rr["ProjectNo"].ToString() == rw["projectid"].ToString())
                        //    {
                        //        node.Cells["projectname"].Value = rr["ProjectId"].ToString();
                        //        node.Cells["projectid"].Value = rr["ProjectId"].ToString();
                        //        break;
                        //    }
                        //}
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
                }
                else
                {
                    ArrayList cmbData1 = new ArrayList();
                    cmbData1.Add(rw["revision"].ToString());
                    cmbData1.Add("Next");
                    TreeGridNode node1 = node.Nodes.Add(false, rw["drawingName"].ToString(), rw["drawingnumber"].ToString(), rw["Classification"].ToString(),
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
                        //RealtyName.ReadOnly = false;
                        ////RealtyId.ReadOnly = false;
                        //node1.Cells["projectid"].Value = "";
                        node1.Cells["projectname"].Value = "";
                        node.Cells["version"].Value = true;
                        //node1.Cells["projectid"].ReadOnly = true;
                        node1.Cells["projectname"].ReadOnly = true;
                        //node1.Cells["realtyid"].Value = "";
                        //node1.Cells["realtyname"].Value = "";
                        node1.Cells["targetrevision"].ReadOnly = true;
                        node1.Cells["drawingnumber"].Value = "AutoFill";
                    }
                    else
                    {
                        //foreach (DataRow rr in dtProjectNo.Rows)
                        //{
                        //    if (rr["ProjectNo"].ToString() == rw["projectid"].ToString())
                        //    {
                        //        node1.Cells["projectname"].Value = rr["ProjectId"].ToString();
                        //        node1.Cells["projectid"].Value = rr["ProjectId"].ToString();
                        //        break;
                        //    }
                        //}

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
                }
            }
            #endregion CreateTreeGrid
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

                // to iterate selected file
                foreach (TreeGridNode currentTreeGrdiNode in selectedTreeGridNodes)
                {
                    if(Convert.ToString(currentTreeGrdiNode.Cells["isEditable"].Value).Length>3)
                    {
                        if (!Convert.ToBoolean(Convert.ToString(currentTreeGrdiNode.Cells["isEditable"].Value).ToLower()))
                        {
                            ShowMessage.InfoMess("You dont have edit permission for this file.");
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    
                    objCmd.FilePath = Convert.ToString(currentTreeGrdiNode.Cells["filepath"].Value);

                    // Is_Save :needs to make changes for multiple file
                    //DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)currentTreeGrdiNode.Cells["State"];
                    //System.Data.DataTable dt = (System.Data.DataTable)cell.DataSource;

                    //objCmd.IsVerChange = Convert.ToString(Convert.ToBoolean(currentTreeGrdiNode.Cells["Version"].Value)).ToLower();
                    //objCmd.ProjectID = Convert.ToString(currentTreeGrdiNode.Cells["ProjectName"].Value);
                    //objCmd.FileStatus = Convert.ToString(currentTreeGrdiNode.Cells["State"].Value);
                    //objCmd.FileType = Convert.ToString(currentTreeGrdiNode.Cells["cadtype"].Value);
                    //objCmd.IsRoot = "true";
                    //objCmd.IsAssociated = "false";
                    //objCmd.FileDescription = CADDescription.Text;
                    Is_Save = objController.ExecuteSave(objCmd);

                    //foreach (TreeGridNode ChildNode in currentTreeGrdiNode.Nodes)
                    //{
                    //    objCmd.IsVerChange = Convert.ToString(Convert.ToBoolean(ChildNode.Cells["Version"].Value)).ToLower();
                    //    objCmd.FileStatus = Convert.ToString(ChildNode.Cells["State"].Value);
                    //    objCmd.FileType = Convert.ToString(ChildNode.Cells["cadtype"].Value);
                    //    objCmd.IsRoot = "false";
                    //    objCmd.IsAssociated = "true";
                    //    Is_Save = objController.ExecuteSave(objCmd);
                    //}

                    if (Is_Save)
                    {
                        // Update document info into document for future refeance
                        try
                        {
                            if (objController.dtDrawingProperty.Rows.Count > 0)
                            {
                                Hashtable htCurrentInfo = Helper.Table2HashTable(objController.dtDrawingProperty, 0);
                                objMgr.SetAttributes(htCurrentInfo);
                                objMgr.UpdateLayoutAttribute1(htCurrentInfo);

                                objMgr.UpdateExRefInfo(objCmd.FilePath, objController.dtDrawingProperty);
                            }
                        }
                        catch (Exception E)
                        {

                        }
                    }

                    // To delete file
                    if (Is_Delete && File.Exists(objCmd.FilePath))
                    {
                        // Save and close the file before deleting. Otherwise system will not allow you to Delete the file.
                        objMgr.SaveActiveDrawing(); // Saves the current active drawing.
                        objMgr.CloseActiveDocument(objCmd.FilePath);    // Close the active document. Specify the file name, other files may be opened.
                        File.Delete(objCmd.FilePath);
                    }
                }
                this.Cursor = Cursors.Default;
                if (Is_Save)
                {
                    ShowMessage.InfoMess("Save operation successfully completed.");
                    this.Close();
                    return;
                }
                else
                {
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
                MessageBox.Show(ex.Message.ToString(), "Error");
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

                        //if (!Convert.ToBoolean(selectedTreeNode.Cells["isletest"].Value))
                        //{
                        //    ShowMessage.ValMess("This file is not latest file, so you can not save it."); selectedTreeNode.Cells["check"].Value = false;savetreeGrid.RefreshEdit();
                        //    return;
                        //}
                        //if (Convert.ToBoolean(selectedTreeNode.Cells["hasStatusClosed"].Value))
                        //{
                        //    ShowMessage.ValMess("This file is closed, so you can not save it."); selectedTreeNode.Cells["check"].Value = false; savetreeGrid.RefreshEdit();
                        //    return;
                        //}
                        //if (!Convert.ToBoolean(selectedTreeNode.Cells["isEditable"].Value))
                        //{
                        //    ShowMessage.InfoMess("This file is not editable. so you can not save it.");
                        //    selectedTreeNode.Cells["check"].Value = false; 
                        //    savetreeGrid.RefreshEdit();
                        //    return;
                        //}
                        if (Convert.ToBoolean(selectedTreeNode.Cells["isowner"].Value))
                        {
                        }
                        else
                        {

                        }
                        if (!Convert.ToBoolean(selectedTreeNode.Cells["canEditStatus"].Value))
                        {
                            ShowMessage.InfoMess("You dont have permission to change status of this file.");
                        }
                    }



                    if (e.RowIndex == 0)
                    {
                        if (Convert.ToString(selectedTreeNode.Cells["drawingid"].Value) == string.Empty)
                        {
                            selectedTreeNode.Cells["projectname"].ReadOnly = false;
                        }
                        else
                        {
                            selectedTreeNode.Cells["projectname"].ReadOnly = true;
                        }

                        // selectedTreeNode.Cells["projectid"].ReadOnly = false;

                        //        String[] strarry = new String[5];
                        //        String DrawingInformation1;
                        //        String DrawingNameandNumber = selectedTreeNode.Cells["drawing"].Value.ToString();
                        //        int index = DrawingNameandNumber.IndexOf('.');
                        //        int length = DrawingNameandNumber.Length;
                        //        if (index > 0)
                        //            DrawingNameandNumber = DrawingNameandNumber.Remove(index);
                        //        DrawingInformation1 = DrawingNameandNumber + ";;" + DrawingNameandNumber + ";" +
                        //                selectedTreeNode.Cells["filepath"].Value.ToString() + ";" + selectedTreeNode.Cells["sourceid"].Value.ToString() +
                        //                ";CAD;" + selectedTreeNode.Cells["isroot"].Value.ToString() + ";" +Convert.ToString(selectedTreeNode.Cells["projectname"].Value) + ";"
                        //                + Convert.ToString(selectedTreeNode.Cells["realtyname"].Value ) + ";" + CADDescription.Text + ";"
                        //                + selectedTreeNode.Cells["sourceid"].Value.ToString() + ";" + MyProjectName + ";" + MyProjectId + ";" + DrawingData["createdon"].ToString()
                        //                + ";" + DrawingData["createdby"].ToString() + ";" + DrawingData["modifiedon"].ToString() + ";" + DrawingData["modifiedby"].ToString() + ";"
                        //                + selectedTreeNode.Cells["Layouts"].Value.ToString();
                        //        htNewDrawings.Add(selectedTreeNode.Cells["drawing"].Value.ToString(), DrawingInformation1);

                        //        //selectedTreeNode.Cells["drawingnumber"].Value = DrawingNameandNumber;
                        //        //selectedTreeNode.Cells["drawing"].Value = DrawingNameandNumber;
                    }
                    else
                    {
                        if (e.RowIndex == 0)
                        {
                            //selectedTreeNode.Cells["projectname"].ReadOnly = false;
                            //selectedTreeNode.Cells["projectid"].ReadOnly = false;
                        }
                        //selectedTreeNode.Cells["realtyname"].ReadOnly = false;
                        //selectedTreeNode.Cells["realtyid"].ReadOnly = false;
                        //if (htNewDrawings.Contains(selectedTreeNode.Cells["drawing"].Value.ToString()))
                        //{
                        //    htNewDrawings.Remove(selectedTreeNode.Cells["drawing"].Value.ToString());
                        //}

                        //if (Convert.ToString(selectedTreeNode.Cells["drawingid"].Value).Trim() == String.Empty)
                        //{
                        //    //selectedTreeNode.Cells["drawingnumber"].Value = "";
                        //    selectedTreeNode.Cells["cadtype"].Value = "";
                        //}

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

                    MyProjectId =Convert.ToString(selectedTreeNode.Cells["projectname"].Value)==string.Empty?"0": Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value));
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];
                    MyProjectNo = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "number", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "id");
                }
                catch
                {
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["projectname"];

                    //c.Value = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "name");
                    //MyProjectId = Convert.ToString(selectedTreeNode.Cells["projectname"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["projectname"].Value));
                    MyProjectId= Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "id", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                    MyProjectNo = Helper.FindIDInCMB((System.Data.DataTable)c.DataSource, "number", Convert.ToString(selectedTreeNode.Cells["projectname"].Value), "PNAMENO");
                }

                try
                {
                    //FileTypeID = Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["cadtype"].Value));
                    FileTypeID = Convert.ToString(selectedTreeNode.Cells["cadtype"].Value) == string.Empty ? "0" : Convert.ToString(Convert.ToDecimal(selectedTreeNode.Cells["cadtype"].Value));
                    FileType= Convert.ToString(selectedTreeNode.Cells["cadtype"].FormattedValue) == string.Empty ? "" :  Convert.ToString(selectedTreeNode.Cells["cadtype"].FormattedValue);
                }
                catch
                {
                    DataGridViewComboBoxCell c = (DataGridViewComboBoxCell)selectedTreeNode.Cells["cadtype"];
                    FileType =Convert.ToString(c.Value);
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

                if(MyProjectId=="0"|| MyProjectId=="-1")
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
                     + ";" + FileTypeID+";"+ selectedTreeNode.Cells["revision"].Value.ToString()+";"+ MyProjectNo + ";" + selectedTreeNode.Cells["DrawingNumber"].Value.ToString()
                     +";"+ FileType + ";" + PreFix;

                
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