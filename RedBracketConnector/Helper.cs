using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
