using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Web.HealthCheck;

namespace PreMigrationHealthChecks.HealthChecks
{
    [HealthCheck(_id, "Foreign keys", Description = "Checks for violations of foreign keys which will be created during migration", Group = "Migration")]
    public class ForeignKeyViolationChecks : BaseHealthCheck
    {
        private const string _id = "5343F742-3362-4FBD-A670-FCFB19AE6611";
        protected override IEnumerable<BaseSqlCheck> Checks => _checks;

        public ForeignKeyViolationChecks(HealthCheckContext healthCheckContext) : base(healthCheckContext)
        {
        }

        private static IEnumerable<BaseSqlCheck> _checks = new List<BaseSqlCheck> {
            new ForeignKeyViolationCheck("umbracoNode", "id",              "cmsContentType2ContentType", "childContentTypeId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "cmsContentType2ContentType", "parentContentTypeId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "cmsContentType", "nodeId"),
            new ForeignKeyViolationCheck("cmsContentType", "nodeId",       "cmsContentTypeAllowedContentType", "AllowedId"),
            new ForeignKeyViolationCheck("cmsContentType", "nodeId",       "cmsContentTypeAllowedContentType", "Id"),
            new ForeignKeyViolationCheck("cmsDictionary", "id",            "cmsDictionary", "parent"),
            new ForeignKeyViolationCheck("cmsContentType", "nodeId",       "cmsDocumentType", "contentTypeNodeId"),            
            new ForeignKeyViolationCheck("umbracoNode", "id",              "cmsDocumentType", "contentTypeNodeId"),
            new ForeignKeyViolationCheck("cmsTemplate", "nodeId",          "cmsDocumentType", "templateNodeId"),
            new ForeignKeyViolationCheck("cmsDictionary", "id",            "cmsLanguageText", "UniqueId"),
            new ForeignKeyViolationCheck("umbracoLanguage", "id",          "cmsLanguageText", "languageId"),
            new ForeignKeyViolationCheck("cmsMacro", "id",                 "cmsMacroProperty", "macro"),
            new ForeignKeyViolationCheck("cmsMember", "nodeId",            "cmsMember2MemberGroup", "Member"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "cmsMember2MemberGroup", "MemberGroup"),
            new ForeignKeyViolationCheck("cmsContent", "nodeId",           "cmsMember", "nodeId"),
            new ForeignKeyViolationCheck("cmsContentType", "nodeId",       "cmsMemberType", "NodeId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "cmsMemberType", "NodeId"),
            new ForeignKeyViolationCheck("cmsContentType", "nodeId",       "cmsPropertyType", "contentTypeId"),
            new ForeignKeyViolationCheck("cmsPropertyTypeGroup", "id",     "cmsPropertyType", "propertyTypeGroupId"),
            new ForeignKeyViolationCheck("cmsDataType", "nodeId",          "cmsPropertyType", "dataTypeId"),
            new ForeignKeyViolationCheck("cmsContentType", "nodeId",       "cmsPropertyTypeGroup", "contenttypeNodeId"),
            new ForeignKeyViolationCheck("cmsContent", "nodeId",           "cmsTagRelationship", "nodeId"),
            new ForeignKeyViolationCheck("cmsPropertyType", "id",          "cmsTagRelationship", "propertyTypeId"),
            new ForeignKeyViolationCheck("cmsTags", "id",                  "cmsTagRelationship", "tagId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "cmsTemplate", "nodeId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "umbracoAccess", "loginNodeId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "umbracoAccess", "noAccessNodeId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "umbracoAccess", "nodeId"),
            new ForeignKeyViolationCheck("umbracoAccess", "id",            "umbracoAccessRule", "accessId"),
            new ForeignKeyViolationCheck("cmsContentType", "nodeId",       "cmsContent", "contentType"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "cmsContent", "nodeId"),
            new ForeignKeyViolationCheck("cmsContent", "nodeId",           "cmsContentVersion", "contentId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "cmsContentVersion", "contentId"),
            //new ForeignKeyViolationCheck("umbracoUser", "id",              "cmsDocument", "documentUser"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "cmsDataType", "nodeId"),
            new ForeignKeyViolationCheck("cmsContent", "nodeId",           "cmsDocument", "nodeId"),
            new ForeignKeyViolationCheck("cmsContentVersion", "VersionId", "cmsDocument", "versionId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "umbracoDomains", "domainRootStructureID"),
            new ForeignKeyViolationCheck("umbracoUser", "id",              "umbracoLog", "userId"),
            new ForeignKeyViolationCheck("cmsContentVersion", "VersionId", "cmsMedia", "versionId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "umbracoNode", "parentId"),
            //new ForeignKeyViolationCheck("umbracoUser", "id",              "umbracoNode", "nodeUser"),
            new ForeignKeyViolationCheck("cmsContentVersion", "VersionId", "cmsPropertyData", "versionId"),
            new ForeignKeyViolationCheck("cmsPropertyType", "id",          "cmsPropertyData", "propertyTypeId"),
            new ForeignKeyViolationCheck("umbracoNode", "uniqueID",        "umbracoRedirectUrl", "contentKey"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "umbracoRelation", "childId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "umbracoRelation", "parentId"),
            new ForeignKeyViolationCheck("umbracoRelationType", "id",      "umbracoRelation", "relType"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "umbracoUser2NodeNotify", "nodeId"),
            new ForeignKeyViolationCheck("umbracoUser", "id",              "umbracoUser2NodeNotify", "userId"),
            new ForeignKeyViolationCheck("umbracoUser", "id",              "umbracoUser2UserGroup", "userId"),
            new ForeignKeyViolationCheck("umbracoUserGroup", "id",         "umbracoUser2UserGroup", "userGroupId"),
            new ForeignKeyViolationCheck("umbracoUserGroup", "id",         "umbracoUserGroup2App", "userGroupId"),
            new ForeignKeyViolationCheck("umbracoUserGroup", "id",         "umbracoUserGroup2NodePermission", "userGroupId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "umbracoUserGroup2NodePermission", "nodeId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "umbracoUserGroup", "startContentId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "umbracoUserGroup", "startMediaId"),
            new ForeignKeyViolationCheck("umbracoUser", "id",              "umbracoUserLogin", "userId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "umbracoUserStartNode", "startNode"),
            new ForeignKeyViolationCheck("umbracoUser", "id",              "umbracoUserStartNode", "userId"),

            new SqlCheck
            {
                Key = "cmsDocument.templateId",
                TestQuery = "SELECT COUNT(*) FROM cmsDocument WHERE templateId NOT IN (SELECT nodeid FROM cmsTemplate)",
                FixQuery = "UPDATE cmsDocument SET templateId = NULL WHERE templateId NOT IN (SELECT nodeid FROM cmsTemplate)",
                ErrorMessage = "Invalid templateId",
                ErrorDescription = "cmsDocument contains {0} rows with invalid templateId",
                FixDescription = "Set invalid template IDs to NULL"
            },
        };
    }
}
