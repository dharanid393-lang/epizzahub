namespace ePizza.UI.TokenHelpers
{

    //using this token handler to add token in each api call
    public class TokenHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        public TokenHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string accessToken = _tokenService.GetToken();

            // TODO : to check if token is expired, then call refresh token endpoint at this layer

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }

}
