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
            this.imageStrip = new System.Windows.Forms.ImageList(this.components);
            this.CADDescription = new System.Windows.Forms.TextBox();
            this.Comments = new System.Windows.Forms.Label();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.drawing = new AdvancedDataGridView.TreeGridColumn();
            this.DrawingNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cadtype = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.ProjectId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.RealtyName = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.RealtyId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.targetrevision = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Version = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.savetreeGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // submit
            // 
            this.submit.Location = new System.Drawing.Point(417, 394);
            this.submit.Name = "submit";
            this.submit.Size = new System.Drawing.Size(75, 23);
            this.submit.TabIndex = 4;
            this.submit.Text = "Submit";
            this.submit.UseVisualStyleBackColor = true;
            this.submit.Click += new System.EventHandler(this.submit_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(530, 394);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
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
            this.savetreeGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.ProjectId,
            this.RealtyName,
            this.RealtyId,
            this.targetrevision,
            this.Version});
            this.savetreeGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.savetreeGrid.ImageList = null;
            this.savetreeGrid.Location = new System.Drawing.Point(11, 23);
            this.savetreeGrid.Name = "savetreeGrid";
            this.savetreeGrid.RowHeadersVisible = false;
            this.savetreeGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.savetreeGrid.Size = new System.Drawing.Size(1115, 348);
            this.savetreeGrid.TabIndex = 7;
            this.savetreeGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.savetreeGrid_CellContentClick);
            this.savetreeGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.savetreeGrid_CellBeginEdit);
            // 
            // imageStrip
            // 
            this.imageStrip.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageStrip.ImageSize = new System.Drawing.Size(17, 17);
            this.imageStrip.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CADDescription
            // 
            this.CADDescription.Location = new System.Drawing.Point(88, 378);
            this.CADDescription.Multiline = true;
            this.CADDescription.Name = "CADDescription";
            this.CADDescription.Size = new System.Drawing.Size(304, 47);
            this.CADDescription.TabIndex = 8;
            // 
            // Comments
            // 
            this.Comments.AutoSize = true;
            this.Comments.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Comments.ForeColor = System.Drawing.Color.Red;
            this.Comments.Location = new System.Drawing.Point(12, 381);
            this.Comments.Name = "Comments";
            this.Comments.Size = new System.Drawing.Size(64, 13);
            this.Comments.TabIndex = 9;
            this.Comments.Text = "Comments";
            // 
            // Check
            // 
            this.Check.FillWeight = 51.53443F;
            this.Check.HeaderText = "";
            this.Check.MinimumWidth = 25;
            this.Check.Name = "Check";
            // 
            // drawing
            // 
            this.drawing.DefaultNodeImage = null;
            this.drawing.HeaderText = "File Name";
            this.drawing.Name = "drawing";
            this.drawing.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DrawingNumber
            // 
            this.DrawingNumber.HeaderText = "File No";
            this.DrawingNumber.Name = "DrawingNumber";
            this.DrawingNumber.ReadOnly = true;
            this.DrawingNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cadtype
            // 
            this.cadtype.HeaderText = "File Type";
            this.cadtype.Name = "cadtype";
            this.cadtype.ReadOnly = true;
            this.cadtype.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // revision
            // 
            this.revision.FillWeight = 30F;
            this.revision.HeaderText = "Rev";
            this.revision.Name = "revision";
            this.revision.ReadOnly = true;
            this.revision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // State
            // 
            this.State.FillWeight = 70F;
            this.State.HeaderText = "FIle Status";
            this.State.Name = "State";
            this.State.ReadOnly = true;
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
            this.ProjectName.HeaderText = "Project Name";
            this.ProjectName.Name = "ProjectName";
            // 
            // ProjectId
            // 
            this.ProjectId.HeaderText = "ProjectId";
            this.ProjectId.Name = "ProjectId";
            this.ProjectId.Visible = false;
            // 
            // RealtyName
            // 
            this.RealtyName.HeaderText = "RealtyName";
            this.RealtyName.Name = "RealtyName";
            this.RealtyName.Visible = false;
            // 
            // RealtyId
            // 
            this.RealtyId.HeaderText = "RealtyId";
            this.RealtyId.Name = "RealtyId";
            this.RealtyId.Visible = false;
            // 
            // targetrevision
            // 
            this.targetrevision.HeaderText = "Taget Rev";
            this.targetrevision.Name = "targetrevision";
            this.targetrevision.Visible = false;
            // 
            // Version
            // 
            this.Version.FillWeight = 30F;
            this.Version.HeaderText = "Create New Ver";
            this.Version.MinimumWidth = 25;
            this.Version.Name = "Version";
            // 
            // Save_Active_Drawings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1142, 428);
            this.Controls.Add(this.Comments);
            this.Controls.Add(this.CADDescription);
            this.Controls.Add(this.savetreeGrid);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.submit);
            this.Name = "Save_Active_Drawings";
            this.Text = "Save Active Drawings";
            this.Load += new System.EventHandler(this.Save_Active_Drawings_Load);
            this.Resize += new System.EventHandler(this.Save_Active_Drawings_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.savetreeGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.Button submit;
        private System.Windows.Forms.Button cancel;
        private AdvancedDataGridView.TreeGridView savetreeGrid;
        private System.Windows.Forms.ImageList imageStrip;
        private System.Windows.Forms.TextBox CADDescription;
        private System.Windows.Forms.Label Comments;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private AdvancedDataGridView.TreeGridColumn drawing;
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn cadtype;
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
        private System.Windows.Forms.DataGridViewComboBoxColumn ProjectId;
        private System.Windows.Forms.DataGridViewComboBoxColumn RealtyName;
        private System.Windows.Forms.DataGridViewComboBoxColumn RealtyId;
        private System.Windows.Forms.DataGridViewComboBoxColumn targetrevision;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Version;
    }
}