using System;
using System.Collections.Generic;
using System.Collections;
namespace RBAutocadPlugIn
{
    public class LockController : BaseController
    {

        public override void Execute(Command command)
        {
            LockCommand cmd = (LockCommand)command;
             
            try
            {
                List<PLMObject> drawingObjects = new List<PLMObject>();
                foreach (String drawingId in cmd.DrawingIds)
                {
                    PLMObject drawing = new PLMObject();

                    drawing.ObjectId = drawingId;
                    drawing.ItemType = "CAD";

                    drawingObjects.Add(drawing);
                }
                Helper.objRBC.LockObject(drawingObjects);
            }
            catch (Exception ex)
            {
                errorString = ex.Message.ToString();
                return;
            }
        }

        public System.Data.DataTable getLockStatus(Command command)
        {
            try
            {
                 
                LockCommand cmd = (LockCommand)command;
                dtNewPlmObjInfomation = cmd.DrawingInfo;
                
                Helper.objRBC.LockStatus(ref dtNewPlmObjInfomation);
                return dtNewPlmObjInfomation;
            }
            catch (Exception ex)
            {
                errorString = ex.Message.ToString();
                return null;
            }
        }
    }
}
