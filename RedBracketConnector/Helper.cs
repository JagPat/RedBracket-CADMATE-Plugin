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
        public static void FIllCMB(DataGridViewComboBoxColumn cmb, DataTable dt, string DisplayMember, string ValueMenmber, bool IsSelect,string FirstRowText=null)
        {
            try
            {
                String Text = "All";
                if(FirstRowText==null)
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




    }
}
