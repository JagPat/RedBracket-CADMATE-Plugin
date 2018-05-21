namespace AutocadPlugIn.UI_Forms
{
    partial class GetDrawinDetail
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
            this.lblCadtype = new System.Windows.Forms.Label();
            this.lblDrawingnumber = new System.Windows.Forms.Label();
            this.txtDrawingNumber = new System.Windows.Forms.TextBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblCadtype
            // 
            this.lblCadtype.AutoSize = true;
            this.lblCadtype.Location = new System.Drawing.Point(43, 116);
            this.lblCadtype.Name = "lblCadtype";
            this.lblCadtype.Size = new System.Drawing.Size(56, 13);
            this.lblCadtype.TabIndex = 0;
            this.lblCadtype.Text = "CAD Type";
            // 
            // lblDrawingnumber
            // 
            this.lblDrawingnumber.AutoSize = true;
            this.lblDrawingnumber.Location = new System.Drawing.Point(42, 70);
            this.lblDrawingnumber.Name = "lblDrawingnumber";
            this.lblDrawingnumber.Size = new System.Drawing.Size(86, 13);
            this.lblDrawingnumber.TabIndex = 1;
            this.lblDrawingnumber.Text = "Drawing Number";
            // 
            // txtDrawingNumber
            // 
            this.txtDrawingNumber.Location = new System.Drawing.Point(134, 67);
            this.txtDrawingNumber.Name = "txtDrawingNumber";
            this.txtDrawingNumber.Size = new System.Drawing.Size(100, 20);
            this.txtDrawingNumber.TabIndex = 2;
            // 
            // cmbType
            // 
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(134, 113);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(121, 21);
            this.cmbType.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(134, 169);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // GetDrawinDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.txtDrawingNumber);
            this.Controls.Add(this.lblDrawingnumber);
            this.Controls.Add(this.lblCadtype);
            this.Name = "GetDrawinDetail";
            this.Text = "GetDrawingDetail";
            this.Load += new System.EventHandler(this.GetDrawinDetail_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCadtype;
        private System.Windows.Forms.Label lblDrawingnumber;
        private System.Windows.Forms.TextBox txtDrawingNumber;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Button btnOk;
    }
}