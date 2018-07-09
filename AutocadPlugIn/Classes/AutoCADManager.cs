using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Collections.Specialized;
using Autodesk.AutoCAD.EditorInput;
//using Autodesk.Windows;
//using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
//using Autodesk.AutoCAD.PlottingServices;
//using Autodesk.AutoCAD.Publishing;
//using Autodesk.AutoCAD.Geometry;
using acadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using System.ComponentModel;
using System.Data;
using Autodesk.AutoCAD.Runtime;

namespace AutocadPlugIn
{
    public class AutoCADManager : ICADManager
    {
        Hashtable currentDocumentProperties;
        public void SaveActiveDrawing()
        {
            SaveActiveDrawing(true);
        }

        public void SaveActiveDrawing(bool isOpenInReadOnly = true)
        {
            try
            {
                //String filePath = acadApp.DocumentManager.MdiActiveDocument.Database.Filename;
                //acadApp.DocumentManager.MdiActiveDocument.CloseAndSave(filePath);

                //acadApp.DocumentManager.Open(filePath, isOpenInReadOnly);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                // throw (new CADController.Exception.CADManagerException("CADController Exception : " + ex.Message.ToString()));
            }
        }

        public void SetAttributes(Hashtable hashTable)
        {
            try
            {
                currentDocumentProperties = hashTable;
                Document Doc = acadApp.DocumentManager.MdiActiveDocument;
                Database Db = Doc.Database;
                DocumentLock dl = Doc.LockDocument(DocumentLockMode.Write, null, null, true);
                DatabaseSummaryInfoBuilder DbSib = new DatabaseSummaryInfoBuilder();

                using (dl)
                {
                    Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = Db.TransactionManager;
                    using (Transaction aTran = tm.StartTransaction())
                    {
                        foreach (DictionaryEntry entry in currentDocumentProperties)
                        {
                            DbSib.CustomProperties.Add(entry.Key.ToString(), entry.Value == null ? string.Empty : Convert.ToString(entry.Value));
                            // DbSib.CustomPropertyTable.Add(entry.Key.ToString(), entry.Value == null ? string.Empty : Convert.ToString( entry.Value));


                        }
                        Db.SummaryInfo = DbSib.ToDatabaseSummaryInfo();
                        aTran.Commit();
                        if (Doc.IsReadOnly)
                            Doc.UpgradeDocOpen();
                    }
                }
                Db.SaveAs(Db.Filename, DwgVersion.Current);


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
                                ShowMessage.ErrorMess("Unresolved xref :" + xgn.Name);
                                break;
                            case XrefStatus.Unloaded:
                                //ed.WriteMessage("\nUnresolved xref \"{0}\"", xgn.Name);
                                ShowMessage.ErrorMess("Unresolved xref :" + xgn.Name);
                                break;
                            case XrefStatus.Unreferenced:
                                //ed.WriteMessage("\nUnresolved xref \"{0}\"", xgn.Name);
                                ShowMessage.ErrorMess("Unresolved xref :" + xgn.Name);
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
                                                SetAttributesXrefFiles(ht, xgn.Database.Filename);
                                                UpdateLayoutAttributeArefFile(ht, xgn.Database.Filename);
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
        public void UpdateLayoutAttributeArefFile(Hashtable documentProperties, string FilePath)
        {
            try
            {


                Database db = new Database(false, true); ;
                db.ReadDwgFile(FilePath, FileOpenMode.OpenForReadAndAllShare, true, null);

                Hashtable drawingAttrs = new Hashtable();
                IDictionaryEnumerator en = db.SummaryInfo.CustomProperties;
                try
                {


                    while (en.MoveNext())
                    {
                        // if(documentProperties==null)
                        {
                            drawingAttrs.Add(en.Key, en.Value == null ? string.Empty : en.Value);
                        }
                        // else
                        {

                        }

                    }
                }
                catch { }
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
                            //LayoutManager.Current.CurrentLayout = layoutName;
                            Hashtable LayoutData = documentProperties; //new Hashtable();
                                                                       // ArasConnector.ArasConnector LayoutDetail = new ArasConnector.ArasConnector();
                                                                       // LayoutData = LayoutDetail.GetLayoutDetail(drawingAttrs["drawingid"].ToString(), layoutName);

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
                    //db.Save();
                    tr.Commit();
                    tr.Dispose();
                    db.SaveAs(FilePath, DwgVersion.Current);
                }//end Transaction tr

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
        public void SetAttributesXrefFiles(Hashtable hashTable, string FilePath)
        {
            try
            {
                currentDocumentProperties = hashTable;

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
                            foreach (DictionaryEntry entry in currentDocumentProperties)
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
        public Hashtable GetAttributes()
        {
            Hashtable hastable = new Hashtable();
            Document Doc = acadApp.DocumentManager.MdiActiveDocument;
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

        public void UpdateAttributes(Hashtable hashTable)
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

            Document Doc = acadApp.DocumentManager.MdiActiveDocument;

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

        //public void UpdateFileProperties(  string FileType = null, string FileStatus = null, string FileTypeID = null, string FileStatusID = null)
        public void UpdateFileProperties(string FileID, string FilePath)
        {
            try
            {
                RBConnector objRBC = new RBConnector();
                ResultSearchCriteria Drawing = objRBC.GetDrawingInformation(FileID);
                Hashtable htFileProperties = GetAttributes();
                System.Data.DataTable dtFileProperties = Helper.HashTable2Table(htFileProperties);




                System.Data.DataTable dtDrawingProperty = Helper.FillDrawingPropertiesTable(Drawing, FilePath, "1", Convert.ToString(dtFileProperties.Rows[0]["prefix"]));




                SetAttributes(Helper.Table2HashTable(dtDrawingProperty, 0));

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
                Hashtable htFileProperties = GetAttributes();
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
                    SetAttributes(Helper.Table2HashTable(dtFileProperties, 0));
                }
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }

        [CommandMethod("OpenActiveDocument", CommandFlags.Session)]
        public void OpenActiveDocument(String fileName, String openMode, Hashtable properties = null)
        {
            currentDocumentProperties = properties;
            try
            {
                if (openMode == "Checkout")
                {
                    acadApp.DocumentManager.Open(fileName, false);
                    this.SetAttributes(currentDocumentProperties);
                    UpdateLayoutAttribute(currentDocumentProperties);
                    acadApp.DocumentManager.MdiActiveDocument.CloseAndSave(fileName);
                }
                else if (openMode == "View")
                {
                    SetAttributesXrefFiles(currentDocumentProperties, fileName);
                    UpdateLayoutAttributeArefFile(currentDocumentProperties, fileName);
                    acadApp.DocumentManager.Open(fileName, false);
                    //this.SetAttributes(currentDocumentProperties);
                    //UpdateLayoutAttribute(currentDocumentProperties);
                }
                else if (openMode == "Edit")
                {
                    acadApp.DocumentManager.Open(fileName, false);
                    this.SetAttributes(currentDocumentProperties);
                    UpdateLayoutAttribute(currentDocumentProperties);
                }
                else if (openMode == "updtMainDrawing")
                {
                    this.SetAttributes(currentDocumentProperties);
                    UpdateLayoutAttribute(currentDocumentProperties);
                    acadApp.DocumentManager.MdiActiveDocument.CloseAndSave(fileName);
                    acadApp.DocumentManager.Open(fileName, false);
                }
                else if (openMode == "updtXRDrawing")
                {
                    acadApp.DocumentManager.Open(fileName, false);
                    this.SetAttributes(currentDocumentProperties);
                    UpdateLayoutAttribute(currentDocumentProperties);
                    acadApp.DocumentManager.MdiActiveDocument.CloseAndSave(fileName);
                }
            }
            catch (System.Exception ex)
            {
                //throw (new System.Exception("AutoCAD getting problem: " + ex.Message));
                ShowMessage.ErrorMess(ex.Message);
            }
        }

        public void CloseActiveDocument(String fileName)
        {
            //acadApp.DocumentManager.CloseAll();// MdiActiveDocument.CloseAndSave(fileName);
            //this.SetAttributes(properties);     
            acadApp.DocumentManager.MdiActiveDocument.CloseAndSave(fileName);
        }

        public void DeleteActiveDocument(String fileName)
        {
            FileInfo file = new FileInfo(fileName);
            if (file.Exists)
            {
                file.Delete();
            }
        }

        public void LockActiveDocument()
        {
            try
            {
                Document doc = acadApp.DocumentManager.MdiActiveDocument;
                doc.UpgradeDocOpen();
            }
            catch (System.Exception ex)
            {
                throw (new System.Exception("AutoCAD getting problem: " + ex.Message));
            }
        }

        public void UnLockActiveDocument()
        {
            Document doc = acadApp.DocumentManager.MdiActiveDocument;
            try
            {
                doc.DowngradeDocOpen(true);
            }
            catch (System.Exception ex)
            {
                throw (new System.Exception("AutoCAD getting problem: " + ex.Message));
            }
        }



        public void UpdateExRefPathInfo2(string FilePath, ref List<PLMObject> plmObjs)
        {
            try
            {

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

                            ShowMessage.ErrorMess("Unresolved xref :" + xgn.Name);
                        }
                        else if (XrefStatus.Unloaded == xgn.XrefStatus)
                        {

                            ShowMessage.ErrorMess("Unloaded xref :" + xgn.Name);
                        }
                        else if (XrefStatus.Unreferenced == xgn.XrefStatus)
                        {

                            ShowMessage.ErrorMess("Unreferenced xref :" + xgn.Name);
                        }
                        else if (XrefStatus.Resolved == xgn.XrefStatus)
                        {


                            Database xdb = xgn.Database;


                            if (xdb != null)
                            {
                                string ProjectName = "", DrawingNO = "", FileType = "", Rev = "", PreFix = "", oldPreFix = "", drawingname = "", projectNO = "";
                                var dbsi = xdb.SummaryInfo.CustomProperties;
                                while (dbsi.MoveNext())
                                {
                                    if (Convert.ToString(dbsi.Key) == "projectno")
                                    {
                                        projectNO = Convert.ToString(dbsi.Value);
                                    }
                                    else if (Convert.ToString(dbsi.Key) == "drawingnumber")
                                    {
                                        DrawingNO = Convert.ToString(dbsi.Value);
                                    }
                                    else if (Convert.ToString(dbsi.Key) == "filetypeid")
                                    {
                                        FileType = Convert.ToString(dbsi.Value);
                                    }
                                    else if (Convert.ToString(dbsi.Key) == "revision")
                                    {
                                        Rev = Convert.ToString(dbsi.Value);
                                        if (Rev.Contains("Ver"))
                                        {

                                            Rev = Rev.Substring(Rev.IndexOf("0"));
                                        }
                                    }
                                    else if (Convert.ToString(dbsi.Key) == "prefix")
                                    {
                                        PreFix = Convert.ToString(dbsi.Value);
                                    }
                                    else if (Convert.ToString(dbsi.Key) == "drawingname")
                                    {
                                        drawingname = Convert.ToString(dbsi.Value);
                                    }
                                    else if (Convert.ToString(dbsi.Key) == "projectname")
                                    {
                                        ProjectName = Convert.ToString(dbsi.Value);
                                    }
                                    else if (Convert.ToString(dbsi.Key) == "oldprefix")
                                    {
                                        oldPreFix = Convert.ToString(dbsi.Value);
                                    }
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
                                    if(NewFilePath!= FilePath)
                                    {
                                        File.Delete(NewFilePath);
                                        ParentNewPath = NewFilePath;
                                        if (FilePath.Contains(checkoutPath))
                                        {
                                            IsMove = true;
                                        }
                                        foreach (PLMObject obj in plmObjs)
                                        {
                                            if (obj.FilePath == xgn.Name)
                                            {
                                                obj.FilePath = NewFilePath;
                                                break;
                                            }
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

                                    bool IsChildinSameFolder = false;
                                    string ParentPath = Path.GetDirectoryName(FilePath);
                                    int L = originalpath.Length;
                                    int L1 = childname.Length;
                                    if(L==L1+2)
                                    {
                                        IsChildinSameFolder = true;
                                    }

                                    childname = Helper.RemovePreFixFromFileName(childname, oldPreFix);
                                    childname = Helper.RemovePreFixFromFileName(childname, PreFix);


                                    newpath = path + childname;
                                    OldChildPath = Path.Combine(Path.GetDirectoryName(path), oldPreFix + childname);
                                     
                                    if (File.Exists(OldChildPath))
                                    {

                                    }
                                    else
                                    {
                                        OldChildPath = Path.Combine(Path.GetDirectoryName(FilePath), childname);

                                    }
                                    if (File.Exists(OldChildPath))
                                    {

                                    }
                                    else
                                    {
                                        OldChildPath = Path.Combine(Path.GetDirectoryName(FilePath), oldPreFix + childname);

                                    }
                                    if (File.Exists(OldChildPath))
                                    {

                                    }
                                    else
                                    {

                                        if (File.Exists(originalpath))
                                        {
                                            FileInfo fi = new FileInfo(originalpath);
                                            string OP = fi.DirectoryName;
                                            OldChildPath = Path.Combine(OP, childname);
                                        }
                                        else
                                        {
                                            //ShowMessage.ErrorMess("File not found.\n" + originalpath);
                                            //return;
                                        }


                                    }
                                    if (File.Exists(OldChildPath) && OldChildPath != newpath)
                                    {
                                        File.Delete(newpath);
                                        if (OldChildPath.Contains(checkoutPath))
                                        {
                                            File.Move(OldChildPath, newpath);
                                        }
                                        else
                                        {
                                            File.Copy(OldChildPath, newpath);
                                        }
                                        foreach (PLMObject obj in plmObjs)
                                        {
                                            string name = Path.GetFileNameWithoutExtension(obj.FilePath);

                                            name = Helper.RemovePreFixFromFileName(name, oldPreFix);

                                            string XrefName = Path.GetFileNameWithoutExtension(Helper.RemovePreFixFromFileName(xgn.Name, oldPreFix));
                                            if (name == XrefName)
                                            {
                                                obj.FilePath = newpath;
                                                break;
                                            }
                                        }
                                        
                                    }
                                    if(File.Exists(newpath))
                                    {
                                        string TnewPath = @".\" + Path.GetFileName(newpath);
                                        btr.PathName = TnewPath;
                                    }
                                    

                                    tr.Commit();

                                    UpdateExRefPathInfo2(newpath, ref plmObjs);
                                }
                            }
                        }
                        else if (XrefStatus.FileNotFound == xgn.XrefStatus)
                        {
                        }

                        if (File.Exists(OldChildPath) && OldChildPath != newpath)
                        {
                            //File.Delete(OldChildPath);
                        }
                        //}//Switch Complete
                    }//For Complete          
                    mainDb.SaveAs(FilePath, DwgVersion.Current);
                }//using db complete
                if (IsMove)
                {
                    if (ParentNewPath.Trim().Length > 0 && File.Exists(FilePath))
                        File.Move(FilePath, ParentNewPath);
                }
                else
                {
                    if (ParentNewPath.Trim().Length>0&& File.Exists(FilePath))
                    File.Copy(FilePath, ParentNewPath);
                }
            }
            catch (System.Exception ex)
            {
                ShowMessage.ErrorMess(ex.Message);
            }
        }
        public System.Data.DataTable GetExternalRefreces(bool IsSaveAsNew = false)
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
                                                ICADManager cadManager = CADFactory.getCADManager();
                                                Hashtable rootdrawingAttrs = new Hashtable();
                                                rootdrawingAttrs = (Hashtable)cadManager.GetAttributes();
                                                rootid = "";
                                                String childrens = "";
                                                for (int j = 0; j < xgn.NumIn; j++)
                                                {
                                                    int tempInt = IsXrNodeEqual(xg, xgn.In(j));
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
                                                    int tempInt = IsXrNodeEqual(xg, xgn.In(j));
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
                                                    dtTreeGrid = Helper.AddRowDrawingPropertiesTable(dtTreeGrid, drawingAttrs);

                                                    int RowIndex = dtTreeGrid.Rows.Count - 1;
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

                                                    if (Convert.ToString(OwnID) != Convert.ToString(de.Key))
                                                    {
                                                        LayoutUnqID = "";
                                                    }
                                                    else
                                                    {
                                                        LayoutUnqID = Convert.ToString(de.Key);
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
                ShowMessage.ErrorMess("Error reading layout info from document " + FilePath + Environment.NewLine + ex.Message);
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
        public bool SetLayoutOwnerID(System.Data.DataTable dtLayoutInfo, string FilePath)
        {
            try
            {
                Database mainDb = new Database(false, true);
                mainDb.ReadDwgFile(FilePath, FileOpenMode.OpenForReadAndAllShare, true, null);
                mainDb.ResolveXrefs(false, false);
                Transaction tr = mainDb.TransactionManager.StartTransaction();


                using (tr)
                {

                    DBDictionary layoutDict = tr.GetObject(mainDb.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
                    foreach (DBDictionaryEntry de in layoutDict)
                    {
                        String layoutName = de.Key;
                        if (layoutName != "Model")
                        {
                            Layout layout = tr.GetObject(de.Value, OpenMode.ForRead) as Layout;

                            string LayoutUnqID = "";

                            for (int i = 0; i < dtLayoutInfo.Rows.Count; i++)
                            {
                                if (Convert.ToString(dtLayoutInfo.Rows[i]["FileLayoutName"]) == de.Key)
                                {
                                    layout.OwnerId = (Autodesk.AutoCAD.DatabaseServices.ObjectId)dtLayoutInfo.Rows[i]["ACLayoutID"];

                                    break;
                                }

                            }



                        }
                        else
                        {
                            continue;
                        }
                    }


                    tr.Commit();
                }
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess("Error while setting layout owner id." + Environment.NewLine + E.Message); return false;
            }
            return true;
        }
        public int IsXrNodeEqual(XrefGraph xrGraph, GraphNode grNode)
        {
            for (int i = 0; i < xrGraph.NumNodes; i++)
            {
                if (grNode == xrGraph.GetXrefNode(i) as GraphNode)
                    return i;
            }
            return -1;
        }

        [Autodesk.AutoCAD.Runtime.CommandMethod("TITLEBLOCK")]
        public void UpdateLayoutAttribute(Hashtable documentProperties = null)
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
                            Hashtable LayoutData = documentProperties; //new Hashtable();
                                                                       // ArasConnector.ArasConnector LayoutDetail = new ArasConnector.ArasConnector();
                                                                       // LayoutData = LayoutDetail.GetLayoutDetail(drawingAttrs["drawingid"].ToString(), layoutName);

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
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
        public void UpdateLayoutAttribute1(Hashtable documentProperties = null)
        {
            try
            {

                Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                DocumentLock doclock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument();
                Database db = doc.Database;
                Editor ed = doc.Editor;
                // Hashtable drawingAttrs = documentProperties; //new Hashtable();
                Hashtable drawingAttrs = new Hashtable();
                IDictionaryEnumerator en = doc.Database.SummaryInfo.CustomProperties;
                while (en.MoveNext())
                {
                    // if(documentProperties==null)
                    {
                        drawingAttrs.Add(en.Key, en.Value == null ? string.Empty : Convert.ToString(en.Value));
                    }
                    // else
                    {

                    }

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
                            Hashtable LayoutData = documentProperties; //new Hashtable();
                                                                       // ArasConnector.ArasConnector LayoutDetail = new ArasConnector.ArasConnector();
                                                                       // LayoutData = LayoutDetail.GetLayoutDetail(drawingAttrs["drawingid"].ToString(), layoutName);

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
                                        int count = 0;
                                        foreach (ObjectId arId in br.AttributeCollection)
                                        {
                                            try
                                            {

                                                //count++;
                                                //if(count==15)
                                                //{

                                                //}
                                                DBObject obj = tr.GetObject(arId, OpenMode.ForRead);
                                                AttributeReference ar = obj as AttributeReference;

                                                if (ar.Tag.ToUpper() == "DRAWINGNUMBER")
                                                {
                                                    ar.UpgradeOpen();
                                                    ar.TextString = LayoutData["drawingnumber"] == null ? string.Empty : LayoutData["drawingnumber"].ToString();
                                                    ar.DowngradeOpen();
                                                }
                                                if (ar.Tag.ToUpper() == "DRAWINGNAME")
                                                {
                                                    ar.UpgradeOpen();
                                                    ar.TextString = drawingAttrs["drawingname"] == null ? string.Empty : drawingAttrs["drawingname"].ToString();
                                                    ar.DowngradeOpen();
                                                }
                                                if (ar.Tag.ToUpper() == "PROJECTNAME")
                                                {
                                                    ar.UpgradeOpen();
                                                    ar.TextString = LayoutData["projectname"] == null ? string.Empty : LayoutData["projectname"].ToString(); //drawingAttrs["projectname"].ToString();
                                                    ar.DowngradeOpen();
                                                }
                                                if (ar.Tag.ToUpper() == "PROJECTID")
                                                {
                                                    ar.UpgradeOpen();
                                                    ar.TextString = LayoutData["projectid"] == null ? string.Empty : LayoutData["projectid"].ToString(); //drawingAttrs["projectid"].ToString();
                                                    ar.DowngradeOpen();
                                                }
                                                if (ar.Tag.ToUpper() == "GENERATION")
                                                {
                                                    ar.UpgradeOpen();
                                                    ar.TextString = drawingAttrs["generation"] == null ? string.Empty : drawingAttrs["generation"].ToString(); //drawingAttrs["generation"].ToString();
                                                    ar.DowngradeOpen();
                                                }
                                                if (ar.Tag.ToUpper() == "REVISION")
                                                {
                                                    ar.UpgradeOpen();
                                                    ar.TextString = LayoutData["revision"] == null ? string.Empty : LayoutData["revision"].ToString(); //drawingAttrs["revision"].ToString();
                                                    ar.DowngradeOpen();
                                                }
                                                if (ar.Tag.ToUpper() == "DWGSTATE")
                                                {
                                                    ar.UpgradeOpen();
                                                    ar.TextString = drawingAttrs["drawingstate"] == null ? string.Empty : drawingAttrs["drawingstate"].ToString();
                                                    ar.DowngradeOpen();
                                                }
                                                string DS = drawingAttrs["drawingstate"] == null ? string.Empty : drawingAttrs["drawingstate"].ToString();
                                                string DS1 = LayoutData["drawingstate"] == null ? string.Empty : LayoutData["drawingstate"].ToString();
                                                if (ar.Tag.ToUpper() == "GOODNOT")
                                                {
                                                    ar.UpgradeOpen();
                                                    if (((DS1 == "GFC") ||
                                                        (DS1 == "Released")) &&
                                                        ((DS == "GFC") ||
                                                        (DS == "Coordinated") ||
                                                        (DS == "Const-Dwg")))
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
                                                    ar.TextString = LayoutData["createdby"] == null ? string.Empty : LayoutData["createdby"].ToString(); //drawingAttrs["createdby"].ToString();
                                                    ar.DowngradeOpen();
                                                }
                                                if (ar.Tag.ToUpper() == "MODIFIEDBY")
                                                {
                                                    ar.UpgradeOpen();
                                                    ar.TextString = LayoutData["modifiedby"] == null ? string.Empty : LayoutData["modifiedby"].ToString(); //drawingAttrs["modifiedby"].ToString();
                                                    ar.DowngradeOpen();
                                                }
                                                if (ar.Tag.ToUpper() == "CREATEDON")
                                                {
                                                    ar.UpgradeOpen();
                                                    ar.TextString = LayoutData["createdon"] == null ? string.Empty : LayoutData["createdon"].ToString().Substring(0, 10); //drawingAttrs["createdon"].ToString().Substring(0, 10); 
                                                    ar.DowngradeOpen();
                                                }
                                                if (ar.Tag.ToUpper() == "MODIFIEDON")
                                                {
                                                    ar.UpgradeOpen();
                                                    ar.TextString = LayoutData["modifiedon"] == null ? string.Empty : LayoutData["modifiedon"].ToString().Substring(0, 10); //drawingAttrs["modifiedon"].ToString().Substring(0,10);
                                                    ar.DowngradeOpen();
                                                }
                                                if (ar.Tag.ToUpper() == "DRAWINGSTATE")
                                                {
                                                    ar.UpgradeOpen();
                                                    ar.TextString = LayoutData["drawingstate"] == null ? string.Empty : LayoutData["drawingstate"].ToString(); //drawingAttrs["drawingstate"].ToString();
                                                    ar.DowngradeOpen();
                                                }
                                            }
                                            catch
                                            {

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
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        //[Autodesk.AutoCAD.Runtime.CommandMethod("DWGPROPS")]
        //[Autodesk.AutoCAD.Runtime.CommandMethod("EATTEDIT")]
        //[Autodesk.AutoCAD.Runtime.CommandMethod("ATTEDIT")]
        //[Autodesk.AutoCAD.Runtime.CommandMethod("-BEDIT")]

        public void DisableCommand()
        {
            // MessageBox.Show("Sorry!!! AutoCAD does not allow you to run this Command!!!");
        }

        #region "Plot Layout to PDF"
        /*
        public void PlotLayout(Document doc, Transaction tr, ObjectIdCollection layoutsToPlot)
        {
            PlotInfo pi = new PlotInfo();
            PlotInfoValidator piv = new PlotInfoValidator();
            piv.MediaMatchingPolicy = MatchingPolicy.MatchEnabled;

            if (PlotFactory.ProcessPlotState == ProcessPlotState.NotPlotting)
            {
                PlotEngine pe = PlotFactory.CreatePublishEngine();
                using (pe)
                {
                    PlotProgressDialog ppd = new PlotProgressDialog(false, layoutsToPlot.Count, true);
                    using (ppd)
                    {
                        int numSheet = 1;
                        foreach (ObjectId layoutId in layoutsToPlot)
                        {
                            Layout lo = (Layout)tr.GetObject(layoutId, OpenMode.ForRead);
                            PlotSettings ps = new PlotSettings(lo.ModelType);
                            ps.CopyFrom(lo);
                            PlotSettingsValidator psv = PlotSettingsValidator.Current;
                            psv.SetPlotType(ps, Autodesk.AutoCAD.DatabaseServices.PlotType.Extents);
                            psv.SetUseStandardScale(ps, true);
                            psv.SetStdScaleType(ps, StdScaleType.ScaleToFit);
                            psv.SetPlotCentered(ps, true);
                            psv.SetPlotConfigurationName(ps, "DWG To PDF.pc3", "ANSI_A_(11.00_x_8.50_Inches)");//Plot to Pdf                            
                            pi.Layout = layoutId;
                            LayoutManager.Current.CurrentLayout = lo.LayoutName;
                            pi.OverrideSettings = ps;
                            piv.Validate(pi);
                            if (numSheet == 1)
                            {
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
                                pe.BeginDocument(pi, doc.Name, null, 1, true, "C:\\Test\\" + lo.LayoutName + ".pdf");//plot to pdf                                
                            }                            
                            ppd.set_PlotMsgString(PlotMessageIndex.SheetName, doc.Name.Substring(doc.Name.LastIndexOf("\\") + 1) + " - sheet " + numSheet.ToString() + " of " + layoutsToPlot.Count.ToString());
                            ppd.OnBeginSheet();
                            ppd.LowerSheetProgressRange = 0;
                            ppd.UpperSheetProgressRange = 100;
                            ppd.SheetProgressPos = 0;
                            PlotPageInfo ppi = new PlotPageInfo();
                            pe.BeginPage(ppi, pi, (numSheet == layoutsToPlot.Count), null);
                            pe.BeginGenerateGraphics(null);
                            ppd.SheetProgressPos = 50;
                            pe.EndGenerateGraphics(null);
                            pe.EndPage(null);
                            ppd.SheetProgressPos = 100;
                            ppd.OnEndSheet();
                            numSheet++;
                            ppd.PlotProgressPos += (100 / layoutsToPlot.Count);
                        }
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
        }
         */
        #endregion "Plot Layout to PDF"

        public bool CheckForCurruntlyOpenDoc(string FilePath, bool MakeActive = true)
        {
            try
            {
                foreach (Document Doc in acadApp.DocumentManager)
                {
                    if (Doc.Database.Filename.ToLower() == FilePath.ToLower())
                    {
                        if (MakeActive)
                        {
                            acadApp.DocumentManager.MdiActiveDocument = Doc;
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
                foreach (Document Doc in acadApp.DocumentManager)
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
        public void OpenDoc(string FilePath)
        {
            try
            {
                acadApp.DocumentManager.Open(FilePath, false);
            }
            catch (System.Exception E)
            {
                ShowMessage.ErrorMess(E.Message);
            }
        }
    }
}


