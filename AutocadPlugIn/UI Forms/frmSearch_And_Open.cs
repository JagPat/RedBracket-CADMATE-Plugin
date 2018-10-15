using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AdvancedDataGridView;

using RBAutocadPlugIn.Properties;

namespace RBAutocadPlugIn.UI_Forms
{
    public partial class frmSearch_And_Open : Form
    {
        public ArrayList drawings = new ArrayList();
        public ArrayList drawingsOpen = new ArrayList();
        public ArrayList OpenMode = new ArrayList();
        public ArrayList OpenMode1 = new ArrayList();
    
        public Dictionary<string, string> projectNameNumberKeyValiuePairList = new Dictionary<string, string>();
        string CheckoutExpandAllEnabled = Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutExpandAllEnabled"));
        string checkoutPath = Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath"));
      
        RBConnector objRBC = new RBConnector();
        public List<string> DownloadedFiles = new List<string>();
        public List<clsDownloadedFiles> lstobjDownloadedFiles = new List<clsDownloadedFiles>();
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
                treeGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                treeGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                IsXRefFile.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                IsXRefFile.FillWeight = 1;
                IsXRefFile.Width = 40;
                treeGridView1.BackgroundColor = Helper.FormBGColor;
                CancelButton.UseVisualStyleBackColor = SearchButton.UseVisualStyleBackColor = FormCancelButton.UseVisualStyleBackColor = OpenDrawingButton.UseVisualStyleBackColor = false;
                this.BackColor = Helper.FormBGColor;
                treeGridView1.RowsDefaultCellStyle.BackColor = Helper.FormBGColor;
                sg_SearchType.SelectedIndex = 0;
                DGName.BackColor= textBox_foldername.BackColor= CDProjectName.BackColor = Helper.FormBGColor;

                //Read the keys from the user registry and load it to the UI.





                #region filetype


                Helper.FIllCMB(CDType, objRBC.GetFIleType(), "name", "name", false);
                #endregion filetype

                #region filestatus

                Helper.FIllCMB(CDState, objRBC.GetFIleStatus(), "statusname", "statusname", false, false);

                #endregion filestatus

                #region projectdetails
                Helper.FIllCMB(CDProjectName, objRBC.GetProjectDetail(), "PNAMENO", "id", false);

                #endregion projectdetails


                List<ResultSearchCriteria> resultSearchCriteriaResponseList = objRBC.SearchLatest5File();
                if (resultSearchCriteriaResponseList != null)
                    BindDataToGrid(resultSearchCriteriaResponseList);



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
                 urlParameters.Add(new KeyValuePair<string, string>("projid", Convert.ToString(CDProjectName.SelectedValue)));
            }

            // If folder name is not null or empty then add the parameter to the URL.
            if (sg_SearchType.SelectedIndex > 0)
            {
                string Location = sg_SearchType.Text;
                Location = Location == "Projects" ? "Project" : Location;
                urlParameters.Add(new KeyValuePair<string, string>("location", Location));
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
            if (CDProjectName.SelectedIndex > 0)
            {

            }

            if (objRBC.GetSearchFileCount(searchCriteria,urlParameters) > 50)
            {
                ShowMessage.ErrorMessUD("Search yields more than 50 records. Please add specific search criteria.");
                return;
            }

            var resultSearchCriteriaResponseList = objRBC.SearchFiles(searchCriteria, urlParameters);
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

            //resultSearchCriteriaResponseList = (from resultSearchCriteriaResponse in resultSearchCriteriaResponseList
            //                                    where resultSearchCriteriaResponse.name.EndsWith("dwg")
            //                                    select resultSearchCriteriaResponse).ToList();

            if (isSortingRequired)
            {
                resultSearchCriteriaResponseList = resultSearchCriteriaResponseList.OrderBy(resultSearchCriteriaResponse => resultSearchCriteriaResponse.name).ToList();
            }

            if (resultSearchCriteriaResponseList.Count > 50)
            {
                ShowMessage.ErrorMessUD("Search yields more than 50 records. Please add specific search criteria.");
                return;
            }

            foreach (ResultSearchCriteria resultSearchCriteriaRecord in resultSearchCriteriaResponseList)
            {


                TreeGridNode treeGridNode = treeGridView1.Nodes.Add(
                    null,
                    null,
                     resultSearchCriteriaRecord.name,
                    Resources.BG,
                    //resultSearchCriteriaRecord.name == null ? new Bitmap(1, 1) : resultSearchCriteriaRecord.name.ToLowerInvariant().EndsWith("dwg", StringComparison.eriaRecord.name.ToLowerInvariant().EndsWith("dwg", StringComparison.InvariantCulture) ? new Bitmap(1, 1) :InvariantCulture) ? new Bitmap(1, 1) : Resources.ReferenceImage,
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

            //var childRecords = objRBC.GetXrefFIleInfo(resultSearchCriteriaRecord.id);
            var childRecords = resultSearchCriteriaRecord.filebean;

            if (childRecords == null || childRecords.Length <= 0)
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
                                                  resultSearchCriteriaChildRecord.name == null ? Resources.BG : resultSearchCriteriaChildRecord.name.ToLowerInvariant().EndsWith("dwg", StringComparison.InvariantCulture) ? Resources.Xref1 : Resources.BG,

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

                AddChildNode(resultSearchCriteriaChildRecord, ref treeGridNode);
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
                ShowMessage.ErrorMess("Exception Occur: " + ex);
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
                ShowMessage.ErrorMess("Exception Occur: " + ex);
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
                ShowMessage.ErrorMess("Exception Occur: " + ex);
                return;
            }
        }

        List<TreeGridNode> selectedTreeGridNodes = new List<TreeGridNode>();
        private void OpenDrawingButton_Click(object sender, EventArgs e)
        {

            List<PLMObject> pLMObjects = new List<PLMObject>();
            RBConnector objRBC = new RBConnector();
            int PBValue = 1;
            try
            {
                this.Cursor = Cursors.WaitCursor;

                foreach (TreeGridNode treeGridNode in treeGridView1.Nodes)
                {
                    if ((bool)treeGridNode.Cells[0].FormattedValue)
                    {
                        selectedTreeGridNodes.Add(treeGridNode);
                        PBValue++;
                        Helper.GetSellectedNode(treeGridNode, ref PBValue);
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
                Helper.GetProgressBar(PBValue, "File Download in Progress...");
                foreach (TreeGridNode currentTreeGrdiNode in selectedTreeGridNodes)
                {
                    DownloadChild(currentTreeGrdiNode);

                    Helper.IncrementProgressBar(1, "Downloading file." + System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(currentTreeGrdiNode.Cells["DrawingName"].FormattedValue)));
                    string FileID = currentTreeGrdiNode.Cells["DrawingID"].FormattedValue.ToString();
                    Helper.DownloadFile(FileID, "true", DownloadedFiles: DownloadedFiles, lstobjDownloadedFiles: lstobjDownloadedFiles);

                    PLMObject objplmo = new PLMObject();
                    objplmo.ObjectId = FileID;
                    pLMObjects.Add(objplmo);

                    break;
                }
                Helper.IncrementProgressBar(1, "Locking files.");
                objRBC.LockObject(pLMObjects);

                Helper.IncrementProgressBar(PBValue, "Finishing Download.");
                CADRibbon ribbon = new CADRibbon();
                ribbon.browseDEnable = true;
                ribbon.createDEnable = true;
                ribbon.browseBEnable = true;
                ribbon.createBEnable = true;
                ribbon.LockEnable = true;
                ribbon.SaveEnable = true;
                ribbon.UnlockEnable = true;
                ribbon.DrawingInfoEnable = true;
                ribbon.RBRibbon();
                Helper.CloseProgressBar();
                this.Cursor = Cursors.Default;
                this.Close();
            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess("Exception Occur: " + ex);
                Helper.CloseProgressBar();
                objRBC.UnlockObject(pLMObjects);
                this.Cursor = Cursors.Default;
                return;
            }
            this.Cursor = Cursors.Default;
        }

        public void DownloadChild(TreeGridNode ParentNode, string ParentFilePath = "")
        {
            try
            {
                foreach (TreeGridNode childNode in ParentNode.Nodes)
                {
                    if ((bool)childNode.Cells[0].FormattedValue)
                    {
                        Helper.IncrementProgressBar(1, "Downloading file." + System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(childNode.Cells["DrawingName"].FormattedValue)));
                        string FP = Helper.DownloadFile(Convert.ToString(childNode.Cells["DrawingID"].FormattedValue), DownloadedFiles: DownloadedFiles, ParentFilePath: ParentFilePath, lstobjDownloadedFiles: lstobjDownloadedFiles);
                        if (FP != null)
                        {
                            DownloadedFiles.Add(FP);
                            DownloadChild(childNode, FP);
                        }

                        // pLMObjects.Add(new PLMObject() { ObjectId = childNode.Cells["DrawingID"].FormattedValue.ToString() });
                    }
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
                        ShowMessage.ErrorMessUD("The Item " + TreeNode1.Cells[2].Value.ToString() + " is already Locked by " + TreeNode1.Cells[9].Value.ToString());
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
                        ShowMessage.ErrorMessUD("The Item " + TreeNode1.Cells[2].Value.ToString() + " is already Locked by you." + TreeNode1.Cells[9].Value.ToString());
                        TreeNode1.Cells[14].Value = false;
                    }
                    else if (sg_lockStatus == "2")
                    {
                        ShowMessage.ErrorMessUD("The Item " + TreeNode1.Cells[2].Value.ToString() + " is already Locked by " + TreeNode1.Cells[9].Value.ToString() + ".");
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
                CDProjectName_SelectedIndexChanged(null, null);
            }
            catch
            {

            }
        }














        private void CDProjectName_KeyUp(object sender, KeyEventArgs e)
        {
            //comboBoxSearch();
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

        private void CDProjectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if ((sg_SearchType.SelectedIndex == 0 || sg_SearchType.SelectedIndex == 2))
                {
                    if (CDProjectName.SelectedIndex == 0)
                    {
                        textBox_foldername.Enabled = false;
                    }
                    else if (CDProjectName.SelectedIndex > 0)
                    {
                        textBox_foldername.Enabled = true;
                    }
                    else
                    {
                        textBox_foldername.Enabled = true;
                    }
                }
                else
                {
                    textBox_foldername.Enabled = true;
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

        }

        private void textBox_foldername_EnabledChanged(object sender, EventArgs e)
        {
            if (!textBox_foldername.Enabled)
            {
                textBox_foldername.Clear();
            }
        }
    }
}
