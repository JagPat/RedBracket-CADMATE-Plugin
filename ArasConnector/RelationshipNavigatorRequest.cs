using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArasConnector
{
    public class RelationshipNavigatorRequest
    {
        private String relName;
        private String relDirection;

        public string RelName
        {
            get { return this.relName; }
            set { this.relName = value; }
        }
        public string RelDirection
        {
            get { return this.relDirection; }
            set { this.relDirection = value; }
        }

    }
}
