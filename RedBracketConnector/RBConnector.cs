using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Aras.IOM;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using Newtonsoft.Json;
using RestSharp;
using System.Web.Script.Serialization;


namespace RedBracketConnector
{
    public class RBConnector
    {

        public void SaveObject(string FilePath)
        {
            try
            {
                KeyValuePair<string, string> L = new KeyValuePair<string, string>("filepath", FilePath);

                List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                urlParameters.Add(L);
                RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                    "/AutocadFiles/uploadFileService", DataFormat.Json,
                    null, true, urlParameters);
                if (restResponse == null)
                {
                    ShowMessage.ErrorMess("Some error occurred while uploading file.");
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        public bool SaveObject(ref List<PLMObject> plmobjs, bool IsFileSave = false)
        {
            try
            {
                //To unlock File in case of update
                //if (UnlockObject(plmobjs))
                //{
                //  return false;
                //}
                string ParentFileID = "";
                foreach (PLMObject obj in plmobjs)
                {

                    SaveFileCommand objSFC = new SaveFileCommand();
                    RestResponse restResponse;
                    //service calling to upload document.
                    String PreFix = obj.PreFix;
                    // if (obj.IsRoot)
                    {
                        if (!File.Exists(obj.FilePath))
                        {
                            ShowMessage.InfoMess(obj.FilePath + Environment.NewLine + "File not found.");
                            return false;
                        }

                        //     restResponse = (RestResponse)ServiceHelper.SaveObject(
                        //Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                        //"/AutocadFiles/uploadFileService", obj.FilePath,
                        //     true, new List<KeyValuePair<string, string>> {
                        //          new KeyValuePair<string, string>("project", obj.ObjectProjectId) ,
                        //          new KeyValuePair<string, string>("fileStatus", obj.ObjectStatus),
                        //          new KeyValuePair<string, string>("fileType", obj.Classification),
                        //           new KeyValuePair<string, string>("fileId", obj.ObjectId),
                        //           new KeyValuePair<string, string>("isNew","true")
                        //     }, obj.PreFix, true);



                        if (IsFileSave)
                        {
                            string IsNew = "false";

                            if (obj.FolderID.Trim().Length > 0)
                            {
                                //Keys = new KeyValuePair<string, string>("folderid", obj.FolderID.Trim());
                            }

                            //save =false
                            restResponse = (RestResponse)ServiceHelper.SaveObject(
                                            Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                                            "/AutocadFiles/uploadFileService", obj.FilePath,
                                            true, new List<KeyValuePair<string, string>>
                                            {       new KeyValuePair<string, string>("fileId", obj.ObjectId),
                                                    new KeyValuePair<string, string>("isNew",Convert.ToString(IsNew).ToLower()),
                                                    new KeyValuePair<string, string>("folderidString", obj.FolderID.Trim())
                                            }, PreFix, true);

                            if (!obj.IsRoot && obj.IsNew)
                            {
                                List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                                KeyValuePair<string, string> Keys = new KeyValuePair<string, string>();


                                Keys = new KeyValuePair<string, string>("parentFileId", ParentFileID);
                                keyValuePairs.Add(Keys);

                                Keys = new KeyValuePair<string, string>("childFileId", obj.ObjectId);
                                keyValuePairs.Add(Keys);


                                // http://redbracketpms.com:8090/red-bracket-pms/AutocadFiles/uploadFileServiceProp?userName=archi@yopmail.com&project=0&fileStatus=905&fileType=692&source=Computer&fileId=9d5c029d-f085-427d-a199-f899f35ec8e7&isNew=false
                                restResponse = (RestResponse)ServiceHelper.PostData(
                                                Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                                                "/AutocadFiles/uploadXRef", DataFormat.Json, null,
                                                     true, keyValuePairs);
                            }
                            else
                            {
                                ParentFileID = obj.ObjectId;
                            }
                        }
                        else
                        {
                            List<KeyValuePair<string, string>> keyValuePairs = new List<KeyValuePair<string, string>>();
                            KeyValuePair<string, string> Keys = new KeyValuePair<string, string>();


                            Keys = new KeyValuePair<string, string>("project", obj.ObjectProjectId);
                            keyValuePairs.Add(Keys);

                            Keys = new KeyValuePair<string, string>("fileStatus", obj.ObjectStatus);
                            keyValuePairs.Add(Keys);

                            Keys = new KeyValuePair<string, string>("fileType", obj.Classification);
                            keyValuePairs.Add(Keys);

                            Keys = new KeyValuePair<string, string>("source", "Computer");
                            keyValuePairs.Add(Keys);
                            string s = Path.GetExtension(obj.FilePath);
                            Keys = new KeyValuePair<string, string>("fileName", obj.ObjectName + s);
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
                            if (obj.FolderID.Trim().Length > 0)
                            {
                                Keys = new KeyValuePair<string, string>("folderid", obj.FolderID.Trim());
                            }
                            keyValuePairs.Add(Keys);
                            // http://redbracketpms.com:8090/red-bracket-pms/AutocadFiles/uploadFileServiceProp?userName=archi@yopmail.com&project=0&fileStatus=905&fileType=692&source=Computer&fileId=9d5c029d-f085-427d-a199-f899f35ec8e7&isNew=false
                            restResponse = (RestResponse)ServiceHelper.SaveObject(
                                            Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                                            "/AutocadFiles/uploadFileServiceProp", obj.FilePath,
                                                 true, keyValuePairs, obj.PreFix, false);
                        }





                    }
                    //else
                    //{
                    //    if (!File.Exists(obj.FilePath))
                    //    {
                    //        ShowMessage.InfoMess(obj.FilePath + Environment.NewLine + "File not found.");
                    //        continue;
                    //    }



                    //    //for save
                    //    if (obj.ObjectId.Trim().Length == 0)
                    //    {
                    //        restResponse = (RestResponse)ServiceHelper.SaveObject(
                    //                    Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                    //                    "/AutocadFiles/uploadXRefFiles", obj.FilePath,
                    //                      false, new List<KeyValuePair<string, string>> {
                    //                             new KeyValuePair<string, string>("source", "Computer") ,
                    //                              new KeyValuePair<string, string>("userName", Helper.UserName) ,
                    //                        new KeyValuePair<string, string>("parentFileId", ParentFileID) ,
                    //                        new KeyValuePair<string, string>("project", obj.ObjectProjectId)  ,
                    //                        new KeyValuePair<string, string>("xrefFileId", obj.ObjectId.Trim()),
                    //                        new KeyValuePair<string, string>("fileStatus", obj.ObjectStatus),
                    //                        new KeyValuePair<string, string>("fileType", obj.Classification),
                    //                        new KeyValuePair<string, string>("isNew", "false") }, obj.PreFix);
                    //    }
                    //    else
                    //    {
                    //        restResponse = (RestResponse)ServiceHelper.SaveObject(
                    //                        Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                    //                        "/AutocadFiles/uploadFileService", obj.FilePath,
                    //                             true, new List<KeyValuePair<string, string>> {
                    //                                                    new KeyValuePair<string, string>("project", obj.ObjectProjectId) ,
                    //                                                    new KeyValuePair<string, string>("fileStatus", obj.ObjectStatus),
                    //                                                    new KeyValuePair<string, string>("fileType", obj.Classification),
                    //                                                     new KeyValuePair<string, string>("fileId", obj.ObjectId)
                    //                             }, obj.PreFix);
                    //    }



                    //}


                    //checking if service call was successful or not.
                    if (restResponse.StatusCode != System.Net.HttpStatusCode.OK && restResponse.StatusCode != System.Net.HttpStatusCode.NotAcceptable)
                    {
                        //ShowMessage.InfoMess(restResponse.Content);
                        //ShowMessage.InfoMess(restResponse.ResponseUri.ToString());
                        ShowMessage.ErrorMess("Some error occurred while uploading file.");
                        return false;
                    }
                    else if (restResponse.Content.Trim().Length > 0)
                    {
                        if (restResponse.StatusCode == System.Net.HttpStatusCode.NotAcceptable)
                        {
                        }
                        else
                        //  if (Convert.ToString(obj.ObjectId).Trim().Length > 0)
                        {


                            SaveResult saveResult = new JavaScriptSerializer().Deserialize<SaveResult>(restResponse.Content);
                            //var saveObjectResponseValueObject = JsonConvert.DeserializeObject< DataofData>(saveResult.dataofdata.Replace("[", "").Replace("]", ""));

                            //var saveObjectResponseValueObject1 = JsonConvert.DeserializeObject<List< DataofData>>(saveResult.dataofdata );
                            //var saveObjectResponseValueObject = saveObjectResponseValueObject1[0];
                            //var Drawing = saveObjectResponseValueObject;


                            bool isError = false;
                            try
                            {
                                //var Drawing3 = JsonConvert.DeserializeObject<DataofData>(saveResult.dataofdata);
                                try
                                {
                                    var Drawing1 = JsonConvert.DeserializeObject<List<DataofData>>(saveResult.dataofdata)[0];
                                }
                                catch
                                {
                                    isError = true;
                                    var Drawing2 = JsonConvert.DeserializeObject<DataofData>(saveResult.dataofdata);
                                }

                            }
                            catch (Exception E)
                            {

                            }


                            var Drawing = isError ? JsonConvert.DeserializeObject<DataofData>(saveResult.dataofdata) : JsonConvert.DeserializeObject<List<DataofData>>(saveResult.dataofdata)[0];

                            if (Drawing != null)
                            {
                                if (obj.IsRoot)
                                    obj.ObjectId = ParentFileID = Drawing.id;
                                else
                                    obj.ObjectId = Drawing.id;

                                obj.ObjectName = Drawing.name;
                                obj.Classification = Drawing.type == null ? string.Empty : Convert.ToString(Drawing.type.id);
                                obj.ObjectState = Drawing.status == null ? string.Empty : Drawing.status.statusname == null ? string.Empty : Drawing.status.statusname;
                                obj.ObjectRevision = Drawing.versionno.Contains("Ver ") ? Drawing.versionno.Substring(4) : Drawing.versionno;
                                obj.LockStatus = Convert.ToString(Drawing.filelock);
                                obj.ObjectGeneration = "123";
                                obj.ItemType = Drawing.coreType == null ? string.Empty : Drawing.coreType.name;
                                obj.ObjectProjectName = Drawing.projectname == null ? string.Empty : Drawing.projectname;
                                //obj.ObjectProjectId = "123";
                                obj.ObjectCreatedById = Drawing.createdby;
                                obj.ObjectCreatedOn = Drawing.created0n;
                                obj.ObjectModifiedById = Drawing.updatedby;
                                obj.ObjectModifiedOn = Drawing.updatedon;
                                obj.ObjectNumber = Drawing.fileNo == null ? string.Empty : Drawing.fileNo;

                                obj.canDelete = Drawing.canDelete;
                                obj.isowner = Drawing.isowner;
                                obj.hasViewPermission = Drawing.hasViewPermission;
                                obj.isActFileLatest = Drawing.isActFileLatest;
                                obj.isEditable = Drawing.isEditable;
                                obj.canEditStatus = Drawing.canEditStatus;
                                obj.hasStatusClosed = Drawing.hasStatusClosed;
                                obj.isletest = Drawing.isletest;
                                obj.objectType = Drawing.type == null ? string.Empty : Convert.ToString(Drawing.type.name);
                                

                            }


                        }
                        //else
                        //{
                        //    try
                        //    {
                        //        var Drawing1 = JsonConvert.DeserializeObject<SaveResult>(restResponse.Content);
                        //    }
                        //    catch (Exception E)
                        //    {

                        //    }
                        //  var Drawing = JsonConvert.DeserializeObject<ResultSearchCriteria>(restResponse.Content);

                        //    obj.ObjectId = ParentFileID = Drawing.id;
                        //    obj.ObjectName = Drawing.name;
                        //    obj.Classification = Drawing.coreType.name;
                        //    obj.ObjectState = Drawing.status.statusname;
                        //    obj.ObjectRevision = Drawing.versionno;
                        //    obj.LockStatus = Convert.ToString(Drawing.filelock);
                        //    obj.ObjectGeneration = "123";
                        //    obj.ItemType = Drawing.coreType.name;
                        //    obj.ObjectProjectName = Drawing.projectname;
                        //    obj.ObjectProjectId = "123";
                        //    obj.ObjectCreatedById = Drawing.createdby;
                        //    obj.ObjectCreatedOn = Drawing.updatedon;
                        //    obj.ObjectModifiedById = Drawing.updatedby;
                        //    obj.ObjectModifiedOn = Drawing.updatedon;
                        //    obj.ObjectNumber = Drawing.fileNo;
                        //}
                    }




                }
                UnlockObject(plmobjs);
                return true;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return false;
            }

        }

        public bool UpdateFileProperties(string FileID, string Type, string Status, string Project, string FilePath, string PreFix)
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
                    ShowMessage.ErrorMess("Some error occurred while updating file properties.");
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
                    ShowMessage.ErrorMess("Some error occurred while searching for folder.");

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




                SaveFileCommand objSFC = new SaveFileCommand();
                RestResponse restResponse;
                //service calling 



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
                    ShowMessage.ErrorMess("Some error occurred while uploading layout info.");
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
                    ShowMessage.ErrorMess("Some error occurred while checking for file duplication.");
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

        public bool SaveUpdateLayoutInfo(DataTable dtLayoutInfo, string ProjectID, string Fileid)
        {
            try
            {

                foreach (DataRow dr in dtLayoutInfo.Rows)
                {
                    if (/*Convert.ToString(dr["IsFile"]) == "1" ||*/ (Convert.ToString(dr["ChangeVersion"]) == "False"))
                    {
                        continue;
                    }

                    SaveFileCommand objSFC = new SaveFileCommand();
                    RestResponse restResponse;
                    //service calling to upload document.

                    if (Convert.ToString(dr["LayoutID"]).Trim().Length == 0)
                    {


                        restResponse = (RestResponse)ServiceHelper.PostData(
                  Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                  "/AutocadFiles/uploadLayoutACFiles", DataFormat.Json, null, true
                     , new List<KeyValuePair<string, string>> {
                                         new KeyValuePair<string, string>("fileId", Fileid),
                                              new KeyValuePair<string, string>("layoutId", Convert.ToString(dr["ACLayoutID"]).Trim()),
                                                 new KeyValuePair<string, string>("layoutFileName",  Convert.ToString(dr["LayoutName1"]).Trim()),
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
                                                    new KeyValuePair<string, string>("isNew", "true")
                                                 });
                    }


                    //checking if service call was successful or not.
                    if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        //ShowMessage.InfoMess(restResponse.Content);
                        //ShowMessage.InfoMess(restResponse.ResponseUri.ToString());
                        ShowMessage.ErrorMess("Some error occurred while uploading layout info.");
                        return false;
                    }
                    else if (restResponse.Content.Trim().Length > 0)
                    {
                        SaveResult saveResult = new JavaScriptSerializer().Deserialize<SaveResult>(restResponse.Content);


                        var Drawing = JsonConvert.DeserializeObject<DataofData>(saveResult.dataofdata);
                        if (Convert.ToString(dr["LayoutID"]).Trim().Length == 0)
                        {
                            dr["ACLayoutID"] = Drawing.id;
                        }



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
                    //ShowMessage.InfoMess(restResponse.Content);
                    //ShowMessage.InfoMess(restResponse.ResponseUri.ToString());
                    ShowMessage.ErrorMess("Some error occurred while checking for file duplication.");
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
        public DataTable GetLayoutInfo(string FileID)
        {
            DataTable dtLayoutInfoRB = new DataTable();
            try
            {

                RestResponse restResponse = (RestResponse)ServiceHelper.GetData(
                         Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                         "/AutocadFiles/downloadAutocadSingleFile",
                         false,
                         new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("fileId",FileID ) ,
                                    new KeyValuePair<string, string>("userName", Helper.UserName)

                         });
                if (restResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    RBConnector objRBC = new RBConnector();
                    //Drawing = objRBC.GetDrawingInformation(FileID); 
                    //Rowdata = restResponse.RawBytes;
                }
                else
                {
                    ShowMessage.ErrorMess("Some error while retrieving file.");
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dtLayoutInfoRB;
        }

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
                        MessageBox.Show("Some error occurred while fetching file information.");
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
                //throw (new Exceptions.ConnectionException("ArasConnector Exception Message :" + ex.Message));
            }
            return newplmobjs;
        }

        public DataTable GetFIleType()
        {
            DataTable dataTableProjectInfo = new DataTable();
            try
            {
                dataTableProjectInfo = GetDataFromWS("/AutocadFiles/fetchFileType", "file type", "POST", typeof(List<ResultStatusData>));
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
                dataTableFileStatus = GetDataFromWS("/AutocadFiles/fetchFileStatus", "file status", "POST", typeof(List<ResultStatusData>));

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
                dataTableProjectInfo = GetDataFromWS("/ProjectAutocad/fetchUserAutocadProjectsService", "project detail", "GET");

                if (dataTableProjectInfo != null)
                {
                    dataTableProjectInfo.Columns.Add("PNAMENO");

                    for (int i = 0; i < dataTableProjectInfo.Rows.Count; i++)
                    {
                        dataTableProjectInfo.Rows[i]["PNAMENO"] = Convert.ToString(dataTableProjectInfo.Rows[i]["name"]) + " (" + Convert.ToString(dataTableProjectInfo.Rows[i]["number"]) + ")";
                    }
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
                    MessageBox.Show("Some error occurred while retrieving the " + MessageText + ".");
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
                            dataTableProjectInfo = Helper.ToDataTable(ObjFileInfo);
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
                    MessageBox.Show("Some error occurred while retrieving the " + MessageText + ".");
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
                        #region Commented Code
                        //Item lockStatusQry = null;
                        //Item lockStatusRes = null;
                        //lockStatusQry = myInnovator.newItem(rw["type"].ToString(), "get");
                        //lockStatusQry.setProperty("id", rw["drawingid"].ToString());
                        //lockStatusQry.setAttribute("select", "locked_by_id, id");
                        //lockStatusRes = lockStatusQry.apply();

                        //if (lockStatusRes.isError())
                        //{
                        //    throw (new Exceptions.ConnectionException("Exception occured in 'LockStatus' method.\n Error string is :" + lockStatusRes.getErrorString()));
                        //}
                        //rw["lockstatus"] = lockStatusRes.getLockStatus().ToString();
                        //rw["drawingid"] = (String)lockStatusRes.getProperty("id");
                        //if (lockStatusRes.getLockStatus() == 2)
                        //    rw["lockby"] = GetLockBy(lockStatusRes.getProperty("locked_by_id"));
                        //else
                        //    rw["lockby"] = " ";
                        #endregion

                        KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", rw["drawingid"].ToString());
                        // KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", "11760c31-d3fb-4acb-9675-551915493fd5");

                        List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                        urlParameters.Add(L);
                        RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                            "/AutocadFiles/fetchFileInfo", DataFormat.Json,
                            null, true, urlParameters);

                        ResultSearchCriteria ObjFileInfo = JsonConvert.DeserializeObject<ResultSearchCriteria>(restResponse.Content);


                        if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            MessageBox.Show("Some error occurred while fetching file status.");
                        }
                        else
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
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("Exception Message :" + ex.Message));

            }
        }

        public void LockObject(List<PLMObject> plmObjs)
        {
            try
            {
                bool IsUpdated = true;
                foreach (PLMObject plmObj in plmObjs)
                {
                    #region Commented Code
                    //Item drawingQuery = myInnovator.newItem(plmObj.ItemType, "get");
                    //Item drawingQueryRes = null;

                    //drawingQuery.setProperty("id", plmObj.ObjectId);
                    //drawingQueryRes = drawingQuery.apply();
                    //bool is_need = true;
                    //if (drawingQueryRes.getProperty("is_current") != "1")
                    //{
                    //    if (MessageBox.Show("Aras has updated Version of Drawing " + drawingQueryRes.getProperty("name").ToString() + ", Eventhough Would you like to update it?", "Aras has updated Version of this Drawing", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    //        is_need = true;
                    //    else
                    //        is_need = false;
                    //}
                    //if (is_need)
                    //{
                    //    if (drawingQueryRes.lockItem().isError())
                    //    {
                    //        throw (new Exceptions.ConnectionException("Exception occured in 'LockObject' method.\n Error string is :" + drawingQueryRes.lockItem().getErrorString()));
                    //    }
                    //}
                    //else
                    //{
                    //    throw (new Exceptions.ConnectionException("Please download latest Version of Drawing from Aras!!!"));
                    //}

                    #endregion

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
                        MessageBox.Show("Some error occurred while locking file.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("RedBracketConnectorS Exception Message : " + ex.Message));
            }
        }

        public bool UnlockObject(List<PLMObject> plmObjs)
        {
            try
            {
                foreach (PLMObject plmObj in plmObjs)
                {
                    #region Commented Code
                    //Item drawingQuery = myInnovator.newItem(plmObj.ItemType, "get");
                    //Item drawingQueryRes = null;

                    //drawingQuery.setProperty("id", plmObj.ObjectId);
                    //drawingQueryRes = drawingQuery.apply();
                    //if (drawingQueryRes.unlockItem().isError())
                    //{
                    //    throw (new Exceptions.ConnectionException("Exception occured in 'UnlockObject' method.\n Error string is :" + drawingQueryRes.unlockItem().getErrorString()));
                    //}
                    #endregion

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
                        MessageBox.Show("Some error occurred while unlocking file.");
                        return true;
                    }

                }
            }
            catch (Exception ex)
            {
                //  throw (new Exceptions.ConnectionException("RedBracketConnector Exception Message : " + ex.Message));
                ShowMessage.ErrorMess(ex.Message);
                return false;
            }
            return false;
        }

        public Hashtable GetDrawingDetail(String drawingid)
        {
            Hashtable DrawingInfo = new Hashtable();
            ResultSearchCriteria Drawing = new ResultSearchCriteria();
            Drawing = GetDrawingInformation(drawingid);

            if (Drawing != null)
            {
                DrawingInfo.Add("createdby", Drawing.createdby);
                DrawingInfo.Add("createdon", Drawing.updatedon);
                DrawingInfo.Add("modifiedby", Drawing.updatedby);
                DrawingInfo.Add("modifiedon", Drawing.updatedon);
            }
            else
            {
                DateTime dt = DateTime.Now;

                DrawingInfo.Add("createdby", Helper.UserFullName);
                DrawingInfo.Add("createdon", dt.ToString());
                DrawingInfo.Add("modifiedby", Helper.UserFullName);
                DrawingInfo.Add("modifiedon", dt.ToString());
            }
            return DrawingInfo;
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





                if (restResponse == null || restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ShowMessage.ErrorMess("Some error occurred while fetching file information.");
                    return null;
                }
                else
                {
                    ObjFileInfo = JsonConvert.DeserializeObject<ResultSearchCriteria>(restResponse.Content);
                    string Response = restResponse.Content;
                    #region Commented Code
                    //DataTable dataTableFileInfo = (DataTable)JsonConvert.DeserializeObject(restResponse.Content, (typeof(DataTable)));
                    //if(dataTableFileInfo.Rows.Count>0)
                    //{
                    //    bool FileLock = Convert.ToBoolean(Convert.ToString(dataTableFileInfo.Rows[0]["filelock"]));
                    //    string UpdatedBy = Convert.ToString(dataTableFileInfo.Rows[0]["updatedBy"]);

                    //    if(FileLock)
                    //    {
                    //        if(UpdatedBy== Helper.UserName)
                    //        {
                    //            rw["lockstatus"] = "1";
                    //            rw["lockby"] = dataTableFileInfo.Rows[0]["updatedBy"];
                    //        }
                    //        else
                    //        {
                    //            rw["lockstatus"] = "2";
                    //            rw["lockby"] = dataTableFileInfo.Rows[0]["updatedBy"];
                    //        }
                    //    }
                    //    else
                    //    {

                    //    }
                    //}
                    #endregion
                }

            }
            catch (Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
                //throw (new Exceptions.ConnectionException("ArasConnector Exception Message :" + ex.Message));
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
                    ShowMessage.ErrorMess("Some error occurred while fetching file information.");
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
                    DrawingProperty.Add("Generation", "123");
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
                //throw (new Exceptions.ConnectionException("ArasConnector Exception Message :" + ex.Message));
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
                    ShowMessage.ErrorMess("Some error occurred while fetching file information.");
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
                //throw (new Exceptions.ConnectionException("ArasConnector Exception Message :" + ex.Message));
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
                    ShowMessage.ErrorMess("Some error while retrieving file.");
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return Drawing;
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
                RestResponse restResponse = (RestResponse)ServiceHelper.PostData(
                Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
               "/AutocadFiles/searchAutocadFiles",
               DataFormat.Json,
               searchCriteria,
               true,
               null);

                var resultSearchCriteriaResponseList = JsonConvert.DeserializeObject<List<ResultSearchCriteria>>(restResponse.Content);

                return resultSearchCriteriaResponseList == null ? string.Empty : resultSearchCriteriaResponseList.Count > 0 ? resultSearchCriteriaResponseList[0].id : string.Empty;
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return string.Empty;
            }
        }
    }
    public class SaveFileCommand
    {
        private BinaryReader bRDocument;
        public BinaryReader BRDocument
        {
            get { return this.bRDocument; }
            set { this.bRDocument = value; }
        }
    }
}
