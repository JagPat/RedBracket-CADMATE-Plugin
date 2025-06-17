using RBAutocadPlugIn.UI_Forms;

// GRX SDK .NET API namespaces
using Gssoft.Gscad.ApplicationServices;
using Gssoft.Gscad.DatabaseServices;
using Gssoft.Gscad.EditorInput;
using Gssoft.Gscad.Runtime;

// System namespaces
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;

// Command namespaces
using RBAutocadPlugIn.Commands;

namespace RBAutocadPlugIn
{
    public class CADRibbon
    {
        // Status flags
        public static bool IsConnected = false;
        public bool BrowseDrawingEnabled = false;
        public bool CreateDrawingEnabled = false;
        public bool BrowseBlockEnabled = false;
        public bool CreateBlockEnabled = false;
        public bool LockEnabled = false;
        public bool UnlockEnabled = false;
        public bool SaveEnabled = false;
        public bool DrawingInfoEnabled = false;
        
        // Timer for version checking
        public Timer timerVersion = new Timer();
        
        // Version information
        public string CurrentVersion = "";
        public string LatestVersion = "";
        
        // Main toolbar
        private ToolStrip toolStrip;
        
        // ToolStrip buttons
        private ToolStripButton btnConnect;
        private ToolStripButton btnBrowseDrawing;
        private ToolStripButton btnSave;
        private ToolStripButton btnLockUnlock;
        private ToolStripButton btnDrawingInfo;
        private ToolStripButton btnHelp;
        private ToolStripButton btnAbout;
        private ToolStripButton btnUserSetting;
        private ToolStripButton btnRefresh;
        
        // Status strip for version info
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblVersionInfo;
        
        // Reference to the main form (if needed)
        private Form mainForm;

        /// <summary>
        /// Initializes a new instance of the CADRibbon class
        /// </summary>
        public CADRibbon(Form parentForm = null)
        {
            mainForm = parentForm;
            InitializeToolbar();
        }

        /// <summary>
        /// Initializes the toolbar and its controls
        /// </summary>
        private void InitializeToolbar()
        {
            // Create the main toolbar
            toolStrip = new ToolStrip();
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.Dock = DockStyle.Top;
            toolStrip.AutoSize = false;
            toolStrip.Height = 60; // Standard toolbar height
            toolStrip.Padding = new Padding(2);
            toolStrip.RenderMode = ToolStripRenderMode.System;
            
            // Set a default font that works well with the application
            toolStrip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            
            // Set up the toolbar appearance
            toolStrip.BackColor = SystemColors.Control;
            toolStrip.Renderer = new ToolStripProfessionalRenderer();
            
            // Create buttons with proper image scaling
            // ImageScaling is set on individual ToolStripItems, not on the ToolStrip itself
            CreateToolbarButtons();

            // Create status strip for version info
            statusStrip = new StatusStrip();
            lblVersionInfo = new ToolStripStatusLabel();
            statusStrip.Items.Add(lblVersionInfo);

            // Add controls to the main form if provided
            if (mainForm != null)
            {
                mainForm.Controls.Add(toolStrip);
                mainForm.Controls.Add(statusStrip);
                statusStrip.Dock = DockStyle.Bottom;
            }
        }

        /// <summary>
        /// Creates and configures the toolbar buttons
        /// </summary>
        private void CreateToolbarButtons()
        {
            // Connect button
            btnConnect = CreateButton("Connect", "Connect to the system", "connect.png");
            btnConnect.Click += (s, e) => new ConnectCommand().Execute(null);
            toolStrip.Items.Add(btnConnect);

            // Browse Drawing button
            btnBrowseDrawing = CreateButton("Browse Drawing", "Browse for drawing", "browse.png");
            btnBrowseDrawing.Click += (s, e) => new BrowseDrawingCommand().Execute(null);
            toolStrip.Items.Add(btnBrowseDrawing);

            // Save button
            btnSave = CreateButton("Save", "Save the current drawing", "save.png");
            btnSave.Click += (s, e) => new SaveCommand().Execute(null);
            toolStrip.Items.Add(btnSave);

            // Lock/Unlock button
            btnLockUnlock = CreateButton("Lock", "Lock/Unlock drawing", "lock.png");
            btnLockUnlock.Click += (s, e) => 
            {
                if (LockEnabled)
                    new LockCommand().Execute(null);
                else
                    new UnlockCommand().Execute(null);
            };
            toolStrip.Items.Add(btnLockUnlock);

            // Drawing Info button
            btnDrawingInfo = CreateButton("Drawing Info", "Show drawing information", "info.png");
            btnDrawingInfo.Click += (s, e) => new DrawingInfoCommand().Execute(null);
            toolStrip.Items.Add(btnDrawingInfo);

            // Add a separator
            toolStrip.Items.Add(new ToolStripSeparator());

            // Help button
            btnHelp = CreateButton("Help", "Show help", "help.png");
            btnHelp.Click += (s, e) => new HelpCommand().Execute(null);
            toolStrip.Items.Add(btnHelp);

            // About button
            btnAbout = CreateButton("About", "About this application", "about.png");
            btnAbout.Click += (s, e) => new AboutCommand().Execute(null);
            toolStrip.Items.Add(btnAbout);

            // User Settings button
            btnUserSetting = CreateButton("Settings", "User settings", "settings.png");
            btnUserSetting.Click += (s, e) => new UserSettingsCommand().Execute(null);
            toolStrip.Items.Add(btnUserSetting);

            // Refresh button
            btnRefresh = CreateButton("Refresh", "Refresh the view", "refresh.png");
            btnRefresh.Click += (s, e) => new RefreshCommand().Execute(null);
            toolStrip.Items.Add(btnRefresh);
        }

        /// <summary>
        /// Helper method to create a standardized toolbar button
        /// </summary>
        private ToolStripButton CreateButton(string text, string tooltip, string imageName)
        {
            var button = new ToolStripButton();
            button.Text = text;
            button.ToolTipText = tooltip;
            button.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
            button.TextImageRelation = TextImageRelation.ImageAboveText;
            button.AutoSize = false;
            button.Height = 50;
            button.Width = 70;
            button.ImageScaling = ToolStripItemImageScaling.None;
            
            // Try to load the image if it exists
            try
            {
                // Look for the image in the application directory
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", imageName);
                if (File.Exists(imagePath))
                {
                    // Explicitly use System.Drawing.Image to avoid ambiguity
                    button.Image = System.Drawing.Image.FromFile(imagePath);
                }
                else
                {
                    // Use a default system image if the file doesn't exist
                    button.Image = SystemIcons.Information.ToBitmap();
                }
            }
            catch
            {
                // If there's an error loading the image, just continue without it
                button.Image = null;
            }
            
            return button;
        }

        /// <summary>
        /// Updates the UI state based on the current connection status
        /// </summary>
        public void UpdateUIState()
        {
            // Update button states based on connection status
            btnBrowseDrawing.Enabled = IsConnected && BrowseDrawingEnabled;
            btnSave.Enabled = IsConnected && SaveEnabled;
            
            // Update lock/unlock button
            if (LockEnabled)
            {
                btnLockUnlock.Text = "Lock";
                btnLockUnlock.ToolTipText = "Lock the current drawing";
                // Update image if needed
            }
            else if (UnlockEnabled)
            {
                btnLockUnlock.Text = "Unlock";
                btnLockUnlock.ToolTipText = "Unlock the current drawing";
                // Update image if needed
            }
            
            btnLockUnlock.Enabled = IsConnected && (LockEnabled || UnlockEnabled);
            btnDrawingInfo.Enabled = IsConnected && DrawingInfoEnabled;
            
            // Always enable these buttons
            btnHelp.Enabled = true;
            btnAbout.Enabled = true;
            btnUserSetting.Enabled = true;
            btnRefresh.Enabled = true;
            
            // Update version info in status bar
            UpdateVersionInfo();
        }

        /// <summary>
        /// Updates the version information in the status bar
        /// </summary>
        private void UpdateVersionInfo()
        {
            if (!string.IsNullOrEmpty(CurrentVersion))
            {
                string versionText = $"Version: {CurrentVersion}";
                if (!string.IsNullOrEmpty(LatestVersion) && CurrentVersion != LatestVersion)
                {
                    versionText += $" (New version {LatestVersion} available)";
                }
                lblVersionInfo.Text = versionText;
            }
        }

        /// <summary>
        /// Shows a message to the user
        /// </summary>
        public void ShowMessage(string message, string caption, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            if (mainForm != null && !mainForm.IsDisposed && mainForm.InvokeRequired)
            {
                mainForm.Invoke(new Action(() => MessageBox.Show(mainForm, message, caption, MessageBoxButtons.OK, icon)));
            }
            else
            {
                MessageBox.Show(mainForm, message, caption, MessageBoxButtons.OK, icon);
            }
        }

        /// <summary>
        /// Shows an error message to the user
        /// </summary>
        public void ShowError(string message, string caption = "Error")
        {
            ShowMessage(message, caption, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Shows a warning message to the user
        /// </summary>
        public void ShowWarning(string message, string caption = "Warning")
        {
            ShowMessage(message, caption, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Shows a confirmation dialog to the user
        /// </summary>
        public DialogResult ShowConfirm(string message, string caption, MessageBoxButtons buttons = MessageBoxButtons.YesNo)
        {
            return MessageBox.Show(message, caption, buttons, MessageBoxIcon.Question);
        }

        /// <summary>
        /// Disposes of resources used by the CADRibbon
        /// </summary>
        public void Dispose()
        {
            try
            {
                // Stop and dispose the version check timer
                if (timerVersion != null)
                {
                    timerVersion.Stop();
                    timerVersion.Dispose();
                }

                // Dispose toolbar controls
                if (toolStrip != null)
                {
                    toolStrip.Items.Clear();
                    toolStrip.Dispose();
                }

                if (statusStrip != null)
                {
                    statusStrip.Items.Clear();
                    statusStrip.Dispose();
                }

                // Dispose button images
                DisposeButtonImages();
            }
            catch (Exception ex)
            {
                // Log error if logging is available
                System.Diagnostics.Debug.WriteLine($"Error disposing CADRibbon: {ex.Message}");
            }
        }

        /// <summary>
        /// Disposes of button images
        /// </summary>
        private void DisposeButtonImages()
        {
            // Dispose of any button images that need explicit cleanup
            var buttons = new[] { btnConnect, btnBrowseDrawing, btnSave, btnLockUnlock, 
                                 btnDrawingInfo, btnHelp, btnAbout, btnUserSetting, btnRefresh };
            
            foreach (var button in buttons)
            {
                if (button?.Image != null)
                {
                    button.Image.Dispose();
                }
            }
        }

        /// <summary>
        /// Updates the connection status and UI
        /// </summary>
        public void SetConnectionStatus(bool isConnected)
        {
            IsConnected = isConnected;
            btnConnect.Text = isConnected ? "Disconnect" : "Connect";
            btnConnect.ToolTipText = isConnected ? "Disconnect from the system" : "Connect to the system";
            
            // Update UI based on new connection state
            UpdateUIState();
        }

        /// <summary>
        /// Updates the lock status and UI
        /// </summary>
        public void SetLockStatus(bool isLocked, string lockedByUser = null)
        {
            LockEnabled = !isLocked;
            UnlockEnabled = isLocked;
            
            // Update lock/unlock button
            if (isLocked)
            {
                btnLockUnlock.Text = "Unlock";
                btnLockUnlock.ToolTipText = $"Unlock the current drawing (Locked by {lockedByUser ?? "another user"})";
                // Update image if needed
            }
            else
            {
                btnLockUnlock.Text = "Lock";
                btnLockUnlock.ToolTipText = "Lock the current drawing";
                // Update image if needed
            }
            
            btnLockUnlock.Enabled = IsConnected && (LockEnabled || UnlockEnabled);
        }

        /// <summary>
        /// Shows the main form (if available)
        /// </summary>
        public void Show()
        {
            if (mainForm != null && !mainForm.IsDisposed)
            {
                mainForm.Show();
                mainForm.BringToFront();
            }
        }

        /// <summary>
        /// Hides the main form (if available)
        /// </summary>
        public void Hide()
        {
            if (mainForm != null && !mainForm.IsDisposed)
            {
                mainForm.Hide();
            }
        }

        /// <summary>
        /// Command to initialize the ribbon UI
        /// </summary>
        [CommandMethod("RBRibbon")]
        public void RBRibbon()
        {
            try
            {
                // This method is kept for backward compatibility
                // The ribbon is now initialized in the constructor
                UpdateUIState();
                
                // Show a message to indicate the command was executed
                var doc = Gssoft.Gscad.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                if (doc != null)
                {
                    doc.Editor.WriteMessage("\nRedBracket CAD Integration is ready. Use the toolbar for commands.\n");
                }
            }
            catch (System.Exception ex)
            {
                ShowError($"Failed to initialize CAD integration: {ex.Message}");
                Debug.WriteLine($"RBRibbon error: {ex}");
            }
        }

        /// <summary>
        /// Command to show the about dialog
        /// </summary>
        [CommandMethod("RBAbout")]
        public void RBAbout()
        {
            try
            {
                new AboutCommand().Execute(null);
            }
            catch (Exception ex)
            {
                ShowError($"Failed to show about dialog: {ex.Message}");
            }
        }

        /// <summary>
        /// Command to show the help dialog
        /// </summary>
        [CommandMethod("RBHelp")]
        public void RBHelp()
        {
            try
            {
                new HelpCommand().Execute(null);
            }
            catch (Exception ex)
            {
                ShowError($"Failed to show help: {ex.Message}");
            }
        }

        /// <summary>
        /// Command to show user settings
        /// </summary>
        [CommandMethod("RBUserSettings")]
        public void RBUserSettings()
        {
            try
            {
                new UserSettingsCommand().Execute(null);
            }
            catch (Exception ex)
            {
                ShowError($"Failed to show user settings: {ex.Message}");
            }
        }
        /// </summary>
        public void UpdateConnectionStatus(bool isConnected)
        {
            IsConnected = isConnected;
            
            if (btnConnect != null)
            {
                btnConnect.Text = isConnected ? "Disconnect" : "Connect";
                btnConnect.ToolTipText = isConnected ? "Disconnect from the system" : "Connect to the system";
            }
            
            // Update other UI elements based on connection status
            UpdateUIState();
        }

        /// <summary>
        /// Updates the state of UI elements based on the current application state
        /// </summary>
        public void UpdateUIState()
        {
            if (btnBrowseDrawing != null)
                btnBrowseDrawing.Enabled = IsConnected && BrowseDrawingEnabled;
                
            if (btnSave != null)
                btnSave.Enabled = IsConnected && SaveEnabled;
                
            if (btnLockUnlock != null)
            {
                btnLockUnlock.Enabled = IsConnected && (LockEnabled || UnlockEnabled);
                btnLockUnlock.Text = LockEnabled ? "Lock" : "Unlock";
                btnLockUnlock.ToolTipText = LockEnabled ? "Lock the drawing" : "Unlock the drawing";
            }
            
            if (btnDrawingInfo != null)
                btnDrawingInfo.Enabled = IsConnected && DrawingInfoEnabled;
        }

        /// <summary>
        /// Updates the version information in the status bar
        /// </summary>
        public void UpdateVersionInfo(string currentVersion, string latestVersion)
        {
            CurrentVersion = currentVersion;
            LatestVersion = latestVersion;
            
            if (lblVersionInfo != null)
            {
                lblVersionInfo.Text = $"Current Version: {currentVersion} | Latest Version: {latestVersion}";
            }
        }

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
                //Following code will commented as per instruction of Mahesh Saraswati on date 23-02-2019
                //because of System hangs while we fetch file info every time user change tab
                // right now its not commented 
                //this event requires to be changed as one time per file.
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

                if (dtDrawing != null)
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
                    rbConnection.ToolTip = "Connect to server";
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
                    rbConnection.ToolTip = "Disconnect from server";
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
                rbBrowseDrawing.ToolTip = "Search & Open existing file from server";
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
                rbLockUnlock.ToolTip = "Lock/Unlock the current file on the server";
                rbLockUnlock.ShowText = true;
                rbLockUnlock.ShowImage = true;
                rbLockUnlock.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.LockUnlock);
                rbLockUnlock.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.LockUnlock);
                rbLockUnlock.Size = RibbonItemSize.Large;
                rbLockUnlock.Orientation = System.Windows.Controls.Orientation.Vertical;
                rbLockUnlock.CommandHandler = new clsUnlock();
                rbLockUnlock.IsEnabled = UnlockEnable;


                rbSave.Text = "Save to redbracket";
                rbSave.ToolTip = "Save the current drawing to server";
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
                rbSaveAS.ToolTip = "Save a copy of current drawing as a new file";
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
                rbDrawingInfo.ToolTip = "Shows information of current version and latest version on server";
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
                rbHelp.ToolTip = "Display help document";
                rbHelp.ShowText = true;
                rbHelp.ShowImage = true;
                rbHelp.Image = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Help);
                rbHelp.LargeImage = clsImages.getBitmap(RBAutocadPlugIn.Properties.Resources.Help);
                rbHelp.Size = RibbonItemSize.Large;
                rbHelp.Orientation = System.Windows.Controls.Orientation.Vertical;
                rbHelp.CommandHandler = new clsHelp();



                rbAbout.Text = "About";
                rbAbout.ToolTip = "Information about redbracket and user license";
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
                rbSetting.ToolTip = "Sets username, server URL and check-out drive";
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
                rbRefresh.ToolTip = "Checks & Updates file and layout information from the server";
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
                ShowMessage.ErrorMessUD("Please Save Document on Local Computer");
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
            frmAbout about = new frmAbout();
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
            Helper.CheckFileInfoFlag = false;
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            //PromptStringOptions pso = new PromptStringOptions("\nEnter path to root drawing file: ");
            //pso.AllowSpaces = true;
            // PromptResult pr = ed.GetString(pso);

            if (!File.Exists(doc.Name))
            {
                ShowMessage.ErrorMessUD("Please Save Document on Local Computer"); Helper.CheckFileInfoFlag = true;
                return;
            }
            try
            {
                //Geting File info form summuryinfo


                Cursor.Current = Cursors.WaitCursor;

                string drawingid = "", updatedon = "", projectnameOnly = "";
                string DrawingNO = "", Rev = "", PreFix = "";



                try
                {
                    
                    System.Data.DataTable dtDrawing = Helper.cadManager.GetDrawingAttributes(db.Filename);
                    if (dtDrawing != null)
                    {
                        if (dtDrawing.Rows.Count > 0)
                        {
                            drawingid = Convert.ToString(dtDrawing.Rows[0]["DrawingId"]);
                            Rev = Convert.ToString(dtDrawing.Rows[0]["revision"]);
                            DrawingNO = Convert.ToString(dtDrawing.Rows[0]["DrawingNumber"]);
                            updatedon = Convert.ToString(dtDrawing.Rows[0]["modifiedon"]);
                            projectnameOnly = Convert.ToString(dtDrawing.Rows[0]["projectid"]);
                            PreFix = Convert.ToString(dtDrawing.Rows[0]["prefix"]);
                        }
                    }

                     


                }
                catch (System.Exception E)
                {

                }
                if (drawingid.Trim().Length == 0 || updatedon.Trim().Length == 0)
                {
                    Cursor.Current = Cursors.Default; Helper.CheckFileInfoFlag = true;
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
                        string checkoutPath = "";
                      
                       
                        checkoutPath= Helper.GetCheckoutDirectory(projectnameOnly);
                        Helper.cadManager.ChecknCloseOpenedDoc(db.Filename);

                        DownloadOpenDocument(Drawing.id, checkoutPath);

                    }
                    else
                    {
                        Helper.CheckFileInfoFlag = true;
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
            Helper.CheckFileInfoFlag = true;
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

                Helper.DownloadFile(fileId, "true");

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

