using RBAutocadPlugIn.UI_Forms;
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

namespace RBAutocadPlugIn
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


        public RibbonPanel rpMain = new RibbonPanel();
        public Autodesk.Windows.RibbonPanelSource rpsConnection = new RibbonPanelSource();
        public RibbonButton rbConnection = new RibbonButton();


        public RibbonPanel rpFileOperations = new RibbonPanel();
        public RibbonPanelSource rpsFileOperations = new RibbonPanelSource();

        public RibbonButton rbBrowseDrawing = new RibbonButton();  

        public RibbonPanelSource rpsSave = new RibbonPanelSource();
        public RibbonPanel rpSave = new RibbonPanel();

    
        public RibbonButton rbLockUnlock = new RibbonButton();
        public RibbonButton rbSave = new RibbonButton();
        public RibbonButton rbSaveAS = new RibbonButton();

        public RibbonPanel rpHelpnAbout = new RibbonPanel();
        public RibbonPanelSource rpsHelpnAbout = new RibbonPanelSource();

        public RibbonButton rbHelp = new RibbonButton();
        public RibbonButton rbAbout = new RibbonButton();

        public RibbonPanel rpSetting = new RibbonPanel();
        public RibbonPanelSource rpsSetting = new RibbonPanelSource();

        public RibbonButton rbSetting = new RibbonButton();
  
        public RibbonButton rbDrawingInfo = new RibbonButton();

      
        public RibbonButton rbRefresh = new RibbonButton();

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
                                    clsRefresh objrfs = new clsRefresh();
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

                System.Data.DataTable dtDrawing = Helper.cadManager.GetDrawingAttributes(Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database.Filename);

                if(dtDrawing !=null)
                {
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
                rpsConnection = new RibbonPanelSource();
                rpMain = new RibbonPanel();

                rpFileOperations = new RibbonPanel();
                rpsFileOperations = new RibbonPanelSource(); 

                rpsSave = new RibbonPanelSource();
                rpSave = new RibbonPanel();

                rpHelpnAbout = new RibbonPanel();
                rpsHelpnAbout = new RibbonPanelSource();

                rpSetting = new RibbonPanel();
                rpsSetting = new RibbonPanelSource(); 

                rpCurrentDI = new RibbonPanel();
                rpsCurrentDI = new RibbonPanelSource();







                //if (!RedBracketConnector.Helper.IsEventAssign)
                {

                    timerVersion.Tick += new System.EventHandler(timerVersion_Tick);
                    timerVersion.Interval = 1000;
                    timerVersion.Start();
                    RBAutocadPlugIn.Helper.IsEventAssign = true;
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
                rpsConnection.Title = "Connection";
                rpMain.Source = rpsConnection;
                Tab.Panels.Add(rpMain);

                rbConnection.Orientation = System.Windows.Controls.Orientation.Vertical;
                rbConnection.Size = RibbonItemSize.Large;


                ConnectionController connController = new ConnectionController();
                if (!connect)
                {
                    rbConnection.Text = "Log-in";
                    rbConnection.ShowText = true;
                    rbConnection.ShowImage = true;
                    rbConnection.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.connect);
                    rbConnection.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.connect);
                    rbConnection.CommandHandler = new clsConnect();

                    Helper.CurrentVersion = "";
                    Helper.LatestVersion = "";
                }
                else
                {
                    rbConnection.Text = "Log-out";
                    rbConnection.ShowText = true;
                    rbConnection.ShowImage = true;
                    rbConnection.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Disconnect);
                    rbConnection.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Disconnect);
                    rbConnection.CommandHandler = new clsDisconnect();
                    browseDEnable = true;
                    createDEnable = true;
                    browseBEnable = true;
                    createBEnable = true;
                    LockEnable = true;
                    UnlockEnable = true;
                    SaveEnable = true;
                    DrawingInfoEnable = true;
                }

                rpsConnection.Items.Add(rbConnection);



                rpsFileOperations.Title = "File Operations";
                rpFileOperations.Source = rpsFileOperations;
                Tab.Panels.Add(rpFileOperations);



                rbBrowseDrawing.Text = "Open File";
                rbBrowseDrawing.ShowText = true;
                rbBrowseDrawing.ShowImage = true;
                rbBrowseDrawing.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Open);
                rbBrowseDrawing.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Open);
                rbBrowseDrawing.Size = RibbonItemSize.Large;
                rbBrowseDrawing.Orientation = System.Windows.Controls.Orientation.Vertical;
                rbBrowseDrawing.CommandHandler = new clsBrowseDrawing();
                rbBrowseDrawing.IsEnabled = browseDEnable;



                RibbonRowPanel pan2row1 = new RibbonRowPanel();
                pan2row1.Items.Add(rbBrowseDrawing);
                rpsFileOperations.Items.Add(pan2row1);




                //Lock, Unlock, and Save







                rbLockUnlock.Text = "Lock & Unlock";
                rbLockUnlock.ShowText = true;
                rbLockUnlock.ShowImage = true;
                rbLockUnlock.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.LockUnlock);
                rbLockUnlock.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.LockUnlock);
                rbLockUnlock.Size = RibbonItemSize.Large;
                rbLockUnlock.Orientation = System.Windows.Controls.Orientation.Vertical;
                rbLockUnlock.CommandHandler = new clsUnlock();
                rbLockUnlock.IsEnabled = UnlockEnable;


                rbSave.Text = "Save to redbracket";
                rbSave.Name = "Save";
                rbSave.ShowText = true;
                rbSave.ShowImage = true;
                rbSave.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Save);
                rbSave.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Save);
                rbSave.Size = RibbonItemSize.Large;
                rbSave.Orientation = System.Windows.Controls.Orientation.Vertical;
                rbSave.CommandHandler = new clsSave();
                rbSave.IsEnabled = SaveEnable;

                rbSaveAS.Text = "Save As New";
                rbSave.Name = "SaveAs";
                rbSaveAS.ShowText = true;
                rbSaveAS.ShowImage = true;
                rbSaveAS.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.SaveAs);
                rbSaveAS.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.SaveAs);
                rbSaveAS.Size = RibbonItemSize.Large;
                rbSaveAS.Orientation = System.Windows.Controls.Orientation.Vertical;
                rbSaveAS.CommandHandler = new clsSave();
                rbSaveAS.IsEnabled = SaveEnable;

                RibbonRowPanel rrpSave = new RibbonRowPanel();
                rrpSave.Items.Add(rbSave);

                RibbonRowPanel rrpSaveAS = new RibbonRowPanel();
                rrpSaveAS.Items.Add(rbSaveAS);
                //rpsSave.Items.Add(rrpSave);

                RibbonRowPanel pan4row1 = new RibbonRowPanel();
                //pan4row1.Items.Add(Btn_Lock);

                pan4row1.Items.Add(rbLockUnlock);
                //panel4Panel.Items.Add(pan4row1);

                rpsFileOperations.Items.Add(rrpSave);
                rpsFileOperations.Items.Add(rrpSaveAS);
                rpsFileOperations.Items.Add(pan4row1);


                //Drawing Info
                //panel7Panel.Title = "Drawing Info";
                //panel7.Source = panel7Panel;
                //Tab.Panels.Add(panel7);


                rbDrawingInfo.Text = "Drawing \n Info";
                rbDrawingInfo.ShowText = true;
                rbDrawingInfo.ShowImage = true;
                rbDrawingInfo.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.DrawingInfo);
                rbDrawingInfo.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.DrawingInfo);
                rbDrawingInfo.Size = RibbonItemSize.Large;
                rbDrawingInfo.Orientation = System.Windows.Controls.Orientation.Vertical;
                rbDrawingInfo.CommandHandler = new clsDrawingInfo();
                rbDrawingInfo.IsEnabled = DrawingInfoEnable;

                RibbonRowPanel pan7row1 = new RibbonRowPanel();
                pan7row1.Items.Add(rbDrawingInfo);
                //panel7Panel.Items.Add(pan7row1);
                rpsFileOperations.Items.Add(pan7row1);


                //Help and About
                rpsHelpnAbout.Title = "Help & About";
                rpHelpnAbout.Source = rpsHelpnAbout;
                Tab.Panels.Add(rpHelpnAbout);


                rbHelp.Text = "Help";
                rbHelp.ShowText = true;
                rbHelp.ShowImage = true;
                rbHelp.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Help);
                rbHelp.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Help);
                rbHelp.Size = RibbonItemSize.Large;
                rbHelp.Orientation = System.Windows.Controls.Orientation.Vertical;
                rbHelp.CommandHandler = new clsHelp();



                rbAbout.Text = "About";
                rbAbout.ShowText = true;
                rbAbout.ShowImage = true;
                rbAbout.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.About);
                rbAbout.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.About);
                rbAbout.Size = RibbonItemSize.Large;
                rbAbout.Orientation = System.Windows.Controls.Orientation.Vertical;
                rbAbout.CommandHandler = new clsAbout();


                RibbonRowPanel pan5row1 = new RibbonRowPanel();
                pan5row1.Items.Add(rbHelp);
                //pan5row1.Items.Add(new RibbonRowBreak());
                pan5row1.Items.Add(rbAbout);
                rpsHelpnAbout.Items.Add(pan5row1);



                //Setting
                rpsSetting.Title = "Settings";
                rpSetting.Source = rpsSetting;
                Tab.Panels.Add(rpSetting);


                rbSetting.Text = "Settings";
                rbSetting.ShowText = true;
                rbSetting.ShowImage = true;
                rbSetting.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Setting);
                rbSetting.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Setting);
                rbSetting.Size = RibbonItemSize.Large;
                rbSetting.Orientation = System.Windows.Controls.Orientation.Vertical;
                rbSetting.CommandHandler = new clsUserSetting();

                RibbonRowPanel pan6row1 = new RibbonRowPanel();
                pan6row1.Items.Add(rbSetting);
                rpsSetting.Items.Add(pan6row1);

                 

                rbRefresh.Text = "Refresh";
                rbRefresh.ShowText = true;
                rbRefresh.ShowImage = true;
                rbRefresh.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Refresh);
                rbRefresh.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Refresh);
                rbRefresh.Size = RibbonItemSize.Large;
                rbRefresh.Orientation = System.Windows.Controls.Orientation.Horizontal;
                rbRefresh.Width = 300;
                rbRefresh.CommandHandler = new clsRefresh();
                rbRefresh.IsEnabled = LockEnable;
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
                rpsCurrentDI.Items.Add(rbRefresh);
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
                    rbSave.IsEnabled = SaveEnable;
                    if (SaveEnable)
                    {
                        rbSaveAS.IsEnabled = true;
                    }
                    else
                    {
                        rbSaveAS.IsEnabled = SaveEnable;
                    }
                }
                else
                {
                    rbSave.IsEnabled = SaveEnable;
                    if (SaveEnable)
                    {
                        if (V1.Trim().Length == 0 || V2.Trim().Length == 0)
                            rbSaveAS.IsEnabled = false;
                        else
                            rbSaveAS.IsEnabled = true;
                    }
                    else
                    {
                        rbSaveAS.IsEnabled = SaveEnable;
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
    public class clsConnect : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            RBAutocadPlugIn.frmLogin mylogin = new RBAutocadPlugIn.frmLogin();
            mylogin.ShowDialog();
        }
    }

    public class clsDisconnect : System.Windows.Input.ICommand
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
                        ShowMessage.ErrorMess(controller.errorString);
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
                ShowMessage.ErrorMess("Logout Fail due to " + ex);
                return;
            }

        }
        #endregion LogOut
    }

    public class clsBrowseDrawing : System.Windows.Input.ICommand
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
                RBAutocadPlugIn.UI_Forms.frmSearch_And_Open browseDrawing = new RBAutocadPlugIn.UI_Forms.frmSearch_And_Open();
                browseDrawing.ShowDialog();
            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess("Exception Occur: " + ex.Message);
                return;
            }
        }
    }


    public class clsSave : System.Windows.Input.ICommand
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
                ShowMessage.ErrorMessUD("Please Save Document on Local Computer" );
                return;
            }
            try
            {
                bool FormOpen = true;
                RBAutocadPlugIn.UI_Forms.frmSave_Active_Drawings objSave = new RBAutocadPlugIn.UI_Forms.frmSave_Active_Drawings(ref FormOpen, IsSaveAs);
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

    public class clsDrawingInfo : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            //DocumentInformationDisplay objDRGInfo = new DocumentInformationDisplay();

            System.Data.DataRow[] dtCurrentData = Helper.cadManager.GetDrawingExternalRefreces().Select("isroot=true");

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
                    ShowMessage.ValMess("Please save this drawing to " + Helper.CompanyName + ".");
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

    public class clsAbout : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
             frmAbout about = new  frmAbout();
            about.ShowDialog();
        }
    }

    public class clsHelp : System.Windows.Input.ICommand
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
                    ShowMessage.ErrorMess(controller.errorString);
                    return;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
            }

        }
    }

    public class clsUserSetting : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            RBAutocadPlugIn.UI_Forms.UserSettings MySettings = RBAutocadPlugIn.UI_Forms.UserSettings.createUserSetting();
            MySettings.ShowDialog();
        }
    }

    public class clsBrowseBlock : System.Windows.Input.ICommand
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





    public class clsUnlock : System.Windows.Input.ICommand
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
                //RBAutocadPlugIn.UI_Forms.UnLock lockForm = new RBAutocadPlugIn.UI_Forms.UnLock();
                RBAutocadPlugIn.UI_Forms.frmLockUnLock lockForm = new RBAutocadPlugIn.UI_Forms.frmLockUnLock();

                lockForm.ShowDialog();


            }

            catch (System.Exception ex)
            {
                //ed.WriteMessage("\nProblem reading/processing \"{0}\": {1}", doc.Name, ex.Message);
            }

        }
    }


    public class clsRefresh : System.Windows.Input.ICommand
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
                ShowMessage.ErrorMessUD("Please Save Document on Local Computer" );
                return;
            }
            try
            {
                //Geting File info form summuryinfo


                Cursor.Current = Cursors.WaitCursor;

                string drawingid = "", updatedon = "",   projectnameOnly = "";
                string   DrawingNO = "",  Rev = "", PreFix = "";



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
                         
                        else if (Convert.ToString(dbsi.Key) == "drawingnumber")
                        {
                            DrawingNO = Convert.ToString(dbsi.Value);
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

                        Helper.cadManager.ChecknCloseOpenedDoc(db.Filename);

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
                            Helper.cadManager.UpdateFileProperties(drawingid, db.Filename);
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

    public class clsImages
    {
        public static BitmapImage getBitmap(Bitmap bmimage)
        {
            try
            {
                MemoryStream msstream = new MemoryStream();
                bmimage.Save(msstream, ImageFormat.Png);
                BitmapImage bmibmp = new BitmapImage();
                bmibmp.BeginInit();
                bmibmp.StreamSource = msstream;
                bmibmp.EndInit();
                return bmibmp;
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return null;

           
        }
    }

}

