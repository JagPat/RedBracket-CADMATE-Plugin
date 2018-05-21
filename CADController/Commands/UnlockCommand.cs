using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace CADController.Commands
{
    public class UnlockCommand : Command
    {
        private ArrayList drawingIds;
        public ArrayList DrawingIds
        {
            get { return this.drawingIds; }
            set { this.drawingIds = value; }
        }

        private DataTable drawingInfo;
        public DataTable DrawingInfo
        {
            get { return this.drawingInfo; }
            set { this.drawingInfo = value; }
        }
    }
}
