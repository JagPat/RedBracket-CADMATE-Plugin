using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace AutocadPlugIn
{
    public class Command
    {
        
            private Hashtable htDocProp = new Hashtable();

            public Hashtable DocumentProperty
            {
                get { return this.htDocProp; }
                set { this.htDocProp = value; }
            }
          
    }

}
