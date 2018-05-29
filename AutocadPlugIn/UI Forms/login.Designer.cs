namespace AutocadPlugIn
{
    partial class login
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
            this.txt_username = new System.Windows.Forms.TextBox();
            this.txt_Password = new System.Windows.Forms.TextBox();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.txt_DataBase = new System.Windows.Forms.TextBox();
            this.txt_url = new System.Windows.Forms.TextBox();
            this.lbl_UserName = new System.Windows.Forms.Label();
            this.lbl_PassWord = new System.Windows.Forms.Label();
            this.lbl_Database = new System.Windows.Forms.Label();
            this.lbl_url = new System.Windows.Forms.Label();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_username
            // 
            this.txt_username.Location = new System.Drawing.Point(210, 34);
            this.txt_username.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(201, 26);
            this.txt_username.TabIndex = 0;
            // 
            // txt_Password
            // 
            this.txt_Password.Location = new System.Drawing.Point(210, 100);
            this.txt_Password.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_Password.Name = "txt_Password";
            this.txt_Password.PasswordChar = '*';
            this.txt_Password.Size = new System.Drawing.Size(201, 26);
            this.txt_Password.TabIndex = 1;
            this.txt_Password.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LogIn_KeyPress);
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(47, 252);
            this.btn_Connect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(112, 35);
            this.btn_Connect.TabIndex = 4;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // txt_DataBase
            // 
            this.txt_DataBase.Location = new System.Drawing.Point(210, 209);
            this.txt_DataBase.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_DataBase.Name = "txt_DataBase";
            this.txt_DataBase.Size = new System.Drawing.Size(201, 26);
            this.txt_DataBase.TabIndex = 2;
            this.txt_DataBase.Visible = false;
            // 
            // txt_url
            // 
            this.txt_url.Location = new System.Drawing.Point(210, 164);
            this.txt_url.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txt_url.Name = "txt_url";
            this.txt_url.ReadOnly = true;
            this.txt_url.Size = new System.Drawing.Size(201, 26);
            this.txt_url.TabIndex = 3;
            this.txt_url.TextChanged += new System.EventHandler(this.txt_url_TextChanged);
            // 
            // lbl_UserName
            // 
            this.lbl_UserName.AutoSize = true;
            this.lbl_UserName.Location = new System.Drawing.Point(44, 42);
            this.lbl_UserName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_UserName.Name = "lbl_UserName";
            this.lbl_UserName.Size = new System.Drawing.Size(89, 20);
            this.lbl_UserName.TabIndex = 5;
            this.lbl_UserName.Text = "User Name";
            // 
            // lbl_PassWord
            // 
            this.lbl_PassWord.AutoSize = true;
            this.lbl_PassWord.Location = new System.Drawing.Point(44, 106);
            this.lbl_PassWord.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_PassWord.Name = "lbl_PassWord";
            this.lbl_PassWord.Size = new System.Drawing.Size(78, 20);
            this.lbl_PassWord.TabIndex = 6;
            this.lbl_PassWord.Text = "Password";
            this.lbl_PassWord.Click += new System.EventHandler(this.lbl_PassWord_Click);
            // 
            // lbl_Database
            // 
            this.lbl_Database.AutoSize = true;
            this.lbl_Database.Location = new System.Drawing.Point(44, 209);
            this.lbl_Database.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_Database.Name = "lbl_Database";
            this.lbl_Database.Size = new System.Drawing.Size(79, 20);
            this.lbl_Database.TabIndex = 7;
            this.lbl_Database.Text = "Database";
            this.lbl_Database.Visible = false;
            this.lbl_Database.Click += new System.EventHandler(this.lbl_Database_Click);
            // 
            // lbl_url
            // 
            this.lbl_url.AutoSize = true;
            this.lbl_url.Location = new System.Drawing.Point(44, 170);
            this.lbl_url.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_url.Name = "lbl_url";
            this.lbl_url.Size = new System.Drawing.Size(131, 20);
            this.lbl_url.TabIndex = 8;
            this.lbl_url.Text = "RedBracket URL";
            this.lbl_url.Click += new System.EventHandler(this.lbl_url_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(210, 252);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(112, 35);
            this.btn_Cancel.TabIndex = 5;
            this.btn_Cancel.Text = "Cancel";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 345);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.lbl_url);
            this.Controls.Add(this.lbl_Database);
            this.Controls.Add(this.lbl_PassWord);
            this.Controls.Add(this.lbl_UserName);
            this.Controls.Add(this.txt_url);
            this.Controls.Add(this.txt_DataBase);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.txt_Password);
            this.Controls.Add(this.txt_username);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "login";
            this.Text = "login";
            this.Load += new System.EventHandler(this.LoginInformation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_username;
        private System.Windows.Forms.TextBox txt_Password;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.TextBox txt_DataBase;
        private System.Windows.Forms.TextBox txt_url;
        private System.Windows.Forms.Label lbl_UserName;
        private System.Windows.Forms.Label lbl_PassWord;
        private System.Windows.Forms.Label lbl_Database;
        private System.Windows.Forms.Label lbl_url;
        private System.Windows.Forms.Button btn_Cancel;
    }
}