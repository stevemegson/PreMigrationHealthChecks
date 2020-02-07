using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Persistence;
using Umbraco.Web.HealthCheck;

namespace PreMigrationHealthChecks
{
    public class SqlCheck : BaseSqlCheck
    {
        public string TestQuery { get; set; }
        public string FixQuery { get; set; }

        public string ErrorMessage { get; set; }
        public string ErrorDescription { get; set; }
        public string FixDescription { get; set; }

        public override Status GetStatus(Database db)
        {
            int result = db.ExecuteScalar<int>(TestQuery);
            if (result > 0)
            {
                return new Status
                {
                    Message = String.Format(ErrorMessage, result),
                    Description = String.Format(ErrorDescription, result),
                    FixDescription = String.Format(FixDescription, result),
                    CanFix = (FixQuery != null)
                };
            }

            return null;
        }

        public override void Fix(Database db)
        {
            db.Execute(FixQuery);
        }

    }
}
