using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;
using System.Management;
using AdvancedDataGridView;

namespace AutocadPlugIn
{
    public static class Helper
    {
        public static string UserName = "";//Like X@ymail.com
        public static string UserFullName = "";// Like X User
        public static string FirstName = "";// Like X
        public static string LastName = "";// Like User
        public static string UserID = ""; // Like 1

        public static string CompanyName = "redbracket";
        public static bool IsEventAssign = false;
        public static bool IsSavePassword = false;
        public static decimal FileLayoutNameLength = 255;

        public static string CurrentVersion = "";
        public static string LatestVersion = "";

        public static bool IsRenameChild = true;
        public static RBConnector objRBC = new RBConnector();
        public static AutoCADManager cadManager = new AutoCADManager();
        public static Color clrChildPopupBorderColor = Color.FromArgb(130, 130, 156);
        public static Color clrParentPopupBorderColor = Color.FromArgb(46, 49, 50);
        public static Color clrDiffHighlighColor = Color.FromArgb(255, 180, 180);
        public static string DateFormat = "dd-MMM-yy";
        public static string DateTimeFormat = "dd-MMM-yy hh:mm:ss tt";
        public static string TimeFormat = "hh:mm:ss tt";
        public static bool CheckFileInfoFlag = true;
        public static bool TitleBlockFlag = false;
        public static bool TitleBlockVisibilityFlag = false;
        public static bool IsUpdateLayoutInfo = true;
        public static frmProgressBar objfrmPB = new frmProgressBar();
        public static Color FormBGColor = Color.Azure;

        public static List<string> InvalidCharacter = new List<string>() { "?", "`", "^", "{", "}", "|", @"\", @"&", "/", "'", "#" };
        public static string InvalidCharacterString = "";
        public static void GetProgressBar(int MaxValue, string Title = null, string Status = null)
        {
            try
            {
                objfrmPB = new frmProgressBar();
                objfrmPB.lblTitle.Text = Title;
                objfrmPB.lblStatus.Text = Status;
                objfrmPB.pbProcess.Minimum = objfrmPB.pbProcess.Value = 0;
                objfrmPB.pbProcess.Maximum = MaxValue;
                objfrmPB.TopMost = true;
                objfrmPB.Show();
                objfrmPB.pbProcess.Refresh();
                objfrmPB.Refresh();
                IsPBActive = true;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

        }
        public static string FirstStatusName = "";
        public static string FirstStatusID = "";
        public static bool IsPBHiden = false;
        public static bool IsPBActive = false;
        public static DataTable dtFileType = new DataTable();
        public static DataTable dtFileStatus = new DataTable();
        public static DataTable dtProjectDetail = new DataTable();
        public static List<string> DrawingAttributes = new List<string>();
        public static List<string> LayoutAttributes = new List<string>();
        public static List<string> TestingAttributes = new List<string>();

        public static void IncrementProgressBar(int IntcrementValue = 1, string Status = null)
        {
            try
            {
                IsPBActive = true;
                objfrmPB.TopMost = true;
                objfrmPB.pbProcess.Increment(IntcrementValue);
                objfrmPB.lblStatus.Text = Status;
                objfrmPB.pbProcess.Refresh();
                objfrmPB.Refresh();
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

        }
        public static void CloseProgressBar()
        {
            try
            {
                IsPBActive = false;
                objfrmPB.TopMost = true;
                objfrmPB.pbProcess.Value = objfrmPB.pbProcess.Maximum;
                objfrmPB.lblStatus.Text = string.Empty;
                objfrmPB.Close();
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

        }
        public static void HideProgressBar()
        {
            try
            {
                if (IsPBActive)
                {
                    objfrmPB.Hide();
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

        }
        public static void ShowProgressBar()
        {
            try
            {
                if (IsPBActive)
                {
                    objfrmPB.Show();
                    objfrmPB.pbProcess.Refresh();
                    objfrmPB.Refresh();
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

        }
        public static object GetValueRegistry(string subKeyName, string keyName)
        {
            // Read the keys from the user registry and load it to the UI.
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software", true);

            registryKey = registryKey.OpenSubKey("RedBracketConnector", true);

            if (registryKey != null)
            {
                registryKey = registryKey.OpenSubKey(subKeyName, true);

                if (registryKey != null)
                {
                    return registryKey.GetValue(keyName);
                }
            }

            return null;
        }

        public static bool SetValueRegistry(string subKeyName, string keyName, string Value)
        {
            // Read the keys from the user registry and load it to the UI.
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software", true);

            registryKey = registryKey.OpenSubKey("RedBracketConnector", true);

            if (registryKey != null)
            {
                registryKey = registryKey.OpenSubKey(subKeyName, true);

                if (registryKey != null)
                {
                    registryKey.SetValue(keyName, Value);
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Passing True for IsSelect Means that you want first item of Combobox as ---Select---
        /// false means ---All---
        /// </summary>
        /// <param name="IsSelect"></param>
        /// <returns></returns>
        public static void FIllCMB(ComboBox cmb, DataTable dt, string DisplayMember, string ValueMenmber, bool IsSelect, bool IsSortByDisplayMember = true)
        {
            try
            {
                String Text = "All";
                if (IsSelect)
                    Text = "---Select---";
                dt = Helper.AddFirstRowToTable(dt, Text, DisplayMember, IsSortByDisplayMember);

                cmb.DataSource = dt;
                cmb.DisplayMember = DisplayMember;
                cmb.ValueMember = ValueMenmber;

                if (dt.Rows.Count > 0)
                    cmb.SelectedIndex = 0;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }
        public static void FIllCMB(DataGridViewComboBoxColumn cmb, DataTable dt, string DisplayMember, string ValueMenmber, bool IsSelect, string FirstRowText = null, bool IsSortByDisplayMember = true)
        {
            try
            {
                String Text = "All";
                if (FirstRowText == null)
                {
                    if (IsSelect)
                        Text = "---Select---";
                }
                else
                {
                    Text = FirstRowText;
                }

                dt = Helper.AddFirstRowToTable(dt, Text, DisplayMember, IsSortByDisplayMember);

                cmb.DataSource = dt;
                cmb.DisplayMember = DisplayMember;
                cmb.ValueMember = ValueMenmber;


                //if (dt.Rows.Count > 0)
                //    cmb.SelectedIndex = 0;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        public static DataTable AddFirstRowToTable(DataTable dt, string Text, string DisplayMember, bool IsSortByDisplayMember = true)
        {
            try
            {

                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("id");
                    dt.Columns.Add(DisplayMember);

                }
                if (!dt.Columns.Contains("id"))
                {
                    dt.Columns.Add("id");
                }
                if (!dt.Columns.Contains(DisplayMember))
                {
                    dt.Columns.Add(DisplayMember);
                }
                dt.Columns.Add("Rank");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Rank"] = 2;
                }
                DataRow dr = dt.NewRow();
                dr["id"] = -1;
                //dr[DisplayMember] = "---" + Text + "---";
                dr[DisplayMember] = Text;
                dr["Rank"] = 1;

                dt.Rows.Add(dr);

                DataView dv = dt.DefaultView;
                if (IsSortByDisplayMember)
                    dv.Sort = "Rank," + DisplayMember;
                else
                    dv.Sort = "Rank";
                dt = dv.ToTable();

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dt;
        }


        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public static string FindValueInCMB(DataTable dt, string ValueMember, string Value, string DisplayMember)
        {
            string RValue = "";
            try
            {
                if(dt.Columns.Contains(ValueMember))
                {
                    DataRow[] dr = dt.Select(ValueMember + " = '" + Value + "'");

                    if (dr.Length > 0)
                    {
                        
                        if (dr.CopyToDataTable().Columns.Contains(DisplayMember))
                            RValue = Convert.ToString(dr[0][DisplayMember]);
                    }
                }
                
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

            return RValue;
        }
        public static string FindIDInCMB(DataTable dt, string ValueMember, string Value, string DisplayMember)
        {
            string RValue = "";
            try
            {
                DataRow[] dr = dt.Select(DisplayMember + " = '" + Value + "'");

                if (dr.Length > 0)
                {
                    RValue = Convert.ToString(dr[0][ValueMember]);
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

            return RValue;
        }

        /// <summary>
        /// Gets the file bytes array to save to the server.
        /// </summary>
        /// <param name="filePath">Path of the file to convert to bytes array.</param>
        /// <returns>Array of file bytes.</returns>
        public static byte[] GetFileDataBytes(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
        public static Hashtable Table2HashTable(DataTable dt, int i)
        {
            Hashtable DrawingProperty = new Hashtable();

            try
            {
                if (dt.Rows.Count > i)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        DrawingProperty.Add(dt.Columns[j].ColumnName, Convert.ToString(dt.Rows[i][j]));
                    }

                    //DrawingProperty.Add("DrawingId", dt.Rows[i]["DrawingId"]);
                    //DrawingProperty.Add("DrawingName", dt.Rows[i]["DrawingName"]);
                    //DrawingProperty.Add("Classification", dt.Rows[i]["Classification"]);
                    //DrawingProperty.Add("DrawingNumber", dt.Rows[i]["DrawingNumber"]);
                    //DrawingProperty.Add("DrawingState", dt.Rows[i]["DrawingState"]);
                    //DrawingProperty.Add("Revision", dt.Rows[i]["Revision"]);
                    //DrawingProperty.Add("Generation", dt.Rows[i]["Generation"]);
                    //DrawingProperty.Add("Type", dt.Rows[i]["Type"]);
                    //DrawingProperty.Add("filepath", dt.Rows[i]["filepath"]);
                    //DrawingProperty.Add("isroot", dt.Rows[i]["isroot"]);
                    //DrawingProperty.Add("ProjectName", dt.Rows[i]["ProjectName"]);
                    //DrawingProperty.Add("ProjectId", dt.Rows[i]["ProjectId"]);
                    //DrawingProperty.Add("CreatedOn", dt.Rows[i]["createdon"]);
                    //DrawingProperty.Add("CreatedBy", dt.Rows[i]["createdby"]);
                    //DrawingProperty.Add("ModifiedOn", dt.Rows[i]["modifiedon"]);
                    //DrawingProperty.Add("ModifiedBy", dt.Rows[i]["modifiedby"]);
                    //DrawingProperty.Add("sourceid", dt.Rows[i]["sourceid"]);
                    //DrawingProperty.Add("Layouts", dt.Rows[i]["Layouts"]);

                    //DrawingProperty.Add("canDelete", dt.Rows[i]["canDelete"]);
                    //DrawingProperty.Add("isowner", dt.Rows[i]["isowner"]);
                    //DrawingProperty.Add("hasViewPermission", dt.Rows[i]["hasViewPermission"]);
                    //DrawingProperty.Add("isActFileLatest", dt.Rows[i]["isActFileLatest"]);
                    //DrawingProperty.Add("isEditable", dt.Rows[i]["isEditable"]);
                    //DrawingProperty.Add("canEditStatus", dt.Rows[i]["canEditStatus"]);
                    //DrawingProperty.Add("hasStatusClosed", dt.Rows[i]["hasStatusClosed"]);
                    //DrawingProperty.Add("isletest", dt.Rows[i]["isletest"]);
                    //DrawingProperty.Add("projectno", dt.Rows[i]["projectno"]);
                    //DrawingProperty.Add("prefix", dt.Rows[i]["prefix"]);
                    //DrawingProperty.Add("filetypeid", dt.Rows[i]["Classification"]);
                    //DrawingProperty.Add("layoutinfo", dt.Rows[i]["layoutinfo"]);
                    //DrawingProperty.Add("oldprefix", dt.Rows[i]["oldprefix"]);

                    string LayoutInfo1 = Convert.ToString(dt.Rows[i]["layoutinfo"]).Trim();
                    if (LayoutInfo1.Length > 0)
                    {
                        List<LayoutInfo> objLI = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LayoutInfo>>(LayoutInfo1);
                        //int Count = 1;
                        foreach (LayoutInfo objLI1 in objLI)
                        {
                            int Count = Convert.ToInt16(objLI1.fileNo.Contains("_") ? objLI1.fileNo.Substring(0, objLI1.fileNo.IndexOf("_")) : "0");
                            DrawingProperty.Add("Layout_" + Count + "_Name", objLI1.name);
                            DrawingProperty.Add("Layout_" + Count + "_Number", objLI1.fileNo);
                            DrawingProperty.Add("Layout_" + Count + "_Type", objLI1.typename);
                            DrawingProperty.Add("Layout_" + Count + "_Status", objLI1.statusname);
                            DrawingProperty.Add("Layout_" + Count + "_VersionNote", objLI1.description);
                            DrawingProperty.Add("Layout_" + Count + "_CreatedBy", objLI1.createdby);
                            DrawingProperty.Add("Layout_" + Count + "_CreatedOn", objLI1.created0n);
                            DrawingProperty.Add("Layout_" + Count + "_UpdatedBy", objLI1.updatedby);
                            DrawingProperty.Add("Layout_" + Count + "_UpdatedOn", objLI1.updatedon);


                            // Count++;
                        }
                    }
                }
                else
                {
                    ShowMessage.ErrorMessUD("Specified index not found.");
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

            return DrawingProperty;

        }
        public static LayoutInfo FindLayoutDetail(Hashtable ht, string LayoutName)
        {
            LayoutInfo objLayoutInfo = new LayoutInfo();
            try
            {
                string LayoutInfo1 = Convert.ToString(ht["layoutinfo"]);
                if (LayoutInfo1.Trim().Length > 0)
                {
                    List<LayoutInfo> objLI = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LayoutInfo>>(LayoutInfo1);
                    if (objLI != null)
                    {
                        foreach (LayoutInfo objLI1 in objLI)
                        {
                            if (objLI1.name == LayoutName)
                            {
                                objLayoutInfo = objLI1;
                                break;
                            }
                        }
                    }

                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return objLayoutInfo;
        }


        public static DataTable HashTable2Table(Hashtable htDrawingProperty)
        {
            DataTable dtDrawingProperty = new DataTable();

            try
            {
                dtDrawingProperty.Rows.Add();
                DataColumn dc;
                foreach (DictionaryEntry pair in htDrawingProperty)
                {
                    dc = new DataColumn();
                    dc.ColumnName = Convert.ToString(pair.Key);
                    dtDrawingProperty.Columns.Add(dc);
                    dtDrawingProperty.Rows[0][dc.ColumnName] = pair.Value;
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

            return dtDrawingProperty;

        }

        public static DataTable SortTable(DataTable dt, string ColumnName)
        {

            try
            {
                DataView dv = dt.Copy().DefaultView;
                dv.Sort = ColumnName + " ASC";
                dt = dv.ToTable();
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dt;
        }
        public static List<LayoutInfo> SortLayoutInfo(List<LayoutInfo> fileLayout)
        {
            try
            {
                #region Sorting
                DataTable dtLayoutInfo = new DataTable();
                try
                {



                    dtLayoutInfo.Columns.Add("LayoutNo");
                    dtLayoutInfo.Columns.Add("FileLayoutName");
                    dtLayoutInfo.Columns.Add("LayoutID");
                    dtLayoutInfo.Columns.Add("LayoutType");
                    dtLayoutInfo.Columns.Add("LayoutStatus");
                    dtLayoutInfo.Columns.Add("Version");
                    dtLayoutInfo.Columns.Add("Description");
                    dtLayoutInfo.Columns.Add("TypeID");
                    dtLayoutInfo.Columns.Add("StatusID");
                    dtLayoutInfo.Columns.Add("ACLayoutID");

                    dtLayoutInfo.Columns.Add("islatest");
                    dtLayoutInfo.Columns.Add("Seq", typeof(decimal));


                    dtLayoutInfo.Columns.Add("CreatedBy");
                    dtLayoutInfo.Columns.Add("CreatedOn");
                    dtLayoutInfo.Columns.Add("UpdatedBy");
                    dtLayoutInfo.Columns.Add("UpdatedOn");
                    dtLayoutInfo.Columns.Add("CoreStatus");
                    if (fileLayout != null)
                    {
                        foreach (LayoutInfo obj in fileLayout)
                        {
                            DataRow dr = dtLayoutInfo.NewRow();

                            dr["LayoutID"] = obj.id;

                            dr["Description"] = obj.description;
                            dr["islatest"] = obj.isletest;
                            dr["FileLayoutName"] = obj.name;
                            dr["LayoutNo"] = obj.fileNo;
                            dr["StatusID"] = obj.statusId == null || obj.statusId == string.Empty ? obj.status == null ? string.Empty : Convert.ToString(obj.status.id) : obj.statusId;
                            dr["LayoutStatus"] = obj.statusname == null || obj.statusname == string.Empty ? obj.status == null ? string.Empty : Convert.ToString(obj.status.statusname) : obj.statusname;
                            dr["TypeID"] = obj.typeId == null || obj.typeId == string.Empty ? obj.type == null ? string.Empty : Convert.ToString(obj.type.id) : obj.typeId;
                            dr["Version"] = obj.versionno;
                            dr["ACLayoutID"] = obj.layoutId == null ? string.Empty : obj.layoutId;
                            dr["LayoutType"] = obj.typename == null || obj.typename == string.Empty ? obj.type == null ? string.Empty : Convert.ToString(obj.type.name) : obj.typename;
                            dr["Seq"] = obj.fileNo.Contains("_") ? obj.fileNo.Substring(0, obj.fileNo.IndexOf("_")) : "0";


                            dr["CreatedBy"] = obj.createdby;
                            dr["CreatedOn"] = obj.created0n;
                            dr["UpdatedBy"] = obj.updatedby;
                            dr["UpdatedOn"] = obj.updatedon;
                            dr["FileLayoutName"] = obj.name;
                            dr["CoreStatus"] = obj.status != null ? obj.status.coretype != null ? obj.status.coretype.name != null ? obj.status.coretype.name : string.Empty : string.Empty : string.Empty; ;
                            dtLayoutInfo.Rows.Add(dr);
                        }
                        dtLayoutInfo = Helper.SortTable(dtLayoutInfo.Copy(), "Seq");
                    }
                }
                catch (Exception E)
                {
                    ShowMessage.ErrorMess(E.Message);
                }
                List<LayoutInfo> objFLI = new List<LayoutInfo>();
                foreach (DataRow dr in dtLayoutInfo.Rows)
                {
                    LayoutInfo objLI = new LayoutInfo();
                    objLI.id = Convert.ToString(dr["LayoutID"]);

                    objLI.description = Convert.ToString(dr["Description"]);
                    objLI.isletest = Convert.ToBoolean(dr["islatest"]);
                    objLI.name = Convert.ToString(dr["FileLayoutName"]);
                    objLI.fileNo = Convert.ToString(dr["LayoutNo"]);
                    objLI.statusId = Convert.ToString(dr["StatusID"]);
                    objLI.statusname = Convert.ToString(dr["LayoutStatus"]);
                    objLI.typeId = Convert.ToString(dr["TypeID"]);
                    objLI.versionno = Convert.ToString(dr["Version"]);
                    objLI.layoutId = Convert.ToString(dr["ACLayoutID"]);
                    objLI.typename = Convert.ToString(dr["LayoutType"]);

                    if (objLI.typeId.Trim().Length > 0)
                    {
                        objLI.type = new ResultSearchCriteriaType();
                        objLI.type.id = Convert.ToInt16(objLI.typeId);
                        objLI.type.name = objLI.typename;
                    }
                    if (objLI.statusId.Trim().Length > 0)
                    {
                        objLI.status = new SaveResultStatus();
                        objLI.status.id = Convert.ToInt16(objLI.statusId);
                        objLI.status.statusname = objLI.statusname;
                    }

                    objLI.createdby = Convert.ToString(dr["CreatedBy"]);
                    objLI.created0n = Convert.ToString(dr["CreatedOn"]);
                    objLI.updatedby = Convert.ToString(dr["UpdatedBy"]);
                    objLI.updatedon = Convert.ToString(dr["UpdatedOn"]);
                    objLI.CoreStatus = Convert.ToString(dr["CoreStatus"]);



                    objFLI.Add(objLI);
                }
                fileLayout = objFLI;
                #endregion
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return fileLayout;
        }
        public static string GetLayoutInfo(List<LayoutInfo> fileLayout)
        {
            string LayoutInfos = "";
            try
            {

                if (fileLayout != null && fileLayout.Count > 0)
                {
                    LayoutInfos = "[";
                    int count = 0;




                    fileLayout = Helper.SortLayoutInfo(fileLayout);
                    foreach (LayoutInfo obj in fileLayout)
                    {
                        if (count == 0)
                            LayoutInfos += "{";
                        else
                            LayoutInfos += ",{";
                        count++;


                        LayoutInfos += @"""id""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.id;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";


                        LayoutInfos += @"""description""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.description;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""isletest""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.isletest;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""name""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.name;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""fileNo""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.fileNo;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""statusId""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.status == null ? string.Empty : Convert.ToString(obj.status.id);
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""statusname""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.status == null ? string.Empty : obj.status.statusname;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""typeId""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.type == null ? string.Empty : Convert.ToString(obj.type.id);
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""versionNo""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.versionno;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""createdby""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.createdby;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""created0n""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.created0n;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""updatedby""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.updatedby;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""updatedon""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.updatedon;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""layoutId""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.layoutId;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";

                        LayoutInfos += @"""CoreStatus""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.CoreStatus != null ? obj.CoreStatus : string.Empty;
                        LayoutInfos += @"""";
                        LayoutInfos += @",";



                        LayoutInfos += @"""typename""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.type == null ? string.Empty : obj.type.name;
                        LayoutInfos += @"""";
                        //LayoutInfos += @",";


                        LayoutInfos += "}";

                    }
                    LayoutInfos += "]";
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return string.Empty;
            }
            return LayoutInfos;
        }

        public static string VerTextAdjustment(string Ver)
        {
            string RetVal = "";
            try
            {
                RetVal = Ver = Ver.Trim();
                if (Ver.Length > 0)
                {
                    int VL = Ver.Length;
                    int RS = 14 - VL;

                    for (int i = 0; i < RS / 2; i++)
                    {
                        RetVal = " " + RetVal;
                    }
                    RS = 11 - RetVal.Length;
                    for (int i = 0; i < RS; i++)
                    {
                        RetVal = RetVal + " ";
                    }
                }
            }
            catch (Exception E)
            {
                RetVal = "";
                ShowMessage.ErrorMess(E.Message);
            }
            return RetVal;

        }

        public static DataTable GetDrawingPropertiesTableStructure()
        {
            DataTable dtDrawingProperty = new DataTable();
            try
            {
                dtDrawingProperty.Columns.Add("DrawingId");
                dtDrawingProperty.Columns.Add("DrawingName");
                dtDrawingProperty.Columns.Add("Classification");
                dtDrawingProperty.Columns.Add("DrawingNumber");
                dtDrawingProperty.Columns.Add("DrawingState");
                dtDrawingProperty.Columns.Add("Revision");
                dtDrawingProperty.Columns.Add("Generation");
                dtDrawingProperty.Columns.Add("Type");
                dtDrawingProperty.Columns.Add("filepath");
                dtDrawingProperty.Columns.Add("isroot");
                dtDrawingProperty.Columns.Add("ProjectName");
                dtDrawingProperty.Columns.Add("ProjectNameNo");
                dtDrawingProperty.Columns.Add("ProjectId");
                dtDrawingProperty.Columns.Add("createdon");
                dtDrawingProperty.Columns.Add("createdby");
                dtDrawingProperty.Columns.Add("modifiedon");
                dtDrawingProperty.Columns.Add("modifiedby");
                dtDrawingProperty.Columns.Add("sourceid");
                dtDrawingProperty.Columns.Add("Layouts");
                dtDrawingProperty.Columns.Add("lockstatus");
                dtDrawingProperty.Columns.Add("lockby");
                dtDrawingProperty.Columns.Add("canDelete");
                dtDrawingProperty.Columns.Add("isowner");
                dtDrawingProperty.Columns.Add("hasViewPermission");
                dtDrawingProperty.Columns.Add("isActFileLatest");
                dtDrawingProperty.Columns.Add("isEditable");
                dtDrawingProperty.Columns.Add("canEditStatus");
                dtDrawingProperty.Columns.Add("hasStatusClosed");
                dtDrawingProperty.Columns.Add("isletest");
                dtDrawingProperty.Columns.Add("projectno");
                dtDrawingProperty.Columns.Add("prefix");
                dtDrawingProperty.Columns.Add("layoutinfo");
                dtDrawingProperty.Columns.Add("oldprefix");

                dtDrawingProperty.Columns.Add("folderid");
                dtDrawingProperty.Columns.Add("folderpath");
                dtDrawingProperty.Columns.Add("IsNewXref");
                dtDrawingProperty.Columns.Add("IsRBCFile");

                dtDrawingProperty.Columns.Add("PK");
                dtDrawingProperty.Columns.Add("FK");
                dtDrawingProperty.Columns.Add("projectManager");
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return null;
            }

            return dtDrawingProperty;
        }

        public static string GetPreFix(object Revision1, object ProjectNo1, object FileNo1, object FileType1)
        {
            string PreFix = "";
            try
            {
                string Revision = Convert.ToString(Revision1);
                string ProjectNo = Convert.ToString(ProjectNo1);
                string FileNo = Convert.ToString(FileNo1);
                string FileType = Convert.ToString(FileType1);
                Revision = Revision.Contains("Ver") ? Revision.Substring(Revision.IndexOf("0")) : Revision;



                PreFix = ProjectNo.Trim().Length > 0 ? ProjectNo + "-" : string.Empty;

                PreFix += FileNo.Trim() == string.Empty ? string.Empty : FileNo + "-";

                PreFix += FileType == string.Empty ? string.Empty : FileType + "-";

                PreFix += Revision == string.Empty ? string.Empty : Revision + "#";


            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return PreFix;
            }

            return PreFix;
        }

        public static string RemovePreFixFromFileName(string FileName, string PreFix)
        {
            try
            {
                if (FileName.Contains(PreFix))
                {
                    if (FileName.Substring(0, PreFix.Length) == PreFix)
                    {
                        FileName = FileName.Substring(PreFix.Length);
                    }
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);

            }
            return FileName;
        }

        public static string DownloadFile(string FileID, string IsRoot = "false", bool IsTemp = false, List<string> DownloadedFiles = null, string ParentFilePath = "", List<clsDownloadedFiles> lstobjDownloadedFiles = null, string ParentPreFix = "")
        {

            Hashtable DrawingProperty = new Hashtable();
            string FilePath = "";
            try
            {
                ResultSearchCriteria Drawing = objRBC.GetDrawingInformation(FileID);


                string checkoutPath = Path.Combine(Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath")), IsTemp ? "Temp" : Drawing.projectname == null || Drawing.projectname == string.Empty ? "My Files" : Drawing.projectname);
                DirectoryInfo di = new DirectoryInfo(checkoutPath);
                di = Directory.Exists(checkoutPath) ? di : Directory.CreateDirectory(checkoutPath);
                string PreFix = Helper.GetPreFix(Drawing.versionno, Drawing.projectNumber, Drawing.fileNo, Drawing.type == null ? string.Empty : Drawing.type.name == null ? string.Empty : Drawing.type.name);

                FilePath = Path.Combine(checkoutPath, PreFix + Drawing.name);

                if (File.Exists(FilePath))
                {
                    if (cadManager.CheckForCurruntlyOpenDoc(FilePath))
                    {
                        Helper.CloseProgressBar();
                        ShowMessage.ValMess("This file is already open.");

                        return null;
                    }
                    else
                    {
                        if (!IsTemp)
                        {
                            if (DownloadedFiles == null || !DownloadedFiles.Contains(FilePath))
                            {

                                if (ShowMessage.InfoYNMess("This file is already in working directory,\n Do you want to replace it ?\n" + FilePath) == DialogResult.Yes)
                                {
                                    File.Delete(FilePath);
                                }
                                else
                                {
                                    FileRename(FilePath);
                                }
                            }
                            else
                            {
                                return null;
                            }

                        }

                    }

                }
                DataTable dtDrawing = FillDrawingPropertiesTable(Drawing, FilePath, IsRoot, PreFix);



                if (dtDrawing.Rows.Count > 0)
                    DrawingProperty = Helper.Table2HashTable(dtDrawing, 0);

                using (var binaryWriter = new BinaryWriter(File.Open(FilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Delete)))
                {
                    binaryWriter.Write(objRBC.GetSingleFileInfo(FileID));
                }
                clsDownloadedFiles objDownloadedFile = new clsDownloadedFiles()
                {
                    FileName = Helper.RemovePreFixFromFileName(Path.GetFileName(FilePath), PreFix),

                    ParentFilePath = ParentFilePath,
                    FilePath = FilePath,
                    XrefStatus = false,
                    Prefix = PreFix
                };
                if (lstobjDownloadedFiles == null)
                {
                    lstobjDownloadedFiles = new List<clsDownloadedFiles>();
                }
                lstobjDownloadedFiles.Add(objDownloadedFile);
                if (IsRoot == "true" && !IsTemp)
                {
                    List<clsDownloadedFiles> lstobjDownloadedFiles1 = new List<clsDownloadedFiles>();
                    clsDownloadedFiles objDF = new clsDownloadedFiles()
                    {
                        FileName = Drawing.name,
                        ParentFileName = Drawing.name,
                        ParentPrefix = PreFix,
                        Prefix = PreFix

                    };

                    lstobjDownloadedFiles1.Add(objDF);
                    if (Drawing.filebean != null)
                        GetTotalChild(Drawing.filebean, Drawing, lstobjDownloadedFiles1);

                    //Helper.CloseProgressBar();
                    foreach (var item in lstobjDownloadedFiles)
                    {
                        item.MainFilePath = FilePath;
                        if (item.ParentFilePath == null || item.ParentFilePath == string.Empty)
                        {
                            item.ParentFilePath = FilePath;
                            item.ParentFileName = Helper.RemovePreFixFromFileName(Path.GetFileName(FilePath), PreFix);
                        }
                    }

                    foreach (var item1 in lstobjDownloadedFiles1)
                    {
                        foreach (var item in lstobjDownloadedFiles)
                        {
                            if (item1.ParentFileName == Helper.RemovePreFixFromFileName(Path.GetFileName(item.ParentFilePath), item1.ParentPrefix))
                            {
                                item1.MainFilePath = item.MainFilePath;
                                item1.ParentFilePath = item.ParentFilePath;
                            }
                            if (item1.ParentFileName == Helper.RemovePreFixFromFileName(Path.GetFileName(item.FilePath), item1.ParentPrefix))
                            {
                                item1.MainFilePath = item.MainFilePath;
                                item1.ParentFilePath = item.FilePath;
                            }
                            if (item1.FileName == Helper.RemovePreFixFromFileName(Path.GetFileName(item.FilePath), item1.Prefix))
                            {
                                item1.MainFilePath = item.MainFilePath;
                                item1.FilePath = item.FilePath;
                            }
                        }

                    }

                    cadManager.CheckXrefStatus(FilePath, FilePath, lstobjDownloadedFiles1);
                    cadManager.AttachingExternalReference(FilePath, lstobjDownloadedFiles1);
                    //cadManager.AddingAttributeToABlock(FilePath);
                    cadManager.OpenActiveDocument(FilePath, "View", DrawingProperty);
                }
                else
                {
                    if (!IsTemp)
                    {
                        cadManager.SetAttributesXrefFiles(DrawingProperty, FilePath);
                        cadManager.UpdateLayoutAttributeArefFile(DrawingProperty, FilePath, false);
                    }
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return FilePath;
        }
        public static void GetTotalChild(ResultSearchCriteria[] Children, ResultSearchCriteria father, List<clsDownloadedFiles> lstobjDownloadedFiles)
        {
            try
            {
                foreach (ResultSearchCriteria child in Children)
                {
                    clsDownloadedFiles objDF = new clsDownloadedFiles()
                    {
                        FileName = child.name,
                        ParentFileName = father.name,
                        Prefix = Helper.GetPreFix(child.versionno, child.projectNumber, child.fileNo, child.type == null ? string.Empty : child.type.name == null ? string.Empty : child.type.name),
                        ParentPrefix = Helper.GetPreFix(father.versionno, father.projectNumber, father.fileNo, father.type == null ? string.Empty : father.type.name == null ? string.Empty : father.type.name)

                    };
                    lstobjDownloadedFiles.Add(objDF);
                    if (child.filebean != null)
                        GetTotalChild(child.filebean, child, lstobjDownloadedFiles);
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }
        public static bool FileRename(string FilePath, int Count = 1)
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string Path1 = Directory.GetParent(FilePath).ToString();
                    string FileName = Path.GetFileNameWithoutExtension(FilePath);
                    string Ext = Path.GetExtension(FilePath);

                    string NewPath = Path.Combine(Path1, FileName + "(" + Count + ")" + Ext);
                    if (File.Exists(NewPath))
                    {
                        return FileRename(FilePath, ++Count);
                    }
                    else
                    {
                        File.Move(FilePath, NewPath); return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return false;
            }
        }
        public static DataTable FillDrawingPropertiesTable(ResultSearchCriteria Drawing, string FilePath, string IsRoot, String PreFix)
        {

            DataTable dtDrawing = Helper.GetDrawingPropertiesTableStructure();



            try
            {
                DataRow dr = dtDrawing.NewRow();
                List<Hashtable> LayoutProperty = new List<Hashtable>();
                #region LayoutInfo
                String LayoutInfos = "";
                LayoutInfos = Helper.GetLayoutInfo(Drawing.fileLayout);
                #endregion

                dr["DrawingId"] = Drawing.id;

                dr["DrawingName"] = Drawing.name;
                dr["Classification"] = Drawing.type == null ? string.Empty : Drawing.type.name == null ? string.Empty : Drawing.type.name;
                dr["DrawingNumber"] = Drawing.fileNo == null ? string.Empty : Drawing.fileNo;
                dr["DrawingState"] = Drawing.status == null ? string.Empty : Drawing.status.statusname;
                dr["Revision"] = Drawing.versionno == null ? string.Empty : Drawing.versionno;
                dr["Generation"] = Drawing.versionno == null ? string.Empty : Drawing.versionno;
                dr["Type"] = Drawing.coreType == null ? string.Empty : Drawing.coreType.name;
                dr["filepath"] = FilePath;
                dr["isroot"] = IsRoot;
                dr["ProjectName"] = Drawing.projectname == null || Drawing.projectname == string.Empty ? "My Files" : Drawing.projectname;
                dr["ProjectNameNo"] = Drawing.projectname == null || Drawing.projectname == string.Empty ? "" : Drawing.projectname + " (" + Drawing.projectNumber + ")";
                dr["ProjectId"] = Drawing.projectinfo;
                dr["createdon"] = Drawing.created0n;
                dr["createdby"] = Drawing.createdby;
                dr["modifiedon"] = Drawing.updatedon;
                dr["modifiedby"] = Drawing.updatedby;
                dr["sourceid"] = "";
                dr["Layouts"] = "";
                dr["lockstatus"] = Drawing.filelock;
                dr["lockby"] = Drawing.updatedby;
                dr["canDelete"] = Drawing.canDelete;
                dr["isowner"] = Drawing.isowner;
                dr["hasViewPermission"] = Drawing.hasViewPermission;
                dr["isActFileLatest"] = Drawing.isActFileLatest;
                dr["isEditable"] = Drawing.isEditable;
                dr["canEditStatus"] = Drawing.canEditStatus;
                dr["hasStatusClosed"] = Drawing.hasStatusClosed;
                dr["isletest"] = Drawing.isletest;
                dr["projectno"] = Drawing.projectNumber;
                dr["prefix"] = PreFix; ;
                dr["layoutinfo"] = LayoutInfos;
                dr["oldprefix"] = PreFix;

                dr["folderid"] = Drawing.folderid == null ? string.Empty : Drawing.folderid;
                dr["folderpath"] = Drawing.folderpath == null ? string.Empty : Drawing.folderpath;
                dr["IsNewXref"] = "";//IsNewXref  not to assign value from here, if ever assign assign fasle.
                dr["IsRBCFile"] = "true";//Always true.
                dr["PK"] = "";// will be assign later
                dr["FK"] = "";// will be assign later
                dr["projectManager"] = Drawing.projectManager;


                dtDrawing.Rows.Add(dr);

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dtDrawing;
        }
        public static DataTable AddRowDrawingPropertiesTable(DataTable dtDrawingProperties, Hashtable htDrawingProperties)
        {
            try
            {
                bool IsAnyValueMatch = false;
                DataRow dr = dtDrawingProperties.NewRow();
                if (htDrawingProperties.Count > 0)
                {
                    foreach (DictionaryEntry de in htDrawingProperties)
                    {
                        for (int i = 0; i < dtDrawingProperties.Columns.Count; i++)
                        {
                            if (Convert.ToString(de.Key).ToLower() == dtDrawingProperties.Columns[i].ColumnName.ToLower())
                            {
                                IsAnyValueMatch = true;
                                dr[dtDrawingProperties.Columns[i].ColumnName] = de.Value == null ? string.Empty : Convert.ToString(de.Value);
                                break;
                            }
                        }
                    }
                    if (IsAnyValueMatch)
                    {
                        if (Convert.ToString(dr["IsRBCFile"]) == "true")
                        {
                            dtDrawingProperties.Rows.Add(dr);
                        }

                    }
                    else
                    {

                    }
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dtDrawingProperties;
        }

        public static string CreateTempFile(string FileName, byte[] RawBytes = null)
        {
            string TempFilePath = "";
            try
            {
                DeleteTempFolder();
                if (RawBytes != null)
                {
                    string checkoutPath = Path.Combine(Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath")), "Temp");
                    DirectoryInfo di = new DirectoryInfo(checkoutPath);
                    di = Directory.Exists(checkoutPath) ? di : Directory.CreateDirectory(checkoutPath);

                    TempFilePath = Path.Combine(checkoutPath, "Temp" + FileName);

                    using (var binaryWriter = new BinaryWriter(File.Open(TempFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Delete)))
                    {
                        binaryWriter.Write(RawBytes);
                    }
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return "";
            }
            return TempFilePath;
        }
        public static string CopyTempFile(string FilePath)
        {
            string TempFilePath = "";
            try
            {
                string FileName = Path.GetFileName(FilePath);


                string checkoutPath = Path.Combine(Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath")), "Temp");
                DirectoryInfo di = new DirectoryInfo(checkoutPath);
                di = Directory.Exists(checkoutPath) ? di : Directory.CreateDirectory(checkoutPath);

                TempFilePath = Path.Combine(checkoutPath, "Temp" + FileName);
                File.Delete(TempFilePath);
                File.Copy(FilePath, TempFilePath);


            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return "";
            }
            return TempFilePath;
        }
        public static void DeleteTempFolder()
        {
            try
            {

                string checkoutPath = Path.Combine(Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath")), "Temp");
                DirectoryInfo di = new DirectoryInfo(checkoutPath);


                if (Directory.Exists(checkoutPath))
                {
                    Directory.Delete(checkoutPath);
                }




            }
            catch (Exception E)
            {
                return;
                ShowMessage.ErrorMess(E.Message);
            }
        }
        public static void DownloadOpenDoc(string FileID)
        {
            try
            {
                byte[] RawBytes = null;
                ResultSearchCriteria Drawing = objRBC.GetSingleFileInfo(FileID, ref RawBytes);


            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        public static Stack<List<clsFolderSearchReasult>> CopyStack(Stack<List<clsFolderSearchReasult>> StackFolderSearchReasult)
        {
            Stack<List<clsFolderSearchReasult>> TStackFolderSearchReasult1 = new Stack<List<clsFolderSearchReasult>>();
            Stack<List<clsFolderSearchReasult>> TStackFolderSearchReasult = new Stack<List<clsFolderSearchReasult>>();
            try
            {
                int Count = StackFolderSearchReasult.Count;

                for (int i = 0; i < Count; i++)
                {
                    List<clsFolderSearchReasult> objFSRL = StackFolderSearchReasult.Pop();
                    TStackFolderSearchReasult1.Push(objFSRL);
                }
                Count = TStackFolderSearchReasult1.Count;
                for (int i = 0; i < Count; i++)
                {
                    List<clsFolderSearchReasult> objFSRL = TStackFolderSearchReasult1.Pop();
                    TStackFolderSearchReasult.Push(objFSRL);
                    StackFolderSearchReasult.Push(objFSRL);
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return TStackFolderSearchReasult;
        }

        public static bool FileCompare(string file1, string file2)
        {
            int file1byte;
            int file2byte;
            FileStream fs1;
            FileStream fs2;

            try
            {

                // Determine if the same file was referenced two times.
                if (file1 == file2)
                {
                    // Return true to indicate that the files are the same.
                    return true;
                }

                // Open the two files.
                fs1 = new FileStream(file1, FileMode.Open);
                fs2 = new FileStream(file2, FileMode.Open);

                // Check the file sizes. If they are not the same, the files 
                // are not the same.
                if (fs1.Length != fs2.Length)
                {
                    // Close the file
                    fs1.Close();
                    fs2.Close();

                    // Return false to indicate files are different
                    return false;
                }

                // Read and compare a byte from each file until either a
                // non-matching set of bytes is found or until the end of
                // file1 is reached.
                do
                {
                    // Read one byte from each file.
                    file1byte = fs1.ReadByte();
                    file2byte = fs2.ReadByte();
                }
                while ((file1byte == file2byte) && (file1byte != -1));

                // Close the files.
                fs1.Close();
                fs2.Close();

                // Return the success of the comparison. "file1byte" is 
                // equal to "file2byte" at this point only if the files are 
                // the same.
                return ((file1byte - file2byte) == 0);
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return true;
            }
        }

        public static long GetFileSizeOnDisk(string file)
        {
            FileInfo info = new FileInfo(file);
            uint dummy, sectorsPerCluster, bytesPerSector;
            int result = GetDiskFreeSpaceW(info.Directory.Root.FullName, out sectorsPerCluster, out bytesPerSector, out dummy, out dummy);
            if (result == 0)
            {
                return 0;
            }
            uint clusterSize = sectorsPerCluster * bytesPerSector;
            uint hosize;
            uint losize = GetCompressedFileSizeW(file, out hosize);
            long size;
            size = (long)hosize << 32 | losize;
            return ((size + clusterSize - 1) / clusterSize) * clusterSize;
        }

        [DllImport("kernel32.dll")]
        static extern uint GetCompressedFileSizeW([In, MarshalAs(UnmanagedType.LPWStr)] string lpFileName,
           [Out, MarshalAs(UnmanagedType.U4)] out uint lpFileSizeHigh);

        [DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
        static extern int GetDiskFreeSpaceW([In, MarshalAs(UnmanagedType.LPWStr)] string lpRootPathName,
           out uint lpSectorsPerCluster, out uint lpBytesPerSector, out uint lpNumberOfFreeClusters,
           out uint lpTotalNumberOfClusters);

        public static string GetLatestVersion(string DrawingNo)
        {
            string VersionNo = "";
            try
            {
                RBConnector objRBC = new RBConnector();

                ResultSearchCriteria Drawing = objRBC.GetDrawingInformation(objRBC.SearchLatestFile(DrawingNo));
                if (Drawing != null)
                {
                    VersionNo = Drawing.versionno == null ? string.Empty : Helper.VerTextAdjustment(Drawing.versionno);
                }
                else
                {
                    VersionNo = "0";
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return VersionNo;
        }

        public static string GetFolderPath(clsFolderInfo folderInfo, string ProjectName)
        {
            string FolderPath = "";
            try
            {
                if (folderInfo == null)
                {
                    return ProjectName + "/";
                }
                else
                {
                    GetParentFolder(folderInfo, ref FolderPath);
                    FolderPath = ProjectName + "/" + FolderPath;
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return FolderPath;
        }

        public static string GetParentFolder(clsFolderInfo folderInfo, ref string FolderPath)
        {

            try
            {
                if (folderInfo == null)
                {
                    return null;
                }
                else
                {

                    if (folderInfo.parentFolder == null)
                    {
                        return FolderPath += folderInfo.name + @"/";
                    }
                    else
                    {
                        GetParentFolder(folderInfo.parentFolder, ref FolderPath);
                        FolderPath = FolderPath + folderInfo.name + "/";
                        return FolderPath;
                    }



                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message); return null;
            }

        }

        public static DataTable RowFilter(DataTable dt, string ColumnName, string Value)
        {
            DataTable dtResult = dt.Clone();
            try
            {
                DataView dv = dt.Copy().DefaultView;
                dv.RowFilter = ColumnName + "='" + Value + "'";
                dtResult = dv.ToTable();
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dtResult;
        }
        public static DataTable RowFilter(DataTable dt, string ColumnName1, string ColumnName2, string Operator)
        {
            DataTable dtResult = dt.Clone();
            try
            {
                DataView dv = dt.Copy().DefaultView;
                dv.RowFilter = ColumnName1 + Operator + ColumnName2;
                dtResult = dv.ToTable();
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dtResult;
        }

        public static void GetSellectedNode(TreeGridNode ParentNode, ref int PBValue, int IncrementValue = 1)
        {
            try
            {
                foreach (TreeGridNode childNode in ParentNode.Nodes)
                {
                    if ((bool)childNode.Cells["Check"].FormattedValue)
                    {
                        PBValue += IncrementValue;

                        GetSellectedNode(childNode, ref PBValue, IncrementValue);
                    }
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }
        public static string FormatDateTime(object dtDateTime)
        {
            return FormatDateTimeString(dtDateTime, 1);
        }
        public static string FormatDate(object dtDateTime)
        {
            return FormatDateTimeString(dtDateTime, 2);
        }
        public static string FormatTime(object dtDateTime)
        {
            return FormatDateTimeString(dtDateTime, 3);
        }
        /// <summary>
        /// Returns formated date time string
        /// </summary>
        /// <param name="dtDateTime"></param>
        /// <param name="Flag">1 for datetime, 2 for date and 3 for Time</param>
        /// <returns></returns>
        public static string FormatDateTimeString(object dtDateTime, int Flag)
        {
            string DT = "";
            try
            {
                if (dtDateTime != null)
                {
                    string Format = Flag == 1 ? Helper.DateTimeFormat : Flag == 2 ? Helper.DateFormat : Flag == 3 ? Helper.TimeFormat : string.Empty;
                    DT = Convert.ToString(dtDateTime) == string.Empty ? string.Empty : Convert.ToString(dtDateTime);
                    DT = DT == string.Empty || Format == string.Empty ? string.Empty : Convert.ToDateTime(DT).ToString(Format);
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return DT;
        }

        public static bool IsClosedStatus(string Status)
        {
            try
            {
                DataTable dtStatus = objRBC.GetFIleStatus();
                string FileStatus = Status;
                if (dtStatus != null)
                {
                    DataRow[] dr1 = dtStatus.Select("statusname = '" + FileStatus + "' and IsClosed ='True'");
                    if (dr1.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return false;
        }

        public static void LoadMasterData()
        {
            try
            {
                objRBC.GetFIleType();
                objRBC.GetFIleStatus();
                objRBC.GetProjectDetail();
                DrawingAttributes = new List<string>() {
                    "DRAWINGNUMBER",
                    "DRAWINGNAME", "PROJECTNAME", "PROJECTNUMBER", "DRAWINGVER",
                    "TYPE", "DRAWINGSTATE"  };

                LayoutAttributes = new List<string>() {
                      "LAYOUTNAME", "CREATEDBY",
                    "MODIFIEDBY", "CREATEDON", "MODIFIEDON", "LAYOUTSTATE",
                    "GOODNOT", "LAYOUTVER" };

                TestingAttributes = new List<string>() {
                      "LAYOUTNAME", };


                GenerateInvalidCharacterString();

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

        }

        public static bool FileNameCheckForSpecialCharacter(string FileName)
        {
            try
            {
                GenerateInvalidCharacterString();
                foreach (string character in InvalidCharacter)
                {
                    if (FileName.Contains(character))
                    {
                        ShowMessage.ValMess("File name must not contain following character.\n" + @InvalidCharacterString);
                        return false;
                    }
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message); return false;
            }
            return true;
        }
        public static bool GenerateInvalidCharacterString()
        {
            try
            {
                InvalidCharacterString = "";
                foreach (string character in InvalidCharacter)
                {
                    @InvalidCharacterString = @InvalidCharacterString + @character;
                }
                //@InvalidCharacterString = @InvalidCharacterString.Substring(0, @InvalidCharacterString.Length - 1);
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return true;
        }
    }
}
