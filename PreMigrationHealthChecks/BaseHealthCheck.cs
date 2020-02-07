using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core;
using Umbraco.Web.HealthCheck;

namespace PreMigrationHealthChecks
{
    public abstract class BaseHealthCheck : HealthCheck
    {
        protected DatabaseContext _databaseContext;
        protected abstract IEnumerable<BaseSqlCheck> Checks { get; }

        public BaseHealthCheck(HealthCheckContext healthCheckContext) : base(healthCheckContext)
        {
            _databaseContext = healthCheckContext.ApplicationContext.DatabaseContext;
        }

        public override HealthCheckStatus ExecuteAction(HealthCheckAction action)
        {
            var check = Checks.FirstOrDefault(c => c.Key == action.Alias);

            if (check == null)
                return new HealthCheckStatus("Unknown check") { ResultType = StatusResultType.Error };

            check.Fix(_databaseContext.Database);

            return new HealthCheckStatus("Done") { ResultType = StatusResultType.Success };
        }

        public override IEnumerable<HealthCheckStatus> GetStatus()
        {
            bool success = true;

            foreach (var check in Checks)
            {
                BaseSqlCheck.Status status = check.GetStatus(_databaseContext.Database);
                if (status != null)
                {
                    success = false;

                    yield return new HealthCheckStatus(status.Message)
                    {
                        Description = status.Description,
                        ResultType = StatusResultType.Error,
                        Actions = status.CanFix
                            ? new HealthCheckAction[] { new HealthCheckAction(check.Key, Id) { Name = "Fix", Description = status.FixDescription } }
                            : Array.Empty<HealthCheckAction>()
                    };
                }
            }

            if (success)
            {
                yield return new HealthCheckStatus("OK") { ResultType = StatusResultType.Success };
            }
        }
    }
}
