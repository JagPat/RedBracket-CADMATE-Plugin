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

        private String fileStatus;

        public String FileStatus
        {
            get { return this.fileStatus; }
            set { this.fileStatus = value; }
        }
        private String fileType;
        public String FileType
        {
            get { return this.fileType; }
            set { this.fileType = value; }
        }

        private String fileDescription;
        public String FileDescription
        {
            get { return this.fileDescription; }
            set { this.fileDescription = value; }
        }
    }
   
    }
