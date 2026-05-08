namespace ePizza.UI.Models.Responses
{
    public class TokenResponseModelDto
    {
        public string AccessToken { get; set; } = default!;

        public string RefreshToken { get; set; } = default!;
    }
}
