namespace AutocadPlugIn.UI_Forms
{
    partial class frmSave_Active_Drawings
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.submit = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.savetreeGrid = new AdvancedDataGridView.TreeGridView();
            this.imageStrip = new System.Windows.Forms.ImageList(this.components);
            this.CADDescription = new System.Windows.Forms.TextBox();
            this.Comments = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.drawing = new AdvancedDataGridView.TreeGridColumn();
            this.DrawingNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cadtype = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.drawingID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filepath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemtype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockstatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sourceid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isroot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Layouts = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.revision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.canDelete = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isowner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hasViewPermission = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isActFileLatest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isEditable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.canEditStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hasStatusClosed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isletest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prefix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OldProject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FolderPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FolderID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OldFolderID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsNewXref = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnBrowseFolder = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.savetreeGrid)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // submit
            // 
            this.submit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.submit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.submit.Location = new System.Drawing.Point(674, 3);
            this.submit.Name = "submit";
            this.submit.Size = new System.Drawing.Size(144, 32);
            this.submit.TabIndex = 4;
            this.submit.Text = "Save";
            this.submit.UseVisualStyleBackColor = true;
            this.submit.Click += new System.EventHandler(this.submit_Click);
            // 
            // cancel
            // 
            this.cancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.Location = new System.Drawing.Point(824, 3);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(144, 32);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // savetreeGrid
            // 
            this.savetreeGrid.AllowUserToAddRows = false;
            this.savetreeGrid.AllowUserToDeleteRows = false;
            this.savetreeGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.savetreeGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.savetreeGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            this.drawing,
            this.DrawingNumber,
            this.cadtype,
            this.State,
            this.drawingID,
            this.filepath,
            this.itemtype,
            this.lockstatus,
            this.sourceid,
            this.isroot,
            this.Layouts,
            this.ProjectName,
            this.revision,
            this.canDelete,
            this.isowner,
            this.hasViewPermission,
            this.isActFileLatest,
            this.isEditable,
            this.canEditStatus,
            this.hasStatusClosed,
            this.isletest,
            this.prefix,
            this.OldProject,
            this.FolderPath,
            this.FolderID,
            this.OldFolderID,
            this.IsNewXref,
            this.ProjectID,
            this.StatusID,
            this.TypeID,
            this.ProjNo,
            this.BtnBrowseFolder});
            this.savetreeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.savetreeGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.savetreeGrid.ImageList = null;
            this.savetreeGrid.Location = new System.Drawing.Point(3, 3);
            this.savetreeGrid.Name = "savetreeGrid";
            this.savetreeGrid.RowHeadersVisible = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.savetreeGrid.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.savetreeGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.savetreeGrid.Size = new System.Drawing.Size(971, 342);
            this.savetreeGrid.TabIndex = 7;
            this.savetreeGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.savetreeGrid_CellContentClick);
            this.savetreeGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.savetreeGrid_CellBeginEdit);
            this.savetreeGrid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.savetreeGrid_CellMouseClick);
            this.savetreeGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.savetreeGrid_CellValueChanged);
            this.savetreeGrid.CurrentCellDirtyStateChanged += new System.EventHandler(this.savetreeGrid_CurrentCellDirtyStateChanged);
            this.savetreeGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.savetreeGrid_DataError);
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
            this.CADDescription.Location = new System.Drawing.Point(110, 3);
            this.CADDescription.Multiline = true;
            this.CADDescription.Name = "CADDescription";
            this.CADDescription.Size = new System.Drawing.Size(418, 32);
            this.CADDescription.TabIndex = 8;
            // 
            // Comments
            // 
            this.Comments.AutoSize = true;
            this.Comments.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Comments.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Comments.ForeColor = System.Drawing.Color.Black;
            this.Comments.Location = new System.Drawing.Point(3, 3);
            this.Comments.Margin = new System.Windows.Forms.Padding(3);
            this.Comments.Name = "Comments";
            this.Comments.Size = new System.Drawing.Size(101, 32);
            this.Comments.TabIndex = 9;
            this.Comments.Text = "Version Note :";
            this.Comments.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.savetreeGrid, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 43);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.01869F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.98131F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(977, 392);
            this.tableLayoutPanel1.TabIndex = 10;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 424F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.Controls.Add(this.cancel, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.submit, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.CADDescription, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.Comments, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 351);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(971, 38);
            this.tableLayoutPanel2.TabIndex = 11;
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
            this.tableLayoutPanel3.Size = new System.Drawing.Size(1003, 448);
            this.tableLayoutPanel3.TabIndex = 11;
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
            this.pnlTop.Size = new System.Drawing.Size(1003, 40);
            this.pnlTop.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(306, 29);
            this.label1.TabIndex = 12;
            this.label1.Text = "Save Active Drawings";
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 40);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(10, 398);
            this.pnlLeft.TabIndex = 0;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.tableLayoutPanel3.SetColumnSpan(this.pnlBottom, 3);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(0, 438);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(1003, 10);
            this.pnlBottom.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(993, 40);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(10, 398);
            this.pnlRight.TabIndex = 0;
            // 
            // Check
            // 
            this.Check.FillWeight = 41.15082F;
            this.Check.HeaderText = "";
            this.Check.MinimumWidth = 25;
            this.Check.Name = "Check";
            this.Check.Width = 90;
            // 
            // drawing
            // 
            this.drawing.DefaultNodeImage = null;
            this.drawing.FillWeight = 79.85113F;
            this.drawing.HeaderText = "File Name";
            this.drawing.Name = "drawing";
            this.drawing.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.drawing.Width = 177;
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
            this.ProjectName.Width = 178;
            // 
            // revision
            // 
            this.revision.FillWeight = 23.95534F;
            this.revision.HeaderText = "Rev";
            this.revision.Name = "revision";
            this.revision.ReadOnly = true;
            this.revision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.revision.Width = 53;
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
            // prefix
            // 
            this.prefix.HeaderText = "prefix";
            this.prefix.Name = "prefix";
            this.prefix.ReadOnly = true;
            this.prefix.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.prefix.Visible = false;
            // 
            // OldProject
            // 
            this.OldProject.HeaderText = "OldProject";
            this.OldProject.Name = "OldProject";
            this.OldProject.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.OldProject.Visible = false;
            // 
            // FolderPath
            // 
            this.FolderPath.HeaderText = "Server Path";
            this.FolderPath.Name = "FolderPath";
            this.FolderPath.ReadOnly = true;
            this.FolderPath.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FolderPath.Width = 120;
            // 
            // FolderID
            // 
            this.FolderID.HeaderText = "FolderID";
            this.FolderID.Name = "FolderID";
            this.FolderID.ReadOnly = true;
            this.FolderID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.FolderID.Visible = false;
            // 
            // OldFolderID
            // 
            this.OldFolderID.HeaderText = "OldFolderID";
            this.OldFolderID.Name = "OldFolderID";
            this.OldFolderID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.OldFolderID.Visible = false;
            // 
            // IsNewXref
            // 
            this.IsNewXref.HeaderText = "IsNewXref";
            this.IsNewXref.Name = "IsNewXref";
            this.IsNewXref.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.IsNewXref.Visible = false;
            // 
            // ProjectID
            // 
            this.ProjectID.HeaderText = "ProjectID";
            this.ProjectID.Name = "ProjectID";
            this.ProjectID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProjectID.Visible = false;
            // 
            // StatusID
            // 
            this.StatusID.HeaderText = "StatusID";
            this.StatusID.Name = "StatusID";
            this.StatusID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StatusID.Visible = false;
            // 
            // TypeID
            // 
            this.TypeID.HeaderText = "TypeID";
            this.TypeID.Name = "TypeID";
            this.TypeID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.TypeID.Visible = false;
            // 
            // ProjNo
            // 
            this.ProjNo.HeaderText = "ProjNo";
            this.ProjNo.Name = "ProjNo";
            this.ProjNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProjNo.Visible = false;
            // 
            // BtnBrowseFolder
            // 
            this.BtnBrowseFolder.HeaderText = "";
            this.BtnBrowseFolder.Image = global::AutocadPlugIn.Properties.Resources.FolderBrowse;
            this.BtnBrowseFolder.Name = "BtnBrowseFolder";
            this.BtnBrowseFolder.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.BtnBrowseFolder.Width = 50;
            // 
            // frmSave_Active_Drawings
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1003, 448);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmSave_Active_Drawings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Save Active Drawings";
            this.Load += new System.EventHandler(this.Save_Active_Drawings_Load);
            this.Resize += new System.EventHandler(this.Save_Active_Drawings_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.savetreeGrid)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private AdvancedDataGridView.TreeGridColumn drawing;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingNumber;
        private System.Windows.Forms.DataGridViewComboBoxColumn cadtype;
        private System.Windows.Forms.DataGridViewComboBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn drawingID;
        private System.Windows.Forms.DataGridViewTextBoxColumn filepath;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemtype;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockstatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn sourceid;
        private System.Windows.Forms.DataGridViewTextBoxColumn isroot;
        private System.Windows.Forms.DataGridViewTextBoxColumn Layouts;
        private System.Windows.Forms.DataGridViewComboBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn revision;
        private System.Windows.Forms.DataGridViewTextBoxColumn canDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn isowner;
        private System.Windows.Forms.DataGridViewTextBoxColumn hasViewPermission;
        private System.Windows.Forms.DataGridViewTextBoxColumn isActFileLatest;
        private System.Windows.Forms.DataGridViewTextBoxColumn isEditable;
        private System.Windows.Forms.DataGridViewTextBoxColumn canEditStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn hasStatusClosed;
        private System.Windows.Forms.DataGridViewTextBoxColumn isletest;
        private System.Windows.Forms.DataGridViewTextBoxColumn prefix;
        private System.Windows.Forms.DataGridViewTextBoxColumn OldProject;
        private System.Windows.Forms.DataGridViewTextBoxColumn FolderPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn FolderID;
        private System.Windows.Forms.DataGridViewTextBoxColumn OldFolderID;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsNewXref;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectID;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusID;
        private System.Windows.Forms.DataGridViewTextBoxColumn TypeID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjNo;
        private System.Windows.Forms.DataGridViewImageColumn BtnBrowseFolder;
    }
}