using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using AdvancedDataGridView;

namespace ArasConnector
{
       

    public class SearchResult 
    {
       /* private System.Windows.Forms.DataGridViewCheckBoxColumn Check = new DataGridViewCheckBoxColumn();
        private TreeGridColumn DrawingName = new TreeGridColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingNumber = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn CADType = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn Revision = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingID = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewButtonColumn ExpandButton = new DataGridViewButtonColumn();
        public SearchResult()
        {

            this.ResultTable.Columns.Add(Check);
            this.ResultTable.Columns.Add(ExpandButton);
            this.ResultTable.Columns.Add(DrawingName);
            this.ResultTable.Columns.Add(DrawingNumber);
            this.ResultTable.Columns.Add(CADType);
            this.ResultTable.Columns.Add(Revision);
            this.ResultTable.Columns.Add(DrawingID);

        }
        */

        private List<PLMObject> objectList = new List<PLMObject>();

        public List<PLMObject> ObjectList
        {
            get
            {
                return this.objectList;
            }
            set
            {
                this.objectList = value;
            }
        }




     /*   private TreeGridView resultTable = new TreeGridView();

        public TreeGridView ResultTable
        {
            get
            {
                return this.resultTable;
            }
            set
            {
                this.resultTable = value;
            }
        }
    */
        private String infoMessage;
        public String InfoMessage
        {
            get
            {
                return this.infoMessage;
            }
            set
            {
                this.infoMessage = value;
            }
        }



        private String errorMsg;
        public String ErrorMsg
        {
            get
            {
                return this.errorMsg;
            }
            set
            {
                this.errorMsg = value;
            }
        }

    }
}
