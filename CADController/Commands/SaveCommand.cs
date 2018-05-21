using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArasConnector;

namespace CADController.Commands
{
    public class SaveCommand : Command
    {
        private List<String> drawingList = new List<String>();

        public List<String> Drawings
        {
            get { return this.drawingList; }
            set { this.drawingList = value; }
        }

        private List<String> newDrawings = new List<String>();

        public List<String> NewDrawings
        {
            get { return this.newDrawings; }
            set { this.newDrawings = value; }        
        }

        private DataTable drawingInfo;
        public DataTable DrawingInfo
        {
            get { return this.drawingInfo; }
            set { this.drawingInfo = value; }
        }
    }
}
