using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
 
namespace AutocadPlugIn
{
    public class CADFactory
    {
        static ICADManager cadManager = null;
        public static ICADManager getCADManager()
        {

            if (cadManager == null)
            {
                cadManager = new AutoCADManager();
            }
            return cadManager;
        }
    }
}
