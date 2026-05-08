namespace ePizza.Application.DTOs.Request
{
    public class RefreshTokenRequestDto
    {
        public string AcessToken { get; set; } = default!;

        public string RefreshToken { get; set; } = default!;
    }
}
