using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArasConnector.Configuration
{
    public class RealtyConfiguration
    {
        private static List<String> rltConfig = new List<String>();
        public RealtyConfiguration(String configStr)
        {
            rltConfig.Add(configStr);
        }
        public static List<String> RltConfig
        {
            get { return rltConfig; }
        }
    }
}
