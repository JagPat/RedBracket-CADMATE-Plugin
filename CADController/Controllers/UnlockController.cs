using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using RedBracketConnector.Exceptions;
using RedBracketConnector;
using CADController.Commands;
using CADController.Controllers;
using System.Windows.Forms;


namespace CADController.Controllers
{
    public class UnlockController : BaseController
    {
        public override void Execute(Command command)
        {
            UnlockCommand cmd = (UnlockCommand)command;
            LockUnLockUpdater lockUnLockUpdater = new LockUnLockUpdater();
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
                lockUnLockUpdater.UnlockObject(drawingObjects);
            }
            catch (ConnectionException ex)
            {
                errorString = ex.Message.ToString();
            }
        }

        public System.Data.DataTable getLockStatus(Command command)
        {
            try
            {
                LockUnLockUpdater lockUnLockUpdater = new LockUnLockUpdater();
                UnlockCommand cmd = (UnlockCommand)command;
                dtNewPlmObjInfomation = cmd.DrawingInfo;
                //objConnector.LockStatus(ref dtNewPlmObjInfomation);
                lockUnLockUpdater.LockStatus(ref dtNewPlmObjInfomation);
                return dtNewPlmObjInfomation;
            }
            catch (ConnectionException ex)
            {
                errorString = ex.Message.ToString();
                return null;
            }
        }
    }
}
