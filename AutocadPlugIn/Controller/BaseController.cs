using System;
using System.Windows.Forms;
using System.Data;
 
using AdvancedDataGridView; 

namespace RBAutocadPlugIn
{
    public abstract class BaseController
    {
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check = new DataGridViewCheckBoxColumn();
        private TreeGridColumn DrawingName = new TreeGridColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingNumber = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn CADType = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn Revision = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn Generation = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn DrawingID = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn LockStatus = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn LockBy = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewButtonColumn ExpandButton = new DataGridViewButtonColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectName = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectId = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn State = new DataGridViewTextBoxColumn();
        private System.Windows.Forms.DataGridViewTextBoxColumn OwnerCompany = new DataGridViewTextBoxColumn();
        public BaseController()
        {
            this.dtDocuments.Columns.Add(Check);
            this.dtDocuments.Columns.Add(ExpandButton);
            this.dtDocuments.Columns.Add(DrawingName);
            this.dtDocuments.Columns.Add(DrawingNumber);
            this.dtDocuments.Columns.Add(CADType);
            this.dtDocuments.Columns.Add(Revision);
            this.dtDocuments.Columns.Add(Generation);
            this.dtDocuments.Columns.Add(DrawingID);
            this.dtDocuments.Columns.Add(LockStatus);
            this.dtDocuments.Columns.Add(LockBy);
            this.dtDocuments.Columns.Add(ProjectName);
            this.dtDocuments.Columns.Add(ProjectId);
            this.dtDocuments.Columns.Add(State);
            this.dtDocuments.Columns.Add(OwnerCompany);
        }
        public DataTable dtNewPlmObjInfomation = new DataTable();
        public string errorString = null;
        public string infoMessage = null;
        public TreeGridView dtDocuments = new TreeGridView();
   
 
       

        public RBConnector ObjRBC = new RBConnector();
 
        public abstract void Execute(Command command);

    }
}
