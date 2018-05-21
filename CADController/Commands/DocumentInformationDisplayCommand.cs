using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace CADController.Commands
{
    public class DocumentInformationDisplayCommand : Command
    {
        private List<String> drawinginformation = new List<String>();// String fortmate "DrawingId:Itemtype".
        public List<String> DarawingInformation
        {
            get { return this.drawinginformation; }
            set { this.drawinginformation = value;}
        }

    }
}
