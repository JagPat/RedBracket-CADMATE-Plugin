using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArasConnector.Configuration
{
     public class ProjectConfiguration
        {
            private static List<String> proConfig = new List<String>();
            public ProjectConfiguration(String configStr)
            {
                proConfig.Add(configStr);
            }
            public static List<String> ProConfig
            {
                get { return proConfig; }
            }
        }
    
}
