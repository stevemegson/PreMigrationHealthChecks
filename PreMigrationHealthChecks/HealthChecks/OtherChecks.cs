using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web.HealthCheck;

namespace PreMigrationHealthChecks.HealthChecks
{
    [HealthCheck(_id, "Other checks", Description = "Checks for other invalid data", Group = "Migration")]
    public class OtherChecks : BaseHealthCheck
    {
        private const string _id = "8596a64a-d44d-4508-bff1-7c0d64d139d5";
        protected override IEnumerable<BaseSqlCheck> Checks => _checks;

        public OtherChecks(HealthCheckContext healthCheckContext) : base(healthCheckContext)
        {
        }

        private static IEnumerable<BaseSqlCheck> _checks = new List<BaseSqlCheck> {
            new SqlCheck
            {
                Key = "no-newest-version",
                TestQuery = "SELECT COUNT(DISTINCT nodeId) FROM cmsdocument WHERE nodeid NOT IN (SELECT nodeid FROM cmsdocument WHERE newest=1)",
                FixQuery = @"
UPDATE cmsDocument 
SET newest=1
FROM cmsDocument
INNER JOIN (
	SELECT versionId, ROW_NUMBER() OVER (PARTITION BY nodeId ORDER BY updateDate DESC) as [rowNumber]
	FROM cmsdocument
	WHERE nodeid NOT IN (SELECT nodeid FROM cmsdocument WHERE newest=1)
	) sub ON sub.versionId = cmsDocument.versionId
WHERE rowNumber=1",
                ErrorMessage = "Content with no newest version",
                ErrorDescription = "{0} documents have no newest version",
                FixDescription = "Mark most recent versions as newest"
            },

            new SqlCheck
            {
                Key = "duplicate-newest-version",
                TestQuery = "SELECT COUNT(*) FROM (SELECT nodeId FROM cmsDocument WHERE newest=1 GROUP BY nodeId HAVING COUNT(*) > 1) sub",
                FixQuery = @"
UPDATE cmsDocument 
SET newest=0
FROM cmsDocument
INNER JOIN (
	SELECT versionId, published, updateDate, ROW_NUMBER() OVER (PARTITION BY nodeId ORDER BY updateDate DESC) as [rowNumber]
	FROM cmsdocument
	WHERE newest=1
	) sub ON sub.versionId = cmsDocument.versionId
WHERE rowNumber>1",
                ErrorMessage = "Content with duplicate newest versions",
                ErrorDescription = "{0} documents have more than one newest version",
                FixDescription = "Mark only most recent versions as newest"
            },

            new SqlCheck
            {
                Key = "missing-prevalue-aliases",
                TestQuery = "SELECT COUNT(*) FROM cmsDataTypePreValues WHERE alias='' OR alias IS NULL",
                FixQuery = @"
UPDATE cmsDataTypePreValues
SET alias = newAlias
FROM cmsDataTypePreValues
JOIN (
	SELECT id, newAlias = ROW_NUMBER() OVER (PARTITION BY datatypenodeid ORDER BY sortorder) - 1
	FROM cmsDataTypePreValues
	WHERE alias='' OR alias IS NULL
	) aliases ON aliases.id = cmsDataTypePreValues.id",
                ErrorMessage = "Missing prevalue aliases",
                ErrorDescription = "cmsDataTypePreValues contains {0} blank or NULL aliases",
                FixDescription = "Generate placeholder aliases"
            },

            new SqlCheck
            {
                Key = "blank-umbracoNode-text",
                TestQuery = "SELECT COUNT(*) FROM umbracoNode WHERE [text]=''",
                FixQuery = "UPDATE umbracoNode SET [text] = '(blank)' WHERE [text]=''",
                ErrorMessage = "Blank umbracoNode text",
                ErrorDescription = "umbracoNode contains {0} blank text values",
                FixDescription = "Set to '(blank)'"
            },

            new SqlCheck
            {
                Key = "blank-cmsDocument-text",
                TestQuery = "SELECT COUNT(*) FROM cmsDocument WHERE [text]=''",
                FixQuery = "UPDATE cmsDocument SET [text] = '(blank)' WHERE [text]=''",
                ErrorMessage = "Blank cmsDocument text",
                ErrorDescription = "cmsDocument contains {0} blank text values",
                FixDescription = "Set to '(blank)'"
            },

            new CollationCheck()
        };
    }
}
