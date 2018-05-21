using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CADController.Commands
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
