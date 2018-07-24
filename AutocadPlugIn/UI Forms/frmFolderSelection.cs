using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using System.IO;
using AdvancedDataGridView;

namespace AutocadPlugIn.UI_Forms
{
    public partial class frmFolderSelection : Form
    {
        RBConnector objRBC = new RBConnector();
        public string FolderID = "";
        public string ProjectID = "";
        public string FolderPath = "";
        public Stack<List<clsFolderSearchReasult>> StackFolderSearchReasult = new Stack<List<clsFolderSearchReasult>>();
        public bool IsSelect = false;
        public bool LoadFlag = false;

        public frmFolderSelection(string FolderID = "", string ProjectID = "", Stack<List<clsFolderSearchReasult>> StackFolderSearchReasult = null, string FolderPath = "")
        {
            InitializeComponent(); FormBorderStyle = FormBorderStyle.None;
            this.FolderID = FolderID;
            this.ProjectID = ProjectID;
            this.FolderPath = FolderPath;
            if (StackFolderSearchReasult == null)
            {
                StackFolderSearchReasult = new Stack<List<clsFolderSearchReasult>>();
            }
            else
            {
                this.StackFolderSearchReasult = StackFolderSearchReasult;
            }
            pnlTop.BackColor = pnlRight.BackColor = pnlLeft.BackColor = pnlBottom.BackColor = Helper.clrChildPopupBorderColor;
        }

        private void frmFolderSelection_Load(object sender, EventArgs e)
        {
            LoadFlag = true;
            try
            {
                Location = new Point(Location.X, Location.Y + 25);
                dgvFolderSelection.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9, FontStyle.Bold);
                btnSelect.Enabled = false;
                List<clsFolderSearchReasult> objFolderSearchResult = StackFolderSearchReasult.Pop();
                StackFolderSearchReasult.Push(objFolderSearchResult);


                if (objFolderSearchResult.Count > 0)
                {
                    int RC = dgvFolderSelection.Rows.Count;
                    if (StackFolderSearchReasult.Count > 1)
                    {
                        dgvFolderSelection.Rows.Add();
                        dgvFolderSelection.Rows[RC].Cells["FolderName"].Value = "Go Back";
                        dgvFolderSelection.Rows[RC].Cells["HasFolder"].Value = "";
                        dgvFolderSelection.Rows[RC].Cells["FolderID1"].Value = "-1";
                    }

                    if (FolderPath.Length > 0)
                        lblParentFolder.Text = Path.GetDirectoryName(FolderPath) + @"\";

                    lblSelectedPath.Text = FolderPath;

                    foreach (clsFolderSearchReasult obj in objFolderSearchResult)
                    {
                        if (obj != null)
                        {
                            RC = dgvFolderSelection.Rows.Count;
                            dgvFolderSelection.Rows.Add();
                            dgvFolderSelection.Rows[RC].Cells["FolderName"].Value = obj.name;
                            dgvFolderSelection.Rows[RC].Cells["HasFolder"].Value = obj.childFolderSize;
                            dgvFolderSelection.Rows[RC].Cells["FolderID1"].Value = obj.id;


                            if (FolderPath.Length > 0)
                            {
                                if (obj.id == FolderID)
                                    dgvFolderSelection.Rows[dgvFolderSelection.Rows.Count - 1].Selected = true;
                            }

                        }
                    }
                    if (FolderPath.Length == 0)
                    {
                        dgvFolderSelection.ClearSelection();
                    }


                }
                else
                {
                    ShowMessage.InfoMess("No folder found");
                    this.Close(); return;
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            LoadFlag = false;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                FolderPath = lblSelectedPath.Text.Trim();

                if (FolderPath.Length > 0)
                {
                    FolderID = Convert.ToString(lblSelectedPath.Tag);
                }
                else
                {
                    ShowMessage.ValMess("Please select a folder.");
                }
                IsSelect = true;
                this.Close();
            }
            catch (Exception E)
            {
                IsSelect = false;
                ShowMessage.ErrorMess(E.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                IsSelect = false;
                this.Close();
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        private void dgvFolderSelection_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                FolderID = Convert.ToString(dgvFolderSelection.Rows[e.RowIndex].Cells["FolderID1"].Value);
                List<clsFolderSearchReasult> objFolderSearchResult;

                if (Convert.ToDecimal(FolderID) > 0 || Convert.ToDecimal(FolderID) == -2)
                {
                    objFolderSearchResult = objRBC.SearchFolder(ProjectID, FolderID);
                }
                else
                {
                    StackFolderSearchReasult.Pop();
                    objFolderSearchResult = StackFolderSearchReasult.Pop();
                }


                if (objFolderSearchResult == null)
                {
                    ShowMessage.InfoMess("No folder found inside folder " + lblSelectedPath.Text);
                }
                else
                {



                    if (objFolderSearchResult.Count > 0)
                    {
                        if (Convert.ToDecimal(FolderID) > 0 || Convert.ToDecimal(FolderID) == -2)
                        {
                            lblParentFolder.Text = lblParentFolder.Text + Convert.ToString(dgvFolderSelection.Rows[e.RowIndex].Cells["FolderName"].Value) + @"\";
                        }
                        else
                        {
                            lblParentFolder.Text = lblParentFolder.Text.Substring(0, lblParentFolder.Text.LastIndexOf(@"\"));
                            lblParentFolder.Text = lblParentFolder.Text.Substring(0, lblParentFolder.Text.LastIndexOf(@"\") > 0 ? lblParentFolder.Text.LastIndexOf(@"\") + 1 : 0);
                        }
                        dgvFolderSelection.Rows.Clear();
                        //if (Convert.ToDecimal(FolderID) > 0)
                        {
                            StackFolderSearchReasult.Push(objFolderSearchResult);
                        }
                        int RC = dgvFolderSelection.Rows.Count;
                        if (StackFolderSearchReasult.Count > 1)
                        {
                            dgvFolderSelection.Rows.Add();
                            dgvFolderSelection.Rows[RC].Cells["FolderName"].Value = "<== Go Back";
                            dgvFolderSelection.Rows[RC].Cells["HasFolder"].Value = "";
                            dgvFolderSelection.Rows[RC].Cells["FolderID1"].Value = "-1";
                        }


                        //  lblParentFolder.Text = lblSelectedPath.Text;
                        //lblSelectedPath.Text = "";

                        FolderPath = lblSelectedPath.Text = "";
                        LoadFlag = true;
                        foreach (clsFolderSearchReasult obj in objFolderSearchResult)
                        {
                            if (obj != null)
                            {
                                RC = dgvFolderSelection.Rows.Count;
                                dgvFolderSelection.Rows.Add();
                                dgvFolderSelection.Rows[RC].Cells["FolderName"].Value = obj.name;
                                dgvFolderSelection.Rows[RC].Cells["HasFolder"].Value = obj.childFolderSize;
                                dgvFolderSelection.Rows[RC].Cells["FolderID1"].Value = obj.id;


                                if (FolderPath.Length > 0)
                                {
                                    if (FolderPath == Path.Combine(lblParentFolder.Text, obj.name))
                                        dgvFolderSelection.Rows[dgvFolderSelection.Rows.Count - 1].Selected = true;
                                }

                            }
                        }
                        if (FolderPath.Length == 0)
                        {
                            dgvFolderSelection.ClearSelection();
                        }
                        LoadFlag = false;
                    }
                    else
                    {
                        ShowMessage.InfoMess("No folder found inside folder " + lblSelectedPath.Text);
                    }



                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        private void dgvFolderSelection_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (LoadFlag)
                    return;

                if ((dgvFolderSelection.CurrentRow.Index > 0 && dgvFolderSelection.Rows.Count > 1) || (dgvFolderSelection.CurrentRow.Index > -1 && dgvFolderSelection.Rows.Count == 1))
                {
                    lblSelectedPath.Text = lblParentFolder.Text + Convert.ToString(dgvFolderSelection.CurrentRow.Cells["FolderName"].Value);
                    lblSelectedPath.Tag = Convert.ToString(dgvFolderSelection.CurrentRow.Cells["FolderID1"].Value);
                }

                else
                {
                    lblSelectedPath.Tag = lblSelectedPath.Text = "";
                }
            }

            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        private void lblSelectedPath_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (lblSelectedPath.Text.Trim().Length == 0)
                {
                    btnSelect.Enabled = false;
                }
                else
                {
                    btnSelect.Enabled = true;
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }
    }
}
