namespace RBAutocadPlugIn.UI_Forms
{
    partial class frmFolderSelection
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSelectedPath = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.lblParentFolder = new System.Windows.Forms.Label();
            this.dgvFolderSelection = new System.Windows.Forms.DataGridView();
            this.FolderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HasFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FolderID1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel3.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFolderSelection)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.Controls.Add(this.pnlTop, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.pnlLeft, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.pnlBottom, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.pnlRight, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 1, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(800, 416);
            this.tableLayoutPanel3.TabIndex = 12;
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
            this.pnlTop.Size = new System.Drawing.Size(800, 40);
            this.pnlTop.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(229, 29);
            this.label1.TabIndex = 12;
            this.label1.Text = "Folder Selection";
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 40);
            this.pnlLeft.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(10, 366);
            this.pnlLeft.TabIndex = 0;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.tableLayoutPanel3.SetColumnSpan(this.pnlBottom, 3);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(0, 406);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(800, 10);
            this.pnlBottom.TabIndex = 0;
            // 
            // pnlRight
            // 
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(50)))));
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(790, 40);
            this.pnlRight.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(10, 366);
            this.pnlRight.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblParentFolder, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgvFolderSelection, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 43);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(774, 360);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 107F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.Controls.Add(this.btnCancel, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblSelectedPath, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSelect, 3, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 313);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(768, 44);
            this.tableLayoutPanel2.TabIndex = 11;
            // 
            // btnCancel
            // 
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(621, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(144, 38);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSelectedPath
            // 
            this.lblSelectedPath.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.lblSelectedPath, 2);
            this.lblSelectedPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSelectedPath.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedPath.ForeColor = System.Drawing.Color.Black;
            this.lblSelectedPath.Location = new System.Drawing.Point(3, 3);
            this.lblSelectedPath.Margin = new System.Windows.Forms.Padding(3);
            this.lblSelectedPath.Name = "lblSelectedPath";
            this.lblSelectedPath.Size = new System.Drawing.Size(451, 38);
            this.lblSelectedPath.TabIndex = 9;
            this.lblSelectedPath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSelectedPath.UseMnemonic = false;
            this.lblSelectedPath.TextChanged += new System.EventHandler(this.lblSelectedPath_TextChanged);
            // 
            // btnSelect
            // 
            this.btnSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelect.Location = new System.Drawing.Point(471, 3);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(144, 38);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lblParentFolder
            // 
            this.lblParentFolder.AutoSize = true;
            this.lblParentFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblParentFolder.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblParentFolder.Location = new System.Drawing.Point(3, 0);
            this.lblParentFolder.Name = "lblParentFolder";
            this.lblParentFolder.Size = new System.Drawing.Size(768, 35);
            this.lblParentFolder.TabIndex = 12;
            this.lblParentFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblParentFolder.UseMnemonic = false;
            // 
            // dgvFolderSelection
            // 
            this.dgvFolderSelection.AllowUserToAddRows = false;
            this.dgvFolderSelection.AllowUserToDeleteRows = false;
            this.dgvFolderSelection.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvFolderSelection.BackgroundColor = System.Drawing.Color.Azure;
            this.dgvFolderSelection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFolderSelection.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FolderName,
            this.HasFolder,
            this.FolderID1});
            this.dgvFolderSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFolderSelection.Location = new System.Drawing.Point(3, 38);
            this.dgvFolderSelection.MultiSelect = false;
            this.dgvFolderSelection.Name = "dgvFolderSelection";
            this.dgvFolderSelection.RowHeadersVisible = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvFolderSelection.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFolderSelection.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvFolderSelection.RowTemplate.Height = 35;
            this.dgvFolderSelection.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFolderSelection.Size = new System.Drawing.Size(768, 269);
            this.dgvFolderSelection.TabIndex = 13;
            this.dgvFolderSelection.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvFolderSelection_CellMouseDoubleClick);
            this.dgvFolderSelection.SelectionChanged += new System.EventHandler(this.dgvFolderSelection_SelectionChanged);
            // 
            // FolderName
            // 
            this.FolderName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FolderName.HeaderText = "Folder Name";
            this.FolderName.Name = "FolderName";
            this.FolderName.ReadOnly = true;
            // 
            // HasFolder
            // 
            this.HasFolder.HeaderText = "No of Folder";
            this.HasFolder.Name = "HasFolder";
            this.HasFolder.ReadOnly = true;
            this.HasFolder.Width = 115;
            // 
            // FolderID1
            // 
            this.FolderID1.HeaderText = "FolderID";
            this.FolderID1.Name = "FolderID1";
            this.FolderID1.ReadOnly = true;
            this.FolderID1.Visible = false;
            // 
            // frmFolderSelection
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(800, 416);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmFolderSelection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Folder Selection";
            this.Load += new System.EventHandler(this.frmFolderSelection_Load);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFolderSelection)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblParentFolder;
        private System.Windows.Forms.DataGridView dgvFolderSelection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn FolderName;
        private System.Windows.Forms.DataGridViewTextBoxColumn HasFolder;
        private System.Windows.Forms.DataGridViewTextBoxColumn FolderID1;
        private System.Windows.Forms.Label lblSelectedPath;
    }
}