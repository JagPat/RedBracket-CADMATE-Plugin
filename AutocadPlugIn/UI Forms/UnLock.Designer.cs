using AdvancedDataGridView;
namespace AutocadPlugIn.UI_Forms
{
    partial class UnLock
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
            this.UnLockBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.UnLockTree = new AdvancedDataGridView.TreeGridView();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DrawingName = new AdvancedDataGridView.TreeGridColumn();
            this.DrawingNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CADType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Revision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DrawingID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LockStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Expand = new System.Windows.Forms.DataGridViewButtonColumn();
            this.imageStrip = new System.Windows.Forms.ImageList(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.UnLockTree)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // UnLockBtn
            // 
            this.UnLockBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UnLockBtn.Location = new System.Drawing.Point(667, 3);
            this.UnLockBtn.Name = "UnLockBtn";
            this.UnLockBtn.Size = new System.Drawing.Size(144, 39);
            this.UnLockBtn.TabIndex = 1;
            this.UnLockBtn.Text = "UnLock";
            this.UnLockBtn.UseVisualStyleBackColor = true;
            this.UnLockBtn.Click += new System.EventHandler(this.UnLockBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CancelBtn.Location = new System.Drawing.Point(817, 3);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(144, 39);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // UnLockTree
            // 
            this.UnLockTree.AllowUserToAddRows = false;
            this.UnLockTree.AllowUserToDeleteRows = false;
            this.UnLockTree.AllowUserToOrderColumns = true;
            this.UnLockTree.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.UnLockTree.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.UnLockTree.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.UnLockTree.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            this.DrawingName,
            this.DrawingNumber,
            this.CADType,
            this.Revision,
            this.DrawingID,
            this.LockStatus,
            this.ProjectName,
            this.ProjectId,
            this.State});
            this.UnLockTree.DefaultCellStyle = dataGridViewCellStyle1;
            this.UnLockTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UnLockTree.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.UnLockTree.ImageList = null;
            this.UnLockTree.Location = new System.Drawing.Point(3, 3);
            this.UnLockTree.Name = "UnLockTree";
            this.UnLockTree.RowHeadersVisible = false;
            this.UnLockTree.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.UnLockTree.Size = new System.Drawing.Size(964, 401);
            this.UnLockTree.TabIndex = 3;
            this.UnLockTree.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.UnLockTree_CellContentClick);
            // 
            // Check
            // 
            this.Check.FillWeight = 51.53443F;
            this.Check.HeaderText = "";
            this.Check.MinimumWidth = 25;
            this.Check.Name = "Check";
            // 
            // DrawingName
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DrawingName.DefaultCellStyle = dataGridViewCellStyle1;
            this.DrawingName.DefaultNodeImage = null;
            this.DrawingName.FillWeight = 186.9562F;
            this.DrawingName.HeaderText = "Drawing";
            this.DrawingName.Name = "DrawingName";
            this.DrawingName.ReadOnly = true;
            this.DrawingName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DrawingNumber
            // 
            this.DrawingNumber.HeaderText = "Drawing Number";
            this.DrawingNumber.Name = "DrawingNumber";
            this.DrawingNumber.ReadOnly = true;
            this.DrawingNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CADType
            // 
            this.CADType.FillWeight = 150F;
            this.CADType.HeaderText = "CAD Type";
            this.CADType.Name = "CADType";
            this.CADType.ReadOnly = true;
            this.CADType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Revision
            // 
            this.Revision.FillWeight = 50F;
            this.Revision.HeaderText = "Revision";
            this.Revision.Name = "Revision";
            this.Revision.ReadOnly = true;
            this.Revision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // LockStatus
            // 
            this.LockStatus.FillWeight = 50F;
            this.LockStatus.HeaderText = "LockStatus";
            this.LockStatus.Name = "LockStatus";
            this.LockStatus.ReadOnly = true;
            this.LockStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LockStatus.Visible = false;
            // 
            // ProjectName
            // 
            this.ProjectName.HeaderText = "ProjectName";
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.ReadOnly = true;
            this.ProjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ProjectId
            // 
            this.ProjectId.HeaderText = "ProjectId";
            this.ProjectId.Name = "ProjectId";
            this.ProjectId.ReadOnly = true;
            this.ProjectId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // State
            // 
            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            this.State.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Expand
            // 
            this.Expand.HeaderText = "";
            this.Expand.Name = "Expand";
            this.Expand.ReadOnly = true;
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
            this.tableLayoutPanel1.Controls.Add(this.UnLockTree, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 88.86463F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.13537F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(970, 458);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.Controls.Add(this.UnLockBtn, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.CancelBtn, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 410);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(964, 45);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // UnLock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 458);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimizeBox = false;
            this.Name = "UnLock";
            this.Text = "UnLock";
            this.Load += new System.EventHandler(this.UnLock_Load);
            this.Resize += new System.EventHandler(this.UnLock_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.UnLockTree)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button UnLockBtn;
        private System.Windows.Forms.Button CancelBtn;
        private AdvancedDataGridView.TreeGridView UnLockTree;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private System.Windows.Forms.DataGridViewButtonColumn Expand;
        private TreeGridColumn DrawingName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn CADType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Revision;
        private System.Windows.Forms.DataGridViewTextBoxColumn LockStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectId;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.ImageList imageStrip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}