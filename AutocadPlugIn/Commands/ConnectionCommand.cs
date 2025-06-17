using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using RBAutocadPlugIn.UI_Forms;

namespace RBAutocadPlugIn
{
    public class ConnectionCommand : Command
    {
        private string url;
        private string dbName;
        private string userName;
        private string passwd;
        private string authoringtool;
        private bool isConnected = false;

        public String AuthoringTool
        {
            get
            {
                return this.authoringtool;
            }
            set
            {
                this.authoringtool = value;
            }

        } 
        
        public String Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }
        public String DbName
        {
            get
            {
                return this.dbName;
            }
            set
            {
                this.dbName = value;
            }
        }
        public String UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }
        public String Passwd
        {
            get
            {
                return this.passwd;
            }
            set
            {
                this.passwd = value;
            }
        }
        

    
        public void Execute(object parameter)
        {
            try
            {
                if (!isConnected)
                {
                    // Show login form
                    using (var loginForm = new frmLogin())
                    {
                        if (loginForm.ShowDialog() == DialogResult.OK)
                        {
                            // TODO: Implement actual connection logic
                            // For now, just set the connection state
                            isConnected = true;
                            MessageBox.Show("Successfully connected to the server.", "Connected", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    // Disconnect
                    isConnected = false;
                    MessageBox.Show("Successfully disconnected from the server.", "Disconnected",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                // Update UI state through the parameter if it's a CADRibbon instance
                if (parameter is CADRibbon ribbon)
                {
                    ribbon.SetConnectionStatus(isConnected);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
