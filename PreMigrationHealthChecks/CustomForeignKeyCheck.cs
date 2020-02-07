using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;

namespace PreMigrationHealthChecks
{
    public class CustomForeignKeyCheck : BaseSqlCheck
    {
        public override void Fix(Database db)
        {
            foreach(var k in GetCustomForeignKeys(db))
            {
                db.Execute($"ALTER TABLE [{k.Item1}] DROP CONSTRAINT [{k.Item2}]");
            }
        }

        public override Status GetStatus(Database db)
        {
            var toDelete = GetCustomForeignKeys(db);
            if ( toDelete.Any())
            {
                return new Status
                {
                    CanFix = true,
                    ResultType = Umbraco.Web.HealthCheck.StatusResultType.Warning,
                    Message = "Foreign keys referencing Umbraco tables exist on custom tables",
                    Description = String.Join(", ", toDelete.Select(k => k.Item2)),
                    FixDescription = "Delete these foreign keys"
                };
            }

            return null;
        }

        private IEnumerable<Tuple<string,string>> GetCustomForeignKeys(Database db)
        {
            var items = db.Fetch<dynamic>(@"
SELECT obj.name AS [name],
    tab1.name AS [table],
    tab2.name AS [referenced_table]
FROM sys.foreign_key_columns fkc
INNER JOIN sys.objects obj
    ON obj.object_id = fkc.constraint_object_id
INNER JOIN sys.tables tab1
    ON tab1.object_id = fkc.parent_object_id
INNER JOIN sys.schemas sch
    ON tab1.schema_id = sch.schema_id
INNER JOIN sys.tables tab2
    ON tab2.object_id = fkc.referenced_object_id
WHERE sch.name = (SELECT SCHEMA_NAME())");

            return items.Where(item => !_tables.Contains(item.table) && _tables.Contains(item.referenced_table))
                        .Select(item => Tuple.Create(item.table as string, item.name as string))
                        .ToArray();
        }

        private static HashSet<string> _tables = new HashSet<string> ( new[]
        {
            "cmsContent",
            "cmsContentType",
            "cmsContentType2ContentType",
            "cmsContentTypeAllowedContentType",
            "cmsContentVersion",
            "cmsContentXml",
            "cmsDataType",
            "cmsDataTypePreValues",
            "cmsDictionary",
            "cmsDocument",
            "cmsDocumentType",
            "cmsLanguageText",
            "cmsMacro",
            "cmsMacroProperty",
            "cmsMedia",
            "cmsMember",
            "cmsMember2MemberGroup",
            "cmsMemberType",
            "cmsPreviewXml",
            "cmsPropertyData",
            "cmsPropertyType",
            "cmsPropertyTypeGroup",
            "cmsTagRelationship",
            "cmsTags",
            "cmsTask",
            "cmsTaskType",
            "cmsTemplate",
            "umbracoAccess",
            "umbracoAccessRule",
            "umbracoAudit",
            "umbracoCacheInstruction",
            "umbracoConsent",
            "umbracoDomains",
            "umbracoExternalLogin",
            "umbracoLanguage",
            "umbracoLock",
            "umbracoLog",
            "umbracoMigration",
            "umbracoNode",
            "umbracoRedirectUrl",
            "umbracoRelation",
            "umbracoRelationType",
            "umbracoServer",
            "umbracoUser",
            "umbracoUser2NodeNotify",
            "umbracoUser2UserGroup",
            "umbracoUserGroup",
            "umbracoUserGroup2App",
            "umbracoUserGroup2NodePermission",
            "umbracoUserLogin",
            "umbracoUserStartNode",
        } );
    }
}
