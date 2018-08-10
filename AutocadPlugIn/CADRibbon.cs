using AutocadPlugIn.UI_Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

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
        public Timer timerVersion = new Timer();
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
        public RibbonButton Btn_SaveAS = new RibbonButton();

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
        public RibbonButton Btn_Refresh_H = new RibbonButton();

        public RibbonPanel rpCurrentDI = new RibbonPanel();
        public RibbonPanelSource rpsCurrentDI = new RibbonPanelSource();
        public RibbonLabel lblCurrentFileVersion = new RibbonLabel();
        public RibbonLabel lblCurrentFileVersion1 = new RibbonLabel();
        public RibbonLabel lblCurrentFileVersionRB = new RibbonLabel();
        public RibbonLabel lblCurrentFileVersionRB1 = new RibbonLabel();
        public RibbonLabel txtCurrentFileVersion = new RibbonLabel();
        public RibbonLabel txtCurrentFileVersionRB = new RibbonLabel();

        public string CurrentVersion = "";
        public string LatestVersion = "";






        //[assembly: CommandClass(typeof(DocumentActevatedEvent.MyCommands))]
        //[assembly: ExtensionApplication(typeof(DocumentActevatedEvent.MyCommands))]


        public class MyCommands : IExtensionApplication
        {
            #region IExtensionApplication Members

            public void Initialize()
            {
                try
                {

                    RunMyCommand();
                    DocumentCollection DC = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager;
                    if (DC != null)
                    {
                        Document dwg = DC.MdiActiveDocument;
                        if (dwg != null)
                        {
                            Editor ed = dwg.Editor;

                            if (ed != null)
                            {
                                try
                                {
                                    ed.WriteMessage("\nInitializing...");

                                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.DocumentActivated += DocumentManager_DocumentActivated;

                                    ed.WriteMessage("completed.");
                                }
                                catch (System.Exception ex)
                                {
                                    ed.WriteMessage("failed:\n");
                                    ed.WriteMessage(ex.Message);
                                }
                            }
                        }
                    }






                    Autodesk.AutoCAD.Internal.Utils.PostCommandPrompt();
                }
                catch { }
            }

            private void DocumentManager_DocumentActivated(object sender, DocumentCollectionEventArgs e)
            {
                try
                {
                    CADRibbon objcr = new CADRibbon();
                    if (!connect)
                    {
                        //objcr.GetVersion(ref objcr.CurrentVersion,ref objcr.LatestVersion);
                        objcr.RBRibbon();
                        return;
                    }

                    if (!Helper.CheckFileInfoFlag)
                        return;

                    if (e != null)
                    {
                        if (e.Document != null)
                        {
                            Document doc = e.Document;

                            if (doc != null)
                            {
                                Cursor.Current = Cursors.WaitCursor;
                                doc.Editor.WriteMessage("\n{0} activated.", e.Document.Name);


                                objcr.RBRibbon();
                                if (Helper.CurrentVersion != Helper.LatestVersion)
                                {
                                    Refresh objrfs = new Refresh();
                                    objrfs.Execute(null);
                                }
                                Autodesk.AutoCAD.Internal.Utils.PostCommandPrompt();
                                Cursor.Current = Cursors.Default;
                            }
                        }


                    }
                }
                catch
                {

                }
                //if(e!=null)
                //{

                //}




            }

            public void Terminate()
            {

            }

            #endregion

            [CommandMethod("AddNewDoc", CommandFlags.Session)]
            public static void RunMyCommand()
            {
                try
                {
                    if (Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.Count > 0)
                    {

                    }
                    else
                    {
                        Document dwg = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.Add(null);
                        Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument = dwg;
                    }

                }
                catch
                {
                    Document dwg = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.Add(null);
                    Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument = dwg;
                }


            }
        }
        public void GetVersion(ref string CurrentVersion, ref string LatestVersion)
        {
            //return;

            if (!connect)
            {
                Helper.CurrentVersion = CurrentVersion = "";
                Helper.LatestVersion = LatestVersion = "";
                return;
            }

            if (!Helper.CheckFileInfoFlag)
                return;
            Cursor.Current = Cursors.WaitCursor;


            // find version of local file and assign.
            string drawingid = "", Rev = "", DrawingNO = "";

            try
            {

                System.Data.DataTable dtDrawing = Helper.cadManager.GetAttributes(Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database.Filename);

                if (dtDrawing.Rows.Count > 0)
                {
                    drawingid = Convert.ToString(dtDrawing.Rows[0]["DrawingId"]);
                    Rev = Convert.ToString(dtDrawing.Rows[0]["revision"]);
                    DrawingNO = Convert.ToString(dtDrawing.Rows[0]["DrawingNumber"]);

                    Helper.CurrentVersion = CurrentVersion = Helper.VerTextAdjustment(Rev);
                    Helper.LatestVersion = LatestVersion = Helper.GetLatestVersion(DrawingNO);
                }

                if (drawingid.Trim().Length == 0)
                {
                    //write code to hide current drawing information panel.
                    Helper.CurrentVersion = "";
                    Helper.LatestVersion = "";
                    return;
                }


            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            Cursor.Current = Cursors.Default;
        }

        //AutoCAD Command

        [CommandMethod("RBRibbon")]
        public void RBRibbon()
        {
            try
            {

                Tab = new RibbonTab();
                panel1Panel = new RibbonPanelSource();
                Panel1 = new RibbonPanel();

                panel2 = new RibbonPanel();
                panel2Panel = new RibbonPanelSource();

                Panel3 = new RibbonPanel();
                pan3Panel = new RibbonPanelSource(); 

                panel4Panel = new RibbonPanelSource();
                panel4 = new RibbonPanel();

                rpsSave = new RibbonPanelSource();
                rpSave = new RibbonPanel(); 

                panel5 = new RibbonPanel();
                panel5Panel = new RibbonPanelSource(); 

                panel6 = new RibbonPanel();
                panel6Panel = new RibbonPanelSource(); 

                panel7 = new RibbonPanel();
                panel7Panel = new RibbonPanelSource();


                rpCurrentDI = new RibbonPanel();
                rpsCurrentDI = new RibbonPanelSource();







                //if (!RedBracketConnector.Helper.IsEventAssign)
                {

                    timerVersion.Tick += new System.EventHandler(timerVersion_Tick);
                    timerVersion.Interval = 1000;
                    timerVersion.Start();
                    AutocadPlugIn.Helper.IsEventAssign = true;
                }

                Tab.Title = "Redbracket";
                Tab.Id = "Redbracket_TAB_ID";

                if (ribbonStatus)
                {
                    RibbonTab Tab1;
                    do
                    {
                        try
                        {
                            Tab1 = ribbonControl.FindTab("Redbracket_TAB_ID");
                            if (Tab1 != null)
                            {
                                Tab1.Panels.Clear();
                                ribbonControl.Tabs.Remove(Tab1);
                            }

                        }
                        catch (System.Exception E)
                        {
                            Tab1 = new RibbonTab();
                        }
                    } while (Tab1 != null);

                }

                if (ribbonControl == null)
                {
                    return;
                }
                ribbonControl.Tabs.Add(Tab);

                ribbonStatus = true;

                //Connect Functionality
                panel1Panel.Title = "Connection";
                Panel1.Source = panel1Panel;
                Tab.Panels.Add(Panel1);

                Btn_Connection.Orientation = System.Windows.Controls.Orientation.Vertical;
                Btn_Connection.Size = RibbonItemSize.Large;


                ConnectionController connController = new ConnectionController();
                if (!connect)
                {
                    Btn_Connection.Text = "Log-in";
                    Btn_Connection.ShowText = true;
                    Btn_Connection.ShowImage = true;
                    Btn_Connection.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.connect);
                    Btn_Connection.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.connect);
                    Btn_Connection.CommandHandler = new Connect();

                    Helper.CurrentVersion = "";
                    Helper.LatestVersion = "";
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



                panel2Panel.Title = "File Operations";
                panel2.Source = panel2Panel;
                Tab.Panels.Add(panel2);

                

                Btn_BrowseDrawing.Text = "Open File";
                Btn_BrowseDrawing.ShowText = true;
                Btn_BrowseDrawing.ShowImage = true;
                Btn_BrowseDrawing.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Open);
                Btn_BrowseDrawing.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Open);
                Btn_BrowseDrawing.Size = RibbonItemSize.Large;
                Btn_BrowseDrawing.Orientation = System.Windows.Controls.Orientation.Vertical;
                Btn_BrowseDrawing.CommandHandler = new BrowseDrawing();
                Btn_BrowseDrawing.IsEnabled = browseDEnable;



                RibbonRowPanel pan2row1 = new RibbonRowPanel();
                pan2row1.Items.Add(Btn_BrowseDrawing);
                panel2Panel.Items.Add(pan2row1);




                //Lock, Unlock, and Save







                Btn_Unlock.Text = "Lock & Unlock";
                Btn_Unlock.ShowText = true;
                Btn_Unlock.ShowImage = true;
                Btn_Unlock.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.LockUnlock);
                Btn_Unlock.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.LockUnlock);
                Btn_Unlock.Size = RibbonItemSize.Large;
                Btn_Unlock.Orientation = System.Windows.Controls.Orientation.Vertical;
                Btn_Unlock.CommandHandler = new Unlock();
                Btn_Unlock.IsEnabled = UnlockEnable;


                Btn_Save.Text = "Save to redbracket";
                Btn_Save.Name = "Save";
                Btn_Save.ShowText = true;
                Btn_Save.ShowImage = true;
                Btn_Save.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Save);
                Btn_Save.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Save);
                Btn_Save.Size = RibbonItemSize.Large;
                Btn_Save.Orientation = System.Windows.Controls.Orientation.Vertical;
                Btn_Save.CommandHandler = new Save();
                Btn_Save.IsEnabled = SaveEnable;

                Btn_SaveAS.Text = "Save As New";
                Btn_Save.Name = "SaveAs";
                Btn_SaveAS.ShowText = true;
                Btn_SaveAS.ShowImage = true;
                Btn_SaveAS.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.SaveAs);
                Btn_SaveAS.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.SaveAs);
                Btn_SaveAS.Size = RibbonItemSize.Large;
                Btn_SaveAS.Orientation = System.Windows.Controls.Orientation.Vertical;
                Btn_SaveAS.CommandHandler = new Save();
                Btn_SaveAS.IsEnabled = SaveEnable;

                RibbonRowPanel rrpSave = new RibbonRowPanel();
                rrpSave.Items.Add(Btn_Save);

                RibbonRowPanel rrpSaveAS = new RibbonRowPanel();
                rrpSaveAS.Items.Add(Btn_SaveAS);
                //rpsSave.Items.Add(rrpSave);

                RibbonRowPanel pan4row1 = new RibbonRowPanel();
                //pan4row1.Items.Add(Btn_Lock);

                pan4row1.Items.Add(Btn_Unlock);
                //panel4Panel.Items.Add(pan4row1);

                panel2Panel.Items.Add(rrpSave);
                panel2Panel.Items.Add(rrpSaveAS);
                panel2Panel.Items.Add(pan4row1);


                //Drawing Info
                //panel7Panel.Title = "Drawing Info";
                //panel7.Source = panel7Panel;
                //Tab.Panels.Add(panel7);


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
                //panel7Panel.Items.Add(pan7row1);
                panel2Panel.Items.Add(pan7row1);


                //Help and About
                panel5Panel.Title = "Help & About";
                panel5.Source = panel5Panel;
                Tab.Panels.Add(panel5);


                Btn_Help.Text = "Help";
                Btn_Help.ShowText = true;
                Btn_Help.ShowImage = true;
                Btn_Help.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Help);
                Btn_Help.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Help);
                Btn_Help.Size = RibbonItemSize.Large;
                Btn_Help.Orientation = System.Windows.Controls.Orientation.Vertical;
                Btn_Help.CommandHandler = new Help();



                Btn_About.Text = "About";
                Btn_About.ShowText = true;
                Btn_About.ShowImage = true;
                Btn_About.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.About);
                Btn_About.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.About);
                Btn_About.Size = RibbonItemSize.Large;
                Btn_About.Orientation = System.Windows.Controls.Orientation.Vertical;
                Btn_About.CommandHandler = new About();


                RibbonRowPanel pan5row1 = new RibbonRowPanel();
                pan5row1.Items.Add(Btn_Help);
                //pan5row1.Items.Add(new RibbonRowBreak());
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

                Btn_Refresh_H.Text = "Refresh";
                Btn_Refresh_H.ShowText = true;
                Btn_Refresh_H.ShowImage = true;
                Btn_Refresh_H.Image = Images.getBitmap(AutocadPlugIn.Properties.Resources.Refresh);
                Btn_Refresh_H.LargeImage = Images.getBitmap(AutocadPlugIn.Properties.Resources.Refresh);
                Btn_Refresh_H.Size = RibbonItemSize.Large;
                Btn_Refresh_H.Orientation = System.Windows.Controls.Orientation.Horizontal;
                Btn_Refresh_H.Width = 300;
                Btn_Refresh_H.CommandHandler = new Refresh();
                Btn_Refresh_H.IsEnabled = LockEnable;
                //pan7row1.Items.Add(Btn_Refresh);













                lblCurrentFileVersion.Text = "  Current  ";
                lblCurrentFileVersion1.Text = "  Version  ";
                lblCurrentFileVersionRB.Text = "   Latest  ";
                lblCurrentFileVersionRB1.Text = "  Version  ";
                GetVersion(ref CurrentVersion, ref LatestVersion);
                txtCurrentFileVersion.Text = CurrentVersion;//"        0.1       ";
                txtCurrentFileVersion.Tag = CurrentVersion;// "        0.1       ";
                txtCurrentFileVersionRB.Text = LatestVersion;// "        0.2       ";

                RibbonRowPanel rrpCurrentDI = new RibbonRowPanel();


                rrpCurrentDI.Items.Add(lblCurrentFileVersion);
                rrpCurrentDI.Items.Add(new RibbonRowBreak());
                rrpCurrentDI.Items.Add(lblCurrentFileVersion1);
                rrpCurrentDI.Items.Add(new RibbonRowBreak());
                rrpCurrentDI.Items.Add(txtCurrentFileVersion);

                rpsCurrentDI.Items.Add(rrpCurrentDI);

                RibbonRowPanel rrpCurrentDIV = new RibbonRowPanel();


                rrpCurrentDIV.Items.Add(lblCurrentFileVersionRB);
                rrpCurrentDIV.Items.Add(new RibbonRowBreak());
                rrpCurrentDIV.Items.Add(lblCurrentFileVersionRB1);
                rrpCurrentDIV.Items.Add(new RibbonRowBreak());
                rrpCurrentDIV.Items.Add(txtCurrentFileVersionRB);



                rpsCurrentDI.Items.Add(rrpCurrentDIV);
                rpsCurrentDI.Items.Add(new RibbonRowBreak());
                rpsCurrentDI.Items.Add(Btn_Refresh_H);
                //Compare Drawing Info
                rpsCurrentDI.Title = "Compare Drawing Info";
                rpCurrentDI.Source = rpsCurrentDI;
                Tab.Panels.Add(rpCurrentDI);




                Tab.IsActive = true;
            }
            catch (System.Exception E)
            { }

        }

        public void AssignVersion(string CurrentVersion = null, string LatestVersion = null)
        {
            try
            {
                if (CurrentVersion != null && LatestVersion != null)
                {
                    txtCurrentFileVersion.Tag = txtCurrentFileVersion.Text = CurrentVersion;
                    txtCurrentFileVersionRB.Text = LatestVersion;
                }
            }
            catch { }
        }

        private void timerVersion_Tick(object sender, EventArgs e)
        {
            try
            {
                //string V1 = txtCurrentFileVersion.Text == null ? string.Empty : txtCurrentFileVersion.Text.Trim();
                //string V2 = txtCurrentFileVersionRB.Text == null ? string.Empty : txtCurrentFileVersionRB.Text.Trim();

                string V1 = Helper.CurrentVersion;
                string V2 = Helper.LatestVersion;

                if (V1 != V2 && V1.Length > 0)
                {
                    int s = DateTime.Now.Second;

                    if (s % 2 == 0)
                    {
                        txtCurrentFileVersion.Text = Helper.VerTextAdjustment("is old");// "      is old      ";
                    }
                    else
                    {
                        txtCurrentFileVersion.Text = Helper.CurrentVersion;
                    }

                    //Btn_Save.IsEnabled = false;
                    Btn_Save.IsEnabled = SaveEnable;
                    if (SaveEnable)
                    {
                        Btn_SaveAS.IsEnabled = true;
                    }
                    else
                    {
                        Btn_SaveAS.IsEnabled = SaveEnable;
                    }
                }
                else
                {
                    Btn_Save.IsEnabled = SaveEnable;
                    if (SaveEnable)
                    {
                        if (V1.Trim().Length == 0 || V2.Trim().Length == 0)
                            Btn_SaveAS.IsEnabled = false;
                        else
                            Btn_SaveAS.IsEnabled = true;
                    }
                    else
                    {
                        Btn_SaveAS.IsEnabled = SaveEnable;
                    }
                }
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
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
                if (ShowMessage.InfoYNMess("Do you want to disconnect from Redbracket?", "Confirmation") == System.Windows.Forms.DialogResult.Yes)
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
                    cr.RBRibbon();
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
                AutocadPlugIn.UI_Forms.frmSearch_And_Open browseDrawing = new AutocadPlugIn.UI_Forms.frmSearch_And_Open();
                browseDrawing.ShowDialog();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex.Message);
                return;
            }
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
            bool IsSaveAs = false;
            RibbonButton btn = (RibbonButton)parameter;
            if (btn.Text == "Save As New")
            {
                IsSaveAs = true;
            }
            if (btn.Text != "Save As New" && btn.Text != "Save to redbracket")
            {
                return;
            }
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            Database db = doc.Database;
            Editor ed = doc.Editor;



            if (!File.Exists(doc.Name))
            {
                MessageBox.Show("Please Save Document on Local Computer", "Information");
                return;
            }
            try
            {
                bool FormOpen = true;
                AutocadPlugIn.UI_Forms.frmSave_Active_Drawings objSave = new AutocadPlugIn.UI_Forms.frmSave_Active_Drawings(ref FormOpen, IsSaveAs);
                //Save_Active_Drawings objSave = new Save_Active_Drawings();
                FormOpen = FormOpen ? objSave.FormLoad() : FormOpen;
                if (FormOpen)
                    objSave.ShowDialog();

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nProblem reading/processing CAD File\"{0}\": {1}", doc.Name, ex.Message);
            }


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
            //DocumentInformationDisplay objDRGInfo = new DocumentInformationDisplay();
            AutoCADManager cadManager = new AutoCADManager();
            System.Data.DataRow[] dtCurrentData = cadManager.GetExternalRefreces().Select("isroot=true");

            if (dtCurrentData.Length > 0)
            {
                string DrawingID = Convert.ToString(dtCurrentData[0]["drawingid"]);
                if (DrawingID.Trim().Length > 0)
                {
                    frmDrawingInfo objDRGInfo = new frmDrawingInfo();
                    objDRGInfo.ShowDialog();
                }
                else
                {
                    ShowMessage.ValMess("Please save this drawing to "+Helper.CompanyName+".");
                    return;
                }
            }
            else
            {
                ShowMessage.ValMess("Please save this drawing to " + Helper.CompanyName + ".");
                return;
            }
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
                HelpCommand objcmd = new HelpCommand();
                BaseController controller = new HelpController();

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
                //AutocadPlugIn.UI_Forms.UnLock lockForm = new AutocadPlugIn.UI_Forms.UnLock();
                AutocadPlugIn.UI_Forms.frmLockUnLock lockForm = new AutocadPlugIn.UI_Forms.frmLockUnLock();

                lockForm.ShowDialog();


            }

            catch (System.Exception ex)
            {
                //ed.WriteMessage("\nProblem reading/processing \"{0}\": {1}", doc.Name, ex.Message);
            }

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


                Cursor.Current = Cursors.WaitCursor;

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


                }
                catch (System.Exception E)
                {

                }
                if (drawingid.Trim().Length == 0 || updatedon.Trim().Length == 0)
                {
                    Cursor.Current = Cursors.Default;
                    return;
                }


                //Checking if file is in redbracket or not;
                RBConnector objRBC = new RBConnector();

                ResultSearchCriteria Drawing = objRBC.GetDrawingInformation(objRBC.SearchLatestFile(DrawingNO));

                //if (Drawing == null)
                //{
                //    RedBracketConnector.ShowMessage.InfoMess("File is no longer available in RedBracket.");
                //    return;
                //}
                if ((Drawing.versionno) != (Rev))
                {
                    if (ShowMessage.InfoYNMess("RedBracket has updated version of this file, do you want to download it ?." + Environment.NewLine + "Your changes  in current file will be lost.") == DialogResult.Yes)
                    {
                        string checkoutPath = Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath").ToString();
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
                    if (Rev == Drawing.versionno)
                    {
                        if (Convert.ToDateTime(Drawing.updatedon) != Convert.ToDateTime(updatedon))
                        {
                            AutoCADManager cadManager = new AutoCADManager();
                            cadManager.UpdateFileProperties(drawingid, db.Filename);
                        }

                    }
                    Cursor.Current = Cursors.Default;
                    if (parameter != null)
                        ShowMessage.InfoMess("This file is latest file.");
                }

            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nProblem reading/processing CAD File\"{0}\": {1}", doc.Name, ex.Message);
            }
            Cursor.Current = Cursors.Default;
        }


        public void DownloadOpenDocument(string fileId, string checkoutPath)
        {
            try
            {
                RBConnector objRBC = new RBConnector();
                var XrefFIle = objRBC.GetXrefFIleInfo(fileId);

                foreach (ResultSearchCriteria obj in XrefFIle)
                {
                    Helper.DownloadFile(obj.id);
                }

                Helper.DownloadFile(fileId);

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

