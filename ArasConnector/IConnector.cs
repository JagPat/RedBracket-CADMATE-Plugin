using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Data;
using System.Text;

namespace ArasConnector
{
    public interface IConnector
    {
        void Connect(string url, string dbName, string usrName, string passwd, string authoringtool);
        void Disconnect();
        String[] getDataBaseList(string serverPath); 
        //PLMObject AddObject(string filepath, string fileName, string objName, string objNumber, string objDescription, string configUniqName, string authoringtool, string template);
        //PLMObject AddObject(PLMObject obj);
        //Need to understand why searchAuthoringTool was used
        SearchResult SearchObject(PLMObject ItemObject, string objLatest,ObjectDataSpecs dataSpecs);
        void ExpandObject(ref PLMObject obj, List<RelationshipNavigatorRequest> relRequests, RelationshipDataSpecs dataSpecs, int expandcount);
        void ExpandObjectRecursively(ref PLMObject obj, List<RelationshipNavigatorRequest> relRequests, RelationshipDataSpecs dataSpecs, int count);
        //removed paramter string itemType
        //by default based on relationshiplist, objects will be loaded
        void LoadObject(ref DataTable ItemInfo, string checkOutDir, ObjectDataSpecs dataSpecs);
        //PLMObject LoadObjectAllLevels(PLMObject obj, List<RelationshipNavigateRequest> relRequests, ObjectDataSpecs dataSpecs);
        //removed paramter string itemType
        void LockObject(List<PLMObject> plmObjs);
        void UnlockObject(List<PLMObject> plmObjs);
        void SaveObject(ref List<PLMObject> obj);
        void LockStatus(ref DataTable ItemInfo);
       // PLMConstants getPLMConstants();
       // String GetLatestRevision(PLMObject obj);
        List<PLMObject> GetPLMObjectInformation(List<PLMObject> plmobjs);
        String GetAttributes(ObjectDataSpecs dataSpecs);
        String GetLockBy(String lockById);
    }

}
