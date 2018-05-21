using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArasConnector.Configuration
{
   public class DrawingConfiguration
    {
      private  static List<String> docConfig = new List<String>() ;
       public DrawingConfiguration(String configStr)
       {
           docConfig.Add(configStr);
       }
       public static List<String> DocConfig
       {
           get { return docConfig; }
       }
    }
}
