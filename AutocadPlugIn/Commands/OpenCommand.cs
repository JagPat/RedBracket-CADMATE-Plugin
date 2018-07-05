using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data; 

namespace AutocadPlugIn
{
    public class OpenCommand : Command
    {
    
        private DataTable itemInfo = new DataTable();

        public OpenCommand()
        {
            itemInfo.Columns.Add("DrawingId", typeof(String));
            itemInfo.Columns.Add("NativeFileName", typeof(String));
            itemInfo.Columns.Add("DrawingName", typeof(String));
            itemInfo.Columns.Add("Classification", typeof(String));
            itemInfo.Columns.Add("DrawingNumber", typeof(String));
            itemInfo.Columns.Add("DrawingState", typeof(String));
            itemInfo.Columns.Add("Revision", typeof(String));
            itemInfo.Columns.Add("LockStatus", typeof(String));
            itemInfo.Columns.Add("Generation", typeof(String));
            itemInfo.Columns.Add("Type", typeof(String));
            itemInfo.Columns.Add("IsFile", typeof(Boolean));           
            itemInfo.Columns.Add("LockBy", typeof(String)); 
            itemInfo.Columns.Add("Error", typeof(String));
            itemInfo.Columns.Add("ProjectName", typeof(String));
            itemInfo.Columns.Add("ProjectId", typeof(String));
            itemInfo.Columns.Add("CreatedOn", typeof(String));
            itemInfo.Columns.Add("CreatedBy", typeof(String));
            itemInfo.Columns.Add("ModifiedOn", typeof(String));
            itemInfo.Columns.Add("ModifiedBy", typeof(String));                  
        }

        public DataTable ItemInfo
        {
            get { return this.itemInfo; }
            set { this.itemInfo = value; }
        }

        private PLMObject itemObject = new PLMObject();
        public PLMObject ItemObject
        {
            get { return this.itemObject; }
            set { this.itemObject = value; }
        }
     
        private string itemType;
        public string ItemType
        {
            get { return this.itemType; }
            set { this.itemType = value; }
        }

        private string itemId;
        public string ItemId
        {
            get { return this.itemId; }
            set { this.itemId = value; }
        }
        
        private String ckeckOutPath;
        public String CkeckOutPath
        {
            get { return this.ckeckOutPath; }
            set { this.ckeckOutPath = value; }
        }


    

    }
}
