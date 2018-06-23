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
            this.ChangeVersion = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.FileLayoutName = new AdvancedDataGridView.TreeGridColumn();
            this.FileID1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LayoutID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LayoutType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.LayoutStatus = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Version = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ACLayoutID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LayoutName1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tgvLayouts)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tgvLayouts, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.66666F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1107, 450);
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
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 393);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1101, 54);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(954, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 48);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(804, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(144, 48);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // tgvLayouts
            // 
            this.tgvLayouts.AllowUserToAddRows = false;
            this.tgvLayouts.AllowUserToDeleteRows = false;
            this.tgvLayouts.AllowUserToOrderColumns = true;
            this.tgvLayouts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.tgvLayouts.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.tgvLayouts.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.tgvLayouts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ChangeVersion,
            this.FileLayoutName,
            this.FileID1,
            this.LayoutID,
            this.LayoutType,
            this.LayoutStatus,
            this.Version,
            this.Description,
            this.IsFile,
            this.TypeID,
            this.StatusID,
            this.ACLayoutID,
            this.LayoutName1});
            this.tgvLayouts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tgvLayouts.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.tgvLayouts.ImageList = null;
            this.tgvLayouts.Location = new System.Drawing.Point(3, 3);
            this.tgvLayouts.Name = "tgvLayouts";
            this.tgvLayouts.RowHeadersVisible = false;
            this.tgvLayouts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.tgvLayouts.Size = new System.Drawing.Size(1101, 384);
            this.tgvLayouts.TabIndex = 8;
            this.tgvLayouts.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.tgvLayouts_CellValueChanged);
            this.tgvLayouts.CurrentCellDirtyStateChanged += new System.EventHandler(this.tgvLayouts_CurrentCellDirtyStateChanged);
            this.tgvLayouts.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.tgvLayouts_DataError);
            // 
            // ChangeVersion
            // 
            this.ChangeVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ChangeVersion.HeaderText = "Change Version";
            this.ChangeVersion.Name = "ChangeVersion";
            this.ChangeVersion.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // FileLayoutName
            // 
            this.FileLayoutName.DefaultNodeImage = null;
            this.FileLayoutName.HeaderText = "FIle Layout Name";
            this.FileLayoutName.Name = "FileLayoutName";
            this.FileLayoutName.ReadOnly = true;
            this.FileLayoutName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            this.LayoutType.Width = 150;
            // 
            // LayoutStatus
            // 
            this.LayoutStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LayoutStatus.HeaderText = "Status";
            this.LayoutStatus.Name = "LayoutStatus";
            this.LayoutStatus.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LayoutStatus.Width = 150;
            // 
            // Version
            // 
            this.Version.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Version.HeaderText = "Version";
            this.Version.Name = "Version";
            this.Version.ReadOnly = true;
            this.Version.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Version.Width = 48;
            // 
            // Description
            // 
            this.Description.HeaderText = "Version Note";
            this.Description.Name = "Description";
            this.Description.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // frmLayoutVersionUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmLayoutVersionUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Layout Version Update";
            this.Load += new System.EventHandler(this.frmLayoutVersionUpdate_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tgvLayouts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AdvancedDataGridView.TreeGridView tgvLayouts;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ChangeVersion;
        private AdvancedDataGridView.TreeGridColumn FileLayoutName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileID1;
        private System.Windows.Forms.DataGridViewTextBoxColumn LayoutID;
        private System.Windows.Forms.DataGridViewComboBoxColumn LayoutType;
        private System.Windows.Forms.DataGridViewComboBoxColumn LayoutStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn Version;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ACLayoutID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LayoutName1;
    }
}