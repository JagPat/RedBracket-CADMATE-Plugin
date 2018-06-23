namespace AutocadPlugIn.UI_Forms
{
    partial class UserSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserSettings));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.CheckoutDirLabel = new System.Windows.Forms.Label();
            this.txtSettingDbNm = new System.Windows.Forms.ComboBox();
            this.LoginPictureBox = new System.Windows.Forms.PictureBox();
            this.txtSettingUrl = new System.Windows.Forms.TextBox();
            this.UserNameLabel = new System.Windows.Forms.Label();
            this.txtSettingUserNm = new System.Windows.Forms.TextBox();
            this.DatabaseLabel = new System.Windows.Forms.Label();
            this.ArasUrlLabel = new System.Windows.Forms.Label();
            this.userSettingsCancelbutton = new System.Windows.Forms.Button();
            this.userSettingsSavebtn = new System.Windows.Forms.Button();
            this.panel_Settings = new System.Windows.Forms.Panel();
            this.groupBox_Checkout = new System.Windows.Forms.GroupBox();
            this.lbDriveList = new System.Windows.Forms.ListBox();
            this.txtWorkingDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox_Login = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoginPictureBox)).BeginInit();
            this.panel_Settings.SuspendLayout();
            this.groupBox_Checkout.SuspendLayout();
            this.groupBox_Login.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(70, 80);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // CheckoutDirLabel
            // 
            this.CheckoutDirLabel.AutoSize = true;
            this.CheckoutDirLabel.Location = new System.Drawing.Point(98, 102);
            this.CheckoutDirLabel.Name = "CheckoutDirLabel";
            this.CheckoutDirLabel.Size = new System.Drawing.Size(116, 13);
            this.CheckoutDirLabel.TabIndex = 1;
            this.CheckoutDirLabel.Text = "Checkout Directory";
            // 
            // txtSettingDbNm
            // 
            this.txtSettingDbNm.FormattingEnabled = true;
            this.txtSettingDbNm.Location = new System.Drawing.Point(198, 77);
            this.txtSettingDbNm.Name = "txtSettingDbNm";
            this.txtSettingDbNm.Size = new System.Drawing.Size(179, 21);
            this.txtSettingDbNm.TabIndex = 10;
            this.txtSettingDbNm.Visible = false;
            this.txtSettingDbNm.Click += new System.EventHandler(this.txtSettingDbNm_Click);
            // 
            // LoginPictureBox
            // 
            this.LoginPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LoginPictureBox.ErrorImage = null;
            this.LoginPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("LoginPictureBox.Image")));
            this.LoginPictureBox.Location = new System.Drawing.Point(10, 16);
            this.LoginPictureBox.Name = "LoginPictureBox";
            this.LoginPictureBox.Size = new System.Drawing.Size(68, 78);
            this.LoginPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LoginPictureBox.TabIndex = 9;
            this.LoginPictureBox.TabStop = false;
            // 
            // txtSettingUrl
            // 
            this.txtSettingUrl.Location = new System.Drawing.Point(198, 48);
            this.txtSettingUrl.Name = "txtSettingUrl";
            this.txtSettingUrl.Size = new System.Drawing.Size(179, 20);
            this.txtSettingUrl.TabIndex = 7;
            // 
            // UserNameLabel
            // 
            this.UserNameLabel.AutoSize = true;
            this.UserNameLabel.Location = new System.Drawing.Point(95, 16);
            this.UserNameLabel.Name = "UserNameLabel";
            this.UserNameLabel.Size = new System.Drawing.Size(65, 13);
            this.UserNameLabel.TabIndex = 6;
            this.UserNameLabel.Text = "UserName";
            // 
            // txtSettingUserNm
            // 
            this.txtSettingUserNm.Location = new System.Drawing.Point(198, 15);
            this.txtSettingUserNm.Name = "txtSettingUserNm";
            this.txtSettingUserNm.Size = new System.Drawing.Size(179, 20);
            this.txtSettingUserNm.TabIndex = 4;
            // 
            // DatabaseLabel
            // 
            this.DatabaseLabel.AutoSize = true;
            this.DatabaseLabel.Location = new System.Drawing.Point(95, 80);
            this.DatabaseLabel.Name = "DatabaseLabel";
            this.DatabaseLabel.Size = new System.Drawing.Size(61, 13);
            this.DatabaseLabel.TabIndex = 1;
            this.DatabaseLabel.Text = "Database";
            this.DatabaseLabel.Visible = false;
            // 
            // ArasUrlLabel
            // 
            this.ArasUrlLabel.AutoSize = true;
            this.ArasUrlLabel.Location = new System.Drawing.Point(95, 50);
            this.ArasUrlLabel.Name = "ArasUrlLabel";
            this.ArasUrlLabel.Size = new System.Drawing.Size(103, 13);
            this.ArasUrlLabel.TabIndex = 0;
            this.ArasUrlLabel.Text = "RedBracket URL";
            // 
            // userSettingsCancelbutton
            // 
            this.userSettingsCancelbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userSettingsCancelbutton.Location = new System.Drawing.Point(215, 273);
            this.userSettingsCancelbutton.Name = "userSettingsCancelbutton";
            this.userSettingsCancelbutton.Size = new System.Drawing.Size(70, 30);
            this.userSettingsCancelbutton.TabIndex = 12;
            this.userSettingsCancelbutton.Text = "Cancel";
            this.userSettingsCancelbutton.UseVisualStyleBackColor = true;
            this.userSettingsCancelbutton.Click += new System.EventHandler(this.userSettingsCancelBtn_Click);
            // 
            // userSettingsSavebtn
            // 
            this.userSettingsSavebtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userSettingsSavebtn.Location = new System.Drawing.Point(112, 273);
            this.userSettingsSavebtn.Name = "userSettingsSavebtn";
            this.userSettingsSavebtn.Size = new System.Drawing.Size(66, 30);
            this.userSettingsSavebtn.TabIndex = 0;
            this.userSettingsSavebtn.Text = "Save ";
            this.userSettingsSavebtn.UseVisualStyleBackColor = true;
            this.userSettingsSavebtn.Click += new System.EventHandler(this.userSettingsSavebtn_Click);
            // 
            // panel_Settings
            // 
            this.panel_Settings.BackColor = System.Drawing.Color.White;
            this.panel_Settings.Controls.Add(this.groupBox_Checkout);
            this.panel_Settings.Controls.Add(this.userSettingsCancelbutton);
            this.panel_Settings.Controls.Add(this.userSettingsSavebtn);
            this.panel_Settings.Controls.Add(this.groupBox_Login);
            this.panel_Settings.Location = new System.Drawing.Point(9, 9);
            this.panel_Settings.Margin = new System.Windows.Forms.Padding(2);
            this.panel_Settings.Name = "panel_Settings";
            this.panel_Settings.Size = new System.Drawing.Size(393, 316);
            this.panel_Settings.TabIndex = 13;
            this.panel_Settings.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_Settings_Paint);
            // 
            // groupBox_Checkout
            // 
            this.groupBox_Checkout.Controls.Add(this.lbDriveList);
            this.groupBox_Checkout.Controls.Add(this.txtWorkingDirectory);
            this.groupBox_Checkout.Controls.Add(this.pictureBox1);
            this.groupBox_Checkout.Controls.Add(this.label1);
            this.groupBox_Checkout.Controls.Add(this.CheckoutDirLabel);
            this.groupBox_Checkout.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Checkout.Location = new System.Drawing.Point(2, 112);
            this.groupBox_Checkout.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_Checkout.Name = "groupBox_Checkout";
            this.groupBox_Checkout.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_Checkout.Size = new System.Drawing.Size(386, 156);
            this.groupBox_Checkout.TabIndex = 2;
            this.groupBox_Checkout.TabStop = false;
            this.groupBox_Checkout.Text = "Check-Out";
            // 
            // lbDriveList
            // 
            this.lbDriveList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDriveList.FormattingEnabled = true;
            this.lbDriveList.ItemHeight = 17;
            this.lbDriveList.Location = new System.Drawing.Point(98, 32);
            this.lbDriveList.Margin = new System.Windows.Forms.Padding(2);
            this.lbDriveList.Name = "lbDriveList";
            this.lbDriveList.Size = new System.Drawing.Size(279, 55);
            this.lbDriveList.TabIndex = 18;
            this.lbDriveList.SelectedIndexChanged += new System.EventHandler(this.lbDriveList_SelectedIndexChanged);
            // 
            // txtWorkingDirectory
            // 
            this.txtWorkingDirectory.Location = new System.Drawing.Point(98, 119);
            this.txtWorkingDirectory.Multiline = true;
            this.txtWorkingDirectory.Name = "txtWorkingDirectory";
            this.txtWorkingDirectory.ReadOnly = true;
            this.txtWorkingDirectory.Size = new System.Drawing.Size(281, 24);
            this.txtWorkingDirectory.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Drive";
            // 
            // groupBox_Login
            // 
            this.groupBox_Login.Controls.Add(this.txtSettingDbNm);
            this.groupBox_Login.Controls.Add(this.LoginPictureBox);
            this.groupBox_Login.Controls.Add(this.txtSettingUrl);
            this.groupBox_Login.Controls.Add(this.UserNameLabel);
            this.groupBox_Login.Controls.Add(this.ArasUrlLabel);
            this.groupBox_Login.Controls.Add(this.txtSettingUserNm);
            this.groupBox_Login.Controls.Add(this.DatabaseLabel);
            this.groupBox_Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Login.Location = new System.Drawing.Point(2, 1);
            this.groupBox_Login.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox_Login.Name = "groupBox_Login";
            this.groupBox_Login.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox_Login.Size = new System.Drawing.Size(386, 107);
            this.groupBox_Login.TabIndex = 0;
            this.groupBox_Login.TabStop = false;
            this.groupBox_Login.Text = "Login";
            // 
            // UserSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(408, 332);
            this.Controls.Add(this.panel_Settings);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.HelpButton = true;
            this.Name = "UserSettings";
            this.Text = "UserSettings";
            this.TransparencyKey = System.Drawing.Color.WhiteSmoke;
            this.Load += new System.EventHandler(this.UserSettingLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoginPictureBox)).EndInit();
            this.panel_Settings.ResumeLayout(false);
            this.groupBox_Checkout.ResumeLayout(false);
            this.groupBox_Checkout.PerformLayout();
            this.groupBox_Login.ResumeLayout(false);
            this.groupBox_Login.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtSettingUrl;
        private System.Windows.Forms.Label DatabaseLabel;
        private System.Windows.Forms.Label ArasUrlLabel;
        private System.Windows.Forms.Label UserNameLabel;
        private System.Windows.Forms.TextBox txtSettingUserNm;
        private System.Windows.Forms.PictureBox LoginPictureBox;
        private System.Windows.Forms.Button userSettingsSavebtn;
        
        private System.Windows.Forms.Label CheckoutDirLabel;
        private System.Windows.Forms.Button userSettingsCancelbutton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ComboBox txtSettingDbNm;
        private System.Windows.Forms.Panel panel_Settings;
        private System.Windows.Forms.GroupBox groupBox_Login;
        private System.Windows.Forms.GroupBox groupBox_Checkout;
        private System.Windows.Forms.TextBox txtWorkingDirectory;
        private System.Windows.Forms.ListBox lbDriveList;
        private System.Windows.Forms.Label label1;
    }
}