using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
