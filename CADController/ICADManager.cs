using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace CADController
{

   
    public interface ICADManager
    {
        //<Summary>
        //   <Comment>This Interface is subjected to CHANGE according to requirment.</Comment> 
        //   <Author>Vasant PADHIYAR</Author>
        //   <Date>19/05/2012(dd/mm/yyyy)</Date>
        //</Summary>

        void SetAttributes(Hashtable hashTable);    
        Hashtable GetAttributes();
         void UpdateAttributes(Hashtable hashTable);
         void OpenActiveDocument(String fileName, String openMode, Hashtable properties);
         void CloseActiveDocument(String fileName);
         void DeleteActiveDocument(String fileName);
         void LockActiveDocument();
         void UnLockActiveDocument();
         System.Data.DataTable GetExternalRefreces();         
         void SaveActiveDrawing();         
    }
}
