using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace CADController.Commands
{
    public class LockCommand : Command
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

        //private String projectname;
        //public String ProjectName
        //{
        //    get { return this.projectname; }
        //    set { this.projectname = value; }
        //}

        //private String drawingType;
        //public String DrawingType
        //{
        //    get { return this.drawingType; }
        //    set { this.drawingType = value; }
        //}

    }
}