using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArasConnector;

namespace CADController.Commands
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




        private List<RelationshipNavigatorRequest> relationship = new List<RelationshipNavigatorRequest>();

        public List<RelationshipNavigatorRequest> Relationship
        {
            get { return this.relationship; }
            set { this.relationship = value; }
        }

        private ObjectDataSpecs toSideObjectAtrribute = new ObjectDataSpecs();

        public ObjectDataSpecs ToSideObjectAtrribute
        {
            get { return this.toSideObjectAtrribute; }
            set { this.toSideObjectAtrribute = value; }
        }

        private ObjectDataSpecs fromSideObjectAtrribute = new ObjectDataSpecs();

        public ObjectDataSpecs FromSideObjectAtrribute
        {
            get { return this.fromSideObjectAtrribute; }
            set { this.fromSideObjectAtrribute = value; }
        }


        private RelationshipDataSpecs relObjectSpec = new RelationshipDataSpecs();

        public RelationshipDataSpecs RelObjectSpec
        {
            get { return this.relObjectSpec; }
            set { this.relObjectSpec = value; }
        }

        private RelationshipNavigatorRequest drawingRel = new RelationshipNavigatorRequest();

        public RelationshipNavigatorRequest DrawingRel
        {
            get { return this.drawingRel; }
            set { this.drawingRel = value; }
        }

    }
}
