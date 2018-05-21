using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ArasConnector.Configuration;

namespace ArasConnector
{
    class ConfigItemType 
    {
        String[] arConfigStr = null;
        Hashtable htConfigItemClassification = new Hashtable();


        public Hashtable GetConfigItemType()
        {
            int itemCount = DrawingConfiguration.DocConfig.Count;
            htConfigItemClassification.Add("Non", "CAD");
            foreach (String configStr in DrawingConfiguration.DocConfig)
            {
                arConfigStr = configStr.Split('|');
                htConfigItemClassification.Add(arConfigStr[0], arConfigStr[1]);
            }

            //ConfigCmd.GetConfigItemType = htConfigItemClassification;
            return htConfigItemClassification;
        }

        public string GetConfigClassification(string uniquename)
        {
            int itemCount = DrawingConfiguration.DocConfig.Count;
            string classificationPath = null;
            foreach (String configStr in DrawingConfiguration.DocConfig)
            {
                arConfigStr = configStr.Split('|');
                if (arConfigStr[0].ToString() == uniquename)
                {
                    classificationPath = arConfigStr[2].ToString();
                    break;
                }
            }
            if (uniquename == "Non")
            {
                classificationPath = "";
            }

            return classificationPath;
        }
    } 
}
