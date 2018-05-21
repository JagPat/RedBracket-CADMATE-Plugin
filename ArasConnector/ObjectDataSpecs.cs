using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArasConnector
{
    public class ObjectDataSpecs
    {
        //public static string STATE;
        //public static string LOCKSTATUS;
        //public static string GENERATION;

        bool state = false;
        public bool State
        {
            get { return this.state; }
            set { this.state = value; }
        }

        bool configId = false;
        public bool ConfigId
        {
            get { return this.configId; }
            set { this.configId = value; }
        }

        bool id = false;
        public bool Id
        {
            get { return this.id; }
            set { this.id = value; }
        }

        bool name = false;
        public bool Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        bool number = false;
        public bool Number
        {
            get { return this.number; }
            set { this.number = value; }
        }
        bool project = false;
        public bool Project
        {
            get { return this.project; }
            set { this.project = value; }
        }

        bool projectid = false;
        public bool ProjectId
        {
            get { return this.projectid; }
            set { this.projectid = value; }
        }
        bool created_on = false;
        public bool CreatedOn
        {
            get { return this.created_on; }
            set { this.created_on = value; }
        }
        bool created_by_id = false;
        public bool CreatedById
        {
            get { return this.created_by_id; }
            set { this.created_by_id = value; }
        }
        bool modified_on = false;
        public bool ModifiedOn
        {
            get { return this.modified_on; }
            set { this.modified_on = value; }
        }
        bool modified_by_id = false;
        public bool ModifiedById
        {
            get { return this.modified_by_id; }
            set { this.modified_by_id = value; }
        }
        bool revision = false;
        public bool Revision
        {
            get { return this.revision; }
            set { this.revision = value; }
        }

        bool status = false;
        public bool LockStatus
        {
            get { return this.status; }
            set { this.status = value; }
        }

        bool generation = false;
        public bool Generation
        {
            get { return this.generation; }
            set { this.generation = value; }
        }
        bool sg_doc_owner = false;
        public bool SGOwnerCompany
        {
            get { return this.sg_doc_owner; }
            set { this.sg_doc_owner = value; }
        }

      /*  bool type = false;
        public bool Type
        {
            get { return this.type; }
            set { this.type = value; }
        }
        
        /*
        public void setStateRequired()
        {
            stateRequired = true;
        }

        public bool getStateRequired()
        {
            return stateRequired;
        }*/
        
       
        List<String> attributes;
        public List<String> Attributes
        {
            get { return this.attributes; }
            set { this.attributes = value; }
        }
       // ObjectDataSpec spec = new ObjectDataSpec();
       // spec.State=true;

        
    }
}
