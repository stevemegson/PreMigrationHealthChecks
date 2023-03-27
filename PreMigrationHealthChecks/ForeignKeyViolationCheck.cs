using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core.Persistence;
using Umbraco.Web.HealthCheck;

namespace PreMigrationHealthChecks
{
    public class ForeignKeyViolationCheck : BaseSqlCheck
    {
        public string ParentTable { get; set; }
        public string ParentColumn { get; set; }
        public string ChildTable { get; set; }
        public string ChildColumn { get; set; }

        public ForeignKeyViolationCheck(string parentTable, string parentColumn, string childTable, string childColumn)
        {
            ParentTable = parentTable;
            ParentColumn = parentColumn;
            ChildTable = childTable;
            ChildColumn = childColumn;

            Key = $"{ParentTable}.{ParentColumn} -> {ChildTable}.{ChildColumn}";
        }

        public override Status GetStatus(Database db)
        {
            string query = $"SELECT COUNT(*) FROM {ChildTable} WHERE {ChildColumn} NOT IN (SELECT {ParentColumn} FROM {ParentTable})";
            int count = db.ExecuteScalar<int>(query);
            if (count > 0)
            {
                return new Status
                {
                    CanFix = true,
                    Message = "Foreign key violation in " + ChildTable,
                    Description = $"{ChildTable} contains {count} {ChildColumn} values which don't exist in {ParentTable}.{ParentColumn}.<div class='umb-healthcheck-group__details-status-action'>{query.Replace("COUNT(*)", "TOP 100 *")}</div>",
                    FixDescription = $"Delete {count} rows from {ChildTable}"
                };
            }

            return null;
        }

        public override void Fix(Database db)
        {
            db.Execute($"DELETE FROM {ChildTable} WHERE {ChildColumn} NOT IN (SELECT {ParentColumn} FROM {ParentTable})");
        }


    }

}