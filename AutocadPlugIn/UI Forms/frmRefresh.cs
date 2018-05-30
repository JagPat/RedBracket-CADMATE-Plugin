using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.IO;

namespace AutocadPlugIn.UI_Forms
{
    public partial class frmRefresh : Form
    {
        public frmRefresh()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
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
                var dbsi = ObjectToDictionary(db.SummaryInfo.ToString());

                //Checking if file is in redbracket or not;
                string drawingid = "", updatedon = "", projectname = "";
                try
                {
                    drawingid = dbsi["drawingid"];
                    updatedon = dbsi["modifiedon"];
                    projectname = dbsi["projectname"];
                }
                catch (Exception E)
                {

                }
                RedBracketConnector.RBConnector objRBC = new RedBracketConnector.RBConnector();
                RedBracketConnector.ResultSearchCriteria Drawing = objRBC.GetDrawingInformation(drawingid);

                //if (Drawing == null)
                //{
                //    RedBracketConnector.ShowMessage.InfoMess("File is no longer available in RedBracket.");
                //    return;
                //}
                if (Convert.ToDateTime(Drawing.updatedon) > Convert.ToDateTime(updatedon))
                {
                    if (RedBracketConnector.ShowMessage.InfoYNMess("RedBracket has updated version of this file, do you want to download it ?.") == DialogResult.Yes)
                    {
                        string checkoutPath =RedBracketConnector.Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath").ToString(); 
                        string ProjectName = projectname;
                        if (ProjectName.Trim().Length == 0)
                        {
                            ProjectName = "MyFiles";
                        }
                        checkoutPath = Path.Combine(checkoutPath, ProjectName);
                        if (!Directory.Exists(checkoutPath))
                        {
                            Directory.CreateDirectory(checkoutPath);
                        }
                        System.Collections.Hashtable DrawingProperty = new System.Collections.Hashtable();
                        string filePathName = objRBC.DownloadOpenDocument(drawingid, checkoutPath, ref DrawingProperty);
                        CADController.ICADManager cadManager = new AutoCADManager();
                        cadManager.OpenActiveDocument(filePathName, "View", DrawingProperty);
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

                //Geting file info from redbracket


                // string S = dbsi ;              
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nProblem reading/processing CAD File\"{0}\": {1}", doc.Name, ex.Message);
            }

        }
        public static Dictionary<string, string> ObjectToDictionary(object value)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (value != null)
            {
                string s = Convert.ToString(value);
                string s1 = s.Substring(18);
                s1 = s1.Substring(0, s1.Length - 2);
                s = s1;
                string KeyName = "", KeyValue = "";

                bool isKey = false;
                foreach (Char c in s)
                {
                    if (c == '(')
                    {
                        KeyName = ""; KeyValue = "";
                        isKey = true;
                    }
                    else if (c == ')')
                    {
                        dictionary.Add(KeyName, KeyValue);
                    }
                    else if (c == ',')
                    {
                        isKey = false;
                    }
                    else if (isKey)
                    {
                        KeyName = KeyName + c;
                    }
                    else
                    {
                        KeyValue += c;
                    }

                }

            }
            return dictionary;
        }
    }
}
