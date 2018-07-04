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
using CADController.Controllers;
using CADController;
using CADController.Configuration;
using Microsoft.Win32;
using RestSharp;
using RedBracketConnector;
using Newtonsoft.Json;
using System.IO;
using Microsoft.CSharp;
using AutocadPlugIn.Properties;

namespace AutocadPlugIn.UI_Forms
{
    public partial class frmSearch_And_Open : Form
    {
        public ArrayList drawings = new ArrayList();
        public ArrayList drawingsOpen = new ArrayList();
        public ArrayList OpenMode = new ArrayList();
        public ArrayList OpenMode1 = new ArrayList();
        AutoCADManager cadManager = new AutoCADManager();
        public Dictionary<string, string> projectNameNumberKeyValiuePairList = new Dictionary<string, string>();
        string CheckoutExpandAllEnabled = Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutExpandAllEnabled"));
        string checkoutPath = Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath"));
        ////RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software", true);

        public frmSearch_And_Open()
        {
            InitializeComponent(); this.FormBorderStyle = FormBorderStyle.None;
        }

        private void Search_And_Open_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                treeGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);
                RBConnector objRBC = new RBConnector();
                sg_SearchType.SelectedIndex = 0;

                //Read the keys from the user registry and load it to the UI.
                RestResponse restResponse;




                #region filetype


                Helper.FIllCMB(CDType, objRBC.GetFIleType(), "name", "name", false);
                #endregion filetype

                #region filestatus

                Helper.FIllCMB(CDState, objRBC.GetFIleStatus(), "statusname", "statusname", false);

                #endregion filestatus

                #region projectdetails
                Helper.FIllCMB(CDProjectName, objRBC.GetProjectDetail(), "PNAMENO", "id", false);

                #endregion projectdetails

                restResponse = (RestResponse)ServiceHelper.GetData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), "/AutocadFiles/getLatestRecords", true, null);

                if (restResponse == null || restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error occurred while fetching latest records.");
                    return;
                }
                else
                {
                    try
                    {
                        var res = JsonConvert.DeserializeObject<List<ResultSearchCriteria>>(restResponse.Content);
                    }
                    catch { }


                    var resultSearchCriteriaResponseList = JsonConvert.DeserializeObject<List<ResultSearchCriteria>>(restResponse.Content);
                    BindDataToGrid(resultSearchCriteriaResponseList);
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            Cursor.Current = Cursors.Default;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            treeGridView1.Nodes.Clear();
            this.Cursor = Cursors.WaitCursor;



            List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();


            if (CDProjectName.SelectedIndex > 0)
            {
                string s = Helper.FindIDInCMB((System.Data.DataTable)CDProjectName.DataSource, "id", CDProjectName.Text, "PNAMENO");
                urlParameters.Add(new KeyValuePair<string, string>("projno", Helper.FindIDInCMB((System.Data.DataTable)CDProjectName.DataSource, "number", CDProjectName.Text, "PNAMENO")));
            }

            // If folder name is not null or empty then add the parameter to the URL.
            if (sg_SearchType.SelectedIndex > 0)
            {
                urlParameters.Add(new KeyValuePair<string, string>("location", sg_SearchType.Text));
            }

            dynamic searchCriteria = null;

            if (!string.IsNullOrEmpty(DGName.Text) || CDType.SelectedIndex > 0 || CDState.SelectedIndex > 0 || !string.IsNullOrEmpty(textBox_foldername.Text))
            {
                searchCriteria = new SearchCriteria();
            }

            if (!string.IsNullOrEmpty(DGName.Text))
            {
                searchCriteria.fileNo = DGName.Text;
            }

            if (CDType.SelectedIndex > 0)
            {
                searchCriteria.type = new SearchCriteriaType
                {
                    name = CDType.Text
                };
            }

            if (CDState.SelectedIndex > 0)
            {
                searchCriteria.status = new StatusCriteria
                {
                    statusname = CDState.Text
                };
            }

            if (!string.IsNullOrEmpty(textBox_foldername.Text))
            {
                searchCriteria.folder = new SearchCriteriaFolder
                {
                    name = textBox_foldername.Text
                };
            }


            RestResponse restResponse = (RestResponse)ServiceHelper.PostData(
                Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
               "/AutocadFiles/searchAutocadFiles",
               DataFormat.Json,
               searchCriteria,
               true,
               urlParameters);

            var resultSearchCriteriaResponseList = JsonConvert.DeserializeObject<List<ResultSearchCriteria>>(restResponse.Content);
            BindDataToGrid(resultSearchCriteriaResponseList, true);
            this.Cursor = Cursors.Default;


        }

        //private void BindDataToGrid(List<ResultSearchCriteria> resultSearchCriteriaResponseList)
        //{
        //    foreach (ResultSearchCriteria resultSearchCriteriaRecord in resultSearchCriteriaResponseList)
        //    {
        //        TreeGridNode treeGridNode = treeGridView1.Nodes.Add(
        //            null,
        //            null,
        //            resultSearchCriteriaRecord.name,
        //            resultSearchCriteriaRecord.fileNo,
        //            (bool)resultSearchCriteriaRecord.filelock,
        //            resultSearchCriteriaRecord.coreType.name,
        //            resultSearchCriteriaRecord.status.statusname,
        //            resultSearchCriteriaRecord.versionno,
        //            null,
        //            null,
        //            resultSearchCriteriaRecord.size,
        //            resultSearchCriteriaRecord.id,
        //            null,
        //            resultSearchCriteriaRecord.createdby,
        //            null);
        //    }

        //    treeGridView1.Show();
        //    searchStatus.Visible = true;
        //    busyLabel.Visible = false;
        //    this.Cursor = Cursors.Default;
        //    this.searchStatus.Text = resultSearchCriteriaResponseList.Count.ToString() + " Items Found..";

        //}

        private void BindDataToGrid(List<ResultSearchCriteria> resultSearchCriteriaResponseList, bool isSortingRequired = false)
        {
            if (resultSearchCriteriaResponseList == null)
            {
                this.searchStatus.Text = "No Items Found..";
                return;
            }

            resultSearchCriteriaResponseList = (from resultSearchCriteriaResponse in resultSearchCriteriaResponseList
                                                where resultSearchCriteriaResponse.name.EndsWith("dwg")
                                                select resultSearchCriteriaResponse).ToList();

            if (isSortingRequired)
            {
                resultSearchCriteriaResponseList = resultSearchCriteriaResponseList.OrderBy(resultSearchCriteriaResponse => resultSearchCriteriaResponse.name).ToList();
            }

            if (resultSearchCriteriaResponseList.Count > 50)
            {
                MessageBox.Show("Search yields more than 50 records. Please add specific search criteria.");
                return;
            }

            foreach (ResultSearchCriteria resultSearchCriteriaRecord in resultSearchCriteriaResponseList)
            {


                TreeGridNode treeGridNode = treeGridView1.Nodes.Add(
                    null,
                    null,
                     resultSearchCriteriaRecord.name,
                     resultSearchCriteriaRecord.name == null ? new Bitmap(1, 1) : resultSearchCriteriaRecord.name.ToLowerInvariant().EndsWith("dwg", StringComparison.InvariantCulture) ? new Bitmap(1, 1) : Resources.ReferenceImage,
                    //resultSearchCriteriaRecord.name == null ? new Bitmap(1, 1) : resultSearchCriteriaRecord.name.ToLowerInvariant().EndsWith("dwg", StringComparison.InvariantCulture) ? new Bitmap(1, 1) : Resources.ReferenceImage,
                    resultSearchCriteriaRecord.fileNo,
                    (bool)resultSearchCriteriaRecord.filelock,
                    resultSearchCriteriaRecord.type == null ? null : resultSearchCriteriaRecord.type.name,
                    resultSearchCriteriaRecord.status.statusname,
                    resultSearchCriteriaRecord.versionno,
                    resultSearchCriteriaRecord.projectname,
                    resultSearchCriteriaRecord.projectinfo,
                    resultSearchCriteriaRecord.size,
                    resultSearchCriteriaRecord.id,
                    null,
                    null,
                    null);

                AddChildNode(resultSearchCriteriaRecord, ref treeGridNode);
                if (CheckoutExpandAllEnabled == "True")
                {
                    //ExpandNode(treeGridNode);
                    treeGridNode.Expand();
                }
            }

            treeGridView1.Show();
            searchStatus.Visible = true;
            busyLabel.Visible = false;
            this.Cursor = Cursors.Default;
            this.searchStatus.Text = resultSearchCriteriaResponseList.Count.ToString() + " Items Found..";
        }

        private void AddChildNode(ResultSearchCriteria resultSearchCriteriaRecord, ref TreeGridNode parentTreeGridNode)
        {
            RestResponse restResponse = (RestResponse)ServiceHelper.GetData(
                Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                "/AutocadFiles/getAssoFile",
                false,
                new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("fileId", resultSearchCriteriaRecord.id),
                new KeyValuePair<string, string>("userName", Helper.UserName)});
            if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var childRecords = JsonConvert.DeserializeObject<List<ResultSearchCriteria>>(restResponse.Content);

                if (childRecords == null || childRecords.Count <= 0)
                {
                    return;
                }

                foreach (ResultSearchCriteria resultSearchCriteriaChildRecord in childRecords)
                {
                    TreeGridNode treeGridNode = parentTreeGridNode.Nodes.Add(
                                                    null,
                                                    null,
                                                    resultSearchCriteriaChildRecord.name,
                                                      //resultSearchCriteriaChildRecord.name == null ? new Bitmap(1, 1) : resultSearchCriteriaChildRecord.name.ToLowerInvariant().EndsWith("dwg", StringComparison.InvariantCulture) ? new Bitmap(1, 1) : Resources.ReferenceImage,
                                                      resultSearchCriteriaChildRecord.name == null ? new Bitmap(1, 1) : resultSearchCriteriaChildRecord.name.ToLowerInvariant().EndsWith("dwg", StringComparison.InvariantCulture) ? Resources.ReferenceImage : new Bitmap(1, 1) ,

                                                    resultSearchCriteriaChildRecord.fileNo,
                                                    (bool)resultSearchCriteriaChildRecord.filelock,
                                                    resultSearchCriteriaChildRecord.type.name,
                                                    resultSearchCriteriaChildRecord.status.statusname,
                                                    resultSearchCriteriaChildRecord.versionno,
                                                    resultSearchCriteriaChildRecord.projectname,
                                                    resultSearchCriteriaChildRecord.projectinfo,
                                                    resultSearchCriteriaChildRecord.size,
                                                    resultSearchCriteriaChildRecord.id,
                                                    null,
                                                    null,
                                                    null);

                    //AddChildNode(resultSearchCriteriaChildRecord, ref treeGridNode);
                }
            }
            else
            {
                ShowMessage.ErrorMess("Something went wrong while retreiving associated file. ");
            }

        }





        private void ExpandNode(TreeGridNode CurrentNode)
        {
            try
            {
                for (int nodeCount = 0; nodeCount < CurrentNode.Nodes.Count(); nodeCount++)
                {
                    if (CurrentNode.Nodes.ElementAt(nodeCount).Nodes.Count() > 0)
                    {
                        TreeGridNode node1 = CurrentNode.Nodes.ElementAt(nodeCount);
                        CurrentNode.Nodes.ElementAt(nodeCount).Expand();
                        this.ExpandNode(node1);
                    }
                }
            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess("Exception Occur: " + ex);
                return;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void SetTreeView(TreeGridNode senderNode, TreeGridNode recieverNode)
        {
            try
            {
                for (int nodeCount = 0; nodeCount < senderNode.Nodes.Count(); nodeCount++)
                {
                    if (senderNode.Nodes.ElementAt(nodeCount).Nodes.Count() == 0)
                    {
                        TreeGridNode node1 = recieverNode.Nodes.Add(senderNode.Nodes.ElementAt(nodeCount).Cells[0].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[1].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[2].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[3].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[4].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[5].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[6].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[7].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[8].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[9].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[10].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[11].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[12].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[13].Value);

                        if ((String)senderNode.Nodes.ElementAt(nodeCount).Cells[8].Value == "1")
                        {
                            node1.ImageIndex = 0;
                            node1.Cells[14].ReadOnly = true;
                        }
                        else if ((String)senderNode.Nodes.ElementAt(nodeCount).Cells[8].Value == "2")
                        {
                            node1.ImageIndex = 1;
                            node1.Cells[14].ReadOnly = true;
                        }
                    }
                    else
                    {
                        TreeGridNode node1 = recieverNode.Nodes.Add(senderNode.Nodes.ElementAt(nodeCount).Cells[0].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[1].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[2].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[3].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[4].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[5].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[6].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[7].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[8].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[9].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[10].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[11].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[12].Value, senderNode.Nodes.ElementAt(nodeCount).Cells[13].Value);

                        if ((String)senderNode.Nodes.ElementAt(nodeCount).Cells[8].Value == "1")
                        {
                            node1.ImageIndex = 0;
                            node1.Cells[14].ReadOnly = true;
                        }
                        else if ((String)senderNode.Nodes.ElementAt(nodeCount).Cells[8].Value == "2")
                        {
                            node1.ImageIndex = 1;
                            node1.Cells[14].ReadOnly = true;
                        }

                        this.SetTreeView(senderNode.Nodes.ElementAt(nodeCount), node1);
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
                for (int nodeCount = 0; nodeCount < CurrentNode.Nodes.Count(); nodeCount++)
                {
                    if (CurrentNode.Nodes.ElementAt(nodeCount).Nodes.Count() == 0)
                    {
                        treeGridView1.Nodes.Remove(CurrentNode.Nodes.ElementAt(nodeCount));
                    }
                    else
                    {
                        TreeGridNode node1 = CurrentNode.Nodes.ElementAt(nodeCount);
                        this.ClearTreeView(node1);
                        treeGridView1.Nodes.Remove(node1);
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
                return;
            }

        }

        private void ExpandNodeForCheck(TreeGridNode CurrentNode)
        {
            string id = null;
            String Mode = "View";
            try
            {
                for (int nodeCount = 0; nodeCount < CurrentNode.Nodes.Count(); nodeCount++)
                {
                    Mode = "View";
                    if (CurrentNode.Nodes.ElementAt(nodeCount).Nodes.Count() == 0)
                    {
                        CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].Value = CurrentNode.Cells[0].EditedFormattedValue;

                        id = (string)CurrentNode.Nodes.ElementAt(nodeCount).Cells[7].Value;
                        if (CurrentNode.Nodes.ElementAt(nodeCount).Cells[14].Value != null && CurrentNode.Nodes.ElementAt(nodeCount).Cells[14].Value.ToString() != "False")
                            Mode = "Edit";
                        if ((bool)CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].Value)
                        {
                            drawings.Add(id);
                            OpenMode.Add(Mode);
                        }
                        else
                        {
                            drawings.Remove(id); OpenMode.Remove(Mode);
                        }
                    }
                    else
                    {
                        TreeGridNode node1 = CurrentNode.Nodes.ElementAt(nodeCount);
                        CurrentNode.Nodes.ElementAt(nodeCount).Expand();
                        CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].Value = CurrentNode.Cells[0].EditedFormattedValue;

                        id = (string)CurrentNode.Nodes.ElementAt(nodeCount).Cells[7].Value;
                        if (CurrentNode.Nodes.ElementAt(nodeCount).Cells[14].Value != null && CurrentNode.Nodes.ElementAt(nodeCount).Cells[14].Value.ToString() != "False")
                            Mode = "Edit";
                        if ((bool)CurrentNode.Nodes.ElementAt(nodeCount).Cells[0].Value)
                        {
                            drawings.Add(id);
                            OpenMode.Add(Mode);
                        }
                        else
                        {
                            drawings.Remove(id); OpenMode.Remove(Mode);
                        }

                        this.ExpandNodeForCheck(node1);
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
                return;
            }
        }

        List<TreeGridNode> selectedTreeGridNodes = new List<TreeGridNode>();
        private void OpenDrawingButton_Click(object sender, EventArgs e)
        {

            List<PLMObject> pLMObjects = new List<PLMObject>();
            RBConnector objRBC = new RBConnector();
            try
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (TreeGridNode treeGridNode in treeGridView1.Nodes)
                {
                    if ((bool)treeGridNode.Cells[0].FormattedValue)
                    {
                        selectedTreeGridNodes.Add(treeGridNode);
                    }
                }

                if (selectedTreeGridNodes.Count < 1)
                {
                    this.Cursor = Cursors.Default;
                    ShowMessage.ValMess("Please select at least one main file to Open");
                    return;
                }

                if (string.IsNullOrEmpty(checkoutPath))
                {
                    this.Cursor = Cursors.Default;
                    ShowMessage.ValMess("Please set checkout path under settings, before opening any file.");
                    return;
                }

                foreach (TreeGridNode currentTreeGrdiNode in selectedTreeGridNodes)
                {

                    string ProjectName = Convert.ToString(currentTreeGrdiNode.Cells["ProjectId"].FormattedValue);
                    if (ProjectName.Trim().Length == 0)
                    {
                        ProjectName = "MyFiles";
                    }

                    checkoutPath = Path.Combine(checkoutPath, ProjectName);
                    if (!Directory.Exists(checkoutPath))
                    {
                        Directory.CreateDirectory(checkoutPath);
                    }

                    string PreFix = "";
                    string PreFix1 = "";
                    if (ProjectName != "MyFiles")
                    {
                        PreFix = Convert.ToString(currentTreeGrdiNode.Cells["ProjectName"].Value) + "-";
                    }
                    PreFix = PreFix + Convert.ToString(currentTreeGrdiNode.Cells["DrawingNumber"].Value) + "-";

                    PreFix += Convert.ToString(currentTreeGrdiNode.Cells["CADType"].Value) == string.Empty ? string.Empty : Convert.ToString(currentTreeGrdiNode.Cells["CADType"].Value) + "-";

                    PreFix += Convert.ToString(currentTreeGrdiNode.Cells["Generation"].Value) == string.Empty ? string.Empty : Convert.ToString(currentTreeGrdiNode.Cells["Generation"].Value) + "#";

                    PreFix1 = PreFix;
                    string filePathName = Path.Combine(checkoutPath, PreFix + Convert.ToString(currentTreeGrdiNode.Cells["DrawingName"].Value));

                    if (File.Exists(filePathName))
                    {
                        if (cadManager.CheckForCurruntlyOpenDoc(filePathName))
                        {
                            this.Cursor = Cursors.Default;
                            ShowMessage.ValMess("This file is already open.");
                            this.Close();
                            return;
                        }
                    }


                    foreach (TreeGridNode childNode in currentTreeGrdiNode.Nodes)
                    {
                        if ((bool)childNode.Cells[0].FormattedValue)
                        {
                            PreFix = "";
                            if (ProjectName != "MyFiles")
                            {
                                PreFix = Convert.ToString(childNode.Cells["ProjectName"].Value) + "-";
                            }
                            PreFix = PreFix + Convert.ToString(childNode.Cells["DrawingNumber"].Value) + "-";

                            PreFix += Convert.ToString(childNode.Cells["CADType"].Value) == string.Empty ? string.Empty : Convert.ToString(childNode.Cells["CADType"].Value) + "-";

                            PreFix += Convert.ToString(childNode.Cells["Generation"].Value) == string.Empty ? string.Empty : Convert.ToString(childNode.Cells["Generation"].Value) + "#";

                            if (!Helper.IsRenameChild)
                            {
                                PreFix = "";
                            }

                            DownloadOpenDocument(childNode.Cells["DrawingID"].FormattedValue.ToString(), childNode.Cells["DrawingName"].FormattedValue.ToString(), checkoutPath, "Checkout", false, null, PreFix);


                            pLMObjects.Add(new PLMObject() { ObjectId = childNode.Cells["DrawingID"].FormattedValue.ToString() });
                        }
                    }
                    string FileID = currentTreeGrdiNode.Cells["DrawingID"].FormattedValue.ToString();
                    DownloadOpenDocument(currentTreeGrdiNode.Cells["DrawingID"].FormattedValue.ToString(), currentTreeGrdiNode.Cells["DrawingName"].FormattedValue.ToString(), checkoutPath, "Checkout", true, currentTreeGrdiNode, PreFix1);
                    PLMObject objplmo = new PLMObject();

                    try
                    {

                        objplmo.ObjectId = FileID;
                    }
                    catch { }
                    try
                    {
                        pLMObjects.Add(objplmo);
                        cadManager.SaveActiveDrawing(false);
                    }
                    catch { }
                    //foreach (TreeGridNode childNode in currentTreeGrdiNode.Nodes)
                    //{
                    //    string oldFileName = Path.Combine(checkoutPath, Convert.ToString(childNode.Cells["DrawingName"].FormattedValue));
                    //    string newFileName = Path.Combine(checkoutPath, Helper.FileNamePrefix + Convert.ToString(childNode.Cells["DrawingName"].FormattedValue));
                    //    if (File.Exists(oldFileName))
                    //    {
                    //        File.Delete(newFileName); // Delete the existing file if exists
                    //        File.Move(oldFileName, newFileName); // Rename the oldFileName into newFileName
                    //    }
                    //}
                }
                objRBC.LockObject(pLMObjects);
                CADRibbon ribbon = new CADRibbon();
                ribbon.browseDEnable = true;
                ribbon.createDEnable = true;
                ribbon.browseBEnable = true;
                ribbon.createBEnable = true;
                ribbon.LockEnable = true;
                ribbon.SaveEnable = true;
                ribbon.UnlockEnable = true;
                ribbon.DrawingInfoEnable = true;
                ribbon.MyRibbon();
                this.Cursor = Cursors.Default;
                this.Close();
            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess("Exception Occur: " + ex);

                objRBC.UnlockObject(pLMObjects);
                this.Cursor = Cursors.Default;
                return;
            }
            this.Cursor = Cursors.Default;
        }

        private void DownloadOpenDocument(string fileId, string fileName, string checkoutPath, string fileMode, bool isParentFile = false, TreeGridNode tgnParent = null, string FilePreFix = null)
        {
            try
            {

                RestResponse restResponse = (RestResponse)ServiceHelper.GetData(
                                               Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                                               "/AutocadFiles/downloadAutocadSingleFile",
                                               true,
                                               new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("fileId", fileId )
                                               });
                if (restResponse.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(restResponse.Content))
                {
                    //RBConnector objRBC = new RBConnector();
                    //ResultSearchCriteria Drawing = objRBC.GetDrawingInformation(fileId);
                    //Hashtable DrawingProperty = new Hashtable();
                    //List<Hashtable> LayoutProperty = new List<Hashtable>();

                    //#region LayoutInfo
                    //String LayoutInfos = "";
                    //LayoutInfos = Helper.GetLayoutInfo(Drawing.fileLayout);
                    //#endregion

                    //DrawingProperty.Add("DrawingId", Drawing.id);
                    //DrawingProperty.Add("DrawingName", Drawing.name);
                    //DrawingProperty.Add("Classification", "");
                    //DrawingProperty.Add("FileTypeID", Drawing.type == null ? string.Empty : Drawing.type.name == null ? string.Empty : Drawing.type.name);
                    //DrawingProperty.Add("DrawingNumber", Drawing.fileNo);
                    //DrawingProperty.Add("DrawingState", Drawing.status == null ? string.Empty : Drawing.status.statusname);
                    //DrawingProperty.Add("Revision", Drawing.versionno.Contains("Ver ") ? Drawing.versionno.Substring(4) : Drawing.versionno);
                    //DrawingProperty.Add("LockStatus", Drawing.filelock);
                    //DrawingProperty.Add("Generation", "123");
                    //DrawingProperty.Add("Type", Drawing.coreType.id);
                    ////DrawingProperty.Add("ProjectName", Drawing.projectname );
                    //if (Drawing.projectname.Trim().Length == 0)
                    //{
                    //    DrawingProperty.Add("ProjectName", "My Files");
                    //}
                    //else
                    //{
                    //    DrawingProperty.Add("ProjectName", Drawing.projectname + " (" + Drawing.projectNumber + ")");
                    //}

                    //DrawingProperty.Add("ProjectId", Drawing.projectinfo);
                    //DrawingProperty.Add("CreatedOn", Drawing.updatedon);
                    //DrawingProperty.Add("CreatedBy", Drawing.createdby);
                    //DrawingProperty.Add("ModifiedOn", Drawing.updatedon);
                    //DrawingProperty.Add("ModifiedBy", Drawing.updatedby);

                    //DrawingProperty.Add("canDelete", Drawing.canDelete);
                    //DrawingProperty.Add("isowner", Drawing.isowner);
                    //DrawingProperty.Add("hasViewPermission", Drawing.hasViewPermission);
                    //DrawingProperty.Add("isActFileLatest", Drawing.isActFileLatest);

                    //DrawingProperty.Add("isEditable", Drawing.isEditable);
                    //DrawingProperty.Add("canEditStatus", Drawing.canEditStatus);
                    //DrawingProperty.Add("hasStatusClosed", Drawing.hasStatusClosed);
                    //DrawingProperty.Add("isletest", Drawing.isletest);

                    //DrawingProperty.Add("projectno", Drawing.projectNumber);
                    //DrawingProperty.Add("prefix", FilePreFix);
                    //DrawingProperty.Add("LayoutInfo", LayoutInfos);

                    ////DrawingProperty.Add("isroot", true);
                    ////DrawingProperty.Add("sourceid","");
                    ////DrawingProperty.Add("Layouts","");

                    //// string filePathName = Path.Combine(checkoutPath, Helper.FileNamePrefix + "Drawing1.dwg");
                    if (fileName.Length > FilePreFix.Length)
                    {
                        if (fileName.Substring(0, FilePreFix.Length) == FilePreFix)
                        {
                            fileName = fileName.Substring(FilePreFix.Length);
                        }
                    }


                    string filePathName = Path.Combine(checkoutPath, fileName);

                    //if (isParentFile)
                    {
                        filePathName = Path.Combine(checkoutPath, FilePreFix + fileName);
                    }

                    using (var binaryWriter = new BinaryWriter(File.Open(filePathName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Delete)))
                    {
                        binaryWriter.Write(restResponse.RawBytes);
                    }

                    if (isParentFile)
                    {
                        //if (Helper.IsRenameChild)
                        //{
                        //    cadManager.UpdateExRefPathInfo(filePathName);
                        //}

                        //cadManager.OpenActiveDocument(filePathName, "View", DrawingProperty);
                        cadManager.OpenActiveDocument(filePathName, "View");
                        try
                        {
                            //  cadManager.SaveActiveDrawing(false);
                        }
                        catch { }


                    }
                    else
                    {
                        //cadManager.SetAttributesXrefFiles(DrawingProperty, filePathName);
                        //cadManager.UpdateLayoutAttributeArefFile(DrawingProperty, filePathName);
                    }
                }
                else
                {
                    ShowMessage.ErrorMess("Some error occures while retrieving file.\nThis may be because of you may not have the proper access to the file.");
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }
        private void FormCancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void treeGridView1_CellBeginEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 14)
            {
                dataGridView1 = (System.Windows.Forms.DataGridView)treeGridView1;
                TreeGridNode TreeNode1 = (TreeGridNode)dataGridView1.Rows[e.RowIndex];
                if ((bool)TreeNode1.Cells[14].EditedFormattedValue)
                {
                    String sg_lockStatus = (string)TreeNode1.Cells[8].Value;
                    if (sg_lockStatus == "1" || sg_lockStatus == "2")
                    {
                        MessageBox.Show("The Item " + TreeNode1.Cells[2].Value.ToString() + " is already Locked by " + TreeNode1.Cells[9].Value.ToString());
                        TreeNode1.Cells[14].Value = false;
                        TreeNode1.Cells[14].ReadOnly = true;
                    }
                    else
                    {
                        //TreeNode1.Cells[13].Value = true;
                    }
                }
            }
        }

        private void treeGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 14)
            {
                dataGridView1 = (System.Windows.Forms.DataGridView)treeGridView1;
                TreeGridNode TreeNode1 = (TreeGridNode)dataGridView1.Rows[e.RowIndex];
                if ((bool)TreeNode1.Cells[14].ReadOnly)
                {
                    String sg_lockStatus = (string)TreeNode1.Cells[8].Value;
                    if (sg_lockStatus == "1")
                    {
                        MessageBox.Show("The Item " + TreeNode1.Cells[2].Value.ToString() + " is already Locked by you." + TreeNode1.Cells[9].Value.ToString());
                        TreeNode1.Cells[14].Value = false;
                    }
                    else if (sg_lockStatus == "2")
                    {
                        MessageBox.Show("The Item " + TreeNode1.Cells[2].Value.ToString() + " is already Locked by " + TreeNode1.Cells[9].Value.ToString() + ".");
                        TreeNode1.Cells[14].Value = false;
                    }
                    else
                    {
                        //TreeNode1.Cells[13].ReadOnly = false;
                    }
                }
            }

            if (e.ColumnIndex == 0)
            {
                dataGridView1 = (System.Windows.Forms.DataGridView)treeGridView1;
                TreeGridNode TreeNode1 = (TreeGridNode)dataGridView1.Rows[e.RowIndex];

                if ((bool)TreeNode1.Cells[0].EditedFormattedValue)
                {
                    // Handling check box check and uncheck features.
                    foreach (TreeGridNode node in dataGridView1.Rows)
                    {
                        node.Cells[0].Value = false;
                    }

                    TreeNode1.Cells[0].Value = true;
                }

                TreeNode1.Expand();
                String id = (string)TreeNode1.Cells[7].Value;
                String Mode = "View";
                if (TreeNode1.Cells[14].Value != null && TreeNode1.Cells[14].Value.ToString() != "False")
                    Mode = "Edit";
                if ((bool)TreeNode1.Cells[0].EditedFormattedValue)
                {
                    drawingsOpen.Add(id);
                    OpenMode1.Add(Mode);
                    TreeNode1.Cells[14].ReadOnly = true;
                }
                else
                {
                    drawings.Remove(id);
                    if (drawingsOpen.Contains(id))
                        OpenMode1.Remove(Mode);
                    drawingsOpen.Remove(id);
                    OpenMode.Remove(Mode);
                    TreeNode1.Cells[14].ReadOnly = false;
                }
                this.ExpandNodeForCheck(TreeNode1);
            }

            else if (e.ColumnIndex == 1)
            {
                dataGridView1 = (System.Windows.Forms.DataGridView)treeGridView1;
                TreeGridNode TreeNode1 = (TreeGridNode)dataGridView1.Rows[e.RowIndex];

                if (Convert.ToString(TreeNode1.Cells[1].Value) == "+")
                {
                    TreeNode1.Cells[1].Value = "-";

                    if (!TreeNode1.HasChildren)
                    {
                        ExpandObjectCommand expandDrawing = new ExpandObjectCommand();

                        expandDrawing.PLMObjectInfo = Convert.ToString(TreeNode1.Cells[7].Value) + ":" + "CAD";
                        expandDrawing.RelationshipName = "CAD Structure";

                        ExpandObjectController expandDrwingCon = new ExpandObjectController();
                        expandDrwingCon.Execute(expandDrawing);

                        IEnumerator<TreeGridNode> it = expandDrwingCon.dtDocuments.Nodes.GetEnumerator();

                        while (it.MoveNext())
                        {
                            TreeGridNode relNode = (TreeGridNode)it.Current;
                            TreeGridNode node = TreeNode1.Nodes.Add(relNode.Cells[0].Value, relNode.Cells[1].Value, relNode.Cells[2].Value, relNode.Cells[3].Value, relNode.Cells[4].Value, relNode.Cells[5].Value, relNode.Cells[6].Value, relNode.Cells[7].Value, relNode.Cells[8].Value);

                            if ((String)relNode.Cells[8].Value == "1")
                            {
                                node.ImageIndex = 0;
                                node.Cells[14].ReadOnly = true;
                            }
                            else if ((String)relNode.Cells[8].Value == "2")
                            {
                                node.ImageIndex = 1;
                                node.Cells[14].ReadOnly = true;
                            }
                        }
                    }
                    TreeNode1.Expand();

                }
                else if (Convert.ToString(TreeNode1.Cells[1].Value) == "-")
                {
                    TreeNode1.Cells[1].Value = "+";
                    TreeNode1.Collapse();
                }
                treeGridView1.Show();
            }

        }

        private void treeGridView1_NodeCollapsing(object sender, CollapsingEventArgs e)
        {
            TreeGridNode TreeNode1 = (TreeGridNode)e.Node;
            TreeNode1.Cells[1].Value = "+";
            treeGridView1.Show();
        }

        private void treeGridView1_NodeExpanding(object sender, ExpandingEventArgs e)
        {
            TreeGridNode TreeNode1 = (TreeGridNode)e.Node;

            TreeNode1.Cells[1].Value = "-";
            treeGridView1.Show();
        }





        private void sg_SearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (sg_SearchType.Text.ToLower() == "my files")
                {
                    CDProjectName.SelectedIndex = 0;
                    CDProjectName.Enabled = false;
                }
                else
                {
                    CDProjectName.Enabled = true;
                }
            }
            catch
            {

            }
        }














        private void CDProjectName_KeyUp(object sender, KeyEventArgs e)
        {
            comboBoxSearch();
        }

        private void comboBoxSearch()
        {
            CDProjectName.DroppedDown = true;

            object[] actualItemList = (object[])CDProjectName.Tag;
            if (actualItemList == null)
            {
                // Maintain back of the original list
                actualItemList = new object[CDProjectName.Items.Count];
                CDProjectName.Items.CopyTo(actualItemList, 0);
                CDProjectName.Tag = actualItemList;
            }

            // prepare list of matching items
            string searchText = CDProjectName.Text.ToLower();
            IEnumerable<object> newList = actualItemList;
            if (searchText.Length > 0)
            {
                newList = actualItemList.Where(item => item.ToString().ToLower().Contains(searchText));
            }

            // Clear the list before attaching the same again.
            while (CDProjectName.Items.Count > 0)
            {
                CDProjectName.Items.RemoveAt(0);
            }

            // re-set list
            CDProjectName.Items.AddRange(newList.ToArray());
        }

        private void treeGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
