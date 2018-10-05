using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Web.Script.Serialization;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using Newtonsoft.Json;
using RestSharp;


namespace RBAutocadPlugIn
{
    public class RBConnector
    {


        public bool SaveObject(ref List<PLMObject> plmobjs, bool IsFileSave = false)
        {
            try
            {
                //To unlock File in case of update
                //if (UnlockObject(plmobjs))
                //{
                //  return false;
                //}
                bool IsParentNew = false;
                string ParentFileID = "";
                foreach (PLMObject obj in plmobjs)
                {


                    RestResponse restResponse;
                    //service calling to upload document.
                    String PreFix = obj.PreFix;

                    if (!File.Exists(obj.FilePath))
                    {
                        ShowMessage.InfoMess(obj.FilePath + Environment.NewLine + "File not found.");
                        return false;
                    }





                    if (IsFileSave)
                    {
                        Helper.IncrementProgressBar(1, "Uploading file : " + Path.GetFileNameWithoutExtension(obj.FilePath));

                        if (obj.IsNew || obj.originalFileID == string.Empty)
                        {
                            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                            KeyValuePair<string, string> Keys = new KeyValuePair<string, string>();


                            //Keys = new KeyValuePair<string, string>("userName", Helper.UserName);
                            //keyValuePairs.Add(Keys);

                            Keys = new KeyValuePair<string, string>("fileId", obj.ObjectId);
                            keyValuePairs.Add(Keys);


                            Keys = new KeyValuePair<string, string>("source", "Computer");
                            keyValuePairs.Add(Keys);
                            Keys = new KeyValuePair<string, string>("project", obj.ObjectProjectId);
                            keyValuePairs.Add(Keys);

                            if (obj.FolderID.Trim().Length > 0 && Convert.ToDecimal(obj.FolderID.Trim()) > 0)
                            {
                                Keys = new KeyValuePair<string, string>("folderId", obj.FolderID.Trim());
                                keyValuePairs.Add(Keys);
                            }


                            //http://redbracketpms.com:8090/red-bracket-pms/AutocadFiles/uploadFile?
                            //userName =testing@yopmail.com&project=143&source=Computer&fileId=a8b2ec09-12bc-4907-9cf9-280918095b5a
                            restResponse = (RestResponse)ServiceHelper.SaveObject(
                                Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                                "/AutocadFiles/uploadFile", obj.FilePath,
                                true, keyValuePairs, PreFix, true);
                        }
                        else
                        {

                            //http://redbracketpms.com:8090/red-bracket-pms/AutocadFiles/uploadFileVersion?
                            //userName =testing@yopmail.com&fileId=a8b2ec09-12bc-4907-9cf9-280918095b5a&source=Computer
                            restResponse = (RestResponse)ServiceHelper.SaveObject(
                                Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                                "/AutocadFiles/uploadFileVersion", obj.FilePath,
                                true, new List<KeyValuePair<string, string>>
                                {
                                        new KeyValuePair<string, string>("fileId", obj.ObjectId),
                                        new KeyValuePair<string, string>("source","Computer"),
                                        new KeyValuePair<string, string>("versionType",obj.VersionType)

                                }, PreFix, true);

                            if (!obj.IsRoot)
                            {
                                // Helper.IncrementProgressBar(1, "Checking for association in redbracket." + Path.GetFileNameWithoutExtension(obj.FilePath));
                                if (!IsXrefNew(obj.FK, obj.ObjectId))
                                {
                                    obj.IsNewXref = true;
                                }
                            }
                        }


                        if ((!obj.IsRoot && (obj.IsNew || obj.IsNewXref || IsParentNew)))
                        {
                            if (SaveXref(obj.FK, obj.ObjectId))
                                return false;
                        }
                        else
                        {
                            ParentFileID = obj.ObjectId;
                            if (obj.IsNew)
                            {
                                IsParentNew = true;
                            }
                        }
                    }
                    else
                    {
                        Helper.IncrementProgressBar(1, "Uploading file properties." + Path.GetFileNameWithoutExtension(obj.FilePath));
                        List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                        KeyValuePair<string, string> Keys = new KeyValuePair<string, string>();

                        if (obj.ObjectProjectId.Trim().Length > 0)
                        {
                            Keys = new KeyValuePair<string, string>("project", obj.ObjectProjectId);
                            keyValuePairs.Add(Keys);
                        }

                        Keys = new KeyValuePair<string, string>("fileStatus", obj.ObjectStatus);
                        keyValuePairs.Add(Keys);

                        if (obj.Classification.Trim().Length > 0)
                        {
                            Keys = new KeyValuePair<string, string>("fileType", obj.Classification);
                            keyValuePairs.Add(Keys);
                        }

                        Keys = new KeyValuePair<string, string>("source", "Computer");
                        keyValuePairs.Add(Keys);
                        string s = Path.GetExtension(obj.FilePath);
                        Keys = new KeyValuePair<string, string>("fileName", Path.GetFileNameWithoutExtension(obj.ObjectName) + s);
                        keyValuePairs.Add(Keys);

                        string IsNew = "";
                        if (obj.ObjectId.Trim().Length > 0)
                        {
                            Keys = new KeyValuePair<string, string>("fileId", obj.ObjectId);
                            keyValuePairs.Add(Keys);
                        }



                        if (obj.ObjectId == string.Empty)
                        {
                            IsNew = "false";
                        }
                        else
                        {
                            IsNew = "true";
                        }

                        Keys = new KeyValuePair<string, string>("isNew", IsNew);
                        keyValuePairs.Add(Keys);

                        Keys = new KeyValuePair<string, string>("versiondesc", obj.ObjectDescription);
                        keyValuePairs.Add(Keys);

                        if (obj.FolderID.Trim().Length > 0 && Convert.ToDecimal(obj.FolderID.Trim()) > 0)
                        {
                            Keys = new KeyValuePair<string, string>("folderidString", obj.FolderID.Trim());
                            keyValuePairs.Add(Keys);
                        }

                        // http://redbracketpms.com:8090/red-bracket-pms/AutocadFiles/uploadFileServiceProp?userName=archi@yopmail.com&project=0&fileStatus=905&fileType=692&source=Computer&fileId=9d5c029d-f085-427d-a199-f899f35ec8e7&isNew=false
                        restResponse = (RestResponse)ServiceHelper.SaveObject(
                                        Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                                        "/AutocadFiles/uploadFileServiceProp", obj.FilePath,
                                             true, keyValuePairs, obj.PreFix, false);
                    }








                    //checking if service call was successful or not.
                    if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        //ShowMessage.InfoMess(restResponse.Content);
                        //ShowMessage.InfoMess(restResponse.ResponseUri.ToString());
                        ShowMessage.ErrorMess("Some error occurred while uploading file.", restResponse);
                        return false;
                    }
                    else if (restResponse.Content.Trim().Length > 0)
                    {
                        if (restResponse.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
                        {
                        }
                        else

                        {


                            SaveResult saveResult = new JavaScriptSerializer().Deserialize<SaveResult>(restResponse.Content);





                            //var Drawing123 =  isError ? JsonConvert.DeserializeObject<DataofData>(saveResult.dataofdata) : JsonConvert.DeserializeObject<List<DataofData>>(saveResult.dataofdata) ;
                            var Drawing123 = JsonConvert.DeserializeObject<List<DataofData>>(saveResult.dataofdata);
                            foreach (DataofData Drawing in Drawing123)
                            {

                                if (Drawing != null)
                                {

                                    if (obj.IsRoot)
                                    {
                                        obj.ObjectId = ParentFileID = Drawing.id;

                                        string PK = "";
                                        foreach (PLMObject obj1 in plmobjs)
                                        {
                                            if (obj1.FilePath == obj.FilePath)
                                            {
                                                PK = obj1.PK;
                                                if (obj.PK == obj.FK)
                                                {
                                                    obj.PK = obj.FK = Drawing.id;
                                                }
                                            }

                                        }
                                        foreach (PLMObject obj1 in plmobjs)
                                        {
                                            if (PK == obj1.FK)
                                            {
                                                obj1.FK = Drawing.id;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        obj.ObjectId = Drawing.id;

                                        string PK = "";
                                        foreach (PLMObject obj1 in plmobjs)
                                        {
                                            if (obj1.FilePath == obj.FilePath)
                                            {
                                                PK = obj1.PK;
                                                obj.PK = Drawing.id;
                                            }
                                        }
                                        foreach (PLMObject obj1 in plmobjs)
                                        {
                                            if (PK == obj1.FK)
                                            {
                                                obj1.FK = Drawing.id;
                                            }
                                        }
                                    }



                                    if (!IsFileSave)
                                    {
                                        obj.ObjectNumber = Drawing.fileNo == null ? string.Empty : Drawing.fileNo;
                                        //Helper.IncrementProgressBar(1, "Uploading layout properties." + Path.GetFileNameWithoutExtension(obj.FilePath));
                                        List<LayoutInfo> LayoutInfolst = SaveUpdateLayoutInfo(obj.dtLayoutInfo, obj.ObjectProjectId, obj.ObjectId, obj.ObjectNumber, obj.FilePath);
                                        // ResultSearchCriteria Drawing = GetDrawingInformation(obj.ObjectId);

                                        obj.ObjectName = Drawing.name;
                                        obj.Classification = Drawing.type == null ? string.Empty : Convert.ToString(Drawing.type.id);
                                        obj.ObjectState = Drawing.status == null ? string.Empty : Drawing.status.statusname == null ? string.Empty : Drawing.status.statusname;
                                        obj.ObjectRevision = Drawing.versionno.Contains("Ver ") ? Drawing.versionno.Substring(4) : Drawing.versionno;
                                        obj.LockStatus = Convert.ToString(Drawing.filelock);
                                        obj.ObjectGeneration = obj.ObjectRevision;
                                        obj.ItemType = Drawing.coreType == null ? string.Empty : Drawing.coreType.name;
                                        obj.ObjectProjectName = Drawing.projectname == null ? obj.ObjectProjectName : Drawing.projectname;
                                        //obj.ObjectProjectId = "123";
                                        obj.ObjectCreatedById = Drawing.createdby;
                                        obj.ObjectCreatedOn = Drawing.created0n;
                                        obj.ObjectModifiedById = Drawing.updatedby;
                                        obj.ObjectModifiedOn = Drawing.updatedon;
                                        obj.ObjectDescription = Drawing.description;

                                        obj.canDelete = Drawing.canDelete;
                                        obj.isowner = Drawing.isowner;
                                        obj.hasViewPermission = Drawing.hasViewPermission;
                                        obj.isActFileLatest = Drawing.isActFileLatest;
                                        obj.isEditable = Drawing.isEditable;
                                        obj.canEditStatus = Drawing.canEditStatus;
                                        obj.hasStatusClosed = Drawing.hasStatusClosed;
                                        obj.isletest = Drawing.isletest;
                                        obj.objectType = Drawing.type == null ? string.Empty : Convert.ToString(Drawing.type.name);
                                        obj.objectProjectNo = Drawing.projectno == null ? obj.objectProjectNo : Drawing.projectno;
                                        obj.IsSaved = true;
                                        obj.originalFileID = Drawing.originalFileId;
                                        obj.LayoutInfo = Helper.GetLayoutInfo(LayoutInfolst);
                                    }







                                }

                            }


                        }

                    }






                }
                //if (IsFileSave)
                    UnlockObject(plmobjs);
                return true;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return false;
            }


        }
        public bool SaveXref(string parentFileId, string childFileId)
        {
            try
            {
                if (IsXrefNew(parentFileId, childFileId))
                {
                    return false;
                }
                List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                KeyValuePair<string, string> Keys = new KeyValuePair<string, string>();


                //Keys = new KeyValuePair<string, string>("parentFileId", ParentFileID);
                Keys = new KeyValuePair<string, string>("parentFileId", parentFileId);
                keyValuePairs.Add(Keys);

                Keys = new KeyValuePair<string, string>("childFileId", childFileId);
                keyValuePairs.Add(Keys);


                // http://redbracketpms.com:8090/red-bracket-pms/AutocadFiles/uploadFileServiceProp?userName=archi@yopmail.com&project=0&fileStatus=905&fileType=692&source=Computer&fileId=9d5c029d-f085-427d-a199-f899f35ec8e7&isNew=false
                RestResponse restResponse1 = (RestResponse)ServiceHelper.PostData(
                                Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                                "/AutocadFiles/uploadXRef", DataFormat.Json, null,
                                     true, keyValuePairs);

                if (restResponse1.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    //ShowMessage.InfoMess(restResponse.Content);
                    //ShowMessage.InfoMess(restResponse.ResponseUri.ToString());
                    ShowMessage.ErrorMess("Some error occurred while defining file relationship.", restResponse1);
                    return true;
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return false;
        }
        public bool IsXrefNew(string ParentID, string ChildID)
        {
            try
            {
                var objRSC = GetXrefFIleInfo(ParentID);

                foreach (var item in objRSC)
                {
                    if (item.id == ChildID)
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message); return true;
            }
        }
        public bool UpdateFileProperties(string FileID, string Type, string Status, string Project,string FolderID)
        {
            try
            {
                Type = Type == "-1" ? string.Empty : Type;
                Status = Status == "-1" ? string.Empty : Status;
                RestResponse restResponse;



                //restResponse = (RestResponse)ServiceHelper.SaveObject(
                //   Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                //   "/AutocadFiles/uploadFileService", FilePath,
                //        true, new List<KeyValuePair<string, string>> {
                //            new KeyValuePair<string, string>("project", Project) ,
                //            new KeyValuePair<string, string>("fileStatus", Status),
                //            new KeyValuePair<string, string>("fileType", Type),
                //             new KeyValuePair<string, string>("fileId", FileID)
                //        }, PreFix, true);

                List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                KeyValuePair<string, string> Keys = new KeyValuePair<string, string>();


                Keys = new KeyValuePair<string, string>("project", Project);
                keyValuePairs.Add(Keys);

                Keys = new KeyValuePair<string, string>("fileStatus", Status);
                keyValuePairs.Add(Keys);

                Keys = new KeyValuePair<string, string>("fileType", Type);
                keyValuePairs.Add(Keys);

                Keys = new KeyValuePair<string, string>("source", "Computer");
                keyValuePairs.Add(Keys);
                Keys = new KeyValuePair<string, string>("folderidString", FolderID==string.Empty|| FolderID == null ?"0":FolderID);
                keyValuePairs.Add(Keys);

                string IsNew = "";
                if (FileID.Trim().Length > 0)
                {
                    Keys = new KeyValuePair<string, string>("fileId", FileID);
                    keyValuePairs.Add(Keys);
                }





                Keys = new KeyValuePair<string, string>("isNew", "false");
                keyValuePairs.Add(Keys);
                // http://redbracketpms.com:8090/red-bracket-pms/AutocadFiles/uploadFileServiceProp?userName=archi@yopmail.com&project=0&fileStatus=905&fileType=692&source=Computer&fileId=9d5c029d-f085-427d-a199-f899f35ec8e7&isNew=false
                restResponse = (RestResponse)ServiceHelper.PostData(
                                Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                                "/AutocadFiles/uploadFileServiceProp", DataFormat.Json, null,
                                     true, keyValuePairs);

                //checking if service call was successful or not.
                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error occurred while updating file properties.", restResponse);
                    return false;
                }
                else if (restResponse.Content.Trim().Length > 0)
                {

                    //SaveResult saveResult = new JavaScriptSerializer().Deserialize<SaveResult>(restResponse.Content);

                    //if (saveResult.message == "allow")
                    //{
                    //    return true;
                    //}
                    //else
                    //{
                    //    ShowMessage.ValMess(FileName + Environment.NewLine + "File with the same name exist in this peoject, please change file name.");
                    //    return false;
                    //}
                    return true;
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return false;
        }
        public List<clsFolderSearchReasult> SearchFolder(string Project, string Folder)
        {
            List<clsFolderSearchReasult> saveResult = null;
            try
            {

                RestResponse restResponse;
                List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();

                KeyValuePair<string, string> valuePair;

                //if (Convert.ToDecimal(Project) > 0)
                {
                    valuePair = new KeyValuePair<string, string>("projectId", Project);
                    keyValuePairs.Add(valuePair);
                }
                if (Convert.ToDecimal(Folder) > 0)
                {
                    KeyValuePair<string, string> valuePair1 = new KeyValuePair<string, string>("folderId", Folder);
                    keyValuePairs.Add(valuePair1);
                }


                restResponse = (RestResponse)ServiceHelper.GetData(
                   Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                   "/AutocadFiles/getFolderPath",
                        true, keyValuePairs
                         );


                //checking if service call was successful or not.
                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error occurred while searching for folder.", restResponse);

                }
                else if (restResponse.Content.Trim().Length > 0)
                {
                    saveResult = JsonConvert.DeserializeObject<List<clsFolderSearchReasult>>(restResponse.Content);
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return saveResult;
        }

        public bool UpdateLayoutInfo(string ProjectID, string Fileid, string LayoutID, string StatusID, string TypeID, string LayoutName, string LayoutDesc)
        {
            try
            {

                RestResponse restResponse;
                //service calling 

                if (LayoutID ==null|| LayoutID.Trim().Length==0)
                {
                    return false;
                }

                restResponse = (RestResponse)ServiceHelper.PostData(
              Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
              "/AutocadFiles/uploadLayoutACFileVersion", DataFormat.Json, null, false
                 , new List<KeyValuePair<string, string>> {
                              new KeyValuePair<string, string>("source", "Computer"),
                                         new KeyValuePair<string, string>("fileId", Fileid),
                                             new KeyValuePair<string, string>("layoutId", LayoutID)                                             ,
                                             new KeyValuePair<string, string>("versiondesc",  LayoutDesc),
                                                 new KeyValuePair<string, string>("userName",  Helper.UserName),
                                             new KeyValuePair<string, string>("status",  StatusID),
                                              new KeyValuePair<string, string>("type",  TypeID),
                                             new KeyValuePair<string, string>("layoutname",  LayoutName),
                                               new KeyValuePair<string, string>("project", ProjectID),
                                                    new KeyValuePair<string, string>("isNew", "false")
                                         });



                //checking if service call was successful or not.
                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    //ShowMessage.InfoMess(restResponse.Content);
                    //ShowMessage.InfoMess(restResponse.ResponseUri.ToString());
                    ShowMessage.ErrorMess("Some error occurred while uploading layout info.", restResponse);
                    return false;
                }
                else if (restResponse.Content.Trim().Length > 0)
                {
                }
                return true;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return false;
            }


        }

        public bool CheckFileExistance(string ProjectID, string FileName)
        {
            try
            {
                ProjectID = ProjectID == string.Empty ? "0" : ProjectID;
                RestResponse restResponse;
                //service calling to Check File Existance.

                //http://redbracketpms.com:8090/red-bracket-pms/AutocadFiles/checkFileName?fileName=M1.dwg&userName=archi@yopmail.com&projectId=0

                //https://test.redbracket.in:8090/AutocadFiles/checkFileName?fileName=Parent114.dwg&userName=archi@yopmail.com&projectId=0

                restResponse = (RestResponse)ServiceHelper.GetData(
                  Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                  "/AutocadFiles/checkFileName", false
                     , new List<KeyValuePair<string, string>> {
                                         new KeyValuePair<string, string>("fileName", FileName),
                                              new KeyValuePair<string, string>("userName", Helper.UserName),
                                                      new KeyValuePair<string, string>("projectId",  ProjectID)   });

                //checking if service call was successful or not.
                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    //ShowMessage.InfoMess(restResponse.Content);
                    //ShowMessage.InfoMess(restResponse.ResponseUri.ToString());
                    ShowMessage.ErrorMess("Some error occurred while checking for file duplication.", restResponse);
                    return false;
                }
                else if (restResponse.Content.Trim().Length > 0)
                {

                    SaveResult saveResult = new JavaScriptSerializer().Deserialize<SaveResult>(restResponse.Content);

                    if (saveResult.message == "allow")
                    {
                        return true;
                    }
                    else
                    {
                        ShowMessage.ValMess(FileName + Environment.NewLine + "File with the same name exist in this peoject, please change file name.");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return false;
            }
        }

        public List<LayoutInfo> SaveUpdateLayoutInfo(DataTable dtLayoutInfo, string ProjectID, string Fileid, string DrawingNO, string FilePath)
        {
            List<LayoutInfo> LayoutInfolst = new List<LayoutInfo>();
            try
            {

                int Count = -1;
                foreach (DataRow dr in dtLayoutInfo.Rows)
                {
                    Count++;
                    if (Convert.ToString(dr["IsFile"]) == "1")
                    { 
                        continue;
                    }
                    else if ((Convert.ToString(dr["ChangeVersion"]) == "False"))
                    {
                        LayoutInfo objLI = new LayoutInfo();

                        objLI.id = Convert.ToString(dr["LayoutID"]).Trim();
                        objLI.canDelete = false;
                        objLI.canEditStatus = false;

                        objLI.updatedby = Convert.ToString(dr["UpdatedBy"]).Trim();
                        objLI.updatedon = Convert.ToString(dr["UpdatedOn"]).Trim();
                        objLI.createdby = Convert.ToString(dr["CreatedBy"]).Trim();
                        objLI.created0n = Convert.ToString(dr["CreatedOn"]).Trim();
                        objLI.description = Convert.ToString(dr["Description"]).Trim();
                        objLI.fileNo = Convert.ToString(dr["LayoutNo"]).Trim(); ;
                        objLI.hasStatusClosed = false;
                        objLI.isActFileLatest = false;
                        objLI.isEditable = false;
                        objLI.isletest = true;
                        objLI.isowner = false;
                        //objLI.layoutId = Drawing.;
                        objLI.name = Convert.ToString(dr["LayoutName1"]).Trim();

                        objLI.statusId = Convert.ToString(dr["StatusID"]).Trim();
                        objLI.statusname = Convert.ToString(dr["LayoutStatus"]).Trim();

                        objLI.typeId = Convert.ToString(dr["TypeID"]).Trim();
                        objLI.typename = Convert.ToString(dr["LayoutType"]).Trim();
                        objLI.versionno = Convert.ToString(dr["Version"]).Trim();

                        LayoutInfolst.Add(objLI);
                        continue;
                    }


                    RestResponse restResponse;
                    //service calling to upload document.

                    if (Convert.ToString(dr["LayoutID"]).Trim().Length == 0)
                    {
                        string Sufix = string.Format("{0:00}", Count) + "_" + DrawingNO;
                        string NewLayoutName = Convert.ToString(dr["LayoutName1"]).Trim() + "_" + Sufix;
                        //Helper.cadManager.renamelayoutName(FilePath, Convert.ToString(dr["FileLayoutName"]).Trim(), NewLayoutName);

                        dr["LayoutName1"] = NewLayoutName;
                        dr["FileLayoutName"] = NewLayoutName;
                        restResponse = (RestResponse)ServiceHelper.PostData(
                  Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                  "/AutocadFiles/uploadLayoutACFiles", DataFormat.Json, null, true
                     , new List<KeyValuePair<string, string>> {
                                         new KeyValuePair<string, string>("fileId", Fileid),
                                              new KeyValuePair<string, string>("layoutId", Convert.ToString(dr["ACLayoutID"]).Trim()),
                                                 new KeyValuePair<string, string>("layoutFileName", NewLayoutName),
                                                      new KeyValuePair<string, string>("project",  ProjectID),
                                             new KeyValuePair<string, string>("status",  Convert.ToString(dr["StatusID"]).Trim()),
                                              new KeyValuePair<string, string>("type",  Convert.ToString(dr["TypeID"]).Trim()),
                                              new KeyValuePair<string, string>("source",  "Computer"),
                                             new KeyValuePair<string, string>("description",  Convert.ToString(dr["Description"]).Trim()) });




                    }
                    else
                    {

                        restResponse = (RestResponse)ServiceHelper.PostData(
                      Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                      "/AutocadFiles/uploadLayoutACFileVersion", DataFormat.Json, null, false
                         , new List<KeyValuePair<string, string>> {
                              new KeyValuePair<string, string>("source", "Computer"),
                                         new KeyValuePair<string, string>("fileId", Fileid),
                                             new KeyValuePair<string, string>("layoutId", Convert.ToString(dr["LayoutID"]).Trim())                                             ,
                                            new KeyValuePair<string, string>("versiondesc",  Convert.ToString(dr["Description"]).Trim()),
                                                 new KeyValuePair<string, string>("userName",  Helper.UserName),
                                             new KeyValuePair<string, string>("status",  Convert.ToString(dr["StatusID"]).Trim()),
                                              new KeyValuePair<string, string>("type",  Convert.ToString(dr["TypeID"]).Trim()),
                                              new KeyValuePair<string, string>("layoutname",  Convert.ToString(dr["FileLayoutName"]).Trim()),
                                               new KeyValuePair<string, string>("project", ProjectID),
                                                    new KeyValuePair<string, string>("isNew", "true"),
                                                new KeyValuePair<string, string>("versionType", Convert.ToString(dr["VersionType"]).Trim())
                                                 });
                    }


                    //checking if service call was successful or not.
                    if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        ShowMessage.ErrorMess("Some error occurred while uploading layout info.", restResponse);
                    }
                    else if (restResponse.Content.Trim().Length > 0)
                    {
                        SaveResult saveResult = new JavaScriptSerializer().Deserialize<SaveResult>(restResponse.Content);


                        var Drawing = JsonConvert.DeserializeObject<DataofData>(saveResult.dataofdata);
                        if (Convert.ToString(dr["LayoutID"]).Trim().Length == 0)
                        {
                            dr["ACLayoutID"] = Drawing.id;
                        }

                        LayoutInfo objLI = new LayoutInfo();

                        objLI.id = Drawing.id;
                        objLI.canDelete = Drawing.canDelete;
                        objLI.canEditStatus = Drawing.canEditStatus;

                        objLI.updatedby = Drawing.updatedby;
                        objLI.updatedon = Drawing.updatedon;
                        objLI.createdby = Drawing.createdby;
                        objLI.created0n = Drawing.created0n == null ? string.Empty : Drawing.created0n;
                        objLI.description = Drawing.description;
                        objLI.fileNo = Drawing.fileNo;
                        objLI.hasStatusClosed = Drawing.hasStatusClosed;
                        objLI.isActFileLatest = Drawing.isActFileLatest;
                        objLI.isEditable = Drawing.isEditable;
                        objLI.isletest = Drawing.isletest;
                        objLI.isowner = Drawing.isowner;
                        //objLI.layoutId = Drawing.;
                        objLI.name = Drawing.name;
                        objLI.status = Drawing.status;
                        objLI.statusId = Drawing.status == null ? "0" : Convert.ToString(Drawing.status.id);
                        objLI.statusname = Drawing.status == null ? "" : Convert.ToString(Drawing.status.statusname);
                        objLI.type = Drawing.type;
                        objLI.typeId = Drawing.type == null ? "0" : Convert.ToString(Drawing.type.id);
                        objLI.typename = Drawing.type == null ? "" : Convert.ToString(Drawing.type.name);
                        objLI.versionno = Drawing.versionno;

                        LayoutInfolst.Add(objLI);
                    }
                }



            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);

            }
            return LayoutInfolst;

        }


        public bool CheckLayoutExistance(string ProjectID, string LayoutName, string FIleID)
        {
            try
            {

                RestResponse restResponse;
                //service calling to Check File Existance.

                // http://redbracketpms.com:8090/red-bracket-pms/AutocadFiles/checkLayoutACFileName?userName=archi@yopmail.com&
                //fileId =601188bf-1be0-4cc3-9498-b5c6d4d651c5&layoutFileName=Layout33&projectId=0

                restResponse = (RestResponse)ServiceHelper.GetData(
                  Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                  "/AutocadFiles/checkLayoutACFileName", false
                     , new List<KeyValuePair<string, string>> {

                                              new KeyValuePair<string, string>("userName", Helper.UserName),
                                                  new KeyValuePair<string, string>("fileId", FIleID),
                                               new KeyValuePair<string, string>("layoutFileName", LayoutName),
                                                      new KeyValuePair<string, string>("projectId",  ProjectID)   });







                //checking if service call was successful or not.
                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error occurred while checking for file duplication.", restResponse);
                    return false;
                }
                else if (restResponse.Content.Trim().Length > 0)
                {

                    SaveResult saveResult = new JavaScriptSerializer().Deserialize<SaveResult>(restResponse.Content);

                    if (saveResult.message == "allow")
                    {
                        return true;
                    }
                    else
                    {
                        ShowMessage.ValMess(LayoutName + Environment.NewLine + "This layout is already exist in this file, please change layout name.");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return false;
            }


        }

        /// <summary>
        /// Get Layout version and other info from RB by providing fileid
        /// </summary>
        /// <param name="FileID"></param>
        /// <returns></returns>

        public List<PLMObject> GetPLMObjectInformation(List<PLMObject> plmobjs)
        {
            List<PLMObject> newplmobjs = new List<PLMObject>();
            try
            {
                foreach (PLMObject obj in plmobjs)
                {

                    KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", obj.ObjectId);
                    // KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", "11760c31-d3fb-4acb-9675-551915493fd5");

                    List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                    urlParameters.Add(L);
                    RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                        "/AutocadFiles/fetchFileInfo", DataFormat.Json,
                        null, true, urlParameters);





                    if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        ShowMessage.ErrorMess("Some error occurred while fetching file information.", restResponse);
                    }
                    else
                    {
                        var ObjFileInfo = JsonConvert.DeserializeObject<ResultSearchCriteria>(restResponse.Content);
                        string Response = restResponse.Content;


                        PLMObject objPlm = new PLMObject();
                        objPlm.Classification = "";
                        objPlm.ObjectGeneration = "";
                        objPlm.ObjectNumber = ObjFileInfo.fileNo;
                        objPlm.ObjectRevision = ObjFileInfo.versionno;
                        objPlm.ObjectState = ObjFileInfo.status.statusname;
                        objPlm.ObjectName = ObjFileInfo.name;
                        objPlm.ObjectProjectId = ObjFileInfo.projectinfo;
                        objPlm.ObjectProjectName = ObjFileInfo.projectname;
                        objPlm.LockStatus = ObjFileInfo.filelock.ToString();
                        newplmobjs.Add(objPlm);

                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
            }
            return newplmobjs;
        }

        public DataTable GetFIleType()
        {
            DataTable dataTableProjectInfo = new DataTable();
            try
            {
                if (Helper.dtFileType.Rows.Count > 0)
                {
                    dataTableProjectInfo = Helper.dtFileType.Copy();
                }
                else
                {
                    dataTableProjectInfo = GetDataFromWS("/AutocadFiles/fetchFileType", "file type", "POST", typeof(List<ResultStatusData>));
                    Helper.dtFileType = dataTableProjectInfo.Copy();
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dataTableProjectInfo;
        }

        public void GetFIleStatus(ComboBox cmb, string DisplayMember, string ValueMenmber, bool IsSelect)
        {
        }

        public DataTable GetFIleStatus()
        {
            DataTable dataTableFileStatus = new DataTable();
            try
            {
                if (Helper.dtFileStatus.Rows.Count > 0)
                {
                    dataTableFileStatus = Helper.dtFileStatus.Copy();
                }
                else
                {
                    dataTableFileStatus = GetDataFromWS("/AutocadFiles/fetchFileStatus", "file status", "POST", typeof(List<ResultStatusData>));
                    Helper.dtFileStatus = dataTableFileStatus.Copy();
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dataTableFileStatus;
        }

        public DataTable GetProjectDetail()
        {
            DataTable dataTableProjectInfo = new DataTable();
            try
            {
                if (Helper.dtProjectDetail.Rows.Count > 1)
                {
                    dataTableProjectInfo = Helper.dtProjectDetail.Copy();
                }
                else
                {
                    dataTableProjectInfo = GetDataFromWS("/ProjectAutocad/fetchUserAutocadProjectsService", "project detail", "GET");

                    if (dataTableProjectInfo != null)
                    {
                        dataTableProjectInfo.Columns.Add("PNAMENO");

                        for (int i = 0; i < dataTableProjectInfo.Rows.Count; i++)
                        {
                            dataTableProjectInfo.Rows[i]["PNAMENO"] = Convert.ToString(dataTableProjectInfo.Rows[i]["name"]) + " (" + Convert.ToString(dataTableProjectInfo.Rows[i]["number"]) + ")";
                        }
                    }
                    Helper.dtProjectDetail = dataTableProjectInfo.Copy();
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dataTableProjectInfo;
        }

        public DataTable GetDataFromWS(string ServiceName, string MessageText)
        {
            return GetDataFromWS(ServiceName, MessageText, "POST");
        }

        public DataTable GetDataFromWS(string ServiceName, string MessageText, String MethodType)
        {
            return GetDataFromWS(ServiceName, MessageText, MethodType, typeof(DataTable));
        }

        public DataTable GetDataFromWS(string ServiceName, string MessageText, String MethodType, Type type)
        {
            DataTable dataTableProjectInfo = null;
            try
            {
                RestResponse restResponse;
                if (MethodType == "POST")
                    restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), ServiceName, DataFormat.Json, null, true, null);
                else
                    restResponse = (RestResponse)ServiceHelper.GetData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), ServiceName, true);
                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error occurred while retrieving the " + MessageText + ".", restResponse);
                }
                else
                {
                    if (type == typeof(DataTable))
                    {
                        dataTableProjectInfo = (DataTable)JsonConvert.DeserializeObject(restResponse.Content, (typeof(DataTable)));
                    }
                    else
                    {
                        if (type == typeof(List<ResultStatusData>))
                        {
                            List<ResultStatusData> ObjFileInfo = JsonConvert.DeserializeObject<List<ResultStatusData>>(restResponse.Content);

                            foreach (ResultStatusData obj in ObjFileInfo)
                            {
                                if ((obj.coretype == null ? string.Empty : obj.coretype.name == null ? string.Empty : obj.coretype.name).ToLower() == "closed")
                                {
                                    obj.IsClosed = true;
                                }
                                else
                                {
                                    obj.IsClosed = false;
                                }
                            }
                            dataTableProjectInfo = Helper.ToDataTable(ObjFileInfo);
                            if (ServiceName == "/AutocadFiles/fetchFileStatus") ;
                            {
                                if (dataTableProjectInfo.Rows.Count > 0 && dataTableProjectInfo.Columns.Contains("statusname"))
                                {
                                    Helper.FirstStatusName = Convert.ToString(dataTableProjectInfo.Rows[0]["statusname"]);
                                    Helper.FirstStatusID = Convert.ToString(dataTableProjectInfo.Rows[0]["id"]);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dataTableProjectInfo;
        }

        public RestResponse GetDataFromWS1(string ServiceName, string MessageText)
        {
            RestResponse restResponse = null;
            try
            {
                restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), ServiceName, DataFormat.Json, null, true, null);
                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error occurred while retrieving the " + MessageText + ".", restResponse);
                }
                else
                {

                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return restResponse;
        }

        public void LockStatus(ref DataTable itemInfo)
        {
            try
            {
                foreach (DataRow rw in itemInfo.Rows)
                {
                    if (rw["drawingid"].ToString() != "")
                    {


                        KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", rw["drawingid"].ToString());
                        // KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", "11760c31-d3fb-4acb-9675-551915493fd5");

                        List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                        urlParameters.Add(L);
                        RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                            "/AutocadFiles/fetchFileInfo", DataFormat.Json,
                            null, true, urlParameters);

                        ResultSearchCriteria ObjFileInfo = null;




                        if (restResponse.Content == null || restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            ShowMessage.ErrorMess("Some error occurred while fetching file status.", restResponse);
                            return;
                        }
                        else
                        {
                            ObjFileInfo = JsonConvert.DeserializeObject<ResultSearchCriteria>(restResponse.Content);
                            if (ObjFileInfo != null)
                            {
                                bool FileLock = Convert.ToBoolean(Convert.ToString(ObjFileInfo.filelock));
                                string UpdatedBy = Convert.ToString(ObjFileInfo.updatedby);

                                if (FileLock)
                                {
                                    if (UpdatedBy == Helper.UserFullName)
                                    {
                                        rw["lockstatus"] = "1";
                                        rw["lockby"] = UpdatedBy;
                                    }
                                    else
                                    {
                                        rw["lockstatus"] = "2";
                                        rw["lockby"] = UpdatedBy;
                                    }
                                }
                                else
                                {
                                    rw["lockstatus"] = "0";
                                    rw["lockby"] = UpdatedBy;
                                }
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);

            }
        }

        public bool LockObject(List<PLMObject> plmObjs)
        {
            try
            {
                bool IsUpdated = true;
                foreach (PLMObject plmObj in plmObjs)
                {


                    List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                    KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileid", plmObj.ObjectId);
                    //  KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileid", "11760c31-d3fb-4acb-9675-551915493fd5");
                    urlParameters.Add(L);
                    L = new KeyValuePair<string, string>("userName", Helper.UserName);
                    urlParameters.Add(L);

                    RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                        "/AutocadFiles/lockfile", DataFormat.Json,
                        null, false, urlParameters);
                    if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        ShowMessage.ErrorMess("Some error occurred while locking file.", restResponse);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message); return false;
            }
            return true;
        }

        public bool UnlockObject(List<PLMObject> plmObjs)
        {
            try
            {
                foreach (PLMObject plmObj in plmObjs)
                {


                    List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                    //KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileid", "11760c31-d3fb-4acb-9675-551915493fd5");
                    KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileid", plmObj.ObjectId);
                    urlParameters.Add(L);
                    L = new KeyValuePair<string, string>("userName", Helper.UserName);
                    urlParameters.Add(L);

                    RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                        "/AutocadFiles/unlockfile", DataFormat.Json,
                        null, false, urlParameters);
                    if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        ShowMessage.ErrorMess("Some error occurred while unlocking file.", restResponse);
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
                return false;
            }
            return false;
        }


        public ResultSearchCriteria GetDrawingInformation(String drawingid)
        {
            ResultSearchCriteria ObjFileInfo = null;
            try
            {
              
                KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", drawingid);
                //KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", "11760c31-d3fb-4acb-9675-551915493fd5");

                List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                urlParameters.Add(L);
                RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                    "/AutocadFiles/fetchFileInfo", DataFormat.Json,
                    null, true, urlParameters);


                //Helper.CloseProgressBar();
                if (restResponse == null || restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error occurred while fetching file information.", restResponse);
                    return null;
                }
                else
                {
                    ObjFileInfo = JsonConvert.DeserializeObject<ResultSearchCriteria>(restResponse.Content);
                    try
                    {
                        ObjFileInfo.folderid = ObjFileInfo.folder == null ? string.Empty : ObjFileInfo.folder.id;
                        ObjFileInfo.folderpath = Helper.GetFolderPath(ObjFileInfo.folder, ObjFileInfo.projectname.Trim() == string.Empty ? "My Files" : ObjFileInfo.projectname);

                    }
                    catch { }
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
            }
            return ObjFileInfo;
        }

        public Hashtable GetDrawingInformationHT(String drawingid)
        {
            Hashtable DrawingProperty = new Hashtable();

            try
            {
                KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", drawingid);
                //KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", "11760c31-d3fb-4acb-9675-551915493fd5");

                List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                urlParameters.Add(L);
                RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                    "/AutocadFiles/fetchFileInfo", DataFormat.Json,
                    null, true, urlParameters);





                if (restResponse == null || restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error occurred while fetching file information.", restResponse);
                    return null;
                }
                else
                {
                    ResultSearchCriteria Drawing = JsonConvert.DeserializeObject<ResultSearchCriteria>(restResponse.Content);
                    string Response = restResponse.Content;


                    DrawingProperty.Add("DrawingId", Drawing.id);
                    DrawingProperty.Add("DrawingName", Drawing.name);
                    DrawingProperty.Add("Classification", "");
                    DrawingProperty.Add("FileTypeID", Drawing.type == null ? string.Empty : Drawing.type.name == null ? string.Empty : Drawing.type.name);
                    DrawingProperty.Add("DrawingNumber", Drawing.fileNo);

                    DrawingProperty.Add("DrawingState", Drawing.status == null ? string.Empty : Drawing.status.statusname == null ? string.Empty : Drawing.status.statusname);
                    DrawingProperty.Add("Revision", Drawing.versionno);
                    DrawingProperty.Add("LockStatus", Drawing.filelock);
                    DrawingProperty.Add("Generation", Drawing.versionno);
                    DrawingProperty.Add("Type", Drawing.coreType.id);
                    //DrawingProperty.Add("ProjectName", Drawing.projectname );
                    if (Drawing.projectname.Trim().Length == 0)
                    {
                        DrawingProperty.Add("ProjectName", "My Files");
                    }
                    else
                    {
                        DrawingProperty.Add("ProjectName", Drawing.projectname + " (" + Drawing.projectNumber + ")");
                    }

                    DrawingProperty.Add("ProjectId", Drawing.projectinfo);
                    DrawingProperty.Add("CreatedOn", Drawing.updatedon);
                    DrawingProperty.Add("CreatedBy", Drawing.createdby);
                    DrawingProperty.Add("ModifiedOn", Drawing.updatedon);
                    DrawingProperty.Add("ModifiedBy", Drawing.updatedby);

                    DrawingProperty.Add("canDelete", Drawing.canDelete);
                    DrawingProperty.Add("isowner", Drawing.isowner);
                    DrawingProperty.Add("hasViewPermission", Drawing.hasViewPermission);
                    DrawingProperty.Add("isActFileLatest", Drawing.isActFileLatest);

                    DrawingProperty.Add("isEditable", Drawing.isEditable);
                    DrawingProperty.Add("canEditStatus", Drawing.canEditStatus);
                    DrawingProperty.Add("hasStatusClosed", Drawing.hasStatusClosed);
                    DrawingProperty.Add("isletest", Drawing.isletest);


                    DrawingProperty.Add("projectno", Drawing.projectNumber == null ? string.Empty : Drawing.projectNumber);

                    string ProjectNo = Drawing.projectNumber == null ? string.Empty : Drawing.projectNumber;


                    string FileType = Drawing.type == null ? string.Empty : Drawing.type.name == null ? string.Empty : Drawing.type.name;

                    string PreFix = "";
                    if (ProjectNo.Trim().Length > 0)
                    {
                        PreFix = ProjectNo + "-";
                    }
                    PreFix = PreFix + Drawing.fileNo + "-";

                    PreFix += Convert.ToString(FileType) == string.Empty ? string.Empty : Convert.ToString(FileType) + "-";

                    PreFix += Convert.ToString(Drawing.versionno) == string.Empty ? string.Empty : Convert.ToString(Drawing.versionno) + "#";

                    DrawingProperty.Add("prefix", PreFix);
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
            }
            return DrawingProperty;
        }
        public List<ResultSearchCriteria> GetXrefFIleInfo(String drawingid)
        {
            List<ResultSearchCriteria> ObjFileInfo = null;
            try
            {

                KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", drawingid);
                //KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", "11760c31-d3fb-4acb-9675-551915493fd5");

                List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                urlParameters.Add(L);


                KeyValuePair<string, string> L1 = new KeyValuePair<string, string>("userName", Helper.UserName);
                urlParameters.Add(L1);
                RestResponse restResponse = (RestResponse)ServiceHelper.GetData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                    "/AutocadFiles/getAssoFile",
                    false, urlParameters);




                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error occurred while fetching file information.", restResponse);
                    return null;
                }
                else
                {
                    ObjFileInfo = JsonConvert.DeserializeObject<List<ResultSearchCriteria>>(restResponse.Content);

                    string Response = restResponse.Content;

                }

            }
            catch (Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
            }
            return ObjFileInfo;
        }

        public ResultSearchCriteria GetSingleFileInfo(string fileID, ref byte[] Rowdata)
        {
            ResultSearchCriteria Drawing = null;
            try
            {

                RestResponse restResponse = (RestResponse)ServiceHelper.GetData(
                         Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                         "/AutocadFiles/downloadAutocadSingleFile",
                         false,
                         new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("fileId",fileID ) ,
                                    new KeyValuePair<string, string>("userName", Helper.UserName)

                         });
                if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    RBConnector objRBC = new RBConnector();
                    Drawing = objRBC.GetDrawingInformation(fileID);

                    Rowdata = restResponse.RawBytes;
                }
                else
                {
                    ShowMessage.ErrorMess("Some error while retrieving file.", restResponse);
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return Drawing;
        }
        public byte[] GetSingleFileInfo(string fileID)
        {

            try
            {

                RestResponse restResponse = (RestResponse)ServiceHelper.GetData(
                         Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                         "/AutocadFiles/downloadAutocadSingleFile",
                         false,
                         new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("fileId",fileID ) ,
                                    new KeyValuePair<string, string>("userName", Helper.UserName)

                         });
                if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return restResponse.RawBytes;
                }
                else
                {
                    ShowMessage.ErrorMess("Some error while retrieving file.", restResponse);
                    return null;

                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return null;
            }

        }


        /// <summary>
        /// This function takes oldversion's file No and return latest version's fileid
        /// </summary>
        /// <param name="OldFileID"></param>
        /// <returns></returns>
        public string SearchLatestFile(string OldFileNo)
        {
            try
            {

                SearchCriteria searchCriteria = new SearchCriteria();
                searchCriteria.fileNo = OldFileNo;


                var resultSearchCriteriaResponseList = SearchFiles(searchCriteria);

                return resultSearchCriteriaResponseList == null ? string.Empty : resultSearchCriteriaResponseList.Count > 0 ? resultSearchCriteriaResponseList[0].id : string.Empty;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return string.Empty;
            }
        }

        public List<ResultSearchCriteria> SearchFiles(SearchCriteria searchCriteria, List<KeyValuePair<string, string>> urlParameters = null)
        {
            try
            {


                RestResponse restResponse = (RestResponse)ServiceHelper.PostData(
                Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
               "/AutocadFiles/searchAutocadFiles",
               DataFormat.Json,
               searchCriteria,
               true,
               urlParameters);
                if (restResponse == null || restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error while searching file.", restResponse);
                    return null;
                }
                else
                {
                    var resultSearchCriteriaResponseList = JsonConvert.DeserializeObject<List<ResultSearchCriteria>>(restResponse.Content);

                    return resultSearchCriteriaResponseList;
                }

            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);

            }
            return null;
        }

        public List<ResultSearchCriteria> SearchLatest5File()
        {
            try
            {
                RestResponse restResponse = (RestResponse)ServiceHelper.GetData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), "/AutocadFiles/getLatestRecords", true, null);

                if (restResponse == null || restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error occurred while fetching latest records.", restResponse);
                    return null;
                }
                else
                {

                    return JsonConvert.DeserializeObject<List<ResultSearchCriteria>>(restResponse.Content);

                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return null;
            }
        }

        public int GetSearchFileCount(SearchCriteria searchCriteria, List<KeyValuePair<string, string>> urlParameters)
        {
            try
            {
                //RestResponse restResponse = (RestResponse)ServiceHelper.GetData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), "/AutocadFiles/searchAutocadFilesCount", true, urlParameters);
                RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), "/AutocadFiles/searchAutocadFilesCount", DataFormat.Json, searchCriteria, true, urlParameters);

                if (restResponse == null || restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    //ShowMessage.ErrorMess("Some error occurred while fetching latest records.", restResponse);
                    return 51;
                }
                else
                {

                    //return JsonConvert.DeserializeObject<List<ResultSearchCriteria>>(restResponse.Content);
                    return Convert.ToInt32(restResponse.Content);
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return 0;
            }
        }

        public System.Net.HttpStatusCode LoginValidation(string UserName, string Passwd, out UserDetails loggedUserDetails)
        {
            try
            {

                RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(), "/Login/login", DataFormat.Json, new UserLoginDetails
                {
                    email = UserName,
                    password = Passwd
                }, false);
                if (restResponse == null || restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error occurred while fetching latest records.", restResponse);
                    loggedUserDetails = null;
                    return restResponse.StatusCode;
                }
                else
                {
                    loggedUserDetails = JsonConvert.DeserializeObject<UserDetails>(restResponse.Content);
                    //return JsonConvert.DeserializeObject<List<ResultSearchCriteria>>(restResponse.Content);
                    return restResponse.StatusCode;
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                loggedUserDetails = null;
                return 0;
            }
        }

        public bool CheckFileExistance(String drawingid)
        {
            ResultSearchCriteria ObjFileInfo = null;
            try
            {
                KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", drawingid);

                List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                urlParameters.Add(L);
                RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                    "/AutocadFiles/fetchFileInfo", DataFormat.Json,
                    null, true, urlParameters);


                //Helper.CloseProgressBar();
                if (restResponse == null || restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    if (restResponse.StatusCode != System.Net.HttpStatusCode.InternalServerError)
                        ShowMessage.ErrorMess("Some error occurred while fetching file information.", restResponse);
                    return false;
                }
                else
                {
                    ObjFileInfo = JsonConvert.DeserializeObject<ResultSearchCriteria>(restResponse.Content);
                    return ObjFileInfo == null || ObjFileInfo.id == null || ObjFileInfo.id.Trim().Length == 0 ? false : true;

                }

            }
            catch (Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
            }
            return false;
        }
    }

}
