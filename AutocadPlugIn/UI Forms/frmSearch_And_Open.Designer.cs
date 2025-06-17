using AdvancedDataGridView;
namespace RBAutocadPlugIn.UI_Forms
{
    partial class frmSearch_And_Open
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.DGName = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.Doc_name = new System.Windows.Forms.Label();
            this.Doc_description = new System.Windows.Forms.Label();
            this.OpenDrawingButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CDType = new System.Windows.Forms.ComboBox();
            this.treeGridView1 = new AdvancedDataGridView.TreeGridView();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ExpandButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.DrawingName = new AdvancedDataGridView.TreeGridColumn();
            this.IsXRefFile = new System.Windows.Forms.DataGridViewImageColumn();
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
            this.CDProjectName = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CDState = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.sg_SearchType = new System.Windows.Forms.ComboBox();
            this.searchStatus = new System.Windows.Forms.Label();
            this.busyLabel = new System.Windows.Forms.Label();
            this.label_foldername = new System.Windows.Forms.Label();
            this.textBox_foldername = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.FormCancelButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.treeGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // DGName
            // 
            this.DGName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DGName.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DGName.Location = new System.Drawing.Point(421, 13);
            this.DGName.Name = "DGName";
            this.DGName.Size = new System.Drawing.Size(177, 22);
            this.DGName.TabIndex = 20;
            // 
            // SearchButton
            // 
            this.SearchButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchButton.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchButton.Location = new System.Drawing.Point(376, 3);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(94, 33);
            this.SearchButton.TabIndex = 0;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CancelButton.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelButton.Location = new System.Drawing.Point(476, 3);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(94, 33);
            this.CancelButton.TabIndex = 1;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // Doc_name
            // 
            this.Doc_name.AutoSize = true;
            this.Doc_name.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Doc_name.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Doc_name.Location = new System.Drawing.Point(296, 10);
            this.Doc_name.Name = "Doc_name";
            this.Doc_name.Size = new System.Drawing.Size(119, 27);
            this.Doc_name.TabIndex = 6;
            this.Doc_name.Text = "File Name/No. : ";
            this.Doc_name.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Doc_description
            // 
            this.Doc_description.AutoSize = true;
            this.Doc_description.Location = new System.Drawing.Point(295, 81);
            this.Doc_description.Name = "Doc_description";
            this.Doc_description.Size = new System.Drawing.Size(0, 13);
            this.Doc_description.TabIndex = 7;
            // 
            // OpenDrawingButton
            // 
            this.OpenDrawingButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OpenDrawingButton.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenDrawingButton.Location = new System.Drawing.Point(376, 3);
            this.OpenDrawingButton.Name = "OpenDrawingButton";
            this.OpenDrawingButton.Size = new System.Drawing.Size(94, 33);
            this.OpenDrawingButton.TabIndex = 50;
            this.OpenDrawingButton.Text = "Open";
            this.OpenDrawingButton.UseVisualStyleBackColor = true;
            this.OpenDrawingButton.Click += new System.EventHandler(this.OpenDrawingButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 27);
            this.label1.TabIndex = 11;
            this.label1.Text = "File Type : ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CDType
            // 
            this.CDType.DisplayMember = "AssemblyModel, PartModel, Drawing";
            this.CDType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CDType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CDType.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CDType.FormattingEnabled = true;
            this.CDType.Location = new System.Drawing.Point(113, 50);
            this.CDType.Name = "CDType";
            this.CDType.Size = new System.Drawing.Size(177, 22);
            this.CDType.TabIndex = 30;
            this.CDType.ValueMember = "AssemblyModel, PartModel, Drawing";
            // 
            // treeGridView1
            // 
            this.treeGridView1.AllowUserToAddRows = false;
            this.treeGridView1.AllowUserToDeleteRows = false;
            this.treeGridView1.AllowUserToOrderColumns = true;
            this.treeGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.treeGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.treeGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            this.ExpandButton,
            this.DrawingName,
            this.IsXRefFile,
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
            this.treeGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.treeGridView1.GridColor = System.Drawing.SystemColors.Control;
            this.treeGridView1.ImageList = null;
            this.treeGridView1.Location = new System.Drawing.Point(3, 138);
            this.treeGridView1.Name = "treeGridView1";
            this.treeGridView1.RowHeadersVisible = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeGridView1.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.treeGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.treeGridView1.Size = new System.Drawing.Size(947, 268);
            this.treeGridView1.TabIndex = 3;
            this.treeGridView1.NodeExpanding += new AdvancedDataGridView.ExpandingEventHandler(this.treeGridView1_NodeExpanding);
            this.treeGridView1.NodeCollapsing += new AdvancedDataGridView.CollapsingEventHandler(this.treeGridView1_NodeCollapsing);
            this.treeGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.treeGridView1_CellContentClick_1);
            this.treeGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.treeGridView1_CellBeginEdit);
            this.treeGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.treeGridView1_DataError);
            // 
            // Check
            // 
            this.Check.FillWeight = 55.92374F;
            this.Check.HeaderText = "";
            this.Check.MinimumWidth = 20;
            this.Check.Name = "Check";
            this.Check.Width = 61;
            // 
            // ExpandButton
            // 
            this.ExpandButton.FillWeight = 22.2834F;
            this.ExpandButton.HeaderText = "";
            this.ExpandButton.Name = "ExpandButton";
            this.ExpandButton.ReadOnly = true;
            this.ExpandButton.Text = "Expand Node";
            this.ExpandButton.Width = 25;
            // 
            // DrawingName
            // 
            this.DrawingName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DrawingName.DefaultCellStyle = dataGridViewCellStyle1;
            this.DrawingName.DefaultNodeImage = null;
            this.DrawingName.FillWeight = 202.8797F;
            this.DrawingName.HeaderText = "Name";
            this.DrawingName.Name = "DrawingName";
            this.DrawingName.ReadOnly = true;
            this.DrawingName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DrawingName.Width = 227;
            // 
            // IsXRefFile
            // 
            this.IsXRefFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.IsXRefFile.DefaultCellStyle = dataGridViewCellStyle2;
            this.IsXRefFile.FillWeight = 31.94505F;
            this.IsXRefFile.HeaderText = "XRef";
            this.IsXRefFile.Image = global::RBAutocadPlugIn.Properties.Resources.BG;
            this.IsXRefFile.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.IsXRefFile.Name = "IsXRefFile";
            this.IsXRefFile.ReadOnly = true;
            this.IsXRefFile.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.IsXRefFile.Width = 40;
            // 
            // DrawingNumber
            // 
            this.DrawingNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DrawingNumber.FillWeight = 108.5172F;
            this.DrawingNumber.HeaderText = "Number";
            this.DrawingNumber.Name = "DrawingNumber";
            this.DrawingNumber.ReadOnly = true;
            this.DrawingNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LockStatus
            // 
            this.LockStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LockStatus.FillWeight = 50F;
            this.LockStatus.HeaderText = "Lock";
            this.LockStatus.Name = "LockStatus";
            this.LockStatus.ReadOnly = true;
            this.LockStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LockStatus.Visible = false;
            // 
            // CADType
            // 
            this.CADType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.CADType.FillWeight = 108.5172F;
            this.CADType.HeaderText = "Type";
            this.CADType.Name = "CADType";
            this.CADType.ReadOnly = true;
            this.CADType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CADType.Width = 122;
            // 
            // State
            // 
            this.State.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.State.FillWeight = 75.96207F;
            this.State.HeaderText = "Status";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            this.State.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.State.Width = 85;
            // 
            // Generation
            // 
            this.Generation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Generation.FillWeight = 32.55518F;
            this.Generation.HeaderText = "Ver No";
            this.Generation.Name = "Generation";
            this.Generation.ReadOnly = true;
            this.Generation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Generation.Width = 52;
            // 
            // ProjectId
            // 
            this.ProjectId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ProjectId.FillWeight = 108.5172F;
            this.ProjectId.HeaderText = "Project";
            this.ProjectId.Name = "ProjectId";
            this.ProjectId.ReadOnly = true;
            this.ProjectId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ProjectName
            // 
            this.ProjectName.FillWeight = 75.96207F;
            this.ProjectName.HeaderText = "Project No";
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.ReadOnly = true;
            this.ProjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProjectName.Visible = false;
            this.ProjectName.Width = 84;
            // 
            // Size
            // 
            this.Size.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Size.FillWeight = 75.96207F;
            this.Size.HeaderText = "Size";
            this.Size.Name = "Size";
            this.Size.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Size.Width = 37;
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
            // CDProjectName
            // 
            this.CDProjectName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.CDProjectName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CDProjectName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CDProjectName.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CDProjectName.FormattingEnabled = true;
            this.CDProjectName.Location = new System.Drawing.Point(754, 13);
            this.CDProjectName.Name = "CDProjectName";
            this.CDProjectName.Size = new System.Drawing.Size(177, 22);
            this.CDProjectName.TabIndex = 70;
            this.CDProjectName.SelectedIndexChanged += new System.EventHandler(this.CDProjectName_SelectedIndexChanged);
            this.CDProjectName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CDProjectName_KeyUp);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(604, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 27);
            this.label5.TabIndex = 65;
            this.label5.Text = "Project Name/No. : ";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // CDState
            // 
            this.CDState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CDState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CDState.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CDState.FormattingEnabled = true;
            this.CDState.Location = new System.Drawing.Point(421, 50);
            this.CDState.Name = "CDState";
            this.CDState.Size = new System.Drawing.Size(177, 22);
            this.CDState.TabIndex = 80;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(296, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 27);
            this.label6.TabIndex = 68;
            this.label6.Text = "Status : ";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(13, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 27);
            this.label8.TabIndex = 72;
            this.label8.Text = "Location : ";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sg_SearchType
            // 
            this.sg_SearchType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sg_SearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sg_SearchType.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sg_SearchType.FormattingEnabled = true;
            this.sg_SearchType.Items.AddRange(new object[] {
            "All",
            "My files",
            "Projects"});
            this.sg_SearchType.Location = new System.Drawing.Point(113, 13);
            this.sg_SearchType.Name = "sg_SearchType";
            this.sg_SearchType.Size = new System.Drawing.Size(177, 22);
            this.sg_SearchType.TabIndex = 100;
            this.sg_SearchType.SelectedIndexChanged += new System.EventHandler(this.sg_SearchType_SelectedIndexChanged);
            // 
            // searchStatus
            // 
            this.searchStatus.AutoSize = true;
            this.searchStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchStatus.Location = new System.Drawing.Point(3, 0);
            this.searchStatus.Name = "searchStatus";
            this.searchStatus.Size = new System.Drawing.Size(367, 19);
            this.searchStatus.TabIndex = 101;
            this.searchStatus.Text = "Ready to Search...";
            this.searchStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // busyLabel
            // 
            this.busyLabel.AutoSize = true;
            this.busyLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.busyLabel.Location = new System.Drawing.Point(3, 19);
            this.busyLabel.Name = "busyLabel";
            this.busyLabel.Size = new System.Drawing.Size(367, 20);
            this.busyLabel.TabIndex = 102;
            this.busyLabel.Text = "RedBracket Connector is Busy in Searching...";
            this.busyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.busyLabel.Visible = false;
            // 
            // label_foldername
            // 
            this.label_foldername.AutoSize = true;
            this.label_foldername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label_foldername.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_foldername.Location = new System.Drawing.Point(603, 47);
            this.label_foldername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label_foldername.Name = "label_foldername";
            this.label_foldername.Size = new System.Drawing.Size(146, 27);
            this.label_foldername.TabIndex = 103;
            this.label_foldername.Text = "Folder Name : ";
            this.label_foldername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox_foldername
            // 
            this.textBox_foldername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_foldername.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_foldername.Location = new System.Drawing.Point(753, 48);
            this.textBox_foldername.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.textBox_foldername.Name = "textBox_foldername";
            this.textBox_foldername.Size = new System.Drawing.Size(179, 22);
            this.textBox_foldername.TabIndex = 104;
            this.textBox_foldername.EnabledChanged += new System.EventHandler(this.textBox_foldername_EnabledChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 183F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 183F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 183F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Controls.Add(this.label8, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBox_foldername, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.sg_SearchType, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label_foldername, 5, 3);
            this.tableLayoutPanel1.Controls.Add(this.Doc_name, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.DGName, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.CDState, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.CDProjectName, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.CDType, 2, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(947, 84);
            this.tableLayoutPanel1.TabIndex = 105;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.SearchButton, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.CancelButton, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 93);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(947, 39);
            this.tableLayoutPanel2.TabIndex = 106;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.FormCancelButton, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.OpenDrawingButton, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 412);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(947, 39);
            this.tableLayoutPanel3.TabIndex = 107;
            // 
            // FormCancelButton
            // 
            this.FormCancelButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FormCancelButton.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormCancelButton.Location = new System.Drawing.Point(476, 3);
            this.FormCancelButton.Name = "FormCancelButton";
            this.FormCancelButton.Size = new System.Drawing.Size(94, 33);
            this.FormCancelButton.TabIndex = 60;
            this.FormCancelButton.Text = "Cancel";
            this.FormCancelButton.UseVisualStyleBackColor = true;
            this.FormCancelButton.Click += new System.EventHandler(this.FormCancelButton_Click);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.searchStatus, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.busyLabel, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(373, 39);
            this.tableLayoutPanel4.TabIndex = 61;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel3, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.treeGridView1, 0, 2);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(13, 43);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 4;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(953, 454);
            this.tableLayoutPanel5.TabIndex = 108;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel6.Controls.Add(this.pnlTop, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.tableLayoutPanel5, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.pnlLeft, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.pnlBottom, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.pnlRight, 2, 1);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 3;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(979, 510);
            this.tableLayoutPanel6.TabIndex = 109;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.tableLayoutPanel6.SetColumnSpan(this.pnlTop, 3);
            this.pnlTop.Controls.Add(this.label2);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(979, 40);
            this.pnlTop.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(7, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(241, 29);
            this.label2.TabIndex = 12;
            this.label2.Text = "Search and Open";
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 40);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(10, 460);
            this.pnlLeft.TabIndex = 0;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.tableLayoutPanel6.SetColumnSpan(this.pnlBottom, 3);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(0, 500);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(979, 10);
            this.pnlBottom.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(969, 40);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(10, 460);
            this.pnlRight.TabIndex = 0;
            // 
            // frmSearch_And_Open
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(979, 510);
            this.Controls.Add(this.tableLayoutPanel6);
            this.Controls.Add(this.Doc_description);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmSearch_And_Open";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Search and Open";
            this.Load += new System.EventHandler(this.Search_And_Open_Load);
            ((System.ComponentModel.ISupportInitialize)(this.treeGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.ComboBox CDProjectName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CDState;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox sg_SearchType;
        private System.Windows.Forms.Label searchStatus;
        private System.Windows.Forms.Label busyLabel;
        private System.Windows.Forms.Label label_foldername;
        private System.Windows.Forms.TextBox textBox_foldername;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private System.Windows.Forms.DataGridViewButtonColumn ExpandButton;
        private TreeGridColumn DrawingName;
        private System.Windows.Forms.DataGridViewImageColumn IsXRefFile;
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
    }
}