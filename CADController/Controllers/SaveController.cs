using System;
using System.Collections.Generic;
using RedBracketConnector.Exceptions;
using RedBracketConnector;
using CADController.Commands;
using CADController.Controllers;
using System.Collections;
using System.Data;
using System.Windows.Forms;
namespace CADController.Controllers
{
    public class SaveController : BaseController
    {
        public DataTable dtDrawingProperty = new DataTable();
        public override void Execute(Command command)
        {
        }
        public bool ExecuteSave(SaveCommand command)
        {
            try
            {
                bool IsNewStructure = false;
                bool IsCreateNewRevision = false;
                String ProjectName = "";
                String ProjectId = "";
                String ModifiedOn = "";
                String ModifiedBy = "";
                String CreatedOn = "";
                String CreatedBy = "";
                dtDrawingProperty.Columns.Add("DrawingId");
                dtDrawingProperty.Columns.Add("DrawingName");
                dtDrawingProperty.Columns.Add("Classification");
                dtDrawingProperty.Columns.Add("DrawingNumber");
                dtDrawingProperty.Columns.Add("DrawingState");
                dtDrawingProperty.Columns.Add("Revision");
                dtDrawingProperty.Columns.Add("Generation");
                dtDrawingProperty.Columns.Add("Type");
                dtDrawingProperty.Columns.Add("filepath");
                dtDrawingProperty.Columns.Add("isroot", typeof(bool));
                dtDrawingProperty.Columns.Add("ProjectName");
                dtDrawingProperty.Columns.Add("ProjectId");
                dtDrawingProperty.Columns.Add("createdon");
                dtDrawingProperty.Columns.Add("createdby");
                dtDrawingProperty.Columns.Add("modifiedon");
                dtDrawingProperty.Columns.Add("modifiedby");
                dtDrawingProperty.Columns.Add("sourceid");
                dtDrawingProperty.Columns.Add("Layouts");

                SaveCommand cmd = (SaveCommand)command;
                List<PLMObject> plmObjs = new List<PLMObject>();
                foreach (String str in cmd.NewDrawings)
                {
                    String[] plmobjInfo = new String[18];
                    plmobjInfo = str.Split(';');
                    if (plmobjInfo[5] == "")
                        IsNewStructure = true;
                    break;
                }
                foreach (String str in cmd.Drawings)
                {
                    PLMObject plmObj = new PLMObject();
                    String[] plmobjInfo = new String[15];
                    plmobjInfo = str.Split(';');
                    plmObj.ObjectId = plmobjInfo[0];
                    plmObj.ItemType = plmobjInfo[1];
                    plmObj.FilePath = plmobjInfo[2];
                    plmObj.IsManualVersion = false;
                    if (plmobjInfo[5].ToString() == "True")
                        plmObj.IsManualVersion = true;
                    if (plmobjInfo[4] == "Next")
                        IsCreateNewRevision = true;
                    plmObj.IsCreateNewRevision = IsCreateNewRevision;
                    plmObj.IsNew = false;
                    plmObj.IsRoot = false;
                    if (plmobjInfo[3] == "1")
                        plmObj.IsRoot = true;
                    plmObj.IsNewStructure = IsNewStructure;
                    if (plmObj.IsRoot)
                        plmObjs.Insert(0, plmObj);
                    else
                        plmObjs.Add(plmObj);
                    IsCreateNewRevision = false;
                    plmObj.ObjectDescription = plmobjInfo[6];
                    plmObj.ObjectSourceId = plmobjInfo[7];
                    ProjectName = plmobjInfo[8];
                    ProjectId = plmobjInfo[9];
                    CreatedOn = plmobjInfo[10];
                    CreatedBy = plmobjInfo[11];
                    ModifiedOn = plmobjInfo[12];
                    ModifiedBy = plmobjInfo[13];
                    plmObj.ObjectLayouts = plmobjInfo[14];
                }
                foreach (String str in cmd.NewDrawings)
                {
                    PLMObject plmObj = new PLMObject();
                    String[] plmobjInfo = new String[18];
                    plmobjInfo = str.Split(';');
                    plmObj.ObjectNumber = plmobjInfo[0];
                    plmObj.Classification = plmobjInfo[1];
                    plmObj.ObjectName = plmobjInfo[2];
                    plmObj.FilePath = plmobjInfo[3];
                    plmObj.ObjectId = plmobjInfo[4];
                    plmObj.ItemType = plmobjInfo[5];
                    plmObj.ObjectProjectId = plmobjInfo[7];
                    plmObj.ObjectRealtyId = plmobjInfo[8];
                    plmObj.ObjectDescription = plmobjInfo[9];
                    plmObj.ObjectSourceId = plmobjInfo[10];
                    plmObj.AuthoringTool = "AutoCAD";
                    plmObj.IsCreateNewRevision = false;
                    plmObj.IsNew = true;
                    plmObj.IsRoot = false;
                    if (plmobjInfo[6] == "1")
                        plmObj.IsRoot = true;
                    plmObj.IsNewStructure = IsNewStructure;
                    if (plmObj.IsRoot)
                        plmObjs.Insert(0, plmObj);
                    else
                        plmObjs.Add(plmObj);
                    ProjectName = plmobjInfo[11];
                    ProjectId = plmobjInfo[12];
                    CreatedOn = plmobjInfo[13];
                    CreatedBy = plmobjInfo[14];
                    ModifiedOn = plmobjInfo[15];
                    ModifiedBy = plmobjInfo[16];
                    plmObj.ObjectLayouts = plmobjInfo[17];
                }
                //  objConnector.SaveObject(ref plmObjs);
              bool RetVal=  ObjRBC.SaveObject(ref plmObjs, command.FilePath);
                foreach (PLMObject plmobj in plmObjs)
                {
                    dtDrawingProperty.Rows.Add(plmobj.ObjectId, plmobj.ObjectName, plmobj.Classification, plmobj.ObjectNumber, plmobj.ObjectState, plmobj.ObjectRevision, plmobj.ObjectGeneration, plmobj.ItemType, plmobj.FilePath, plmobj.IsRoot, ProjectName, ProjectId, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy);
                }
                return RetVal;
            }
            catch (ConnectionException ex)
            {
                ShowMessage.ErrorMess(ex.Message);
                return false;
            }
        }

        public System.Data.DataTable getLockStatus(Command command)
        {
            try
            {
                SaveCommand cmd = (SaveCommand)command;
                dtNewPlmObjInfomation = cmd.DrawingInfo;
                objConnector.LockStatus(ref dtNewPlmObjInfomation);
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
