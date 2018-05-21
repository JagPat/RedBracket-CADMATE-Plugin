using Aras.IOM;
using ArasConnector.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace ArasConnector
{
    public class ArasConnector : IConnector
    {
        #region "Variables"

        public static Aras.IOM.HttpServerConnection myConnection;
        public static Aras.IOM.Innovator myInnovator;
        private PLMObject itemObject = new PLMObject();
        private DrawingInfo drawingInfo = new DrawingInfo();
        public static Boolean Isconnected = false;
        private ItemInformation myMessage = new ItemInformation();
        Item drawingQueryRes = null;
        PLMObject newdrawingObject = new PLMObject();
        String newItemObject;
        String CheckoutView;
        public static String sg_WorkingDir = "";
        public static String sg_UserId = "";
        Hashtable hashtableResult = new Hashtable();

        #endregion

        #region "Public Methods"

        public String[] getDataBaseList(string serverPath)
        {
            HttpServerConnection serverConn = IomFactory.CreateHttpServerConnection(serverPath);
            String[] dataBaseList = serverConn.GetDatabases();
            return dataBaseList;
        }

        public void Connect(string url, string dbName, string usrName, string passwd, string authoringtool)
        {
            try
            {
                myConnection = IomFactory.CreateHttpServerConnection(url, dbName, usrName, passwd);
                Item loginResult = myConnection.Login();
                if (loginResult.isError())
                {
                    throw (new Exceptions.ConnectionException("Exception occured in 'Connect' method.\n Error string is :" + loginResult.getErrorString()));

                }
                else
                {
                    if (myConnection != null)
                    {
                        myInnovator = new Innovator(myConnection);
                        sg_UserId = myInnovator.getUserID();
                        Item sg_User = myInnovator.getItemById("User", sg_UserId);
                        sg_WorkingDir = sg_User.getItemByIndex(0).getProperty("working_directory","C:");
                        #region SetWorking Directory

                        char[] RemoveFromDirPath = { 'f', 'i', 'l', 'e', ':', '\\', '\\' };
                        String Dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).TrimStart(RemoveFromDirPath);
                        string filename = Dir + "\\UserSettingFile";
                        XmlDocument doc = new XmlDocument();
                        doc.Load(filename);
                        doc.SelectSingleNode("Settings//Checkout//CheckoutDirNode").InnerText = sg_WorkingDir;
                        doc.Save(filename);

                        #endregion SetWorking Directory

                        #region CADIntegrationConfiguration
                        Item docConfig = myInnovator.newItem("CADIntegrationConfiguration", "get");
                        docConfig.setAttribute("select", "configuration_string");
                        Item result = docConfig.apply();
                        if (result.isError())
                        {
                            throw (new Exceptions.ConnectionException("Exception occured in 'Connect' method.\n Error string is :" + result.getErrorString()));
                        }
                        int itemCount = result.getItemCount();
                        for (int itemNo = 0; itemNo < itemCount; itemNo++)
                        {
                            new DrawingConfiguration(result.getItemByIndex(itemNo).getProperty("configuration_string"));

                        }
                        Isconnected = true;
                        #endregion CADIntegrationConfiguration

                        #region Project
                        Item proConfig = myInnovator.newItem("Project", "get");
                        proConfig.setAttribute("select","id, keyed_name, project_number");
                        Item pro_result = proConfig.apply();
                        /*if (pro_result.isError())
                        {
                            throw (new Exceptions.ConnectionException("Exception occured in 'Getting Project' method.\n Error string is :" + pro_result.getErrorString()));
                        }*/
                        int pro_itemCount = pro_result.getItemCount();
                        for (int itemNo = 0; itemNo < pro_itemCount; itemNo++)
                        {
                            String Pro_Info = pro_result.getItemByIndex(itemNo).getProperty("id") + "|" + pro_result.getItemByIndex(itemNo).getProperty("keyed_name") + "|" + pro_result.getItemByIndex(itemNo).getProperty("project_number");
                            new ProjectConfiguration(Pro_Info);
                        }
                        Isconnected = true;
                        #endregion Project

                        #region RealtyEntity
                        Item rltConfig = myInnovator.newItem("Part", "get");
                        rltConfig.setAttribute("select", "sg_project_name, keyed_name, item_number");
                        Item rlt_result = rltConfig.apply();
                        /*if (rlt_result.isError())
                        {
                            throw (new Exceptions.ConnectionException("Exception occured in 'Getting Realty Entity' method.\n Error string is :" + rlt_result.getErrorString()));
                        }*/
                        int rlt_itemCount = rlt_result.getItemCount();
                        for (int itemNo = 0; itemNo < rlt_itemCount; itemNo++)
                        {
                            String Rlt_Info = rlt_result.getItemByIndex(itemNo).getProperty("sg_project_name","NA") + "|" + rlt_result.getItemByIndex(itemNo).getProperty("keyed_name") + "|" + rlt_result.getItemByIndex(itemNo).getProperty("item_number");
                            new RealtyConfiguration(Rlt_Info);
                        }
                        Isconnected = true;
                        #endregion RealtyEntity

                        #region CADLifeCycleState
                        Item cadLCConfig = myInnovator.newItem("ItemType Life Cycle", "get");
                        cadLCConfig.setAttribute("select", "source_id, related_id, class_path");
                        cadLCConfig.setAttribute("orderBy", "class_path");
                        cadLCConfig.setProperty("source_id", "CCF205347C814DD1AF056875E0A880AC");//id of ItemType CAD
                        Item cadLCConfigRes = cadLCConfig.apply();
                        
                        int stateItemCount = cadLCConfigRes.getItemCount();
                        for (int itemNo = 0; itemNo < stateItemCount; itemNo++)
                        {
                            String cadLCId = cadLCConfigRes.getItemByIndex(itemNo).getProperty("related_id");
                            String cadLCPath = cadLCConfigRes.getItemByIndex(itemNo).getProperty("class_path", "");
                            Item lcStateItm = myInnovator.newItem("Life Cycle State","get");
                            lcStateItm.setAttribute("select","name, label");
                            lcStateItm.setAttribute("orderBy", "sort_order");
                            lcStateItm.setProperty("source_id",cadLCId);
                            Item lcStateItmRes = lcStateItm.apply();
                            for (int stateCount = 0; stateCount < lcStateItmRes.getItemCount(); stateCount++)
                            {
                                String stateInfo = cadLCPath + "|" + lcStateItmRes.getItemByIndex(stateCount).getProperty("name") + "|" + lcStateItmRes.getItemByIndex(stateCount).getProperty("label", lcStateItmRes.getItemByIndex(stateCount).getProperty("name"));
                                new LifeCycleConfiguration(stateInfo);
                            }
                        }
                        Isconnected = true;
                        #endregion CADLifeCycleState
                    }

                }

            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("ArasConnector Connect Exception Message : " + ex.Message));
            }
        }

        public void Disconnect()
        {
            try
            {
                myConnection.Logout();
                myConnection = null;
                myInnovator = null;
                DrawingConfiguration.DocConfig.RemoveRange(0, DrawingConfiguration.DocConfig.Count);
                ProjectConfiguration.ProConfig.RemoveRange(0, ProjectConfiguration.ProConfig.Count);
                RealtyConfiguration.RltConfig.RemoveRange(0, RealtyConfiguration.RltConfig.Count);
                LifeCycleConfiguration.LCConfig.RemoveRange(0, LifeCycleConfiguration.LCConfig.Count);
            }
            catch (Exception ex)
            {
                myConnection = null;
                myInnovator = null;
                throw (new Exceptions.ConnectionException("ArasConnector Disconnect Exception Message : " + ex.Message));
            }
        }

        public SearchResult SearchObject(PLMObject ItemObject, string objLatest, ObjectDataSpecs dataSpecs)
        {

            hashtableResult.Clear();

            if (objLatest == null)
            {
                objLatest = "";
            }

            newItemObject = ItemObject.Classification.ToString();
            CheckoutView = objLatest;

            SearchResult searchRslt = new SearchResult();

            try
            {
                if (myConnection != null)
                {
                    myInnovator = new Innovator(myConnection);
                }

                String attributeString = GetAttributes(dataSpecs);
                string queryString = null;
                int cdocumentCount = 0;
                Item NewQueryResult = null;
                int NewdocumentCount = 0;

                #region "replaces * with %"
                if (ItemObject.ObjectName.ToString() != "")
                {
                    ItemObject.ObjectName = ItemObject.ObjectName.Replace("*", "%");
                }
                if (ItemObject.ObjectNumber.ToString() != "")
                {
                    ItemObject.ObjectNumber = ItemObject.ObjectNumber.Replace("*", "%");
                }
                if (ItemObject.ObjectRevision.ToString() != "")
                {
                    ItemObject.ObjectRevision = ItemObject.ObjectRevision.Replace("*", "%");
                }
                if (ItemObject.ObjectRealtyId.ToString() != "")
                {
                    ItemObject.ObjectRealtyId = ItemObject.ObjectRealtyId.Replace("*", "%");
                }

                #endregion

                #region DefaultSearch
                if (ItemObject.ObjectName.ToString() != "")
                {
                    queryString += ItemObject.ItemType + ".name like '" + ItemObject.ObjectName.ToString() + "' and ";
                }
                if (ItemObject.ObjectNumber.ToString() != "")
                {
                    queryString += ItemObject.ItemType + ".item_number like '" + ItemObject.ObjectNumber + "' and ";
                }
                if (ItemObject.Classification.ToString() != "")
                {
                    queryString += ItemObject.ItemType + ".classification like '" + ItemObject.Classification + "' and ";
                }
                else
                {
                    queryString += "(" + ItemObject.ItemType + ".classification not like 'Layout' or " + ItemObject.ItemType + ".classification is null) and ";
                }
                if (ItemObject.ObjectProjectName.ToString() != "")
                {
                    queryString += ItemObject.ItemType + ".sg_project_name like '" + ItemObject.ObjectProjectName + "' and ";
                }
                if (ItemObject.ObjectState.ToString() != "")
                {
                    queryString += ItemObject.ItemType + ".state like '" + ItemObject.ObjectState + "' and ";
                }
                if (ItemObject.ObjectRevision.ToString() != "")
                {
                    queryString += " ";
                }

                Item NewDrawingQuery = myInnovator.newItem(ItemObject.ItemType.ToString(), "get");
                NewDrawingQuery.setAttribute("select", attributeString);
                NewDrawingQuery.setAttribute("where", queryString + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "'");
                NewQueryResult = NewDrawingQuery.apply();
                NewdocumentCount = NewQueryResult.getItemCount();
                if ((ItemObject.ObjectDesktop.ToString() == "Full") || (ItemObject.ObjectDesktop.ToString() == ""))
                {
                    if (ItemObject.ObjectRealtyId.ToString() != "")
                    {
                        Item Part = myInnovator.newItem("Part", "get");
                        Part.setProperty("keyed_name", ItemObject.ObjectRealtyId);
                        Part.setAttribute("select", "id,keyed_name");
                        Part.setAttribute("levels", "0");
                        Item PartRes = Part.apply();
                        String CADID = "";
                        for (int i = 0; i < PartRes.getItemCount(); i++)
                        {
                            Item PartCAD = myInnovator.newItem("Part CAD", "get");
                            PartCAD.setProperty("source_id", PartRes.getItemByIndex(i).getProperty("id"));
                            PartCAD.setAttribute("select", "source_id, related_id");
                            Item PartCADRes = PartCAD.apply();
                            for (int j = 0; j < PartCADRes.getItemCount(); j++)
                            {
                                CADID += PartCADRes.getItemByIndex(j).getProperty("related_id") + ",";
                            }
                        }
                        if (CADID.Length > 30)
                            CADID = CADID.Substring(0, CADID.Length - 1);
                        NewDrawingQuery = myInnovator.newItem(ItemObject.ItemType.ToString(), "get");
                        NewDrawingQuery.setAttribute("select", attributeString);
                        NewDrawingQuery.setAttribute("where", queryString + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "'");
                        NewDrawingQuery.setProperty("id", CADID);
                        NewDrawingQuery.setPropertyCondition("id", "in");
                        NewQueryResult = NewDrawingQuery.apply();
                        NewdocumentCount = NewQueryResult.getItemCount();
                    }
                }
                else if (ItemObject.ObjectDesktop.ToString() == "Desktop")
                {
                    Item Desktop = myInnovator.newItem("Desktop", "get");
                    Desktop.setProperty("related_id", sg_UserId);
                    Desktop.setProperty("source_type", "CCF205347C814DD1AF056875E0A880AC");
                    Item DesktopRes = Desktop.apply();
                    String CADID = "";
                    for (int j = 0; j < DesktopRes.getItemCount(); j++)
                    {
                        CADID += DesktopRes.getItemByIndex(j).getProperty("source_id","") + ",";
                    }
                    if (CADID.Length > 30)
                        CADID = CADID.Substring(0, CADID.Length - 1);
                    NewDrawingQuery = myInnovator.newItem(ItemObject.ItemType.ToString(), "get");
                    NewDrawingQuery.setAttribute("select", attributeString);
                    NewDrawingQuery.setAttribute("where", queryString + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "'");
                    NewDrawingQuery.setProperty("id", CADID);
                    NewDrawingQuery.setProperty("is_current", "1");
                    NewDrawingQuery.setPropertyCondition("id", "in");
                    NewQueryResult = NewDrawingQuery.apply();
                    NewdocumentCount = NewQueryResult.getItemCount();
                }
                else if (ItemObject.ObjectDesktop.ToString() == "InBasket")
                {
                    Item sg_Activities=myInnovator.getAssignedActivities("Active",sg_UserId);
                    String CADID = "";
                    for (int j = 0; j < sg_Activities.getItemCount(); j++)
                    {
                        string sg_WorkFlowActivityId = sg_Activities.getItemByIndex(j).getProperty("id","");
                        Item sg_SourceItem = myInnovator.applyMethod("sg_GetItemByWFActivityId", "<sg_ActivityId>" + sg_WorkFlowActivityId + "</sg_ActivityId>");
                        if (sg_SourceItem.getItemByIndex(0).getType().ToString() != "CAD") continue;
                        CADID += sg_SourceItem.getItemByIndex(0).getProperty("config_id", "") + ",";
                    }
                    if (CADID.Length > 30)
                        CADID = CADID.Substring(0, CADID.Length - 1);
                    NewDrawingQuery = myInnovator.newItem(ItemObject.ItemType.ToString(), "get");
                    NewDrawingQuery.setAttribute("select", attributeString);
                    NewDrawingQuery.setAttribute("where", queryString + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "'");
                    NewDrawingQuery.setProperty("config_id", CADID);
                    NewDrawingQuery.setProperty("is_current", "1");
                    NewDrawingQuery.setPropertyCondition("config_id", "in");
                    NewQueryResult = NewDrawingQuery.apply();
                    NewdocumentCount = NewQueryResult.getItemCount();
                }
                else { }
                
                for (int newDocCount = 0; newDocCount < NewdocumentCount; newDocCount++)
                {
                    Item NewresultDrawing = NewQueryResult.getItemByIndex(newDocCount);
                    newdrawingObject.ObjectId = NewresultDrawing.getProperty("id");
                    newdrawingObject.NativeFileId = NewresultDrawing.getProperty("native_file");
                    newdrawingObject.ObjectId = NewresultDrawing.getProperty("id");
                    newdrawingObject.ObjectName = NewresultDrawing.getProperty("name");
                    newdrawingObject.ObjectNumber = NewresultDrawing.getProperty("item_number");
                    newdrawingObject.ObjectProjectId = NewresultDrawing.getPropertyAttribute("sg_project_name", "keyed_name", " ");
                    newdrawingObject.ObjectProjectName = NewresultDrawing.getProperty("sg_project_id");
                    newdrawingObject.ObjectOwnerCompany = NewresultDrawing.getProperty("sg_owner_company");
                    newdrawingObject.ObjectRevision = NewresultDrawing.getProperty("major_rev");
                    newdrawingObject.ObjectState = NewresultDrawing.getProperty("state");
                    newdrawingObject.LockStatus = NewresultDrawing.getLockStatus().ToString();
                    newdrawingObject.ObjectGeneration = NewresultDrawing.getProperty("generation");
                    newdrawingObject.ItemType = ItemObject.ItemType;
                    newdrawingObject.ObjectDescription = NewresultDrawing.getProperty("description");
                    newdrawingObject.Classification = NewresultDrawing.getProperty("classification");
                    hashtableResult.Add(newdrawingObject.ObjectId, newdrawingObject.Classification);
                }
                #endregion DefaultSearch

                #region ReleasedSearch
                if (objLatest.ToString() == "Released")
                {
                    Item RdrawingQuery = myInnovator.newItem(ItemObject.ItemType.ToString(), "get");

                    if ("".Equals(ItemObject.ObjectRevision))
                    {
                        ItemObject.ObjectRevision = "%";
                    }
                    RdrawingQuery.setAttribute("where", queryString + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "' and " + ItemObject.ItemType + ".major_rev like '" + ItemObject.ObjectRevision + "'");
                    RdrawingQuery.setAttribute("queryType", "Released");
                    drawingQueryRes = RdrawingQuery.apply();
                    cdocumentCount = drawingQueryRes.getItemCount();
                }
                #endregion ReleasedSearch

                #region CurrentSearch
                else if (objLatest.ToString() == "Current")
                {
                    drawingQueryRes = null;
                    foreach (DictionaryEntry entry in hashtableResult)
                    {
                        Item drawingQuery = myInnovator.newItem(ItemObject.ItemType.ToString(), "PE_GetResolvedStructure");
                        string s = entry.Key.ToString();
                        drawingQuery.setAttribute("resolution", "Current");
                        drawingQuery.setAttribute("id", s);

                        Item rel1 = myInnovator.newItem("CAD Structure");
                        rel1.setAttribute("repeatTimes", "0");
                        drawingQuery.addRelationship(rel1);
                        if (drawingQueryRes == null)
                            drawingQueryRes = drawingQuery.apply();
                        else
                            drawingQueryRes.appendItem(drawingQuery.apply());
                        cdocumentCount = drawingQueryRes.getItemCount();
                    }
                }
                #endregion CurrentSearch

                #region AsSavedSearch
                else if (objLatest.ToString() == "AsSaved")    //It is same as Latest
                {
                    Item drawingQuery = myInnovator.newItem(ItemObject.ItemType.ToString(), "get");
                    drawingQuery.setAttribute("select", attributeString);
                    drawingQuery.setAttribute("where", queryString + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "'");
                    drawingQueryRes = drawingQuery.apply();
                    cdocumentCount = drawingQueryRes.getItemCount();
                }
                #endregion AsSavedSearch

                #region SQLSearch
                else if (queryString != null)
                {

                    if (queryString == "")
                    {
                        String query = "select " + attributeString + " from " + ItemObject.ItemType + " where " + ItemObject.ItemType + ".major_rev like '" + ItemObject.ObjectRevision + "' and " + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "'";
                        drawingQueryRes = myInnovator.applyMethod("sg_run_sql_query", "<query>" + query + "</query>");
                    }
                    else
                    {
                        String query = "select " + attributeString + " from " + ItemObject.ItemType + " where " + queryString + " " + ItemObject.ItemType + ".major_rev like '" + ItemObject.ObjectRevision + "' and " + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "'";
                        drawingQueryRes = myInnovator.applyMethod("sg_run_sql_query", "<query>" + query + "</query>");
                    }
                    int cdocumentCountTemp = drawingQueryRes.getItemCount();

                    List<String> cadItems = new List<string>();
                    for (int items = 0; items < cdocumentCountTemp; items++)
                    {
                        Item resultDrawing = drawingQueryRes.getItemByIndex(items);
                        if (!cadItems.Contains((String)resultDrawing.getProperty("config_id")))
                            cadItems.Add((String)resultDrawing.getProperty("config_id"));

                    }


                    foreach (String configId in cadItems)
                    {
                        Item resultDrawing = null;
                        String queryString1;
                        queryString1 = queryString;


                        if (queryString == "")
                        {
                            queryString1 += " generation in(select max(generation) from " + ItemObject.ItemType + " where " + ItemObject.ItemType + ".major_rev like '" + ItemObject.ObjectRevision + "' and " + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "' and " + ItemObject.ItemType + ".config_id like '" + configId + "')";//ItemObject.ItemType + ".major_rev like '" + ItemObject.ObjectRevision + "' and ";

                        }
                        else
                        {
                            queryString1 += " generation in(select max(generation) from " + ItemObject.ItemType + " where " + queryString + ItemObject.ItemType + ".major_rev like '" + ItemObject.ObjectRevision + "' and " + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "' and " + ItemObject.ItemType + ".config_id like '" + configId + "')";//ItemObject.ItemType + ".major_rev like '" + ItemObject.ObjectRevision + "' and ";
                        }

                        if (queryString1 == "")
                        {
                            String query = "select " + attributeString + " from " + ItemObject.ItemType + " where " + ItemObject.ItemType + ".major_rev like '" + ItemObject.ObjectRevision + "' and " + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "' and " + ItemObject.ItemType + ".config_id like '" + configId + "'";
                            drawingQueryRes = myInnovator.applyMethod("sg_run_sql_query", "<query>" + query + "</query>");                            
                        }
                        else
                        {
                            String query = "select " + attributeString + " from " + ItemObject.ItemType + " where " + queryString1 + " and " + ItemObject.ItemType + ".major_rev like '" + ItemObject.ObjectRevision + "' and " + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "' and " + ItemObject.ItemType + ".config_id like '" + configId + "'";
                            drawingQueryRes = myInnovator.applyMethod("sg_run_sql_query", "<query>" + query + "</query>");
                        }

                        resultDrawing = drawingQueryRes;
                        PLMObject drawingObject = new PLMObject();

                        drawingObject.NativeFileId = resultDrawing.getProperty("native_file");
                        drawingObject.ObjectId = resultDrawing.getProperty("id");
                        drawingObject.ObjectName = resultDrawing.getProperty("name");
                        drawingObject.ObjectNumber = resultDrawing.getProperty("item_number");
                        drawingObject.ObjectProjectId = resultDrawing.getPropertyAttribute("sg_project_name", "keyed_name", " ");
                        drawingObject.ObjectProjectName = resultDrawing.getProperty("sg_project_id");
                        drawingObject.ObjectOwnerCompany = resultDrawing.getProperty("sg_owner_company");
                        drawingObject.ObjectRevision = resultDrawing.getProperty("major_rev");
                        drawingObject.ObjectState = resultDrawing.getProperty("state");
                        drawingObject.LockStatus = resultDrawing.getLockStatus().ToString();
                        drawingObject.ObjectGeneration = resultDrawing.getProperty("generation");
                        drawingObject.ItemType = ItemObject.ItemType;
                        drawingObject.ObjectDescription = resultDrawing.getProperty("description");
                        drawingObject.Classification = resultDrawing.getProperty("classification");
                        if (resultDrawing.getLockStatus() == 2)
                            drawingObject.LockBy = GetLockBy(resultDrawing.getProperty("locked_by_id"));
                        else
                            drawingObject.LockBy = " ";

                        searchRslt.ObjectList.Add(drawingObject);

                    }

                    cdocumentCount = 0;
                }
                else
                {
                    String query = "select " + attributeString + " from " + ItemObject.ItemType + " where " + ItemObject.ItemType + ".authoring_tool like '" + ItemObject.AuthoringTool + "' ";
                    drawingQueryRes = myInnovator.applyMethod("sg_run_sql_query", "<query>" + query + "</query>");
                    cdocumentCount = drawingQueryRes.getItemCount();
                }
                #endregion SQLSearch

                if (cdocumentCount != 0)
                {
                    if (drawingQueryRes.isError())
                    {
                        throw (new Exceptions.ConnectionException("Exception occured in 'SearchObject' method.\n Error string is :" + drawingQueryRes.getErrorString()));
                    }
                }

                for (int docCount = 0; docCount < cdocumentCount; docCount++)
                {
                    Item resultDrawing = drawingQueryRes.getItemByIndex(docCount);
                    PLMObject drawingObject = new PLMObject();


                    drawingObject.NativeFileId = resultDrawing.getProperty("native_file");
                    drawingObject.ObjectId = resultDrawing.getProperty("id");
                    drawingObject.ObjectName = resultDrawing.getProperty("name");

                    drawingObject.ObjectNumber = resultDrawing.getProperty("item_number");
                    drawingObject.ObjectProjectId = resultDrawing.getPropertyAttribute("sg_project_name", "keyed_name", " ");
                    drawingObject.ObjectProjectName = resultDrawing.getProperty("sg_project_id");
                    drawingObject.ObjectOwnerCompany = resultDrawing.getProperty("sg_owner_company");
                    drawingObject.ObjectRevision = resultDrawing.getProperty("major_rev");
                    drawingObject.ObjectState = resultDrawing.getProperty("state");
                    drawingObject.LockStatus = resultDrawing.getLockStatus().ToString();
                    drawingObject.ObjectGeneration = resultDrawing.getProperty("generation");
                    drawingObject.ItemType = resultDrawing.getType().ToString();
                    drawingObject.ObjectDescription = resultDrawing.getProperty("description");
                    drawingObject.Classification = resultDrawing.getProperty("classification");
                    if (resultDrawing.getLockStatus() == 2)
                        drawingObject.LockBy = GetLockBy(resultDrawing.getProperty("locked_by_id"));
                    else
                        drawingObject.LockBy = " ";

                    searchRslt.ObjectList.Add(drawingObject);


                }
                return searchRslt;

            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("ArasConnector SearchObject Exception Message : " + ex.Message));
            }
        }

        public void ExpandObject(ref PLMObject obj, List<RelationshipNavigatorRequest> relRequests, RelationshipDataSpecs dataSpecs, int ExpandObjcount)
        {

            try
            {

                if (myConnection != null)
                {
                    myInnovator = new Innovator(myConnection);
                }

                String formSideAttributeString = GetAttributes(dataSpecs.FromObjectDataSpecs);

                foreach (RelationshipNavigatorRequest rel in relRequests)
                {

                    String relname = rel.RelName;
                    Item CADStructure = myInnovator.newItem(relname, "get");
                    CADStructure.setProperty("source_id", obj.ObjectId);
                    CADStructure.setAttribute("select", "related_id(" + formSideAttributeString + "),id");

                    Item CADStructureRes = CADStructure.apply();

                    if (CADStructureRes.isError())
                    {
                        throw (new Exceptions.ConnectionException("Exception occured in 'ExpandObject' method.\n Error string is :" + CADStructureRes.getErrorString()));

                    }

                    int cdocumentCount = CADStructureRes.getItemCount();

                    for (int docCount = 0; docCount < cdocumentCount; docCount++)
                    {
                        PLMObject drawingObject = new PLMObject();
                        Relationship fromSide = new Relationship();

                        Item resultRel = null;
                        Item resultDrawing = null;

                        if (CheckoutView == "Current" && newItemObject == "" || CheckoutView == "Released" && newItemObject == "" || CheckoutView == "Released" && newItemObject == "Mechanical/Assembly")
                        {
                            resultRel = CADStructureRes.getItemByIndex(docCount);
                            resultDrawing = resultRel.getItemsByXPath("related_id/Item[@type='" + obj.ItemType + "']");
                            drawingObject.NativeFileId = resultDrawing.getProperty("native_file");
                            drawingObject.ObjectId = resultDrawing.getProperty("id");
                            drawingObject.ObjectName = resultDrawing.getProperty("name");
                            drawingObject.ObjectNumber = resultDrawing.getProperty("item_number");
                            drawingObject.ObjectProjectId = resultDrawing.getPropertyAttribute("sg_project_name", "keyed_name", " ");
                            drawingObject.ObjectProjectName = resultDrawing.getProperty("sg_project_id");
                            drawingObject.ObjectOwnerCompany = resultDrawing.getProperty("sg_owner_company");
                            drawingObject.ObjectRevision = resultDrawing.getProperty("major_rev");
                            drawingObject.ObjectState = resultDrawing.getProperty("state");
                            drawingObject.LockStatus = resultDrawing.getLockStatus().ToString();
                            drawingObject.ObjectGeneration = resultDrawing.getProperty("generation");
                            drawingObject.ItemType = resultDrawing.getType().ToString();
                            drawingObject.ObjectDescription = resultDrawing.getProperty("description");
                            drawingObject.Classification = resultDrawing.getProperty("classification");
                            if (resultDrawing.getLockStatus() == 2)
                                drawingObject.LockBy = GetLockBy(resultDrawing.getProperty("locked_by_id"));
                            else
                                drawingObject.LockBy = " ";

                            fromSide.FromObject = obj;
                            fromSide.ToObject = drawingObject;
                            fromSide.RelationshipName = relname;
                            fromSide.RelationshipID = CADStructureRes.getItemByIndex(docCount).getProperty("id");

                            obj.FromRelationships.Add(fromSide);

                        }
                        if (CheckoutView == "Current" && newItemObject == "Mechanical/Assembly")
                        {
                            resultRel = drawingQueryRes.getItemByIndex(docCount);             //ExpandObjcount


                            resultDrawing = resultRel.getItemsByXPath("Relationships/Item[@type='CAD Structure']/related_id/Item[@type='CAD']");

                            drawingObject.NativeFileId = resultDrawing.getItemByIndex(docCount).getProperty("native_file");
                            drawingObject.ObjectId = resultDrawing.getItemByIndex(docCount).getProperty("id");
                            drawingObject.ObjectName = resultDrawing.getItemByIndex(docCount).getProperty("name");
                            drawingObject.ObjectNumber = resultDrawing.getItemByIndex(docCount).getProperty("item_number");
                            drawingObject.ObjectProjectId = resultDrawing.getItemByIndex(docCount).getPropertyAttribute("sg_project_name", "keyed_name", " ");
                            drawingObject.ObjectProjectName = resultDrawing.getItemByIndex(docCount).getProperty("sg_project_id");
                            drawingObject.ObjectOwnerCompany = resultDrawing.getItemByIndex(docCount).getProperty("sg_owner_company");
                            drawingObject.ObjectRevision = resultDrawing.getItemByIndex(docCount).getProperty("major_rev");
                            drawingObject.ObjectState = resultDrawing.getItemByIndex(docCount).getProperty("state");
                            drawingObject.LockStatus = resultDrawing.getItemByIndex(docCount).getLockStatus().ToString();
                            drawingObject.ObjectGeneration = resultDrawing.getItemByIndex(docCount).getProperty("generation");
                            drawingObject.ItemType = resultDrawing.getItemByIndex(docCount).getType().ToString();
                            drawingObject.ObjectDescription = resultDrawing.getItemByIndex(docCount).getProperty("description");
                            drawingObject.Classification = resultDrawing.getItemByIndex(docCount).getProperty("classification");
                            if (resultDrawing.getItemByIndex(docCount).getLockStatus() == 2)
                                drawingObject.LockBy = GetLockBy(resultDrawing.getItemByIndex(docCount).getProperty("locked_by_id"));
                            else
                                drawingObject.LockBy = " ";

                            fromSide.FromObject = obj;
                            fromSide.ToObject = drawingObject;
                            fromSide.RelationshipName = relname;
                            fromSide.RelationshipID = drawingQueryRes.getItemByIndex(docCount).getProperty("id");

                            obj.FromRelationships.Add(fromSide);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("ArasConnector Exception Message : " + ex.Message));

            }

        }

        public void ExpandObjectRecursively(ref PLMObject obj, List<RelationshipNavigatorRequest> relRequests, RelationshipDataSpecs dataSpecs, int staticCount)
        {
            try
            {
                SearchResult searchRslt = new SearchResult();
                if (myConnection != null)
                {
                    myInnovator = new Innovator(myConnection);
                }
                String formSideAttributeString = GetAttributes(dataSpecs.FromObjectDataSpecs);
                foreach (RelationshipNavigatorRequest rel in relRequests)
                {
                    String relname = rel.RelName;
                    Item CADStructure = myInnovator.newItem(relname, "get");
                    CADStructure.setProperty("source_id", obj.ObjectId);
                    CADStructure.setAttribute("select", "related_id(" + formSideAttributeString + "),id");
                    Item CADStructureRes = CADStructure.apply();
                    int cdocumentCount = CADStructureRes.getItemCount();
                    if (cdocumentCount != 0)
                    {
                        if (CADStructureRes.isError())
                        {
                            throw (new Exceptions.ConnectionException("Exception occured in 'ExpandObjectRecursively' method.\n Error string is :" + CADStructureRes.getErrorString()));
                        }
                        int relCount = 0;
                        for (int docCount = 0; docCount < cdocumentCount; docCount++)
                        {
                            Relationship toSide = new Relationship();
                            Relationship fromSide = new Relationship();
                            Item resultRel = null;
                            Item resultDrawing = null;
                            PLMObject drawingObject = new PLMObject();

                            #region "CheckOutView: Current"
                            if (CheckoutView == "Current")// && newItemObject == "Mechanical/Assembly" || CheckoutView == "Current" && newItemObject == "DWG")
                            {
                                resultRel = CADStructureRes.getItemByIndex(docCount);
                                Item resultDrawing1 = resultRel.getItemsByXPath("related_id/Item[@type='" + obj.ItemType + "']");
                                Item resultDrawing2 = myInnovator.getItemById(obj.ItemType, resultDrawing1.getProperty("id"));
                                resultDrawing = myInnovator.applyAML("<AML><Item type='CAD' action='get'><config_id condition='eq'>" + resultDrawing2.getItemByIndex(0).getProperty("config_id") + "</config_id><is_current>1</is_current></Item></AML>");
                                if (resultDrawing.getItemCount() < 1) continue;
                                //resultDrawing = myInnovator.getItemByKeyedName("CAD", resultDrawing2.getItemByIndex(0).getProperty("keyed_name"));
                                drawingObject.ObjectId = resultDrawing.getItemByIndex(0).getProperty("id");
                                drawingObject.NativeFileId = resultDrawing.getItemByIndex(0).getProperty("native_file");
                                drawingObject.ObjectName = resultDrawing.getItemByIndex(0).getProperty("name");
                                drawingObject.ObjectNumber = resultDrawing.getItemByIndex(0).getProperty("item_number");
                                drawingObject.ObjectProjectId = resultDrawing.getItemByIndex(0).getPropertyAttribute("sg_project_name", "keyed_name", " ");
                                drawingObject.ObjectProjectName = resultDrawing.getItemByIndex(0).getProperty("sg_project_id");
                                drawingObject.ObjectOwnerCompany = resultDrawing.getItemByIndex(0).getProperty("sg_owner_company");
                                drawingObject.ObjectRevision = resultDrawing.getItemByIndex(0).getProperty("major_rev");
                                drawingObject.ObjectState = resultDrawing.getItemByIndex(0).getProperty("state");
                                drawingObject.LockStatus = resultDrawing.getItemByIndex(0).getLockStatus().ToString();
                                drawingObject.ObjectGeneration = resultDrawing.getItemByIndex(0).getProperty("generation");
                                drawingObject.ItemType = resultDrawing.getItemByIndex(0).getType().ToString();
                                drawingObject.ObjectDescription = resultDrawing.getItemByIndex(0).getProperty("description");
                                drawingObject.Classification = resultDrawing.getItemByIndex(0).getProperty("classification");
                                if (resultDrawing.getLockStatus() == 2)
                                    drawingObject.LockBy = GetLockBy(resultDrawing.getItemByIndex(0).getProperty("locked_by_id"));
                                else
                                    drawingObject.LockBy = " ";
                                fromSide.FromObject = obj;
                                fromSide.ToObject = drawingObject;
                                fromSide.RelationshipName = relname;
                                fromSide.RelationshipID = CADStructureRes.getItemByIndex(docCount).getProperty("id");
                                obj.FromRelationships.Add(fromSide);
                                searchRslt.ObjectList.Add(drawingObject);
                                Item treeDrawing = myInnovator.newItem(relname, "get");
                                treeDrawing.setProperty("source_id", drawingObject.ObjectId);
                                treeDrawing.setAttribute("select", "source_id(id,keyed_name),related_id(id,keyed_name)");
                                Item treeDrawingRes = treeDrawing.apply();
                                relCount = treeDrawingRes.getItemCount();
                                if (relCount > 0)
                                {
                                    drawingObject.IsRel = "+";
                                    staticCount++;
                                    ExpandObjectRecursively(ref drawingObject, relRequests, dataSpecs, staticCount);
                                }
                                else
                                    drawingObject.IsRel = " ";
                            }
                            #endregion "CheckOutView: Current"

                            #region "CheckOutView: AsSaved"
                            else
                            {
                                resultRel = CADStructureRes.getItemByIndex(docCount);
                                resultDrawing = resultRel.getItemsByXPath("related_id/Item[@type='" + obj.ItemType + "']");
                                drawingObject.ObjectId = resultDrawing.getProperty("id");
                                drawingObject.NativeFileId = resultDrawing.getProperty("native_file");
                                drawingObject.ObjectName = resultDrawing.getProperty("name");
                                drawingObject.ObjectNumber = resultDrawing.getProperty("item_number");
                                drawingObject.ObjectProjectId = resultDrawing.getPropertyAttribute("sg_project_name", "keyed_name", " ");
                                drawingObject.ObjectProjectName = resultDrawing.getProperty("sg_project_id");
                                drawingObject.ObjectOwnerCompany = resultDrawing.getProperty("sg_owner_company");
                                drawingObject.ObjectRevision = resultDrawing.getProperty("major_rev");
                                drawingObject.ObjectState = resultDrawing.getProperty("state");
                                drawingObject.LockStatus = resultDrawing.getLockStatus().ToString();
                                drawingObject.ObjectGeneration = resultDrawing.getProperty("generation");
                                drawingObject.ItemType = resultDrawing.getType().ToString();
                                drawingObject.ObjectDescription = resultDrawing.getProperty("description");
                                drawingObject.Classification = resultDrawing.getProperty("classification");
                                if (resultDrawing.getLockStatus() == 2)
                                    drawingObject.LockBy = GetLockBy(resultDrawing.getProperty("locked_by_id"));
                                else
                                    drawingObject.LockBy = " ";
                                fromSide.FromObject = obj;
                                fromSide.ToObject = drawingObject;
                                fromSide.RelationshipName = relname;
                                fromSide.RelationshipID = CADStructureRes.getItemByIndex(docCount).getProperty("id");
                                obj.FromRelationships.Add(fromSide);
                                searchRslt.ObjectList.Add(drawingObject);
                                Item treeDrawing = myInnovator.newItem(relname, "get");
                                treeDrawing.setProperty("source_id", drawingObject.ObjectId);
                                treeDrawing.setAttribute("select", "source_id(id),related_id(id)");
                                Item treeDrawingRes = treeDrawing.apply();
                                relCount = treeDrawingRes.getItemCount();
                                if (relCount > 0)
                                {
                                    drawingObject.IsRel = "+";
                                    staticCount++;
                                    ExpandObjectRecursively(ref drawingObject, relRequests, dataSpecs, staticCount);
                                }
                                else
                                    drawingObject.IsRel = " ";
                            }
                            #endregion "CheckOutView: AsSaved"
                        }//end for                      
                    }
                }//end foreach
            }//end Try          
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("ArasConnector Exception Message : " + ex.Message));
            }
        }

        public void LoadObject(ref DataTable itemInfo, string checkOutPath, ObjectDataSpecs dataSpecs)
        {
            try
            {
                if (myConnection != null)
                {
                    myInnovator = new Innovator(myConnection);
                }

                String attributeString = GetAttributes(dataSpecs);

                foreach (DataRow rw in itemInfo.Rows)
                {
                    string openMode = rw["ProjectId"].ToString();
                    Item drawinItemgQuery = myInnovator.newItem(rw["Type"].ToString(), "get");
                    Item drawingItem = null;

                    drawinItemgQuery.setProperty("id", rw["DrawingId"].ToString());
                    drawinItemgQuery.setAttribute("select", attributeString);
                    drawingItem = drawinItemgQuery.apply();
                    if (drawingItem.isError())
                    {
                        throw (new Exceptions.ConnectionException("Exception occured in 'LoadObject' method.\n Error string is :" + drawingItem.getErrorString()));
                    }

                    #region "Checkout Layout"

                    if (drawingItem.getItemByIndex(0).getProperty("classification") == "Layout")
                    {
                        Item sg_Layout = myInnovator.newItem();
                        sg_Layout.loadAML("<Item type='sg_RelatedCADLayout' action='get'>" +
                                                "<related_id>" +
                                                    "<Item type='CAD' action='get'>" +
                                                        "<id>" + rw["DrawingId"].ToString() + "</id>" +
                                                    "</Item>" +
                                                "</related_id>" +
                                           "</Item>");
                        Item sg_LayoutRes = sg_Layout.apply();

                        if (sg_LayoutRes.isError())
                        {
                            throw (new Exceptions.ConnectionException("Exception occured during 'LoadLayoutRel'.\n Error string is :" + sg_LayoutRes.getErrorString()));
                        }
                        drawingItem = myInnovator.getItemById("CAD", sg_LayoutRes.getItemByIndex(0).getProperty("source_id"));

                        if (drawingItem.isError())
                        {
                            throw (new Exceptions.ConnectionException("Exception occured during 'LoadLayout'.\n Error string is :" + drawingItem.getErrorString()));
                        }
                    }
                    #endregion "Checkout Layout"

                    String nativeFileId = "";
                    if (drawingItem.getItemByIndex(0).getProperty("native_file") != null)
                    {
                        nativeFileId = drawingItem.getItemByIndex(0).getProperty("native_file");
                        Item UserQuery = myInnovator.newItem("User", "get");
                        UserQuery.setAttribute("id", myInnovator.getUserID());
                        UserQuery.setAttribute("select", "working_directory");
                        Item UserItem = UserQuery.apply();
                        String workingDirectory = UserItem.getItemByIndex(0).getProperty("working_directory");
                        String lockedBy = "";
                        if (openMode == "Edit" && drawingItem.getItemByIndex(0).getLockStatus() == 0)
                            drawingItem.lockItem();
                        else if (drawingItem.getItemByIndex(0).getLockStatus() == 2)
                            lockedBy = GetLockBy(drawingItem.getItemByIndex(0).getProperty("locked_by_id"));
                        else
                            lockedBy = " ";


                        Item nativeFile = myInnovator.newItem("File", "get");
                        UserQuery.setAttribute("select", "filename");
                        nativeFile.setProperty("id", nativeFileId);
                        Item nativeFileRes = nativeFile.apply();
                        //String fileName = nativeFileRes.getItemByIndex(0).getProperty("filename");

                        itemObject.NativeFileName = nativeFileRes.getItemByIndex(0).getProperty("filename");
                        bool overwrite = true;
                        checkOutPath  = checkOutPath + "\\";
                        FileInfo file = new FileInfo(checkOutPath  + itemObject.NativeFileName);
                        if (file.Exists)
                        {
                            if (MessageBox.Show(checkOutPath + itemObject.NativeFileName + " already exist. Do you want to overwrite it?", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                                overwrite = false;
                        }
                        if (overwrite)
                        {
                            try
                            {
                                //nativeFileRes.lockItem();
                                nativeFileRes.getItemByIndex(0).checkout(checkOutPath);
                            }
                            catch (Exception ex)
                            {
                                rw["error"] = "Failed to Download File: " + ex.Message;
                            }
                        }
                        try
                        {
                            rw["DrawingId"] = drawingItem.getProperty("id");
                            rw["NativeFileName"] = nativeFileRes.getItemByIndex(0).getProperty("filename");
                            rw["DrawingNumber"] = drawingItem.getProperty("item_number");
                            rw["DrawingName"] = drawingItem.getProperty("name");
                            rw["Classification"] = drawingItem.getProperty("classification");
                            rw["DrawingState"] = drawingItem.getProperty("state");
                            rw["Revision"] = drawingItem.getProperty("major_rev");
                            rw["LockStatus"] = drawingItem.getLockStatus().ToString();
                            rw["Generation"] = drawingItem.getProperty("generation");
                            rw["Type"] = drawingItem.getType().ToString();
                            rw["IsFile"] = true;
                            rw["LockBy"] = lockedBy;
                            rw["error"] = "";
                            rw["ProjectName"] = drawingItem.getPropertyAttribute("sg_project_name", "keyed_name", " ").ToString();
                            rw["ProjectId"] = drawingItem.getProperty("sg_project_id").ToString();
                            rw["CreatedBy"] = drawingItem.getPropertyAttribute("created_by_id", "keyed_name", " ").ToString();
                            rw["CreatedOn"] = drawingItem.getProperty("created_on").ToString();
                            rw["ModifiedBy"] = drawingItem.getPropertyAttribute("modified_by_id", "keyed_name", " ").ToString();
                            rw["ModifiedOn"] = drawingItem.getProperty("modified_on").ToString();
                        }
                        catch (Exception ex)
                        {
                            rw["error"] = "ArasConnector Exception Message : " + ex.Message;
                        }
                    }
                    else
                    {
                        itemObject.IsFile = false;
                        itemObject.ObjectNumber = drawingItem.getProperty("item_number");
                        itemObject.ObjectRevision = drawingItem.getProperty("major_rev");
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("ArasConnector Exception Message : " + ex.Message));
            }
        }

        public void LockObject(List<PLMObject> plmObjs)
        {
            try
            {
                if (myConnection != null)
                {
                    myInnovator = new Innovator(myConnection);

                    foreach (PLMObject plmObj in plmObjs)
                    {
                        Item drawingQuery = myInnovator.newItem(plmObj.ItemType, "get");
                        Item drawingQueryRes = null;

                        drawingQuery.setProperty("id", plmObj.ObjectId);
                        drawingQueryRes = drawingQuery.apply();
                        bool is_need = true;
                        if (drawingQueryRes.getProperty("is_current") != "1")
                        {
                            if (MessageBox.Show("Aras has updated Version of Drawing " + drawingQueryRes.getProperty("name").ToString() + ", Eventhough Would you like to update it?", "Aras has updated Version of this Drawing", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                                is_need = true;
                            else
                                is_need = false;
                        }
                        if (is_need)
                        {
                            if (drawingQueryRes.lockItem().isError())
                            {
                                throw (new Exceptions.ConnectionException("Exception occured in 'LockObject' method.\n Error string is :" + drawingQueryRes.lockItem().getErrorString()));
                            }
                        }
                        else
                        {
                            throw (new Exceptions.ConnectionException("Please download latest Version of Drawing from Aras!!!"));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("ArasConnector Exception Message : " + ex.Message));
            }
        }

        public void UnlockObject(List<PLMObject> plmObjs)
        {
            try
            {
                if (myConnection != null)
                {
                    myInnovator = new Innovator(myConnection);

                    foreach (PLMObject plmObj in plmObjs)
                    {
                        Item drawingQuery = myInnovator.newItem(plmObj.ItemType, "get");
                        Item drawingQueryRes = null;

                        drawingQuery.setProperty("id", plmObj.ObjectId);
                        drawingQueryRes = drawingQuery.apply();
                        if (drawingQueryRes.unlockItem().isError())
                        {
                            throw (new Exceptions.ConnectionException("Exception occured in 'UnlockObject' method.\n Error string is :" + drawingQueryRes.unlockItem().getErrorString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("ArasConnector Exception Message : " + ex.Message));

            }
        }

        public void SaveObject(ref List<PLMObject> plmobjs)
        {
            Hashtable SavedData = new Hashtable();
            try
            {
                String sourceid = "";
                String newDrawingId = "";
                String ProjectName = "";
                if (myConnection != null)
                {
                    myInnovator = new Innovator(myConnection);
                }

                #region "Release PLMObjects"
                ReleasePLMObjects(plmobjs);
                #endregion

                #region "Save PLMObjects"
                foreach (PLMObject obj in plmobjs)
                {
                    Item cadDocument_QRY = null;
                    Item cadDocument_RES = null;
                    Array Layouts = obj.ObjectLayouts.Split('$');

                    #region "If Drawing is in ARASInnovator"

                    if (!obj.IsNew)
                    {
                        if ((ProjectName == null || ProjectName == "") && (obj.ObjectId != null || obj.ObjectId != ""))
                        {
                            Item MyCAD = myInnovator.getItemById(obj.ItemType, obj.ObjectId);
                            ProjectName = MyCAD.getItemByIndex(0).getProperty("sg_project_name");
                        }

                        #region CreateManualVersion
                        if (obj.IsManualVersion)
                        {
                            cadDocument_QRY = myInnovator.newItem(obj.ItemType, "get");
                            cadDocument_QRY.setProperty("id", obj.ObjectId);
                            cadDocument_RES = cadDocument_QRY.apply();
                            if (cadDocument_RES.isError())
                            {
                                throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + cadDocument_RES.getErrorString()));
                            }
                            string prevFileId = cadDocument_RES.getItemByIndex(0).getProperty("native_file");
                            Item fileItem = myInnovator.newItem("File", "add");
                            fileItem.setAttribute("where", "id='" + prevFileId + "'");
                            string seperatefilename = Path.GetFileName(obj.FilePath);
                            fileItem.setProperty("filename", seperatefilename);
                            fileItem.setID(myInnovator.getNewID());
                            fileItem.attachPhysicalFile(obj.FilePath);
                            Item checkin_result = fileItem.apply();
                            string newFileId = checkin_result.getItemByIndex(0).getProperty("id");
                            if (checkin_result.isError())
                            {
                                throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + checkin_result.getErrorString()));
                            }
                            if (obj.IsCreateNewRevision)
                            {
                                cadDocument_RES.unlockItem();
                                if (cadDocument_RES.apply("PE_CreateNewRevision").isError())
                                {
                                    throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + cadDocument_RES.apply("PE_CreateNewRevision").getErrorString()));
                                }

                            }
                            else
                            {
                                //if (cadDocument_RES.getProperty("is_released").ToString() != "1")
                                    if (cadDocument_RES.apply("version").isError())
                                    {
                                        throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + cadDocument_RES.apply("version").getErrorString()));
                                    }
                            }
                            cadDocument_QRY = myInnovator.newItem(obj.ItemType, "get");
                            cadDocument_QRY.setProperty("item_number", cadDocument_RES.getProperty("item_number"));
                            cadDocument_QRY.setProperty("is_current", "1");
                            cadDocument_RES = cadDocument_QRY.apply();
                            if (cadDocument_RES.isError())
                            {
                                throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is : " + cadDocument_RES.getErrorString()));
                            }
                            Item newCadFile = myInnovator.newItem(obj.ItemType, "edit");
                            newCadFile.setAttribute("where", "id='" + cadDocument_RES.getProperty("id").ToString() + "'");
                            newCadFile.setProperty("native_file", newFileId);
                            if (obj.ObjectDescription.ToString().Length > 0)
                                newCadFile.setProperty("description", obj.ObjectDescription);
                            Item newCadFile_RES = newCadFile.apply();
                            newCadFile_RES.unlockItem();
                            SavedData.Add(seperatefilename, obj.ObjectId);
                            if (newCadFile_RES.isError())
                            {
                                throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + newCadFile_RES.getErrorString()));
                            }
                            newDrawingId = newCadFile_RES.getProperty("id");
                        }
                        #endregion CreateManualVersion

                        #region NoManualVersion
                        else
                        {
                            cadDocument_QRY = myInnovator.newItem(obj.ItemType, "get");
                            cadDocument_QRY.setProperty("id", obj.ObjectId);
                            cadDocument_RES = cadDocument_QRY.apply();
                            if (cadDocument_RES.isError())
                            {
                                throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + cadDocument_RES.getErrorString()));
                            }
                            string prevFileId = cadDocument_RES.getItemByIndex(0).getProperty("native_file");

                            Item fileItem = myInnovator.newItem("File", "add");
                            fileItem.setAttribute("where", "id='" + prevFileId + "'");
                            string seperatefilename = Path.GetFileName(obj.FilePath);
                            fileItem.setProperty("filename", seperatefilename);
                            fileItem.setID(myInnovator.getNewID());
                            fileItem.attachPhysicalFile(obj.FilePath);
                            Item checkin_result = fileItem.apply();
                            string newFileId = checkin_result.getItemByIndex(0).getProperty("id");
                            if (checkin_result.isError())
                            {
                                throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject NoManualVersion' method.\n Error string is :" + checkin_result.getErrorString()));
                            }
                            Item newCadFile = myInnovator.newItem(obj.ItemType, "edit");
                            newCadFile.setAttribute("where", "id='" + cadDocument_RES.getProperty("id").ToString() + "'");
                            newCadFile.setProperty("native_file", newFileId);
                            if (obj.ObjectDescription.ToString().Length > 1)
                                newCadFile.setProperty("description", obj.ObjectDescription);
                            Item newCadFile_RES = newCadFile.apply();
                            newCadFile_RES.unlockItem();
                            if (newCadFile_RES.isError())
                            {
                                throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject NoManualVersion' method.\n Error string is :" + newCadFile_RES.getErrorString()));
                            }
                            newDrawingId = newCadFile_RES.getProperty("id");
                        }
                        #endregion NoManualVersion
                    }

                    #endregion "If Drawing is in ARASInnovator"

                    #region "If Drawing is not in ARASInnovator"
                    else
                    {
                        obj.IsManualVersion = true;
                        Item fileItem = myInnovator.newItem("File", "add");
                        string seperatefilename = Path.GetFileName(obj.FilePath);
                        fileItem.setProperty("filename", seperatefilename);
                        fileItem.setID(myInnovator.getNewID());
                        fileItem.attachPhysicalFile(obj.FilePath);
                        Item checkin_result = fileItem.apply();
                        string newFileId = checkin_result.getItemByIndex(0).getProperty("id");
                        if (checkin_result.isError())
                        {
                            throw (new Exceptions.ConnectionException("Exception occured in 'SaveObject' method.\n Error string is :" + checkin_result.getErrorString()));
                        }
                        cadDocument_QRY = myInnovator.newItem(obj.ItemType, "add");
                        cadDocument_QRY.setProperty("name", obj.ObjectName);
                        cadDocument_QRY.setProperty("item_number", obj.ObjectNumber);
                        if (obj.ObjectProjectId.ToString().Length > 1)
                        {
                            cadDocument_QRY.setProperty("sg_project_name", obj.ObjectProjectId);
                        }
                        if (obj.ObjectDescription.ToString().Length > 1)
                            cadDocument_QRY.setProperty("description", obj.ObjectDescription);
                        cadDocument_QRY.setProperty("authoring_tool", obj.AuthoringTool);
                        if (obj.Classification != "Non" && obj.Classification != null && obj.Classification != "")
                            cadDocument_QRY.setProperty("classification", obj.Classification);
                        cadDocument_QRY.setProperty("native_file", newFileId);
                        cadDocument_RES = cadDocument_QRY.apply();
                        newDrawingId = cadDocument_RES.getProperty("id");
                        SavedData.Add(seperatefilename, cadDocument_RES.getProperty("id"));

                        #region AddtoPart
                        if (obj.ObjectRealtyId.ToString().Length > 1)
                        {
                            Item sg_Part = myInnovator.applyAML("<AML><Item type='Part' action='get'><item_number>" + obj.ObjectRealtyId + "</item_number></Item></AML>");

                            Item PartCADItem = myInnovator.newItem("Part CAD", "add");
                            PartCADItem.setProperty("source_id", sg_Part.getProperty("id"));
                            PartCADItem.setProperty("related_id", cadDocument_RES.getProperty("id"));
                            Item PartCADItem_RES = PartCADItem.apply();
                            if (PartCADItem_RES.isError())
                            {
                                throw (new Exceptions.ConnectionException("Exception occured in 'Realty Entity SaveObject' method.\n Error string is :" + PartCADItem_RES.getErrorString()));
                            }
                        }
                        #endregion AddtoPart

                    }
                    #endregion

                    #region "Manage Layout"
                    Item sg_CAD = myInnovator.getItemById("CAD", newDrawingId);

                    String query = "select distinct(keyed_name) from innovator.[CAD] where id in (select related_id from innovator.[sg_RelatedCADLayout] where source_id in (select id from innovator.[CAD] where item_number='" + sg_CAD.getProperty("item_number","") + "'))";
                    Item DwgLayout = myInnovator.applyMethod("sg_run_sql_query", "<query>" + query + "</query>");

                    Hashtable Layout_Old = new Hashtable();

                    for (int l = 0; l < DwgLayout.getItemCount(); l++)
                    {
                        Item Old_Layout = myInnovator.getItemByKeyedName("CAD", DwgLayout.getItemByIndex(l).getProperty("keyed_name"));
                        if (Old_Layout == null) continue;
                        if (Layout_Old.Contains(Old_Layout.getItemByIndex(0).getProperty("name"))) continue;
                        Layout_Old.Add(Old_Layout.getItemByIndex(0).getProperty("name"), Old_Layout.getItemByIndex(0).getProperty("id"));
                    }

                    foreach (string layout in Layouts)
                    {
                        if (Layout_Old.Contains(layout))
                        {
                            if (obj.IsManualVersion)
                            {
                                Item CAD = myInnovator.getItemById("CAD", Layout_Old[layout].ToString());
                                String sg_LayoutState = CAD.getProperty("state").ToString();
                                if ((sg_LayoutState != "Preliminary"))
                                {
                                    MessageBox.Show("The Layout \"" + layout + "\" is in \"" + CAD.getProperty("state").ToString() + "\" LifeCycle State. So we are unable to create new generation of it. To Create new generation of Layout \"" + layout + "\" create new REVISION of Layout using Change Process.", "Warning");
                                    //myInnovator.applyMethod("sg_run_sql_query", "<query>delete from innovator.[sg_RelatedCADLayout] where related_id='" + CAD.getProperty("id").ToString() + "' and source_id='" + newDrawingId + "'</query>");
                                }
                                else
                                {
                                    CAD = myInnovator.newItem("CAD", "edit");
                                    CAD.setAttribute("where", "id='" + Layout_Old[layout] + "'");
                                    CAD.setProperty("description", obj.ObjectDescription.ToString());
                                    Item CADRes = CAD.apply("version");
                                    CADRes.unlockItem();
                                    query = "select * from innovator.[sg_RelatedCADLayout] where related_id='" + CADRes.getProperty("id").ToString() + "' and source_id='" + newDrawingId + "'";
                                    Item LayoutCurr = myInnovator.applyMethod("sg_run_sql_query", "<query>"+query+"</query>");                                         

                                    if (LayoutCurr.getItemCount() < 1)//If Layout is not in Relationship "sg_RelatedCADLayout"
                                    {
                                        Item Layout = myInnovator.newItem("sg_RelatedCADLayout", "add");
                                        Layout.setProperty("source_id", newDrawingId);
                                        Layout.setProperty("related_id", CADRes.getProperty("id").ToString());
                                        Layout = Layout.apply();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (layout.Length > 0)
                            {
                                Item sg_Layout = myInnovator.newItem("CAD", "add");
                                sg_Layout.setProperty("name", layout);
                                sg_Layout.setProperty("item_number", myInnovator.getNextSequence("sg_CADLayoutSequence") + "_" + sg_CAD.getProperty("item_number"));
                                sg_Layout.setProperty("classification", "Layout");
                                sg_Layout.setProperty("description", obj.ObjectDescription.ToString());
                                sg_Layout.setProperty("sg_project_name", sg_CAD.getProperty("sg_project_name"));
                                Item sg_LayoutRes = sg_Layout.apply();

                                Item sg_CADItem = myInnovator.newItem("CAD", "edit");
                                sg_CADItem.setAttribute("where", "id='" + newDrawingId + "'");
                                Item CADLayoutItem = myInnovator.newItem("sg_RelatedCADLayout", "add");
                                CADLayoutItem.setRelatedItem(sg_LayoutRes);
                                sg_CADItem.addRelationship(CADLayoutItem);
                                Item sg_CADItemRES = sg_CADItem.apply();
                            }
                        }
                    }

                    //If Layout is deleted or Renamed then treat as deletion of Relationship
                    foreach (DictionaryEntry layout_old in Layout_Old)
                    {
                        Boolean flag = true;
                        foreach (string layout in Layouts)
                        {
                            if (layout == layout_old.Key.ToString())
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            Item delRelItm = myInnovator.applyMethod("sg_run_sql_query", "<query>delete from innovator.[sg_RelatedCADLayout] where source_id='" + newDrawingId + "' and related_id='" + layout_old.Value.ToString() + "'</query>");
                            // delRelItm = myInnovator.applyMethod("sg_run_sql_query", "<query>delete from innovator.[sg_RelatedEntityLayout] where related_id='" + layout_old.Value.ToString() + "'</query>");
                        }
                    }

                    //Update Realty Entity After File Checkin To Maintain Relationship "sg_RelatedEntityLayout" according to related "CAD" Items 
                    query = "select distinct(config_id) from innovator.[Part] where id in (select source_id from innovator.[Part_CAD] where related_id ='" + newDrawingId + "')";
                    Item sg_Realty =  myInnovator.applyMethod("sg_run_sql_query", "<query>"+query+"</query>");
                    
                    for (int i = 0; i < sg_Realty.getItemCount(); i++)
                    {
                        Item sg_Part = myInnovator.newItem("Part", "edit");
                        sg_Part.setAttribute("where", "config_id='" + sg_Realty.getItemByIndex(i).getProperty("config_id") + "'");
                        sg_Part = sg_Part.apply();
                    }

                    #endregion "Manage Layout"

                    #region "Add Drawings under Root Drawing"
                    /*
                       if (obj.IsManualVersion && !obj.IsNew)
                      {
                          if (obj.IsRoot)
                              sourceid = cadDocument_RES.getProperty("id");
                          else
                              newDrawingId = cadDocument_RES.getProperty("id");

                          if (!obj.IsRoot)
                          {
                              Item relationship_RES;
                              cadDocument_QRY = myInnovator.newItem(obj.ItemType, "get");
                              cadDocument_QRY.setProperty("id", sourceid);
                              relationship_RES = cadDocument_QRY.apply();

                              cadDocument_QRY = myInnovator.newItem(obj.ItemType, "get");
                              cadDocument_QRY.setProperty("config_id", relationship_RES.getProperty("config_id"));
                              cadDocument_QRY.setProperty("is_current", "1");
                              relationship_RES = cadDocument_QRY.apply();
                              Item selItm = myInnovator.applyMethod("sg_run_sql_query", "<query>select * from innovator.[CAD_Structure] where source_id='" + relationship_RES.getProperty("id") + "' and related_id in (select id from innovator.[CAD] where item_number='" + cadDocument_RES.getProperty("item_number") + "')</query>");
                              if (selItm.getItemCount() > 0)
                              {
                                  Item delItm = myInnovator.applyMethod("sg_run_sql_query", "<query>Delete from innovator.[CAD_Structure] where source_id='" + relationship_RES.getProperty("id") + "' and related_id in (select id from innovator.[CAD] where item_number='" + cadDocument_RES.getProperty("item_number") + "')</query>");  
                              }
                                cadDocument_QRY = myInnovator.newItem("CAD Structure", "add");
                                cadDocument_QRY.setProperty("source_id", relationship_RES.getProperty("id"));
                                if (relationship_RES.getProperty("project_name") != null || relationship_RES.getProperty("project_name") != "")
                                {
                                    Item NewCAD = myInnovator.getItemById("CAD", newDrawingId);
                                    if (NewCAD.getItemByIndex(0).getProperty("project_name") == null || NewCAD.getItemByIndex(0).getProperty("project_name") == "")
                                    {
                                       Item updateItm = myInnovator.applyMethod("sg_run_sql_query", "<query>update innovator.[CAD] set project_name='" + relationship_RES.getProperty("project_name") + "' where id='" + newDrawingId + "'</query>");
                                    }
                                }
                                cadDocument_QRY.setProperty("related_id", newDrawingId);
                                relationship_RES = cadDocument_QRY.apply();
                              }                          
                      }*/
                    #endregion "Add Drawings under Root Drawing"

                    #region "Update PLMObject information"
                    obj.ObjectId = cadDocument_RES.getProperty("id");
                    obj.Classification = cadDocument_RES.getProperty("classification");
                    obj.ObjectGeneration = cadDocument_RES.getProperty("generation");
                    obj.ObjectNumber = cadDocument_RES.getProperty("item_number");
                    obj.ObjectRevision = cadDocument_RES.getProperty("major_rev");
                    obj.ObjectState = cadDocument_RES.getProperty("state");
                    obj.ObjectName = cadDocument_RES.getProperty("name");
                    obj.ObjectProjectId = cadDocument_RES.getProperty("sg_project_name");
                    //obj.ObjectProjectId = cadDocument_RES.getProperty("project_id");
                    obj.LockStatus = cadDocument_RES.fetchLockStatus().ToString();
                    #endregion
                }//end For
                #endregion "Save PLMObjects"

                #region ManageCADStructureRel
                foreach (PLMObject obj in plmobjs)
                {
                    String seperatefilename = Path.GetFileName(obj.FilePath);
                    String MyDrawingId = "";
                    foreach (DictionaryEntry de in SavedData)
                    {
                        if (de.Key.ToString() == seperatefilename)
                        {
                            MyDrawingId = de.Value.ToString();
                        }
                    }
                    #region "If Drawing is in ARASInnovator"
                    if (!obj.IsNew)
                    {
                        #region "Manage RelatedData: if Source updated"

                        String query = "select distinct related_id from innovator.[CAD_STRUCTURE] where source_id='" + MyDrawingId + "'";
                        Item PrevRel = myInnovator.applyMethod("sg_run_sql_query", "<query>"+query+"</query>");
                            
                        for (int related = 0; related < PrevRel.getItemCount(); related++)
                        {
                            Item Child = myInnovator.getItemById("CAD", PrevRel.getItemByIndex(related).getProperty("related_id").ToString());
                            Item LatestChild = myInnovator.getItemByKeyedName("CAD", Child.getProperty("keyed_name").ToString());
                            
                            query = "delete from innovator.[CAD_STRUCTURE] where source_id='" + obj.ObjectId + "' and related_id='" + LatestChild.getItemByIndex(0).getProperty("id").ToString() + "'";
                            Item delRes = myInnovator.applyMethod("sg_run_sql_query", "<query>"+query+"</query>");
                            
                            Item NewCADStr = myInnovator.newItem("CAD Structure", "add");
                            NewCADStr.setProperty("source_id", obj.ObjectId);
                            NewCADStr.setProperty("related_id", LatestChild.getItemByIndex(0).getProperty("id").ToString());
                            Item NewCADStrRes = NewCADStr.apply();
                        }
                        #endregion "Manage RelatedData: if Source updated"

                        #region "Manage SourceData: if RelatedItem updated"
                        /*PrevRel = myInnovator.applyMethod("sg_run_sql_query", "<query>select distinct source_id from innovator.[CAD_STRUCTURE] where related_id='" + MyDrawingId + "'</query>");
                        for (int related = 0; related < PrevRel.getItemCount(); related++)
                        {                            
                            Item Child = myInnovator.getItemById("CAD", PrevRel.getItemByIndex(related).getProperty("source_id").ToString());
                            Item LatestChild = myInnovator.getItemByKeyedName("CAD", Child.getProperty("keyed_name").ToString());
                            Item delItm = myInnovator.applyMethod("sg_run_sql_query", "<query>delete from innovator.[CAD_STRUCTURE] where related_id='" + obj.ObjectId + "' and source_id='" + LatestChild.getItemByIndex(0).getProperty("id").ToString() + "'</query>");
                         
                            Item NewCADStr = myInnovator.newItem("CAD Structure", "add");
                            NewCADStr.setProperty("source_id", LatestChild.getItemByIndex(0).getProperty("id").ToString());
                            NewCADStr.setProperty("related_id", obj.ObjectId);
                            Item NewCADStrRes = NewCADStr.apply();                            
                            delItm = myInnovator.applyMethod("sg_run_sql_query", "<query>delete from innovator.[CAD_STRUCTURE] where related_id='" + MyDrawingId + "' and source_id='" + LatestChild.getItemByIndex(0).getProperty("id").ToString() + "'</query>");
                         
                        }*/
                        #endregion "Manage SourceData: if RelatedItem updated"
                    }
                    #endregion "If Drawing is in ARASInnovator"

                    #region "If Drawing is not in ARASInnovator"
                    else
                    {
                        if ((obj.ObjectProjectId == null || obj.ObjectProjectId == "") && (ProjectName!=""))
                        {
                            Item updateItm = myInnovator.applyMethod("sg_run_sql_query", "<query>update innovator.[CAD] set sg_project_name='" + ProjectName + "' where id='" + obj.ObjectId + "'</query>");
                        }                       
                    }
                    #endregion "If Drawing is not in ARASInnovator"

                    if (!obj.IsRoot)
                    {
                        String[] SaveID = new String[10];
                        ArrayList SourceIds = new ArrayList();
                        SaveID = obj.ObjectSourceId.ToString().Split(',');
                        for (int ids = 0; ids < SaveID.Length; ids++)
                        {
                            foreach (DictionaryEntry de in SavedData)
                            {
                                if (de.Key.ToString() == SaveID[ids] || de.Key.ToString().Substring(0, de.Key.ToString().Length - 4) == SaveID[ids])
                                {
                                    SourceIds.Add(de.Value.ToString());
                                }
                            }
                        }

                        for (int rel = 0; rel < SourceIds.Count; rel++)
                        {
                            Item Latest = myInnovator.getItemById("CAD", SourceIds[rel].ToString());
                            Item LatestCAD = myInnovator.getItemByKeyedName("CAD", Latest.getItemByIndex(0).getProperty("keyed_name"));
                            Item delItm = myInnovator.applyMethod("sg_run_sql_query", "<query>delete from innovator.[CAD_STRUCTURE] where related_id='" + obj.ObjectId + "' and source_id='" + LatestCAD.getItemByIndex(0).getProperty("id").ToString() + "'</query>");
                            
                            Item CADStructure = myInnovator.newItem("CAD Structure", "add");
                            CADStructure.setProperty("source_id", LatestCAD.getItemByIndex(0).getProperty("id"));
                            CADStructure.setProperty("related_id", obj.ObjectId);
                            Item CADStructureRes = CADStructure.apply();
                        }
                        SourceIds.Clear();
                    }
                }
                #endregion ManageCADStructureRel
            }//end Catch
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("ArasConnector SaveObject Exception Message : " + ex.Message));
            }
        }

        public String GetLatestRevision(PLMObject obj)
        {
            return null;
        }

        public List<PLMObject> GetPLMObjectInformation(List<PLMObject> plmobjs)
        {
            try
            {

                if (myConnection != null)
                {
                    myInnovator = new Innovator(myConnection);

                }

                List<PLMObject> newplmobjs = new List<PLMObject>();
                foreach (PLMObject obj in plmobjs)
                {
                    PLMObject objPlm = new PLMObject();
                    Item cadDocument_QRY = null;
                    Item cadDocument_RES = null;
                    cadDocument_QRY = myInnovator.newItem(obj.ItemType, "get");
                    cadDocument_QRY.setProperty("item_number", obj.ObjectNumber);
                    cadDocument_QRY.setProperty("is_current", "1");
                    cadDocument_RES = cadDocument_QRY.apply();
                    if (cadDocument_RES.isError())
                    {
                        throw (new Exceptions.ConnectionException("Exception occured in 'GetPLMObjectInformation' method.\n Error string is : " + cadDocument_RES.getErrorString()));

                    }
                    objPlm.Classification = cadDocument_RES.getProperty("classification");
                    objPlm.ObjectGeneration = cadDocument_RES.getProperty("generation");
                    objPlm.ObjectNumber = cadDocument_RES.getProperty("item_number");
                    objPlm.ObjectRevision = cadDocument_RES.getProperty("major_rev");
                    objPlm.ObjectState = cadDocument_RES.getProperty("state");
                    objPlm.ObjectName = cadDocument_RES.getProperty("name");
                    objPlm.ObjectProjectId = cadDocument_RES.getPropertyAttribute("sg_project_name", "keyed_name", " ");
                    objPlm.ObjectProjectName = cadDocument_RES.getProperty("sg_project_id");
                    objPlm.LockStatus = cadDocument_RES.fetchLockStatus().ToString();
                    newplmobjs.Add(objPlm);
                }
                return newplmobjs;

            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("ArasConnector Exception Message :" + ex.Message));
            }

        }

        private void ReleasePLMObjects(List<PLMObject> plmObjs)
        {
            Item cadDocument_QRY = null;
            Item cadDocument_RES = null;

            foreach (PLMObject plmObj in plmObjs)
            {
                if (plmObj.IsCreateNewRevision)
                {
                    cadDocument_QRY = myInnovator.newItem(plmObj.ItemType, "get");
                    cadDocument_QRY.setProperty("id", plmObj.ObjectId);
                    cadDocument_QRY.setAttribute("select", "owned_by_id,is_released");
                    cadDocument_RES = cadDocument_QRY.apply();

                    if (cadDocument_RES.isError())
                    {
                        throw (new Exceptions.ConnectionException("Exception occured in 'ReleasePLMObjects' method.\n Error string is :" + cadDocument_RES.getErrorString()));
                    }

                    if (plmObj.IsCreateNewRevision)
                    {
                        if (cadDocument_RES.getItemByIndex(0).getProperty("owned_by_id").ToString() == null || cadDocument_RES.getItemByIndex(0).getProperty("owned_by_id").ToString() == "")
                        {
                            throw (new Exceptions.ConnectionException("Exception occured in 'ReleasePLMObjects' method.\n Error string is :You must be a member of the Owner identity to perform this action."));

                        }

                        cadDocument_RES.getItemByIndex(0).unlockItem();

                        if (cadDocument_RES.getItemByIndex(0).getProperty("is_released").ToString() != "1")
                        {
                            Item cadDocumentRelease = cadDocument_RES.getItemByIndex(0).promote("Released", "This action is done from AutoCAD");

                            if (cadDocumentRelease.isError())
                            {

                                throw (new Exceptions.ConnectionException("Exception occured in 'ReleasePLMObjects' method.\n Error string is :" + cadDocumentRelease.getErrorString()));

                            }


                        }

                    }

                }

            }

        }

        public void LockStatus(ref DataTable itemInfo)
        {
            try
            {
                foreach (DataRow rw in itemInfo.Rows)
                {
                    if (rw["drawingid"].ToString() != "")
                    {
                        Item lockStatusQry = null;
                        Item lockStatusRes = null;
                        lockStatusQry = myInnovator.newItem(rw["type"].ToString(), "get");
                        lockStatusQry.setProperty("id", rw["drawingid"].ToString());
                        lockStatusQry.setAttribute("select", "locked_by_id, id");
                        lockStatusRes = lockStatusQry.apply();

                        if (lockStatusRes.isError())
                        {
                            throw (new Exceptions.ConnectionException("Exception occured in 'LockStatus' method.\n Error string is :" + lockStatusRes.getErrorString()));
                        }
                        rw["lockstatus"] = lockStatusRes.getLockStatus().ToString();
                        rw["drawingid"] = (String)lockStatusRes.getProperty("id");
                        if (lockStatusRes.getLockStatus() == 2)
                            rw["lockby"] = GetLockBy(lockStatusRes.getProperty("locked_by_id"));
                        else
                            rw["lockby"] = " ";
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new Exceptions.ConnectionException("ArasConnector Exception Message :" + ex.Message));

            }
        }

        public String GetAttributes(ObjectDataSpecs dataSpecs)
        {
            String attributeString = null;

            if (dataSpecs.Id)
            {
                if (attributeString != null)
                    attributeString += ",id";
                else
                    attributeString += "id";
            }
            if (dataSpecs.SGOwnerCompany)
            {
                if (attributeString != null)
                    attributeString += ",sg_owner_company";
                else
                    attributeString += "sg_owner_company";
            }
            if (dataSpecs.LockStatus)
            {
                if (attributeString != null)
                    attributeString += ",locked_by_id";
                else
                    attributeString += ",locked_by_id";
            }

            if (dataSpecs.Name)
            {
                if (attributeString != null)
                    attributeString += ",name";
                else
                    attributeString += "name";
            }

            if (dataSpecs.Number)
            {
                if (attributeString != null)
                    attributeString += ",item_number";
                else
                    attributeString += "item_number";
            }

            if (dataSpecs.Revision)
            {
                if (attributeString != null)
                    attributeString += ",major_rev";
                else
                    attributeString += "major_rev";
            }

            if (dataSpecs.State)
            {
                if (attributeString != null)
                    attributeString += ",state";
                else
                    attributeString += "state";
            }

            if (dataSpecs.Generation)
            {
                if (attributeString != null)
                    attributeString += ",generation";
                else
                    attributeString += "generation";
            }

            if (dataSpecs.ConfigId)
            {
                if (attributeString != null)
                    attributeString += ",config_id";
                else
                    attributeString += "config_id";
            }
            if (dataSpecs.Project)
            {
                if (attributeString != null)
                    attributeString += ",sg_project_name";
                else
                    attributeString += "sg_project_name";
            }
            if (dataSpecs.ProjectId)
            {
                if (attributeString != null)
                    attributeString += ",sg_project_id";
                else
                    attributeString += "sg_project_id";
            }
            if (dataSpecs.CreatedById)
            {
                if (attributeString != null)
                    attributeString += ",created_by_id";
                else
                    attributeString += "created_by_id";
            }
            if (dataSpecs.CreatedOn)
            {
                if (attributeString != null)
                    attributeString += ",created_on";
                else
                    attributeString += "created_on";
            }
            if (dataSpecs.ModifiedById)
            {
                if (attributeString != null)
                    attributeString += ",modified_by_id";
                else
                    attributeString += "modified_by_id";
            }
            if (dataSpecs.ModifiedOn)
            {
                if (attributeString != null)
                    attributeString += ",modified_on";
                else
                    attributeString += "modified_on";
            }
            foreach (string attribute in dataSpecs.Attributes)
            {
                if (attributeString != null)
                    attributeString += ("," + attribute);
                else
                    attributeString += attribute;
            }

            return attributeString;
        }

        public String GetLockBy(String lockById)
        {
            String lockedBy = " ";

            Item userItem = myInnovator.newItem("user", "get");
            userItem.setProperty("id", lockById);
            userItem.setAttribute("select", "first_name, last_name");

            Item userItemRes = userItem.apply();

            lockedBy = userItemRes.getItemByIndex(0).getProperty("first_name") + " " + userItemRes.getItemByIndex(0).getProperty("last_name");

            return lockedBy;
        }

        public Hashtable GetDrawingDetail(String drawingid)
        {
            Hashtable DrawingInfo = new Hashtable();
            Item Drawing = myInnovator.newItem("CAD", "get");
            Drawing.setProperty("id", drawingid);
            Drawing = Drawing.apply();
            if (Drawing.getItemCount() > 0)
            {
                DrawingInfo.Add("createdby", Drawing.getItemByIndex(0).getPropertyAttribute("created_by_id", "keyed_name", " ").ToString());
                DrawingInfo.Add("createdon", Drawing.getItemByIndex(0).getProperty("created_on").ToString());
                DrawingInfo.Add("modifiedby", Drawing.getItemByIndex(0).getPropertyAttribute("modified_by_id", "keyed_name", " ").ToString());
                DrawingInfo.Add("modifiedon", Drawing.getItemByIndex(0).getProperty("modified_on").ToString());
            }
            else
            {
                DateTime dt = DateTime.Now;
                Drawing = myInnovator.newItem("User", "get");
                Drawing.setProperty("id", myInnovator.getUserID());
                Drawing = Drawing.apply();
                DrawingInfo.Add("createdby", Drawing.getItemByIndex(0).getProperty("keyed_name").ToString());
                DrawingInfo.Add("createdon", dt.ToString());
                DrawingInfo.Add("modifiedby", Drawing.getItemByIndex(0).getProperty("keyed_name").ToString());
                DrawingInfo.Add("modifiedon", dt.ToString());
            }
            return DrawingInfo;
        }

        public Hashtable GetLayoutDetail(String drawingid, String layoutname)
        {
            Hashtable LayoutInfo = new Hashtable();
            /*
            String aml="<AML>"+
                            "<Item type='CAD' action='get'>"+
                                "<name>" + layoutname + "</name>" +
                                "<id condition='in'>(select related_id from innovator.[sg_RelatedCADLayout] where source_id='" + drawingid + "')</id>" +
                            "</Item>"+
                        "</AML>";
            Item sg_Layout = myInnovator.applyAML(aml); */
            String query = "SELECT id FROM innovator.[CAD] WHERE NAME='" + layoutname.Replace("&","&amp;") + "' AND ID IN (SELECT RELATED_ID FROM innovator.[sg_RelatedCADLayout] WHERE SOURCE_ID='" + drawingid + "') ORDER BY GENERATION DESC";
            Item sg_PreLayout = myInnovator.applyMethod("sg_run_sql_query", "<query>" + query + "</query>");
                
            if (sg_PreLayout.getItemCount() < 1) return LayoutInfo;
            Item sg_Layout = myInnovator.getItemById("CAD", sg_PreLayout.getItemByIndex(0).getProperty("id").ToString());

            if (sg_Layout.getItemCount() > 0)
            {
                LayoutInfo.Add("drawingname", sg_Layout.getItemByIndex(0).getProperty("name").ToString());
                LayoutInfo.Add("drawingnumber", sg_Layout.getItemByIndex(0).getProperty("item_number").ToString());
                LayoutInfo.Add("classification", sg_Layout.getItemByIndex(0).getProperty("classification").ToString());
                LayoutInfo.Add("revision", sg_Layout.getItemByIndex(0).getProperty("major_rev").ToString());
                LayoutInfo.Add("drawingid", sg_Layout.getItemByIndex(0).getProperty("id").ToString());
                LayoutInfo.Add("drawingstate", sg_Layout.getItemByIndex(0).getProperty("state").ToString());
                LayoutInfo.Add("generation", sg_Layout.getItemByIndex(0).getProperty("generation").ToString());
                LayoutInfo.Add("projectname", sg_Layout.getItemByIndex(0).getPropertyAttribute("sg_project_name", "keyed_name", " ").ToString());
                LayoutInfo.Add("projectid", sg_Layout.getItemByIndex(0).getProperty("sg_project_id"," ").ToString());
                LayoutInfo.Add("createdby", sg_Layout.getItemByIndex(0).getPropertyAttribute("created_by_id", "keyed_name", " ").ToString());
                LayoutInfo.Add("createdon", sg_Layout.getItemByIndex(0).getProperty("created_on").ToString());
                LayoutInfo.Add("modifiedby", sg_Layout.getItemByIndex(0).getPropertyAttribute("modified_by_id", "keyed_name", " ").ToString());
                LayoutInfo.Add("modifiedon", sg_Layout.getItemByIndex(0).getProperty("modified_on").ToString());
            }
            return LayoutInfo;
        }
    }
}
        #endregion
