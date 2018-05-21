using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CADController.Commands;
using ArasConnector.Exceptions;
using ArasConnector;
using AdvancedDataGridView;


namespace CADController.Controllers
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

                cmd.DrawingRel.RelName = cmd.RelationshipName;
                cmd.Relationship.Add(cmd.DrawingRel);
              //  searchResult = (SearchResult)
               // drawingRel.RelName = cmd.RelationshipName;
             //   relationship.Add(drawingRel);

                cmd.FromSideObjectAtrribute.Id = true;
                cmd.FromSideObjectAtrribute.LockStatus = true;
                cmd.FromSideObjectAtrribute.Name = true;
                cmd.FromSideObjectAtrribute.Number = true;
                cmd.FromSideObjectAtrribute.Revision = true;
                cmd.FromSideObjectAtrribute.State = true;
                cmd.FromSideObjectAtrribute.Generation = true;
                cmd.FromSideObjectAtrribute.Project = true;
                attribute.Add("native_file");
                attribute.Add("classification");
                cmd.FromSideObjectAtrribute.Attributes = attribute;


                cmd.RelObjectSpec.FromObjectDataSpecs = cmd.FromSideObjectAtrribute;

                objConnector.ExpandObject(ref plmobject, cmd.Relationship, cmd.RelObjectSpec, expandObjcount);

                expandObjcount++;
                foreach (Relationship rel in plmobject.FromRelationships)
                {

                   PLMObject pl = rel.ToObject;
                   pl.IsRel = "+";
                   TreeGridNode node = this.dtDocuments.Nodes.Add(false, pl.IsRel, pl.ObjectName, pl.ObjectNumber, pl.Classification, pl.ObjectRevision, pl.ObjectId, pl.LockStatus, pl.LockBy,pl.ObjectProjectId);

                  
                }
                return;
            }
            catch (ConnectionException ex)
            {
                errorString = ex.Message;
                return;
            }

        }


       
        #endregion
    }
}
