using System;
using System.Collections.Generic;
using ArasConnector;
using System.Windows.Forms;



namespace CADController.Commands
{
    public class SearchCommand : Command
    {
       
       private string latestItem;
       private ObjectDataSpecs objectDataInfo = new ObjectDataSpecs();
       private PLMObject itemObject = new PLMObject();
       private List<RelationshipNavigatorRequest> relationship = new List<RelationshipNavigatorRequest>();       
       private bool expandAll;
       private String plmObjectInfo;
       private String relationshipName;
       private RelationshipNavigatorRequest drawingRel = new RelationshipNavigatorRequest();


       public String RelationshipName
       {
           get { return this.relationshipName; }
           set { this.relationshipName = value; }
       }


       public String PLMObjectInfo
       {
           get { return this.plmObjectInfo; }
           set { this.plmObjectInfo = value; }
       }



       public bool ExpandAll
       {
           get { return this.expandAll; }
           set { this.expandAll = value; }
       }

      public RelationshipNavigatorRequest DrawingRel
       {
           get { return this.drawingRel; }
           set { this.drawingRel = value; }
       } 

       public List<RelationshipNavigatorRequest> Relationship
       {
           get { return this.relationship; }
           set { this.relationship = value; }
       }

       public PLMObject ItemObject
        {
            get { return this.itemObject; }
            set { this.itemObject = value; }
        }
       

       public string LatestItem
        {
            get { return this.latestItem; }
            set { this.latestItem = value; }
        }
        public ObjectDataSpecs ObjectDataInfo
        {
            get { return this.objectDataInfo; }
            set { this.objectDataInfo = value; }
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
    }
}
