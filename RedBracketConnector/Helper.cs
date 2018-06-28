using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

using System.Windows.Forms;
using System.IO;
using System.Collections;
namespace RedBracketConnector
{
    public static class Helper
    {
        public static string UserName = "";//Like X@ymail.com
        public static string UserFullName = "";// Like X User
        public static string FirstName = "";// Like X
        public static string LastName = "";// Like User
        public static string UserID = ""; // Like 1
        public static string FileNamePrefix = "RB-";
        public static string CompanyName = "RedBracket";
        public static bool IsEventAssign = false;
        public static bool IsSavePassword = true;
        public static decimal FileLayoutNameLength = 255;

        public static string CurrentVersion = "";
        public static string LatestVersion = "";

        public static bool IsRenameChild = true;
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
        public static void FIllCMB(ComboBox cmb, DataTable dt, string DisplayMember, string ValueMenmber, bool IsSelect)
        {
            try
            {
                String Text = "All";
                if (IsSelect)
                    Text = "---Select---";
                dt = Helper.AddFirstRowToTable(dt, Text, DisplayMember);

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
        public static void FIllCMB(DataGridViewComboBoxColumn cmb, DataTable dt, string DisplayMember, string ValueMenmber, bool IsSelect, string FirstRowText = null)
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

                dt = Helper.AddFirstRowToTable(dt, Text, DisplayMember);

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

        public static DataTable AddFirstRowToTable(DataTable dt, string Text, string DisplayMember)
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
                dv.Sort = "Rank," + DisplayMember;
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
                DataRow[] dr = dt.Select(ValueMember + " = '" + Value + "'");

                if (dr.Length > 0)
                {
                    RValue = Convert.ToString(dr[0][DisplayMember]);
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
                    DrawingProperty.Add("DrawingId", dt.Rows[i]["DrawingId"]);
                    DrawingProperty.Add("DrawingName", dt.Rows[i]["DrawingName"]);
                    DrawingProperty.Add("Classification", dt.Rows[i]["Classification"]);
                    DrawingProperty.Add("DrawingNumber", dt.Rows[i]["DrawingNumber"]);
                    DrawingProperty.Add("DrawingState", dt.Rows[i]["DrawingState"]);
                    DrawingProperty.Add("Revision", dt.Rows[i]["Revision"]);
                    DrawingProperty.Add("Generation", dt.Rows[i]["Generation"]);
                    DrawingProperty.Add("Type", dt.Rows[i]["Type"]);
                    DrawingProperty.Add("filepath", dt.Rows[i]["filepath"]);
                    DrawingProperty.Add("isroot", dt.Rows[i]["isroot"]);
                    DrawingProperty.Add("ProjectName", dt.Rows[i]["ProjectName"]);
                    DrawingProperty.Add("ProjectId", dt.Rows[i]["ProjectId"]);
                    DrawingProperty.Add("CreatedOn", dt.Rows[i]["createdon"]);
                    DrawingProperty.Add("CreatedBy", dt.Rows[i]["createdby"]);
                    DrawingProperty.Add("ModifiedOn", dt.Rows[i]["modifiedon"]);
                    DrawingProperty.Add("ModifiedBy", dt.Rows[i]["modifiedby"]);
                    DrawingProperty.Add("sourceid", dt.Rows[i]["sourceid"]);
                    DrawingProperty.Add("Layouts", dt.Rows[i]["Layouts"]);

                    DrawingProperty.Add("canDelete", dt.Rows[i]["canDelete"]);
                    DrawingProperty.Add("isowner", dt.Rows[i]["isowner"]);
                    DrawingProperty.Add("hasViewPermission", dt.Rows[i]["hasViewPermission"]);
                    DrawingProperty.Add("isActFileLatest", dt.Rows[i]["isActFileLatest"]);
                    DrawingProperty.Add("isEditable", dt.Rows[i]["isEditable"]);
                    DrawingProperty.Add("canEditStatus", dt.Rows[i]["canEditStatus"]);
                    DrawingProperty.Add("hasStatusClosed", dt.Rows[i]["hasStatusClosed"]);
                    DrawingProperty.Add("isletest", dt.Rows[i]["isletest"]);
                    DrawingProperty.Add("projectno", dt.Rows[i]["projectno"]);
                    DrawingProperty.Add("prefix", dt.Rows[i]["prefix"]);
                    DrawingProperty.Add("filetypeid", dt.Rows[i]["Classification"]);
                    
                }
                else
                {
                    ShowMessage.ErrorMess("Specified index not found.");
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

            return DrawingProperty;

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
                            dr["Seq"] = obj.fileNo.Substring(0, obj.fileNo.IndexOf("_"));
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
                        objLI.status = new ResultSearchCriteriaStatus();
                        objLI.status.id = Convert.ToInt16(objLI.statusId);
                        objLI.status.statusname = objLI.statusname;
                    }

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

                        LayoutInfos += @"""layoutId""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.layoutId;
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
                RetVal= Ver = Ver.Trim();
                if (Ver.Length>0)
                {
                    int VL = Ver.Length;
                    int RS = 14 - VL;

                    for (int i = 0; i < RS/2; i++)
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


    }
}
