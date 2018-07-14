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


        String ProjectId = "";

        public List<PLMObject> plmObjs = new List<PLMObject>();
        public List<PLMObject> AllplmObjs = new List<PLMObject>();
        public List<PLMObject> TempplmObjs = new List<PLMObject>();
        public override void Execute(Command command)
        {
        }
        public bool ExecuteSave(SaveCommand command, bool IsFileSave = false, List<DataTable> lstdtLayoutInfo = null)
        {
            try
            {

                if (!IsFileSave)
                {
                    //Helper.CloseProgressBar();

                    plmObjs = new List<PLMObject>();
                    dtDrawingProperty = new DataTable();
                    // creating table to store document info
                    dtDrawingProperty = Helper.GetDrawingPropertiesTableStructure();

                    SaveCommand cmd = (SaveCommand)command;

                    foreach (String str in cmd.AllDrawing)
                    {
                        PLMObject plmObj = new PLMObject();
                        String[] plmobjInfo = null;
                        plmobjInfo = str.Split(';');


                        plmObj.ObjectId = plmobjInfo[5];
                        string fileName = System.IO.Path.GetFileName(plmobjInfo[6]);
                        plmObj.ObjectProjectName = plmobjInfo[34];
                        plmObj.objectProjectNo = plmobjInfo[31];
                        plmObj.ObjectStatus = plmobjInfo[29];
                        plmObj.Classification = plmobjInfo[30];
                       plmObj.Oldfilepath= plmObj.FilePath = plmobjInfo[6];
                        plmObj.IsNew = false;
                        if (plmObj.ObjectId.Trim().Length == 0)
                        {
                            plmObj.IsNew = true;
                        }

                        plmObj.IsRoot = false;
                        if (plmobjInfo[10] == "true")
                            plmObj.IsRoot = true;

                        if (plmObj.IsRoot)
                            plmObjs.Insert(0, plmObj);
                        else
                            plmObjs.Add(plmObj);

                        if (plmObj.IsRoot)
                            plmObj.ObjectDescription = plmobjInfo[plmobjInfo.Length - 2];


                        plmObj.ObjectProjectId = ProjectId = plmobjInfo[28];
                        plmObj.ObjectLayouts = plmobjInfo[11];
                        plmObj.FolderID = plmobjInfo[25];
                        plmObj.FolderPath = plmobjInfo[24];
                        if (!plmObj.IsRoot)
                            plmObj.IsNewXref = plmobjInfo[27].Contains("true") || plmobjInfo[27].Contains("true") ? Convert.ToBoolean(plmobjInfo[27]) : false;
                        plmObj.PreFix = plmobjInfo[22];
                        fileName = Helper.RemovePreFixFromFileName(fileName, plmObj.PreFix);
                        plmObj.ObjectName = fileName;
                        plmObj.OldPK = plmObj.PK = plmobjInfo[32];
                        plmObj.OldFK = plmObj.FK = plmobjInfo[33];
                        plmObj.IsSaved = false;
                        //return false;

                        bool IsNotFound = true;
                        foreach (DataTable dt in lstdtLayoutInfo)
                        {
                            if (dt.Rows.Count > 0)
                            {
                                if (plmObj.IsNew && Convert.ToString(dt.Rows[0]["FileID1"]) == plmObj.ObjectId)
                                {
                                    IsNotFound = false;
                                }
                                else if (System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(dt.Rows[0]["FileLayoutName"])) == System.IO.Path.GetFileNameWithoutExtension(fileName))
                                {
                                    IsNotFound = false;
                                }
                                else
                                {
                                    continue;
                                }

                                if (!IsNotFound)
                                {
                                    plmObj.dtLayoutInfo = dt;
                                    break;
                                }

                            }

                        }
                        if (IsNotFound)
                        {
                            plmObj.dtLayoutInfo = new DataTable();
                        }
                    }
                    //Helper.CloseProgressBar();
                    for (int i = 0; i < plmObjs.Count; ++i)
                    {
                        PLMObject obj = plmObjs[i];
                        bool IsNotFound = true;
                        for (int j = 0; j < TempplmObjs.Count; ++j)
                        {
                            PLMObject obj1 = plmObjs[j];
                            if (obj.FilePath == obj1.FilePath)
                            {
                                IsNotFound = false;
                            }
                        }
                        if (IsNotFound)
                        {
                            TempplmObjs.Add(obj);
                        }
                    }
                    AllplmObjs = plmObjs;
                    plmObjs = TempplmObjs;
                }
                //Helper.CloseProgressBar();

                bool RetVal = ObjRBC.SaveObject(ref plmObjs, IsFileSave);

                try
                {
                    if (!IsFileSave && RetVal)
                    {
                        dtDrawingProperty.Rows.Clear();
                        // updating document info
                        foreach (PLMObject plmobj in plmObjs)
                        {
                            string PreFix = Helper.GetPreFix(plmobj.ObjectRevision, plmobj.objectProjectNo, plmobj.ObjectNumber, plmobj.objectType);



                            string OldPrefix = plmobj.PreFix;
                            plmobj.PreFix = PreFix;
                            plmobj.objectProjectNo = plmobj.objectProjectNo == null ? string.Empty : plmobj.objectProjectNo;
                            //dtDrawingProperty.Rows.Clear();
                            dtDrawingProperty.Rows.Add(plmobj.ObjectId,
                                plmobj.ObjectName,
                                plmobj.objectType,
                                plmobj.ObjectNumber,
                                plmobj.ObjectState,
                                plmobj.ObjectRevision,
                                plmobj.ObjectGeneration,
                                "CAD",
                                plmobj.FilePath,
                                plmobj.IsRoot,
                                plmobj.ObjectProjectName,
                                plmobj.objectProjectNo.Trim().Length > 0 ? plmobj.ObjectProjectName + " (" + plmobj.objectProjectNo + ")" : "My Files",

                                ProjectId,
                                plmobj.ObjectCreatedOn,
                                plmobj.ObjectCreatedById,
                                plmobj.ObjectModifiedOn,
                                plmobj.ObjectModifiedById, ""
                                , plmobj.ObjectLayouts,
                                "", ""
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
                                OldPrefix
                                , plmobj.FolderID
                                , plmobj.FolderPath
                                , "" //IsNewXref  not to assign value from here, if ever assign assign fasle.
                                , "true"//Always true
                                , plmobj.PK//PK
                                , plmobj.FK //FK
                                );
                        }
                    }
                    else
                    {
                        if (RetVal)
                        {
                            //TempplmObjs = AllplmObjs;
                            //AllplmObjs = TempplmObjs;
                             //  Helper.CloseProgressBar();
                            
                            for (int i = 0; i < AllplmObjs.Count; ++i)
                            {
                                PLMObject obj = AllplmObjs[i];
                                if (!obj.IsSaved)
                                {
                                    for (int j = 0; j < AllplmObjs.Count; ++j)
                                    {
                                        if (i != j)
                                        {
                                            PLMObject obj1 = AllplmObjs[j];
                                            if (obj.FilePath == obj1.Oldfilepath)
                                            {
                                                obj.PK = obj1.PK;
                                            }
                                            if (obj.FK == obj1.OldPK)
                                            {
                                                obj.FK = obj1.PK;
                                            }
                                        }
                                    }
                                    ObjRBC.SaveXref(obj.FK, obj.PK);

                                }
                            }
                           
                        }

                    }
                }
                catch (System.Exception E)
                {

                }

                return RetVal;
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                errorString = ex.Message.ToString();
                return null;
            }


        }
    }
}
