
//<Note: This Calass is tobe remove>



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArasConnector
{
    public class ItemInformation 
    {

        private String infoMessage;
        private String itemId;
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
        public String ItemId
        {
            get { return this.itemId; }
            set { this.itemId = value; }
        }
    
    
    }
}
