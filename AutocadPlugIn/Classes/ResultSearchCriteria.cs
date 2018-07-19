using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadPlugIn
{
    public class ResultSearchCriteria
    {
        public string id { set; get; }

        public string fileNo { set; get; }

        public string name { set; get; }

        public string updatedon { set; get; }

        public ResultSearchCriteriaCoreType coreType { set; get; }

        public string updatedby { set; get; }

        public string size { set; get; }

        public ResultSearchCriteriaStatus status { set; get; }

        public string createdby { set; get; }

        public string versionno { set; get; }

        public string projectname { set; get; }

        public string projectinfo { set; get; }

        public bool isletest { set; get; }

        public bool shared { set; get; }

        public bool sharing { set; get; }

        public bool isImportant { set; get; }

        public string fileExt { set; get; }

        public bool filelock { set; get; }

        public string fileicon { set; get; }

        public int unreadcount { set; get; }

        public bool isCollection { set; get; }

        public int milisecond { set; get; }

        public bool previewavailable { set; get; }

        public bool isowner { set; get; }

        public bool supportpdf { set; get; }

        public bool unreadItem { set; get; }

        public bool hasViewPermission { set; get; }

        public bool isActFileLatest { set; get; }

        public bool isEditable { set; get; }

        public bool canDelete { set; get; }

        public bool canEditStatus { set; get; }

        public bool hasStatusClosed { set; get; }

        public ResultSearchCriteriaType type { set; get; }

        public string projectNumber { set; get; }

        public List<LayoutInfo> fileLayout { set; get; }

        public string created0n { get; set; }

        public string createdOn { get; set; }

        ////public string resource { set; get; }


        public string folderid { get; set; }

        public string folderpath { get; set; }

        public clsFolderInfo folder { get; set; }
        public string projectManager { get; set; }

        public ResultSearchCriteria[] filebean { get; set; }

    }
 
    public class clsDownloadedFiles
    {
        public string MainFilePath { set; get; }
        public string ParentFilePath { set; get; }
        public string FilePath { set; get; }
        public string FileName { set; get; }
        public string ParentFileName { set; get; }
        public string Prefix { set; get; }
        public string ParentPrefix { set; get; }
        public bool XrefStatus { set; get; }
    }
    public class clsFolderInfo
    {
        public string id { set; get; }
        public string name { set; get; }
        public string companyId { set; get; }
        public string childFolderSize { set; get; }
        public clsFolderInfo parentFolder { set; get; }
    }
    public class ResultSearchCriteriaStatus
    {
        public int id { set; get; }

        public string statusname { set; get; }

        public string type { set; get; }

        public bool iscustom { set; get; }

        public int companyid { set; get; }

        public ResultSearchCriteriaCoreType coretype { set; get; }

        public int priority { set; get; }

        public int createdBy { set; get; }

        public string createdOn { set; get; }

        public int updatedBy { set; get; }

        public string updatedOn { set; get; }

        public bool active { set; get; }

        public bool deleted { set; get; }
    }

    public class ResultSearchCriteriaCoreType
    {
        public string id { set; get; }

        public string name { set; get; }

        public string type { set; get; }

        public bool iscustom { set; get; }

        public int companyid { set; get; }

        public string master { set; get; }

        public int createdBy { set; get; }

        public string createdOn { set; get; }

        public int updatedBy { set; get; }

        public string updatedOn { set; get; }

        public bool active { set; get; }

        public bool deleted { set; get; }

        public string colorId { set; get; }
    }

    public class ResultSearchCriteriaType
    {
        public int id { set; get; }

        public string name { set; get; }

        public string type { set; get; }

        public bool iscustom { set; get; }

        public int companyid { set; get; }

        public int createdBy { set; get; }

        public string createdOn { set; get; }

        public int updatedBy { set; get; }

        public string updatedOn { set; get; }

        public bool active { set; get; }

        public bool deleted { set; get; }
    }




    public class LayoutInfo
    {
        public string id { set; get; }

        public string fileNo { set; get; }

        public string name { set; get; }

        public string updatedon { set; get; }

        public string createdon { set; get; }

        public ResultSearchCriteriaCoreType coreType { set; get; }

        public string updatedby { set; get; }

        public string size { set; get; }

        public SaveResultStatus status { set; get; }

        public string createdby { set; get; }

        public string versionno { set; get; }

        public string projectname { set; get; }

        public string projectinfo { set; get; }

        public bool isletest { set; get; }

        public bool shared { set; get; }

        public bool sharing { set; get; }

        public bool isImportant { set; get; }

        public string fileExt { set; get; }

        public bool filelock { set; get; }

        public string fileicon { set; get; }

        public int unreadcount { set; get; }

        public bool isCollection { set; get; }

        public int milisecond { set; get; }

        public bool previewavailable { set; get; }

        public bool isowner { set; get; }

        public bool supportpdf { set; get; }

        public bool unreadItem { set; get; }

        public bool hasViewPermission { set; get; }

        public bool isActFileLatest { set; get; }

        public bool isEditable { set; get; }

        public bool canDelete { set; get; }

        public bool canEditStatus { set; get; }

        public bool hasStatusClosed { set; get; }

        public ResultSearchCriteriaType type { set; get; }

        //public string id { set; get; }    

        //public string name { set; get; }
        //public string number { set; get; }

        //public ResultSearchCriteriaType type { set; get; }
        //public ResultSearchCriteriaStatus status { set; get; }

        //public string islatest { set; get; }
        public string description { set; get; }
        //public string active { set; get; }
        //public string deleted { set; get; }

        public string statusId { set; get; }
        public string statusname { set; get; }
        public string typeId { set; get; }
        public string typename { set; get; }

        //public string versionNo { set; get; }
        public string layoutId { set; get; }

    }
}
