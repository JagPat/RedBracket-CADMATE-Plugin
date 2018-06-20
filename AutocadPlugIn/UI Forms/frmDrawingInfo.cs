using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RedBracketConnector;

namespace AutocadPlugIn.UI_Forms
{
    public partial class frmDrawingInfo : Form
    {
        public string DrawingID = "";
        AutoCADManager cadManager = new AutoCADManager();
        RBConnector objRBC = new RBConnector();
        public frmDrawingInfo()
        {
            InitializeComponent();
        }

        private void frmDrawingInfo_Load(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                int LocalFileLayoutCount = 0;
                int FileInfoRowCount = 0;
                Helper.FIllCMB(cmbFileTypeC, objRBC.GetFIleType(), "name", "id", true);
                Helper.FIllCMB(cmbLayoutTypeC1, objRBC.GetFIleType(), "name", "id", true);
                Helper.FIllCMB(cmbLayoutTypeC2, objRBC.GetFIleType(), "name", "id", true);

                Helper.FIllCMB(cmbFileStatusC, objRBC.GetFIleStatus(), "statusname", "id", true);
                Helper.FIllCMB(cmbLayoutStatusC1, objRBC.GetFIleStatus(), "statusname", "id", true);
                Helper.FIllCMB(cmbLayoutStatusC2, objRBC.GetFIleStatus(), "statusname", "id", true);
                //if (DrawingID.Trim().Length == 0)
                //{
                //    ShowMessage.ValMess("Please save this drawing to RedBracket.");
                //    return;
                //}

                // Get Current File Info from Custum Properties and Display
                DataRow[] dtCurrentData = cadManager.GetExternalRefreces().Select("isroot=1");
                if (dtCurrentData.Length > 0)
                {
                    lbDrawingIDC.Text = DrawingID = Convert.ToString(dtCurrentData[0]["drawingid"]);
                    if (DrawingID.Trim().Length == 0)
                    {
                        ShowMessage.ValMess("Please save this drawing to RedBracket."); this.Close();
                        return;
                    }
                    lbDrawingNameC.Text = Convert.ToString(dtCurrentData[0]["drawingname"]);

                    lbDrawingNoC.Text = Convert.ToString(dtCurrentData[0]["drawingnumber"]);
                    lbVersionC.Text = Convert.ToString(dtCurrentData[0]["revision"]);
                    lbProjectNameC.Text = Convert.ToString(dtCurrentData[0]["projectid"]);
                    lbProjectNoC.Text = Convert.ToString(dtCurrentData[0]["projectno"]);
                    string T = Convert.ToString(dtCurrentData[0]["FileTypeID"]) == string.Empty ? "---Select---" : Convert.ToString(dtCurrentData[0]["FileTypeID"]);
                    cmbFileTypeC.Text = T;
                    cmbFileStatusC.Text = Convert.ToString(dtCurrentData[0]["drawingstate"]) == string.Empty ? "---Select---" : Convert.ToString(dtCurrentData[0]["drawingstate"]);
                    lbDrawingIDC.Text = DrawingID = Convert.ToString(dtCurrentData[0]["drawingid"]);

                    lbCreatedByC.Text = Convert.ToString(dtCurrentData[0]["createdby"]);
                    lbModifiedByC.Text = Convert.ToString(dtCurrentData[0]["modifiedby"]);


                    string LayoutInfo1 = Convert.ToString(dtCurrentData[0]["layoutinfo"]);
                    if (LayoutInfo1.Trim().Length > 0)
                    {
                        #region Layout Info
                        List<LayoutInfo> objLI = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LayoutInfo>>(LayoutInfo1);

                        int count = 0;

                        foreach (LayoutInfo objLI1 in objLI)
                        {
                            count++;

                            if (count == 1)
                            {
                                lbLayoutNameC1.Text = Convert.ToString(objLI1.name);
                                lbLayoutNoC1.Text = Convert.ToString(objLI1.number);
                                lbLayoutVersionC1.Text = Convert.ToString(objLI1.versionNo);
                                object o = objLI1.statusId == null || objLI1.statusId == string.Empty ? 0 : Convert.ToInt16(objLI1.statusId);
                                cmbLayoutStatusC1.SelectedValue = objLI1.statusId == null || objLI1.statusId == string.Empty ? 0 : Convert.ToInt16(objLI1.statusId);
                                cmbLayoutTypeC1.SelectedValue = objLI1.typeId == null || objLI1.typeId == string.Empty ? 0 : Convert.ToInt16(objLI1.typeId);
                            }
                            else if (count == 2)
                            {
                                lbLayoutNameC2.Text = Convert.ToString(objLI1.name);
                                lbLayoutNoC2.Text = Convert.ToString(objLI1.number);
                                lbLayoutVersionC2.Text = Convert.ToString(objLI1.versionNo);
                                cmbLayoutStatusC2.SelectedValue = objLI1.statusId == null || objLI1.statusId == string.Empty ? 0 : Convert.ToInt16(objLI1.statusId);
                                cmbLayoutTypeC2.SelectedValue = objLI1.typeId == null || objLI1.typeId == string.Empty ? 0 : Convert.ToInt16(objLI1.typeId);

                                FileInfoRowCount = tlpMain.RowCount;
                            }
                            else
                            {
                                Font font = label14.Font;
                                int CRC = tlpMain.RowCount;
                                if (count == 3)
                                {
                                    FileInfoRowCount = CRC;
                                }
                                tlpMain.RowCount += 6;
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                                tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));


                                tlpMain.Controls.Add(new Label() { Text = "Layout Name", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = "Layout No", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = "Layout Version", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = "Layout Status", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = "Layout Discipline", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);

                                CRC -= 5;
                                tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.name), Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill, Name = "lbLayoutNameC" + count }, 1, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.number), Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 1, CRC++);
                                tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.versionNo), Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 1, CRC++);
                                tlpMain.Controls.Add(new ComboBox() { Font = font, Name = "cmbLayoutStatusC" + count, Dock = DockStyle.Fill }, 1, CRC++);
                                tlpMain.Controls.Add(new ComboBox() { Font = font, Name = "cmbLayoutTypeC" + count, Dock = DockStyle.Fill }, 1, CRC++);

                                ComboBox combo1 = tlpMain.Controls.Find("cmbLayoutStatusC" + count, true).FirstOrDefault() as ComboBox;

                                if (combo1 != null)
                                {
                                    Helper.FIllCMB(combo1, objRBC.GetFIleStatus(), "statusname", "id", true);
                                    combo1.SelectedValue = objLI1.statusId == null || objLI1.statusId == string.Empty ? 0 : Convert.ToInt16(objLI1.statusId);
                                }

                                ComboBox combo2 = tlpMain.Controls.Find("cmbLayoutTypeC" + count, true).FirstOrDefault() as ComboBox;

                                if (combo2 != null)
                                {
                                    Helper.FIllCMB(combo2, objRBC.GetFIleType(), "name", "id", true);
                                    combo2.SelectedValue = objLI1.typeId == null || objLI1.typeId == string.Empty ? 0 : Convert.ToInt16(objLI1.typeId);
                                }
                            }
                        }

                        LocalFileLayoutCount = count;
                        #endregion
                    }




                }

                // Get Latest FIle Info from RB and Display

                ResultSearchCriteria objRSC = objRBC.GetDrawingInformation(DrawingID);


                lbDrawingNameL.Text = Convert.ToString(objRSC.name);

                lbDrawingNoL.Text = Convert.ToString(objRSC.fileNo);
                lbVersionL.Text = Convert.ToString(objRSC.versionno);
                lbProjectNameL.Text = Convert.ToString(objRSC.projectname);
                lbProjectNoL.Text = Convert.ToString(objRSC.projectNumber);
                lbFileTypeL.Text = Convert.ToString(objRSC.type == null ? string.Empty : objRSC.type.name == null ? string.Empty : objRSC.type.name);
                lbFileStatusL.Text = Convert.ToString(objRSC.status == null ? string.Empty : objRSC.status.statusname == null ? string.Empty : objRSC.status.statusname);
                lbDrawingIDL.Text = DrawingID = Convert.ToString(objRSC.id);

                lbCreatedByL.Text = Convert.ToString(objRSC.createdby);
                lbModifiedByL.Text = Convert.ToString(objRSC.updatedby);

                List<LayoutInfo> objLI2 = objRSC.fileLayout;
                int count1 = 0;

                foreach (LayoutInfo objLI1 in objLI2)
                {

                    string LLName = objLI1.name;
                    bool IsLayoutFound = false;
                    for (int i = 1; i <= LocalFileLayoutCount; i++)
                    {
                        string CLName = "";
                        if (i == 1)
                        {
                            CLName = lbLayoutNameC1.Text;
                        }
                        else if (i == 2)
                        {
                            CLName = lbLayoutNameC2.Text;
                        }
                        else
                        {
                            Label lblLayoutName = tlpMain.Controls.Find("lbLayoutNameC" + i, true).FirstOrDefault() as Label;

                            if(lblLayoutName!=null)
                            {
                                CLName = lblLayoutName.Text;
                            }
                        }
                        if(LLName==CLName)
                        {
                            IsLayoutFound = true;
                            count1 = i;
                            break;
                        }

                    }
                    if(IsLayoutFound)
                    {

                    }
                    else
                    {
                        count1 = LocalFileLayoutCount + 1;
                    }
                    //count1++;

                    if (count1 == 1)
                    {
                        lbLayoutNameL1.Text = Convert.ToString(objLI1.name);
                        lbLayoutNoL1.Text = Convert.ToString(objLI1.number);
                        lbLayoutVersionL1.Text = Convert.ToString(objLI1.versionNo);
                        lbLayoutStatusL1.Text = Convert.ToString(objLI1.status == null ? string.Empty : objLI1.status.statusname == null ? string.Empty : objLI1.status.statusname);

                        lbLayoutTypeL1.Text = Convert.ToString(objLI1.type == null ? string.Empty : objLI1.type.name == null ? string.Empty : objLI1.type.name);

                    }
                    else if (count1 == 2)
                    {
                        lbLayoutNameL2.Text = Convert.ToString(objLI1.name);
                        lbLayoutNoL2.Text = Convert.ToString(objLI1.number);
                        lbLayoutVersionL2.Text = Convert.ToString(objLI1.versionNo);
                        lbLayoutStatusL2.Text = Convert.ToString(objLI1.status == null ? string.Empty : objLI1.status.statusname == null ? string.Empty : objLI1.status.statusname);
                        lbLayoutTypeL2.Text = Convert.ToString(objLI1.type == null ? string.Empty : objLI1.type.name == null ? string.Empty : objLI1.type.name);
                    }
                    else
                    {

                        int CRC = FileInfoRowCount + (6 * (count1 - 3));
                        Font font = label14.Font;
                        if (count1 > LocalFileLayoutCount)
                        {

                            //CRC = tlpMain.RowCount;
                            tlpMain.RowCount += 6;
                            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));



                            tlpMain.Controls.Add(new Label() { Text = "Layout Name", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                            tlpMain.Controls.Add(new Label() { Text = "Layout No", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                            tlpMain.Controls.Add(new Label() { Text = "Layout Version", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                            tlpMain.Controls.Add(new Label() { Text = "Layout Status", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);
                            tlpMain.Controls.Add(new Label() { Text = "Layout Discipline", Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 0, CRC++);

                            CRC -= 5;
                        }

                        tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.name), Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 2, CRC++);
                        tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.number), Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 2, CRC++);
                        tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.versionNo), Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 2, CRC++);
                        tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.status == null ? string.Empty : objLI1.status.statusname == null ? string.Empty : objLI1.status.statusname), Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 2, CRC++);
                        tlpMain.Controls.Add(new Label() { Text = Convert.ToString(objLI1.type == null ? string.Empty : objLI1.type.name == null ? string.Empty : objLI1.type.name), Font = font, TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill }, 2, CRC++);


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
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message); this.Close(); return;
            }

            this.Show();
        }


    }
}
