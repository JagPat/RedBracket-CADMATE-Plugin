using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections;

namespace RBAutocadPlugIn
{
    public class PLMObject
    {
        private String itemType;
        private String objectId;
        private String objectName;
        private String objectRevision;
        private String objectDescription;
        private String classification;
        private String objectProjectId;
        private String objectProjectName;
        private String objectOwnerCompany;
        private String objectCreatedById;
        private String objectCreatedOn;
        private String objectModifiedById;
        private String objectModifiedOn;
        private String objectRealtyId;
        private String objectSourceId;
        private String objectLayouts;
        private String objectDesktop;
        private String filePath;
        private String nativeFileId;
        private String nativeFileName;
        private String relatedId;

        private bool isFile;
        private String isRel;
        private bool isNew;
        private bool isNewStructure;
        private bool isroot;
        private String objectNumber;
        private String authoringTool;
        private Hashtable objectAttributes;

        private String objectState;
        private String objectGeneration;
        private String lockStatus;
        private String lockBy;
        private String objectStatus; 
       
        private bool isCreateNewRevision;
        private bool isManualVersion;


        public bool IsCreateNewRevision
        {
            get { return isCreateNewRevision; }
            set { isCreateNewRevision = value; }
        }
        public bool IsManualVersion
        {
            get { return isManualVersion; }
            set { isManualVersion = value; }
        }
        public bool IsRoot
        {
            get { return this.isroot; }
            set { this.isroot = value; }
        }
        public bool IsNewStructure
        {
            get { return this.isNewStructure; }
            set { this.isNewStructure = value; }
        }
        public bool IsNew
        {
            get { return this.isNew; }
            set { this.isNew = value; }
        }
        public String LockBy
        {
            get { return this.lockBy; }
            set { this.lockBy = value; }
        }
        public String RelatedId
        {
            get { return this.relatedId; }
            set { this.relatedId = value; }
        }
        public String ItemType
        {
            get { return this.itemType; }
            set { this.itemType = value; }
        }
        public String ObjectId
        {
            get { return this.objectId; }
            set { this.objectId = value; }
        }
        public String ObjectName
        {
            get { return this.objectName; }
            set { this.objectName = value; }
        }
        public String ObjectSourceId
        {
            get { return this.objectSourceId; }
            set { this.objectSourceId = value; }
        }
        public String ObjectLayouts
        {
            get { return this.objectLayouts; }
            set { this.objectLayouts = value; }
        }
        public String ObjectDesktop
        {
            get { return this.objectDesktop; }
            set { this.objectDesktop = value; }
        }
        public String ObjectRevision
        {
            get { return this.objectRevision; }
            set { this.objectRevision = value; }
        }
        public String ObjectDescription
        {
            get { return this.objectDescription; }
            set { this.objectDescription = value; }
        }
        public String Classification
        {
            get { return this.classification; }
            set { this.classification = value; }
        }
        public String FilePath
        {
            get { return this.filePath; }
            set { this.filePath = value; }
        }
        public String NativeFileId
        {
            get { return this.nativeFileId; }
            set { this.nativeFileId = value; }
        }
        public String NativeFileName
        {
            get { return this.nativeFileName; }
            set { this.nativeFileName = value; }
        }
        public bool IsFile
        {
            get { return this.isFile; }
            set { this.isFile = value; }
        }
        public String IsRel
        {
            get { return this.isRel; }
            set { this.isRel = value; }
        }
        public String ObjectNumber
        {
            get { return this.objectNumber; }
            set { this.objectNumber = value; }
        }
        public String ObjectProjectId
        {
            get { return this.objectProjectId; }
            set { this.objectProjectId = value; }
        }
        public String ObjectRealtyId
        {
            get { return this.objectRealtyId; }
            set { this.objectRealtyId = value; }
        }
        public String ObjectProjectName
        {
            get { return this.objectProjectName; }
            set { this.objectProjectName = value; }
        }
        public String AuthoringTool
        {
            get { return this.authoringTool; }
            set { this.authoringTool = value; }
        }
        public Hashtable ObjectAttributes
        {
            get { return this.objectAttributes; }
            set { this.objectAttributes = value; }
        }
        public String ObjectState
        {
            get { return this.objectState; }
            set { this.objectState = value; }
        }
        public String ObjectOwnerCompany
        {
            get { return this.objectOwnerCompany; }
            set { this.objectOwnerCompany = value; }
        }
        public String ObjectGeneration
        {
            get { return this.objectGeneration; }
            set { this.objectGeneration = value; }
        }
        public String ObjectModifiedOn
        {
            get { return this.objectModifiedOn; }
            set { this.objectModifiedOn = value; }
        }
        public String ObjectModifiedById
        {
            get { return this.objectModifiedById; }
            set { this.objectModifiedById = value; }
        }
        public String ObjectCreatedOn
        {
            get { return this.objectCreatedOn; }
            set { this.objectCreatedOn = value; }
        }
        public String ObjectCreatedById
        {
            get { return this.objectCreatedById; }
            set { this.objectCreatedById = value; }
        }
        public String LockStatus
        {
            get { return this.lockStatus; }
            set { this.lockStatus = value; }
        }
        public String ObjectStatus 
        {
            get { return this.objectStatus; }
            set { this.objectStatus = value; }
        }
       
        public bool canDelete { get; set; }
        public bool isowner { get; set; }
        public bool hasViewPermission { get; set; }
        public bool isActFileLatest { get; set; }
        public bool isEditable { get; set; }
        public bool canEditStatus { get; set; }
        public bool hasStatusClosed { get; set; }
        public bool isletest { get; set; }
        public String objectProjectNo { get; set; }
        public String objectType { get; set; }
        public String PreFix { get; set; }
        public String FolderID { get; set; }
        public String FolderPath { get; set; }

        public String LayoutInfo { get; set; }
        public System.Data.DataTable dtLayoutInfo { get; set; }
        public bool IsNewXref { get; set; }

        public String PK { get; set; }
        public String FK { get; set; }
        public String OldPK { get; set; }
        public String OldFK { get; set; }
        public bool IsSaved { get; set; }
        public String Oldfilepath { get; set; }
        public String OldStatus { get; set; }
        public String VersionType { get; set; }
        private List<Relationship> fromRelationships = new List<Relationship>();
        private List<Relationship> toRelationships = new List<Relationship>();
        public String projectManager { get; set; }
        public String originalFileID { get; set; }
        public String TempFilePath { get; set; }

        public List<Relationship> FromRelationships
        {
            get { return this.fromRelationships; }
            set { this.fromRelationships = value; }
        }
        public List<Relationship> ToRelationships
        {
            get { return this.toRelationships; }
            set { this.toRelationships = value; }
        }
        /*		public List<Relationship> getFromRelationships();
                public List<Relationship> getToRelationships();
                public List<Relationship> getRelationships();
        */
        public void addRelationship(Relationship rel)
        { }
        public void remove(Relationship rel)
        { }
        public void addFromRelationship(String relName, PLMObject fromObject)
        { }
        public void removeFromRelationship(String relName, PLMObject fromObject)
        { }
        public void addToRelationship(String relName, PLMObject toObject)
        { }
        public void removeToRelationship(String relName, PLMObject toObject)
        { }
    }
}
