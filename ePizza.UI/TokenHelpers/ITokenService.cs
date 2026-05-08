namespace ePizza.UI.TokenHelpers
{
    public interface ITokenService
    {
        void SetToken(string token);

        void SetRefreshToken(string refreshToken);

        string GetToken();
    }


    

}
