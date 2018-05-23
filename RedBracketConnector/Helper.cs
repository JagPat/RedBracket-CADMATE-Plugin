using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using System.Windows.Forms;
namespace RedBracketConnector
{
    public static class Helper
    {
        public static string UserName = "";
        public static string UserFullName = "Archi User";
        public static string FirstName = "Archi";
        public static string LastName = "User";
        public static string UserID = "1";
        public static string FileNamePrefix = "RBDF-";
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
        public static void FIllCMB(ComboBox cmb, DataTable dt, string DisplayMember, string ValueMenmber ,bool IsSelect)
        {
            try
            {
                String Text = "All";
                if (IsSelect)
                    Text = "Select";
                dt= Helper.AddFirstRowToTable(dt, Text, DisplayMember);

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
        public static void FIllCMB(DataGridViewComboBoxColumn cmb, DataTable dt, string DisplayMember, string ValueMenmber, bool IsSelect)
        {
            try
            {
                String Text = "All";
                if (IsSelect)
                    Text = "Select";
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

        public static DataTable AddFirstRowToTable(DataTable dt,string Text, string DisplayMember)
        {
            try
            {
                if (dt == null)
                {
                    dt.Columns.Add("id");
                    dt.Columns.Add(DisplayMember);
                     
                }
                dt.Columns.Add("Rank");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Rank"] = 2;
                }
                DataRow dr = dt.NewRow();
                dr["id"] = -1;
                dr[DisplayMember] = "---"+ Text + "---";
                dr["Rank"] = 1;

                dt.Rows.Add(dr);

                DataView dv = dt.DefaultView;
                dv.Sort = "Rank,"+ DisplayMember;
                dt = dv.ToTable();

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dt;
        }


    }
}
