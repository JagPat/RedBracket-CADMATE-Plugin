using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Autodesk.Windows;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using System.ComponentModel;
using System.Windows.Forms;
using CADController.Commands;
using CADController.Controllers;
using AutocadPlugIn.UI_Forms;
using CADController;

namespace AutocadPlugIn
{
    public class CADRibbon
    {
        public static bool ribbonStatus = false;
        public static bool connect = false;
        public bool browseDEnable = false;
        public bool createDEnable = false;
        public bool browseBEnable = false;
        public bool createBEnable = false;
        public bool LockEnable = false;
        public bool UnlockEnable = false;
        public bool SaveEnable = false;
        public bool DrawingInfoEnable = false;

        //Button structure in ribbon

        public Autodesk.Windows.RibbonControl ribbonControl = Autodesk.Windows.ComponentManager.Ribbon;
        public RibbonTab Tab = new RibbonTab();


        public RibbonPanel Panel1 = new RibbonPanel();
        public Autodesk.Windows.RibbonPanelSource panel1Panel = new RibbonPanelSource();
        public RibbonButton Btn_Connection = new RibbonButton();


        public RibbonPanel panel2 = new RibbonPanel();
        public RibbonPanelSource panel2Panel = new RibbonPanelSource();

        public RibbonButton Btn_BrowseDrawing = new RibbonButton();
        public RibbonButton Btn_CreateDrawing = new RibbonButton();

        public RibbonPanel Panel3 = new RibbonPanel();
        public RibbonPanelSource pan3Panel = new RibbonPanelSource();

        public RibbonButton Btn_BrowseBlock = new RibbonButton();
        public RibbonButton Btn_AddBlock = new RibbonButton();

        public RibbonPanelSource panel4Panel = new RibbonPanelSource();
        public RibbonPanel panel4 = new RibbonPanel();

        public RibbonButton Btn_Lock = new RibbonButton();
        public RibbonButton Btn_Unlock = new RibbonButton();
        public RibbonButton Btn_Save = new RibbonButton();

        public RibbonPanel panel5 = new RibbonPanel();
        public RibbonPanelSource panel5Panel = new RibbonPanelSource();

        public RibbonButton Btn_Help = new RibbonButton();
        public RibbonButton Btn_About = new RibbonButton();

        public RibbonPanel panel6 = new RibbonPanel();
        public RibbonPanelSource panel6Panel = new RibbonPanelSource();

        public RibbonButton Btn_Setting = new RibbonButton();

        public RibbonPanel panel7 = new RibbonPanel();
        public RibbonPanelSource panel7Panel = new RibbonPanelSource();
        public RibbonButton pan7button1 = new RibbonButton();
        public RibbonButton Btn_DrawingInfo = new RibbonButton();

        //AutoCAD Command

        [CommandMethod("MyRibbon")]
        public void MyRibbon()
        {
            Tab.Title = "Redbracket";
            Tab.Id = "RibbonSample_TAB_ID";

            if (ribbonStatus)
            {
                Tab = ribbonControl.FindTab("RibbonSample_TAB_ID");
                Tab.Panels.Clear();
                ribbonControl.Tabs.Remove(Tab);
            }


            ribbonControl.Tabs.Add(Tab);

            ribbonStatus = true;

            //Connect Functionality
            panel1Panel.Title = "Connection";
            Panel1.Source = panel1Panel;
            Tab.Panels.Add(Panel1);

            Btn_Connection.Orientation = System.Windows.Controls.Orientation.Vertical;
            Btn_Connection.Size = RibbonItemSize.Large;

            //if (ArasConnector.ArasConnector.Isconnected == false)
            CADController.Controllers.ConnectionController connController = new CADController.Controllers.ConnectionController();
            if (!connect)
            {
                Btn_Connection.Text = "Log-in";
                Btn_Connection.ShowText = true;
                Btn_Connection.ShowImage = true;
                Btn_Connection.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.connect);
                Btn_Connection.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.connect);
                Btn_Connection.CommandHandler = new Connect();
            }
            else
            {
                Btn_Connection.Text = "Log-out";
                Btn_Connection.ShowText = true;
                Btn_Connection.ShowImage = true;
                Btn_Connection.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Disconnect);
                Btn_Connection.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Disconnect);
                Btn_Connection.CommandHandler = new Disconnect();
                browseDEnable = true;
                createDEnable = true;
                browseBEnable = true;
                createBEnable = true;
                LockEnable = true;
                UnlockEnable = true;
                SaveEnable = true;
                DrawingInfoEnable = true;
            }

            panel1Panel.Items.Add(Btn_Connection);


            // Browse Drawing and Create drawign Using Template
            panel2Panel.Title = "Basics";
            panel2.Source = panel2Panel;
            Tab.Panels.Add(panel2);

            Btn_BrowseDrawing.Text = "Open \nFile";
            Btn_BrowseDrawing.ShowText = true;
            Btn_BrowseDrawing.ShowImage = true;
            Btn_BrowseDrawing.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Open);
            Btn_BrowseDrawing.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Open);
            Btn_BrowseDrawing.Size = RibbonItemSize.Large;
            Btn_BrowseDrawing.Orientation = System.Windows.Controls.Orientation.Vertical;
            Btn_BrowseDrawing.CommandHandler = new BrowseDrawing();
            Btn_BrowseDrawing.IsEnabled = browseDEnable;


            /*  Btn_CreateDrawing.Text = "Create drawing \nUsing Template";
              Btn_CreateDrawing.ShowText = true;
              Btn_CreateDrawing.ShowImage = true;
              Btn_CreateDrawing.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Add_To_Aras);
              Btn_CreateDrawing.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Add_To_Aras);
              Btn_CreateDrawing.Size = RibbonItemSize.Large;
              Btn_CreateDrawing.Orientation = System.Windows.Controls.Orientation.Vertical;
              Btn_CreateDrawing.CommandHandler = new DrawingUsingTemplate();
              Btn_CreateDrawing.IsEnabled = createDEnable;
           */
            RibbonRowPanel pan2row1 = new RibbonRowPanel();
            pan2row1.Items.Add(Btn_BrowseDrawing);
            //  pan2row1.Items.Add(Btn_CreateDrawing);
            panel2Panel.Items.Add(pan2row1);



            //Blocks browse and Add functionalities
            /*   pan3Panel.Title = "Blocks";
               Panel3.Source = pan3Panel;
               Tab.Panels.Add(Panel3);


               Btn_BrowseBlock.Text = "Browse \nBlocks";
               Btn_BrowseBlock.ShowText = true;
               Btn_BrowseBlock.ShowImage = true;
               Btn_BrowseBlock.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Blocks);
               Btn_BrowseBlock.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Blocks);
               Btn_BrowseBlock.Size = RibbonItemSize.Large;
               Btn_BrowseBlock.Orientation = System.Windows.Controls.Orientation.Vertical;
               Btn_BrowseBlock.CommandHandler = new BrowseBlock();
               Btn_BrowseBlock.IsEnabled = browseBEnable;



               Btn_AddBlock.Text = "Add Blocks \nTo Aras";
               Btn_AddBlock.ShowText = true;
               Btn_AddBlock.ShowImage = true;
               Btn_AddBlock.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Add_Blocks);
               Btn_AddBlock.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Add_Blocks);
               Btn_AddBlock.Size = RibbonItemSize.Large;
               Btn_AddBlock.Orientation = System.Windows.Controls.Orientation.Vertical;
               Btn_AddBlock.CommandHandler = new AddBlock();
               Btn_AddBlock.IsEnabled = createBEnable;

               RibbonRowPanel pan3row1 = new RibbonRowPanel();
               pan3row1.Items.Add(Btn_BrowseBlock);
               pan3row1.Items.Add(Btn_AddBlock);
               pan3Panel.Items.Add(pan3row1);
            */
            //Lock, Unlock, and Save
            panel4Panel.Title = "Status";
            panel4.Source = panel4Panel;
            Tab.Panels.Add(panel4);


            Btn_Lock.Text = "Lock";
            Btn_Lock.ShowText = true;
            Btn_Lock.ShowImage = true;
            Btn_Lock.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Lock);
            Btn_Lock.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Lock);
            Btn_Lock.Size = RibbonItemSize.Large;
            Btn_Lock.Orientation = System.Windows.Controls.Orientation.Vertical;
            Btn_Lock.CommandHandler = new Lock();
            Btn_Lock.IsEnabled = LockEnable;


            Btn_Unlock.Text = "Unlock";
            Btn_Unlock.ShowText = true;
            Btn_Unlock.ShowImage = true;
            Btn_Unlock.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Unlock);
            Btn_Unlock.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Unlock);
            Btn_Unlock.Size = RibbonItemSize.Large;
            Btn_Unlock.Orientation = System.Windows.Controls.Orientation.Vertical;
            Btn_Unlock.CommandHandler = new Unlock();
            Btn_Unlock.IsEnabled = UnlockEnable;


            Btn_Save.Text = "Save";
            Btn_Save.ShowText = true;
            Btn_Save.ShowImage = true;
            Btn_Save.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Save);
            Btn_Save.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Save);
            Btn_Save.Size = RibbonItemSize.Large;
            Btn_Save.Orientation = System.Windows.Controls.Orientation.Vertical;
            Btn_Save.CommandHandler = new Save();
            Btn_Save.IsEnabled = SaveEnable;

            RibbonRowPanel pan4row1 = new RibbonRowPanel();
            pan4row1.Items.Add(Btn_Lock);
            pan4row1.Items.Add(Btn_Save);
            pan4row1.Items.Add(Btn_Unlock);
            panel4Panel.Items.Add(pan4row1);


            //Drawing Info
            panel7Panel.Title = "Drawing Info";
            panel7.Source = panel7Panel;
            Tab.Panels.Add(panel7);


            Btn_DrawingInfo.Text = "Drawing \n Info";
            Btn_DrawingInfo.ShowText = true;
            Btn_DrawingInfo.ShowImage = true;
            Btn_DrawingInfo.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.DrawingInfo);
            Btn_DrawingInfo.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.DrawingInfo);
            Btn_DrawingInfo.Size = RibbonItemSize.Large;
            Btn_DrawingInfo.Orientation = System.Windows.Controls.Orientation.Vertical;
            Btn_DrawingInfo.CommandHandler = new DrawingInfo();
            Btn_DrawingInfo.IsEnabled = DrawingInfoEnable;

            RibbonRowPanel pan7row1 = new RibbonRowPanel();
            pan7row1.Items.Add(Btn_DrawingInfo);
            panel7Panel.Items.Add(pan7row1);



            //Help and About
            panel5Panel.Title = "Help";
            panel5.Source = panel5Panel;
            Tab.Panels.Add(panel5);


            Btn_Help.Text = "Help";
            Btn_Help.ShowText = true;
            Btn_Help.ShowImage = true;
            Btn_Help.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Help);
            Btn_Help.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Help);
            Btn_Help.Size = RibbonItemSize.Standard;
            Btn_Help.CommandHandler = new Help();



            Btn_About.Text = "About";
            Btn_About.ShowText = true;
            Btn_About.ShowImage = true;
            Btn_About.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.about);
            Btn_About.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.about);
            Btn_About.Size = RibbonItemSize.Standard;
            Btn_About.CommandHandler = new About();


            RibbonRowPanel pan5row1 = new RibbonRowPanel();
            pan5row1.Items.Add(Btn_Help);
            pan5row1.Items.Add(new RibbonRowBreak());
            pan5row1.Items.Add(Btn_About);
            panel5Panel.Items.Add(pan5row1);



            //Setting
            panel6Panel.Title = "Setting";
            panel6.Source = panel6Panel;
            Tab.Panels.Add(panel6);


            Btn_Setting.Text = "Setting";
            Btn_Setting.ShowText = true;
            Btn_Setting.ShowImage = true;
            Btn_Setting.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Setting);
            Btn_Setting.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Setting);
            Btn_Setting.Size = RibbonItemSize.Large;
            Btn_Setting.Orientation = System.Windows.Controls.Orientation.Vertical;
            Btn_Setting.CommandHandler = new UserSetting();

            RibbonRowPanel pan6row1 = new RibbonRowPanel();
            pan6row1.Items.Add(Btn_Setting);
            panel6Panel.Items.Add(pan6row1);

            Tab.IsActive = true;
        }
    }

    //Implemented code for connect functionality
    public class Connect : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            AutocadPlugIn.login mylogin = new AutocadPlugIn.login();
            mylogin.ShowDialog();
        }
    }

    public class Disconnect : System.Windows.Input.ICommand
         {
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;
            #region LogOut
            public void Execute(object parameter)
            {
                try
                {
                    if (MessageBox.Show("Do you want to disconnect from Redbracket?", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                       DisconnectCommand objCmd = new DisconnectCommand();
                        BaseController controller = new DisconnectController();
                        controller.Execute(objCmd);

                        if (controller.errorString != null)
                        {
                            MessageBox.Show(controller.errorString);
                            return;
                        }

                        CADRibbon cr = new CADRibbon();


                        CADRibbon.connect = false;
                        cr.browseDEnable = false;
                        cr.createDEnable = false;
                        cr.browseBEnable = false;
                        cr.createBEnable = false;
                        cr.LockEnable = false;
                        cr.UnlockEnable = false;
                        cr.SaveEnable = false;
                        cr.DrawingInfoEnable = false;
                        cr.MyRibbon();
                      //  MessageBox.Show("Logged Out Successfully");
                    }

                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Logout Fail due to " + ex);
                    return;
                }

            }
            #endregion LogOut
         }

    public class BrowseDrawing : System.Windows.Input.ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                try
                {
                AutocadPlugIn.UI_Forms.Search_And_Open browseDrawing = new AutocadPlugIn.UI_Forms.Search_And_Open();
                browseDrawing.ShowDialog();
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex.Message);
                    return;
            }
            }
        }

    public class DrawingUsingTemplate : System.Windows.Input.ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {


            }
        }

    public class Save : System.Windows.Input.ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

                Database db = doc.Database;
                Editor ed = doc.Editor;

                //PromptStringOptions pso = new PromptStringOptions("\nEnter path to root drawing file: ");
                //pso.AllowSpaces = true;
               // PromptResult pr = ed.GetString(pso);

                if (!File.Exists(doc.Name))
                {
                    MessageBox.Show("Please Save Document on Local Computer", "Information");
                    return;
                }
                try
                {
                    AutocadPlugIn.UI_Forms.Save_Active_Drawings objSave = new AutocadPlugIn.UI_Forms.Save_Active_Drawings();
                    //Save_Active_Drawings objSave = new Save_Active_Drawings();
                    objSave.ShowDialog();

                 }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\nProblem reading/processing CAD File\"{0}\": {1}", doc.Name, ex.Message);
                }

              /* try
                {
                Autodesk.AutoCAD.ApplicationServices.Document objActivedoc = acadApp.DocumentManager.MdiActiveDocument;
                CADController.Commands.SaveCommand cmdSave = new SaveCommand();

                    CADManger objDocMgr = new CADManger();
                    SaveCommand objcmd = new SaveCommand();
                    Hashtable htAttributes = new Hashtable();
                    BaseController controller = new SaveController();

                    string path = objActivedoc.Name;

                    if (!path.Contains("\\"))
                    {
                        System.Windows.MessageBox.Show("Please save document first on local computer.");
                        return;
                    }

                    CADManger cadManager = new CADManger();
                    Hashtable drawingAttrs = new Hashtable();
                    drawingAttrs = (Hashtable)cadManager.GetAttributes();


                    //cmdSave.DrawingInformation.ItemType = drawingAttrs["type"].ToString();
                    //cmdSave.DrawingInformation.ObjectId = drawingAttrs["documentid"].ToString();
                    //cmdSave.DrawingInformation.FilePath = path;
                    //controller.Execute(cmdSave);

                   /* if (htAttributes.Contains(PresentationManager.documentProperties.DocumentId.ToString()))
                        objDocMgr.UpdateAttributes(controller.htDocumentProperty);
                    else
                        objDocMgr.SetAttributes(controller.htDocumentProperty);
                    SetControl_After_Save();
                    objDocMgr.CloseDocument(objDocMgr.GetActiveDocument(), false, System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                    MessageBox.Show("Document Saved Successfully");

                 */
                 /*  if (controller.errorString != null)
                    {
                        MessageBox.Show(controller.errorString.ToString());
                        return;
                    }
                    MessageBox.Show("Document Saved Successfully");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error is ::" + ex.Message);
                }*/
            }
        }

    public class DrawingInfo : System.Windows.Input.ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                DocumentInformationDisplay objDRGInfo = new DocumentInformationDisplay();
                objDRGInfo.ShowDialog();
            }
        }

    public class About : System.Windows.Input.ICommand
        {
            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;

            public void Execute(object parameter)
            {
                RibbonSample.UI_Forms.About about = new RibbonSample.UI_Forms.About();
                about.ShowDialog();
            }
        }

    public class Help : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
        try
            {
            CADController.Commands.HelpCommand objcmd = new CADController.Commands.HelpCommand();
            CADController.Controllers.BaseController controller = new CADController.Controllers.HelpController();

            String Dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //MessageBox.Show("directory path=" + Dir);
            objcmd.HelpFilePath = Dir + "\\Avrut_AutoCAD2013_Integration_UserGuide.chm";
            controller.Execute(objcmd);
            if (controller.errorString != null)
            {
                MessageBox.Show(controller.errorString);
                return;
            }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
            MessageBox.Show(ex.Message);
            }

        }
    }

    public class UserSetting : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            AutocadPlugIn.UI_Forms.UserSettings MySettings = AutocadPlugIn.UI_Forms.UserSettings.createUserSetting();
            MySettings.ShowDialog();
        }
    }

    public class BrowseBlock : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            /*

                */
        }
    }

    public class AddBlock : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            /*

                */
        }
    }

    public class Lock : System.Windows.Input.ICommand
    {
        /* lock status 0: not locked,
            *             1: Locked by logged-in User
            *             2: locked by other user.
            */
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            /*
            Connector cn = new Connector();
            cn.UpdateDocPropert();
            String lockstatus = cn.getLockStatus();

            if (lockstatus == "0")
            {
                cn.LockDocument();
                cn.UpdateDocPropert();
                acadApp.ShowAlertDialog("Document Locked");
                acadApp.ShowAlertDialog(cn.getLockStatus());
            }
            else if (lockstatus == "1")
            {
                acadApp.ShowAlertDialog("Already lock by you only");
                cn.UpdateDocPropert();
                acadApp.ShowAlertDialog(cn.getLockStatus());
            }
            else
            {
                acadApp.ShowAlertDialog("Already lock by other user");
                cn.UpdateDocPropert();
                acadApp.ShowAlertDialog(cn.getLockStatus());
            }

            */
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            //PromptStringOptions pso = new PromptStringOptions("\nEnter path to root drawing file: ");
            //pso.AllowSpaces = true;
            // PromptResult pr = ed.GetString(pso);



            if (!File.Exists(doc.Name))
            {
                ed.WriteMessage("\nFile does not exist.");
                return;
            }
            try
            {
            //AutocadPlugIn.UI_Forms.frmLock lockForm= new AutocadPlugIn.UI_Forms.frmLock();
            //lockForm.ShowDialog();

                  frmLock obj = new  frmLock();
               // frmRefresh obj = new frmRefresh();
                obj.ShowDialog();

            }

            catch (System.Exception ex)
            {
                //ed.WriteMessage("\nProblem reading/processing \"{0}\": {1}", doc.Name, ex.Message);
            }

        }
    }

    public class Unlock : System.Windows.Input.ICommand
    {
        /* lock status 0: not locked,
            *             1: Locked by logged-in User
            *             2: locked by other user.
            */
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            /*Connector cn = new Connector();
            cn.UpdateDocPropert();
            String lockstatus = cn.getLockStatus();


            if (lockstatus == "0")
            {
                acadApp.ShowAlertDialog("Document is not locked");
                acadApp.ShowAlertDialog(cn.getLockStatus());
            }
            else if (lockstatus == "1")
            {
                // acadApp.ShowAlertDialog("Already lock by you only");
                cn.UnlockDocument();
                cn.UpdateDocPropert();
                acadApp.ShowAlertDialog("Document Unlocked");
                acadApp.ShowAlertDialog(cn.getLockStatus());
            }
            else
            {
                //acadApp.ShowAlertDialog("Already lock by other user");
                cn.UnlockDocument();
                cn.UpdateDocPropert();
                acadApp.ShowAlertDialog("Document Unlocked");
                acadApp.ShowAlertDialog(cn.getLockStatus());
            }

            */
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            //PromptStringOptions pso = new PromptStringOptions("\nEnter path to root drawing file: ");
            //pso.AllowSpaces = true;
            // PromptResult pr = ed.GetString(pso);



            if (!File.Exists(doc.Name))
            {
                ed.WriteMessage("\nFile does not exist.");
                return;
            }
            try
            {
                AutocadPlugIn.UI_Forms.UnLock lockForm = new AutocadPlugIn.UI_Forms.UnLock();
                lockForm.ShowDialog();


            }

            catch (System.Exception ex)
            {
                //ed.WriteMessage("\nProblem reading/processing \"{0}\": {1}", doc.Name, ex.Message);
            }

        }
    }

    public class RibbonCommandHandler : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {


        }


    }

    public class Refresh : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            //PromptStringOptions pso = new PromptStringOptions("\nEnter path to root drawing file: ");
            //pso.AllowSpaces = true;
            // PromptResult pr = ed.GetString(pso);

            if (!File.Exists(doc.Name))
            {
                MessageBox.Show("Please Save Document on Local Computer", "Information");
                return;
            }
            try
            {
                DatabaseSummaryInfo dbsi = db.SummaryInfo;
                string S= dbsi.ToString();

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nProblem reading/processing CAD File\"{0}\": {1}", doc.Name, ex.Message);
            }

            /* try
              {
              Autodesk.AutoCAD.ApplicationServices.Document objActivedoc = acadApp.DocumentManager.MdiActiveDocument;
              CADController.Commands.SaveCommand cmdSave = new SaveCommand();

                  CADManger objDocMgr = new CADManger();
                  SaveCommand objcmd = new SaveCommand();
                  Hashtable htAttributes = new Hashtable();
                  BaseController controller = new SaveController();

                  string path = objActivedoc.Name;

                  if (!path.Contains("\\"))
                  {
                      System.Windows.MessageBox.Show("Please save document first on local computer.");
                      return;
                  }

                  CADManger cadManager = new CADManger();
                  Hashtable drawingAttrs = new Hashtable();
                  drawingAttrs = (Hashtable)cadManager.GetAttributes();


                  //cmdSave.DrawingInformation.ItemType = drawingAttrs["type"].ToString();
                  //cmdSave.DrawingInformation.ObjectId = drawingAttrs["documentid"].ToString();
                  //cmdSave.DrawingInformation.FilePath = path;
                  //controller.Execute(cmdSave);

                 /* if (htAttributes.Contains(PresentationManager.documentProperties.DocumentId.ToString()))
                      objDocMgr.UpdateAttributes(controller.htDocumentProperty);
                  else
                      objDocMgr.SetAttributes(controller.htDocumentProperty);
                  SetControl_After_Save();
                  objDocMgr.CloseDocument(objDocMgr.GetActiveDocument(), false, System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                  MessageBox.Show("Document Saved Successfully");

               */
            /*  if (controller.errorString != null)
               {
                   MessageBox.Show(controller.errorString.ToString());
                   return;
               }
               MessageBox.Show("Document Saved Successfully");
           }
           catch (System.Exception ex)
           {
               MessageBox.Show("Error is ::" + ex.Message);
           }*/
        }
    }

    public class Images
    {
        public static BitmapImage getBitmap(Bitmap image)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = stream;
            bmp.EndInit();

            return bmp;
        }
    }

    }

