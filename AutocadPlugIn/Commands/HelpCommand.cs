using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Gssoft.Gscad.ApplicationServices;

namespace RBAutocadPlugIn
{
    public class HelpCommand : Command
    {
        private String helpFilePath;

        public String HelpFilePath
        {
            get { return this.helpFilePath; }
            set { this.helpFilePath = value; }
        }


    }
}
