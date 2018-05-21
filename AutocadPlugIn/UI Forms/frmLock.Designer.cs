namespace AutocadPlugIn.UI_Forms
{
    partial class frmLock
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
            this.LockTree = new AdvancedDataGridView.TreeGridView();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.LockBtn = new System.Windows.Forms.Button();
            this.imageStrip = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DrawingName = new AdvancedDataGridView.TreeGridColumn();
            this.DrawingNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CADType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Revision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DrawingID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LockStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LockStatus1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.LockTree)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // LockTree
            // 
            this.LockTree.AllowUserToAddRows = false;
            this.LockTree.AllowUserToDeleteRows = false;
            this.LockTree.AllowUserToOrderColumns = true;
            this.LockTree.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.LockTree.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.LockTree.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.LockTree.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            this.DrawingName,
            this.DrawingNumber,
            this.CADType,
            this.Revision,
            this.DrawingID,
            this.LockStatus,
            this.LockStatus1,
            this.ProjectName,
            this.ProjectId,
            this.State});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.LockTree.DefaultCellStyle = dataGridViewCellStyle2;
            this.LockTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LockTree.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.LockTree.ImageList = null;
            this.LockTree.Location = new System.Drawing.Point(3, 3);
            this.LockTree.Name = "LockTree";
            this.LockTree.RowHeadersVisible = false;
            this.LockTree.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LockTree.Size = new System.Drawing.Size(638, 315);
            this.LockTree.TabIndex = 4;
            this.LockTree.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.LockTree_CellContentClick);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CancelBtn.Location = new System.Drawing.Point(491, 3);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(144, 33);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // LockBtn
            // 
            this.LockBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LockBtn.Location = new System.Drawing.Point(341, 3);
            this.LockBtn.Name = "LockBtn";
            this.LockBtn.Size = new System.Drawing.Size(144, 33);
            this.LockBtn.TabIndex = 5;
            this.LockBtn.Text = "Lock";
            this.LockBtn.UseVisualStyleBackColor = true;
            this.LockBtn.Click += new System.EventHandler(this.LockBtn_Click);
            // 
            // imageStrip
            // 
            this.imageStrip.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageStrip.ImageSize = new System.Drawing.Size(16, 16);
            this.imageStrip.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.LockTree, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.97814F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.02186F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(644, 366);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.Controls.Add(this.LockBtn, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.CancelBtn, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 324);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(638, 39);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // Check
            // 
            this.Check.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Check.FillWeight = 177.665F;
            this.Check.HeaderText = "Check";
            this.Check.Name = "Check";
            this.Check.Width = 80;
            // 
            // DrawingName
            // 
            this.DrawingName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DrawingName.DefaultNodeImage = null;
            this.DrawingName.FillWeight = 87.05584F;
            this.DrawingName.HeaderText = "Drawing Name";
            this.DrawingName.Name = "DrawingName";
            this.DrawingName.ReadOnly = true;
            this.DrawingName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DrawingName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DrawingName.Width = 200;
            // 
            // DrawingNumber
            // 
            this.DrawingNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.DrawingNumber.FillWeight = 87.05584F;
            this.DrawingNumber.HeaderText = "Drawing Number";
            this.DrawingNumber.Name = "DrawingNumber";
            this.DrawingNumber.ReadOnly = true;
            this.DrawingNumber.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DrawingNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DrawingNumber.Width = 92;
            // 
            // CADType
            // 
            this.CADType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.CADType.FillWeight = 87.05584F;
            this.CADType.HeaderText = "CAD Type";
            this.CADType.Name = "CADType";
            this.CADType.ReadOnly = true;
            this.CADType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.CADType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.CADType.Width = 62;
            // 
            // Revision
            // 
            this.Revision.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Revision.FillWeight = 87.05584F;
            this.Revision.HeaderText = "Revision";
            this.Revision.Name = "Revision";
            this.Revision.ReadOnly = true;
            this.Revision.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Revision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Revision.Width = 54;
            // 
            // DrawingID
            // 
            this.DrawingID.HeaderText = "DrawingID";
            this.DrawingID.Name = "DrawingID";
            this.DrawingID.ReadOnly = true;
            this.DrawingID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.DrawingID.Visible = false;
            // 
            // LockStatus
            // 
            this.LockStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.LockStatus.FillWeight = 87.05584F;
            this.LockStatus.HeaderText = "Lock Status";
            this.LockStatus.Name = "LockStatus";
            this.LockStatus.ReadOnly = true;
            this.LockStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LockStatus.Width = 70;
            // 
            // LockStatus1
            // 
            this.LockStatus1.HeaderText = "LockStatus1";
            this.LockStatus1.Name = "LockStatus1";
            this.LockStatus1.ReadOnly = true;
            this.LockStatus1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LockStatus1.Visible = false;
            // 
            // ProjectName
            // 
            this.ProjectName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ProjectName.FillWeight = 87.05584F;
            this.ProjectName.HeaderText = "Project Name";
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.ReadOnly = true;
            this.ProjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProjectName.Width = 77;
            // 
            // ProjectId
            // 
            this.ProjectId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ProjectId.HeaderText = "ProjectId";
            this.ProjectId.Name = "ProjectId";
            this.ProjectId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProjectId.Visible = false;
            this.ProjectId.Width = 55;
            // 
            // State
            // 
            this.State.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.State.Visible = false;
            this.State.Width = 38;
            // 
            // frmLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 366);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmLock";
            this.Text = "Lock";
            this.Load += new System.EventHandler(this.frmLock_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LockTree)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AdvancedDataGridView.TreeGridView LockTree;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button LockBtn;
        private System.Windows.Forms.ImageList imageStrip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private AdvancedDataGridView.TreeGridColumn DrawingName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn CADType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Revision;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LockStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn LockStatus1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectId;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
    }
}