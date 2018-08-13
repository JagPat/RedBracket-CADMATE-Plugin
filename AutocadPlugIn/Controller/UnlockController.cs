using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
  
 
using System.Windows.Forms;


namespace AutocadPlugIn
{
    public class UnlockController : BaseController
    {
        public override void Execute(Command command)
        {
            UnlockCommand cmd = (UnlockCommand)command;
           
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

                // objConnector.UnlockObject(drawingObjects);
                Helper.objRBC.UnlockObject(drawingObjects);
            }
            catch ( Exception ex)
            {
                errorString = ex.Message.ToString();
            }
        }

        public System.Data.DataTable getLockStatus(Command command)
        {
            try
            {
                 
                UnlockCommand cmd = (UnlockCommand)command;
                dtNewPlmObjInfomation = cmd.DrawingInfo;
                //objConnector.LockStatus(ref dtNewPlmObjInfomation);
                Helper.objRBC.LockStatus(ref dtNewPlmObjInfomation);
                return dtNewPlmObjInfomation;
            }
            catch ( Exception ex)
            {
                errorString = ex.Message.ToString();
                return null;
            }
        }
    }
}
