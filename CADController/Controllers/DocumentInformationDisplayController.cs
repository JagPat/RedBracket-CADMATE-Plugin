using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CADController.Controllers;
using CADController.Commands;
using ArasConnector;
using System.Windows.Forms;
using System.Data;
namespace CADController.Controllers
{
    public class DocumentInformationDisplayController : BaseController
    {
        public override void Execute(Command command)
        {
            dtNewPlmObjInfomation.Columns.Add("drawingnumber");
              dtNewPlmObjInfomation.Columns.Add("LatestRevision");
              dtNewPlmObjInfomation.Columns.Add("LatestGeneration");
              dtNewPlmObjInfomation.Columns.Add("LatestState");
              dtNewPlmObjInfomation.Columns.Add("LockStatus");
              dtNewPlmObjInfomation.Columns.Add("projectname");
              dtNewPlmObjInfomation.Columns.Add("projectid");
            Hashtable htlatestDocProperty = new Hashtable();
            DocumentInformationDisplayCommand cmd = (DocumentInformationDisplayCommand) command;
            List<PLMObject> plmobjs = new List<PLMObject>();
            List<PLMObject> newplmobj = new List<PLMObject>();
            foreach (String str in cmd.DarawingInformation)
            {
                PLMObject plmobj = new PLMObject();
                String[] drawingInfo = new String[2];
                drawingInfo = str.Split(':');
                plmobj.ObjectNumber = drawingInfo[0];
                plmobj.ItemType = drawingInfo[1];
                plmobjs.Add(plmobj);
                
            }
            newplmobj = objConnector.GetPLMObjectInformation(plmobjs);
            foreach (PLMObject obj in newplmobj)
            {
                dtNewPlmObjInfomation.Rows.Add(obj.ObjectNumber,obj.ObjectRevision,obj.ObjectGeneration,obj.ObjectState,obj.LockStatus,obj.ObjectProjectId, obj.ObjectProjectName);
            }







        }
    }
}
