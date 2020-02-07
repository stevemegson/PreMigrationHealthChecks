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
            new ForeignKeyViolationCheck("cmsContent", "nodeId",           "cmsContentVersion", "contentId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "cmsContentVersion", "contentId"),
            new ForeignKeyViolationCheck("cmsContentVersion", "VersionId", "cmsPropertyData", "versionId"),
            new ForeignKeyViolationCheck("cmsPropertyType", "id",          "cmsPropertyData", "propertyTypeId"),
            new ForeignKeyViolationCheck("umbracoNode", "id",              "cmsDataType", "nodeId"),
            new ForeignKeyViolationCheck("cmsContentVersion", "VersionId", "cmsMedia", "versionId"),

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
