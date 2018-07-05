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

namespace AutocadPlugIn
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
        public static RBConnector objRBC = new RBConnector();
        public static AutoCADManager cadManager = new AutoCADManager();
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
                    DrawingProperty.Add("layoutinfo", dt.Rows[i]["layoutinfo"]);
                    DrawingProperty.Add("oldprefix", dt.Rows[i]["oldprefix"]);

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
                            DrawingProperty.Add("Layout_" + Count + "_CreatedOn", objLI1.createdon);
                            DrawingProperty.Add("Layout_" + Count + "_UpdatedBy", objLI1.updatedby);
                            DrawingProperty.Add("Layout_" + Count + "_UpdatedOn", objLI1.updatedon);

                            // Count++;
                        }
                    }
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
                ////if (dt.Rows.Count > i)
                ////{
                ////    DrawingProperty.Add("DrawingId", dt.Rows[i]["DrawingId"]);
                ////    DrawingProperty.Add("DrawingName", dt.Rows[i]["DrawingName"]);
                ////    DrawingProperty.Add("Classification", dt.Rows[i]["Classification"]);
                ////    DrawingProperty.Add("DrawingNumber", dt.Rows[i]["DrawingNumber"]);
                ////    DrawingProperty.Add("DrawingState", dt.Rows[i]["DrawingState"]);
                ////    DrawingProperty.Add("Revision", dt.Rows[i]["Revision"]);
                ////    DrawingProperty.Add("Generation", dt.Rows[i]["Generation"]);
                ////    DrawingProperty.Add("Type", dt.Rows[i]["Type"]);
                ////    DrawingProperty.Add("filepath", dt.Rows[i]["filepath"]);
                ////    DrawingProperty.Add("isroot", dt.Rows[i]["isroot"]);
                ////    DrawingProperty.Add("ProjectName", dt.Rows[i]["ProjectName"]);
                ////    DrawingProperty.Add("ProjectId", dt.Rows[i]["ProjectId"]);
                ////    DrawingProperty.Add("CreatedOn", dt.Rows[i]["createdon"]);
                ////    DrawingProperty.Add("CreatedBy", dt.Rows[i]["createdby"]);
                ////    DrawingProperty.Add("ModifiedOn", dt.Rows[i]["modifiedon"]);
                ////    DrawingProperty.Add("ModifiedBy", dt.Rows[i]["modifiedby"]);
                ////    DrawingProperty.Add("sourceid", dt.Rows[i]["sourceid"]);
                ////    DrawingProperty.Add("Layouts", dt.Rows[i]["Layouts"]);

                ////    DrawingProperty.Add("canDelete", dt.Rows[i]["canDelete"]);
                ////    DrawingProperty.Add("isowner", dt.Rows[i]["isowner"]);
                ////    DrawingProperty.Add("hasViewPermission", dt.Rows[i]["hasViewPermission"]);
                ////    DrawingProperty.Add("isActFileLatest", dt.Rows[i]["isActFileLatest"]);
                ////    DrawingProperty.Add("isEditable", dt.Rows[i]["isEditable"]);
                ////    DrawingProperty.Add("canEditStatus", dt.Rows[i]["canEditStatus"]);
                ////    DrawingProperty.Add("hasStatusClosed", dt.Rows[i]["hasStatusClosed"]);
                ////    DrawingProperty.Add("isletest", dt.Rows[i]["isletest"]);
                ////    DrawingProperty.Add("projectno", dt.Rows[i]["projectno"]);
                ////    DrawingProperty.Add("prefix", dt.Rows[i]["prefix"]);
                ////    DrawingProperty.Add("filetypeid", dt.Rows[i]["Classification"]);
                ////    DrawingProperty.Add("layoutinfo", dt.Rows[i]["layoutinfo"]);
                ////    DrawingProperty.Add("oldprefix", dt.Rows[i]["oldprefix"]);

                ////    string LayoutInfo1 = Convert.ToString(dt.Rows[i]["layoutinfo"]).Trim();
                ////    if (LayoutInfo1.Length > 0)
                ////    {
                ////        List<LayoutInfo> objLI = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LayoutInfo>>(LayoutInfo1);
                ////        int Count = 1;
                ////        foreach (LayoutInfo objLI1 in objLI)
                ////        {
                ////            DrawingProperty.Add("Layout_" + Count + "_Name", objLI1.name);
                ////            DrawingProperty.Add("Layout_" + Count + "_Number", objLI1.fileNo);
                ////            DrawingProperty.Add("Layout_" + Count + "_Type", objLI1.typename);
                ////            DrawingProperty.Add("Layout_" + Count + "_Status", objLI1.statusname);
                ////            DrawingProperty.Add("Layout_" + Count + "_VersionNote", objLI1.description);
                ////            DrawingProperty.Add("Layout_" + Count + "_CreatedBy", objLI1.createdby);
                ////            DrawingProperty.Add("Layout_" + Count + "_UpdatedBy", objLI1.updatedby);
                ////            DrawingProperty.Add("Layout_" + Count + "_UpdatedOn", objLI1.updatedon);

                ////            Count++;
                ////        }
                ////    }
                //}
                //else
                //{
                //    ShowMessage.ErrorMess("Specified index not found.");
                //}
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
                            dr["CreatedOn"] = obj.createdon;
                            dr["UpdatedBy"] = obj.updatedby;
                            dr["UpdatedOn"] = obj.updatedon;
                            dr["FileLayoutName"] = obj.name;
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
                    objLI.createdon = Convert.ToString(dr["CreatedOn"]);
                    objLI.updatedby = Convert.ToString(dr["UpdatedBy"]);
                    objLI.updatedon = Convert.ToString(dr["UpdatedOn"]);




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

                        LayoutInfos += @"""createdon""";
                        LayoutInfos += @":";
                        LayoutInfos += @"""";
                        LayoutInfos += obj.createdon;
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

        public static Hashtable FillhtDrawingProperty(string FileID, string IsRoot)
        {

            Hashtable DrawingProperty = new Hashtable();
            DataTable dtDrawing = Helper.GetDrawingPropertiesTableStructure();


            DataRow dr = dtDrawing.NewRow();
            try
            {
                ResultSearchCriteria Drawing = objRBC.GetDrawingInformation(FileID);


                string checkoutPath = Path.Combine(Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath")), Drawing.projectname == null || Drawing.projectname == string.Empty ? "My Files" : Drawing.projectname);
                DirectoryInfo di = new DirectoryInfo(checkoutPath);
                di = Directory.Exists(checkoutPath) ? di : Directory.CreateDirectory(checkoutPath);
                string PreFix = Helper.GetPreFix(Drawing.versionno, Drawing.projectNumber, Drawing.fileNo, Drawing.type == null ? string.Empty : Drawing.type.name == null ? string.Empty : Drawing.type.name);

                string FilePath = Path.Combine(checkoutPath, PreFix + Drawing.name);

                if (File.Exists(FilePath))
                {
                    if (cadManager.CheckForCurruntlyOpenDoc(FilePath))
                    {

                        ShowMessage.ValMess("This file is already open.");

                        return null;
                    }
                }

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


                dtDrawing.Rows.Add(dr);

                if (dtDrawing.Rows.Count > 0)
                    DrawingProperty = Helper.Table2HashTable(dtDrawing, 0);


                if (IsRoot == "1")
                {

                }
                else
                {

                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return DrawingProperty;
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
                        dtDrawingProperties.Rows.Add(dr);
                    }
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dtDrawingProperties;
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
    }
}
