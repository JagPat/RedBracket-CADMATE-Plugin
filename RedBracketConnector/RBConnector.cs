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

        public bool SaveObject(ref List<PLMObject> plmobjs, string FilePath)
        {
            try
            {
                //To unlock File in case of update
                if (UnlockObject(plmobjs))
                {
                    return false;
                }

                foreach (PLMObject obj in plmobjs)
                {
                    //KeyValuePair<string, string> L = new KeyValuePair<string, string>("filepath", FilePath);
                    SaveFileCommand objSFC = new SaveFileCommand();

                    // to convert file in bytes.
                    using (var binaryReader = new BinaryReader(File.OpenRead(FilePath)))
                    {
                        binaryReader.Read();
                        objSFC.BRDocument = binaryReader;
                    }

                    //Defining parameters
                    KeyValuePair<string, string> L = new KeyValuePair<string, string>("project", obj.ObjectProjectId);

                    List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                    urlParameters.Add(L);

                    //service calling to upload document.
                    RestResponse restResponse = (RestResponse)ServiceHelper.SaveObject(
                        Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                        "/AutocadFiles/uploadFileService", obj.FilePath,
                        obj.NativeFileName, true, new List<KeyValuePair<string, string>> {
                            new KeyValuePair<string, string>("project", obj.ObjectProjectId)
                });

                    SaveResult saveResult = new JavaScriptSerializer().Deserialize<SaveResult>(restResponse.Content);

                    var saveObjectResponseValueObject = JsonConvert.DeserializeObject<DataofData>(saveResult.dataofdata.Replace("[", "").Replace("]", ""));

                    //checking if service call was successful or not.
                    if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        ShowMessage.ErrorMess("Some error occurred while uploading file.");
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return false;
            }
            #region Old Code
            //Hashtable SavedData = new Hashtable();
            //try
            //{
            //    String sourceid = "";
            //    String newDrawingId = "";
            //    String ProjectName = "";


            //    #region "Release PLMObjects"
            //    //ReleasePLMObjects(plmobjs);
            //    #endregion

            //    #region "Save PLMObjects"
            //    foreach (PLMObject obj in plmobjs)
            //    {
            //        Item cadDocument_QRY = null;
            //        Item cadDocument_RES = null;
            //        Array Layouts = obj.ObjectLayouts.Split('$');

            //        #region "If Drawing is in ARASInnovator"

            //        if (!obj.IsNew)
            //        {
            //            if ((ProjectName == null || ProjectName == "") && (obj.ObjectId != null || obj.ObjectId != ""))
            //            {
            //                // Get this info by get file info
            //                Item MyCAD = myInnovator.getItemById(obj.ItemType, obj.ObjectId);
            //                ProjectName = MyCAD.getItemByIndex(0).getProperty("sg_project_name");
            //            }

            //            #region CreateManualVersion
            //            if (obj.IsManualVersion)
            //            {
            //                //
            //                cadDocument_QRY = myInnovator.newItem(obj.ItemType, "get");
            //                cadDocument_QRY.setProperty("id", obj.ObjectId);
            //                cadDocument_RES = cadDocument_QRY.apply();
            //                if (cadDocument_RES.isError())
            //                {
            //                    throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + cadDocument_RES.getErrorString()));
            //                }
            //                string prevFileId = cadDocument_RES.getItemByIndex(0).getProperty("native_file");
            //                Item fileItem = myInnovator.newItem("File", "add");
            //                fileItem.setAttribute("where", "id='" + prevFileId + "'");
            //                string seperatefilename = Path.GetFileName(obj.FilePath);
            //                fileItem.setProperty("filename", seperatefilename);
            //                fileItem.setID(myInnovator.getNewID());
            //                fileItem.attachPhysicalFile(obj.FilePath);
            //                Item checkin_result = fileItem.apply();
            //                string newFileId = checkin_result.getItemByIndex(0).getProperty("id");
            //                if (checkin_result.isError())
            //                {
            //                    throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + checkin_result.getErrorString()));
            //                }
            //                if (obj.IsCreateNewRevision)
            //                {
            //                    cadDocument_RES.unlockItem();
            //                    if (cadDocument_RES.apply("PE_CreateNewRevision").isError())
            //                    {
            //                        throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + cadDocument_RES.apply("PE_CreateNewRevision").getErrorString()));
            //                    }

            //                }
            //                else
            //                {
            //                    //if (cadDocument_RES.getProperty("is_released").ToString() != "1")
            //                    if (cadDocument_RES.apply("version").isError())
            //                    {
            //                        throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + cadDocument_RES.apply("version").getErrorString()));
            //                    }
            //                }
            //                cadDocument_QRY = myInnovator.newItem(obj.ItemType, "get");
            //                cadDocument_QRY.setProperty("item_number", cadDocument_RES.getProperty("item_number"));
            //                cadDocument_QRY.setProperty("is_current", "1");
            //                cadDocument_RES = cadDocument_QRY.apply();
            //                if (cadDocument_RES.isError())
            //                {
            //                    throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is : " + cadDocument_RES.getErrorString()));
            //                }
            //                Item newCadFile = myInnovator.newItem(obj.ItemType, "edit");
            //                newCadFile.setAttribute("where", "id='" + cadDocument_RES.getProperty("id").ToString() + "'");
            //                newCadFile.setProperty("native_file", newFileId);
            //                if (obj.ObjectDescription.ToString().Length > 0)
            //                    newCadFile.setProperty("description", obj.ObjectDescription);
            //                Item newCadFile_RES = newCadFile.apply();
            //                newCadFile_RES.unlockItem();
            //                SavedData.Add(seperatefilename, obj.ObjectId);
            //                if (newCadFile_RES.isError())
            //                {
            //                    throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + newCadFile_RES.getErrorString()));
            //                }
            //                newDrawingId = newCadFile_RES.getProperty("id");
            //            }
            //            #endregion CreateManualVersion

            //            #region NoManualVersion
            //            else
            //            {
            //                cadDocument_QRY = myInnovator.newItem(obj.ItemType, "get");
            //                cadDocument_QRY.setProperty("id", obj.ObjectId);
            //                cadDocument_RES = cadDocument_QRY.apply();
            //                if (cadDocument_RES.isError())
            //                {
            //                    throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + cadDocument_RES.getErrorString()));
            //                }
            //                string prevFileId = cadDocument_RES.getItemByIndex(0).getProperty("native_file");

            //                Item fileItem = myInnovator.newItem("File", "add");
            //                fileItem.setAttribute("where", "id='" + prevFileId + "'");
            //                string seperatefilename = Path.GetFileName(obj.FilePath);
            //                fileItem.setProperty("filename", seperatefilename);
            //                fileItem.setID(myInnovator.getNewID());
            //                fileItem.attachPhysicalFile(obj.FilePath);
            //                Item checkin_result = fileItem.apply();
            //                string newFileId = checkin_result.getItemByIndex(0).getProperty("id");
            //                if (checkin_result.isError())
            //                {
            //                    throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject NoManualVersion' method.\n Error string is :" + checkin_result.getErrorString()));
            //                }
            //                Item newCadFile = myInnovator.newItem(obj.ItemType, "edit");
            //                newCadFile.setAttribute("where", "id='" + cadDocument_RES.getProperty("id").ToString() + "'");
            //                newCadFile.setProperty("native_file", newFileId);
            //                if (obj.ObjectDescription.ToString().Length > 1)
            //                    newCadFile.setProperty("description", obj.ObjectDescription);
            //                Item newCadFile_RES = newCadFile.apply();
            //                newCadFile_RES.unlockItem();
            //                if (newCadFile_RES.isError())
            //                {
            //                    throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject NoManualVersion' method.\n Error string is :" + newCadFile_RES.getErrorString()));
            //                }
            //                newDrawingId = newCadFile_RES.getProperty("id");
            //            }
            //            #endregion NoManualVersion
            //        }

            //        #endregion "If Drawing is in ARASInnovator"

            //        #region "If Drawing is not in ARASInnovator"
            //        else
            //        {
            //            obj.IsManualVersion = true;
            //            Item fileItem = myInnovator.newItem("File", "add");
            //            string seperatefilename = Path.GetFileName(obj.FilePath);
            //            fileItem.setProperty("filename", seperatefilename);
            //            fileItem.setID(myInnovator.getNewID());
            //            fileItem.attachPhysicalFile(obj.FilePath);
            //            Item checkin_result = fileItem.apply();
            //            string newFileId = checkin_result.getItemByIndex(0).getProperty("id");
            //            if (checkin_result.isError())
            //            {
            //                throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + checkin_result.getErrorString()));
            //            }
            //            cadDocument_QRY = myInnovator.newItem(obj.ItemType, "add");
            //            cadDocument_QRY.setProperty("name", obj.ObjectName);
            //            cadDocument_QRY.setProperty("item_number", obj.ObjectNumber);
            //            if (obj.ObjectProjectId.ToString().Length > 1)
            //            {
            //                cadDocument_QRY.setProperty("sg_project_name", obj.ObjectProjectId);
            //            }
            //            if (obj.ObjectDescription.ToString().Length > 1)
            //                cadDocument_QRY.setProperty("description", obj.ObjectDescription);
            //            cadDocument_QRY.setProperty("authoring_tool", obj.AuthoringTool);
            //            if (obj.Classification != "Non" && obj.Classification != null && obj.Classification != "")
            //                cadDocument_QRY.setProperty("classification", obj.Classification);
            //            cadDocument_QRY.setProperty("native_file", newFileId);
            //            cadDocument_RES = cadDocument_QRY.apply();
            //            newDrawingId = cadDocument_RES.getProperty("id");
            //            SavedData.Add(seperatefilename, cadDocument_RES.getProperty("id"));

            //            #region AddtoPart
            //            if (obj.ObjectRealtyId.ToString().Length > 1)
            //            {
            //                Item sg_Part = myInnovator.applyAML("<AML><Item type='Part' action='get'><item_number>" + obj.ObjectRealtyId + "</item_number></Item></AML>");

            //                Item PartCADItem = myInnovator.newItem("Part CAD", "add");
            //                PartCADItem.setProperty("source_id", sg_Part.getProperty("id"));
            //                PartCADItem.setProperty("related_id", cadDocument_RES.getProperty("id"));
            //                Item PartCADItem_RES = PartCADItem.apply();
            //                if (PartCADItem_RES.isError())
            //                {
            //                    throw (new Exceptions.ConnectionException("Exception occured in 'Realty Entity SaveObject' method.\n Error string is :" + PartCADItem_RES.getErrorString()));
            //                }
            //            }
            //            #endregion AddtoPart

            //        }
            //        #endregion

            //        #region "Manage Layout"
            //        Item sg_CAD = myInnovator.getItemById("CAD", newDrawingId);

            //        String query = "select distinct(keyed_name) from innovator.[CAD] where id in (select related_id from innovator.[sg_RelatedCADLayout] where source_id in (select id from innovator.[CAD] where item_number='" + sg_CAD.getProperty("item_number", "") + "'))";
            //        Item DwgLayout = myInnovator.applyMethod("sg_run_sql_query", "<query>" + query + "</query>");

            //        Hashtable Layout_Old = new Hashtable();

            //        for (int l = 0; l < DwgLayout.getItemCount(); l++)
            //        {
            //            Item Old_Layout = myInnovator.getItemByKeyedName("CAD", DwgLayout.getItemByIndex(l).getProperty("keyed_name"));
            //            if (Old_Layout == null) continue;
            //            if (Layout_Old.Contains(Old_Layout.getItemByIndex(0).getProperty("name"))) continue;
            //            Layout_Old.Add(Old_Layout.getItemByIndex(0).getProperty("name"), Old_Layout.getItemByIndex(0).getProperty("id"));
            //        }

            //        foreach (string layout in Layouts)
            //        {
            //            if (Layout_Old.Contains(layout))
            //            {
            //                if (obj.IsManualVersion)
            //                {
            //                    Item CAD = myInnovator.getItemById("CAD", Layout_Old[layout].ToString());
            //                    String sg_LayoutState = CAD.getProperty("state").ToString();
            //                    if ((sg_LayoutState != "Preliminary"))
            //                    {
            //                        MessageBox.Show("The Layout \"" + layout + "\" is in \"" + CAD.getProperty("state").ToString() + "\" LifeCycle State. So we are unable to create new generation of it. To Create new generation of Layout \"" + layout + "\" create new REVISION of Layout using Change Process.", "Warning");
            //                        //myInnovator.applyMethod("sg_run_sql_query", "<query>delete from innovator.[sg_RelatedCADLayout] where related_id='" + CAD.getProperty("id").ToString() + "' and source_id='" + newDrawingId + "'</query>");
            //                    }
            //                    else
            //                    {
            //                        CAD = myInnovator.newItem("CAD", "edit");
            //                        CAD.setAttribute("where", "id='" + Layout_Old[layout] + "'");
            //                        CAD.setProperty("description", obj.ObjectDescription.ToString());
            //                        Item CADRes = CAD.apply("version");
            //                        CADRes.unlockItem();
            //                        query = "select * from innovator.[sg_RelatedCADLayout] where related_id='" + CADRes.getProperty("id").ToString() + "' and source_id='" + newDrawingId + "'";
            //                        Item LayoutCurr = myInnovator.applyMethod("sg_run_sql_query", "<query>" + query + "</query>");

            //                        if (LayoutCurr.getItemCount() < 1)//If Layout is not in Relationship "sg_RelatedCADLayout"
            //                        {
            //                            Item Layout = myInnovator.newItem("sg_RelatedCADLayout", "add");
            //                            Layout.setProperty("source_id", newDrawingId);
            //                            Layout.setProperty("related_id", CADRes.getProperty("id").ToString());
            //                            Layout = Layout.apply();
            //                        }
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                if (layout.Length > 0)
            //                {
            //                    Item sg_Layout = myInnovator.newItem("CAD", "add");
            //                    sg_Layout.setProperty("name", layout);
            //                    sg_Layout.setProperty("item_number", myInnovator.getNextSequence("sg_CADLayoutSequence") + "_" + sg_CAD.getProperty("item_number"));
            //                    sg_Layout.setProperty("classification", "Layout");
            //                    sg_Layout.setProperty("description", obj.ObjectDescription.ToString());
            //                    sg_Layout.setProperty("sg_project_name", sg_CAD.getProperty("sg_project_name"));
            //                    Item sg_LayoutRes = sg_Layout.apply();

            //                    Item sg_CADItem = myInnovator.newItem("CAD", "edit");
            //                    sg_CADItem.setAttribute("where", "id='" + newDrawingId + "'");
            //                    Item CADLayoutItem = myInnovator.newItem("sg_RelatedCADLayout", "add");
            //                    CADLayoutItem.setRelatedItem(sg_LayoutRes);
            //                    sg_CADItem.addRelationship(CADLayoutItem);
            //                    Item sg_CADItemRES = sg_CADItem.apply();
            //                }
            //            }
            //        }

            //        //If Layout is deleted or Renamed then treat as deletion of Relationship
            //        foreach (DictionaryEntry layout_old in Layout_Old)
            //        {
            //            Boolean flag = true;
            //            foreach (string layout in Layouts)
            //            {
            //                if (layout == layout_old.Key.ToString())
            //                {
            //                    flag = false;
            //                    break;
            //                }
            //            }
            //            if (flag)
            //            {
            //                Item delRelItm = myInnovator.applyMethod("sg_run_sql_query", "<query>delete from innovator.[sg_RelatedCADLayout] where source_id='" + newDrawingId + "' and related_id='" + layout_old.Value.ToString() + "'</query>");
            //                // delRelItm = myInnovator.applyMethod("sg_run_sql_query", "<query>delete from innovator.[sg_RelatedEntityLayout] where related_id='" + layout_old.Value.ToString() + "'</query>");
            //            }
            //        }

            //        //Update Realty Entity After File Checkin To Maintain Relationship "sg_RelatedEntityLayout" according to related "CAD" Items
            //        query = "select distinct(config_id) from innovator.[Part] where id in (select source_id from innovator.[Part_CAD] where related_id ='" + newDrawingId + "')";
            //        Item sg_Realty = myInnovator.applyMethod("sg_run_sql_query", "<query>" + query + "</query>");

            //        for (int i = 0; i < sg_Realty.getItemCount(); i++)
            //        {
            //            Item sg_Part = myInnovator.newItem("Part", "edit");
            //            sg_Part.setAttribute("where", "config_id='" + sg_Realty.getItemByIndex(i).getProperty("config_id") + "'");
            //            sg_Part = sg_Part.apply();
            //        }

            //        #endregion "Manage Layout"

            //        #region "Add Drawings under Root Drawing"
            //        /*
            //           if (obj.IsManualVersion && !obj.IsNew)
            //          {
            //              if (obj.IsRoot)
            //                  sourceid = cadDocument_RES.getProperty("id");
            //              else
            //                  newDrawingId = cadDocument_RES.getProperty("id");

            //              if (!obj.IsRoot)
            //              {
            //                  Item relationship_RES;
            //                  cadDocument_QRY = myInnovator.newItem(obj.ItemType, "get");
            //                  cadDocument_QRY.setProperty("id", sourceid);
            //                  relationship_RES = cadDocument_QRY.apply();

            //                  cadDocument_QRY = myInnovator.newItem(obj.ItemType, "get");
            //                  cadDocument_QRY.setProperty("config_id", relationship_RES.getProperty("config_id"));
            //                  cadDocument_QRY.setProperty("is_current", "1");
            //                  relationship_RES = cadDocument_QRY.apply();
            //                  Item selItm = myInnovator.applyMethod("sg_run_sql_query", "<query>select * from innovator.[CAD_Structure] where source_id='" + relationship_RES.getProperty("id") + "' and related_id in (select id from innovator.[CAD] where item_number='" + cadDocument_RES.getProperty("item_number") + "')</query>");
            //                  if (selItm.getItemCount() > 0)
            //                  {
            //                      Item delItm = myInnovator.applyMethod("sg_run_sql_query", "<query>Delete from innovator.[CAD_Structure] where source_id='" + relationship_RES.getProperty("id") + "' and related_id in (select id from innovator.[CAD] where item_number='" + cadDocument_RES.getProperty("item_number") + "')</query>");
            //                  }
            //                    cadDocument_QRY = myInnovator.newItem("CAD Structure", "add");
            //                    cadDocument_QRY.setProperty("source_id", relationship_RES.getProperty("id"));
            //                    if (relationship_RES.getProperty("project_name") != null || relationship_RES.getProperty("project_name") != "")
            //                    {
            //                        Item NewCAD = myInnovator.getItemById("CAD", newDrawingId);
            //                        if (NewCAD.getItemByIndex(0).getProperty("project_name") == null || NewCAD.getItemByIndex(0).getProperty("project_name") == "")
            //                        {
            //                           Item updateItm = myInnovator.applyMethod("sg_run_sql_query", "<query>update innovator.[CAD] set project_name='" + relationship_RES.getProperty("project_name") + "' where id='" + newDrawingId + "'</query>");
            //                        }
            //                    }
            //                    cadDocument_QRY.setProperty("related_id", newDrawingId);
            //                    relationship_RES = cadDocument_QRY.apply();
            //                  }
            //          }*/
            //        #endregion "Add Drawings under Root Drawing"

            //        #region "Update PLMObject information"
            //        obj.ObjectId = cadDocument_RES.getProperty("id");
            //        obj.Classification = cadDocument_RES.getProperty("classification");
            //        obj.ObjectGeneration = cadDocument_RES.getProperty("generation");
            //        obj.ObjectNumber = cadDocument_RES.getProperty("item_number");
            //        obj.ObjectRevision = cadDocument_RES.getProperty("major_rev");
            //        obj.ObjectState = cadDocument_RES.getProperty("state");
            //        obj.ObjectName = cadDocument_RES.getProperty("name");
            //        obj.ObjectProjectId = cadDocument_RES.getProperty("sg_project_name");
            //        //obj.ObjectProjectId = cadDocument_RES.getProperty("project_id");
            //        obj.LockStatus = cadDocument_RES.fetchLockStatus().ToString();
            //        #endregion
            //    }//end For
            //    #endregion "Save PLMObjects"

            //    #region ManageCADStructureRel
            //    foreach (PLMObject obj in plmobjs)
            //    {
            //        String seperatefilename = Path.GetFileName(obj.FilePath);
            //        String MyDrawingId = "";
            //        foreach (DictionaryEntry de in SavedData)
            //        {
            //            if (de.Key.ToString() == seperatefilename)
            //            {
            //                MyDrawingId = de.Value.ToString();
            //            }
            //        }
            //        #region "If Drawing is in ARASInnovator"
            //        if (!obj.IsNew)
            //        {
            //            #region "Manage RelatedData: if Source updated"

            //            String query = "select distinct related_id from innovator.[CAD_STRUCTURE] where source_id='" + MyDrawingId + "'";
            //            Item PrevRel = myInnovator.applyMethod("sg_run_sql_query", "<query>" + query + "</query>");

            //            for (int related = 0; related < PrevRel.getItemCount(); related++)
            //            {
            //                Item Child = myInnovator.getItemById("CAD", PrevRel.getItemByIndex(related).getProperty("related_id").ToString());
            //                Item LatestChild = myInnovator.getItemByKeyedName("CAD", Child.getProperty("keyed_name").ToString());

            //                query = "delete from innovator.[CAD_STRUCTURE] where source_id='" + obj.ObjectId + "' and related_id='" + LatestChild.getItemByIndex(0).getProperty("id").ToString() + "'";
            //                Item delRes = myInnovator.applyMethod("sg_run_sql_query", "<query>" + query + "</query>");

            //                Item NewCADStr = myInnovator.newItem("CAD Structure", "add");
            //                NewCADStr.setProperty("source_id", obj.ObjectId);
            //                NewCADStr.setProperty("related_id", LatestChild.getItemByIndex(0).getProperty("id").ToString());
            //                Item NewCADStrRes = NewCADStr.apply();
            //            }
            //            #endregion "Manage RelatedData: if Source updated"

            //            #region "Manage SourceData: if RelatedItem updated"
            //            /*PrevRel = myInnovator.applyMethod("sg_run_sql_query", "<query>select distinct source_id from innovator.[CAD_STRUCTURE] where related_id='" + MyDrawingId + "'</query>");
            //            for (int related = 0; related < PrevRel.getItemCount(); related++)
            //            {
            //                Item Child = myInnovator.getItemById("CAD", PrevRel.getItemByIndex(related).getProperty("source_id").ToString());
            //                Item LatestChild = myInnovator.getItemByKeyedName("CAD", Child.getProperty("keyed_name").ToString());
            //                Item delItm = myInnovator.applyMethod("sg_run_sql_query", "<query>delete from innovator.[CAD_STRUCTURE] where related_id='" + obj.ObjectId + "' and source_id='" + LatestChild.getItemByIndex(0).getProperty("id").ToString() + "'</query>");

            //                Item NewCADStr = myInnovator.newItem("CAD Structure", "add");
            //                NewCADStr.setProperty("source_id", LatestChild.getItemByIndex(0).getProperty("id").ToString());
            //                NewCADStr.setProperty("related_id", obj.ObjectId);
            //                Item NewCADStrRes = NewCADStr.apply();
            //                delItm = myInnovator.applyMethod("sg_run_sql_query", "<query>delete from innovator.[CAD_STRUCTURE] where related_id='" + MyDrawingId + "' and source_id='" + LatestChild.getItemByIndex(0).getProperty("id").ToString() + "'</query>");

            //            }*/
            //            #endregion "Manage SourceData: if RelatedItem updated"
            //        }
            //        #endregion "If Drawing is in ARASInnovator"

            //        #region "If Drawing is not in ARASInnovator"
            //        else
            //        {
            //            if ((obj.ObjectProjectId == null || obj.ObjectProjectId == "") && (ProjectName != ""))
            //            {
            //                Item updateItm = myInnovator.applyMethod("sg_run_sql_query", "<query>update innovator.[CAD] set sg_project_name='" + ProjectName + "' where id='" + obj.ObjectId + "'</query>");
            //            }
            //        }
            //        #endregion "If Drawing is not in ARASInnovator"

            //        if (!obj.IsRoot)
            //        {
            //            String[] SaveID = new String[10];
            //            ArrayList SourceIds = new ArrayList();
            //            SaveID = obj.ObjectSourceId.ToString().Split(',');
            //            for (int ids = 0; ids < SaveID.Length; ids++)
            //            {
            //                foreach (DictionaryEntry de in SavedData)
            //                {
            //                    if (de.Key.ToString() == SaveID[ids] || de.Key.ToString().Substring(0, de.Key.ToString().Length - 4) == SaveID[ids])
            //                    {
            //                        SourceIds.Add(de.Value.ToString());
            //                    }
            //                }
            //            }

            //            for (int rel = 0; rel < SourceIds.Count; rel++)
            //            {
            //                Item Latest = myInnovator.getItemById("CAD", SourceIds[rel].ToString());
            //                Item LatestCAD = myInnovator.getItemByKeyedName("CAD", Latest.getItemByIndex(0).getProperty("keyed_name"));
            //                Item delItm = myInnovator.applyMethod("sg_run_sql_query", "<query>delete from innovator.[CAD_STRUCTURE] where related_id='" + obj.ObjectId + "' and source_id='" + LatestCAD.getItemByIndex(0).getProperty("id").ToString() + "'</query>");

            //                Item CADStructure = myInnovator.newItem("CAD Structure", "add");
            //                CADStructure.setProperty("source_id", LatestCAD.getItemByIndex(0).getProperty("id"));
            //                CADStructure.setProperty("related_id", obj.ObjectId);
            //                Item CADStructureRes = CADStructure.apply();
            //            }
            //            SourceIds.Clear();
            //        }
            //    }
            //    #endregion ManageCADStructureRel
            //}//end Catch
            //catch (Exception ex)
            //{
            //    throw (new Exceptions.ConnectionException("ArasConnector SaveObject Exception Message : " + ex.Message));
            //}
            #endregion
            return true;
        }

        public List<PLMObject> GetPLMObjectInformation(List<PLMObject> plmobjs)
        {
            try
            {
                List<PLMObject> newplmobjs = new List<PLMObject>();
                foreach (PLMObject obj in plmobjs)
                {

                     KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId",obj.ObjectId);
                   // KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", "11760c31-d3fb-4acb-9675-551915493fd5");

                    List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                    urlParameters.Add(L);
                    RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                        "/AutocadFiles/fetchFileInfo", DataFormat.Json,
                        null, true, urlParameters);


                    var ObjFileInfo = JsonConvert.DeserializeObject<ResultSearchCriteria>(restResponse.Content);


                    if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        MessageBox.Show("Some error occurred while fetching file information.");
                    }
                    else
                    {
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

                    }



                }
                return newplmobjs;

            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("ArasConnector Exception Message :" + ex.Message));
            }

        }

        public DataTable GetFIleType()
        {
            DataTable dataTableProjectInfo = new DataTable();
            try
            {
                dataTableProjectInfo = GetDataFromWS("/AutocadFiles/fetchFileType", "file type");
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
                        MessageBox.Show("Some error occurred while locking file.");
                        return true;
                    }
                    else
                    {
                        return false;
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
            try
            {
                KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", drawingid);
                //KeyValuePair<string, string> L = new KeyValuePair<string, string>("fileId", "11760c31-d3fb-4acb-9675-551915493fd5");

                List<KeyValuePair<string, string>> urlParameters = new List<KeyValuePair<string, string>>();
                urlParameters.Add(L);
                RestResponse restResponse = (RestResponse)ServiceHelper.PostData(Helper.GetValueRegistry("LoginSettings", "Url").ToString(),
                    "/AutocadFiles/fetchFileInfo", DataFormat.Json,
                    null, true, urlParameters);


                ResultSearchCriteria ObjFileInfo = JsonConvert.DeserializeObject<ResultSearchCriteria>(restResponse.Content);


                if (restResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Some error occurred while fetching file information.");
                    return null;
                }
                else
                {
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
                return ObjFileInfo;
            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("ArasConnector Exception Message :" + ex.Message));
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
