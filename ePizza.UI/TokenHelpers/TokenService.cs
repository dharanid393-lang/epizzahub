namespace ePizza.UI.TokenHelpers
{
    public class TokenService : ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string GetToken()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("Token") ?? string.Empty;
        }
        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Session.SetString("Token", token);
        }
        public void SetRefreshToken(string refreshToken)
        {
            _httpContextAccessor.HttpContext?.Session.SetString("RefreshToken", refreshToken);
            // if store it in the session it will be expired , so store the refresh token in the cookies
        }
    }
}
