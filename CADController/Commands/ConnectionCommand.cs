using System;
using System.Collections.Generic;
using System.Text;

namespace CADController.Commands
{
    public class ConnectionCommand : Command
    {

        private String url;
        private String dbName;
        private String userName;
        private String passwd;
        private String authoringtool;

        public String AuthoringTool
        {
            get
            {
                return this.authoringtool;
            }
            set
            {
                this.authoringtool = value;
            }

        } 
        
        public String Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = value;
            }
        }
        public String DbName
        {
            get
            {
                return this.dbName;
            }
            set
            {
                this.dbName = value;
            }
        }
        public String UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                this.userName = value;
            }
        }
        public String Passwd
        {
            get
            {
                return this.passwd;
            }
            set
            {
                this.passwd = value;
            }
        }
        

    }
}
