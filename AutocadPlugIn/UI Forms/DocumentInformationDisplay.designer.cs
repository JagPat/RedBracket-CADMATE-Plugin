namespace AutocadPlugIn.UI_Forms
{
    partial class DocumentInformationDisplay
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
            this.drawinginfotreeGrid = new AdvancedDataGridView.TreeGridView();
            this.drawingname = new AdvancedDataGridView.TreeGridColumn();
            this.drawingnumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lockstatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.classification = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentstate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentgeneration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.currentrevision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lateststate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.latestgeneration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.latestrevision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.projectname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.projectid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.drawinginfotreeGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // drawinginfotreeGrid
            // 
            this.drawinginfotreeGrid.AllowUserToAddRows = false;
            this.drawinginfotreeGrid.AllowUserToDeleteRows = false;
            this.drawinginfotreeGrid.AllowUserToOrderColumns = true;
            this.drawinginfotreeGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.drawinginfotreeGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.drawinginfotreeGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.drawinginfotreeGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.drawinginfotreeGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.drawingname,
            this.drawingnumber,
            this.lockstatus,
            this.classification,
            this.currentstate,
            this.currentgeneration,
            this.currentrevision,
            this.lateststate,
            this.latestgeneration,
            this.latestrevision,
            this.projectname,
            this.projectid});
            this.drawinginfotreeGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.drawinginfotreeGrid.ImageList = null;
            this.drawinginfotreeGrid.Location = new System.Drawing.Point(12, 52);
            this.drawinginfotreeGrid.Name = "drawinginfotreeGrid";
            this.drawinginfotreeGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.drawinginfotreeGrid.Size = new System.Drawing.Size(1016, 380);
            this.drawinginfotreeGrid.TabIndex = 8;
            // 
            // drawingname
            // 
            this.drawingname.DefaultNodeImage = null;
            this.drawingname.HeaderText = "Drawing Name";
            this.drawingname.Name = "drawingname";
            this.drawingname.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.drawingname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // drawingnumber
            // 
            this.drawingnumber.HeaderText = "Drawing Number";
            this.drawingnumber.Name = "drawingnumber";
            this.drawingnumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // lockstatus
            // 
            this.lockstatus.HeaderText = "Lock Status";
            this.lockstatus.Name = "lockstatus";
            this.lockstatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // classification
            // 
            this.classification.HeaderText = "Classification";
            this.classification.Name = "classification";
            this.classification.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // currentstate
            // 
            this.currentstate.HeaderText = "CurrentState";
            this.currentstate.Name = "currentstate";
            this.currentstate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // currentgeneration
            // 
            this.currentgeneration.HeaderText = "CurrentGeneration";
            this.currentgeneration.Name = "currentgeneration";
            this.currentgeneration.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // currentrevision
            // 
            this.currentrevision.HeaderText = "CurrentRevision";
            this.currentrevision.Name = "currentrevision";
            this.currentrevision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // lateststate
            // 
            this.lateststate.HeaderText = "LatestState";
            this.lateststate.Name = "lateststate";
            this.lateststate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // latestgeneration
            // 
            this.latestgeneration.HeaderText = "LatestGenration";
            this.latestgeneration.Name = "latestgeneration";
            this.latestgeneration.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // latestrevision
            // 
            this.latestrevision.HeaderText = "LatestRevison";
            this.latestrevision.Name = "latestrevision";
            this.latestrevision.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // projectname
            // 
            this.projectname.HeaderText = "Project Name";
            this.projectname.Name = "projectname";
            this.projectname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // projectid
            // 
            this.projectid.HeaderText = "Project Id";
            this.projectid.Name = "projectid";
            this.projectid.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // DocumentInformationDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1040, 453);
            this.Controls.Add(this.drawinginfotreeGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DocumentInformationDisplay";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Document Information";
            this.Load += new System.EventHandler(this.DocumentInformation_Load);
            ((System.ComponentModel.ISupportInitialize)(this.drawinginfotreeGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AdvancedDataGridView.TreeGridView drawinginfotreeGrid;
        private AdvancedDataGridView.TreeGridColumn drawingname;
        private System.Windows.Forms.DataGridViewTextBoxColumn drawingnumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn lockstatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn classification;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentstate;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentgeneration;
        private System.Windows.Forms.DataGridViewTextBoxColumn currentrevision;
        private System.Windows.Forms.DataGridViewTextBoxColumn lateststate;
        private System.Windows.Forms.DataGridViewTextBoxColumn latestgeneration;
        private System.Windows.Forms.DataGridViewTextBoxColumn latestrevision;
        private System.Windows.Forms.DataGridViewTextBoxColumn projectname;
        private System.Windows.Forms.DataGridViewTextBoxColumn projectid;

    }
}