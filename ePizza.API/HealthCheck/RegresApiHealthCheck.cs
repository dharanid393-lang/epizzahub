using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ePizza.API.HealthCheck
{
    //health check can  be used for monitoring database , services and all kind of end points
    //we need to inherit the interface of healthcheck
    // health check should not used to call heavy load API or functionality that will take more time
    public class RegresApiHealthCheck : IHealthCheck
    {
        private readonly HttpClient httpClient;

        public RegresApiHealthCheck(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await httpClient.GetAsync("https://reqres.in/api/users?page=2");

                if (response.IsSuccessStatusCode)
                    return HealthCheckResult.Healthy("Regres API is healthy");

                //to say that something issue on the API
                return HealthCheckResult.Degraded($"Regres API returned status : {response.StatusCode}");
            }
            catch (Exception ex)
            {
                // api url itself not loading
                return HealthCheckResult.Unhealthy($"Regres API is unreachable", ex);
            }
        }
    }
}
