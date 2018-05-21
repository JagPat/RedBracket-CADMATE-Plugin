using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Generic;
using ArasConnector.Configuration;
using System.Windows.Forms;
namespace CADController.Configuration
{
    public class CADIntegrationConfiguration
    {
        String[] arConfigStr = null;
        Hashtable htDispNameClassificationPath = new Hashtable();
        Hashtable htconfigItemClassification = new Hashtable();

        public Hashtable GetClassification()
        {
            int itemCount = DrawingConfiguration.DocConfig.Count;

            foreach (String configStr in DrawingConfiguration.DocConfig)
            {
                arConfigStr = configStr.Split('|');
                htDispNameClassificationPath.Add(arConfigStr[1], arConfigStr[0]);

            }

            return htDispNameClassificationPath;
        }
        
        public DataTable GetProject()
        {
            DataTable htDispNameProject = new DataTable();
            htDispNameProject.Columns.Add("ProjectId", typeof(string));
            htDispNameProject.Columns.Add("ProjectName", typeof(string));
            htDispNameProject.Columns.Add("ProjectNo", typeof(string));
            int itemCount = ProjectConfiguration.ProConfig.Count;
            foreach (String configStr in ProjectConfiguration.ProConfig)
            {
                arConfigStr = configStr.Split('|');
                htDispNameProject.Rows.Add(arConfigStr[0],arConfigStr[1],arConfigStr[2]);                
            }
            return htDispNameProject;
        }

        public DataTable GetRealtyEntity()
        {
            DataTable htDispNameRealty = new DataTable();
            htDispNameRealty.Columns.Add("ProjectId", typeof(string));
            htDispNameRealty.Columns.Add("RealtyName", typeof(string));
            htDispNameRealty.Columns.Add("RealtyNo", typeof(string));
            int itemCount = RealtyConfiguration.RltConfig.Count;
            foreach (String configStr in RealtyConfiguration.RltConfig)
            {
                arConfigStr = configStr.Split('|');
                htDispNameRealty.Rows.Add(arConfigStr[0], arConfigStr[1], arConfigStr[2]);
            }
            return htDispNameRealty;
        }

        public DataTable GetLifeCycleState()
        {
            DataTable htDispNameState = new DataTable();
            htDispNameState.Columns.Add("Classification", typeof(string));
            htDispNameState.Columns.Add("StateName", typeof(string));
            htDispNameState.Columns.Add("StateLabel", typeof(string));
            int itemCount = LifeCycleConfiguration.LCConfig.Count;
            foreach (String configStr in LifeCycleConfiguration.LCConfig)
            {
                arConfigStr = configStr.Split('|');
                htDispNameState.Rows.Add(arConfigStr[0], arConfigStr[1], arConfigStr[2]);
            }
            return htDispNameState;
        }
    }
}
