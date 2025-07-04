﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using ACAPP = Autodesk.AutoCAD.ApplicationServices.Application;
using System.Data;
using Autodesk.AutoCAD.Geometry;

namespace RBAutocadPlugIn
{
    public class ACManager
    {

        Hashtable DocProperties;

        public void SetDrawingAttributes(Hashtable hashTable)
        {
            try
            {
                DocProperties = hashTable;
                Document Doc = ACAPP.DocumentManager.MdiActiveDocument;
                Database Db = Doc.Database;
                DocumentLock dl = Doc.LockDocument(DocumentLockMode.Write, null, null, true);
                DatabaseSummaryInfoBuilder DbSib = new DatabaseSummaryInfoBuilder();

                using (dl)
                {
                    Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = Db.TransactionManager;
                    using (Transaction aTran = tm.StartTransaction())
                    {
                        foreach (DictionaryEntry entry in DocProperties)
                        {
                            DbSib.CustomProperties.Add(entry.Key.ToString(), entry.Value == null ? string.Empty : Convert.ToString(entry.Value));
                            // DbSib.CustomPropertyTable.Add(entry.Key.ToString(), entry.Value == null ? string.Empty : Convert.ToString( entry.Value));
                        }
                        Db.SummaryInfo = DbSib.ToDatabaseSummaryInfo();
                        aTran.Commit();
                        if (Doc.IsReadOnly)
                            Doc.UpgradeDocOpen();
                        //Db.SaveAs(Db.Filename, DwgVersion.Current);
                    }
                }
                //Db.SaveAs(Db.Filename, DwgVersion.Current);


            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess("AutoCAD getting problem: AutoCADManager.cs setAttributes" + ex.Message);
                //throw (new System.Exception("AutoCAD getting problem: AutoCADManager.cs setAttributes" + ex.Message));
            }
        }

        public void UpdateExRefInfo(string FilePath, System.Data.DataTable dtFileInfo)
        {
            try
            {
                //Helper.HideProgressBar();

                Database mainDb = new Database(false, true);
                using (mainDb)
                {

                    mainDb.ReadDwgFile(FilePath, FileOpenMode.OpenForReadAndAllShare, true, null);
                    mainDb.ResolveXrefs(false, false);
                    XrefGraph xg = mainDb.GetHostDwgXrefGraph(false);
                    for (int i = 0; i < xg.NumNodes; i++)
                    {
                        XrefGraphNode xgn = xg.GetXrefNode(i);
                        GraphNode root = xg.RootNode;
                        //if (xgn.Name == FilePath)
                        //{

                        //    continue;
                        //}
                        switch (xgn.XrefStatus)
                        {
                            case XrefStatus.Unresolved:
                                //ed.WriteMessage("\nUnresolved xref \"{0}\"", xgn.Name);
                                ShowMessage.ErrorMessUD("Unresolved xref :" + xgn.Name);
                                break;
                            case XrefStatus.Unloaded:
                                //ed.WriteMessage("\nUnresolved xref \"{0}\"", xgn.Name);
                                ShowMessage.ErrorMessUD("Unloaded xref :" + xgn.Name);
                                break;
                            case XrefStatus.Unreferenced:
                                //ed.WriteMessage("\nUnresolved xref \"{0}\"", xgn.Name);
                                ShowMessage.ErrorMessUD("Unreferenced xref :" + xgn.Name);
                                break;
                            case XrefStatus.Resolved:
                                {

                                    if (xgn.Database != null)
                                    {

                                        String drawingName = Path.GetFileName(xgn.Database.Filename);
                                        for (int j = 0; j < dtFileInfo.Rows.Count; j++)
                                        {

                                            string CDrawingName = Convert.ToString(dtFileInfo.Rows[j]["DrawingName"]);
                                            string PreFix = Convert.ToString(dtFileInfo.Rows[j]["oldprefix"]);
                                            drawingName = Helper.RemovePreFixFromFileName(drawingName, PreFix);

                                            if (Path.GetFileNameWithoutExtension(Convert.ToString(dtFileInfo.Rows[j]["DrawingName"])) == Path.GetFileNameWithoutExtension(drawingName))
                                            {

                                                Hashtable ht = Helper.Table2HashTable(dtFileInfo, j);

                                                UpdateLayoutAttributeXrefFile(ht, xgn.Database.Filename, false);
                                                Helper.cadManager.CloseDocSilently(xgn.Database.Filename);
                                                SetAttributesXrefFiles(ht, xgn.Database.Filename);
                                                break;
                                            }

                                        }


                                    }
                                    break;
                                }
                        }//Switch Complete
                    }//For Complete          
                     // mainDb.SaveAs(FilePath, DwgVersion.Current);
                    mainDb.Dispose();
                }//using db complete

            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
            }
        }

        public void UpdateLayoutAttributeXrefFile(Hashtable documentProperties, string FilePath, bool KeepFileOpen)
        {
            try
            {
                // Helper.CloseProgressBar();
                if (!Helper.IsUpdateLayoutInfo)
                    return;
                bool IsDocOpend = false;

                if (!CheckForCurruntlyOpenDoc(FilePath))
                {
                    Helper.CheckFileInfoFlag = false;
                    ACAPP.DocumentManager.Open(FilePath, false);
                    IsDocOpend = true;
                    Helper.CheckFileInfoFlag = true;
                }


                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;

                Hashtable drawingAttrs = new Hashtable();
                drawingAttrs = documentProperties;

                if (drawingAttrs.Count < 0) return;
                //  using (db)
                {
                    using (doc.LockDocument())
                    {
                        using (Transaction tr = db.TransactionManager.StartTransaction())
                        {
                            #region "TraverseForLayout"
                            DBDictionary layoutDict = tr.GetObject(db.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
                            ObjectIdCollection layoutsToPlot = new ObjectIdCollection();
                            foreach (DBDictionaryEntry de in layoutDict)
                            {
                                //Helper.CloseProgressBar();
                                String layoutName = de.Key;
                                //if (layoutName != "Model")
                                {
                                    LayoutManager.Current.CurrentLayout = layoutName;
                                    Hashtable LayoutData = documentProperties; //new Hashtable();







                                    #region "TraverseForTitleblock"

                                    BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                                    string BTRT = "";
                                    if (layoutName != "Model")
                                    {
                                        BTRT = BlockTableRecord.PaperSpace;
                                    }
                                    else
                                    {
                                        BTRT = BlockTableRecord.ModelSpace;
                                    }
                                    //Helper.CloseProgressBar();
                                    //BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BTRT], OpenMode.ForRead);
                                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
                                    //BlockTableRecord btr = (BlockTableRecord)tr.GetObject(de.Value, OpenMode.ForRead);
                                    layoutsToPlot.Add(btr.LayoutId);

                                    foreach (ObjectId objId in btr)
                                    {
                                        Entity ent = (Entity)tr.GetObject(objId, OpenMode.ForRead);
                                        string blkName = ent.BlockName;
                                        if (LayoutData.Count < 1)
                                        {
                                            continue;
                                        }
                                        if (ent != null)
                                        {
                                            BlockReference br = ent as BlockReference;
                                            if (br != null)
                                            {
                                                BlockTableRecord bd = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForRead);
                                                //MessageBox.Show(bd.Name);//Block Name
                                                foreach (ObjectId arId in br.AttributeCollection)
                                                {
                                                    DBObject obj = tr.GetObject(arId, OpenMode.ForRead);
                                                    AttributeReference ar = obj as AttributeReference;

                                                    //if (ar.Tag.ToUpper() == "COORDINATOR")
                                                    //{
                                                    //    ar.UpgradeOpen();
                                                    //    ar.TextString = LayoutData["projectManager"] == null ? string.Empty : Convert.ToString(LayoutData["projectManager"]);//drawingAttrs["drawingnumber"].ToString();
                                                    //    ar.DowngradeOpen();
                                                    //}
                                                    if (ar.Tag.ToUpper() == "DRAWINGNUMBER")
                                                    {
                                                        ar.UpgradeOpen();
                                                        ar.TextString = LayoutData["DrawingNumber"] == null ? string.Empty : Convert.ToString(LayoutData["DrawingNumber"]);//drawingAttrs["drawingnumber"].ToString();
                                                        ar.DowngradeOpen();
                                                    }
                                                    if (ar.Tag.ToUpper() == "DRAWINGNAME")
                                                    {
                                                        ar.UpgradeOpen();
                                                        ar.TextString = drawingAttrs["DrawingName"] == null ? string.Empty : Convert.ToString(drawingAttrs["DrawingName"]);
                                                        ar.TextString = ar.TextString == null || ar.TextString == string.Empty ? drawingAttrs["drawingname"] == null ? string.Empty : Convert.ToString(drawingAttrs["drawingname"]) : ar.TextString;
                                                        ar.DowngradeOpen();
                                                    }
                                                    if (ar.Tag.ToUpper() == "PROJECTNAME")
                                                    {

                                                        ar.UpgradeOpen();
                                                        ar.TextString = LayoutData["ProjectName"] == null ? string.Empty : Convert.ToString(LayoutData["ProjectName"]); //drawingAttrs["projectname"].ToString();
                                                        ar.DowngradeOpen();
                                                    }
                                                    if (ar.Tag.ToUpper() == "PROJECTNUMBER")
                                                    {

                                                        ar.UpgradeOpen();
                                                        ar.TextString = LayoutData["projectno"] == null ? string.Empty : Convert.ToString(LayoutData["projectno"]); ;
                                                        ar.DowngradeOpen();
                                                    }
                                                    if (ar.Tag.ToUpper() == "DRAWINGVER")
                                                    {
                                                        ar.UpgradeOpen();
                                                        ar.TextString = drawingAttrs["Generation"] == null ? string.Empty : Convert.ToString(drawingAttrs["Generation"]); //drawingAttrs["generation"].ToString();
                                                        ar.TextString = ar.TextString == null || ar.TextString == string.Empty ? drawingAttrs["generation"] == null ? string.Empty : Convert.ToString(drawingAttrs["generation"]) : ar.TextString;
                                                        ar.DowngradeOpen();
                                                    }
                                                    if (ar.Tag.ToUpper() == "TYPE")
                                                    {
                                                        ar.UpgradeOpen();
                                                        ar.TextString = drawingAttrs["classification"] == null ? string.Empty : Convert.ToString(drawingAttrs["classification"]);
                                                        ar.DowngradeOpen();
                                                    }
                                                    if (ar.Tag.ToUpper() == "DRAWINGSTATE")
                                                    {
                                                        ar.UpgradeOpen();
                                                        ar.TextString = drawingAttrs["DrawingState"] == null ? string.Empty : Convert.ToString(drawingAttrs["DrawingState"]);
                                                        ar.TextString = ar.TextString == null || ar.TextString == string.Empty ? drawingAttrs["drawingstate"] == null ? string.Empty : Convert.ToString(drawingAttrs["drawingstate"]) : ar.TextString;
                                                        ar.DowngradeOpen();
                                                    }

                                                    if (layoutName != "Model")
                                                    {

                                                        if (ar.Tag.ToUpper() == "LAYOUTNAME")
                                                        {

                                                            ar.UpgradeOpen();
                                                            ar.TextString = layoutName;
                                                            ar.DowngradeOpen();
                                                        }
                                                        LayoutInfo objLI = Helper.FindLayoutDetail(LayoutData, layoutName);
                                                        if (objLI != null)
                                                        {
                                                            if (ar.Tag.ToUpper() == "LAYOUTNUMBER")
                                                            {

                                                                ar.UpgradeOpen();
                                                                ar.TextString = objLI.fileNo == null ? string.Empty : objLI.fileNo;
                                                                ar.DowngradeOpen();
                                                            }

                                                            if (ar.Tag.ToUpper() == "CREATEDBY")
                                                            {

                                                                ar.UpgradeOpen();
                                                                ar.TextString = objLI.createdby == null ? string.Empty : objLI.createdby; //drawingAttrs["createdby"].ToString();
                                                                ar.DowngradeOpen();
                                                            }

                                                            if (ar.Tag.ToUpper() == "MODIFIEDBY")
                                                            {

                                                                ar.UpgradeOpen();
                                                                ar.TextString = objLI.updatedby == null ? string.Empty : objLI.updatedby; //drawingAttrs["modifiedby"].ToString();
                                                                ar.DowngradeOpen();
                                                            }

                                                            if (ar.Tag.ToUpper() == "CREATEDON")
                                                            {

                                                                ar.UpgradeOpen();
                                                                ar.TextString = objLI.created0n == null ? string.Empty : Helper.FormatDate(objLI.created0n); //drawingAttrs["createdon"].ToString().Substring(0, 10); 
                                                                ar.DowngradeOpen();
                                                            }

                                                            if (ar.Tag.ToUpper() == "MODIFIEDON")
                                                            {

                                                                ar.UpgradeOpen();
                                                                ar.TextString = objLI.updatedon == null ? string.Empty : Helper.FormatDate(objLI.updatedon);//drawingAttrs["modifiedon"].ToString().Substring(0,10);
                                                                ar.DowngradeOpen();
                                                            }

                                                            if (ar.Tag.ToUpper() == "LAYOUTSTATE")
                                                            {
                                                                ar.UpgradeOpen();
                                                                ar.TextString = objLI.statusname == null ? string.Empty : objLI.statusname; //drawingAttrs["drawingstate"].ToString();
                                                                ar.DowngradeOpen();
                                                            }
                                                            if (ar.Tag.ToUpper() == "LAYOUTSTATECAPS")
                                                            {
                                                                ar.UpgradeOpen();
                                                                ar.TextString = objLI.statusname == null ? string.Empty : objLI.statusname.ToUpper(); //drawingAttrs["drawingstate"].ToString();
                                                                ar.DowngradeOpen();
                                                            }
                                                            if (ar.Tag.ToUpper() == "LAYOUTDISCIPLINE")
                                                            {
                                                                ar.UpgradeOpen();
                                                                ar.TextString = objLI.typename == null ? string.Empty : objLI.typename; //drawingAttrs["drawingstate"].ToString();
                                                                ar.DowngradeOpen();
                                                            }
                                                            if (ar.Tag.ToUpper() == "GOODNOT")
                                                            {
                                                                ar.UpgradeOpen();
                                                                if ((objLI.CoreStatus == null ? string.Empty : objLI.CoreStatus.ToLower()) == "closed")
                                                                {
                                                                    ar.TextString = "GOOD";
                                                                }
                                                                else
                                                                {
                                                                    ar.TextString = "NOT";
                                                                }
                                                                ar.DowngradeOpen();
                                                            }
                                                            if (ar.Tag.ToUpper() == "LAYOUTVER")
                                                            {

                                                                ar.UpgradeOpen();
                                                                ar.TextString = objLI.versionNo == null ? string.Empty : objLI.versionNo; //drawingAttrs["revision"].ToString();
                                                                ar.DowngradeOpen();
                                                            }

                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    } //end foreach Block
                                    #endregion "TraverseForTitleblock"                  
                                }

                            }
                            #endregion "TraverseForLayout

                            #region "PlotLayouts"
                            //PlotLayout(doc, tr, layoutsToPlot);
                            #endregion "PlotLayouts"
                            //db.Save();
                            tr.Commit();
                            tr.Dispose();
                            //db.SaveAs(FilePath, DwgVersion.Current);
                        }//end Transaction tr
                    }

                    // db.SaveAs(FilePath, DwgVersion.Current);

                }
                // if(!KeepFileOpen)
                {
                    if (IsDocOpend)
                    {
                        if (FilePath != null && FilePath.Trim().Length > 0 && File.Exists(FilePath))
                        {
                            try
                            {
                                Helper.CheckFileInfoFlag = false;
                                ACAPP.DocumentManager.MdiActiveDocument.CloseAndSave(FilePath);
                                Helper.CheckFileInfoFlag = true;
                            }
                            catch
                            {
                                Helper.CheckFileInfoFlag = true;
                                //doc.CloseAndSave(FilePath);
                            }
                        }

                    }
                }

            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message.ToString());
            }

        }

        public void SetAttributesXrefFiles(Hashtable hashTable, string FilePath)
        {
            try
            {
                DocProperties = hashTable;

                DatabaseSummaryInfoBuilder DbSib = new DatabaseSummaryInfoBuilder();


                try
                {


                    Database mainDb = new Database(false, true);
                    using (mainDb)
                    {


                        mainDb.ReadDwgFile(FilePath, FileOpenMode.OpenForReadAndAllShare, true, null);
                        Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = mainDb.TransactionManager;
                        using (Transaction aTran = tm.StartTransaction())
                        {
                            foreach (DictionaryEntry entry in DocProperties)
                            {
                                try
                                {
                                    DbSib.CustomProperties.Add(entry.Key.ToString(), entry.Value == null ? string.Empty : Convert.ToString(entry.Value));
                                    //if (entry.Key.ToString() != "LayoutInfo")
                                    {
                                        //   DbSib.CustomPropertyTable.Add(entry.Key.ToString(), entry.Value == null ? string.Empty : Convert.ToString(entry.Value));
                                    }
                                    //else
                                    //{
                                    //    DbSib.CustomPropertyTable.Add(entry.Key.ToString(), entry.Value == null ? string.Empty :  (entry.Value));
                                    //}
                                }
                                catch (System.Exception E)
                                { }


                            }
                            mainDb.SummaryInfo = DbSib.ToDatabaseSummaryInfo();
                            aTran.Commit();
                            aTran.Dispose();
                            mainDb.SaveAs(FilePath, DwgVersion.Current);
                        }


                    }//using db complete

                }
                catch (System.Exception ex)
                {
                    ShowMessage.ErrorMess(ex.Message);
                }

            }
            catch (System.Exception ex) { throw (new System.Exception("AutoCAD getting problem: AutoCADManager.cs setAttributes" + ex.Message)); }
        }

        public Hashtable GetDrawingAttributes()
        {
            Hashtable hastable = new Hashtable();
            Document Doc = ACAPP.DocumentManager.MdiActiveDocument;
            Database Db = Doc.Database;
            //DocumentLock dl = Doc.LockDocument(DocumentLockMode.Read, null, null, true);          
            IDictionaryEnumerator en = Db.SummaryInfo.CustomProperties;

            //using (dl)
            //{
            while (en.MoveNext())
            {
                //MessageBox.Show("GetAttribute:\n" + Convert.ToString(en.Key) + "------->" + Convert.ToString(en.Value));
                hastable.Add(en.Key, en.Value);
            }
            //}

            return hastable;
        }

        public System.Data.DataTable GetDrawingAttributes(string FilePath)
        {
            System.Data.DataTable dtDrawingInfo = Helper.GetDrawingPropertiesTableStructure();
            try
            {
                if (!File.Exists(FilePath))
                    return null;
                Hashtable hastable = new Hashtable();

                Database Db = new Database(false, true);
                Db.ReadDwgFile(FilePath, FileOpenMode.OpenForReadAndAllShare, true, null);

                IDictionaryEnumerator en = Db.SummaryInfo.CustomProperties;
                while (en.MoveNext())
                {
                    hastable.Add(en.Key, en.Value);
                }
                dtDrawingInfo = Helper.AddRowDrawingPropertiesTable(dtDrawingInfo, hastable);

            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

            return dtDrawingInfo;
        }

        public Hashtable GetDrawingAttributesht(string FilePath)
        {
            Hashtable hastable = new Hashtable();
            try
            {


                Database Db = new Database(false, true);
                Db.ReadDwgFile(FilePath, FileOpenMode.OpenForReadAndAllShare, true, null);

                IDictionaryEnumerator en = Db.SummaryInfo.CustomProperties;
                while (en.MoveNext())
                {
                    hastable.Add(en.Key, en.Value == null ? string.Empty : Convert.ToString(en.Value));
                }


            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }

            return hastable;
        }

        public void UpdateDrawingAttributes(Hashtable hashTable)
        {

            /*   Hashtable hastable = new Hashtable();


               Document Doc = acadApp.DocumentManager.MdiActiveDocument;

               Database Db = Doc.Database;

               DocumentLock dl = Doc.LockDocument(DocumentLockMode.Write, null, null, true);

               DatabaseSummaryInfoBuilder DbSib = new DatabaseSummaryInfoBuilder();

               IDictionaryEnumerator ht = hastable.GetEnumerator();
               IDictionaryEnumerator en = Db.SummaryInfo.CustomProperties;

               using (dl)
               {
                   Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = Db.TransactionManager;

                       using (Transaction aTran = tm.StartTransaction())
                       {
                           while (en.MoveNext())
                           {
                               // MessageBox.Show(Convert.ToString(en.Key) + "------->" + Convert.ToString(en.Value));
                               while(ht.MoveNext())
                               {
                                //   if (en.Key == ht.Key)

                               }
                           }

                       }
               }
             */

            Document Doc = ACAPP.DocumentManager.MdiActiveDocument;

            Database Db = Doc.Database;

            DocumentLock dl = Doc.LockDocument(DocumentLockMode.Write, null, null, true);

            DatabaseSummaryInfoBuilder DbSib = new DatabaseSummaryInfoBuilder();



            using (dl)
            {
                Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = Db.TransactionManager;

                using (Transaction aTran = tm.StartTransaction())
                {
                    foreach (DictionaryEntry entry in hashTable)
                    {
                        //MessageBox.Show("UpdateAttribute:\n"+Convert.ToString(entry.Key) + "------->" + Convert.ToString(entry.Value));
                        DbSib.CustomProperties.Add(entry.Key.ToString(), entry.Value.ToString());

                    }

                    Db.SummaryInfo = DbSib.ToDatabaseSummaryInfo();

                    aTran.Commit();

                }
            }
            if (Doc.IsReadOnly)
                Doc.UpgradeDocOpen();


        }

        public void UpdateFileProperties(string FileID, string FilePath)
        {
            try
            {
                RBConnector objRBC = new RBConnector();
                ResultSearchCriteria Drawing = objRBC.GetDrawingInformation(FileID);
                Hashtable htFileProperties = GetDrawingAttributes();
                System.Data.DataTable dtFileProperties = Helper.HashTable2Table(htFileProperties);

                System.Data.DataTable dtDrawingProperty = Helper.FillDrawingPropertiesTable(Drawing, FilePath, "1", Convert.ToString(dtFileProperties.Rows[0]["prefix"]));

                htFileProperties = Helper.Table2HashTable(dtDrawingProperty, 0);
                SetDrawingAttributes(htFileProperties);
                UpdateLayoutAttributeXrefFile(htFileProperties, FilePath, false);
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        public void UpdateLayoutProperties(System.Data.DataTable dtLayoutInfoChanged)
        {
            try
            {
                Hashtable htFileProperties = GetDrawingAttributes();
                System.Data.DataTable dtFileProperties = Helper.HashTable2Table(htFileProperties);
                bool IsdtChanged = false;


                foreach (DataRow dr in dtLayoutInfoChanged.Rows)
                {
                    string LNS = Convert.ToString(dr["LayoutNo"]);
                    int LN = Convert.ToInt16(LNS.Contains("_") ? LNS.Substring(0, LNS.IndexOf("_")) : "0");

                    dtFileProperties.Rows[0]["Layout_" + LN + "_Status"] = dr["Status"];
                    dtFileProperties.Rows[0]["Layout_" + LN + "_Type"] = dr["Type"];
                    IsdtChanged = true;
                }

                string LayoutInfo1 = Convert.ToString(dtFileProperties.Rows[0]["layoutinfo"]);
                List<LayoutInfo> objLI = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LayoutInfo>>(LayoutInfo1);




                if (IsdtChanged)
                {
                    SetDrawingAttributes(Helper.Table2HashTable(dtFileProperties, 0));
                }
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        public void OpenActiveDocument(String fileName, String openMode, Hashtable properties = null)
        {
            DocProperties = properties;
            try
            {
                if (openMode == "Checkout")
                {
                    ACAPP.DocumentManager.Open(fileName, false);
                    this.SetDrawingAttributes(DocProperties);
                    UpdateDrawingLayoutAttribute(DocProperties);
                    ACAPP.DocumentManager.MdiActiveDocument.CloseAndSave(fileName);
                }
                else if (openMode == "View")
                {
                    SetAttributesXrefFiles(DocProperties, fileName);
                    UpdateLayoutAttributeXrefFile(DocProperties, fileName, true);
                    ACAPP.DocumentManager.Open(fileName, false);

                }
                else if (openMode == "Edit")
                {
                    ACAPP.DocumentManager.Open(fileName, false);
                    this.SetDrawingAttributes(DocProperties);
                    UpdateDrawingLayoutAttribute(DocProperties);
                }
                else if (openMode == "updtMainDrawing")
                {
                    this.SetDrawingAttributes(DocProperties);
                    UpdateDrawingLayoutAttribute(DocProperties);
                    ACAPP.DocumentManager.MdiActiveDocument.CloseAndSave(fileName);
                    ACAPP.DocumentManager.Open(fileName, false);
                }
                else if (openMode == "updtXRDrawing")
                {
                    ACAPP.DocumentManager.Open(fileName, false);
                    this.SetDrawingAttributes(DocProperties);
                    UpdateDrawingLayoutAttribute(DocProperties);
                    ACAPP.DocumentManager.MdiActiveDocument.CloseAndSave(fileName);
                }
            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
            }
        }
        public static int Pass = 0;
        public void UpdateExRefFileName(string ParentFilePath, string FilePath, ref List<PLMObject> plmObjs)
        {
            try
            {
                //Helper.HideProgressBar();

                List<string> lstOldFiles = new List<string>();
                string ParentNewPath = "";
                bool IsMove = false;

                Database mainDb = new Database(false, true);
                using (mainDb)
                {

                    mainDb.ReadDwgFile(FilePath, FileOpenMode.OpenForReadAndAllShare, true, null);
                    mainDb.ResolveXrefs(false, false);
                    XrefGraph xg = mainDb.GetHostDwgXrefGraph(false);

                    for (int i = 0; i < xg.NumNodes; i++)
                    {
                        XrefGraphNode xgn = xg.GetXrefNode(i);
                        GraphNode root = xg.RootNode;
                        string OldChildPath = "";
                        string newpath = "";

                        if (XrefStatus.Unresolved == xgn.XrefStatus)
                        {

                            ShowMessage.ErrorMessUD("Unresolved xref :" + xgn.Name);
                        }
                        else if (XrefStatus.Unloaded == xgn.XrefStatus)
                        {

                            ShowMessage.ErrorMessUD("Unloaded xref :" + xgn.Name);
                        }
                        else if (XrefStatus.Unreferenced == xgn.XrefStatus)
                        {

                            ShowMessage.ErrorMessUD("Unreferenced xref :" + xgn.Name);
                        }
                        else if (XrefStatus.Resolved == xgn.XrefStatus)
                        {
                            if (xgn.IsNested && i > 0)
                            {
                                continue;
                            }


                            Database xdb = xgn.Database;


                            if (xdb != null)
                            {
                                bool IsSelected = false;
                                foreach (PLMObject obj in plmObjs)
                                {
                                    if (obj.FilePath == xgn.Database.Filename)
                                    {
                                        IsSelected = true; break;
                                    }
                                    if (obj.TempFilePath == xgn.Database.Filename)
                                    {
                                        IsSelected = true; break;
                                    }
                                }
                                if (!IsSelected)
                                {
                                    // this file is not selected to save so no further action is required.
                                    continue;
                                }

                                string ProjectName = "", PreFix = "", oldPreFix = "";
                                System.Data.DataTable dtDrawing = Helper.cadManager.GetDrawingAttributes(xgn.Database.Filename);

                                if (dtDrawing.Rows.Count > 0)
                                {

                                    ProjectName = Convert.ToString(dtDrawing.Rows[0]["ProjectName"]);
                                    PreFix = Convert.ToString(dtDrawing.Rows[0]["PreFix"]);
                                    oldPreFix = Convert.ToString(dtDrawing.Rows[0]["oldPreFix"]);
                                }


                                string FN = Path.GetFileNameWithoutExtension(xgn.Name);


                                string checkoutPath = Convert.ToString(Helper.GetValueRegistry("CheckoutSettings", "CheckoutDirectoryPath"));


                                if (ProjectName.Trim().Length == 0)
                                {
                                    ProjectName = "My Files";
                                }
                                checkoutPath = Path.Combine(checkoutPath, ProjectName);

                                if (!Directory.Exists(checkoutPath))
                                {
                                    Directory.CreateDirectory(checkoutPath);
                                }
                                string path = checkoutPath;

                                path += @"\" + PreFix;


                                if (xgn.Name == FilePath)
                                {

                                    string PName = Path.GetFileName(FilePath);
                                    PName = Helper.RemovePreFixFromFileName(PName, oldPreFix);
                                    PName = Helper.RemovePreFixFromFileName(PName, PreFix);



                                    string NewFilePath = path + PName;
                                    if (NewFilePath != FilePath)
                                    {
                                        File.Delete(NewFilePath);
                                        ParentNewPath = NewFilePath;
                                        if (FilePath.Contains(checkoutPath))
                                        {
                                            IsMove = true;
                                        }
                                        else
                                        {
                                            lstOldFiles = CopyXrefs(FilePath, NewFilePath, ref plmObjs);
                                        }
                                        bool IsFileNotFound = true;
                                        bool IsAlreadyAssign = true;
                                        foreach (PLMObject obj in plmObjs)
                                        {

                                            if (obj.FilePath == xgn.Name)
                                            {
                                                IsFileNotFound = false;
                                                obj.FilePath = NewFilePath;
                                                break;
                                            }
                                            if (obj.FilePath == NewFilePath)
                                            {
                                                IsAlreadyAssign = false;
                                            }
                                        }
                                        if (IsFileNotFound && IsAlreadyAssign)
                                        {
                                            ShowMessage.ErrorMessUD("File not found.\n" + xgn.Name); return;
                                        }
                                    }


                                    continue;
                                }


                                Transaction tr = xdb.TransactionManager.StartTransaction();


                                using (tr)
                                {
                                    BlockTableRecord btr = (BlockTableRecord)tr.GetObject(xgn.BlockTableRecordId, OpenMode.ForWrite);
                                    mainDb.XrefEditEnabled = true;


                                    string originalpath = btr.PathName;
                                    string childname = Path.GetFileName(originalpath);
                                    bool IsRelativePath = false;
                                    //if(originalpath.Contains(Path.))

                                    bool IsChildinSameFolder = false;
                                    string ParentPath = Path.GetDirectoryName(ParentFilePath);
                                    int L = originalpath.Length;
                                    int L1 = childname.Length;
                                    if (L == L1 + 2)
                                    {
                                        IsChildinSameFolder = true;
                                    }

                                    childname = Helper.RemovePreFixFromFileName(childname, oldPreFix);
                                    childname = Helper.RemovePreFixFromFileName(childname, PreFix);


                                    newpath = path + childname;
                                    File.Delete(newpath);

                                    OldChildPath = originalpath;
                                    //if (IsChildinSameFolder)
                                    {
                                        FileInfo fi = new FileInfo(originalpath);
                                        OldChildPath = Path.Combine(fi.DirectoryName, Path.GetFileName(originalpath));
                                    }
                                    if (File.Exists(OldChildPath))
                                    {

                                    }
                                    else
                                    {
                                        OldChildPath = Path.Combine(ParentPath, oldPreFix + childname);
                                        if (File.Exists(OldChildPath))
                                        {

                                        }
                                        else
                                        {
                                            OldChildPath = Path.Combine(ParentPath, childname);
                                            if (File.Exists(OldChildPath))
                                            {

                                            }
                                            else
                                            {
                                                OldChildPath = Path.Combine(Path.GetDirectoryName(path), oldPreFix + childname);

                                                if (File.Exists(OldChildPath))
                                                {

                                                }
                                                else
                                                {
                                                    string OP = "";
                                                    if (File.Exists(originalpath))
                                                    {
                                                        FileInfo fi = new FileInfo(originalpath);
                                                        OP = fi.DirectoryName;
                                                        OldChildPath = Path.Combine(OP, childname);
                                                    }
                                                    else
                                                    {
                                                        //ShowMessage.ErrorMess("File not found.\n" + originalpath);
                                                        //return;
                                                    }
                                                    if (File.Exists(OldChildPath))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        OldChildPath = Path.Combine(OP, oldPreFix + childname);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    int IsChildCopy = 0;
                                    if (File.Exists(OldChildPath) && OldChildPath != newpath)
                                    {

                                        if (OldChildPath.Contains(checkoutPath))
                                        {
                                            File.Move(OldChildPath, newpath);
                                            IsChildCopy = 2;
                                            //Helper.CloseProgressBar();
                                        }
                                        else
                                        {
                                            IsChildCopy = 1;

                                        }
                                        bool IsPathNotFound = true;
                                        bool IsAlreadyAssign = true;
                                        foreach (PLMObject obj in plmObjs)
                                        {
                                            string name = Path.GetFileNameWithoutExtension(obj.FilePath);

                                            name = Helper.RemovePreFixFromFileName(name, oldPreFix);

                                            string XrefName = Path.GetFileNameWithoutExtension(Helper.RemovePreFixFromFileName(xgn.Name, oldPreFix));
                                            if (name == XrefName)
                                            {
                                                IsPathNotFound = false;
                                                obj.FilePath = newpath;
                                                break;
                                            }
                                            if (obj.FilePath == newpath)
                                            {
                                                IsAlreadyAssign = false;
                                            }
                                        }
                                        if (IsPathNotFound && IsAlreadyAssign)
                                        {
                                            ShowMessage.ErrorMessUD("File not found.\n" + xgn.Name);
                                            return;
                                        }

                                    }
                                    if (IsChildCopy == 0)
                                    {

                                    }
                                    else if (IsChildCopy == 1)
                                    {
                                        string TnewPath = @".\" + Path.GetFileName(newpath);
                                        btr.PathName = TnewPath;
                                    }
                                    else if (IsChildCopy == 2)
                                    {
                                        if (File.Exists(newpath))
                                        {
                                            string TnewPath = @".\" + Path.GetFileName(newpath);
                                            btr.PathName = TnewPath;
                                        }
                                        else
                                        {
                                            ShowMessage.ErrorMessUD("File not found.\n" + newpath); return;
                                        }
                                    }


                                    tr.Commit();
                                    if (IsChildCopy == 1)
                                    {
                                        File.Copy(OldChildPath, newpath);
                                        //File.Delete(Path.Combine(Path.GetDirectoryName(newpath), Path.GetFileName(OldChildPath)));
                                    }

                                    if (File.Exists(newpath))
                                        UpdateExRefFileName(ParentFilePath, newpath, ref plmObjs);
                                }
                            }
                        }
                        else if (XrefStatus.FileNotFound == xgn.XrefStatus)
                        {
                            if (Path.GetExtension(xgn.Name) == "dwg")
                            {
                                Database xdb = mainDb;
                                DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(ParentFilePath));
                                FileInfo[] Files = di.GetFiles("*.dwg");
                                foreach (var item in Files)
                                {
                                    if (item.FullName.Contains(xgn.Name))
                                    {
                                        newpath = item.FullName;
                                        break;
                                    }
                                }

                                if (xdb != null)
                                {




                                    Transaction tr = xdb.TransactionManager.StartTransaction();


                                    using (tr)
                                    {
                                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(xgn.BlockTableRecordId, OpenMode.ForWrite);
                                        mainDb.XrefEditEnabled = true;
                                        if (File.Exists(newpath))
                                        {
                                            string TnewPath = @".\" + Path.GetFileName(newpath);
                                            btr.PathName = TnewPath;
                                        }
                                        else
                                        {
                                            ShowMessage.ErrorMessUD("File not found.\n" + newpath); return;
                                        }

                                        tr.Commit();



                                        UpdateExRefFileName(ParentFilePath, newpath, ref plmObjs);
                                    }
                                }
                            }

                        }

                        if (File.Exists(OldChildPath) && OldChildPath != newpath)
                        {
                            //File.Delete(OldChildPath);
                        }
                        //}//Switch Complete
                    }//For Complete          
                    mainDb.SaveAs(FilePath, DwgVersion.Current);
                }//using db complete
                if (FilePath == ParentFilePath)
                {
                    if (IsMove)
                    {
                        if (ParentNewPath.Trim().Length > 0 && File.Exists(FilePath))
                            File.Move(FilePath, ParentNewPath);
                    }
                    else
                    {
                        if (ParentNewPath.Trim().Length > 0 && File.Exists(FilePath))
                            File.Copy(FilePath, ParentNewPath);
                    }

                    foreach (string FP in lstOldFiles)
                    {
                        File.Delete(FP);
                    }
                }


            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
            }
        }

        public List<string> CopyXrefs(string OldParentFilePath, string NewParentFilepath, ref List<PLMObject> plmObjs)
        {
            List<string> lstOldFiles = new List<string>();
            try
            {
                string NewDir = Path.GetDirectoryName(NewParentFilepath);
                Database mainDb = new Database(false, true);
                String rootid = "";
                using (mainDb)
                {
                    mainDb.ReadDwgFile(OldParentFilePath, FileOpenMode.OpenForReadAndAllShare, true, null);
                    mainDb.ResolveXrefs(false, false);

                    XrefGraph xg = mainDb.GetHostDwgXrefGraph(false);
                    for (int i = 0; i < xg.NumNodes; i++)
                    {
                        XrefGraphNode xgn = xg.GetXrefNode(i);
                        switch (xgn.XrefStatus)
                        {
                            case XrefStatus.Unresolved:
                                //ed.WriteMessage("\nUnresolved xref \"{0}\"", xgn.Name);
                                break;
                            case XrefStatus.Unloaded:
                                //ed.WriteMessage("\nUnloaded xref \"{0}\"", xgn.Name);
                                break;
                            case XrefStatus.Unreferenced:
                                // ed.WriteMessage("\nUnreferenced xref \"{0}\"", xgn.Name);
                                break;
                            case XrefStatus.Resolved:
                                {
                                    Database xdb = xgn.Database;
                                    if (xdb != null)
                                    {
                                        if (xdb.Filename == mainDb.Filename)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            string FileName = Path.Combine(NewDir, Path.GetFileName(xdb.Filename));
                                            if (File.Exists(xdb.Filename) && xdb.Filename != FileName)
                                            {
                                                File.Delete(FileName);
                                                File.Copy(xdb.Filename, FileName);
                                                lstOldFiles.Add(FileName);

                                                foreach (PLMObject obj in plmObjs)
                                                {
                                                    if (obj.FilePath == xgn.Database.Filename)
                                                    {
                                                        obj.TempFilePath = FileName; break;
                                                    }
                                                }
                                            }

                                        }


                                    }
                                    break;
                                }
                        }//Switch Complete
                    }//For Complete                    
                }//Complete MainDB
                //mainDb.SaveAs(OldParentFilePath, DwgVersion.Current);
            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
            }
            return lstOldFiles;
        }

        public List<string> CheckXrefStatus(string ParentFilePath, string Filepath, List<clsDownloadedFiles> lstobjDownloadedFiles)
        {

            List<string> lstOldFiles = new List<string>();
            if (ParentFilePath == "" && Filepath == "")
            {
                return lstOldFiles;
            }
            if (ParentFilePath != Filepath)
                ParentFilePath = Filepath;
            try
            {
                // Helper.CloseProgressBar();
                Database mainDb = new Database(false, true);


                using (mainDb)
                {
                    mainDb.ReadDwgFile(ParentFilePath, FileOpenMode.OpenForReadAndAllShare, true, null);
                    mainDb.ResolveXrefs(false, false);
                    XrefGraph xg = mainDb.GetHostDwgXrefGraph(false);
                    //Helper.CloseProgressBar();
                    for (int i = 0; i < xg.NumNodes; i++)
                    {
                        XrefGraphNode xgn = xg.GetXrefNode(i);
                        switch (xgn.XrefStatus)
                        {
                            case XrefStatus.Unresolved:
                                //ed.WriteMessage("\nUnresolved xref \"{0}\"", xgn.Name);
                                break;
                            case XrefStatus.Unloaded:
                                //ed.WriteMessage("\nUnloaded xref \"{0}\"", xgn.Name);
                                break;
                            case XrefStatus.Unreferenced:
                                // ed.WriteMessage("\nUnreferenced xref \"{0}\"", xgn.Name);
                                break;
                            case XrefStatus.Resolved:
                                {
                                    Database xdb = xgn.Database;
                                    if (ParentFilePath != xdb.Filename)
                                    {
                                        DirectoryInfo d = new DirectoryInfo(Path.GetDirectoryName(ParentFilePath));

                                        string newpath = "";
                                        newpath = Path.Combine(Path.GetDirectoryName(ParentFilePath), Path.GetFileName(xdb.Filename));

                                        foreach (var item in lstobjDownloadedFiles)
                                        {
                                            if (item.ParentFilePath == ParentFilePath && item.FilePath == newpath)
                                            {
                                                item.XrefStatus = true; break;
                                            }
                                        }
                                        CheckXrefStatus(ParentFilePath, newpath, lstobjDownloadedFiles);
                                    }


                                    break;
                                }
                            case XrefStatus.FileNotFound:
                                {
                                    Database xdb = xgn.Database;

                                    Transaction tr = mainDb.TransactionManager.StartTransaction();


                                    using (tr)
                                    {
                                        BlockTableRecord btr = (BlockTableRecord)tr.GetObject(xgn.BlockTableRecordId, OpenMode.ForWrite);
                                        mainDb.XrefEditEnabled = true;

                                        string originalpath = Path.GetFileNameWithoutExtension(btr.PathName);
                                        string s = originalpath.Replace(btr.Name, "").Replace("#", "");
                                        if (s.Contains("-"))
                                        {
                                            s = s.Remove(s.LastIndexOf('-'));

                                            //if (s.Contains("-"))
                                            //    s = s.Substring(s.LastIndexOf('-') + 1);

                                        }
                                        else
                                        {
                                            s = btr.Name;
                                        }

                                        DirectoryInfo d = new DirectoryInfo(Path.GetDirectoryName(ParentFilePath));
                                        FileInfo[] Files = d.GetFiles("*.dwg");
                                        string newpath = "";
                                        foreach (FileInfo file in Files)
                                        {
                                            if (file.Name.Contains(s))
                                            {
                                                newpath = Path.Combine(d.FullName, file.Name);
                                                string TnewPath = @".\" + Path.GetFileName(newpath);
                                                btr.PathName = TnewPath;

                                                foreach (var item in lstobjDownloadedFiles)
                                                {

                                                    if (item.ParentFilePath == ParentFilePath && item.FilePath == newpath)
                                                    {
                                                        item.XrefStatus = true; break;
                                                    }
                                                }


                                                break;
                                            }
                                        }
                                        if (File.Exists(newpath))
                                            CheckXrefStatus(ParentFilePath, newpath, lstobjDownloadedFiles);
                                    }


                                    break;
                                }
                        }//Switch Complete
                    }//For Complete                  
                    mainDb.SaveAs(ParentFilePath, DwgVersion.Current);
                }//Complete MainDB

            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
            }
            return lstOldFiles;
        }

        public System.Data.DataTable GetDrawingExternalRefreces(bool IsSaveAsNew = false)
        {
            System.Data.DataTable dtTreeGrid = new System.Data.DataTable();
            dtTreeGrid = Helper.GetDrawingPropertiesTableStructure();



            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;


            Database db = doc.Database;
            Editor ed = doc.Editor;
            try
            {
                Database mainDb = new Database(false, true);
                String rootid = "";
                using (mainDb)
                {
                    mainDb.ReadDwgFile(doc.Name, FileOpenMode.OpenForReadAndAllShare, true, null);
                    mainDb.ResolveXrefs(false, false);
                    XrefGraph xg = mainDb.GetHostDwgXrefGraph(false);
                    for (int i = 0; i < xg.NumNodes; i++)
                    {
                        XrefGraphNode xgn = xg.GetXrefNode(i);
                        switch (xgn.XrefStatus)
                        {
                            case XrefStatus.Unresolved:
                                ed.WriteMessage("\nUnresolved xref \"{0}\"", xgn.Name);
                                break;
                            case XrefStatus.Unloaded:
                                ed.WriteMessage("\nUnloaded xref \"{0}\"", xgn.Name);
                                break;
                            case XrefStatus.Unreferenced:
                                ed.WriteMessage("\nUnreferenced xref \"{0}\"", xgn.Name);
                                break;
                            case XrefStatus.Resolved:
                                {
                                    Database xdb = xgn.Database;
                                    if (xdb != null)
                                    {
                                        Transaction tr = xdb.TransactionManager.StartTransaction();
                                        String drawingName;
                                        String[] str = new String[14];
                                        using (tr)
                                        {
                                            String Layouts = "";
                                            DBDictionary layoutDict = tr.GetObject(xdb.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
                                            foreach (DBDictionaryEntry de in layoutDict)
                                            {
                                                String layoutName = de.Key;
                                                if (layoutName != "Model")
                                                {
                                                    Layouts += de.Key.ToString() + "$";
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }

                                            if (xgn.Name.Contains("\\"))
                                            {
                                                str = xgn.Name.Split('\\');
                                                drawingName = str[str.Length - 1];
                                                //acadApp.DocumentManager.MdiActiveDocument.CloseAndSave(xgn.Name);
                                                //acadApp.DocumentManager.Open(xgn.Name, false);
                                            }
                                            else
                                            {
                                                drawingName = xgn.Name;
                                            }

                                            if (i == 0)
                                            {

                                                Hashtable rootdrawingAttrs = new Hashtable();
                                                rootdrawingAttrs = (Hashtable)Helper.cadManager.GetDrawingAttributes();
                                                rootid = "";
                                                String childrens = "";
                                                for (int j = 0; j < xgn.NumIn; j++)
                                                {
                                                    int tempInt = IsXRefNodeEqual(xg, xgn.In(j));
                                                    if (tempInt.Equals(-1))
                                                        continue;
                                                    String MyName = xg.GetXrefNode(tempInt).Name;
                                                    String[] str1 = new String[14];
                                                    if (xg.GetXrefNode(tempInt).Name.Contains("\\"))
                                                    {
                                                        str1 = xg.GetXrefNode(tempInt).Name.Split('\\');
                                                        MyName = str1[str1.Length - 1];
                                                    }
                                                    if (j == xgn.NumIn - 1)
                                                        childrens += MyName;
                                                    else
                                                        childrens += MyName + ",";
                                                }
                                                if (rootdrawingAttrs.Count != 0)
                                                {
                                                    try
                                                    {
                                                        rootid = rootdrawingAttrs["drawingid"].ToString();


                                                        dtTreeGrid = Helper.AddRowDrawingPropertiesTable(dtTreeGrid, rootdrawingAttrs);

                                                        int RowIndex = dtTreeGrid.Rows.Count - 1;
                                                        if (RowIndex < 0)
                                                        {
                                                            DataRow dr = dtTreeGrid.NewRow();
                                                            dr["DrawingName"] = drawingName;
                                                            dr["filepath"] = xgn.Database.Filename.ToString();
                                                            dr["sourceid"] = childrens;
                                                            dr["isroot"] = "true";
                                                            dr["Layouts"] = Layouts.ToString();
                                                            dtTreeGrid.Rows.Add(dr);
                                                            RowIndex = dtTreeGrid.Rows.Count - 1;
                                                        }
                                                        dtTreeGrid.Rows[RowIndex]["filepath"] = xgn.Name;
                                                        dtTreeGrid.Rows[RowIndex]["isroot"] = "true";

                                                        if (IsSaveAsNew)
                                                        {
                                                            dtTreeGrid.Rows[RowIndex]["DrawingName"] = Helper.RemovePreFixFromFileName(Path.GetFileName(xgn.Name), Convert.ToString(dtTreeGrid.Rows[RowIndex]["prefix"]));
                                                            dtTreeGrid.Rows[RowIndex]["DrawingId"] = "";
                                                        }

                                                    }
                                                    catch (System.Exception E)
                                                    {

                                                    }
                                                }
                                                else
                                                {

                                                    DataRow dr = dtTreeGrid.NewRow();
                                                    dr["DrawingName"] = drawingName;
                                                    dr["filepath"] = xgn.Database.Filename.ToString();
                                                    dr["sourceid"] = childrens;
                                                    dr["isroot"] = "true";
                                                    dr["Layouts"] = Layouts.ToString();
                                                    dtTreeGrid.Rows.Add(dr);

                                                }
                                            }
                                            else
                                            {
                                                String childrens = "";
                                                for (int j = 0; j < xgn.NumIn; j++)
                                                {
                                                    int tempInt = IsXRefNodeEqual(xg, xgn.In(j));
                                                    if (tempInt.Equals(-1))
                                                        continue;
                                                    String MyName = xg.GetXrefNode(tempInt).Name;
                                                    String[] str1 = new String[14];
                                                    if (xg.GetXrefNode(tempInt).Name.Contains("\\"))
                                                    {
                                                        str1 = xg.GetXrefNode(tempInt).Name.Split('\\');
                                                        MyName = str1[str1.Length - 1];
                                                    }
                                                    if (j == xgn.NumIn - 1)
                                                        childrens += MyName;
                                                    else
                                                        childrens += MyName + ",";
                                                }
                                                Hashtable drawingAttrs = new Hashtable();
                                                IDictionaryEnumerator en = xgn.Database.SummaryInfo.CustomProperties;
                                                while (en.MoveNext())
                                                {
                                                    drawingAttrs.Add(en.Key, en.Value == null ? string.Empty : Convert.ToString(en.Value));
                                                }
                                                if (drawingAttrs.Count != 0)
                                                {
                                                    int RowDiff = dtTreeGrid.Rows.Count;
                                                    dtTreeGrid = Helper.AddRowDrawingPropertiesTable(dtTreeGrid, drawingAttrs);
                                                    RowDiff = dtTreeGrid.Rows.Count - RowDiff;
                                                    int RowIndex = dtTreeGrid.Rows.Count - 1;

                                                    if (RowDiff == 0)
                                                    {
                                                        DataRow dr = dtTreeGrid.NewRow();
                                                        dr["DrawingName"] = drawingName;
                                                        dr["filepath"] = xgn.Database.Filename.ToString();
                                                        dr["sourceid"] = childrens;
                                                        dr["isroot"] = "true";
                                                        dr["Layouts"] = Layouts.ToString();
                                                        dtTreeGrid.Rows.Add(dr);
                                                        RowIndex = dtTreeGrid.Rows.Count - 1;
                                                    }
                                                    dtTreeGrid.Rows[RowIndex]["filepath"] = xgn.Database.Filename;

                                                    if (Convert.ToString(dtTreeGrid.Rows[RowIndex]["isroot"]).ToLower() == "true")
                                                    {
                                                        dtTreeGrid.Rows[RowIndex]["IsNewXref"] = "true";
                                                    }
                                                    dtTreeGrid.Rows[RowIndex]["isroot"] = "false";

                                                    if (IsSaveAsNew)
                                                        dtTreeGrid.Rows[RowIndex]["DrawingName"] = Helper.RemovePreFixFromFileName(Path.GetFileName(xgn.Name), Convert.ToString(dtTreeGrid.Rows[RowIndex]["prefix"]));
                                                }

                                                else
                                                {
                                                    DataRow dr = dtTreeGrid.NewRow();
                                                    dr["DrawingName"] = xgn.Name;
                                                    dr["filepath"] = xgn.Database.Filename.ToString();
                                                    dr["sourceid"] = childrens;
                                                    dr["isroot"] = "false";
                                                    dr["Layouts"] = Layouts.ToString();
                                                    dr["IsNewXref"] = "true";
                                                    dtTreeGrid.Rows.Add(dr);
                                                }

                                            }
                                            tr.Commit();
                                        }
                                    }
                                    break;
                                }
                        }//Switch Complete
                    }//For Complete                    
                }//Complete MainDB
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage("\nProblem reading/processing \"{0}\": {1}", doc.Name, ex.Message);
            }
            return dtTreeGrid;
        }

        public System.Data.DataTable GetDrawingExternalRefreces1(bool IsSaveAsNew = false)
        {
            System.Data.DataTable dtTreeGrid = Helper.GetDrawingPropertiesTableStructure();
            try
            {
                System.Data.DataTable dtFilePathList = new System.Data.DataTable();
                dtFilePathList.Columns.Add("ParentFile", typeof(string));
                dtFilePathList.Columns.Add("ChildFile", typeof(string));
                dtFilePathList.Columns.Add("LevelNo", typeof(int));
                dtFilePathList.AcceptChanges();
                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                int RefCount = 2;
                if(GetDrawingExternalRefreces(doc.Name, "0", ref dtTreeGrid, ref RefCount,ref dtFilePathList, doc.Name, IsSaveAsNew )==null)
                {
                    dtTreeGrid = null;
                }

            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return dtTreeGrid;
        }


        public System.Data.DataTable GetDrawingExternalRefreces(string FilePath, String Parent, ref System.Data.DataTable dtTreeGrid, ref int RefCount, ref System.Data.DataTable dtFilePathList,string ParentFileName, bool IsSaveAsNew = false)
        {

            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;

            if (!File.Exists(FilePath))
            {
                //Helper.LogBugFinding("File Not Exist");
            }
            else
            {
                

                System.Data.DataTable dtTemp = Helper.RowFilter(dtFilePathList.DefaultView.ToTable().Copy(), "ParentFile", FilePath);
                 if(dtTemp.Rows.Count>0)
                {
                    ShowMessage.ErrorMessUD("Circular referance detected in Xref files,\n Please remove circular referance and try uploading again.");
                    return null;
                }
                else
                {
                    DataRow dr = dtFilePathList.NewRow();
                    dr["ParentFile"] = ParentFileName;
                    dr["ChildFile"] = FilePath;

                    dtFilePathList.Rows.Add(dr);
                    dtFilePathList.AcceptChanges();
                }

                Database Db = new Database(false, true);
                //Helper.LogBugFinding("ReadDwgFile"); 
                Db.ReadDwgFile(FilePath, FileOpenMode.OpenForReadAndAllShare, true, null);
                //Helper.LogBugFinding("ReadDwgFile"); 
                Editor ed = doc.Editor;
                try
                {
                    Database mainDb = new Database(false, true);
                    String rootid = "";
                    using (mainDb)
                    {
                        //Helper.LogBugFinding("ReadDwgFile Xref");
                        mainDb.ReadDwgFile(Db.Filename, FileOpenMode.OpenForReadAndAllShare, true, null);
                        //Helper.LogBugFinding("ReadDwgFile Xref");
                        mainDb.ResolveXrefs(false, false);
                        XrefGraph xg = mainDb.GetHostDwgXrefGraph(false);
                        for (int i = 0; i < xg.NumNodes; i++)
                        {

                            XrefGraphNode xgn = xg.GetXrefNode(i);
                            if (xgn.IsNested && i > 0)
                            {
                                continue;
                            }
                            switch (xgn.XrefStatus)
                            {
                                case XrefStatus.Unresolved:
                                    ed.WriteMessage("\nUnresolved xref \"{0}\"", xgn.Name);
                                    break;
                                case XrefStatus.Unloaded:
                                    ed.WriteMessage("\nUnloaded xref \"{0}\"", xgn.Name);
                                    break;
                                case XrefStatus.Unreferenced:
                                    ed.WriteMessage("\nUnreferenced xref \"{0}\"", xgn.Name);
                                    break;
                                case XrefStatus.Resolved:
                                    {
                                        Database xdb = xgn.Database;
                                        if (xdb != null)
                                        {
                                            Transaction tr = xdb.TransactionManager.StartTransaction();
                                            String drawingName;
                                            String[] str = new String[14];
                                            using (tr)
                                            {
                                                String Layouts = "";
                                                DBDictionary layoutDict = tr.GetObject(xdb.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
                                                foreach (DBDictionaryEntry de in layoutDict)
                                                {
                                                    String layoutName = de.Key;
                                                    if (layoutName != "Model")
                                                    {
                                                        Layouts += de.Key.ToString() + "$";
                                                    }
                                                    else
                                                    {
                                                        continue;
                                                    }
                                                }

                                                if (xgn.Name.Contains("\\"))
                                                {
                                                    str = xgn.Name.Split('\\');
                                                    drawingName = str[str.Length - 1];
                                                }
                                                else
                                                {
                                                    drawingName = xgn.Name;
                                                }

                                                if (i == 0)
                                                {
                                                    if (Parent != "0")
                                                    {
                                                        continue;
                                                    }

                                                    Hashtable rootdrawingAttrs = new Hashtable();

                                                    IDictionaryEnumerator en = xgn.Database.SummaryInfo.CustomProperties;
                                                    while (en.MoveNext())
                                                    {
                                                        rootdrawingAttrs.Add(en.Key, en.Value == null ? string.Empty : Convert.ToString(en.Value));
                                                    }
                                                    rootid = "";
                                                    String childrens = "";
                                                    for (int j = 0; j < xgn.NumIn; j++)
                                                    {
                                                        int tempInt = IsXRefNodeEqual(xg, xgn.In(j));
                                                        if (tempInt.Equals(-1))
                                                            continue;
                                                        String MyName = xg.GetXrefNode(tempInt).Name;
                                                        String[] str1 = new String[14];
                                                        if (xg.GetXrefNode(tempInt).Name.Contains("\\"))
                                                        {
                                                            str1 = xg.GetXrefNode(tempInt).Name.Split('\\');
                                                            MyName = str1[str1.Length - 1];
                                                        }
                                                        if (j == xgn.NumIn - 1)
                                                            childrens += MyName;
                                                        else
                                                            childrens += MyName + ",";
                                                    }
                                                    if (rootdrawingAttrs.Count != 0)
                                                    {
                                                        try
                                                        {
                                                            rootid = rootdrawingAttrs["drawingid"].ToString();


                                                            dtTreeGrid = Helper.AddRowDrawingPropertiesTable(dtTreeGrid, rootdrawingAttrs);

                                                            int RowIndex = dtTreeGrid.Rows.Count - 1;
                                                            if (RowIndex < 0)
                                                            {
                                                                DataRow dr = dtTreeGrid.NewRow();
                                                                dr["DrawingName"] = drawingName;
                                                                dr["filepath"] = xgn.Database.Filename.ToString();
                                                                dr["sourceid"] = childrens;
                                                                dr["isroot"] = "true";
                                                                dr["Layouts"] = Layouts.ToString();
                                                                dtTreeGrid.Rows.Add(dr);
                                                                RowIndex = dtTreeGrid.Rows.Count - 1;
                                                            }
                                                            dtTreeGrid.Rows[RowIndex]["filepath"] = xgn.Name;
                                                            dtTreeGrid.Rows[RowIndex]["isroot"] = "true";

                                                            if (IsSaveAsNew)
                                                            {
                                                                dtTreeGrid.Rows[RowIndex]["DrawingName"] = Helper.RemovePreFixFromFileName(Path.GetFileName(xgn.Name), Convert.ToString(dtTreeGrid.Rows[RowIndex]["prefix"]));
                                                                dtTreeGrid.Rows[RowIndex]["DrawingId"] = "";
                                                            }
                                                            if (Convert.ToString(dtTreeGrid.Rows[RowIndex]["DrawingId"]).Trim().Length == 0)
                                                            {
                                                                Parent = "1";
                                                            }
                                                            else
                                                            {
                                                                Parent = Convert.ToString(dtTreeGrid.Rows[RowIndex]["DrawingId"]).Trim();
                                                            }
                                                            dtTreeGrid.Rows[RowIndex]["PK"] = Parent;
                                                            dtTreeGrid.Rows[RowIndex]["FK"] = Parent;
                                                        }
                                                        catch (System.Exception E)
                                                        {

                                                        }
                                                    }
                                                    else
                                                    {
                                                        Parent = "1";
                                                        DataRow dr = dtTreeGrid.NewRow();
                                                        dr["DrawingName"] = drawingName;
                                                        dr["filepath"] = xgn.Database.Filename.ToString();
                                                        dr["sourceid"] = childrens;
                                                        dr["isroot"] = "true";
                                                        dr["Layouts"] = Layouts.ToString();
                                                        dr["PK"] = Parent;
                                                        dr["FK"] = Parent;
                                                        dtTreeGrid.Rows.Add(dr);

                                                    }
                                                }
                                                else
                                                {
                                                    String childrens = "";
                                                    string PK = "";
                                                    try
                                                    {
                                                        PK = Convert.ToString(++RefCount);
                                                    }
                                                    catch
                                                    {
                                                        PK = Convert.ToString(i + 1);
                                                    }
                                                    for (int j = 0; j < xgn.NumIn; j++)
                                                    {
                                                        int tempInt = IsXRefNodeEqual(xg, xgn.In(j));
                                                        if (tempInt.Equals(-1))
                                                            continue;
                                                        String MyName = xg.GetXrefNode(tempInt).Name;
                                                        String[] str1 = new String[14];
                                                        if (xg.GetXrefNode(tempInt).Name.Contains("\\"))
                                                        {
                                                            str1 = xg.GetXrefNode(tempInt).Name.Split('\\');
                                                            MyName = str1[str1.Length - 1];
                                                        }
                                                        if (j == xgn.NumIn - 1)
                                                            childrens += MyName;
                                                        else
                                                            childrens += MyName + ",";
                                                    }
                                                    Hashtable drawingAttrs = new Hashtable();
                                                    IDictionaryEnumerator en = xgn.Database.SummaryInfo.CustomProperties;
                                                    while (en.MoveNext())
                                                    {
                                                        drawingAttrs.Add(en.Key, en.Value == null ? string.Empty : Convert.ToString(en.Value));
                                                    }
                                                    if (drawingAttrs.Count != 0)
                                                    {
                                                        int RowDiff = dtTreeGrid.Rows.Count;
                                                        dtTreeGrid = Helper.AddRowDrawingPropertiesTable(dtTreeGrid, drawingAttrs);
                                                        RowDiff = dtTreeGrid.Rows.Count - RowDiff;
                                                        int RowIndex = dtTreeGrid.Rows.Count - 1;

                                                        if (RowDiff == 0)
                                                        {
                                                            DataRow dr = dtTreeGrid.NewRow();
                                                            dr["DrawingName"] = drawingName;
                                                            dr["filepath"] = xgn.Database.Filename.ToString();
                                                            dr["sourceid"] = childrens;
                                                            dr["isroot"] = "true";
                                                            dr["Layouts"] = Layouts.ToString();
                                                            dtTreeGrid.Rows.Add(dr);
                                                            RowIndex = dtTreeGrid.Rows.Count - 1;
                                                        }
                                                        dtTreeGrid.Rows[RowIndex]["filepath"] = xgn.Database.Filename;

                                                        if (Convert.ToString(dtTreeGrid.Rows[RowIndex]["isroot"]).ToLower() == "true")
                                                        {
                                                            dtTreeGrid.Rows[RowIndex]["IsNewXref"] = "true";
                                                        }
                                                        dtTreeGrid.Rows[RowIndex]["isroot"] = "false";

                                                        if (IsSaveAsNew)
                                                            dtTreeGrid.Rows[RowIndex]["DrawingName"] = Helper.RemovePreFixFromFileName(Path.GetFileName(xgn.Name), Convert.ToString(dtTreeGrid.Rows[RowIndex]["prefix"]));


                                                        if (Convert.ToString(dtTreeGrid.Rows[RowIndex]["DrawingId"]).Trim().Length != 0)
                                                        {
                                                            PK = Convert.ToString(dtTreeGrid.Rows[RowIndex]["DrawingId"]).Trim();
                                                        }
                                                        dtTreeGrid.Rows[RowIndex]["PK"] = PK;
                                                        dtTreeGrid.Rows[RowIndex]["FK"] = Parent;
                                                    }

                                                    else
                                                    {
                                                        DataRow dr = dtTreeGrid.NewRow();
                                                        dr["DrawingName"] = xgn.Name;
                                                        dr["filepath"] = xgn.Database.Filename.ToString();
                                                        dr["sourceid"] = childrens;
                                                        dr["isroot"] = "false";
                                                        dr["Layouts"] = Layouts.ToString();
                                                        dr["IsNewXref"] = "true";
                                                        dr["PK"] = PK;
                                                        dr["FK"] = Parent;
                                                        dtTreeGrid.Rows.Add(dr);
                                                    }

                                                    if (GetDrawingExternalRefreces(xgn.Database.Filename, PK, ref dtTreeGrid, ref RefCount, ref dtFilePathList, FilePath, IsSaveAsNew) == null)
                                                        return null ;
                                                    else
                                                    {
                                                        DataView dv = dtFilePathList.Copy().DefaultView;
                                                        dv.RowFilter="ParentFile <>'"+ xgn.Database.Filename + "'";
                                                        dtFilePathList = dv.ToTable().Copy();
                                                        dtFilePathList.AcceptChanges();
                                                    }

                                                }
                                                tr.Commit();
                                            }
                                        }
                                        break;
                                    }
                            }//Switch Complete
                        }//For Complete                    
                    }//Complete MainDB
                }
                catch (System.Exception ex)
                {
                    ed.WriteMessage("\nProblem reading/processing \"{0}\": {1}", Db.Filename, ex.Message);
                    ShowMessage.ErrorMess(ex.Message);
                }
            }
            return dtTreeGrid;
        }

        public Hashtable GetLayoutInfo(string FilePath)
        {
            string Layouts = "";



            Hashtable LayoutInfo = new Hashtable();
            try
            {
                Database mainDb = new Database(false, true);
                String rootid = "";
                using (mainDb)
                {
                    mainDb.ReadDwgFile(FilePath, FileOpenMode.OpenForReadAndAllShare, true, null);
                    mainDb.ResolveXrefs(false, false);

                    XrefGraph xg = mainDb.GetHostDwgXrefGraph(false);
                    for (int i = 0; i < xg.NumNodes; i++)
                    {
                        XrefGraphNode xgn = xg.GetXrefNode(i);
                        switch (xgn.XrefStatus)
                        {
                            case XrefStatus.Unresolved:
                                //ed.WriteMessage("\nUnresolved xref \"{0}\"", xgn.Name);
                                break;
                            case XrefStatus.Unloaded:
                                //ed.WriteMessage("\nUnloaded xref \"{0}\"", xgn.Name);
                                break;
                            case XrefStatus.Unreferenced:
                                // ed.WriteMessage("\nUnreferenced xref \"{0}\"", xgn.Name);
                                break;
                            case XrefStatus.Resolved:
                                {
                                    Database xdb = xgn.Database;
                                    if (xdb != null)
                                    {
                                        if (xdb.Filename != mainDb.Filename)
                                        {
                                            break;
                                        }


                                        Transaction tr = xdb.TransactionManager.StartTransaction();

                                        String[] str = new String[14];
                                        using (tr)
                                        {

                                            DBDictionary layoutDict = tr.GetObject(xdb.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
                                            foreach (DBDictionaryEntry de in layoutDict)
                                            {
                                                String layoutName = de.Key;
                                                if (layoutName != "Model")
                                                {
                                                    Layouts += de.Key.ToString() + "$";
                                                    Layout layout = tr.GetObject(de.Value, OpenMode.ForRead) as Layout;
                                                    object OwnID = layout.OwnerId;


                                                    string LayoutUnqID = "";

                                                    //if (Convert.ToString(OwnID) != Convert.ToString(de.Key))
                                                    //{
                                                    //    LayoutUnqID = "";
                                                    //}
                                                    //else
                                                    {
                                                        LayoutUnqID = Convert.ToString(de.Value);
                                                    }


                                                    LayoutInfo.Add(de.Key, LayoutUnqID);
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }


                                            tr.Commit();
                                        }
                                    }
                                    break;
                                }
                        }//Switch Complete
                    }//For Complete                    
                }//Complete MainDB
            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMessUD("Error reading layout info from document " + FilePath + Environment.NewLine + ex.Message);
                // ed.WriteMessage("\nProblem reading/processing \"{0}\": {1}", doc.Name, ex.Message);
            }

            try
            {
                Hashtable ht = new Hashtable();
                foreach (DictionaryEntry key in LayoutInfo)
                {
                    ht.Add(key.Key, key.Value);
                }
                LayoutInfo = ht;
            }
            catch { }
            return LayoutInfo;
        }

        public int IsXRefNodeEqual(XrefGraph xrGraph, GraphNode grNode)
        {
            for (int i = 0; i < xrGraph.NumNodes; i++)
            {
                if (grNode == xrGraph.GetXrefNode(i) as GraphNode)
                    return i;
            }
            return -1;
        }

        public void UpdateDrawingLayoutAttribute(Hashtable documentProperties = null)
        {
            try
            {

                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                DocumentLock doclock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument();
                Database db = doc.Database;
                Editor ed = doc.Editor;
                //   Hashtable drawingAttrs = documentProperties; //new Hashtable();
                Hashtable drawingAttrs = new Hashtable();
                IDictionaryEnumerator en = doc.Database.SummaryInfo.CustomProperties;
                while (en.MoveNext())
                {
                    drawingAttrs.Add(en.Key, en.Value == null ? string.Empty : en.Value);
                }

                if (drawingAttrs.Count < 0) return;
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    #region "TraverseForLayout"
                    DBDictionary layoutDict = tr.GetObject(db.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
                    ObjectIdCollection layoutsToPlot = new ObjectIdCollection();
                    foreach (DBDictionaryEntry de in layoutDict)
                    {
                        String layoutName = de.Key;
                        if (layoutName != "Model")
                        {
                            LayoutManager.Current.CurrentLayout = layoutName;
                            Hashtable LayoutData = documentProperties;

                            #region "TraverseForTitleblock"

                            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.PaperSpace], OpenMode.ForRead);
                            layoutsToPlot.Add(btr.LayoutId);

                            foreach (ObjectId objId in btr)
                            {
                                Entity ent = (Entity)tr.GetObject(objId, OpenMode.ForRead);
                                string blkName = ent.BlockName;
                                if (LayoutData.Count < 1) continue;
                                if (ent != null)
                                {
                                    BlockReference br = ent as BlockReference;
                                    if (br != null)
                                    {
                                        BlockTableRecord bd = (BlockTableRecord)tr.GetObject(br.BlockTableRecord, OpenMode.ForRead);
                                        //MessageBox.Show(bd.Name);//Block Name
                                        foreach (ObjectId arId in br.AttributeCollection)
                                        {
                                            DBObject obj = tr.GetObject(arId, OpenMode.ForRead);
                                            AttributeReference ar = obj as AttributeReference;

                                            if (ar.Tag.ToUpper() == "DRAWINGNUMBER")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = LayoutData["drawingnumber"] == null ? string.Empty : Convert.ToString(LayoutData["drawingnumber"]);//drawingAttrs["drawingnumber"].ToString();
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "DRAWINGNAME")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = drawingAttrs["drawingname"] == null ? string.Empty : Convert.ToString(drawingAttrs["drawingname"]);
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "PROJECTNAME")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = LayoutData["projectname"] == null ? string.Empty : Convert.ToString(LayoutData["projectname"]); //drawingAttrs["projectname"].ToString();
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "PROJECTID")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = LayoutData["projectid"] == null ? string.Empty : Convert.ToString(LayoutData["projectid"]); //drawingAttrs["projectid"].ToString();
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "GENERATION")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = drawingAttrs["generation"] == null ? string.Empty : Convert.ToString(drawingAttrs["generation"]); //drawingAttrs["generation"].ToString();
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "REVISION")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = LayoutData["revision"] == null ? string.Empty : Convert.ToString(LayoutData["revision"]); //drawingAttrs["revision"].ToString();
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "DWGSTATE")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = drawingAttrs["drawingstate"] == null ? string.Empty : Convert.ToString(drawingAttrs["drawingstate"]);
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "GOODNOT")
                                            {
                                                ar.UpgradeOpen();
                                                if (((Convert.ToString(LayoutData["drawingstate"]) == "GFC")
                                                    || (Convert.ToString(LayoutData["drawingstate"]) == "Released")) &&
                                                    ((Convert.ToString(drawingAttrs["drawingstate"]) == "GFC") ||
                                                    (Convert.ToString(drawingAttrs["drawingstate"]) == "Coordinated") ||
                                                    (Convert.ToString(drawingAttrs["drawingstate"]) == "Const-Dwg")))
                                                {
                                                    ar.TextString = "GOOD";
                                                }
                                                else
                                                {
                                                    ar.TextString = "NOT";
                                                }
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "LAYOUTNAME")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = layoutName;
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "CREATEDBY")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = LayoutData["createdby"] == null ? string.Empty : Convert.ToString(LayoutData["createdby"]); //drawingAttrs["createdby"].ToString();
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "MODIFIEDBY")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = LayoutData["modifiedby"] == null ? string.Empty : Convert.ToString(LayoutData["modifiedby"]); //drawingAttrs["modifiedby"].ToString();
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "CREATEDON")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = LayoutData["createdon"] == null ? string.Empty : Convert.ToString(LayoutData["createdon"]).Substring(0, 10); //drawingAttrs["createdon"].ToString().Substring(0, 10); 
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "MODIFIEDON")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = LayoutData["modifiedon"] == null ? string.Empty : Convert.ToString(LayoutData["modifiedon"]).Substring(0, 10); //drawingAttrs["modifiedon"].ToString().Substring(0,10);
                                                ar.DowngradeOpen();
                                            }
                                            if (ar.Tag.ToUpper() == "DRAWINGSTATE")
                                            {
                                                ar.UpgradeOpen();
                                                ar.TextString = LayoutData["drawingstate"] == null ? string.Empty : Convert.ToString(LayoutData["drawingstate"]); //drawingAttrs["drawingstate"].ToString();
                                                ar.DowngradeOpen();
                                            }
                                        }
                                    }
                                }
                            } //end foreach Block
                            #endregion "TraverseForTitleblock"                  
                        }
                        else
                        {
                            #region "if Model is there"
                            /*
                            BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                            BlockTableRecord btr = (BlockTableRecord)tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
                            PlotInfo pi = new PlotInfo();
                            PlotInfoValidator piv = new PlotInfoValidator();
                            piv.MediaMatchingPolicy = MatchingPolicy.MatchEnabled;

                            if (PlotFactory.ProcessPlotState == ProcessPlotState.NotPlotting)
                            {
                                PlotEngine pe = PlotFactory.CreatePublishEngine();
                                using (pe)
                                {
                                    PlotProgressDialog ppd = new PlotProgressDialog(false, 1, true);
                                    using (ppd)
                                    {  
                                        Layout lo = (Layout)tr.GetObject(btr.LayoutId, OpenMode.ForRead);
                                        PlotSettings ps = new PlotSettings(lo.ModelType);
                                        ps.CopyFrom(lo);
                                        PlotSettingsValidator psv = PlotSettingsValidator.Current;
                                        psv.SetPlotType(ps, Autodesk.AutoCAD.DatabaseServices.PlotType.Extents);
                                        psv.SetUseStandardScale(ps, true);
                                        psv.SetStdScaleType(ps, StdScaleType.ScaleToFit);
                                        psv.SetPlotCentered(ps, true);                                        
                                        psv.SetPlotConfigurationName(ps, "PublishToWeb JPG.pc3", "Sun_Hi-Res_(1600.00_x_1280.00_Pixels)");//Plot to jpeg
                                        pi.Layout = btr.LayoutId;
                                        LayoutManager.Current.CurrentLayout = lo.LayoutName;
                                        pi.OverrideSettings = ps;
                                        piv.Validate(pi);
                                        ppd.set_PlotMsgString(PlotMessageIndex.DialogTitle, "Custom Plot Progress");
                                        ppd.set_PlotMsgString(PlotMessageIndex.CancelJobButtonMessage, "Cancel Job");
                                        ppd.set_PlotMsgString(PlotMessageIndex.CancelSheetButtonMessage, "Cancel Sheet");
                                        ppd.set_PlotMsgString(PlotMessageIndex.SheetSetProgressCaption, "Sheet Set Progress");
                                        ppd.set_PlotMsgString(PlotMessageIndex.SheetProgressCaption, "Sheet Progress");
                                        ppd.LowerPlotProgressRange = 0;
                                        ppd.UpperPlotProgressRange = 100;
                                        ppd.PlotProgressPos = 0;
                                        ppd.OnBeginPlot();
                                        ppd.IsVisible = false;
                                        pe.BeginPlot(ppd, null);
                                        pe.BeginDocument(pi, doc.Name, null, 1, true, "C:\\Test\\" + lo.LayoutName + ".jpg");////Plot to jpeg  
                                        ppd.OnBeginSheet();
                                        ppd.LowerSheetProgressRange = 0;
                                        ppd.UpperSheetProgressRange = 100;
                                        ppd.SheetProgressPos = 0;
                                        PlotPageInfo ppi = new PlotPageInfo();
                                        pe.BeginPage(ppi, pi, true, null);
                                        pe.BeginGenerateGraphics(null);
                                        ppd.SheetProgressPos = 50;
                                        pe.EndGenerateGraphics(null);
                                        pe.EndPage(null);
                                        ppd.SheetProgressPos = 100;
                                        ppd.OnEndSheet();                                        
                                        ppd.PlotProgressPos += (100 / layoutsToPlot.Count);
                                        pe.EndDocument(null);
                                        ppd.PlotProgressPos = 100;
                                        ppd.OnEndPlot();
                                        pe.EndPlot(null);
                                    }
                                }
                            }

                            else
                            {
                                MessageBox.Show("\nAnother plot is in progress.");
                            }
                            */
                            #endregion "if Model is there"
                        }
                    }
                    #endregion "TraverseForLayout

                    #region "PlotLayouts"
                    //PlotLayout(doc, tr, layoutsToPlot);
                    #endregion "PlotLayouts"

                    tr.Commit();
                    tr.Dispose();
                }//end Transaction tr
                doclock.Dispose();
            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message.ToString(), "Error");
            }
        }

        public bool CheckForCurruntlyOpenDoc(string FilePath, bool MakeActive = true)
        {
            try
            {
                foreach (Document Doc in ACAPP.DocumentManager)
                {
                    if (Doc.Database.Filename.ToLower() == FilePath.ToLower())
                    {
                        if (MakeActive)
                        {
                            ACAPP.DocumentManager.MdiActiveDocument = Doc;
                        }

                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return true;
            }

        }

        public bool ChecknCloseOpenedDoc(string FilePath)
        {
            try
            {
                foreach (Document Doc in ACAPP.DocumentManager)
                {
                    if (Doc.Database.Filename == FilePath)
                    {

                        Doc.CloseAndSave(FilePath);
                        return true;
                    }
                }
                return false;
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return true;
            }
        }

        public void AttachingExternalReference(string ParentFilePath, List<clsDownloadedFiles> lstobjDownloadedFiles)
        {
            try
            {
                foreach (clsDownloadedFiles item in lstobjDownloadedFiles)
                {
                    if (Convert.ToBoolean(item.XrefStatus) == false && !(item.MainFilePath == item.ParentFilePath && item.ParentFilePath == item.FilePath))
                    {
                        if (item.ParentFilePath != null && item.FilePath != null && item.ParentFilePath != string.Empty && item.FilePath != string.Empty)
                            AttachingExternalReference(item.ParentFilePath, item.FilePath, item.Prefix);
                    }
                }
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        public void AttachingExternalReference(string ParentFilePath, string XrefFilePath, string Prefix)
        {
            // Get the current database and start a transaction
            //Database acCurDb;
            Database acCurDb = new Database(false, true);
            using (acCurDb)
            {
                acCurDb.ReadDwgFile(ParentFilePath, FileOpenMode.OpenForReadAndAllShare, true, null);
                //acCurDb = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
                // to set model as a current space otherwise xref may add to any of layout. behaviour unexpected and uncontroled otherwise.
                Transaction tr = acCurDb.TransactionManager.StartTransaction();
                using (tr)
                {
                    DBDictionary layoutDict = tr.GetObject(acCurDb.LayoutDictionaryId, OpenMode.ForWrite) as DBDictionary;
                    foreach (DBDictionaryEntry de in layoutDict)
                    {
                        String layoutName = de.Key;
                        if (layoutName == "Model")
                        {
                            break;
                        }

                    }
                    tr.Commit();
                }
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // Create a reference to a DWG file
                    string PathName = XrefFilePath;
                    ObjectId acXrefId = acCurDb.AttachXref(PathName, Helper.RemovePreFixFromFileName(Path.GetFileNameWithoutExtension(XrefFilePath), Prefix));

                    // If a valid reference is created then continue
                    if (!acXrefId.IsNull)
                    {
                        // Attach the DWG reference to the current space
                        Point3d insPt = new Point3d(1, 1, 0);
                        using (BlockReference acBlkRef = new BlockReference(insPt, acXrefId))
                        {
                            BlockTableRecord acBlkTblRec;
                            acBlkTblRec = acTrans.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                            acBlkTblRec.AppendEntity(acBlkRef);
                            acTrans.AddNewlyCreatedDBObject(acBlkRef, true);
                        }
                    }

                    // Save the new objects to the database
                    acTrans.Commit();

                    // Dispose of the transaction
                }
                acCurDb.SaveAs(ParentFilePath, DwgVersion.Current);
            }
        }

        public void renamelayoutName(String FilePath, string OldLayoutName, string NewLayoutName)
        {
            try
            {
                Database Db = new Database(false, true);
                Db.ReadDwgFile(FilePath, FileOpenMode.OpenForReadAndAllShare, true, null);
                //get the Layout name

                if (Db != null)
                {
                    using (Db)
                    {
                        Transaction tr = Db.TransactionManager.StartTransaction();
                        using (tr)
                        {
                            DBDictionary layoutDict = tr.GetObject(Db.LayoutDictionaryId, OpenMode.ForWrite) as DBDictionary;
                            int count = 0;
                            foreach (DBDictionaryEntry de in layoutDict)
                            {
                                String layoutName = de.Key;
                                if (layoutName != "Model")
                                {

                                    Layout layout = tr.GetObject(de.Value, OpenMode.ForWrite) as Layout;

                                    if (layout.LayoutName == OldLayoutName)
                                    {
                                        layout.LayoutName = NewLayoutName;
                                        count = 1;
                                        break;
                                    }
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            if (count == 0)
                            {

                            }
                            tr.Commit();
                        }
                        Db.SaveAs(FilePath, DwgVersion.Current);
                    }

                }
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        public void AddingAttributeToABlock(String FilePath, List<string> Attributes, string LayoutName, bool IsLayout)
        {
            // Get the current database and start a transaction


            try
            {

                ObjectId blkRecId = new ObjectId();
                //Database acCurDb = new Database(false, true);
                //acCurDb.ReadDwgFile(FilePath, FileOpenMode.OpenForReadAndAllShare, true, null);
                //Document doc = acDocMgr.GetDocument(acCurDb);
                Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database acCurDb = doc.Database;
                if (acCurDb != null)
                {

                    using (acCurDb)
                    {
                        using (doc.LockDocument())
                        {
                            if (LayoutName != null)
                            {
                                if (IsLayout)
                                {
                                    LayoutManager.Current.CurrentLayout = LayoutName;
                                }
                                else
                                {
                                    LayoutManager.Current.CurrentLayout = "Model";
                                }
                            }

                            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                            {
                                // Open the Block table for read
                                BlockTable acBlkTbl;
                                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                                string BlockName = "TitleBlockWithAttributes_" + LayoutName;
                                if (!acBlkTbl.Has(BlockName))
                                {
                                    BlockTableRecord acBlkTblRec = new BlockTableRecord();
                                    acBlkTblRec.Name = BlockName;



                                    // Set the insertion point for the block
                                    acBlkTblRec.Origin = new Point3d(0, 0, 0);


                                    foreach (string Att in Attributes)
                                    {


                                        // Add an attribute definition to the block
                                        AttributeDefinition acAttDef = new AttributeDefinition();
                                        //using (AttributeDefinition acAttDef = new AttributeDefinition())
                                        //{

                                        acAttDef.Visible = Helper.TitleBlockVisibilityFlag;
                                        acAttDef.Position = new Point3d(0, 0, 0);
                                        acAttDef.Verifiable = true;
                                        acAttDef.Prompt = "ENTER VALUE";
                                        acAttDef.Tag = Att;
                                        acAttDef.TextString = Att + "_VALUE";
                                        acAttDef.Height = 1;
                                        acAttDef.Justify = AttachmentPoint.MiddleCenter;

                                        acBlkTblRec.AppendEntity(acAttDef);
                                        acBlkTbl.UpgradeOpen();


                                        //}

                                    }
                                    acBlkTbl.Add(acBlkTblRec);
                                    acTrans.AddNewlyCreatedDBObject(acBlkTblRec, true);

                                    blkRecId = acBlkTblRec.Id;
                                    //}

                                }
                                else
                                {
                                    blkRecId = acBlkTbl[BlockName];
                                }
                                //acTrans.Commit();
                                //acTrans.Dispose();

                                if (blkRecId != ObjectId.Null)
                                {
                                    //using (Transaction acTrans1 = acCurDb.TransactionManager.StartTransaction())
                                    {
                                        BlockTableRecord acBlkTblRec;
                                        acBlkTblRec = acTrans.GetObject(blkRecId, OpenMode.ForRead) as BlockTableRecord;

                                        string BlockRefName = BlockName + "_Ref";
                                        // Create and insert the new block reference
                                        using (BlockReference acBlkRef = new BlockReference(new Point3d(2, 2, 0), blkRecId))
                                        {
                                            BlockTableRecord acCurSpaceBlkTblRec;
                                            acCurSpaceBlkTblRec = acTrans.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                                            //acBlkRef.Name = BlockRefName;
                                            acCurSpaceBlkTblRec.AppendEntity(acBlkRef);
                                            acTrans.AddNewlyCreatedDBObject(acBlkRef, true);

                                            // Verify block table record has attribute definitions associated with it
                                            if (acBlkTblRec.HasAttributeDefinitions)
                                            {
                                                // Add attributes from the block table record
                                                foreach (ObjectId objID in acBlkTblRec)
                                                {
                                                    DBObject dbObj = acTrans.GetObject(objID, OpenMode.ForRead) as DBObject;

                                                    if (dbObj is AttributeDefinition)
                                                    {
                                                        AttributeDefinition acAtt = dbObj as AttributeDefinition;

                                                        if (!acAtt.Constant)
                                                        {
                                                            using (AttributeReference acAttRef = new AttributeReference())
                                                            {
                                                                acAttRef.Visible = Helper.TitleBlockVisibilityFlag;
                                                                acAttRef.SetAttributeFromBlock(acAtt, acBlkRef.BlockTransform);
                                                                acAttRef.Position = acAtt.Position.TransformBy(acBlkRef.BlockTransform);
                                                                acAttRef.Tag = acAtt.Tag;
                                                                acAttRef.TextString = acAtt.TextString;

                                                                acBlkRef.AttributeCollection.AppendAttribute(acAttRef);

                                                                acTrans.AddNewlyCreatedDBObject(acAttRef, true);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //acTrans1.Commit();
                                    }
                                }
                                acTrans.Commit();
                            }



                            // Save the new object to the database




                            // Dispose of the transaction


                        }

                        //  acCurDb.SaveAs(FilePath, DwgVersion.Current);
                    }
                }

            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        public bool OpenDocSilently(string FilePath)
        {
            bool IsDocOpend = false;
            try
            {
                if (!CheckForCurruntlyOpenDoc(FilePath))
                {
                    Helper.CheckFileInfoFlag = false;

                    ACAPP.DocumentManager.Open(FilePath, false);
                    IsDocOpend = true;
                    Helper.CheckFileInfoFlag = true;
                }
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
            return IsDocOpend;
        }

        public void CloseDocSilently(string FilePath)
        {
            try
            {
                if (CheckForCurruntlyOpenDoc(FilePath, false))
                    ACAPP.DocumentManager.MdiActiveDocument.CloseAndSave(FilePath);
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        public void AddingAttributeToABlock1(String FilePath, List<string> Attributes, string LayoutName)
        {
            // Get the current database and start a transaction


            try
            {
                //DocumentCollection acDocMgr = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager;
                //acDocMgr.Add(FilePath);
                ObjectId blkRecId = new ObjectId();
                //Database acCurDb = new Database(false, true);
                //acCurDb.ReadDwgFile(FilePath, FileOpenMode.OpenForReadAndAllShare, true, null);
                //Document doc = acDocMgr.GetDocument(acCurDb);
                Autodesk.AutoCAD.ApplicationServices.Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database acCurDb = doc.Database;
                if (acCurDb != null)
                {

                    using (acCurDb)
                    {
                        using (doc.LockDocument())
                        {


                            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                            {
                                // Open the Block table for read
                                BlockTable acBlkTbl;
                                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                                string BlockName = "TitleBlockWithAttributes_" + LayoutName;
                                if (!acBlkTbl.Has(BlockName))
                                {
                                    BlockTableRecord acBlkTblRec = new BlockTableRecord();
                                    //using (acBlkTblRec)
                                    //{
                                    acBlkTblRec.Name = BlockName;

                                    // Set the insertion point for the block
                                    acBlkTblRec.Origin = new Point3d(0, 0, 0);


                                    foreach (string Att in Attributes)
                                    {


                                        // Add an attribute definition to the block
                                        AttributeDefinition acAttDef = new AttributeDefinition();
                                        //using (AttributeDefinition acAttDef = new AttributeDefinition())
                                        //{


                                        acAttDef.Position = new Point3d(0, 0, 0);
                                        acAttDef.Verifiable = true;
                                        acAttDef.Prompt = "ENTER VALUE";
                                        acAttDef.Tag = Att;
                                        acAttDef.TextString = Att + "_DXX";
                                        acAttDef.Height = 1;
                                        acAttDef.Justify = AttachmentPoint.MiddleCenter;

                                        acBlkTblRec.AppendEntity(acAttDef);
                                        acBlkTbl.UpgradeOpen();


                                        //}

                                    }
                                    acBlkTbl.Add(acBlkTblRec);
                                    acTrans.AddNewlyCreatedDBObject(acBlkTblRec, true);

                                    blkRecId = acBlkTblRec.Id;
                                    //}

                                }
                                else
                                {
                                    blkRecId = acBlkTbl[BlockName];
                                }
                                //acTrans.Commit();
                                //acTrans.Dispose();

                                if (blkRecId != ObjectId.Null)
                                {
                                    //using (Transaction acTrans1 = acCurDb.TransactionManager.StartTransaction())
                                    {
                                        BlockTableRecord acBlkTblRec;
                                        acBlkTblRec = acTrans.GetObject(blkRecId, OpenMode.ForRead) as BlockTableRecord;

                                        // Create and insert the new block reference
                                        using (BlockReference acBlkRef = new BlockReference(new Point3d(2, 2, 0), blkRecId))
                                        {
                                            BlockTableRecord acCurSpaceBlkTblRec;
                                            acCurSpaceBlkTblRec = acTrans.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                                            acCurSpaceBlkTblRec.AppendEntity(acBlkRef);
                                            acTrans.AddNewlyCreatedDBObject(acBlkRef, true);

                                            // Verify block table record has attribute definitions associated with it
                                            if (acBlkTblRec.HasAttributeDefinitions)
                                            {
                                                // Add attributes from the block table record
                                                foreach (ObjectId objID in acBlkTblRec)
                                                {
                                                    DBObject dbObj = acTrans.GetObject(objID, OpenMode.ForRead) as DBObject;

                                                    if (dbObj is AttributeDefinition)
                                                    {
                                                        AttributeDefinition acAtt = dbObj as AttributeDefinition;

                                                        if (!acAtt.Constant)
                                                        {
                                                            using (AttributeReference acAttRef = new AttributeReference())
                                                            {
                                                                acAttRef.SetAttributeFromBlock(acAtt, acBlkRef.BlockTransform);
                                                                acAttRef.Position = acAtt.Position.TransformBy(acBlkRef.BlockTransform);
                                                                acAttRef.Tag = acAtt.Tag;
                                                                acAttRef.TextString = acAtt.TextString;

                                                                acBlkRef.AttributeCollection.AppendAttribute(acAttRef);

                                                                acTrans.AddNewlyCreatedDBObject(acAttRef, true);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        //acTrans1.Commit();
                                    }
                                }
                                acTrans.Commit();
                            }



                            // Save the new object to the database




                            // Dispose of the transaction


                        }

                        //  acCurDb.SaveAs(FilePath, DwgVersion.Current);
                    }
                }
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        public void InsertingBlockWithAnAttribute()
        {
            // Get the current database and start a transaction
            Database acCurDb;
            acCurDb = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            using (doc.LockDocument())

            {
                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // Open the Block table for read
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                    ObjectId blkRecId = ObjectId.Null;


                    if (!acBlkTbl.Has("CircleBlockWithAttributes"))
                    {
                        using (BlockTableRecord acBlkTblRec = new BlockTableRecord())
                        {
                            acBlkTblRec.Name = "CircleBlockWithAttributes";

                            // Set the insertion point for the block
                            acBlkTblRec.Origin = new Point3d(0, 0, 0);

                            // Add a circle to the block
                            using (Circle acCirc = new Circle())
                            {
                                acCirc.Center = new Point3d(0, 0, 0);
                                acCirc.Radius = 2;

                                acBlkTblRec.AppendEntity(acCirc);

                                // Add an attribute definition to the block
                                using (AttributeDefinition acAttDef = new AttributeDefinition())
                                {
                                    acAttDef.Position = new Point3d(0, 0, 0);
                                    acAttDef.Prompt = "Door #: ";
                                    acAttDef.Tag = "Door#";
                                    acAttDef.TextString = "DXX";
                                    acAttDef.Height = 1;
                                    acAttDef.Justify = AttachmentPoint.MiddleCenter;
                                    acBlkTblRec.AppendEntity(acAttDef);

                                    acBlkTbl.UpgradeOpen();
                                    acBlkTbl.Add(acBlkTblRec);
                                    acTrans.AddNewlyCreatedDBObject(acBlkTblRec, true);
                                }
                            }

                            blkRecId = acBlkTblRec.Id;
                        }
                    }
                    else
                    {
                        blkRecId = acBlkTbl["CircleBlockWithAttributes"];
                    }

                    // Insert the block into the current space
                    if (blkRecId != ObjectId.Null)
                    {
                        BlockTableRecord acBlkTblRec;
                        acBlkTblRec = acTrans.GetObject(blkRecId, OpenMode.ForRead) as BlockTableRecord;

                        // Create and insert the new block reference
                        using (BlockReference acBlkRef = new BlockReference(new Point3d(2, 2, 0), blkRecId))
                        {
                            BlockTableRecord acCurSpaceBlkTblRec;
                            acCurSpaceBlkTblRec = acTrans.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForWrite) as BlockTableRecord;

                            acCurSpaceBlkTblRec.AppendEntity(acBlkRef);
                            acTrans.AddNewlyCreatedDBObject(acBlkRef, true);

                            // Verify block table record has attribute definitions associated with it
                            if (acBlkTblRec.HasAttributeDefinitions)
                            {
                                // Add attributes from the block table record
                                foreach (ObjectId objID in acBlkTblRec)
                                {
                                    DBObject dbObj = acTrans.GetObject(objID, OpenMode.ForRead) as DBObject;

                                    if (dbObj is AttributeDefinition)
                                    {
                                        AttributeDefinition acAtt = dbObj as AttributeDefinition;

                                        if (!acAtt.Constant)
                                        {
                                            using (AttributeReference acAttRef = new AttributeReference())
                                            {
                                                acAttRef.SetAttributeFromBlock(acAtt, acBlkRef.BlockTransform);
                                                acAttRef.Position = acAtt.Position.TransformBy(acBlkRef.BlockTransform);

                                                acAttRef.TextString = acAtt.TextString;

                                                acBlkRef.AttributeCollection.AppendAttribute(acAttRef);

                                                acTrans.AddNewlyCreatedDBObject(acAttRef, true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Save the new object to the database
                    acTrans.Commit();
                }
                // Dispose of the transaction
            }
        }

        static public void InsertBlockReference(string blockName, Point3d insertionPoint)
        {
            Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            using (DocumentLock docLock = doc.LockDocument())
            {
                using (Transaction myT = db.TransactionManager.StartTransaction())
                {
                    //Get the block definition "Check".

                    BlockTable bt =
                        db.BlockTableId.GetObject(OpenMode.ForRead) as BlockTable;
                    BlockTableRecord blockDef =
                      bt[blockName].GetObject(OpenMode.ForRead) as BlockTableRecord;
                    //Also open modelspace - we'll be adding our BlockReference to it
                    BlockTableRecord ms =
                      bt[BlockTableRecord.ModelSpace].GetObject(OpenMode.ForWrite)
                                                              as BlockTableRecord;
                    //Create new BlockReference, and link it to our block definition

                    using (BlockReference blockRef =
                            new BlockReference(insertionPoint, blockDef.ObjectId))
                    {
                        //Add the block reference to modelspace
                        ms.AppendEntity(blockRef);
                        myT.AddNewlyCreatedDBObject(blockRef, true);
                        //Iterate block definition to find all non-constant
                        // AttributeDefinitions
                        foreach (ObjectId id in blockDef)
                        {
                            DBObject obj = id.GetObject(OpenMode.ForRead);
                            AttributeDefinition attDef = obj as AttributeDefinition;
                            if ((attDef != null) && (!attDef.Constant))
                            {
                                //This is a non-constant AttributeDefinition
                                //Create a new AttributeReference
                                using (AttributeReference attRef = new AttributeReference())
                                {
                                    attRef.SetAttributeFromBlock(attDef, blockRef.BlockTransform);
                                    // not setting the attribute value to anything.

                                    //Add the AttributeReference to the BlockReference
                                    blockRef.AttributeCollection.AppendAttribute(attRef);
                                    myT.AddNewlyCreatedDBObject(attRef, true);
                                }
                            }
                        }
                    }
                    //Our work here is done
                    myT.Commit();
                }
            }
        }

        public bool SaveAllOpenDoc()
        {
            try
            {
                var doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Editor ed = doc.Editor;


                foreach (Document Doc in ACAPP.DocumentManager)
                {
                    if (File.Exists(Doc.Database.Filename))
                    {

                        doc.Database.SaveAs(Doc.Database.Filename, DwgVersion.Current);
                    }
                }
                return false;
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
                return true;
            }

        }
    }
}


