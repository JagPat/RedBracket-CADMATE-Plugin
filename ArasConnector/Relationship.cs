using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ArasConnector
{
   public class Relationship
    {
       private String relName;
       private String relID;
       private PLMObject fromObject;
       private PLMObject toObject;
       private Hashtable relAttributes;

       public String RelationshipName
       {
           get { return this.relName; }
           set { this.relName = value; }
       }
       public String RelationshipID
       {
           get { return this.relID; }
           set { this.relID = value; }
       }

       public PLMObject FromObject
       {
           get { return this.fromObject; }
           set { this.fromObject = value; }
       }
       public PLMObject ToObject
       {
           get { return this.toObject; }
           set { this.toObject = value; }
       }

       public Hashtable RelationshipAttributes
       {
           get { return this.relAttributes; }
           set { this.relAttributes = value; }
       }

    }
}
