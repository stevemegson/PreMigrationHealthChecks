using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;

namespace PreMigrationHealthChecks
{
    public class CollationCheck : BaseSqlCheck
    {
        public override void Fix(Database db)
        {
            throw new NotImplementedException();
        }

        public override Status GetStatus(Database db)
        {
            var tableCollations = db.Fetch<string>(@"
SELECT DISTINCT collation_name FROM sys.columns
INNER JOIN sys.tables ON tables.object_id = columns.object_id
WHERE collation_name IS NOT NULL
AND system_type_id=231");

            var databaseCollation = db.ExecuteScalar<string>("SELECT CONVERT (varchar(256), DATABASEPROPERTYEX(DB_NAME(),'collation'))");

            if (tableCollations.Count > 1)
            {
                return new Status
                {
                    CanFix = false,
                    Message = "Multiple collations",
                    Description = "Database contains columns with multiple collations"
                };
            }
            else if (databaseCollation != tableCollations.First())
            {
                return new Status
                {
                    CanFix = false,
                    Message = "Multiple collations",
                    Description = "Database collation does not match column collations"
                };
            }

            return null;
        }
    }
}
