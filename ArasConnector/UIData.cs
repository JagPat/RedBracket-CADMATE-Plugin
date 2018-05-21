using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArasConnector
{
   public class UIData
    {
       private bool selected = false;
       private String template;

       public bool IsSelected
       {
           get { return this.selected ;}
           set { this.selected = value; }
       }

       public String Template
       {
           get { return this.template; }
           set { this.template = value; }
       }
    }
}
