using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace RBAutocadPlugIn
{
    public class ExpandObjectCommand : Command
    {
      /*  private PLMObject drawingObjects = new PLMObject();

        public PLMObject DrawingObjects
        {
            get { return this.drawingObjects; }
            set { this.drawingObjects = value; }
        }
        */
        private String relationshipName;
        public String RelationshipName
        {
            get { return this.relationshipName; }
            set { this.relationshipName = value; }
        }

        private String plmObjectInfo;
        public String PLMObjectInfo
        {
            get { return this.plmObjectInfo; }
            set { this.plmObjectInfo = value; }
        }



         

    }
}
