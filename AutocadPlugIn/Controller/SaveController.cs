using System;
using System.Collections.Generic;
 
using System.Collections;
using System.Data; 
namespace AutocadPlugIn
{
    public class SaveController : BaseController
    {
        public DataTable dtDrawingProperty = new DataTable();
        RBConnector objRBC = new RBConnector();
        String ProjectName = "";
        String ProjectNo = "";
        String ProjectId = "";
        String ModifiedOn = "";
        String ModifiedBy = "";
        String CreatedOn = "";
        String CreatedBy = "";
        public List<PLMObject> plmObjs = new List<PLMObject>();
        public override void Execute(Command command)
        {
        }
        public bool ExecuteSave(SaveCommand command, bool IsFileSave = false, List<DataTable> lstdtLayoutInfo = null)
        {
            try
            {

                if (!IsFileSave)
                {

                    plmObjs = new List<PLMObject>();
                    dtDrawingProperty = new DataTable();
                    bool IsNewStructure = false;
                    bool IsCreateNewRevision = false;

                    // creating table to store document info
                    dtDrawingProperty = Helper.GetDrawingPropertiesTableStructure();

                    SaveCommand cmd = (SaveCommand)command;

                    // gethering document info
                     
                    foreach (String str in cmd.Drawings)
                    {
                        PLMObject plmObj = new PLMObject();
                        String[] plmobjInfo = new String[24];
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
                        
                        plmObj.IsNew = false;
                        plmObj.IsRoot = false;
                        if (plmobjInfo[3] == "true")
                            plmObj.IsRoot = true;
                        
                        if (plmObj.IsRoot)
                            plmObjs.Insert(0, plmObj);
                        else
                            plmObjs.Add(plmObj);
                        IsCreateNewRevision = false;
                        plmObj.ObjectDescription = plmobjInfo[6];
                        plmObj.ObjectSourceId = plmobjInfo[7];

                        plmObj.ObjectProjectId = ProjectId = plmobjInfo[9];
                        CreatedOn = plmobjInfo[10];
                        CreatedBy = plmobjInfo[11];
                        ModifiedOn = plmobjInfo[12];
                        ModifiedBy = plmobjInfo[13];
                        plmObj.ObjectLayouts = plmobjInfo[14];
                        plmObj.ObjectNumber = plmobjInfo[19];
                        plmObj.objectType = plmobjInfo[20];
                        plmObj.FolderID = plmobjInfo[22];
                        plmObj.FolderPath = plmobjInfo[23];
                        plmObj.PreFix = plmobjInfo[21];
                        fileName = Helper.RemovePreFixFromFileName(fileName, plmObj.PreFix);
                        plmObj.ObjectName = fileName;

                        bool IsNotFound = true;
                        foreach (DataTable dt in lstdtLayoutInfo)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if ( Convert.ToString(dt.Rows[0]["FileID1"]) == plmObj.ObjectId)
                                {
                                    plmObj.dtLayoutInfo = dt;
                                    IsNotFound = false;
                                    break;
                                }
                            }

                        }
                        if (IsNotFound)
                        {
                            plmObj.dtLayoutInfo = new DataTable();
                        }
                    }
                    //if (cmd.Drawings.Count==0)
                    {
                        foreach (String str in cmd.NewDrawings)
                        {
                            try
                            {
                                PLMObject plmObj = new PLMObject();
                                String[] plmobjInfo = new String[26];
                                plmobjInfo = str.Split(';');
                                plmObj.ObjectNumber = plmobjInfo[0];
                                plmObj.ObjectName = plmobjInfo[2];
                                plmObj.FilePath = plmobjInfo[3];
                                plmObj.ObjectRevision = plmobjInfo[20];
                                // plmObj.ObjectId = plmobjInfo[4];
                                //Needs to Change
                                plmObj.ObjectId = "";
                                plmObj.ItemType = plmobjInfo[5];
                                plmObj.ObjectProjectId = ProjectId = plmobjInfo[12];
                         
                                plmObj.ObjectDescription = plmobjInfo[9];
                                plmObj.ObjectSourceId = plmobjInfo[10];
                                plmObj.AuthoringTool = "AutoCAD";
                           
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
                                plmObj.objectProjectNo = plmobjInfo[21];
                                plmObj.ObjectNumber = plmobjInfo[22];
                                plmObj.objectType = plmobjInfo[23];
                                plmObj.FolderID = plmobjInfo[25];
                                plmObj.FolderPath = plmobjInfo[26];
                                string fileName = System.IO.Path.GetFileName(plmobjInfo[2]);
                                plmObj.PreFix = plmobjInfo[24];
                                fileName = Helper.RemovePreFixFromFileName(fileName, plmObj.PreFix);
                                plmObj.ObjectName = fileName;

                                plmObj.IsRoot = false;
                                if (plmobjInfo[6] == "true")
                                    plmObj.IsRoot = true;


                                bool IsNotFound = true;
                                foreach (DataTable dt in lstdtLayoutInfo)
                                {
                                    if (dt.Rows.Count > 0)
                                    {
                                        if (System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(dt.Rows[0]["FileLayoutName"])) == fileName)
                                        {
                                            plmObj.dtLayoutInfo = dt;
                                            IsNotFound = false;
                                            break;
                                        }
                                    }

                                }
                                if (IsNotFound)
                                {
                                    plmObj.dtLayoutInfo = new DataTable();
                                }


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
                }

                //  objConnector.SaveObject(ref plmObjs);
                bool RetVal = ObjRBC.SaveObject(ref plmObjs, IsFileSave);

                try
                {
                    if (!IsFileSave&& RetVal)
                    {
                        dtDrawingProperty.Rows.Clear();
                        // updating document info
                        foreach (PLMObject plmobj in plmObjs)
                        {
                            string PreFix = Helper.GetPreFix(plmobj.ObjectRevision, plmobj.objectProjectNo, plmobj.ObjectNumber, plmobj.objectType);

                       

                            string OldPrefix = plmobj.PreFix;
                            plmobj.PreFix = PreFix;
                            
                             //dtDrawingProperty.Rows.Clear();
                            dtDrawingProperty.Rows.Add(plmobj.ObjectId,
                                plmobj.ObjectName,
                                plmobj.objectType,
                                plmobj.ObjectNumber,
                                plmobj.ObjectState,
                                plmobj.ObjectRevision,
                                plmobj.ObjectGeneration,
                                plmobj.ItemType,
                                plmobj.FilePath,
                                plmobj.IsRoot,
                                plmobj.ObjectProjectName,
                                ProjectName,

                                ProjectId,
                                plmobj.ObjectCreatedOn,
                                plmobj.ObjectCreatedById,
                                plmobj.ObjectModifiedOn,
                                plmobj.ObjectModifiedById, ""
                                , plmobj.ObjectLayouts,
                                "",""
                                , plmobj.canDelete
                                , plmobj.isowner
                                , plmobj.hasViewPermission
                                , plmobj.isActFileLatest
                                , plmobj.isEditable
                                , plmobj.canEditStatus
                                , plmobj.hasStatusClosed
                                , plmobj.isletest
                                , plmobj.objectProjectNo
                                , PreFix
                                , plmobj.LayoutInfo,
                                OldPrefix);
                        }
                    }

                }
                catch (System.Exception E)
                {

                }

                return RetVal;
            }
            catch ( Exception ex)
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
            catch ( Exception ex)
            {
                errorString = ex.Message.ToString();
                return null;
            }


        }
    }
}
