namespace AutocadPlugIn.UI_Forms
{
    partial class frmLayoutVersionUpdate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tgvLayouts = new AdvancedDataGridView.TreeGridView();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.ChangeVersion = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FileLayoutName = new AdvancedDataGridView.TreeGridColumn();
            this.FileID1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LayoutID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LayoutType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LayoutStatus = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ACLayoutID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LayoutName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LayoutNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CreatedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpdatedBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UpdatedOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgvLayouts)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tgvLayouts, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 43);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.66666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(958, 394);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 344);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(952, 47);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(805, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 41);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(655, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(144, 41);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tgvLayouts
            // 
            this.tgvLayouts.AllowUserToAddRows = false;
            this.tgvLayouts.AllowUserToDeleteRows = false;
            this.tgvLayouts.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tgvLayouts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.tgvLayouts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChangeVersion,
            this.FileLayoutName,
            this.FileID1,
            this.LayoutID,
            this.LayoutType,
            this.LayoutStatus,
            this.Description,
            this.Version,
            this.IsFile,
            this.TypeID,
            this.StatusID,
            this.ACLayoutID,
            this.LayoutName1,
            this.LayoutNo,
            this.CreatedBy,
            this.CreatedOn,
            this.UpdatedBy,
            this.UpdatedOn});
            this.tgvLayouts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tgvLayouts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.tgvLayouts.ImageList = null;
            this.tgvLayouts.Location = new System.Drawing.Point(3, 3);
            this.tgvLayouts.Name = "tgvLayouts";
            this.tgvLayouts.RowHeadersVisible = false;
            this.tgvLayouts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tgvLayouts.Size = new System.Drawing.Size(952, 335);
            this.tgvLayouts.TabIndex = 8;
            this.tgvLayouts.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgvLayouts_CellValueChanged);
            this.tgvLayouts.CurrentCellDirtyStateChanged += new System.EventHandler(this.tgvLayouts_CurrentCellDirtyStateChanged);
            this.tgvLayouts.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.tgvLayouts_DataError);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.Controls.Add(this.pnlTop, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.pnlLeft, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.pnlBottom, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.pnlRight, 2, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(984, 450);
            this.tableLayoutPanel3.TabIndex = 1;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.tableLayoutPanel3.SetColumnSpan(this.pnlTop, 3);
            this.pnlTop.Controls.Add(this.label1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(984, 40);
            this.pnlTop.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(317, 29);
            this.label1.TabIndex = 12;
            this.label1.Text = "Layout Version Update";
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 40);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(10, 400);
            this.pnlLeft.TabIndex = 0;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.tableLayoutPanel3.SetColumnSpan(this.pnlBottom, 3);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(0, 440);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(984, 10);
            this.pnlBottom.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(974, 40);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(10, 400);
            this.pnlRight.TabIndex = 0;
            // 
            // ChangeVersion
            // 
            this.ChangeVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ChangeVersion.HeaderText = "Change Version";
            this.ChangeVersion.Name = "ChangeVersion";
            this.ChangeVersion.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ChangeVersion.Width = 120;
            // 
            // FileLayoutName
            // 
            this.FileLayoutName.DefaultNodeImage = null;
            this.FileLayoutName.HeaderText = "File & Layout Name";
            this.FileLayoutName.Name = "FileLayoutName";
            this.FileLayoutName.ReadOnly = true;
            this.FileLayoutName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FileLayoutName.Width = 243;
            // 
            // FileID1
            // 
            this.FileID1.HeaderText = "FileID";
            this.FileID1.Name = "FileID1";
            this.FileID1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FileID1.Visible = false;
            // 
            // LayoutID
            // 
            this.LayoutID.HeaderText = "LayoutID";
            this.LayoutID.Name = "LayoutID";
            this.LayoutID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LayoutID.Visible = false;
            // 
            // LayoutType
            // 
            this.LayoutType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LayoutType.HeaderText = "Type";
            this.LayoutType.Name = "LayoutType";
            this.LayoutType.Width = 140;
            // 
            // LayoutStatus
            // 
            this.LayoutStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LayoutStatus.HeaderText = "Status";
            this.LayoutStatus.Name = "LayoutStatus";
            this.LayoutStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LayoutStatus.Width = 140;
            // 
            // Description
            // 
            this.Description.HeaderText = "Version Note";
            this.Description.Name = "Description";
            this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Description.Width = 244;
            // 
            // Version
            // 
            this.Version.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Version.HeaderText = "Version";
            this.Version.Name = "Version";
            this.Version.ReadOnly = true;
            this.Version.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Version.Width = 55;
            // 
            // IsFile
            // 
            this.IsFile.HeaderText = "IsFile";
            this.IsFile.Name = "IsFile";
            this.IsFile.ReadOnly = true;
            this.IsFile.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IsFile.Visible = false;
            // 
            // TypeID
            // 
            this.TypeID.HeaderText = "TypeID";
            this.TypeID.Name = "TypeID";
            this.TypeID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TypeID.Visible = false;
            // 
            // StatusID
            // 
            this.StatusID.HeaderText = "StatusID";
            this.StatusID.Name = "StatusID";
            this.StatusID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StatusID.Visible = false;
            // 
            // ACLayoutID
            // 
            this.ACLayoutID.HeaderText = "ACLayoutID";
            this.ACLayoutID.Name = "ACLayoutID";
            this.ACLayoutID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ACLayoutID.Visible = false;
            // 
            // LayoutName1
            // 
            this.LayoutName1.HeaderText = "LayoutName1";
            this.LayoutName1.Name = "LayoutName1";
            this.LayoutName1.ReadOnly = true;
            this.LayoutName1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LayoutName1.Visible = false;
            // 
            // LayoutNo
            // 
            this.LayoutNo.HeaderText = "LayoutNo";
            this.LayoutNo.Name = "LayoutNo";
            this.LayoutNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LayoutNo.Visible = false;
            // 
            // CreatedBy
            // 
            this.CreatedBy.HeaderText = "CreatedBy";
            this.CreatedBy.Name = "CreatedBy";
            this.CreatedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CreatedBy.Visible = false;
            // 
            // CreatedOn
            // 
            this.CreatedOn.HeaderText = "CreatedOn";
            this.CreatedOn.Name = "CreatedOn";
            this.CreatedOn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CreatedOn.Visible = false;
            // 
            // UpdatedBy
            // 
            this.UpdatedBy.HeaderText = "UpdatedBy";
            this.UpdatedBy.Name = "UpdatedBy";
            this.UpdatedBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.UpdatedBy.Visible = false;
            // 
            // UpdatedOn
            // 
            this.UpdatedOn.HeaderText = "UpdatedOn";
            this.UpdatedOn.Name = "UpdatedOn";
            this.UpdatedOn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.UpdatedOn.Visible = false;
            // 
            // frmLayoutVersionUpdate
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(984, 450);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmLayoutVersionUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Layout Version Update";
            this.Load += new System.EventHandler(this.frmLayoutVersionUpdate_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tgvLayouts)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AdvancedDataGridView.TreeGridView tgvLayouts;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ChangeVersion;
        private AdvancedDataGridView.TreeGridColumn FileLayoutName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileID1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LayoutID;
        private System.Windows.Forms.DataGridViewComboBoxColumn LayoutType;
        private System.Windows.Forms.DataGridViewComboBoxColumn LayoutStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Version;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ACLayoutID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LayoutName1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LayoutNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CreatedOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdatedBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpdatedOn;
    }
}