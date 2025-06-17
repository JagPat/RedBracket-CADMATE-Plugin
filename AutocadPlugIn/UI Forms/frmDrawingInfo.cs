using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RBAutocadPlugIn.UI_Forms
{
    public partial class frmDrawingInfo : Form
    {
        //Public scope variable declaration and initialization
        public string DrawingID1 = ""; 
        RBConnector objRBC = new RBConnector();
        bool IsFilePropertiesChanged = false;
        bool IsLayoutPropertiesChanged = false;
        bool LoadFlag = false;
        string FileID = "";
        string ProjectID = "";
        string FilePath = "";
        string PreFix = "";
        bool IsLayoutinLocalFile = false;
        string FolderID = "";
        public frmDrawingInfo()
        {
            InitializeComponent(); this.FormBorderStyle = FormBorderStyle.None;
        }

        private void frmDrawingInfo_Load(object sender, EventArgs e)
        {
            LoadFlag = true;
            try
            {
                //hiding this form and showing progress form so that user have clear idea that data in form is being loaded
                Cursor.Current = Cursors.WaitCursor;
                this.Hide();

                int LocalFileLayoutCount = 0;
                int FileInfoRowCount = 0;

 
                Helper.GetProgressBar(5, "Fetching File Info in Progress...", "Fetching local file info.");
                //declaring a panelsaperator
                Panel pnlSaperator = new Panel() { BackColor = Helper.clrParentPopupBorderColor, Margin = new Padding(3), Dock = DockStyle.Fill };
                // Get Current File Info from Custum Properties and Display
                DataRow[] dtCurrentData =Helper.cadManager.GetDrawingExternalRefreces().Select("isroot=true");
                if (dtCurrentData.Length > 0)
                {
                    Helper.IncrementProgressBar(1, "Filling local file info.");
                    FilePath = Convert.ToString(dtCurrentData[0]["filepath"]);
                    PreFix = Convert.ToString(dtCurrentData[0]["prefix"]);
                   

                    //Filling combo boxes
                    Helper.FIllCMB(cmbFileTypeC, objRBC.GetFIleType(), "name", "id", true);
                    Helper.FIllCMB(cmbLayoutTypeC1, objRBC.GetFIleType(), "name", "id", true);
                    Helper.FIllCMB(cmbLayoutTypeC2, objRBC.GetFIleType(), "name", "id", true);

                    Helper.FIllCMB(cmbFileStatusC, objRBC.GetFIleStatus(), "statusname", "id", true, false);
                    Helper.FIllCMB(cmbLayoutStatusC1, objRBC.GetFIleStatus(), "statusname", "id", true, false);
                    Helper.FIllCMB(cmbLayoutStatusC2, objRBC.GetFIleStatus(), "statusname", "id", true, false);
                    FileID = lbDrawingIDC.Text = DrawingID1 = Convert.ToString(dtCurrentData[0]["drawingid"]);
                    FolderID =    Convert.ToString(dtCurrentData[0]["folderid"]);
                    if (DrawingID1.Trim().Length == 0)
                    {
                        ShowMessage.ValMess("Please save this drawing to RedBracket."); this.Close(); Cursor.Current = Cursors.Default;
                        return;
                    }
                    lbDrawingNameC.Text = Convert.ToString(dtCurrentData[0]["drawingname"]);

                    lbDrawingNoC.Text = Convert.ToString(dtCurrentData[0]["drawingnumber"]);
                    lbVersionC.Text = Convert.ToString(dtCurrentData[0]["revision"]);
                    lbProjectNameC.Text = Convert.ToString(dtCurrentData[0]["projectname"]);

                    DataTable dtProjectDetail = objRBC.GetProjectDetail();
                    DataRow[] drl = dtProjectDetail.Select("name='" + lbProjectNameC.Text + "'");
                    if (drl.Length > 0)
                    {
                        ProjectID = Convert.ToString(drl[0]["id"]);
                    }
                    else
                    {
                        ProjectID = "";
                    }

                    lbProjectNoC.Text = Convert.ToString(dtCurrentData[0]["projectno"]);
                    string T = Convert.ToString(dtCurrentData[0]["Classification"]) == string.Empty ? "---Select---" : Convert.ToString(dtCurrentData[0]["Classification"]);
                    cmbFileTypeC.Tag = cmbFileTypeC.Text = T;
                    cmbFileStatusC.Tag = cmbFileStatusC.Text = Convert.ToString(dtCurrentData[0]["drawingstate"]) == string.Empty ? "---Select---" : Convert.ToString(dtCurrentData[0]["drawingstate"]);
                    lbDrawingIDC.Text = DrawingID1 = Convert.ToString(dtCurrentData[0]["drawingid"]);

                    lbCreatedByC.Text = Convert.ToString(dtCurrentData[0]["createdby"]) + " (" + Helper.FormatDateTime(dtCurrentData[0]["createdon"]) + ")";
                    lbModifiedByC.Text = Convert.ToString(dtCurrentData[0]["modifiedby"]) + " (" + Helper.FormatDateTime(dtCurrentData[0]["modifiedon"]) + ")";



                    string LayoutInfo1 = Convert.ToString(dtCurrentData[0]["layoutinfo"]);
                    if (LayoutInfo1.Trim().Length > 0)
                    {
                        #region Layout Info
                        List<LayoutInfo> objLI = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LayoutInfo>>(LayoutInfo1);

                        int count = 0;

                        foreach (LayoutInfo objLI1 in objLI)
                        {
                            IsLayoutinLocalFile = true;
                            count++;

                            if (count == 1)
                            {
                                lbLayoutNameC1.Text = Convert.ToString(objLI1.name);
                                lbLayoutNameC1.Tag = objLI1.id;
                                lbLayoutNoC1.Text = Convert.ToString(objLI1.fileNo);
                                lbLayoutNoC1.Tag = objLI1.description;
                                lbLayoutVersionC1.Text = Convert.ToString(objLI1.versionNo);
                                object o = objLI1.statusId == null || objLI1.statusId == string.Empty ? 0 : Convert.ToInt16(objLI1.statusId);
                                cmbLayoutStatusC1.Tag = cmbLayoutStatusC1.SelectedValue = objLI1.statusId == null || objLI1.statusId == string.Empty || objLI1.statusId == "0" ? -1 : Convert.ToInt16(objLI1.statusId);
                                cmbLayoutTypeC1.Tag = cmbLayoutTypeC1.SelectedValue = objLI1.typeId == null || objLI1.typeId == string.Empty || objLI1.typeId == "0" ? -1 : Convert.ToInt16(objLI1.typeId);
                            }
                            else if (count == 2)
                            {
                                lbLayoutNameC2.Text = Convert.ToString(objLI1.name);
                                lbLayoutNameC2.Tag = objLI1.id;
                                lbLayoutNoC2.Text = Convert.ToString(objLI1.fileNo);
                                lbLayoutNoC2.Tag = objLI1.description;
                                lbLayoutVersionC2.Text = Convert.ToString(objLI1.versionNo);
                                cmbLayoutStatusC2.Tag = cmbLayoutStatusC2.SelectedValue = objLI1.statusId == null || objLI1.statusId == string.Empty || objLI1.statusId == "0" ? -1 : Convert.ToInt16(objLI1.statusId);
                                cmbLayoutTypeC2.Tag = cmbLayoutTypeC2.SelectedValue = objLI1.typeId == null || objLI1.typeId == string.Empty || objLI1.typeId == "0" ? -1 : Convert.ToInt16(objLI1.typeId);

                                FileInfoRowCount = tlpMain.RowCount;
                            }
                            else
                            {
                                Font font = label14.Font;
                                Font font1 = cmbFileTypeC.Font;
                                int CRC = tlpMain.RowCount;
                                if (count == 3)
                                {
                                    FileInfoRowCount = CRC;
                                }
                                //adding new rows in panel for layout info when there is more than 2 layout.
                                tlpMain.RowCount += 6;
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 14));

                                //adding controls for layout info
                                Panel pnl = pnlSaperator;
                                tlpMain.Controls.Add(new Label() { Text = "Layout Name", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = "Layout No", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = "Layout Version", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = "Layout Status", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = "Layout Discipline", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(pnl, 0, CRC++);
                                tlpMain.SetColumnSpan(pnl, 3);

                                CRC -= 6;
                                tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.name), Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, Name = "lbLayoutNameC" + count, Tag = objLI1.id, AutoEllipsis = true }, 1, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.fileNo), Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, Name = "lbLayoutNoC" + count, Tag = objLI1.description, AutoEllipsis = true }, 1, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.versionNo), Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, AutoEllipsis = true }, 1, CRC++);
                                tlpMain.Controls.Add(new ComboBox() { Font = font1, Name = "cmbLayoutStatusC" + count, Dock = DockStyle.Fill, Margin = new Padding(0) }, 1, CRC++);
                                tlpMain.Controls.Add(new ComboBox() { Font = font1, Name = "cmbLayoutTypeC" + count, Dock = DockStyle.Fill, Margin = new Padding(0) }, 1, CRC++);

                                ComboBox combo1 = tlpMain.Controls.Find("cmbLayoutStatusC" + count, true).FirstOrDefault() as ComboBox;

                                if (combo1 != null)
                                {
                                    Helper.FIllCMB(combo1, objRBC.GetFIleStatus(), "statusname", "id", true, false);
                                    combo1.Tag = combo1.SelectedValue = objLI1.statusId == null || objLI1.statusId == string.Empty || objLI1.statusId == "0" ? -1 : Convert.ToInt16(objLI1.statusId);
                                    combo1.SelectedValueChanged += new System.EventHandler(cmbFileTypeC_SelectedValueChanged);
                                }

                                ComboBox combo2 = tlpMain.Controls.Find("cmbLayoutTypeC" + count, true).FirstOrDefault() as ComboBox;

                                if (combo2 != null)
                                {
                                    Helper.FIllCMB(combo2, objRBC.GetFIleType(), "name", "id", true);
                                    combo2.Tag = combo2.SelectedValue = objLI1.typeId == null || objLI1.typeId == string.Empty || objLI1.typeId == "0" ? -1 : Convert.ToInt16(objLI1.typeId);
                                    combo2.SelectedValueChanged += new System.EventHandler(cmbFileTypeC_SelectedValueChanged);
                                }
                            }
                        }

                        LocalFileLayoutCount = count;
                        #endregion
                    }




                }
                else
                {
                    Helper.CloseProgressBar();
                    ShowMessage.ValMess("Please save this drawing to RedBracket."); this.Close(); Cursor.Current = Cursors.Default;
                    return;
                }
                // Get Latest FIle Info from RB and Display
                Helper.IncrementProgressBar(1, "Fetching latest file info from server.");
                byte[] DBFile = null;
                string LatestDrawingID = objRBC.SearchLatestFile(lbDrawingNoC.Text);
                ResultSearchCriteria objRSC = objRBC.GetDrawingInformation(LatestDrawingID);
                //ResultSearchCriteria objRSC = objRBC.GetSingleFileInfo(objRBC.SearchLatestFile(lbDrawingNoC.Text), ref DBFile);

                string TempFilePath = "";
                if (objRSC != null)
                {
                    //TempFilePath = Helper.DownloadFile(LatestDrawingID, "true", true);
                    Helper.IncrementProgressBar(1, "Filling Latest file info.");
                    lbDrawingNameL.Text = Convert.ToString(objRSC.name);

                    lbDrawingNoL.Text = Convert.ToString(objRSC.fileNo);
                    lbVersionL.Text = Convert.ToString(objRSC.versionno);
                    lbProjectNameL.Text = Convert.ToString(objRSC.projectname);
                    lbProjectNoL.Text = Convert.ToString(objRSC.projectNumber);
                    lbFileTypeL.Text = Convert.ToString(objRSC.type == null ? string.Empty : objRSC.type.name == null ? string.Empty : objRSC.type.name);
                    lbFileStatusL.Text = Convert.ToString(objRSC.status == null ? string.Empty : objRSC.status.statusname == null ? string.Empty : objRSC.status.statusname);
                    lbDrawingIDL.Text = DrawingID1 = Convert.ToString(objRSC.id);

                    lbCreatedByL.Text = Convert.ToString(objRSC.createdby) + " (" + Helper.FormatDateTime(objRSC.created0n) + ")";
                    lbModifiedByL.Text = Convert.ToString(objRSC.updatedby) + " (" + Helper.FormatDateTime(objRSC.updatedon) + ")";
                    if (Convert.ToBoolean(objRSC.filelock))
                    {
                        lbLockedByL.Text = Convert.ToString(objRSC.updatedby);
                        if (objRSC.updatedby == Helper.UserFullName && lbVersionC.Text == lbVersionL.Text)
                        {
                            lbLockedByC.Text = Convert.ToString(objRSC.updatedby);
                        }
                    }

                    List<LayoutInfo> objLI2 = Helper.SortLayoutInfo(objRSC.fileLayout);
                    int count1 = 0;
                    int count2 = 0;
                    foreach (LayoutInfo objLI1 in objLI2)
                    {

                        string LLName = objLI1.fileNo;
                        bool IsLayoutFound = false;
                        for (int i = 1; i <= LocalFileLayoutCount; i++)
                        {
                            string CLName = "";
                            if (i == 1)
                            {
                                CLName = lbLayoutNoC1.Text;
                            }
                            else if (i == 2)
                            {
                                CLName = lbLayoutNoC2.Text;
                            }
                            else
                            {
                                Label lblLayoutName = tlpMain.Controls.Find("lbLayoutNoC" + i, true).FirstOrDefault() as Label;

                                if (lblLayoutName != null)
                                {
                                    CLName = lblLayoutName.Text;
                                }
                            }
                            if (LLName == CLName)
                            {
                                IsLayoutFound = true;
                                count1 = i;
                                break;
                            }

                        }
                        if (IsLayoutFound)
                        {

                        }
                        else
                        {
                            count1 = count2 + 1;
                        }
                        //count1++;

                        if (count1 == 1)
                        {
                            lbLayoutNameL1.Text = Convert.ToString(objLI1.name);
                            lbLayoutNoL1.Text = Convert.ToString(objLI1.fileNo);
                            lbLayoutVersionL1.Text = Convert.ToString(objLI1.versionNo);
                            lbLayoutStatusL1.Text = Convert.ToString(objLI1.status == null ? string.Empty : objLI1.status.statusname == null ? string.Empty : objLI1.status.statusname);

                            lbLayoutTypeL1.Text = Convert.ToString(objLI1.type == null ? string.Empty : objLI1.type.name == null ? string.Empty : objLI1.type.name);

                        }
                        else if (count1 == 2)
                        {
                            lbLayoutNameL2.Text = Convert.ToString(objLI1.name);
                            lbLayoutNoL2.Text = Convert.ToString(objLI1.fileNo);
                            lbLayoutVersionL2.Text = Convert.ToString(objLI1.versionNo);
                            lbLayoutStatusL2.Text = Convert.ToString(objLI1.status == null ? string.Empty : objLI1.status.statusname == null ? string.Empty : objLI1.status.statusname);
                            lbLayoutTypeL2.Text = Convert.ToString(objLI1.type == null ? string.Empty : objLI1.type.name == null ? string.Empty : objLI1.type.name);
                        }
                        else
                        {

                            int CRC = FileInfoRowCount + (6 * (count1 - 3));
                            Font font = label14.Font;
                            Font font1 = lbLayoutNameC1.Font;
                            if (count1 > LocalFileLayoutCount)
                            {
                                Panel pnl = pnlSaperator;
                                //CRC = tlpMain.RowCount;
                                tlpMain.RowCount += 6;
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 23));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 14));



                                tlpMain.Controls.Add(new Label() { Text = "Layout Name", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = "Layout No", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = "Layout Version", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = "Layout Status", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = "Layout Discipline", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(pnl, 0, CRC++);
                                tlpMain.SetColumnSpan(pnl, 3);

                                CRC -= 6;
                            }

                            tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.name), Font = font1, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, AutoEllipsis = true }, 2, CRC++);
                            tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.fileNo), Font = font1, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, AutoEllipsis = true }, 2, CRC++);
                            tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.versionNo), Font = font1, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, AutoEllipsis = true }, 2, CRC++);
                            tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.status == null ? string.Empty : objLI1.status.statusname == null ? string.Empty : objLI1.status.statusname), Font = font1, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, Name = "lbLayoutStatusL" + count1, AutoEllipsis = true }, 2, CRC++);
                            tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.type == null ? string.Empty : objLI1.type.name == null ? string.Empty : objLI1.type.name), Font = font1, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, Name = "lbLayoutTypeL" + count1, AutoEllipsis = true }, 2, CRC++);


                            //tlpMain.Controls.Add(new ComboBox() { Font = font, Name = "cmbLayoutStatusC" + count1, Dock = DockStyle.Fill }, 2, CRC++);
                            //tlpMain.Controls.Add(new ComboBox() { Font = font, Name = "cmbLayoutTypeC" + count1, Dock = DockStyle.Fill }, 2, CRC++);

                            //ComboBox combo1 = tlpMain.Controls.Find("cmbLayoutStatusC" + count1, true).FirstOrDefault() as ComboBox;

                            //if (combo1 != null)
                            //{
                            //    Helper.FIllCMB(combo1, objRBC.GetFIleStatus(), "statusname", "id", true);
                            //    combo1.SelectedValue = objLI1.statusId == null ? 0 : Convert.ToInt16(objLI1.statusId);
                            //}

                            //ComboBox combo2 = tlpMain.Controls.Find("cmbLayoutTypeC" + count1, true).FirstOrDefault() as ComboBox;

                            //if (combo2 != null)
                            //{
                            //    Helper.FIllCMB(combo2, objRBC.GetFIleType(), "name", "id", true);
                            //    combo2.SelectedValue = objLI1.typeId == null ? 0 : Convert.ToInt16(objLI1.typeId);
                            //}

                        }
                        count2++;
                    }
                }
                else
                {
                    //Helper.CloseProgressBar();
                    ShowMessage.ValMess("Latest file not found on server.");
                    //this.Close();
                    //return;
                }
                Helper.IncrementProgressBar(1, "Generating form layout.");
                tlpMain.RowCount += 1;
                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));

                TableLayoutPanel tlpSave = new TableLayoutPanel();

                tlpSave.ColumnCount = 4;
                tlpSave.RowCount = 1;
                tlpSave.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                tlpSave.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
                tlpSave.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
                tlpSave.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                tlpSave.RowStyles.Add(new RowStyle(SizeType.Absolute, 41));

                tlpSave.Controls.Add(btnSave, 1, 0);
                btnSave.Visible = true;
                btnSave.Dock = DockStyle.Fill;
                btnSave.Font = new Font(label2.Font.FontFamily, label2.Font.Size, FontStyle.Bold);
                btnSave.Margin = new Padding(5);

                tlpSave.Controls.Add(btnCancel, 1, 0);
                btnCancel.Visible = true;
                btnCancel.Dock = DockStyle.Fill;
                btnCancel.Font = new Font(label2.Font.FontFamily, label2.Font.Size, FontStyle.Bold);
                btnCancel.Margin = new Padding(5);

                tlpSave.Margin = new Padding(0);
                tlpMain.Controls.Add(tlpSave, 0, tlpMain.RowCount - 1);
                tlpMain.SetColumnSpan(tlpSave, 3);
                tlpSave.Dock = DockStyle.Fill;

                if (lbVersionC.Text != lbVersionL.Text)
                {
                    cmbFileStatusC.Enabled = false;
                    cmbFileTypeC.Enabled = false;
                    cmbLayoutTypeC1.Enabled = false;
                    cmbLayoutStatusC1.Enabled = false;
                    cmbLayoutTypeC2.Enabled = false;
                    cmbLayoutStatusC2.Enabled = false;



                    bool IsControlNull = false;

                    int count = 3;
                    while (!IsControlNull)
                    {
                        ComboBox cmbtype = tlpMain.Controls.Find("cmbLayoutTypeC" + count, false).FirstOrDefault() as ComboBox;
                        ComboBox cmbstatus = tlpMain.Controls.Find("cmbLayoutStatusC" + count, false).FirstOrDefault() as ComboBox;


                        if (cmbstatus == null || cmbtype == null)
                        {
                            IsControlNull = true;
                        }
                        else
                        {
                            cmbtype.Enabled = false;
                            cmbstatus.Enabled = false;
                        }
                        count++;
                    }
                }
                btnSave.Enabled = false;
                //if (System.IO.File.Exists(TempFilePath))
                //{
                //    Helper.IncrementProgressBar(1, "Comparing file info.");
                //    string TempFilePath1 = Helper.CopyTempFile(FilePath);
                //    //Comparing Files
                //    bool IsSame = Helper.FileCompare(TempFilePath, TempFilePath1);
                //    long LocalFile = Helper.GetFileSizeOnDisk(TempFilePath1);
                //    long ServerFile = Helper.GetFileSizeOnDisk(TempFilePath);
                //    if (LocalFile != ServerFile)
                //    {
                //        lbVersionC.Text = lbVersionC.Text + "+";
                //    }


                //}

                CompareProperties();






                Helper.IncrementProgressBar(1, "Finishing form layout.");
            }
            catch (Exception E)
            {
                Helper.CloseProgressBar();
                ShowMessage.ErrorMess(E.Message); this.Close(); return;
            }
            LoadFlag = false;
            this.Show();
            Cursor.Current = Cursors.Default;
            Helper.CloseProgressBar();
        }
        public void CompareProperties()
        {
            try
            {
                for (int i = 1; i < tlpMain.RowCount; i++)
                {
                    Control lblLabel = tlpMain.GetControlFromPosition(0, i);
                    Control ctrlCurrent = tlpMain.GetControlFromPosition(1, i);
                    Control lblLatest = tlpMain.GetControlFromPosition(2, i);


                    if (ctrlCurrent != null && lblLatest != null && lblLabel != null && lblLabel is Label)
                    {
                        CompareProperties(ctrlCurrent, lblLatest, lblLabel);
                    }
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message); this.Close(); return;
            }
        }
        public void CompareProperties(Control ctrlCurrent, Control lblLatest, Control lblLabel)
        {
            try
            {

                if (ctrlCurrent.Text != lblLatest.Text)
                {
                    if (ctrlCurrent.Text == "---Select---" && lblLatest.Text == string.Empty)
                    {

                    }
                    else
                    {
                        lblLabel.Text = " " + lblLabel.Text;
                        ctrlCurrent.Text = " " + ctrlCurrent.Text;
                        lblLatest.Text = " " + lblLatest.Text;
                        lblLabel.Margin = ctrlCurrent.Margin = lblLatest.Margin = new Padding(0);
                        lblLabel.BackColor = ctrlCurrent.BackColor = lblLatest.BackColor = Helper.clrDiffHighlighColor;
                    }

                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }



        private void tlpMain_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsDrawingPropertiesChanged = false;
                Cursor.Current = Cursors.WaitCursor;
                if (IsFilePropertiesChanged)
                {

                    if (objRBC.UpdateFileProperties(FileID, Convert.ToString(cmbFileTypeC.SelectedValue), Convert.ToString(cmbFileStatusC.SelectedValue), ProjectID, FolderID))
                    {
                        //cadManager.UpdateFileProperties( Convert.ToString(cmbFileTypeC.Text), Convert.ToString(cmbFileStatusC.Text), Convert.ToString(cmbFileTypeC.SelectedValue), Convert.ToString(cmbFileStatusC.SelectedValue));
                        cmbFileTypeC.Tag = cmbFileTypeC.SelectedValue;
                        cmbFileStatusC.Tag = cmbFileStatusC.SelectedValue;

                        lbFileTypeL.Text = Convert.ToString(cmbFileTypeC.SelectedValue) == "-1" ? string.Empty : cmbFileTypeC.Text;
                        lbFileStatusL.Text = Convert.ToString(cmbFileStatusC.SelectedValue) == "-1" ? string.Empty : cmbFileStatusC.Text;
                        IsDrawingPropertiesChanged = true;
                        ShowMessage.InfoMess("File properties updated successfully.");

                    }
                }
                if (IsLayoutPropertiesChanged && IsLayoutinLocalFile)
                {
                    bool IsControlNull = false;

                    int count = 1;
                    //DataTable dtLayoutInfo = new DataTable();
                    //dtLayoutInfo.Columns.Add("LayoutNo");
                    //dtLayoutInfo.Columns.Add("Status");
                    //dtLayoutInfo.Columns.Add("Type");
                    //dtLayoutInfo.Columns.Add("StatusID");
                    //dtLayoutInfo.Columns.Add("Type");
                    bool IsSave = false;
                    while (!IsControlNull)
                    {
                        bool IsValueChanged = false;
                        ComboBox cmbtype = tlpMain.Controls.Find("cmbLayoutTypeC" + count, false).FirstOrDefault() as ComboBox;
                        ComboBox cmbstatus = tlpMain.Controls.Find("cmbLayoutStatusC" + count, false).FirstOrDefault() as ComboBox;
                        Label lblLName = tlpMain.Controls.Find("lbLayoutNameC" + count, false).FirstOrDefault() as Label;

                        Label lblStatus = tlpMain.Controls.Find("lbLayoutStatusL" + count, false).FirstOrDefault() as Label;
                        Label lblType = tlpMain.Controls.Find("lbLayoutTypeL" + count, false).FirstOrDefault() as Label;
                        Label lblLNumber = tlpMain.Controls.Find("lbLayoutNoC" + count, false).FirstOrDefault() as Label;

                        if (cmbstatus == null || cmbtype == null || lblLName == null || lblStatus == null || lblType == null || lblLNumber == null)
                        {
                            IsControlNull = true;
                        }
                        else
                        {
                            if (Convert.ToString(cmbtype.SelectedValue) != Convert.ToString(cmbtype.Tag))
                            {
                                IsValueChanged = true;
                            }
                            if (Convert.ToString(cmbstatus.SelectedValue) != Convert.ToString(cmbstatus.Tag))
                            {
                                IsValueChanged = true;
                            }


                            if (IsValueChanged)
                            {
                                string TypeID = "", StatusID = "", LayoutID = "";
                                LayoutID = Convert.ToString(lblLName.Tag);
                                StatusID = Convert.ToString(cmbstatus.SelectedValue);
                                TypeID = Convert.ToString(cmbtype.SelectedValue);
                                string LayoutName = Convert.ToString(lblLName.Text);
                                string LayoutDesc = Convert.ToString(lblLNumber.Tag);
                                TypeID = TypeID == "-1" ? string.Empty : TypeID;
                                StatusID = StatusID == "-1" ? string.Empty : StatusID;
                                if (objRBC.UpdateLayoutInfo(ProjectID, FileID, LayoutID, StatusID, TypeID, LayoutName, LayoutDesc))
                                {

                                    cmbstatus.Tag = cmbstatus.SelectedValue;
                                    cmbtype.Tag = cmbtype.SelectedValue;

                                    lblType.Text = Convert.ToString(cmbtype.SelectedValue) == "-1" ? string.Empty : cmbtype.Text;
                                    lblStatus.Text = Convert.ToString(cmbstatus.SelectedValue) == "-1" ? string.Empty : cmbstatus.Text;
                                    IsSave = true;
                                    IsDrawingPropertiesChanged = true;
                                }
                                else
                                {
                                    IsSave = false;
                                }
                            }


                            count++;
                        }

                    }
                    if (IsSave)
                    {
                        ShowMessage.InfoMess("Layout properties updated successfully.");
                    }

                }
                if (IsDrawingPropertiesChanged)
                {
                    Helper.cadManager.UpdateFileProperties(DrawingID1, FilePath);
                }

                cmbFileTypeC_SelectedValueChanged(null, null);
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            CompareProperties();
            Cursor.Current = Cursors.Default;
        }

        private void cmbFileTypeC_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (LoadFlag)
                    return;

                bool IsValueChanged1 = CheckStatusChange(((ComboBox)sender), false);
                if (!IsValueChanged1)
                    return;
                bool IsValueChanged = false;
                IsFilePropertiesChanged = false;
                IsLayoutPropertiesChanged = false;

                if (Convert.ToString(cmbFileStatusC.Text) != Convert.ToString(cmbFileStatusC.Tag))
                {
                    IsValueChanged = true; IsFilePropertiesChanged = true;
                }
                if (Convert.ToString(cmbFileTypeC.Text) != Convert.ToString(cmbFileTypeC.Tag))
                {
                    IsValueChanged = true; IsFilePropertiesChanged = true;
                }
                if (Convert.ToString(cmbLayoutTypeC1.SelectedValue) != Convert.ToString(cmbLayoutTypeC1.Tag))
                {
                    IsLayoutPropertiesChanged = IsValueChanged = true;
                }
                if (Convert.ToString(cmbLayoutStatusC1.SelectedValue) != Convert.ToString(cmbLayoutStatusC1.Tag))
                {
                    IsLayoutPropertiesChanged = IsValueChanged = true;
                }
                if (Convert.ToString(cmbLayoutTypeC2.SelectedValue) != Convert.ToString(cmbLayoutTypeC2.Tag))
                {
                    IsLayoutPropertiesChanged = IsValueChanged = true;
                }
                if (Convert.ToString(cmbLayoutStatusC2.SelectedValue) != Convert.ToString(cmbLayoutStatusC2.Tag))
                {
                    IsLayoutPropertiesChanged = IsValueChanged = true;
                }

                bool IsControlNull = false;

                int count = 3;
                while (!IsControlNull)
                {
                    ComboBox cmbtype = tlpMain.Controls.Find("cmbLayoutTypeC" + count, false).FirstOrDefault() as ComboBox;
                    ComboBox cmbstatus = tlpMain.Controls.Find("cmbLayoutStatusC" + count, false).FirstOrDefault() as ComboBox;


                    if (cmbstatus == null || cmbtype == null)
                    {
                        IsControlNull = true;
                    }
                    else
                    {
                        if (Convert.ToString(cmbtype.SelectedValue) != Convert.ToString(cmbtype.Tag))
                        {
                            IsLayoutPropertiesChanged = IsValueChanged = true;
                        }
                        if (Convert.ToString(cmbstatus.SelectedValue) != Convert.ToString(cmbstatus.Tag))
                        {
                            IsLayoutPropertiesChanged = IsValueChanged = true;
                        }
                        count++;
                    }

                }




                btnSave.Enabled = IsValueChanged;

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbFileStatusC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }
        public bool CheckStatusChange(ComboBox cmb, bool ShowFileName = false)
        {
            LoadFlag = true;
            bool RetVal = true;
            try
            {
                //if (cmb.Name != "cmbFileStatusC")
                //{
                //    // write code to check 

                //    LoadFlag = false;
                //    return true;
                //}

                bool OldStatusTypeClosed = IsClosedStatus(cmb, true);
                bool NewStatusTypeCLosed = IsClosedStatus(cmb);
                string FileName = "";

                if (ShowFileName)
                {
                    FileName = lbDrawingNameC.Text + Environment.NewLine;
                }



                if (OldStatusTypeClosed && NewStatusTypeCLosed)
                {

                }
                else if (OldStatusTypeClosed && !NewStatusTypeCLosed)
                {
                    ShowMessage.ValMess(FileName + "Changing status from '" + GetFileStatus(cmb, true) + "' to '" + cmb.Text + "' will lead to creation of new revision, \n So you can not change it.");

                    cmb.SelectedValue = cmb.Tag;
                    RetVal = false;
                }
                if (!OldStatusTypeClosed && NewStatusTypeCLosed)
                {
                    if (cmb.Name == "cmbFileStatusC")
                    {
                        if (!IsAllLayoutClose())
                        {
                            ShowMessage.ValMess("You can not change status to '" + cmb.Text + "' unless all layouts are in Close state.");


                            cmb.Text = Convert.ToString(cmb.Tag);

                            LoadFlag = false;
                            return false;
                        }
                    }
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            LoadFlag = false;
            return RetVal;
        }
        public bool IsAllLayoutClose()
        {
            try
            {
                bool IsControlNull = false;
                bool IsAllLayoutClose = true;
                int count = 1;

                while (!IsControlNull)
                {

                    ComboBox cmbstatus = tlpMain.Controls.Find("cmbLayoutStatusC" + count, false).FirstOrDefault() as ComboBox;

                    Label Name = tlpMain.Controls.Find("lbLayoutNameC" + count, false).FirstOrDefault() as Label;
                    if (cmbstatus == null || Convert.ToString(Name.Tag).Trim().Length == 0)
                    {
                        IsControlNull = true;
                    }
                    else
                    {
                        IsAllLayoutClose = IsClosedStatus(cmbstatus, true);
                        if (!IsAllLayoutClose)
                        {
                            break;
                        }
                        count++;
                    }

                }

                return IsAllLayoutClose;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return false;
        }
        public bool IsClosedStatus(ComboBox cmb, bool IsOld = false)
        {
            try
            {
                if (cmb == null)
                {
                    return false;
                }
                DataTable dtStatus = objRBC.GetFIleStatus();

                string FileStatus = "";
                if (IsOld)
                {
                    FileStatus = Convert.ToString(cmb.Tag);
                }
                else
                {
                    FileStatus = Convert.ToString(cmb.Text);
                }
                if (dtStatus != null)
                {
                    DataRow[] dr1;
                    try
                    {
                        if (IsOld)
                        {
                            dr1 = dtStatus.Select("id = '" + FileStatus + "' and IsClosed ='True'");
                        }
                        else
                        {
                            dr1 = dtStatus.Select("statusname = '" + FileStatus + "' and IsClosed ='True'");
                        }
                    }
                    catch
                    {
                        dr1 = dtStatus.Select("statusname = '" + FileStatus + "' and IsClosed ='True'");
                    }

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

        public string GetFileStatus(ComboBox cmb, bool IsOld = false)
        {
            string FileStatus = "";
            try
            {
                DataTable dtStatus = objRBC.GetFIleStatus();


                if (IsOld)
                {
                    FileStatus = Convert.ToString(cmb.Tag);
                }
                else
                {
                    FileStatus = Convert.ToString(cmb.SelectedValue);
                }
                if (dtStatus != null)
                {

                    DataRow[] dr1 = dtStatus.Select("id = " + FileStatus + "  ");
                    if (dr1.Length > 0)
                    {
                        FileStatus = Convert.ToString(dr1[0]["statusname"]);
                    }
                    else
                    {
                        FileStatus = "";
                    }
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
