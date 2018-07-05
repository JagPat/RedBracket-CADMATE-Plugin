using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadPlugIn
{
    /// <summary>
    /// Class to handle the save service response.
    /// </summary>
    public class SaveResult
    {
        public string msg { get; set; }
        public string message { get; set; }

        public string dataofdata { get; set; }
    }

    /// <summary>
    /// Handles the data response of the data.
    /// </summary>
    public class DataofData
    {
        public string id { get; set; }

        public string fileNo { get; set; }

        public string username { get; set; }

        public string name { get; set; }

        public string thumbnail { get; set; }

        public string projectname { get; set; }

        public string created0n { get; set; }

        public string createdOn { get; set; }

        public SaveResultUserDetails updatedBy { get; set; }

        public string updatedon { get; set; }

        public SaveResultCoreType coreType { get; set; }

        public SaveResultPermissions permission { get; set; }

        public string updatedby { get; set; }

        public string size { get; set; }

        public double originalSize { get; set; }

        public string originalFileId { get; set; }

        public string createdOnString { get; set; }

        public SaveResultStatus status { get; set; }

        public string resource { get; set; }

        public int resourcePk { get; set; }

        public string timediff { get; set; }

        public string createdby { get; set; }

        public string versionno { get; set; }

        public string versioncount { get; set; }

        public bool isletest { get; set; }

        public string downloadlink { get; set; }

        public bool shared { get; set; }

        public bool sharing { get; set; }

        public bool isImportant { get; set; }

        public string fileExt { get; set; }

        public bool filelock { get; set; }

        public string fileicon { get; set; }

        public string totalversioncount { get; set; }

        public string filePath { get; set; }

        public string fileNameWithoutExt { get; set; }

        public int unreadcount { get; set; }

        public string lastUpdatedOn { get; set; }

        public bool isCollection { get; set; }

        public long milisecond { get; set; }

        public bool previewavailable { get; set; }

        public bool isowner { get; set; }

        public bool supportpdf { get; set; }

        public bool unreadItem { get; set; }

        public bool hasViewPermission { get; set; }

        public bool isActFileLatest { get; set; }

        public string sharableLink { get; set; }

        public SaveResultOwnPermissions ownPermission { get; set; }

        public bool isEditable { get; set; }

        public bool canDelete { get; set; }

        public bool canEditStatus { get; set; }

        public bool hasStatusClosed { get; set; }

        public string creatorName { get; set; }

        public bool isAssociate { get; set; }

        public ResultSearchCriteriaType type { set; get; }

        public string description { get; set; }
        
    }

    public class SaveResultOwnPermissions
    {
        public SaveResultOwnPermissionsMap map { get; set; }
    }

    public class SaveResultOwnPermissionsMap
    {
        public bool VIEW_OWN_FILE { get; set; }

        public bool EDIT_OWN_FILE { get; set; }
    }

    public class SaveResultStatus
    {
        public int id { get; set; }

        public string statusname { get; set; }

        public string type { get; set; }

        public string iscustom { get; set; }

        public int companyid { get; set; }

        public SaveResultCoreType coretype { get; set; }

        public int createdBy { get; set; }

        public string createdOn { get; set; }

        public int updatedBy { get; set; }

        public string updatedOn { get; set; }

        public bool active { get; set; }

        public bool deleted { get; set; }
    }

    public class SaveResultPermissions
    {
        public SaveResultPermissionsMap map { get; set; }
    }

    public class SaveResultPermissionsMap
    {
        public bool ADD_FILE { get; set; }

        public bool CHANGE_FILE_STATUS { get; set; }

        public bool SHARE_THIS { get; set; }

        public bool VIEW_FILE { get; set; }

        public bool CREATE_NEW_FILE_TYPE { get; set; }

        public bool DELETE_FOLDER { get; set; }

        public bool SHARE_FILE { get; set; }

        public bool DOWNLOAD_NATIVE_FILE { get; set; }

        public bool DELETE_OWN_FILE { get; set; }

        public bool EDIT_FILE { get; set; }

        public bool SEND_TO_KNOWLEDGE_BASE_FILE { get; set; }

        public bool EDIT_OWN_FILE { get; set; }

        public bool MOVE_FILE { get; set; }

        public bool EDIT_FOLDER { get; set; }

        public bool CREATE_FOLDER { get; set; }

        public bool DELETE_OTHERS_FILE { get; set; }

        public bool VIEW_OWN_FILE { get; set; }

        public bool EDIT_OTHERS_FILE { get; set; }

        public bool VIEW_OTHERS_FILE { get; set; }

        public bool CREATE_NEW_FILE_STATUS { get; set; }

        public bool CHANGE_FILE_PERMISSION { get; set; }

        public bool VIEW_FILE_HISTORY { get; set; }

        public bool CHANGE_STATUS_OF_OTHERS_FILE { get; set; }
    }

    public class SaveResultCoreType
    {
        public int id { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public bool iscustom { get; set; }

        public int companyid { get; set; }

        public string master { get; set; }

        public int createdBy { get; set; }

        public string createdOn { get; set; }

        public int updatedBy { get; set; }

        public string updatedOn { get; set; }

        public bool active { get; set; }

        public bool deleted { get; set; }

        public string colorId { get; set; }
    }

    public class SaveResultUserDetails
    {
        public int id { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string password { get; set; }

        public int companyId { get; set; }

        public int isCompanyAdmin { get; set; }

        public string mobile { get; set; }

        public string email { get; set; }

        public int status { get; set; }

        public bool active { get; set; }

        public bool deleted { get; set; }
    }
}
