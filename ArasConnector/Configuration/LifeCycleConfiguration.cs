using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArasConnector.Configuration
{
    public class LifeCycleConfiguration
    {
        private static List<String> lcConfig = new List<String>();
        public LifeCycleConfiguration(String configStr)
        {
            lcConfig.Add(configStr);
        }
        public static List<String> LCConfig
        {
            get { return lcConfig; }
        }
    }
}
