using AdvancedDataGridView;
namespace AutocadPlugIn.UI_Forms
{
    partial class Search_And_Open
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DGNumber = new System.Windows.Forms.TextBox();
            this.DGName = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.Doc_name = new System.Windows.Forms.Label();
            this.Doc_description = new System.Windows.Forms.Label();
            this.OpenDrawingButton = new System.Windows.Forms.Button();
            this.FormCancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CDType = new System.Windows.Forms.ComboBox();
            this.treeGridView1 = new AdvancedDataGridView.TreeGridView();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ExpandButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DrawingName = new AdvancedDataGridView.TreeGridColumn();
            this.DrawingNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LockStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CADType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Generation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DrawingID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LockBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OwnerCompany = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Checkout = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.imageStrip = new System.Windows.Forms.ImageList(this.components);
            this.CDRevisionValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CheckOutViewCBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CDProjectName = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CDProjectId = new System.Windows.Forms.ComboBox();
            this.CDState = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.CDRealty = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.sg_SearchType = new System.Windows.Forms.ComboBox();
            this.searchStatus = new System.Windows.Forms.Label();
            this.busyLabel = new System.Windows.Forms.Label();
            this.label_foldername = new System.Windows.Forms.Label();
            this.textBox_foldername = new System.Windows.Forms.TextBox();
            this.Doc_Number = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.treeGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // DGNumber
            // 
            this.DGNumber.Location = new System.Drawing.Point(792, 448);
            this.DGNumber.Name = "DGNumber";
            this.DGNumber.Size = new System.Drawing.Size(118, 20);
            this.DGNumber.TabIndex = 10;
            this.DGNumber.Visible = false;
            // 
            // DGName
            // 
            this.DGName.Location = new System.Drawing.Point(416, 29);
            this.DGName.Name = "DGName";
            this.DGName.Size = new System.Drawing.Size(161, 20);
            this.DGName.TabIndex = 20;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(423, 129);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 0;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(549, 129);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // Doc_name
            // 
            this.Doc_name.AutoSize = true;
            this.Doc_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Doc_name.Location = new System.Drawing.Point(285, 30);
            this.Doc_name.Name = "Doc_name";
            this.Doc_name.Size = new System.Drawing.Size(89, 13);
            this.Doc_name.TabIndex = 6;
            this.Doc_name.Text = "File Name/No.";
            this.Doc_name.Click += new System.EventHandler(this.Doc_name_Click);
            // 
            // Doc_description
            // 
            this.Doc_description.AutoSize = true;
            this.Doc_description.Location = new System.Drawing.Point(318, 81);
            this.Doc_description.Name = "Doc_description";
            this.Doc_description.Size = new System.Drawing.Size(0, 13);
            this.Doc_description.TabIndex = 7;
            // 
            // OpenDrawingButton
            // 
            this.OpenDrawingButton.Location = new System.Drawing.Point(423, 417);
            this.OpenDrawingButton.Name = "OpenDrawingButton";
            this.OpenDrawingButton.Size = new System.Drawing.Size(75, 23);
            this.OpenDrawingButton.TabIndex = 50;
            this.OpenDrawingButton.Text = "Open";
            this.OpenDrawingButton.UseVisualStyleBackColor = true;
            this.OpenDrawingButton.Click += new System.EventHandler(this.OpenDrawingButton_Click);
            // 
            // FormCancelButton
            // 
            this.FormCancelButton.Location = new System.Drawing.Point(549, 417);
            this.FormCancelButton.Name = "FormCancelButton";
            this.FormCancelButton.Size = new System.Drawing.Size(75, 23);
            this.FormCancelButton.TabIndex = 60;
            this.FormCancelButton.Text = "Cancel";
            this.FormCancelButton.UseVisualStyleBackColor = true;
            this.FormCancelButton.Click += new System.EventHandler(this.FormCancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "File Type";
            // 
            // CDType
            // 
            this.CDType.DisplayMember = "AssemblyModel, PartModel, Drawing";
            this.CDType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CDType.FormattingEnabled = true;
            this.CDType.Location = new System.Drawing.Point(102, 81);
            this.CDType.Name = "CDType";
            this.CDType.Size = new System.Drawing.Size(121, 21);
            this.CDType.TabIndex = 30;
            this.CDType.ValueMember = "AssemblyModel, PartModel, Drawing";
            this.CDType.SelectedIndexChanged += new System.EventHandler(this.CDType_SelectedIndexChanged);
            // 
            // treeGridView1
            // 
            this.treeGridView1.AllowUserToAddRows = false;
            this.treeGridView1.AllowUserToDeleteRows = false;
            this.treeGridView1.AllowUserToOrderColumns = true;
            this.treeGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.treeGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.treeGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.treeGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            this.ExpandButton,
            this.DrawingName,
            this.DrawingNumber,
            this.LockStatus,
            this.CADType,
            this.State,
            this.Generation,
            this.ProjectId,
            this.ProjectName,
            this.Size,
            this.DrawingID,
            this.LockBy,
            this.OwnerCompany,
            this.Checkout});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.treeGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.treeGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.treeGridView1.ImageList = null;
            this.treeGridView1.Location = new System.Drawing.Point(12, 168);
            this.treeGridView1.Name = "treeGridView1";
            this.treeGridView1.RowHeadersVisible = false;
            this.treeGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.treeGridView1.Size = new System.Drawing.Size(1004, 233);
            this.treeGridView1.TabIndex = 3;
            this.treeGridView1.NodeExpanding += new AdvancedDataGridView.ExpandingEventHandler(this.treeGridView1_NodeExpanding);
            this.treeGridView1.NodeCollapsing += new AdvancedDataGridView.CollapsingEventHandler(this.treeGridView1_NodeCollapsing);
            this.treeGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.treeGridView1_CellContentClick_1);
            this.treeGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.treeGridView1_CellBeginEdit);
            // 
            // Check
            // 
            this.Check.FillWeight = 51.53443F;
            this.Check.HeaderText = "";
            this.Check.MinimumWidth = 20;
            this.Check.Name = "Check";
            // 
            // ExpandButton
            // 
            this.ExpandButton.FillWeight = 20.53443F;
            this.ExpandButton.HeaderText = "";
            this.ExpandButton.Name = "ExpandButton";
            this.ExpandButton.ReadOnly = true;
            this.ExpandButton.Text = "Expand Node";
            // 
            // DrawingName
            // 
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DrawingName.DefaultCellStyle = dataGridViewCellStyle1;
            this.DrawingName.DefaultNodeImage = null;
            this.DrawingName.FillWeight = 186.9562F;
            this.DrawingName.HeaderText = "Name";
            this.DrawingName.Name = "DrawingName";
            this.DrawingName.ReadOnly = true;
            this.DrawingName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DrawingNumber
            // 
            this.DrawingNumber.HeaderText = "Number";
            this.DrawingNumber.Name = "DrawingNumber";
            this.DrawingNumber.ReadOnly = true;
            this.DrawingNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LockStatus
            // 
            this.LockStatus.FillWeight = 50F;
            this.LockStatus.HeaderText = "Lock";
            this.LockStatus.Name = "LockStatus";
            this.LockStatus.ReadOnly = true;
            this.LockStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LockStatus.Visible = false;
            // 
            // CADType
            // 
            this.CADType.HeaderText = "Type";
            this.CADType.Name = "CADType";
            this.CADType.ReadOnly = true;
            this.CADType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // State
            // 
            this.State.FillWeight = 70F;
            this.State.HeaderText = "Status";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            this.State.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Generation
            // 
            this.Generation.FillWeight = 30F;
            this.Generation.HeaderText = "Ver No";
            this.Generation.Name = "Generation";
            this.Generation.ReadOnly = true;
            this.Generation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ProjectId
            // 
            this.ProjectId.HeaderText = "Project";
            this.ProjectId.Name = "ProjectId";
            this.ProjectId.ReadOnly = true;
            this.ProjectId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ProjectName
            // 
            this.ProjectName.FillWeight = 70F;
            this.ProjectName.HeaderText = "Project No";
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.ReadOnly = true;
            this.ProjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Size
            // 
            this.Size.FillWeight = 70F;
            this.Size.HeaderText = "Size";
            this.Size.Name = "Size";
            this.Size.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DrawingID
            // 
            this.DrawingID.FillWeight = 50F;
            this.DrawingID.HeaderText = "DrawingID";
            this.DrawingID.Name = "DrawingID";
            this.DrawingID.ReadOnly = true;
            this.DrawingID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DrawingID.Visible = false;
            // 
            // LockBy
            // 
            this.LockBy.FillWeight = 50F;
            this.LockBy.HeaderText = "LockBy";
            this.LockBy.Name = "LockBy";
            this.LockBy.ReadOnly = true;
            this.LockBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LockBy.Visible = false;
            // 
            // OwnerCompany
            // 
            this.OwnerCompany.FillWeight = 150F;
            this.OwnerCompany.HeaderText = "Owner Company";
            this.OwnerCompany.Name = "OwnerCompany";
            this.OwnerCompany.ReadOnly = true;
            this.OwnerCompany.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.OwnerCompany.Visible = false;
            // 
            // Checkout
            // 
            this.Checkout.FillWeight = 35F;
            this.Checkout.HeaderText = "Lock";
            this.Checkout.Name = "Checkout";
            this.Checkout.Visible = false;
            // 
            // imageStrip
            // 
            this.imageStrip.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageStrip.ImageSize = new System.Drawing.Size(17, 17);
            this.imageStrip.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CDRevisionValue
            // 
            this.CDRevisionValue.Location = new System.Drawing.Point(213, 428);
            this.CDRevisionValue.Name = "CDRevisionValue";
            this.CDRevisionValue.Size = new System.Drawing.Size(123, 20);
            this.CDRevisionValue.TabIndex = 40;
            this.CDRevisionValue.Visible = false;
            this.CDRevisionValue.TextChanged += new System.EventHandler(this.CDRevisionValue_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(155, 430);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Revision";
            this.label2.Visible = false;
            // 
            // CheckOutViewCBox
            // 
            this.CheckOutViewCBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CheckOutViewCBox.Enabled = false;
            this.CheckOutViewCBox.FormattingEnabled = true;
            this.CheckOutViewCBox.Items.AddRange(new object[] {
            "Current",
            "Released",
            "AsSaved"});
            this.CheckOutViewCBox.Location = new System.Drawing.Point(817, 413);
            this.CheckOutViewCBox.Name = "CheckOutViewCBox";
            this.CheckOutViewCBox.Size = new System.Drawing.Size(165, 21);
            this.CheckOutViewCBox.TabIndex = 50;
            this.CheckOutViewCBox.Visible = false;
            this.CheckOutViewCBox.SelectedIndexChanged += new System.EventHandler(this.CheckOutViewCBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(719, 415);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 62;
            this.label3.Text = "CheckOutView";
            this.label3.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(67, 426);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 64;
            this.label4.Text = "Project No";
            this.label4.Visible = false;
            // 
            // CDProjectName
            // 
            this.CDProjectName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CDProjectName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CDProjectName.FormattingEnabled = true;
            this.CDProjectName.Location = new System.Drawing.Point(785, 27);
            this.CDProjectName.Name = "CDProjectName";
            this.CDProjectName.Size = new System.Drawing.Size(159, 21);
            this.CDProjectName.TabIndex = 70;
            this.CDProjectName.SelectedIndexChanged += new System.EventHandler(this.CDProjectName_SelectedIndexChanged);
            this.CDProjectName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CDProjectName_KeyUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(664, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 13);
            this.label5.TabIndex = 65;
            this.label5.Text = "Project Name";
            // 
            // CDProjectId
            // 
            this.CDProjectId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CDProjectId.FormattingEnabled = true;
            this.CDProjectId.Location = new System.Drawing.Point(151, 425);
            this.CDProjectId.Name = "CDProjectId";
            this.CDProjectId.Size = new System.Drawing.Size(121, 21);
            this.CDProjectId.TabIndex = 60;
            this.CDProjectId.Visible = false;
            // 
            // CDState
            // 
            this.CDState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CDState.FormattingEnabled = true;
            this.CDState.Location = new System.Drawing.Point(416, 79);
            this.CDState.Name = "CDState";
            this.CDState.Size = new System.Drawing.Size(161, 21);
            this.CDState.TabIndex = 80;
            this.CDState.SelectedIndexChanged += new System.EventHandler(this.CDState_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(285, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 68;
            this.label6.Text = "Status";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(124, 413);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 71;
            this.label7.Text = "RealtyEntity";
            this.label7.Visible = false;
            // 
            // CDRealty
            // 
            this.CDRealty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CDRealty.FormattingEnabled = true;
            this.CDRealty.Location = new System.Drawing.Point(244, 407);
            this.CDRealty.Name = "CDRealty";
            this.CDRealty.Size = new System.Drawing.Size(107, 21);
            this.CDRealty.TabIndex = 90;
            this.CDRealty.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(18, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 13);
            this.label8.TabIndex = 72;
            this.label8.Text = "Location";
            // 
            // sg_SearchType
            // 
            this.sg_SearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sg_SearchType.FormattingEnabled = true;
            this.sg_SearchType.Items.AddRange(new object[] {
            "All",
            "My files",
            "Projects",
            "Knowledge"});
            this.sg_SearchType.Location = new System.Drawing.Point(102, 25);
            this.sg_SearchType.Name = "sg_SearchType";
            this.sg_SearchType.Size = new System.Drawing.Size(121, 21);
            this.sg_SearchType.TabIndex = 100;
            this.sg_SearchType.SelectedIndexChanged += new System.EventHandler(this.sg_SearchType_SelectedIndexChanged);
            // 
            // searchStatus
            // 
            this.searchStatus.AutoSize = true;
            this.searchStatus.Location = new System.Drawing.Point(12, 422);
            this.searchStatus.Name = "searchStatus";
            this.searchStatus.Size = new System.Drawing.Size(96, 13);
            this.searchStatus.TabIndex = 101;
            this.searchStatus.Text = "Ready to Search...";
            // 
            // busyLabel
            // 
            this.busyLabel.AutoSize = true;
            this.busyLabel.Location = new System.Drawing.Point(12, 452);
            this.busyLabel.Name = "busyLabel";
            this.busyLabel.Size = new System.Drawing.Size(139, 13);
            this.busyLabel.TabIndex = 102;
            this.busyLabel.Text = "Avrut is Busy in Searching...";
            this.busyLabel.Visible = false;
            // 
            // label_foldername
            // 
            this.label_foldername.AutoSize = true;
            this.label_foldername.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_foldername.Location = new System.Drawing.Point(664, 87);
            this.label_foldername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_foldername.Name = "label_foldername";
            this.label_foldername.Size = new System.Drawing.Size(82, 13);
            this.label_foldername.TabIndex = 103;
            this.label_foldername.Text = " Folder Name";
            this.label_foldername.Click += new System.EventHandler(this.label_foldername_Click);
            // 
            // textBox_foldername
            // 
            this.textBox_foldername.Location = new System.Drawing.Point(785, 81);
            this.textBox_foldername.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBox_foldername.Name = "textBox_foldername";
            this.textBox_foldername.Size = new System.Drawing.Size(159, 20);
            this.textBox_foldername.TabIndex = 104;
            this.textBox_foldername.TextChanged += new System.EventHandler(this.textBox_foldername_TextChanged);
            // 
            // Doc_Number
            // 
            this.Doc_Number.AutoSize = true;
            this.Doc_Number.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Doc_Number.Location = new System.Drawing.Point(742, 451);
            this.Doc_Number.Name = "Doc_Number";
            this.Doc_Number.Size = new System.Drawing.Size(47, 13);
            this.Doc_Number.TabIndex = 5;
            this.Doc_Number.Text = "File No";
            this.Doc_Number.Visible = false;
            // 
            // Search_And_Open
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(913, 473);
            this.Controls.Add(this.textBox_foldername);
            this.Controls.Add(this.label_foldername);
            this.Controls.Add(this.busyLabel);
            this.Controls.Add(this.searchStatus);
            this.Controls.Add(this.sg_SearchType);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.CDRealty);
            this.Controls.Add(this.CDState);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.CDProjectId);
            this.Controls.Add(this.CDProjectName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CheckOutViewCBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CDRevisionValue);
            this.Controls.Add(this.treeGridView1);
            this.Controls.Add(this.CDType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FormCancelButton);
            this.Controls.Add(this.OpenDrawingButton);
            this.Controls.Add(this.Doc_description);
            this.Controls.Add(this.Doc_name);
            this.Controls.Add(this.Doc_Number);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.DGName);
            this.Controls.Add(this.DGNumber);
            this.Name = "Search_And_Open";
            this.Text = "Search and Open";
            this.Load += new System.EventHandler(this.Search_And_Open_Load);
            this.Resize += new System.EventHandler(this.Search_And_Open_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.treeGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DGNumber;
        private System.Windows.Forms.TextBox DGName;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label Doc_name;
        private System.Windows.Forms.Label Doc_description;
        private System.Windows.Forms.Button OpenDrawingButton;
        private System.Windows.Forms.Button FormCancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CDType;
        private System.Windows.Forms.DataGridView dataGridView1;
        private AdvancedDataGridView.TreeGridView treeGridView1;

        private System.Windows.Forms.ImageList imageStrip;
        private System.Windows.Forms.TextBox CDRevisionValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CheckOutViewCBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CDProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CDProjectId;
        private System.Windows.Forms.ComboBox CDState;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox CDRealty;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox sg_SearchType;
        private System.Windows.Forms.Label searchStatus;
        private System.Windows.Forms.Label busyLabel;
        private System.Windows.Forms.Label label_foldername;
        private System.Windows.Forms.TextBox textBox_foldername;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private System.Windows.Forms.DataGridViewButtonColumn ExpandButton;
        private TreeGridColumn DrawingName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn LockStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn CADType;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn Generation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Size;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LockBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn OwnerCompany;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Checkout;
        private System.Windows.Forms.Label Doc_Number;
    }
}