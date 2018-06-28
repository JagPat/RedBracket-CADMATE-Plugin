using System; 
using System.Windows.Forms;
using System.Xml;
using System.IO; 
using Microsoft.Win32;
using RedBracketConnector;

namespace AutocadPlugIn.UI_Forms
{
    public partial class UserSettings : Form
    {
        private LoginUserSettings loginUserSettings = null;
        private CheckinUserSettings checkinUserSettings = null;
        private CheckoutUserSettings checkoutUserSettings = null;

        //Get and set for Checkin usersettings

        public LoginUserSettings getLoginUserSettings()
        {
            return loginUserSettings;
        }

        public void setLoginUserSettings(LoginUserSettings settings)
        {
            loginUserSettings = settings;
        }

        //Get and set for Checkin usersettings

        public CheckinUserSettings getCheckinUserSettings()
        {
            return checkinUserSettings;
        }

        public void setCheckinUserSettings(CheckinUserSettings settings)
        {
            checkinUserSettings = settings;
        }

        //Get and set for Checkout usersettings
        public CheckoutUserSettings getCheckoutUserSettings()
        {
            return checkoutUserSettings;
        }

        public void setCheckoutUserSettings(CheckoutUserSettings settings)
        {
            checkoutUserSettings = settings;
        }

        //For Checkin

        public static UserSettings us = null;
        private UserSettings()
        {
            InitializeComponent(); this.FormBorderStyle = FormBorderStyle.None;


        }

        private void readUserSettings()
        {
            ////RegistryKey registryKeyRedBracketConnector = Registry.CurrentUser.OpenSubKey("Software", true);

            ////registryKeyRedBracketConnector.CreateSubKey("RedBracketConnector");
            ////registryKeyRedBracketConnector = registryKeyRedBracketConnector.OpenSubKey("RedBracketConnector", true);

            #region Login Settings
            ////RegistryKey loginSettingsRegistryKey = registryKeyRedBracketConnector.OpenSubKey("LoginSettings", true);
            ////if (loginSettingsRegistryKey == null)
            ////    return;

            loginUserSettings.UserName = Convert.ToString(Helper.GetValueRegistry("LoginSettings", "UserName"));
            loginUserSettings.Url = Convert.ToString(Helper.GetValueRegistry("LoginSettings", "Url"));

            ////loginUserSettings.UserName = loginSettingsRegistryKey.GetValue("UserName").ToString();
            ////loginUserSettings.Url = loginSettingsRegistryKey.GetValue("Url").ToString();
            #endregion Login Settings

            #region Checkin Settings
            ////RegistryKey checkinSettingsRegistryKey = registryKeyRedBracketConnector.OpenSubKey("CheckinSettings", true);
            ////if (checkinSettingsRegistryKey == null)
            ////    return;

            checkinUserSettings.isBackgroundCheckin = Convert.ToString(Helper.GetValueRegistry("CheckinSettings", "BackGroundCheckinEnabled")) == "True";
            checkinUserSettings.isBackgroundCheckin = Convert.ToString(Helper.GetValueRegistry("CheckinSettings", "CheckinExpandAllEnabled")) == "True";

            ////checkinUserSettings.isBackgroundCheckin = checkinSettingsRegistryKey.GetValue("BackGroundCheckinEnabled").ToString() == "True";
            ////checkinUserSettings.isCheckinExpandAll = checkinSettingsRegistryKey.GetValue("CheckinExpandAllEnabled").ToString() == "True";
            #endregion Checkin Settings

            #region Checkout Settings
            ////RegistryKey checkoutSettingsRegistryKey = registryKeyRedBracketConnector.OpenSubKey("CheckoutSettings", true);
            ////if (checkoutSettingsRegistryKey == null)
            ////    return;

            checkoutUserSettings.checkoutDirPath = Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath"));
            checkoutUserSettings.isCheckoutExpandAll = Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutExpandAllEnabled")) == "True";

            ////checkoutUserSettings.checkoutDirPath = checkoutSettingsRegistryKey.GetValue("CheckoutDirectoryPath").ToString();
            ////checkoutUserSettings.isCheckoutExpandAll = checkoutSettingsRegistryKey.GetValue("CheckoutExpandAllEnabled").ToString() == "True";
            #endregion Checkout Settings.
        }

        public static UserSettings createUserSetting()
        {
            if (us == null)
            {
                us = new UserSettings();
            }
            return us;
        }


        // This method is useful for writing the userSettings in the UserSettings File

        private void addToXML()
        {
            //FileName is Declared here
            char[] RemoveFromDirPath = { 'f', 'i', 'l', 'e', ':', '\\', '\\' };
            String Dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).TrimStart(RemoveFromDirPath);
            string filename = Dir + "\\UserSettingFile";

            loginUserSettings.UserName = txtSettingUserNm.Text.Trim();
            loginUserSettings.Url = txtSettingUrl.Text.Trim();
            loginUserSettings.DbName = "";
 

            XmlDocument xmldoc = new XmlDocument();
            XmlNode xmlRoot;
            xmlRoot = xmldoc.CreateElement("Settings");
            xmldoc.AppendChild(xmlRoot);

            //Saving Login user settings to file
            XmlElement LoginsubRoot = xmldoc.CreateElement("Login");
            xmldoc.DocumentElement.AppendChild(LoginsubRoot);

            XmlElement appendedElementUsername = xmldoc.CreateElement("user");
            XmlText xmlTextUserName = xmldoc.CreateTextNode(loginUserSettings.UserName);
            xmlTextUserName.InnerText = txtSettingUserNm.Text.Trim();
            appendedElementUsername.AppendChild(xmlTextUserName);
            LoginsubRoot.AppendChild(appendedElementUsername);
            xmldoc.DocumentElement.AppendChild(LoginsubRoot);


            XmlElement appendedElementDatabasename = xmldoc.CreateElement("db");
            XmlText xmlTextDatabaseName = xmldoc.CreateTextNode(loginUserSettings.DbName);
            appendedElementDatabasename.AppendChild(xmlTextDatabaseName);
            LoginsubRoot.AppendChild(appendedElementDatabasename);
            xmldoc.DocumentElement.AppendChild(LoginsubRoot);


            XmlElement appendedElementArasURL = xmldoc.CreateElement("url");
            XmlText xmlTextArasURLName = xmldoc.CreateTextNode(loginUserSettings.Url);
            appendedElementArasURL.AppendChild(xmlTextArasURLName);
            LoginsubRoot.AppendChild(appendedElementArasURL);
            xmldoc.DocumentElement.AppendChild(LoginsubRoot);

            //Saving the Checkin user settings to file

            XmlElement CheckinsubRoot = xmldoc.CreateElement("Checkin");
            xmldoc.DocumentElement.AppendChild(CheckinsubRoot);


            XmlElement appendedElementBackgroundCheckin = xmldoc.CreateElement("BackgroundCheckInNode");
            XmlText xmlTextBackgroundCheckin = xmldoc.CreateTextNode(checkinUserSettings.isBackgroundCheckin.ToString().Trim());
            appendedElementBackgroundCheckin.AppendChild(xmlTextBackgroundCheckin);
            CheckinsubRoot.AppendChild(appendedElementBackgroundCheckin);
            xmldoc.DocumentElement.AppendChild(CheckinsubRoot);


            XmlElement appendedElementExpandAllCheckin = xmldoc.CreateElement("CheckinExpandAllNode");
            XmlText xmlTextExpandAllCheckin = xmldoc.CreateTextNode(checkinUserSettings.isCheckinExpandAll.ToString().Trim());
            appendedElementExpandAllCheckin.AppendChild(xmlTextExpandAllCheckin);
            CheckinsubRoot.AppendChild(appendedElementExpandAllCheckin);
            xmldoc.DocumentElement.AppendChild(CheckinsubRoot);

            //Saving the Checkout user settings to file

            XmlElement CheckoutsubRoot = xmldoc.CreateElement("Checkout");
            xmldoc.DocumentElement.AppendChild(CheckoutsubRoot);

            XmlElement appendedElementExpandAllCheckout = xmldoc.CreateElement("CheckoutExpandAllNode");
            XmlText xmlTextExpandAllCheckout = xmldoc.CreateTextNode(checkoutUserSettings.isCheckoutExpandAll.ToString().Trim());
            appendedElementExpandAllCheckout.AppendChild(xmlTextExpandAllCheckout);
            CheckoutsubRoot.AppendChild(appendedElementExpandAllCheckout);
            xmldoc.DocumentElement.AppendChild(CheckoutsubRoot);

            //If directory path is not set then set it to path c:\\

            XmlElement appendedElementCheckoutDir = xmldoc.CreateElement("CheckoutDirNode");
            XmlText xmlTextCheckoutDir = null;
            if (checkoutUserSettings.checkoutDirPath != null)
            {
                xmlTextCheckoutDir = xmldoc.CreateTextNode(checkoutUserSettings.checkoutDirPath.ToString().Trim());
            }
            else
            {
                xmlTextCheckoutDir = xmldoc.CreateTextNode("C:\\");
            }

            appendedElementCheckoutDir.AppendChild(xmlTextCheckoutDir);
            CheckoutsubRoot.AppendChild(appendedElementCheckoutDir);
            xmldoc.DocumentElement.AppendChild(CheckoutsubRoot);

            xmldoc.Save(filename);
            MessageBox.Show("User Settings saved sucessfully...");

            return;
        }


        //This method is useful for reading the userSettings from UserSettings File

        public void readFromXML()
        {
            char[] RemoveFromDirPath = { 'f', 'i', 'l', 'e', ':', '\\', '\\' };
            String dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).TrimStart(RemoveFromDirPath);
            string filename = dir + "\\UserSettingFile";


            if (File.Exists(filename))
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(filename);

                XmlNodeList nodeList;

                if (xdoc.HasChildNodes)
                {
                    nodeList = xdoc.SelectSingleNode("Settings").ChildNodes;

                    foreach (XmlNode n in nodeList)
                    {
                        //load Login user settings
                        if (n.Name.ToString() == "Login")
                        {
                            XmlNodeList LoginnodeList = n.ChildNodes;
                            foreach (XmlNode m in LoginnodeList)
                            {
                                if (m.Name.ToString() == "url")
                                {
                                    loginUserSettings.Url = m.InnerText;
                                }
                                if (m.Name.ToString() == "user")
                                {
                                    loginUserSettings.UserName = m.InnerText;
                                }
                                if (m.Name.ToString() == "db")
                                {
                                    loginUserSettings.DbName = m.InnerText;
                                }
                            }
                        }
                        //load checkin user settings
                        if (n.Name.ToString() == "Checkin")
                        {
                            XmlNodeList CheckinnodeList = n.ChildNodes;
                            foreach (XmlNode m in CheckinnodeList)
                            {
                                if (m.Name.ToString() == "BackgroundCheckInNode")
                                {
                                    checkinUserSettings.isBackgroundCheckin = "true".Equals(m.InnerText.ToLower());
                                }
                                if (m.Name.ToString() == "CheckinExpandAllNode")
                                {
                                    checkinUserSettings.isCheckinExpandAll = "true".Equals(m.InnerText.ToLower());
                                }
                            }
                        }

                        //load checkout user settings
                        if (n.Name.ToString() == "Checkout")
                        {
                            XmlNodeList CheckoutnodeList = n.ChildNodes;
                            foreach (XmlNode m in CheckoutnodeList)
                            {
                                if (m.Name.ToString() == "CheckoutExpandAllNode")
                                {
                                    checkoutUserSettings.isCheckoutExpandAll = "true".Equals(m.InnerText.ToLower());
                                }
                                if (m.Name.ToString() == "CheckoutDirNode")
                                {
                                    checkoutUserSettings.checkoutDirPath = m.InnerText;
                                }

                            }
                        }

                    }
                }
            }

        }

        //This method save the userSettings details in the file
        private void userSettingsSavebtn_Click(object sender, EventArgs e)
        {
            ////addToXML();

            RegistryKey redBracketConnectorRegistryKey = Registry.CurrentUser.OpenSubKey("Software", true);

            redBracketConnectorRegistryKey.CreateSubKey("RedBracketConnector");
            redBracketConnectorRegistryKey = redBracketConnectorRegistryKey.OpenSubKey("RedBracketConnector", true);

            #region Login Settings
            redBracketConnectorRegistryKey.CreateSubKey("LoginSettings");
            RegistryKey loginSettingsRegistryKey = redBracketConnectorRegistryKey.OpenSubKey("LoginSettings", true);

            loginSettingsRegistryKey.SetValue("UserName", txtSettingUserNm.Text);
            loginSettingsRegistryKey.SetValue("Url", txtSettingUrl.Text);
            #endregion Login Settings

            #region Checkin Settings
            redBracketConnectorRegistryKey.CreateSubKey("CheckinSettings");
            RegistryKey checkinSettingsRegistryKey = redBracketConnectorRegistryKey.OpenSubKey("CheckinSettings", true);

 
            #endregion Checkin Settings

            #region Checkout Settings
            redBracketConnectorRegistryKey.CreateSubKey("CheckoutSettings");
            RegistryKey checkoutSettingsRegistryKey = redBracketConnectorRegistryKey.OpenSubKey("CheckoutSettings", true);

            string WorkDir = txtWorkingDirectory.Text;
            if (!Directory.Exists(WorkDir))
            {
                Directory.CreateDirectory(WorkDir);
            }
            checkoutSettingsRegistryKey.SetValue("CheckoutDirectoryPath", WorkDir);

            #endregion Checkout Settings.

            ShowMessage.InfoMess("User Settings saved successfully...");
            this.Close();
            return;
        }

        private void userSettingsCancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Load all user settings during loading of the setting dialog

        private void UserSettingLoad(object sender, EventArgs e)
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
                if (d.DriveType == DriveType.Fixed)
                    lbDriveList.Items.Add(d.Name);
            }
            txtWorkingDirectory.Text = checkoutUserSettings.checkoutDirPath;
        }
        private void BkCheckinEnableRadioBtn_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BkCheckinDisableRadioBtn_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CheckInExpEnableRadioBtn_CheckedChanged(object sender, EventArgs e)
        {


        }

        private void CheckInExpDisableRadioBtn_CheckedChanged(object sender, EventArgs e)
        {


        }

        private void CheckOutExpEnableRadioBtn_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void CheckoutdirListBox_DoubleClick(object sender, EventArgs e)
        {
           
            MessageBox.Show(checkoutUserSettings.checkoutDirPath);
        }

        private void CheckOutExpDisableRadioBtn_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void CheckoutdirListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtSettingDbNm_Click(object sender, EventArgs e)
        {
           
        }

        private void panel_Settings_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSelectDrive_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.ShowDialog();
                string DriveName = fbd.SelectedPath;

                if (DriveName.Length > 3)
                {
                    MessageBox.Show("You can only select drive. please select drive only.");
                }
                else
                {
                    string WorkDir = Path.Combine(DriveName, "redbracket", "admin" + "-" + "01");

                    if (Directory.Exists(WorkDir))
                    {
                        // MessageBox.Show("This working directory is already exist.");
                    }
                    else
                    {
                        Directory.CreateDirectory(WorkDir);
                        // MessageBox.Show("working directory created.");
                        //  Process.Start(@WorkDir);

                    }

                    //txtWorkingDirectory.Text = WorkDir;
                    //textBox5.Text = DriveName;
                }
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
                    registryKey = registryKey.OpenSubKey("LoginSettings", true);
                    if(registryKey.GetValue("firstName").ToString().Length>0)
                    {
                        Username =Convert.ToString(registryKey.GetValue("firstName"));
                        Helper.UserID = Convert.ToString(registryKey.GetValue("id"));
                    }
                    
                }

                string WorkDir = Path.Combine(DriveName, Helper.CompanyName, Username + "-" +Helper.UserID);
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