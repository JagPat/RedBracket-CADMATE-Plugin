using System;
using System.Data;
using System.Collections;
using System.Windows.Forms;
using Gssoft.Gscad.ApplicationServices;
using Gssoft.Gscad.DatabaseServices;
using Gssoft.Gscad.EditorInput;

namespace RBAutocadPlugIn
{
    public class UnlockCommand : Command
    {
        private ArrayList drawingIds;
        public ArrayList DrawingIds
        {
            get { return this.drawingIds; }
            set { this.drawingIds = value; }
        }

        private System.Data.DataTable drawingInfo;
        public System.Data.DataTable DrawingInfo
        {
            get { return this.drawingInfo; }
            set { this.drawingInfo = value; }
        }
    }
}
