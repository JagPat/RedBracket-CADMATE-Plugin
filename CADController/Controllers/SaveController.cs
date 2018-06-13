using System;
using System.Collections.Generic;
using RedBracketConnector.Exceptions;
using RedBracketConnector;
using CADController.Commands;
using CADController.Controllers;
using System.Collections;
using System.Data;
using System.Windows.Forms;
using CADController;

namespace CADController.Controllers
{
    public class SaveController : BaseController
    {
        public DataTable dtDrawingProperty = new DataTable();
        RBConnector objRBC = new RBConnector();

        public override void Execute(Command command)
        {
        }
        public bool ExecuteSave(SaveCommand command)
        {
            try
            {
                dtDrawingProperty = new DataTable();
                bool IsNewStructure = false;
                bool IsCreateNewRevision = false;
                String ProjectName = "";
                String ProjectNo = "";
                String ProjectId = "";
                String ModifiedOn = "";
                String ModifiedBy = "";
                String CreatedOn = "";
                String CreatedBy = "";
                // creating table to store document info
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

                dtDrawingProperty.Columns.Add("canDelete");
                dtDrawingProperty.Columns.Add("isowner");
                dtDrawingProperty.Columns.Add("hasViewPermission");
                dtDrawingProperty.Columns.Add("isActFileLatest");
                dtDrawingProperty.Columns.Add("isEditable");
                dtDrawingProperty.Columns.Add("canEditStatus");
                dtDrawingProperty.Columns.Add("hasStatusClosed");
                dtDrawingProperty.Columns.Add("isletest");
                dtDrawingProperty.Columns.Add("projectno");
                dtDrawingProperty.Columns.Add("prefix");


                SaveCommand cmd = (SaveCommand)command;
                List<PLMObject> plmObjs = new List<PLMObject>();
                // gethering document info
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
                    String[] plmobjInfo = new String[22];
                    plmobjInfo = str.Split(';');
                    plmObj.ObjectId = plmobjInfo[0];
                    string fileName = System.IO.Path.GetFileName(plmobjInfo[2]);
                    plmObj.objectProjectNo = plmobjInfo[18];
                    ProjectName = plmobjInfo[8];
                    plmObj.ObjectRevision = plmobjInfo[17];
                    
                   
                   
                    plmObj.ObjectStatus = plmobjInfo[15];
                    plmObj.Classification = plmobjInfo[16];
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

                    plmObj.ObjectProjectId=ProjectId = plmobjInfo[9];
                    CreatedOn = plmobjInfo[10];
                    CreatedBy = plmobjInfo[11];
                    ModifiedOn = plmobjInfo[12];
                    ModifiedBy = plmobjInfo[13];
                    plmObj.ObjectLayouts = plmobjInfo[14];
                    plmObj.ObjectNumber = plmobjInfo[19];
                    plmObj.objectType= plmobjInfo[20];
                    try
                    {
                        string PreFix = plmobjInfo[21];
                        plmObj.PreFix = PreFix;
                        //if (ProjectName != "MyFiles")
                        //{
                        //    PreFix = plmObj.objectProjectNo + "-";
                        //}
                        //PreFix = PreFix + Convert.ToString(plmObj.ObjectNumber) + "-";

                        //PreFix += Convert.ToString(plmObj.objectType) == string.Empty ? string.Empty : Convert.ToString(plmObj.objectType) + "-";

                        //PreFix += Convert.ToString(plmObj.ObjectRevision) == string.Empty ? string.Empty : Convert.ToString(plmObj.ObjectRevision) + "#";
                        if (PreFix.Length <= fileName.Trim().Length)
                        {
                            if (fileName.Substring(0, PreFix.Length) == PreFix)
                            {
                                fileName = fileName.Substring(PreFix.Length);
                            }
                        }

                    }
                    catch
                    {

                    }
                    plmObj.ObjectName = fileName;
                }
                //if (cmd.Drawings.Count==0)
                {
                    foreach (String str in cmd.NewDrawings)
                    {
                        try
                        {
                            PLMObject plmObj = new PLMObject();
                            String[] plmobjInfo = new String[24];
                            plmobjInfo = str.Split(';');
                            plmObj.ObjectNumber = plmobjInfo[0];
                            plmObj.ObjectName = plmobjInfo[2];
                            plmObj.FilePath = plmobjInfo[3];
                            plmObj.ObjectRevision = plmobjInfo[20];
                            // plmObj.ObjectId = plmobjInfo[4];
                            //Needs to Change
                            plmObj.ObjectId = "";
                            plmObj.ItemType = plmobjInfo[5];
                            plmObj.ObjectProjectId = ProjectId = plmobjInfo[7];
                            plmObj.ObjectRealtyId = plmobjInfo[8];
                            plmObj.ObjectDescription = plmobjInfo[9];
                            plmObj.ObjectSourceId = plmobjInfo[10];
                            plmObj.AuthoringTool = "AutoCAD";
                            plmObj.IsCreateNewRevision = false;
                            plmObj.IsNew = true;
                            plmObj.ObjectStatus = plmobjInfo[18];
                            plmObj.Classification = plmobjInfo[19];
                            plmObj.IsNewStructure = IsNewStructure;

                            ProjectName = plmobjInfo[11];
                             
                            CreatedOn = plmobjInfo[13];
                            CreatedBy = plmobjInfo[14];
                            ModifiedOn = plmobjInfo[15];
                            ModifiedBy = plmobjInfo[16];
                            plmObj.ObjectLayouts = plmobjInfo[17];
                            plmObj.objectProjectNo = plmobjInfo[20];
                            plmObj.ObjectNumber = plmobjInfo[21];
                            plmObj.objectType = plmobjInfo[22];
                            string fileName = System.IO.Path.GetFileName(plmobjInfo[2]);
                            try
                            {
                                string PreFix = plmobjInfo[23];
                                plmObj.PreFix = PreFix;
                                //if (ProjectName != "MyFiles")
                                //{
                                //    PreFix = plmObj.objectProjectNo + "-";
                                //}
                                //PreFix = PreFix + Convert.ToString(plmObj.ObjectNumber) + "-";

                                //PreFix += Convert.ToString(plmObj.objectType) == string.Empty ? string.Empty : Convert.ToString(plmObj.objectType) + "-";

                                //PreFix += Convert.ToString(plmObj.ObjectRevision) == string.Empty ? string.Empty : Convert.ToString(plmObj.ObjectRevision) + "#";


                                if (PreFix.Length <= fileName.Trim().Length)
                                {
                                    if (fileName.Substring(0, PreFix.Length) == PreFix)
                                    {
                                        fileName = fileName.Substring(PreFix.Length);
                                    }
                                }
                            }
                            catch (System.Exception E)
                            {

                            }
                            plmObj.ObjectName = fileName;
                    
                            plmObj.IsRoot = false;
                            if (plmobjInfo[6] == "1")
                                plmObj.IsRoot = true;
                            if (plmObj.IsRoot)
                                plmObjs.Insert(0, plmObj);
                            else
                                plmObjs.Add(plmObj);
                        }
                        catch (System.Exception E)
                        {

                        }
                    }
                }


                //  objConnector.SaveObject(ref plmObjs);
                bool RetVal = ObjRBC.SaveObject(ref plmObjs);

                try
                {
                    // updating document info
                    foreach (PLMObject plmobj in plmObjs)
                    {
                        //dtDrawingProperty.Rows.Clear();
                        dtDrawingProperty.Rows.Add(plmobj.ObjectId, plmobj.ObjectName, plmobj.objectType, plmobj.ObjectNumber, plmobj.ObjectState,
                            plmobj.ObjectRevision, plmobj.ObjectGeneration, plmobj.ItemType, plmobj.FilePath, plmobj.IsRoot, ProjectName, ProjectId,
                            CreatedOn, CreatedBy, ModifiedOn, ModifiedBy,"",plmobj.ObjectLayouts
                            , plmobj.canDelete
                            , plmobj.isowner
                            , plmobj.hasViewPermission
                            , plmobj.isActFileLatest
                            , plmobj.isEditable
                            , plmobj.canEditStatus
                            , plmobj.hasStatusClosed
                            , plmobj.isletest
                            , plmobj.objectProjectNo
                            , plmobj.PreFix);
                    }
                }
                catch (System.Exception E)
                {

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
                // objConnector.LockStatus(ref dtNewPlmObjInfomation);
                ObjRBC.LockStatus(ref dtNewPlmObjInfomation);
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
