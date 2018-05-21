using AdvancedDataGridView;
namespace AutocadPlugIn.UI_Forms
{
    partial class frmLock1
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
            this.LockBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.LockTree = new AdvancedDataGridView.TreeGridView();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DrawingName = new AdvancedDataGridView.TreeGridColumn();
            this.DrawingNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CADType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Revision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DrawingID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LockStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LockBy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProjectId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Expand = new System.Windows.Forms.DataGridViewButtonColumn();            
            this.imageStrip = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.LockTree)).BeginInit();
            this.SuspendLayout();
            // 
            // LockBtn
            // 
            this.LockBtn.Location = new System.Drawing.Point(538, 405);
            this.LockBtn.Name = "LockBtn";
            this.LockBtn.Size = new System.Drawing.Size(75, 23);
            this.LockBtn.TabIndex = 1;
            this.LockBtn.Text = "Lock";
            this.LockBtn.UseVisualStyleBackColor = true;
            this.LockBtn.Click += new System.EventHandler(this.LockBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(639, 405);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // LockTree
            // 
            this.LockTree.AllowUserToAddRows = false;
            this.LockTree.AllowUserToDeleteRows = false;
            this.LockTree.AllowUserToOrderColumns = true;
            this.LockTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.LockTree.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.LockTree.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.LockTree.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.LockTree.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            //this.Expand,
            this.DrawingName,
            this.DrawingNumber,
            this.CADType,
            this.Revision,
            this.DrawingID,
            this.LockStatus,
            this.LockBy,
            this.ProjectName,this.ProjectId,this.State});
            this.LockTree.DefaultCellStyle = dataGridViewCellStyle1;
            this.LockTree.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.LockTree.ImageList = null;
            this.LockTree.Location = new System.Drawing.Point(25, 30);
            this.LockTree.Name = "LockTree";
            this.LockTree.RowHeadersVisible = false;
            this.LockTree.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LockTree.Size = new System.Drawing.Size(719, 358);
            this.LockTree.TabIndex = 3;
            this.LockTree.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.LockTree_CellContentClick);
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
            // Expand
            // 
            this.Expand.HeaderText = "";
            this.Expand.Name = "Expand";
            this.Expand.ReadOnly = true;
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
            // LockBy
            // 
            this.LockBy.FillWeight = 50F;
            this.LockBy.HeaderText = "LockStatus";
            this.LockBy.Name = "LockStatus1";
            this.LockBy.ReadOnly = true;
            this.LockBy.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LockBy.Visible = false;
            // 
            this.ProjectName.HeaderText = "ProjectName";
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.ReadOnly = true;
            this.ProjectName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            this.ProjectId.HeaderText = "ProjectId";
            this.ProjectId.Name = "ProjectId";
            this.ProjectId.ReadOnly = true;
            this.ProjectId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            this.State.HeaderText = "State";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            this.State.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // imageStrip
            // 
            this.imageStrip.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageStrip.ImageSize = new System.Drawing.Size(17, 17);
            this.imageStrip.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Lock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 458);
            this.Controls.Add(this.LockTree);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.LockBtn);
            this.Name = "Lock";
            this.Text = "Lock";
            this.Load += new System.EventHandler(this.Lock_Load);
            this.Resize += new System.EventHandler(this.Lock_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.LockTree)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button LockBtn;
        private System.Windows.Forms.Button CancelBtn;
        private AdvancedDataGridView.TreeGridView LockTree;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private System.Windows.Forms.DataGridViewButtonColumn Expand;
        private TreeGridColumn DrawingName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn CADType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Revision;
        private System.Windows.Forms.DataGridViewTextBoxColumn LockStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn LockBy;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectId;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.ImageList imageStrip;
    }
}