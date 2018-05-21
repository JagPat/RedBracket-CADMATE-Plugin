using System;
using System.Collections.Generic;
using ArasConnector.Exceptions;
using ArasConnector;
using System.Windows.Forms;
using CADController.Commands;
using AdvancedDataGridView;

namespace CADController.Controllers
{
   public class SearchController : BaseController
    {
       #region "public Methods"
       public SearchResult searchResult;
       
       public override void Execute(Command command)
       {
           int staticCount = 0;
           List<String> attribute = new List<string>();
           SearchCommand cmd = (SearchCommand)command;
           try
           {
               String[] plmobjInfo = new String[2];
               plmobjInfo =  cmd.PLMObjectInfo.Split(':');
               cmd.ItemObject.ObjectNumber = plmobjInfo[0];
               cmd.ItemObject.ObjectName = plmobjInfo[1];
               cmd.ItemObject.Classification = plmobjInfo[2];
               cmd.ItemObject.ObjectRevision = plmobjInfo[3];
               cmd.ItemObject.AuthoringTool = plmobjInfo[4];
               cmd.ItemObject.ItemType = plmobjInfo[5];
               cmd.ItemObject.ObjectProjectId = plmobjInfo[6];
               cmd.ItemObject.ObjectProjectName = plmobjInfo[7];
               cmd.ItemObject.ObjectState = plmobjInfo[8];
               cmd.ItemObject.ObjectRealtyId = plmobjInfo[9];
               cmd.ItemObject.ObjectDesktop = plmobjInfo[10];
               cmd.DrawingRel.RelName = cmd.RelationshipName;
               cmd.Relationship.Add(cmd.DrawingRel);
               cmd.ObjectDataInfo.Id = true;
               cmd.ObjectDataInfo.LockStatus = true;
               cmd.ObjectDataInfo.Name = true;
               cmd.ObjectDataInfo.Number = true;
               cmd.ObjectDataInfo.Revision = true;
               cmd.ObjectDataInfo.State = true;
               cmd.ObjectDataInfo.Generation = true;
               cmd.ObjectDataInfo.ConfigId = true;
               cmd.ObjectDataInfo.Project = true;
               cmd.ObjectDataInfo.ProjectId = true;
               cmd.ObjectDataInfo.State = true;
               cmd.ObjectDataInfo.SGOwnerCompany = true;
               attribute.Add("native_file");
               attribute.Add("classification");
               // cmd.ObjectDataInfo.Attributes.Add("native_file");
               //cmd.ObjectDataInfo.Attributes.Add("classification");
               cmd.ObjectDataInfo.Attributes = attribute;
               
               searchResult = (SearchResult)objConnector.SearchObject(cmd.ItemObject, cmd.LatestItem, cmd.ObjectDataInfo);
               
               if (!cmd.ExpandAll)
               {
               foreach (PLMObject pl in searchResult.ObjectList)
               {                   
                   pl.IsRel = "+";
                   TreeGridNode node = this.dtDocuments.Nodes.Add(false, pl.IsRel, pl.ObjectName, pl.ObjectNumber, pl.Classification, pl.ObjectRevision,pl.ObjectGeneration, pl.ObjectId, pl.LockStatus, pl.LockBy, pl.ObjectProjectId,pl.ObjectProjectName,pl.ObjectState,pl.ObjectOwnerCompany);
               }
               }
               else
               {
               foreach (PLMObject pl in searchResult.ObjectList)
               {                   
                   cmd.FromSideObjectAtrribute.Id = true;
                   cmd.FromSideObjectAtrribute.LockStatus = true;
                   cmd.FromSideObjectAtrribute.Name = true;
                   cmd.FromSideObjectAtrribute.Number = true;
                   cmd.FromSideObjectAtrribute.Project = true;
                   cmd.FromSideObjectAtrribute.ProjectId = true;
                   cmd.FromSideObjectAtrribute.State = true;
                   cmd.FromSideObjectAtrribute.Revision = true;
                   cmd.FromSideObjectAtrribute.State = true;
                   cmd.FromSideObjectAtrribute.SGOwnerCompany = true;
                   cmd.FromSideObjectAtrribute.Generation = true;
                   cmd.FromSideObjectAtrribute.ConfigId = true;
                   attribute.Add("native_file");
                   attribute.Add("classification");
                   cmd.FromSideObjectAtrribute.Attributes = attribute;
                                      
                       cmd.RelObjectSpec.FromObjectDataSpecs = cmd.FromSideObjectAtrribute;
                       PLMObject drawing = pl;        
                    
                       objConnector.ExpandObjectRecursively(ref drawing, cmd.Relationship, cmd.RelObjectSpec, staticCount);
                       drawing.IsRel = "+";                       
                       TreeGridNode node = this.dtDocuments.Nodes.Add(false, drawing.IsRel, drawing.ObjectName, drawing.ObjectNumber, drawing.Classification, drawing.ObjectRevision,drawing.ObjectGeneration, drawing.ObjectId, drawing.LockStatus, drawing.LockBy, drawing.ObjectProjectId,drawing.ObjectProjectName,drawing.ObjectState,drawing.ObjectOwnerCompany);
                       expandResursiveNode(drawing, node);
                       staticCount++;
                   }
               }

               return ;
           }
           catch (ConnectionException ex)
           {
               errorString = ex.Message;
               return ;
           }

       }

       void expandResursiveNode(PLMObject drawingObject, TreeGridNode treeNode)
       {

           foreach (Relationship rel in drawingObject.FromRelationships)
           {
               PLMObject pl = rel.ToObject;
               if (pl.FromRelationships.Count > 0)
               {
                   TreeGridNode node = treeNode.Nodes.Add(false, pl.IsRel, pl.ObjectName, pl.ObjectNumber, pl.Classification, pl.ObjectRevision,pl.ObjectGeneration, pl.ObjectId, pl.LockStatus, pl.LockBy,pl.ObjectProjectId, pl.ObjectProjectName,pl.ObjectState,pl.ObjectOwnerCompany);
                   expandResursiveNode(pl, node);
               }
               else
               {
                   treeNode.Nodes.Add(false, pl.IsRel, pl.ObjectName, pl.ObjectNumber, pl.Classification, pl.ObjectRevision,pl.ObjectGeneration, pl.ObjectId, pl.LockStatus, pl.LockBy,pl.ObjectProjectId, pl.ObjectProjectName, pl.ObjectState,pl.ObjectOwnerCompany);
               }
           }
       }
        #endregion
    }
}
