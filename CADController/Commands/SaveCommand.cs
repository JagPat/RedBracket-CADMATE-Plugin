using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

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
        private  String filePath ;

        public  String FilePath
        {
            get { return this.filePath; }
            set { this.filePath = value; }
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

        private String filePreFix;

        public String FilePreFix
        {
            get { return this.filePreFix; }
            set { this.filePreFix = value; }
        }
    
        private String projectID;
        public String ProjectID
        {
            get { return this.projectID; }
            set { this.projectID = value; }
        }
         
    }
   
    }
