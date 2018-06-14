using AutocadPlugIn.UI_Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using CADController.Commands;
using CADController.Controllers;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using RedBracketConnector;
using System.Collections.Generic;
using System.Collections;
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

        public RibbonPanelSource rpsSave = new RibbonPanelSource();
        public RibbonPanel rpSave = new RibbonPanel();

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

        public RibbonButton Btn_Refresh = new RibbonButton();



        [CommandMethod("AddDocEvent")]
        //[CommandMethod("AddDwgEvent")]
        [CommandMethod("StartEventHandling")]
        public void AddDocEvent()
        {
            // Get the current document
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            //  var v = Autodesk.AutoCAD.ApplicationServices.Application.MainWindow.

            // acDoc.SendStringToExecute("EXPORT", false, false, false);

            acDoc.BeginDocumentClose += new DocumentBeginCloseEventHandler(docBeginDocClose);
            acDoc.EndDwgOpen += new DrawingOpenEventHandler(docEndDwgOpen);

            acDoc.BeginDwgOpen += new DrawingOpenEventHandler(docBeginDwgOpen);
            acDoc.LayoutSwitched += new LayoutSwitchedEventHandler(docLayoutSwitched);




        }
        public void docLayoutSwitched(object senderObj,
                           LayoutSwitchedEventArgs docBegClsEvtArgs)
        {
            // Display a message box prompting to continue closing the document
            //if (System.Windows.Forms.MessageBox.Show(
            //                     "The document is about to be closed." +
            //                     "\nDo you want to continue?",
            //                     "Close Document",
            //                     System.Windows.Forms.MessageBoxButtons.YesNo) ==
            //                     System.Windows.Forms.DialogResult.No)
            //{
            //    docBegClsEvtArgs.Veto();
            //}
        }
        public void docEndDwgOpen(object senderObj,
                             DrawingOpenEventArgs docBegClsEvtArgs)
        {
            // Display a message box prompting to continue closing the document
            //if (System.Windows.Forms.MessageBox.Show(
            //                     "The document is about to be closed." +
            //                     "\nDo you want to continue?",
            //                     "Close Document",
            //                     System.Windows.Forms.MessageBoxButtons.YesNo) ==
            //                     System.Windows.Forms.DialogResult.No)
            //{
            //    docBegClsEvtArgs.Veto();
            //}
        }
        public void docBeginDwgOpen(object senderObj,
                             DrawingOpenEventArgs docBegClsEvtArgs)
        {
            // Display a message box prompting to continue closing the document
            //if (System.Windows.Forms.MessageBox.Show(
            //                     "The document is about to be closed." +
            //                     "\nDo you want to continue?",
            //                     "Close Document",
            //                     System.Windows.Forms.MessageBoxButtons.YesNo) ==
            //                     System.Windows.Forms.DialogResult.No)
            //{
            //    docBegClsEvtArgs.Veto();
            //}
        }
        public void docBeginDocClose(object senderObj,
                             DocumentBeginCloseEventArgs docBegClsEvtArgs)
        {
            // Display a message box prompting to continue closing the document
            //if (System.Windows.Forms.MessageBox.Show(
            //                     "The document is about to be closed." +
            //                     "\nDo you want to continue?",
            //                     "Close Document",
            //                     System.Windows.Forms.MessageBoxButtons.YesNo) ==
            //                     System.Windows.Forms.DialogResult.No)
            //{
            //    docBegClsEvtArgs.Veto();
            //}
        }

        //AutoCAD Command

        [CommandMethod("MyRibbon")]
        public void MyRibbon()
        {
            if (!RedBracketConnector.Helper.IsEventAssign)
            {
                AddDocEvent();
                //RedBracketConnector.Helper.IsEventAssign = true;
            }

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

            rpsSave.Title = "Save";
            rpSave.Source = rpsSave;
            Tab.Panels.Add(rpSave);

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

            RibbonRowPanel rrpSave = new RibbonRowPanel();
            rrpSave.Items.Add(Btn_Save);
            rpsSave.Items.Add(rrpSave);

            RibbonRowPanel pan4row1 = new RibbonRowPanel();
            pan4row1.Items.Add(Btn_Lock);

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

            Btn_Refresh.Text = "Refresh";
            Btn_Refresh.ShowText = true;
            Btn_Refresh.ShowImage = true;
            Btn_Refresh.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Refresh);
            Btn_Refresh.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Refresh);
            Btn_Refresh.Size = RibbonItemSize.Large;
            Btn_Refresh.Orientation = System.Windows.Controls.Orientation.Vertical;
            Btn_Refresh.CommandHandler = new Refresh();
            Btn_Refresh.IsEnabled = LockEnable;
            pan7row1.Items.Add(Btn_Refresh);



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
                objcmd.HelpFilePath = Dir + "\\RedBracketConnector_Integration_UserGuide.chm";
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

                frmLock obj = new frmLock();
                //frmRefresh obj = new frmRefresh();
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
                //Geting File info form summuryinfo




                string drawingid = "", updatedon = "", projectname = "", projectnameOnly = "";
                string ProjectNo = "", DrawingNO = "", FileType = "", Rev = "", PreFix = "";
                try
                {
                    var dbsi = db.SummaryInfo.CustomProperties;
                    while (dbsi.MoveNext())
                    {
                        if (Convert.ToString(dbsi.Key) == "drawingid")
                        {
                            drawingid = Convert.ToString(dbsi.Value);
                        }
                        else if (Convert.ToString(dbsi.Key) == "modifiedon")
                        {
                            updatedon = Convert.ToString(dbsi.Value);
                        }
                        else if (Convert.ToString(dbsi.Key) == "projectname")
                        {
                            projectname = Convert.ToString(dbsi.Value);
                        }
                        else if (Convert.ToString(dbsi.Key) == "projectno")
                        {
                            ProjectNo = Convert.ToString(dbsi.Value);
                        }
                        else if (Convert.ToString(dbsi.Key) == "drawingnumber")
                        {
                            DrawingNO = Convert.ToString(dbsi.Value);
                        }
                        else if (Convert.ToString(dbsi.Key) == "filetypeid")
                        {
                            FileType = Convert.ToString(dbsi.Value);
                        }
                        else if (Convert.ToString(dbsi.Key) == "revision")
                        {
                            Rev = Convert.ToString(dbsi.Value);
                        }
                        else if (Convert.ToString(dbsi.Key) == "projectid")
                        {
                            projectnameOnly = Convert.ToString(dbsi.Value);
                        }
                        else if (Convert.ToString(dbsi.Key) == "prefix")
                        {
                            PreFix = Convert.ToString(dbsi.Value);
                        }
                    }

                    //if (ProjectNo.Trim().Length > 0)
                    //{
                    //    PreFix = ProjectNo + "-";
                    //}
                    //PreFix = PreFix + DrawingNO + "-";

                    //PreFix += Convert.ToString(FileType) == string.Empty ? string.Empty : Convert.ToString(FileType) + "-";

                    //PreFix += Convert.ToString(Rev) == string.Empty ? string.Empty : Convert.ToString(Rev) + "#";

                }
                catch (System.Exception E)
                {

                }
                if (drawingid.Trim().Length == 0 || updatedon.Trim().Length == 0)
                {
                    return;
                }









                //Checking if file is in redbracket or not;
                RedBracketConnector.RBConnector objRBC = new RedBracketConnector.RBConnector();

                RedBracketConnector.ResultSearchCriteria Drawing = objRBC.GetDrawingInformation(objRBC.SearchLatestFile(DrawingNO));

                //if (Drawing == null)
                //{
                //    RedBracketConnector.ShowMessage.InfoMess("File is no longer available in RedBracket.");
                //    return;
                //}
                if (Convert.ToDateTime(Drawing.updatedon) > Convert.ToDateTime(updatedon))
                {
                    if (RedBracketConnector.ShowMessage.InfoYNMess("RedBracket has updated version of this file, do you want to download it ?." + Environment.NewLine + "Your changes will be lost.") == DialogResult.Yes)
                    {
                        string checkoutPath = RedBracketConnector.Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath").ToString();
                        string ProjectName = projectnameOnly;
                        if (ProjectName.Trim().Length == 0)
                        {
                            ProjectName = "MyFiles";
                        }
                        checkoutPath = Path.Combine(checkoutPath, ProjectName);
                        if (!Directory.Exists(checkoutPath))
                        {
                            Directory.CreateDirectory(checkoutPath);
                        }
                        AutoCADManager objMgr = new AutoCADManager();
                        objMgr.ChecknCloseOpenedDoc(db.Filename);

                        DownloadOpenDocument(Drawing.id, checkoutPath);

                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    RedBracketConnector.ShowMessage.InfoMess("This file is latest file.");
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nProblem reading/processing CAD File\"{0}\": {1}", doc.Name, ex.Message);
            }
        }
        public void DownloadOpenDocument1(string fileId, string checkoutPath, bool IsParent = false)
        {
            try
            {
                RBConnector objRBC = new RBConnector();
                Hashtable DrawingProperty = new Hashtable();

                byte[] RawBytes = null;
                ResultSearchCriteria Drawing = objRBC.GetSingleFileInfo(fileId, ref RawBytes);

                if (Drawing != null)
                {
                    DrawingProperty.Add("DrawingId", Drawing.id);
                    DrawingProperty.Add("DrawingName", Drawing.name);
                    DrawingProperty.Add("Classification", "");
                    DrawingProperty.Add("FileTypeID", Drawing.type == null ? string.Empty : Drawing.type.name == null ? string.Empty : Drawing.type.name);
                    DrawingProperty.Add("DrawingNumber", Drawing.fileNo);

                    DrawingProperty.Add("DrawingState", Drawing.status == null ? string.Empty : Drawing.status.statusname == null ? string.Empty : Drawing.status.statusname);
                    DrawingProperty.Add("Revision", Drawing.versionno);
                    DrawingProperty.Add("LockStatus", Drawing.filelock);
                    DrawingProperty.Add("Generation", "123");
                    DrawingProperty.Add("Type", Drawing.coreType.id);
                    //DrawingProperty.Add("ProjectName", Drawing.projectname );
                    if (Drawing.projectname.Trim().Length == 0)
                    {
                        DrawingProperty.Add("ProjectName", "My Files");
                    }
                    else
                    {
                        DrawingProperty.Add("ProjectName", Drawing.projectname + " (" + Drawing.projectNumber + ")");
                    }

                    DrawingProperty.Add("ProjectId", Drawing.projectinfo);
                    DrawingProperty.Add("CreatedOn", Drawing.updatedon);
                    DrawingProperty.Add("CreatedBy", Drawing.createdby);
                    DrawingProperty.Add("ModifiedOn", Drawing.updatedon);
                    DrawingProperty.Add("ModifiedBy", Drawing.updatedby);

                    DrawingProperty.Add("canDelete", Drawing.canDelete);
                    DrawingProperty.Add("isowner", Drawing.isowner);
                    DrawingProperty.Add("hasViewPermission", Drawing.hasViewPermission);
                    DrawingProperty.Add("isActFileLatest", Drawing.isActFileLatest);

                    DrawingProperty.Add("isEditable", Drawing.isEditable);
                    DrawingProperty.Add("canEditStatus", Drawing.canEditStatus);
                    DrawingProperty.Add("hasStatusClosed", Drawing.hasStatusClosed);
                    DrawingProperty.Add("isletest", Drawing.isletest);

                    DrawingProperty.Add("projectno", Drawing.projectNumber == null ? string.Empty : Drawing.projectNumber);

                    string ProjectNo = Drawing.projectNumber == null ? string.Empty : Drawing.projectNumber;


                    string FileType = Drawing.type == null ? string.Empty : Drawing.type.name == null ? string.Empty : Drawing.type.name;

                    string PreFix = "";
                    if (ProjectNo.Trim().Length > 0)
                    {
                        PreFix = ProjectNo + "-";
                    }
                    PreFix = PreFix + Drawing.fileNo + "-";

                    PreFix += Convert.ToString(FileType) == string.Empty ? string.Empty : Convert.ToString(FileType) + "-";

                    PreFix += Convert.ToString(Drawing.versionno) == string.Empty ? string.Empty : Convert.ToString(Drawing.versionno) + "#";

                    DrawingProperty.Add("prefix", PreFix);
                    //DrawingProperty.Add("isroot", true);
                    //DrawingProperty.Add("sourceid","");
                    //DrawingProperty.Add("Layouts","");

                    string filePathName = Path.Combine(checkoutPath,  Drawing.name);
                    if (IsParent)
                    {
                        filePathName = Path.Combine(checkoutPath, PreFix + Drawing.name);
                    }

                        using (var binaryWriter = new BinaryWriter(File.Open(filePathName, FileMode.OpenOrCreate)))
                    {
                        binaryWriter.Write(RawBytes);
                    }
                    AutoCADManager objMgr = new AutoCADManager();
                    if (IsParent)
                    {

                        objMgr.UpdateExRefPathInfo(filePathName);
                        objMgr.OpenActiveDocument(filePathName, "View", DrawingProperty);
                    }
                    else
                    {
                        objMgr.SetAttributesXrefFiles(DrawingProperty, filePathName);
                        objMgr.UpdateLayoutAttributeArefFile(DrawingProperty, filePathName);
                    }

                }
                else
                {
                    ShowMessage.ErrorMess("Some error occures while retrieving file.");
                }
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

        }

        public void DownloadOpenDocument(string fileId, string checkoutPath)
        {
            try
            {
                RBConnector objRBC = new RBConnector();
                var XrefFIle = objRBC.GetXrefFIleInfo(fileId);

                foreach (ResultSearchCriteria obj in XrefFIle)
                {
                    DownloadOpenDocument1(obj.id, checkoutPath, false);
                }

                DownloadOpenDocument1(fileId, checkoutPath, true);

            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

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

