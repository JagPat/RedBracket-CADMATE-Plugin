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
                        String[] plmobjInfo =  null;
                         plmobjInfo = str.Split(';');


                        plmObj.ObjectId = plmobjInfo[5];
                        string fileName = System.IO.Path.GetFileName(plmobjInfo[6]);
                         
                        plmObj.objectProjectNo = plmobjInfo[31];
                        plmObj.ObjectStatus = plmobjInfo[29];
                        plmObj.Classification = plmobjInfo[30];                         
                        plmObj.FilePath = plmobjInfo[6];
                        plmObj.IsNew = false;
                        if(plmObj.ObjectId.Trim().Length==0)
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
                            plmObj.ObjectDescription = plmobjInfo[plmobjInfo.Length-2];
                         

                        plmObj.ObjectProjectId = ProjectId = plmobjInfo[28];
                        plmObj.ObjectLayouts = plmobjInfo[11];                        
                        plmObj.FolderID = plmobjInfo[25];
                        plmObj.FolderPath = plmobjInfo[24];
                        if (!plmObj.IsRoot)
                            plmObj.IsNewXref = plmobjInfo[27].Contains("true")|| plmobjInfo[27].Contains("true")?  Convert.ToBoolean(plmobjInfo[27]):false;
                        plmObj.PreFix = plmobjInfo[22];
                        fileName = Helper.RemovePreFixFromFileName(fileName, plmObj.PreFix);
                        plmObj.ObjectName = fileName;



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








                        // gethering document info

                    //    foreach (String str in cmd.Drawings)
                    //{
                    //    PLMObject plmObj = new PLMObject();
                    //    String[] plmobjInfo = new String[24];
                    //    plmobjInfo = str.Split(';');
                    //    plmObj.ObjectId = plmobjInfo[0];
                    //    string fileName = System.IO.Path.GetFileName(plmobjInfo[2]);


                    //    plmObj.ObjectRevision = plmobjInfo[17];

                    //    plmObj.objectProjectNo= plmobjInfo[18];

                    //    plmObj.ObjectStatus = plmobjInfo[15];
                    //    plmObj.Classification = plmobjInfo[16];
                    //    plmObj.ItemType = plmobjInfo[1];
                    //    plmObj.FilePath = plmobjInfo[2];

                    //    plmObj.IsNew = false;
                    //    plmObj.IsRoot = false;
                    //    if (plmobjInfo[3] == "true")
                    //        plmObj.IsRoot = true;

                    //    if (plmObj.IsRoot)
                    //        plmObjs.Insert(0, plmObj);
                    //    else
                    //        plmObjs.Add(plmObj);

                    //    if (plmObj.IsRoot)
                    //        plmObj.ObjectDescription = plmobjInfo[6];
                    //    plmObj.ObjectSourceId = plmobjInfo[7];

                    //    plmObj.ObjectProjectId = ProjectId = plmobjInfo[9];

                    //    plmObj.ObjectLayouts = plmobjInfo[14];

                    //    plmObj.objectType = plmobjInfo[20];
                    //    plmObj.FolderID = plmobjInfo[22];
                    //    plmObj.FolderPath = plmobjInfo[23];
                    //    plmObj.PreFix = plmobjInfo[21];
                    //    fileName = Helper.RemovePreFixFromFileName(fileName, plmObj.PreFix);
                    //    plmObj.ObjectName = fileName;

                    //    bool IsNotFound = true;
                    //    foreach (DataTable dt in lstdtLayoutInfo)
                    //    {
                    //        if (dt.Rows.Count > 0)
                    //        {
                    //            if (Convert.ToString(dt.Rows[0]["FileID1"]) == plmObj.ObjectId)
                    //            {
                    //                plmObj.dtLayoutInfo = dt;
                    //                IsNotFound = false;
                    //                break;
                    //            }

                    //        }

                    //    }
                    //    if (IsNotFound)
                    //    {
                    //        plmObj.dtLayoutInfo = new DataTable();
                    //    }
                    //}

                    //foreach (String str in cmd.NewDrawings)
                    //{
                    //    try
                    //    {
                    //        PLMObject plmObj = new PLMObject();
                    //        String[] plmobjInfo = new String[26];
                    //        plmobjInfo = str.Split(';');
                    //        plmObj.ObjectNumber = plmobjInfo[0];
                    //        plmObj.ObjectName = plmobjInfo[2];
                    //        plmObj.FilePath = plmobjInfo[3];
                    //        plmObj.ObjectRevision = plmobjInfo[20];

                    //        plmObj.ObjectId = "";
                    //        plmObj.ItemType = plmobjInfo[5];
                    //        plmObj.ObjectProjectId = ProjectId = plmobjInfo[12];
                    //        plmObj.objectProjectNo = plmobjInfo[21];


                    //        plmObj.AuthoringTool = "AutoCAD";

                    //        plmObj.IsNew = true;
                    //        plmObj.ObjectStatus = plmobjInfo[18];
                    //        plmObj.Classification = plmobjInfo[19];





                    //        plmObj.ObjectLayouts = plmobjInfo[17];

                    //        plmObj.objectType = plmobjInfo[23];
                    //        plmObj.FolderID = plmobjInfo[25];
                    //        plmObj.FolderPath = plmobjInfo[26];
                    //        string fileName = System.IO.Path.GetFileName(plmobjInfo[2]);
                    //        plmObj.PreFix = plmobjInfo[24];
                    //        fileName = Helper.RemovePreFixFromFileName(fileName, plmObj.PreFix);
                    //        plmObj.ObjectName = fileName;

                    //        plmObj.IsRoot = false;
                    //        if (plmobjInfo[6] == "true")
                    //            plmObj.IsRoot = true;

                    //        if (plmObj.IsRoot)
                    //            plmObj.ObjectDescription = plmobjInfo[9];
                    //        bool IsNotFound = true;
                    //        foreach (DataTable dt in lstdtLayoutInfo)
                    //        {
                    //            if (dt.Rows.Count > 0)
                    //            {
                    //                if (System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(dt.Rows[0]["FileLayoutName"])) == System.IO.Path.GetFileNameWithoutExtension(fileName))
                    //                {
                    //                    plmObj.dtLayoutInfo = dt;
                    //                    IsNotFound = false;
                    //                    break;
                    //                }
                    //            }

                    //        }
                    //        if (IsNotFound)
                    //        {
                    //            plmObj.dtLayoutInfo = new DataTable();
                    //        }


                    //        if (plmObj.IsRoot)
                    //            plmObjs.Insert(0, plmObj);
                    //        else
                    //            plmObjs.Add(plmObj);
                    //    }
                    //    catch (System.Exception E)
                    //    {

                    //    }
                    //}

                }

                //  objConnector.SaveObject(ref plmObjs);
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
                                ,"" //IsNewXref  not to assign value from here, if ever assign assign fasle.
                                );
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
