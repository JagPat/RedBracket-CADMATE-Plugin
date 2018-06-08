namespace AutocadPlugIn.UI_Forms
{
    partial class Save_Active_Drawings
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
            this.components = new System.ComponentModel.Container();
            this.submit = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.savetreeGrid = new AdvancedDataGridView.TreeGridView();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.drawing = new AdvancedDataGridView.TreeGridColumn();
            this.DrawingNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cadtype = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.revision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.drawingID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filepath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemtype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockstatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isroot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Layouts = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.canDelete = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isowner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hasViewPermission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActFileLatest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isEditable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.canEditStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hasStatusClosed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isletest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.targetrevision = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Version = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.imageStrip = new System.Windows.Forms.ImageList(this.components);
            this.CADDescription = new System.Windows.Forms.TextBox();
            this.Comments = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.savetreeGrid)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // submit
            // 
            this.submit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.submit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.submit.Location = new System.Drawing.Point(839, 3);
            this.submit.Name = "submit";
            this.submit.Size = new System.Drawing.Size(144, 35);
            this.submit.TabIndex = 4;
            this.submit.Text = "Submit";
            this.submit.UseVisualStyleBackColor = true;
            this.submit.Click += new System.EventHandler(this.submit_Click);
            // 
            // cancel
            // 
            this.cancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.Location = new System.Drawing.Point(989, 3);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(144, 35);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // savetreeGrid
            // 
            this.savetreeGrid.AllowUserToAddRows = false;
            this.savetreeGrid.AllowUserToDeleteRows = false;
            this.savetreeGrid.AllowUserToOrderColumns = true;
            this.savetreeGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.savetreeGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.savetreeGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.savetreeGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            this.drawing,
            this.DrawingNumber,
            this.cadtype,
            this.revision,
            this.State,
            this.drawingID,
            this.filepath,
            this.itemtype,
            this.lockstatus,
            this.sourceid,
            this.isroot,
            this.Layouts,
            this.ProjectName,
            this.canDelete,
            this.isowner,
            this.hasViewPermission,
            this.isActFileLatest,
            this.isEditable,
            this.canEditStatus,
            this.hasStatusClosed,
            this.isletest,
            this.targetrevision,
            this.Version});
            this.savetreeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.savetreeGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.savetreeGrid.ImageList = null;
            this.savetreeGrid.Location = new System.Drawing.Point(3, 3);
            this.savetreeGrid.Name = "savetreeGrid";
            this.savetreeGrid.RowHeadersVisible = false;
            this.savetreeGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.savetreeGrid.Size = new System.Drawing.Size(1136, 375);
            this.savetreeGrid.TabIndex = 7;
            this.savetreeGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.savetreeGrid_CellContentClick);
            this.savetreeGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.savetreeGrid_CellBeginEdit);
            this.savetreeGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.savetreeGrid_CurrentCellDirtyStateChanged);
            this.savetreeGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.savetreeGrid_DataError);
            // 
            // Check
            // 
            this.Check.FillWeight = 41.15082F;
            this.Check.HeaderText = "";
            this.Check.MinimumWidth = 25;
            this.Check.Name = "Check";
            // 
            // drawing
            // 
            this.drawing.DefaultNodeImage = null;
            this.drawing.FillWeight = 79.85113F;
            this.drawing.HeaderText = "File Name";
            this.drawing.Name = "drawing";
            this.drawing.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DrawingNumber
            // 
            this.DrawingNumber.FillWeight = 79.85113F;
            this.DrawingNumber.HeaderText = "File No";
            this.DrawingNumber.Name = "DrawingNumber";
            this.DrawingNumber.ReadOnly = true;
            this.DrawingNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cadtype
            // 
            this.cadtype.FillWeight = 79.85113F;
            this.cadtype.HeaderText = "File Type";
            this.cadtype.Name = "cadtype";
            this.cadtype.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // revision
            // 
            this.revision.FillWeight = 23.95534F;
            this.revision.HeaderText = "Rev";
            this.revision.Name = "revision";
            this.revision.ReadOnly = true;
            this.revision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // State
            // 
            this.State.FillWeight = 55.89579F;
            this.State.HeaderText = "FIle Status";
            this.State.Name = "State";
            this.State.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // drawingID
            // 
            this.drawingID.HeaderText = "Drawing ID";
            this.drawingID.Name = "drawingID";
            this.drawingID.ReadOnly = true;
            this.drawingID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.drawingID.Visible = false;
            // 
            // filepath
            // 
            this.filepath.HeaderText = "FilePath";
            this.filepath.Name = "filepath";
            this.filepath.ReadOnly = true;
            this.filepath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.filepath.Visible = false;
            // 
            // itemtype
            // 
            this.itemtype.HeaderText = "Itemtype";
            this.itemtype.Name = "itemtype";
            this.itemtype.ReadOnly = true;
            this.itemtype.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.itemtype.Visible = false;
            // 
            // lockstatus
            // 
            this.lockstatus.HeaderText = "LockStatus";
            this.lockstatus.Name = "lockstatus";
            this.lockstatus.ReadOnly = true;
            this.lockstatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.lockstatus.Visible = false;
            // 
            // sourceid
            // 
            this.sourceid.HeaderText = "SourceId";
            this.sourceid.Name = "sourceid";
            this.sourceid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.sourceid.Visible = false;
            // 
            // isroot
            // 
            this.isroot.HeaderText = "IsRoot";
            this.isroot.Name = "isroot";
            this.isroot.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.isroot.Visible = false;
            // 
            // Layouts
            // 
            this.Layouts.HeaderText = "Layouts";
            this.Layouts.Name = "Layouts";
            this.Layouts.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Layouts.Visible = false;
            // 
            // ProjectName
            // 
            this.ProjectName.FillWeight = 79.85113F;
            this.ProjectName.HeaderText = "Project Name";
            this.ProjectName.Name = "ProjectName";
            // 
            // canDelete
            // 
            this.canDelete.HeaderText = "canDelete";
            this.canDelete.Name = "canDelete";
            this.canDelete.ReadOnly = true;
            this.canDelete.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.canDelete.Visible = false;
            // 
            // isowner
            // 
            this.isowner.HeaderText = "isowner";
            this.isowner.Name = "isowner";
            this.isowner.ReadOnly = true;
            this.isowner.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.isowner.Visible = false;
            // 
            // hasViewPermission
            // 
            this.hasViewPermission.HeaderText = "hasViewPermission";
            this.hasViewPermission.Name = "hasViewPermission";
            this.hasViewPermission.ReadOnly = true;
            this.hasViewPermission.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.hasViewPermission.Visible = false;
            // 
            // isActFileLatest
            // 
            this.isActFileLatest.HeaderText = "isActFileLatest";
            this.isActFileLatest.Name = "isActFileLatest";
            this.isActFileLatest.ReadOnly = true;
            this.isActFileLatest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.isActFileLatest.Visible = false;
            // 
            // isEditable
            // 
            this.isEditable.HeaderText = "isEditable";
            this.isEditable.Name = "isEditable";
            this.isEditable.ReadOnly = true;
            this.isEditable.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.isEditable.Visible = false;
            // 
            // canEditStatus
            // 
            this.canEditStatus.HeaderText = "canEditStatus";
            this.canEditStatus.Name = "canEditStatus";
            this.canEditStatus.ReadOnly = true;
            this.canEditStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.canEditStatus.Visible = false;
            // 
            // hasStatusClosed
            // 
            this.hasStatusClosed.HeaderText = "hasStatusClosed";
            this.hasStatusClosed.Name = "hasStatusClosed";
            this.hasStatusClosed.ReadOnly = true;
            this.hasStatusClosed.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.hasStatusClosed.Visible = false;
            // 
            // isletest
            // 
            this.isletest.HeaderText = "isletest";
            this.isletest.Name = "isletest";
            this.isletest.ReadOnly = true;
            this.isletest.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.isletest.Visible = false;
            // 
            // targetrevision
            // 
            this.targetrevision.HeaderText = "Taget Rev";
            this.targetrevision.Name = "targetrevision";
            this.targetrevision.Visible = false;
            // 
            // Version
            // 
            this.Version.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Version.FillWeight = 149.7981F;
            this.Version.HeaderText = "Create New Ver";
            this.Version.MinimumWidth = 25;
            this.Version.Name = "Version";
            this.Version.Width = 88;
            // 
            // imageStrip
            // 
            this.imageStrip.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageStrip.ImageSize = new System.Drawing.Size(17, 17);
            this.imageStrip.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CADDescription
            // 
            this.CADDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CADDescription.Location = new System.Drawing.Point(103, 3);
            this.CADDescription.Multiline = true;
            this.CADDescription.Name = "CADDescription";
            this.CADDescription.Size = new System.Drawing.Size(394, 35);
            this.CADDescription.TabIndex = 8;
            // 
            // Comments
            // 
            this.Comments.AutoSize = true;
            this.Comments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Comments.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Comments.ForeColor = System.Drawing.Color.Red;
            this.Comments.Location = new System.Drawing.Point(3, 0);
            this.Comments.Name = "Comments";
            this.Comments.Size = new System.Drawing.Size(94, 41);
            this.Comments.TabIndex = 9;
            this.Comments.Text = "Comments :";
            this.Comments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.savetreeGrid, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.01869F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.98131F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1142, 428);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.Controls.Add(this.cancel, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.submit, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.CADDescription, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.Comments, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.pictureBox1, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 384);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1136, 41);
            this.tableLayoutPanel2.TabIndex = 11;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = global::AutocadPlugIn.Properties.Resources.Refresh;
            this.pictureBox1.Location = new System.Drawing.Point(503, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 35);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // Save_Active_Drawings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1142, 428);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Save_Active_Drawings";
            this.Text = "Save Active Drawings";
            this.Load += new System.EventHandler(this.Save_Active_Drawings_Load);
            this.Resize += new System.EventHandler(this.Save_Active_Drawings_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.savetreeGrid)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.Button submit;
        private System.Windows.Forms.Button cancel;
        private AdvancedDataGridView.TreeGridView savetreeGrid;
        private System.Windows.Forms.ImageList imageStrip;
        private System.Windows.Forms.TextBox CADDescription;
        private System.Windows.Forms.Label Comments;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private AdvancedDataGridView.TreeGridColumn drawing;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingNumber;
        private System.Windows.Forms.DataGridViewComboBoxColumn cadtype;
        private System.Windows.Forms.DataGridViewTextBoxColumn revision;
        private System.Windows.Forms.DataGridViewComboBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn drawingID;
        private System.Windows.Forms.DataGridViewTextBoxColumn filepath;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemtype;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockstatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceid;
        private System.Windows.Forms.DataGridViewTextBoxColumn isroot;
        private System.Windows.Forms.DataGridViewTextBoxColumn Layouts;
        private System.Windows.Forms.DataGridViewComboBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn canDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn isowner;
        private System.Windows.Forms.DataGridViewTextBoxColumn hasViewPermission;
        private System.Windows.Forms.DataGridViewTextBoxColumn isActFileLatest;
        private System.Windows.Forms.DataGridViewTextBoxColumn isEditable;
        private System.Windows.Forms.DataGridViewTextBoxColumn canEditStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn hasStatusClosed;
        private System.Windows.Forms.DataGridViewTextBoxColumn isletest;
        private System.Windows.Forms.DataGridViewComboBoxColumn targetrevision;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Version;
    }
}