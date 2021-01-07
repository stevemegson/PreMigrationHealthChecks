using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web.HealthCheck;

namespace PreMigrationHealthChecks.HealthChecks
{
    [HealthCheck(_id, "Duplicate data", Description = "Checks for violations of unique indexes which will be created during migration", Group = "Migration")]
    public class DuplicateChecks : BaseHealthCheck
    {
        private const string _id = "55064ff2-f041-4df4-bf9e-f1dcd25d1bac";
        protected override IEnumerable<BaseSqlCheck> Checks => _checks;

        public DuplicateChecks(HealthCheckContext healthCheckContext) : base(healthCheckContext)
        {
        }

        private static IEnumerable<SqlCheck> _checks = new List<SqlCheck> {
            new SqlCheck
            {
                Key = "cmsPropertyData",
                TestQuery = "SELECT COUNT(*) FROM cmsPropertyData WHERE id NOT IN (SELECT MIN(id) FROM cmsPropertyData GROUP BY contentNodeId, versionId, propertytypeid HAVING MIN(id) IS NOT NULL)",
                FixQuery = "DELETE FROM cmsPropertyData WHERE id NOT IN (SELECT MIN(id) FROM cmsPropertyData GROUP BY contentNodeId, versionId, propertytypeid HAVING MIN(id) IS NOT NULL)",
                ErrorMessage = "Duplicate cmsPropertyData",
                ErrorDescription = "cmsPropertyData contains {0} rows for duplicate versionId/propertyTypeId",
                FixDescription = "Delete {0} rows in cmsPropertyData"
            },

            new SqlCheck
            {
                Key = "cmsTags",
                TestQuery = "SELECT COUNT(*) FROM cmsTags WHERE id NOT IN (SELECT MIN(id) FROM cmsTags GROUP BY [group],[tag] HAVING MIN(id) IS NOT NULL)",
                FixQuery = "DELETE FROM cmsTags WHERE id NOT IN (SELECT MIN(id) FROM cmsTags GROUP BY [group],[tag] HAVING MIN(id) IS NOT NULL)",
                ErrorMessage = "Duplicate cmsTags",
                ErrorDescription = "cmsTags contains {0} rows for duplicate group/tag",
                FixDescription = "Delete {0} rows in cmsTags"
            },

            new SqlCheck
            {
                Key = "umbracoAccessRule",
                TestQuery = "SELECT COUNT(*) FROM umbracoAccessRule WHERE id NOT IN (SELECT MIN(id) FROM umbracoAccessRule GROUP BY [ruleValue], [accessId] HAVING MIN(id) IS NOT NULL)",
                FixQuery = "DELETE FROM umbracoAccessRule WHERE id NOT IN (SELECT MIN(id) FROM umbracoAccessRule GROUP BY [ruleValue], [accessId] HAVING MIN(id) IS NOT NULL)",
                ErrorMessage = "Duplicate umbracoAccessRules",
                ErrorDescription = "umbracoAccessRule contains {0} rows for duplicate accessId/ruleValue - content/memberGroup",
                FixDescription = "Delete {0} rows in umbracoAccessRule"
            }
        };
    }
}
