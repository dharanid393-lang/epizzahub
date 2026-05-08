using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ePizza.API.HealthCheck
{
    public class SecondApiHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return HealthCheckResult.Healthy("SecondApi is healthy");
        }
    }
}
