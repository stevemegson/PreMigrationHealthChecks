using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Web.HealthCheck;

namespace PreMigrationHealthChecks
{
    public abstract class BaseSqlCheck
    {
        public string Key { get; set; }

        public abstract Status GetStatus(Database db);
        public abstract void Fix(Database db);

        public class Status
        {
            public string Message { get; set; }
            public string Description { get; set; }
            public string FixDescription { get; set; }
            public bool CanFix { get; set; }
        }
    }
}
