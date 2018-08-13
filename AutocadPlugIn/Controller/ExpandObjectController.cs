using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using AdvancedDataGridView;


namespace RBAutocadPlugIn
{
    public class ExpandObjectController : BaseController
    {
        #region "public Methods"
        List<String> attribute = new List<string>();
       

        public override void Execute(Command command)
        {
            int expandObjcount = 0;    

            ExpandObjectCommand cmd = (ExpandObjectCommand)command;
            try
            {
                PLMObject plmobject = new PLMObject();

                String[] plmobjInfo = new String[2];
                plmobjInfo = cmd.PLMObjectInfo.Split(':');

                plmobject.ObjectId = plmobjInfo[0];
                plmobject.ItemType = plmobjInfo[1];
 

             

                expandObjcount++;
                foreach (Relationship rel in plmobject.FromRelationships)
                {

                   PLMObject pl = rel.ToObject;
                   pl.IsRel = "+";
                   TreeGridNode node = this.dtDocuments.Nodes.Add(false, pl.IsRel, pl.ObjectName, pl.ObjectNumber, pl.Classification, pl.ObjectRevision, pl.ObjectId, pl.LockStatus, pl.LockBy,pl.ObjectProjectId);

                  
                }
                return;
            }
            catch (Exception ex)
            {
                errorString = ex.Message;
                return;
            }

        }


       
        #endregion
    }
}
