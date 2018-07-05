using System;
using System.Collections.Generic;
 
using System.Windows.Forms;



namespace AutocadPlugIn
{
    public class SearchCommand : Command
    {
       
       private string latestItem;
 
       private PLMObject itemObject = new PLMObject();
           
       private bool expandAll;
       private String plmObjectInfo;
       private String relationshipName;
       


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
        
    }
}
