using System;
using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace NETSprinkler.ApiWorker
{
	public class DashboardNoAuthorizationFilter: IDashboardAuthorizationFilter
	{
		public DashboardNoAuthorizationFilter()
		{
		}

        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}

