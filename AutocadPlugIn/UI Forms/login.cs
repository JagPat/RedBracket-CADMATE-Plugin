using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using AutocadPlugIn;
using CADController.Controllers;
using CADController.Commands;
using ArasConnector;
using Microsoft.Win32;
using RedBracketConnector;

namespace AutocadPlugIn
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void LoginInformation_Load(object sender, EventArgs e)
        {
            try
            {
                ////// Read the keys from the user registry and load it to the  UI.
                ////RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software", true);

                ////registryKey = registryKey.OpenSubKey("RedBracketConnector", true);

                ////    if (registryKey == null)
                ////    return;

                ////registryKey = registryKey.OpenSubKey("LoginSettings", true);

                ////if (registryKey == null)
                ////    return;

                txt_username.Text = Convert.ToString(Helper.GetValueRegistry("LoginSettings", "UserName"));
                txt_url.Text = Convert.ToString(Helper.GetValueRegistry("LoginSettings", "Url"));
                txt_Password.Text = Encoding.UTF8.GetString(Convert.FromBase64String(Convert.ToString(Helper.GetValueRegistry("LoginSettings", "Password"))));

                //AutocadPlugIn.UI_Forms.LoginUserSettings loginUS = AutocadPlugIn.UI_Forms.UserSettings.createUserSetting().getLoginUserSettings();
                //txt_username.Text = loginUS.UserName;
                //txt_url.Text = loginUS.Url;
                //txt_DataBase.Text = loginUS.DbName;
            }
            catch (FileNotFoundException fex)
            {
                System.Windows.Forms.MessageBox.Show(fex.Message);
                return;
            }
        }

        private void LogIn_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if ((this.txt_username.Text.Length > 0) && (this.txt_url.Text.Length > 0) && (this.txt_Password.Text.Length > 0) && (this.txt_DataBase.Text.Length > 0))
                {
                    ConnectToRB();
                }
            }
        }

        /// <summary>
        /// Saves the settings to user registry.
        /// </summary>
        private void SaveSettigsToRegistry()
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software", true);

            registryKey.CreateSubKey("RedBracketConnector");
            registryKey = registryKey.OpenSubKey("RedBracketConnector", true);

            registryKey.CreateSubKey("LoginSettings");
            registryKey = registryKey.OpenSubKey("LoginSettings", true);

            registryKey.SetValue("UserName", txt_username.Text);
            registryKey.SetValue("Password", Convert.ToBase64String(Encoding.UTF8.GetBytes(txt_Password.Text)));
            registryKey.SetValue("Url", txt_url.Text);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectToRB();
           /*try
            {
                this.Cursor = Cursors.WaitCursor;
                ConnectionCommand connectionCmd = new ConnectionCommand();
                connectionCmd.DbName = txt_DataBase.Text;
                connectionCmd.Passwd = txt_Password.Text;
                connectionCmd.Url = txt_url.Text;
                connectionCmd.UserName = txt_username.Text;

                BaseController controller = null;

                controller = new MessageController();
                controller.Execute(connectionCmd);

                if (controller.infoMessage != null)
                {
                    MessageBox.Show(controller.infoMessage, "Error");
                    this.Cursor = Cursors.Default;
                    return;
                }

                ConnectionController controller1 = new ConnectionController();
                controller1.Execute(connectionCmd);


                if (controller1.errorString != null)
                {
                    MessageBox.Show(controller1.errorString);
                    //Globals.Ribbons.ARASRibbon.IsConnected = false;
                    this.Cursor = Cursors.Default;
                    return;
                }

                if (controller1.isConnect)
                {
                    AutocadPlugIn.UI_Forms.CheckoutUserSettings CheckOUT = AutocadPlugIn.UI_Forms.UserSettings.createUserSetting().getCheckoutUserSettings();
                    CheckOUT.checkoutDirPath = ArasConnector.ArasConnector.sg_WorkingDir.ToString();
                    CADRibbon cr = new CADRibbon();
                    //ArasConnector.ArasConnector.Isconnected = true;
                    CADRibbon.connect = controller1.isConnect;
                    cr.browseDEnable = true;
                    cr.createDEnable = true;
                    cr.browseBEnable = true;
                    cr.createBEnable = true;
                    cr.SaveEnable = true;
                    cr.MyRibbon();
                }

                //Globals.Ribbons.ARASRibbon.IsConnected = true;
                this.Cursor = Cursors.Default;
                //MessageBox.Show(controller.infoMessage);

                this.Close();

               }
            catch (Exception excep)
            {
                MessageBox.Show(excep.ToString());
            }*/
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
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
                    MessageBox.Show(controller.infoMessage, "Error");
                    this.Cursor = Cursors.Default;
                    return;
                }

                ConnectionController controller1 = new ConnectionController();
                controller1.Execute(connectionCmd);


                if (controller1.errorString != null)
                {
                    MessageBox.Show(controller1.errorString);
                    //Globals.Ribbons.ARASRibbon.IsConnected = false;
                    this.Cursor = Cursors.Default;
                    return;
                }

                if (controller1.isConnect)
                {
                    //Helper.UserName = txt_username.Text;
                    //Helper.UserID = "1";
                    AutocadPlugIn.UI_Forms.CheckoutUserSettings CheckOUT = AutocadPlugIn.UI_Forms.UserSettings.createUserSetting().getCheckoutUserSettings();
                    //CheckOUT.checkoutDirPath = ArasConnector.ArasConnector.sg_WorkingDir.ToString();
                    CADRibbon cr = new CADRibbon();
                    //ArasConnector.ArasConnector.Isconnected = true;
                    CADRibbon.connect = controller1.isConnect;
                    cr.browseDEnable = true;
                    cr.createDEnable = true;
                    cr.browseBEnable = true;
                    cr.createBEnable = true;
                    cr.SaveEnable = true;
                    cr.MyRibbon();
                }

                //Globals.Ribbons.ARASRibbon.IsConnected = true;
                this.Cursor = Cursors.Default;
                //MessageBox.Show(controller.infoMessage);

                this.Close();

            }
            catch (Exception excep)
            {
                MessageBox.Show(excep.ToString());
            }
        }

        private void txt_url_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbl_url_Click(object sender, EventArgs e)
        {

        }

        private void lbl_Database_Click(object sender, EventArgs e)
        {

        }

        private void lbl_PassWord_Click(object sender, EventArgs e)
        {

        }
    }
}
