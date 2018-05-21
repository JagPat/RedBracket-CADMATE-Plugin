using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace ArasConnector
{
    public class PLMConnectorFactory
    {
        static IConnector connector = null;
        public static IConnector getPLMConnector(String strConnector){
           
            if (connector == null)
            {
                if (strConnector.Equals("Aras"))
                {
                   
                    connector = new ArasConnector();
                }
            }
            return connector;
        }
    }
}
