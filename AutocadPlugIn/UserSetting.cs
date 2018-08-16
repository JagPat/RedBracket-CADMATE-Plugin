using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using Microsoft.Win32;

namespace RBAutocadPlugIn.UI_Forms
{
    public partial class UserSettings : Form
    {
        private LoginUserSettings loginUserSettings = null;
        private CheckinUserSettings checkinUserSettings = null;
        private CheckoutUserSettings checkoutUserSettings = null;

        public static UserSettings us = null;
        private UserSettings()
        {
            InitializeComponent(); this.FormBorderStyle = FormBorderStyle.None;
        }

        public CheckoutUserSettings getCheckoutUserSettings()
        {
            return checkoutUserSettings;
        }

        private void readUserSettings()
        {
            try
            {
                loginUserSettings.UserName = Convert.ToString(Helper.GetValueRegistry("LoginSettings", "UserName"));
                loginUserSettings.Url = Convert.ToString(Helper.GetValueRegistry("LoginSettings", "Url"));
                checkoutUserSettings.checkoutDirPath = Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath"));
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        public static UserSettings createUserSetting()
        {
            if (us == null)
            {
                us = new UserSettings();
            }
            return us;
        }


        private void userSettingsSavebtn_Click(object sender, EventArgs e)
        {

            try
            {

                RegistryKey redBracketConnectorRegistryKey = Registry.CurrentUser.OpenSubKey("Software", true);

                redBracketConnectorRegistryKey.CreateSubKey("RedBracketConnector");
                redBracketConnectorRegistryKey = redBracketConnectorRegistryKey.OpenSubKey("RedBracketConnector", true);

                #region Login Settings
                redBracketConnectorRegistryKey.CreateSubKey("LoginSettings");
                RegistryKey loginSettingsRegistryKey = redBracketConnectorRegistryKey.OpenSubKey("LoginSettings", true);

                loginSettingsRegistryKey.SetValue("UserName", txtSettingUserNm.Text);
                loginSettingsRegistryKey.SetValue("Url", txtSettingUrl.Text);
                #endregion Login Settings



                #region Checkout Settings
                redBracketConnectorRegistryKey.CreateSubKey("CheckoutSettings");
                RegistryKey checkoutSettingsRegistryKey = redBracketConnectorRegistryKey.OpenSubKey("CheckoutSettings", true);



                #endregion Checkout Settings.
                string WorkDir = txtWorkingDirectory.Text;
                if (!Directory.Exists(WorkDir))
                {
                    Directory.CreateDirectory(WorkDir);
                }
                checkoutSettingsRegistryKey.SetValue("CheckoutDirectoryPath", WorkDir);
                ShowMessage.InfoMess("User Settings saved successfully...");
                this.Close();
                return;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        private void userSettingsCancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Load all user settings during loading of the setting dialog

        private void UserSettingLoad(object sender, EventArgs e)
        {
            try
            {
                checkinUserSettings = new CheckinUserSettings();
                checkoutUserSettings = new CheckoutUserSettings();
                loginUserSettings = new LoginUserSettings();
                readUserSettings();

                txtSettingUserNm.Text = loginUserSettings.UserName;
                txtSettingUrl.Text = loginUserSettings.Url;


                lbDriveList.Items.Clear();
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                foreach (DriveInfo d in allDrives)
                {
                    if (d.DriveType == DriveType.Fixed || d.DriveType == DriveType.Network)
                        lbDriveList.Items.Add(d.Name);
                }
                txtWorkingDirectory.Text = checkoutUserSettings.checkoutDirPath;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }




        private void lbDriveList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string DriveName = lbDriveList.SelectedItem.ToString();
                string Username = txtSettingUserNm.Text;
                if (Helper.UserName.Trim().Length > 0)
                {
                    Username = Helper.FirstName;
                }
                else
                {
                    RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software", true);
                    registryKey = registryKey.OpenSubKey("RedBracketConnector", true);
                    if (registryKey != null)
                    {
                        registryKey = registryKey.OpenSubKey("LoginSettings", true);
                        if (registryKey != null)
                        {
                            if (registryKey.GetValue("firstName").ToString().Length > 0)
                            {
                                Username = Convert.ToString(registryKey.GetValue("firstName"));
                                Helper.UserID = Convert.ToString(registryKey.GetValue("id"));
                            }
                        }
                    }


                }

                string WorkDir = Path.Combine(DriveName, Helper.CompanyName, Username + "-" + Helper.UserID);
                txtWorkingDirectory.Text = WorkDir;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }
    }
    public class LoginUserSettings
    {
        private String url;
        private String dbName;
        private String userName;
        private String passwd;

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
    }

    public class CheckinUserSettings
    {
        private Boolean Bkgroundcheckin;
        private Boolean Checkinexpandall;

        public Boolean isBackgroundCheckin
        {
            get
            {
                return Bkgroundcheckin;
            }
            set
            {
                Bkgroundcheckin = value;
            }
        }

        public Boolean isCheckinExpandAll
        {
            get
            {
                return Checkinexpandall;
            }
            set
            {
                Checkinexpandall = value;
            }
        }

    }
    public class CheckoutUserSettings
    {

        private Boolean Checkoutexpandall;
        private string Checkoutdirpath;


        public Boolean isCheckoutExpandAll
        {
            get
            {
                return Checkoutexpandall;
            }
            set
            {
                Checkoutexpandall = value;
            }
        }

        public string checkoutDirPath
        {
            get
            {
                return Checkoutdirpath;
            }
            set
            {
                Checkoutdirpath = value;
            }
        }
    }

}