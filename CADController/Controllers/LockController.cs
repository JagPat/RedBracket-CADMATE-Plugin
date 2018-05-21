using System;
using System.Collections.Generic;
using System.Collections;
using CADController.Commands;
using CADController.Controllers;
using ArasConnector.Exceptions;
using RedBracketConnector;
namespace CADController.Controllers
{
    public class LockController : BaseController
    {

        public override void Execute(Command command)
        {
            LockCommand cmd = (LockCommand)command;
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
                lockUnLockUpdater.LockObject(drawingObjects);
                // objConnector.LockObject(drawingObjects);
            }
            catch (ConnectionException ex)
            {
                errorString = ex.Message.ToString();
                return;
            }
        }

        public System.Data.DataTable getLockStatus(Command command)
        {
            try
            {
                LockUnLockUpdater lockUnLockUpdater = new LockUnLockUpdater();
                LockCommand cmd = (LockCommand)command;
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
