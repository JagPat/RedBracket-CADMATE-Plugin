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

namespace AutocadPlugIn.UI_Forms
{
    public partial class Search_And_Open : Form
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

        public Search_And_Open()
        {
            InitializeComponent();
            ////registryKey = registryKey.OpenSubKey("RedBracketConnector", true);

            ////if (registryKey == null)
            ////    return;

            ////registryKey = registryKey.OpenSubKey("LoginSettings", true);

            ////if (registryKey == null)
            ////    return;
        }

        private void Search_And_Open_Load(object sender, EventArgs e)
        {
            RBConnector objRBC = new RBConnector();
            sg_SearchType.SelectedIndex = 0;
            //Read the keys from the user registry and load it to the UI.
            RestResponse restResponse;
            //RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), "/AutocadFiles/fetchFileType", DataFormat.Json, null, true, null);
            // if (restResponse == null)
            // {
            //     MessageBox.Show("Some error occurred while retrieving the files list.");
            // }

            //DataTable dataTableProjectInfo = (DataTable)JsonConvert.DeserializeObject(restResponse.Content, (typeof(DataTable)));

            #region filetype
            //DataView dataView = dataTableProjectInfo.DefaultView;
            //dataView.Sort = "name asc";
            //dataTableProjectInfo = dataView.ToTable();
            //DataRow dataRow = dataTableProjectInfo.NewRow();
            //dataRow.ItemArray = new object[] { null, "All", null, null, null, null, null, null, null, null, null };
            //dataTableProjectInfo.Rows.InsertAt(dataRow, 0);
            //CDType.DataSource = dataTableProjectInfo;
            //CDType.DisplayMember = "name";
            //CDType.ValueMember = "name";

            Helper.FIllCMB(CDType, objRBC.GetFIleType(), "name", "name", false);
            #endregion filetype

            #region filestatus
            restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), "/AutocadFiles/fetchFileStatus", DataFormat.Json, null, true, null);
            var statusInfoList = JsonConvert.DeserializeObject<List<ResultStatusData>>(restResponse.Content);
            statusInfoList = statusInfoList.OrderBy(statusInfo => statusInfo.statusname).ToList();
            statusInfoList.Insert(0, new ResultStatusData { statusname = "All" });
            CDState.DataSource = statusInfoList;
            CDState.DisplayMember = "statusname";
            CDState.ValueMember = "statusname";
            #endregion filestatus

            #region projectdetails
            restResponse = (RestResponse)ServiceHelper.GetData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), "/ProjectAutocad/fetchUserAutocadProjectsService", true, null);
            DataTable dataTableProjectNameNumber = (DataTable)JsonConvert.DeserializeObject(restResponse.Content, (typeof(DataTable)));
            List<string> nameNumberList = new List<string>();
            foreach (DataRow dr in dataTableProjectNameNumber.Rows)
            {
                nameNumberList.Add(dr["name"].ToString() + " (" + dr["number"].ToString() + ")");
                projectNameNumberKeyValiuePairList.Add(dr["name"].ToString() + " (" + dr["number"].ToString() + ")", dr["name"].ToString());
            }

            nameNumberList.Sort();
            nameNumberList.Insert(0, "All");

            //dataView = dataTableProjectNameNumber.DefaultView;
            //dataView.Sort = "number asc";
            //dataTableProjectInfo = dataView.ToTable();
            //dataRow = dataTableProjectNameNumber.NewRow();
            //dataRow.ItemArray = new object[] { null, "All", null, null, null, null, null, null, null, null, null };
            //dataTableProjectInfo.Rows.InsertAt(dataRow, 0);

            //CDProjectId.DataSource = dataTableProjectNameNumber;
            //CDProjectId.DisplayMember = "number";
            //CDProjectId.ValueMember = "number";

            CDProjectName.Items.AddRange(nameNumberList.ToArray());
            CDProjectName.SelectedIndex = 0;
            //CDProjectName.DisplayMember = "name";
            //CDProjectName.ValueMember = "name";
            #endregion projectdetails

            restResponse = (RestResponse)ServiceHelper.GetData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), "/AutocadFiles/getLatestRecords", true, null);
            var resultSearchCriteriaResponseList = JsonConvert.DeserializeObject<List<ResultSearchCriteria>>(restResponse.Content);
            BindDataToGrid(resultSearchCriteriaResponseList);


            //CADIntegrationConfiguration objWordConfig = new CADIntegrationConfiguration();
            //Hashtable htdisplayName = null;

            ////Build datatable for Item Classification and Set Value in Dropdown
            //DataTable dtDocTypes = new DataTable();
            //dtDocTypes.Columns.Add("DisplayName", typeof(string));
            //dtDocTypes.Columns.Add("ClassificationPath", typeof(string));
            //dtDocTypes.Rows.Add("Non", "Non");
            //htdisplayName = objWordConfig.GetClassification();
            //ICollection keys = htdisplayName.Keys;
            //IEnumerator getNames = keys.GetEnumerator();
            //while (getNames.MoveNext())
            //{
            //    dtDocTypes.Rows.Add(getNames.Current.ToString(), getNames.Current.ToString());
            //}
            //CDType.DataSource = dtDocTypes;
            //CDType.DisplayMember = "DisplayName";
            //CDType.ValueMember = "ClassificationPath";

            ////set default value for Checlout view Box
            //CheckOutViewCBox.Text = "Current";

            ////set Default Value for Search Scope
            //sg_SearchType.Text = "Full";

            ////Build datatable for Project and Set Value in Dropdown
            //DataTable dtProjectNo = new DataTable();
            //dtProjectNo.Columns.Add("ProjectId", typeof(string));
            //dtProjectNo.Columns.Add("ProjectName", typeof(string));
            //dtProjectNo.Columns.Add("ProjectNo", typeof(string));
            //dtProjectNo = objWordConfig.GetProject();
            //dtProjectNo.Rows.Add("", "Non", "Non");
            //CDProjectName.DataSource = dtProjectNo;
            //CDProjectName.DisplayMember = "ProjectName";
            //CDProjectName.ValueMember = "ProjectId";

            //CDProjectId.DataSource = dtProjectNo;
            //CDProjectId.DisplayMember = "ProjectNo";
            //CDProjectId.ValueMember = "ProjectId";
            //CDProjectId.SelectedValue = "";

            //CDRealty.SelectedValue = "";
            //CDState.SelectedValue = "";

            //this.imageStrip.ImageSize = new System.Drawing.Size(17, 17);
            //this.imageStrip.TransparentColor = System.Drawing.Color.Magenta;
            //this.imageStrip.ImageSize = new Size(17, 17);
            //this.imageStrip.Images.AddStrip(Properties.Resources.LockImageStrip1);

            //treeGridView1.ImageList = imageStrip;
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            treeGridView1.Nodes.Clear();
            this.Cursor = Cursors.WaitCursor;

            ////RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
            ////    "/AutocadFiles/searchAutocadFiles?userName=archi@yopmail.com&projno=proj001&projname=attune",
            ////    DataFormat.Json,
            ////    new SearchCriteria
            ////    {
            ////        fileNo = "jpg",
            ////        name = "c",
            ////        status = new StatusCriteria
            ////        {
            ////            statusname = "draft"
            ////        }
            ////    },
            ////   false,
            ////   null);

            List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
            ////if (CDProjectId.SelectedIndex != -1)
            ////{
            ////    urlParameters.Add(new KeyValuePair<string, string>("projno", CDProjectId.SelectedText));
            ////}

            if (CDProjectName.SelectedIndex > 0)
            {
                urlParameters.Add(new KeyValuePair<string, string>("projno", Convert.ToString(projectNameNumberKeyValiuePairList[CDProjectName.Text])));
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

            ////object dataToPost = null;

            ////if (searchCriteria != null)
            ////{
            ////    dataToPost = JsonConvert.SerializeObject(searchCriteria, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            ////}

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

            //IEnumerator<TreeGridNode> it = SearchDrawingCon.dtDocuments.Nodes.GetEnumerator();
            //while (it.MoveNext())
            //{
            //    TreeGridNode TreeNode1 = (TreeGridNode)it.Current;
            //    TreeGridNode node = treeGridView1.Nodes.Add(TreeNode1.Cells[0].Value, TreeNode1.Cells[1].Value, TreeNode1.Cells[2].Value, TreeNode1.Cells[3].Value, TreeNode1.Cells[4].Value, TreeNode1.Cells[5].Value, TreeNode1.Cells[6].Value, TreeNode1.Cells[7].Value, TreeNode1.Cells[8].Value, TreeNode1.Cells[9].Value, TreeNode1.Cells[10].Value, TreeNode1.Cells[11].Value, TreeNode1.Cells[12].Value, TreeNode1.Cells[13].Value);

            //    if ((String)TreeNode1.Cells[8].Value == "1")
            //    {
            //        node.ImageIndex = 0;
            //        node.Cells[14].ReadOnly = true;
            //    }
            //    else if ((String)TreeNode1.Cells[8].Value == "2")
            //    {
            //        node.ImageIndex = 1;
            //        node.Cells[14].ReadOnly = true;
            //    }
            //    this.SetTreeView(TreeNode1, node);
            //    if (expandAll == true)
            //    {
            //        node.Expand();
            //        ExpandNode(node);
            //    }
            //}




            //drawings.Clear();
            //OpenMode.Clear();
            //try
            //{
            //    busyLabel.Visible = true;
            //    searchStatus.Visible = false;
            //    this.Cursor = Cursors.WaitCursor;

            //    int totalCount = treeGridView1.Nodes.Count;
            //    for (int i = 0; i < totalCount; i++)
            //    {
            //        TreeGridNode TreeNode1 = treeGridView1.Nodes.ElementAt(0);
            //        this.ClearTreeView(TreeNode1);
            //        treeGridView1.Nodes.Remove(TreeNode1);
            //    }

            //    String DRAWINGNUMBER = DGNumber.Text;
            //    String DRAWINGNAME = DGName.Text;
            //    String CADREVISION = CDRevisionValue.Text;
            //    String REVISION = null;
            //    String AUTHORINGTOOL = "AutoCAD";
            //    String PROJECTID = CDProjectId.SelectedValue.ToString();
            //    String PROJECTNAME = CDProjectName.SelectedValue.ToString();
            //    String STATE = CDState.Text;
            //    String Realty = CDRealty.Text;
            //    String Desktop = sg_SearchType.Text;

            //    if (CheckOutViewCBox.Text.Length != 0)
            //    {
            //        REVISION = CheckOutViewCBox.Text;
            //    }

            //    String CADTYPE = CDType.SelectedValue.ToString();
            //    if (CADTYPE == "Non")
            //        CADTYPE = "";

            //    if (CADREVISION == "")
            //    { }
            //    SearchCommand SearchDrawing = new SearchCommand();
            //    SearchDrawing.PLMObjectInfo = DRAWINGNUMBER + ":" + DRAWINGNAME + ":" + CADTYPE + ":" + CADREVISION + ":" + AUTHORINGTOOL + ":" + "CAD" + ":" + PROJECTID + ":" + PROJECTNAME + ":" + STATE + ":" + Realty + ":" + Desktop;
            //    SearchDrawing.LatestItem = REVISION;
            //    SearchDrawing.RelationshipName = "CAD Structure";

            //    AutocadPlugIn.UI_Forms.CheckoutUserSettings checkout = AutocadPlugIn.UI_Forms.UserSettings.createUserSetting().getCheckoutUserSettings();
            //    SearchDrawing.ExpandAll = checkout.isCheckoutExpandAll;
            //    bool expandAll = checkout.isCheckoutExpandAll;

            //    SearchController SearchDrawingCon = new SearchController();
            //    SearchDrawingCon.Execute(SearchDrawing);

            //    if (SearchDrawingCon.errorString != null)
            //    {
            //        MessageBox.Show("SearchDrawingCon Error:" + SearchDrawingCon.errorString);
            //        this.Cursor = Cursors.Default;
            //        searchStatus.Text = "Ready to Search...";
            //        searchStatus.Visible = true;
            //        busyLabel.Visible = false;
            //        return;
            //    }

            //    IEnumerator<TreeGridNode> it = SearchDrawingCon.dtDocuments.Nodes.GetEnumerator();
            //    while (it.MoveNext())
            //    {
            //        TreeGridNode TreeNode1 = (TreeGridNode)it.Current;
            //        TreeGridNode node = treeGridView1.Nodes.Add(TreeNode1.Cells[0].Value, TreeNode1.Cells[1].Value, TreeNode1.Cells[2].Value, TreeNode1.Cells[3].Value, TreeNode1.Cells[4].Value, TreeNode1.Cells[5].Value, TreeNode1.Cells[6].Value, TreeNode1.Cells[7].Value, TreeNode1.Cells[8].Value, TreeNode1.Cells[9].Value, TreeNode1.Cells[10].Value, TreeNode1.Cells[11].Value, TreeNode1.Cells[12].Value, TreeNode1.Cells[13].Value);

            //        if ((String)TreeNode1.Cells[8].Value == "1")
            //        {
            //            node.ImageIndex = 0;
            //            node.Cells[14].ReadOnly = true;
            //        }
            //        else if ((String)TreeNode1.Cells[8].Value == "2")
            //        {
            //            node.ImageIndex = 1;
            //            node.Cells[14].ReadOnly = true;
            //        }
            //        this.SetTreeView(TreeNode1, node);
            //        if (expandAll == true)
            //        {
            //            node.Expand();
            //            ExpandNode(node);
            //        }
            //    }

            //    treeGridView1.Show();
            //    searchStatus.Visible = true;
            //    busyLabel.Visible = false;
            //    this.Cursor = Cursors.Default;
            //    this.searchStatus.Text = SearchDrawingCon.dtDocuments.Nodes.Count.ToString() +" Items Found..";
            //}
            //catch (System.Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
            //    this.Cursor = Cursors.Default;
            //    searchStatus.Visible = true;
            //    busyLabel.Visible = false;
            //    this.searchStatus.Text = "Ready to Search...";
            //    return;
            //}

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
                    resultSearchCriteriaRecord.fileNo,
                    (bool)resultSearchCriteriaRecord.filelock,
                    resultSearchCriteriaRecord.coreType.name,
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
                new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("fileId", resultSearchCriteriaRecord.id) });

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
                                                resultSearchCriteriaChildRecord.fileNo,
                                                (bool)resultSearchCriteriaChildRecord.filelock,
                                                resultSearchCriteriaChildRecord.coreType.name,
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
                System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
                return;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void CDRevisionValue_TextChanged(object sender, EventArgs e)
        {
            if (CDRevisionValue.Text == "")

                this.CheckOutViewCBox.Enabled = true;
            else
                if (this.CheckOutViewCBox.SelectedValue == "Released")
            {
                this.CheckOutViewCBox.Enabled = true;
            }
            else
                this.CheckOutViewCBox.Enabled = false;
            this.CheckOutViewCBox.Text = "";
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
            #region Old Code
            //if (drawings.Count < 1 && drawingsOpen.Count < 1)
            //{
            //    MessageBox.Show("Please Select any file to Open");
            //    return;
            //}
            //try
            //{
            //    this.Cursor = Cursors.WaitCursor;
            //    AutocadPlugIn.UI_Forms.CheckoutUserSettings checkoutDirPath = AutocadPlugIn.UI_Forms.UserSettings.createUserSetting().getCheckoutUserSettings();
            //    String checkoutPath = checkoutDirPath.checkoutDirPath;
            //    if (checkoutPath.EndsWith("\\"))
            //        checkoutPath = checkoutPath.Substring(0, checkoutPath.Length - 1);

            //    OpenCommand OpenRefDrawing = new OpenCommand();

            //    OpenRefDrawing.ItemType = "CAD";
            //    //OpenRefDrawing.OpenMode = "View";
            //    OpenRefDrawing.CkeckOutPath = checkoutPath;
            //    for (int v = 0; v < drawings.Count; v++)
            //    {
            //        String ItemId = drawings[v].ToString();
            //        OpenRefDrawing.ItemId = ItemId;
            //        //OpenRefDrawing.OpenMode = OpenMode[v + 1].ToString();
            //        String Checkout = OpenMode[v].ToString();
            //        OpenRefDrawing.ItemInfo.Rows.Add(ItemId, "", "", "", "", "", "", "", "", "CAD", false, "", "", "", Checkout, "", "", "", "");
            //    }

            //    OpenController OpenDrawingCon1 = new OpenController();
            //    OpenDrawingCon1.Execute(OpenRefDrawing);

            //    if (OpenDrawingCon1.errorString != null)
            //    {
            //        MessageBox.Show(OpenDrawingCon1.errorString);
            //        this.Cursor = Cursors.Default;
            //        return;
            //    }

            //    foreach (DataRow rw in OpenRefDrawing.ItemInfo.Rows)
            //    {
            //        Hashtable DrawingProperty = new Hashtable();
            //        /* int i = 0;
            //         foreach (DataColumn column in OpenRefDrawing.ItemInfo.Columns)
            //         {
            //             MessageBox.Show(column.ColumnName.ToString() + ":" + rw[i].ToString());
            //             i++;
            //         }*/
            //        if (rw["Error"].ToString() == "")
            //        {
            //            if ((Boolean)rw["IsFile"])
            //            {
            //                DrawingProperty.Add("DrawingId", rw["DrawingId"].ToString());
            //                DrawingProperty.Add("DrawingName", rw["DrawingName"].ToString());
            //                DrawingProperty.Add("Classification", rw["Classification"].ToString());
            //                DrawingProperty.Add("DrawingNumber", rw["DrawingNumber"].ToString());
            //                DrawingProperty.Add("DrawingState", rw["DrawingState"].ToString());
            //                DrawingProperty.Add("Revision", rw["Revision"].ToString());
            //                DrawingProperty.Add("LockStatus", rw["LockStatus"].ToString());
            //                DrawingProperty.Add("Generation", rw["Generation"].ToString());
            //                DrawingProperty.Add("Type", rw["Type"].ToString());
            //                DrawingProperty.Add("ProjectName", rw["ProjectName"].ToString());
            //                DrawingProperty.Add("ProjectId", rw["ProjectId"].ToString());
            //                DrawingProperty.Add("CreatedOn", rw["CreatedOn"].ToString());
            //                DrawingProperty.Add("CreatedBy", rw["CreatedBy"].ToString());
            //                DrawingProperty.Add("ModifiedOn", rw["ModifiedOn"].ToString());
            //                DrawingProperty.Add("ModifiedBy", rw["ModifiedBy"].ToString());
            //                String fileOpenPath = checkoutPath + "\\" + rw["NativeFileName"].ToString();

            //                cadManager.OpenActiveDocument(fileOpenPath, "Checkout", DrawingProperty);
            //            }
            //            else
            //            {
            //                MessageBox.Show("File not Found at --> " + rw["DrawingName"].ToString());
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show(rw["Error"].ToString());
            //        }
            //    }

            //    OpenCommand OpenMainDrawing = new OpenCommand();

            //    OpenMainDrawing.ItemType = "CAD";
            //    //OpenMainDrawing.OpenMode = "View";
            //    OpenMainDrawing.CkeckOutPath = checkoutPath;

            //    for (int v = 0; v < drawingsOpen.Count; v++)
            //    {
            //        String ItemId = drawingsOpen[v].ToString();
            //        OpenMainDrawing.ItemId = ItemId;
            //        //OpenMainDrawing.OpenMode = OpenMode[v].ToString();
            //        String Checkout = OpenMode1[v].ToString();
            //        OpenMainDrawing.ItemInfo.Rows.Add(ItemId, "", "", "", "", "", "", "", "", "CAD", false, "", "", "", Checkout, "", "", "", "");
            //    }
            //    OpenDrawingCon1.Execute(OpenMainDrawing);

            //    if (OpenDrawingCon1.errorString != null)
            //    {
            //        MessageBox.Show(OpenDrawingCon1.errorString);
            //        this.Cursor = Cursors.Default;
            //        return;
            //    }

            //    foreach (DataRow rw in OpenMainDrawing.ItemInfo.Rows)
            //    {
            //        Hashtable DrawingProperty = new Hashtable();

            //        if (rw["Error"].ToString() == "")
            //        {
            //            if ((Boolean)rw["IsFile"])
            //            {
            //                DrawingProperty.Add("DrawingId", rw["DrawingId"].ToString());
            //                DrawingProperty.Add("DrawingName", rw["DrawingName"].ToString());
            //                DrawingProperty.Add("Classification", rw["Classification"].ToString());
            //                DrawingProperty.Add("DrawingNumber", rw["DrawingNumber"].ToString());
            //                DrawingProperty.Add("DrawingState", rw["DrawingState"].ToString());
            //                DrawingProperty.Add("Revision", rw["Revision"].ToString());
            //                DrawingProperty.Add("LockStatus", rw["LockStatus"].ToString());
            //                DrawingProperty.Add("Generation", rw["Generation"].ToString());
            //                DrawingProperty.Add("Type", rw["Type"].ToString());
            //                DrawingProperty.Add("ProjectName", rw["ProjectName"].ToString());
            //                DrawingProperty.Add("ProjectId", rw["ProjectId"].ToString());
            //                DrawingProperty.Add("CreatedOn", rw["CreatedOn"].ToString());
            //                DrawingProperty.Add("CreatedBy", rw["CreatedBy"].ToString());
            //                DrawingProperty.Add("ModifiedOn", rw["ModifiedOn"].ToString());
            //                DrawingProperty.Add("ModifiedBy", rw["ModifiedBy"].ToString());
            //                String fileOpenPath = checkoutPath + "\\" + rw["NativeFileName"].ToString();
            //                cadManager.OpenActiveDocument(fileOpenPath, "View", DrawingProperty);
            //            }
            //            else
            //            {
            //                MessageBox.Show("File not Found at --> " + rw["DrawingName"].ToString());
            //            }
            //        }
            //        else
            //        {
            //            MessageBox.Show(rw["Error"].ToString());
            //        }
            //    }

            //    CADRibbon ribbon = new CADRibbon();
            //    ribbon.browseDEnable = true;
            //    ribbon.createDEnable = true;
            //    ribbon.browseBEnable = true;
            //    ribbon.createBEnable = true;
            //    ribbon.LockEnable = true;
            //    ribbon.SaveEnable = true;
            //    ribbon.UnlockEnable = true;
            //    ribbon.DrawingInfoEnable = true;
            //    ribbon.MyRibbon();
            //    this.Cursor = Cursors.Default;
            //    this.Close();

            //}
            //catch (System.Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
            //    this.Cursor = Cursors.Default;
            //    return;
            //}
            #endregion
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
                    MessageBox.Show("Please select at least one file to Open");
                    return;
                }

                if (string.IsNullOrEmpty(checkoutPath))
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show("Please set checkout path under settings, before opening any file.");
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

                    foreach (TreeGridNode childNode in currentTreeGrdiNode.Nodes)
                    {
                        DownloadOpenDocument(childNode.Cells["DrawingID"].FormattedValue.ToString(), childNode.Cells["DrawingName"].FormattedValue.ToString(), checkoutPath, "Checkout");
                    }

                    DownloadOpenDocument(currentTreeGrdiNode.Cells["DrawingID"].FormattedValue.ToString(), currentTreeGrdiNode.Cells["DrawingName"].FormattedValue.ToString(), checkoutPath, "Checkout", true, currentTreeGrdiNode);

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
                System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
                this.Cursor = Cursors.Default;
                return;
            }
            this.Cursor = Cursors.Default;
        }

        private void DownloadOpenDocument(string fileId, string fileName, string checkoutPath, string fileMode, bool isParentFile = false, TreeGridNode tgnParent=null)
        {
            try
            {
                //   fileId = "11760c31-d3fb-4acb-9675-551915493fd5";
                //RestResponse restResponse = (RestResponse)ServiceHelper.GetData(
                //    Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                //    "/AutocadFiles/downloadAutocadSingleFile",
                //    true,
                //    new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("fileId", fileId) });


                RestResponse restResponse = (RestResponse)ServiceHelper.GetData(
                                               Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                                               "/AutocadFiles/downloadAutocadSingleFile",
                                               true,
                                               new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("fileId", fileId )
                                               });
                if (restResponse.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(restResponse.Content))
                {
                    RBConnector objRBC = new RBConnector();
                    ResultSearchCriteria Drawing = objRBC.GetDrawingInformation(fileId);
                    Hashtable DrawingProperty = new Hashtable();

                    DrawingProperty.Add("DrawingId", Drawing.id);
                    DrawingProperty.Add("DrawingName", Drawing.name);
                    DrawingProperty.Add("Classification", Drawing.status.id);
                    DrawingProperty.Add("DrawingNumber", Drawing.fileNo);
                    DrawingProperty.Add("DrawingState", Drawing.status.statusname);
                    DrawingProperty.Add("Revision", Drawing.versionno);
                    DrawingProperty.Add("LockStatus", Drawing.filelock);
                    DrawingProperty.Add("Generation", "123");
                    DrawingProperty.Add("Type", Drawing.coreType.id);
                    DrawingProperty.Add("ProjectName", Drawing.projectname);
                    DrawingProperty.Add("ProjectId", Drawing.projectinfo);
                    DrawingProperty.Add("CreatedOn", Drawing.updatedon);
                    DrawingProperty.Add("CreatedBy", Drawing.createdby);
                    DrawingProperty.Add("ModifiedOn", Drawing.updatedon);
                    DrawingProperty.Add("ModifiedBy", Drawing.updatedby);
                    //DrawingProperty.Add("isroot", true);
                    //DrawingProperty.Add("sourceid","");
                    //DrawingProperty.Add("Layouts","");

                    // string filePathName = Path.Combine(checkoutPath, Helper.FileNamePrefix + "Drawing1.dwg");
                    if (fileName.Substring(0, Helper.FileNamePrefix.Length) == Helper.FileNamePrefix)
                    {
                        fileName = fileName.Substring(Helper.FileNamePrefix.Length);
                    }

                    string filePathName = Path.Combine(checkoutPath, fileName);

                    if (isParentFile)
                    {
                        filePathName = Path.Combine(checkoutPath, Helper.FileNamePrefix + fileName);
                    }

                    using (var binaryWriter = new BinaryWriter(File.Open(filePathName, FileMode.OpenOrCreate)))
                    {
                        binaryWriter.Write(restResponse.RawBytes);
                        //binaryWriter.Flush();
                        //binaryWriter.Close();
                        //binaryWriter.Dispose();
                    }

                    if (isParentFile)
                    {
                        cadManager.UpdateExRefPathInfo(filePathName);

                        foreach (TreeGridNode childNode in tgnParent.Nodes)
                        {
                            string oldFileName = Path.Combine(checkoutPath, Convert.ToString(childNode.Cells["DrawingName"].FormattedValue));
                            string newFileName = Path.Combine(checkoutPath, Helper.FileNamePrefix + Convert.ToString(childNode.Cells["DrawingName"].FormattedValue));
                            if (File.Exists(oldFileName))
                            {
                                File.Delete(newFileName); // Delete the existing file if exists
                                File.Move(oldFileName, newFileName); // Rename the oldFileName into newFileName
                            }
                        }
                        cadManager.OpenActiveDocument(filePathName, "View", DrawingProperty);
                    }
                    else
                    {
                        // Code pending of Updating XrefFile Attributes
                        cadManager.SetAttributesXrefFiles(DrawingProperty, filePathName);
                        cadManager.UpdateLayoutAttributeArefFile(DrawingProperty, filePathName);
                        //cadManager.SetAttributes(DrawingProperty);
                        //cadManager.UpdateLayoutAttribute1(DrawingProperty);
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
            this.Hide();
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
            // MessageBox.Show("Collapsed clicked");

            TreeGridNode TreeNode1 = (TreeGridNode)e.Node;

            TreeNode1.Cells[1].Value = "+";
            treeGridView1.Show();
        }

        private void treeGridView1_NodeExpanding(object sender, ExpandingEventArgs e)
        {
            //  MessageBox.Show("NodeExpanding clicked");

            TreeGridNode TreeNode1 = (TreeGridNode)e.Node;

            TreeNode1.Cells[1].Value = "-";
            treeGridView1.Show();
        }

        private void CheckOutViewCBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CheckOutViewCBox.Text == "Released" || CheckOutViewCBox.Text == "AsSaved")
            {
                CDRevisionValue.Enabled = true;
            }
            else
            {
                CDRevisionValue.Enabled = false;
            }
        }

        private void Search_And_Open_Resize(object sender, EventArgs e)
        {
            //int f_height = this.Height;
            //int f_width = this.Width;
            //OpenDrawingButton.Location = new Point((f_width / 2) - 100, f_height - 85);
            //FormCancelButton.Location = new Point((f_width / 2) + 100, f_height - 85);
            //searchStatus.Location = new Point(20, f_height - 85);
            //busyLabel.Location = new Point(20, f_height - 85);
        }

        private void sg_SearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sg_SearchType.Text == "Full")
            {
                CDRealty.Enabled = true;
            }
            else
            {
                CDRealty.Text = "";
                CDRealty.Enabled = false;
            }
        }

        private void CDProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //////Build datatable for Realty Entity and Set Value in Dropdown
            ////CADIntegrationConfiguration objWordConfig = new CADIntegrationConfiguration();
            ////DataTable dtRealtyNo = new DataTable();
            ////dtRealtyNo.Columns.Add("ProjectId", typeof(string));
            ////dtRealtyNo.Columns.Add("RealtyName", typeof(string));
            ////dtRealtyNo.Columns.Add("RealtyNo", typeof(string));
            ////dtRealtyNo = objWordConfig.GetRealtyEntity();
            ////String sg_projectid = CDProjectName.SelectedValue.ToString();
            ////DataView Realty = new DataView(dtRealtyNo, "ProjectId='" + sg_projectid + "'", "RealtyName", DataViewRowState.CurrentRows);
            ////Realty.AddNew();
            ////CDRealty.DataSource = Realty;
            ////CDRealty.DisplayMember = "RealtyName";
            ////CDRealty.ValueMember = "RealtyNo";
            ////CDRealty.SelectedValue = "";
        }

        private void CDType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //////Build datatable for Lifecycle State and Set Value in Dropdown
            ////CADIntegrationConfiguration objWordConfig = new CADIntegrationConfiguration();
            ////DataTable lcState = new DataTable();
            ////lcState.Columns.Add("Classification", typeof(string));
            ////lcState.Columns.Add("StateName", typeof(string));
            ////lcState.Columns.Add("StateLabel", typeof(string));
            ////lcState = objWordConfig.GetLifeCycleState();

            ////String classificationValue = CDType.SelectedValue.ToString();
            ////DataView LCState = new DataView(lcState, "Classification='" + classificationValue + "'", "", DataViewRowState.CurrentRows);
            ////if (LCState.Count < 1) LCState = new DataView(lcState, "Classification=''", "", DataViewRowState.CurrentRows);
            ////LCState.AddNew();
            ////CDState.DataSource = LCState;
            ////CDState.DisplayMember = "StateLabel";
            ////CDState.ValueMember = "StateName";
            ////CDState.SelectedValue = "";
        }

        private void textBox_foldername_TextChanged(object sender, EventArgs e)
        {

        }

        private void label_foldername_Click(object sender, EventArgs e)
        {

        }

        private void CDState_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Doc_name_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void CDProjectName_KeyUp(object sender, KeyEventArgs e)
        {
            comboBoxSearch();
        }

        /// <summary>
        /// Start searching the text box with the result list.
        /// </summary>
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
    }
}
