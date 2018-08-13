using System;
using System.Text;
using System.Windows.Forms; 
using Microsoft.Win32;
using System.IO;

namespace RBAutocadPlugIn
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {


                txt_username.Text = Convert.ToString(Helper.GetValueRegistry("LoginSettings", "UserName"));
                txt_url.Text = Convert.ToString(Helper.GetValueRegistry("LoginSettings", "Url"));
                if (Helper.IsSavePassword)
                {
                    txt_Password.Text = Encoding.UTF8.GetString(Convert.FromBase64String(Convert.ToString(Helper.GetValueRegistry("LoginSettings", "Password"))));
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            txt_Password.Focus();
        }

        private void LogIn_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    if ((this.txt_username.Text.Length > 0) && (this.txt_url.Text.Length > 0) && (this.txt_Password.Text.Length > 0))
                    {
                        ConnectToRB();
                    }
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        /// <summary>
        /// Saves the settings to user registry.
        /// </summary>
        private void SaveSettigsToRegistry()
        {
            try
            {
                // to check whether there is a checkout path for current user or not and if not create one in current directory
                // if there is no current checkout director at all then prompt for that first.



                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software", true);

                registryKey.CreateSubKey("RedBracketConnector");
                registryKey = registryKey.OpenSubKey("RedBracketConnector", true);

                registryKey.CreateSubKey("LoginSettings");
                registryKey = registryKey.OpenSubKey("LoginSettings", true);

                registryKey.SetValue("UserName", txt_username.Text);
                if (Helper.IsSavePassword)
                {
                    registryKey.SetValue("Password", Convert.ToBase64String(Encoding.UTF8.GetBytes(txt_Password.Text)));
                }

                registryKey.SetValue("Url", txt_url.Text);
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectToRB();
            this.Cursor = Cursors.Default;

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ConnectToRB()
        {
            try
            {
                // Save the settings to user registry before trying to connect.
                SaveSettigsToRegistry();

                this.Cursor = Cursors.WaitCursor;
                ConnectionCommand connectionCmd = new ConnectionCommand();
                //connectionCmd.DbName = txt_DataBase.Text;
                connectionCmd.Passwd = txt_Password.Text;
                connectionCmd.Url = txt_url.Text;
                connectionCmd.UserName = txt_username.Text;

                BaseController controller = null;

                controller = new MessageController();
                controller.Execute(connectionCmd);

                if (controller.infoMessage != null)
                {
                    ShowMessage.ValMess(controller.infoMessage);
                    this.Cursor = Cursors.Default;
                    return;
                }
                Helper.GetProgressBar(4, "Login in Progress", "Connecting to Server.");
                ConnectionController controller1 = new ConnectionController();
                controller1.Execute(connectionCmd);

                if (controller1.errorString != null)
                {
                    ShowMessage.ErrorMess(controller1.errorString);
                    this.Cursor = Cursors.Default;
                    Helper.CloseProgressBar();
                    return;
                }

                saveUserLoginDetails(controller1.loggedUserDetails);

                if (controller1.isConnect)
                {
                    try
                    {
                        Helper.IncrementProgressBar(2, "Preparing Connector.");
                        Helper.LoadMasterData();
                        string CurrentCheckoutPath = Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath"));
                        if (Directory.Exists(CurrentCheckoutPath))
                        {
                            string DriveName = Path.GetPathRoot(CurrentCheckoutPath);
                            string Username = "";
                            if (Helper.UserName.Trim().Length > 0)
                                Username = Helper.FirstName;
                            string WorkDir = Path.Combine(DriveName, Helper.CompanyName, Username + "-" + Helper.UserID);

                            if (!Directory.Exists(WorkDir))
                            {
                                Directory.CreateDirectory(WorkDir);

                            }
                            Helper.SetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath", WorkDir);

                        }
                        else
                        {
                            ShowMessage.InfoMess("Please select working directory from user setting.");
                        }
                        Helper.IncrementProgressBar(1, "Preparing Connector.");
                    }
                    catch (Exception E)
                    {
                        ShowMessage.ErrorMess(E.Message);
                    }


                    RBAutocadPlugIn.UI_Forms.CheckoutUserSettings CheckOUT = RBAutocadPlugIn.UI_Forms.UserSettings.createUserSetting().getCheckoutUserSettings();

                    CADRibbon cr = new CADRibbon();

                    CADRibbon.connect = controller1.isConnect;
                    cr.browseDEnable = true;
                    cr.createDEnable = true;
                    cr.browseBEnable = true;
                    cr.createBEnable = true;
                    cr.SaveEnable = true;
                    cr.RBRibbon();
                }
                Helper.CloseProgressBar(); 
                this.Cursor = Cursors.Default; 
                this.Close(); 
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        private void saveUserLoginDetails(UserDetails loggedUserDetails)
        {
            try
            {
                if (loggedUserDetails == null)
                { 
                    return;
                }

                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software", true);
                registryKey = registryKey.OpenSubKey("RedBracketConnector", true);
                registryKey = registryKey.OpenSubKey("LoginSettings", true);

                registryKey.SetValue("id", loggedUserDetails.id);
                registryKey.SetValue("firstName", loggedUserDetails.firstName);
                registryKey.SetValue("lastName", loggedUserDetails.lastName);
                registryKey.SetValue("companyId", loggedUserDetails.companyId);
                registryKey.SetValue("isCompanyAdmin", loggedUserDetails.isCompanyAdmin == null ? "" : loggedUserDetails.isCompanyAdmin);
                registryKey.SetValue("mobile", loggedUserDetails.mobile == null ? string.Empty : loggedUserDetails.mobile);
                registryKey.SetValue("status", loggedUserDetails.status == null ? string.Empty : loggedUserDetails.status);
                registryKey.SetValue("active", loggedUserDetails.active);
                registryKey.SetValue("deleted", loggedUserDetails.deleted);
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
