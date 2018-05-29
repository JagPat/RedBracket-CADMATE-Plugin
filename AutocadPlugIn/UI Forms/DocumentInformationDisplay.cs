using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CADController.Commands;
using CADController.Controllers;
using CADController;
using System.Collections;

using AdvancedDataGridView;

namespace AutocadPlugIn.UI_Forms
{
    public partial class DocumentInformationDisplay : Form
    {
        ICADManager objCADMgr = CADFactory.getCADManager();
        BaseController controller = new DocumentInformationDisplayController();
        DocumentInformationDisplayCommand objCmd = new DocumentInformationDisplayCommand();
        DataTable dtCurrentInformation = new DataTable("currentinformation");
        DataTable dtLatestInformation = new DataTable("latestinformation");
        DataTable dtFinalInformation = new DataTable("finaltable");
        DataSet dsTables = new DataSet();
        
        public DocumentInformationDisplay()
        {
            InitializeComponent();
        }

        private void DocumentInformation_Load(object sender, EventArgs e)
        {
            try
            {
                List<String> drawings = new List<String>();
               TreeGridNode  node = null;
                dtCurrentInformation= objCADMgr.GetExternalRefreces();
                foreach(DataRow row in dtCurrentInformation.Rows)
                 {
                     drawings.Add(row["drawingnumber"] + ":" + row["type"] + ":" + row["drawingid"]);
                 } 
                 objCmd.DarawingInformation = drawings;
                controller.Execute(objCmd);
                dtLatestInformation = controller.dtNewPlmObjInfomation;
                dsTables.Tables.Add(dtCurrentInformation);
                dsTables.Tables.Add(dtLatestInformation);
                DataRelation drel = new DataRelation("EquiJoin", dtLatestInformation.Columns["drawingnumber"], dtCurrentInformation.Columns["drawingnumber"]);
                //try
                //{
                //    drel = new DataRelation("EquiJoin", dtLatestInformation.Columns["drawingnumber"], dtCurrentInformation.Columns["drawingnumber"]);
                //}
                //catch(Exception E)
                //{

                //}

                 dsTables.Relations.Add(drel);
                dtFinalInformation.Columns.Add("drawingnumber", typeof(String));
                dtFinalInformation.Columns.Add("drawingname", typeof(String));               
                dtFinalInformation.Columns.Add("lockstatus", typeof(String));
                dtFinalInformation.Columns.Add("classification", typeof(String));
                dtFinalInformation.Columns.Add("currentstate", typeof(String));
                dtFinalInformation.Columns.Add("currentgeneration", typeof(String));
                dtFinalInformation.Columns.Add("currentrevision", typeof(String));
                dtFinalInformation.Columns.Add("lateststate", typeof(String));
                dtFinalInformation.Columns.Add("latestgeneration", typeof(String));
                dtFinalInformation.Columns.Add("latestrevision", typeof(String));
                dtFinalInformation.Columns.Add("projectname", typeof(String));
                dtFinalInformation.Columns.Add("projectid", typeof(String));
                dsTables.Tables.Add(dtFinalInformation);

                foreach (DataRow dr in dsTables.Tables[0].Rows)
                {

                    DataRow parent = dr.GetParentRow("EquiJoin");
                    //DataRow parent = dr.GetParentRow(drel);

                    DataRow current = dtFinalInformation.NewRow();

                    // Just add all the columns' data in "dr" to the New table.


                    current["drawingname"] = dr["drawingname"];
                    current["drawingnumber"] = dr["drawingnumber"];
                    current["classification"] = dr["classification"];
                    current["currentstate"] = dr["drawingstate"];
                    current["currentgeneration"] = dr["generation"];
                    current["currentrevision"] = dr["revision"];

                    // Add the column that is not present in the child, which is present in the parent.

                   // current["drawingid"] = parent["drawingid"];
                   if(parent!=null)
                    {
                        current["latestrevision"] = parent["LatestRevision"];
                        current["latestgeneration"] = parent["LatestGeneration"];
                        current["lateststate"] = parent["LatestState"];
                        current["lockstatus"] = parent["LockStatus"];
                        current["projectname"] = parent["projectname"];
                        current["projectid"] = parent["projectid"];
                    }
                   

                    dtFinalInformation.Rows.Add(current);

                }
                for (int i = 0; i < drawinginfotreeGrid.Nodes.Count; i++)
                {
                    TreeGridNode TreeNode1 = drawinginfotreeGrid.Nodes.ElementAt(0);

                    this.ClearTreeView(TreeNode1);

                    drawinginfotreeGrid.Nodes.Remove(TreeNode1);
                }

                int counter = 0;
                foreach (DataRow rw in dtFinalInformation.Rows)
                {
                    
                    if (counter == 0)
                    {
                        node = drawinginfotreeGrid.Nodes.Add(rw["drawingname"], rw["drawingnumber"], rw["lockstatus"], rw["classification"], rw["currentstate"], rw["currentgeneration"], rw["currentrevision"], rw["lateststate"], rw["latestgeneration"], rw["latestrevision"],rw["projectname"],rw["projectid"]);
                        node.Expand();
                        counter = 1;
                    }
                    else
                    {
                        node.Nodes.Add(rw["drawingname"], rw["drawingnumber"], rw["lockstatus"], rw["classification"], rw["currentstate"], rw["currentgeneration"], rw["currentrevision"], rw["lateststate"], rw["latestgeneration"], rw["latestrevision"],rw["projectname"],rw["projectid"]);
                    }
                }
                
                                }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return;
            }
        }

        private void ClearTreeView(TreeGridNode CurrentNode)
        {
            try
            {
                for (int nodeCount = 0; nodeCount < CurrentNode.Nodes.Count; nodeCount++)
                {
                    if (CurrentNode.Nodes.ElementAt(nodeCount).Nodes.Count() == 0)
                    {
                        drawinginfotreeGrid.Nodes.Remove(CurrentNode.Nodes.ElementAt(nodeCount));
                    }
                    else
                    {
                        TreeGridNode node1 = CurrentNode.Nodes.ElementAt(nodeCount);
                        this.ClearTreeView(node1);
                        drawinginfotreeGrid.Nodes.Remove(node1);
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Exception Occur: " + ex);
                return;
            }

        }

     
    }
}
