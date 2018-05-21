using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArasConnector
{
    public class RelationshipDataSpecs
    {

        private ObjectDataSpecs fromObjectDataSpecs;
        public ObjectDataSpecs FromObjectDataSpecs
        {
            get { return this.fromObjectDataSpecs; }
            set { this.fromObjectDataSpecs = value; }
        }

        private ObjectDataSpecs toObjectDataSpecs;
        public ObjectDataSpecs ToObjectDataSpecs
        {
            get { return this.toObjectDataSpecs; }
            set { this.toObjectDataSpecs = value; }
        }

        private Hashtable relAttributes;
        public Hashtable RelAttributes
        {
            get { return this.relAttributes; }
            set { this.relAttributes = value; }
        }
    }
}
