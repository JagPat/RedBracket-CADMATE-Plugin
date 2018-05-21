//<Note : - this class is to be remove>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArasConnector
{
    public class DrawingInfo 
    {
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


        private String drawingFullPath;
        public String DrawingFullPath
        {
            get
            {
                return this.drawingFullPath;
            }
            set
            {
                this.drawingFullPath = value;
            }
        }


        private String nativeFileId;
        public String NativeFileId
        {
            get
            {
                return this.nativeFileId;
            }
            set
            {
                this.nativeFileId = value;
            }
        }

        private bool isFile;
        public bool IsFile
        {
            get
            {
                return this.isFile;
            }
            set
            {
                this.isFile = value;
            }
        }


        private String drawingID;
        public String DrawingID
        {
            get
            {
                return this.drawingID;
            }
            set
            {
                this.drawingID = value;
            }
        }

        private String revision;
        public String Revision
        {
            get
            {
                return this.revision;
            }
            set
            {
                this.revision = value;
            }
        }

        private String drawingNumber;
        public String DrawingNumber
        {
            get
            {
                return this.drawingNumber;
            }
            set
            {
                this.drawingNumber = value;
            }
        }

        private String drawingState;
        public String DrawingState
        {
            get
            {
                return this.drawingState;
            }
            set
            {
                this.drawingState = value;
            }
        }
        private String lockStatus;
        public String LockStatus
        {
            get
            {
                return this.lockStatus;
            }
            set
            {
                this.lockStatus = value;
            }
        }
        private String generation;
        public String Generation
        {
            get
            {
                return this.generation;
            }
            set
            {
                this.generation = value;
            }
        }
        private String itemTypeName;
        public String ItemTypeName
        {
            get { return this.itemTypeName; }
            set { this.itemTypeName = value; }
        }
        private String projectname;
        public String ProjectName
        {
            get { return this.projectname; }
            set { this.projectname = value; }
        }


    }
}
