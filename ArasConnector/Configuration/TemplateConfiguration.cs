using System;
using System.Collections;
using System.Data;
namespace ArasConnector.Configuration
{
   public class TemplateConfiguration
    {
       private static DataTable dtTemplates = new DataTable();
       public static DataTable  Templates
       {
           get {return dtTemplates;}
       }
  
    }
}
