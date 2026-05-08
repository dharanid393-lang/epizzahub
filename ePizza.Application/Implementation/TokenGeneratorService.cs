using AutoMapper.Configuration.Annotations;
using ePizza.Application.Contracts;
using ePizza.Application.DTOs.Request;
using ePizza.Application.DTOs.Response;
using ePizza.Application.Exceptions;
using ePizza.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ePizza.Application.Implementation
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        //private readonly IUserTokenService _userTokenService;
        //private readonly IMapper _mapper;

        public TokenGeneratorService(IConfiguration configuration,
            IUserService userService
            
         )
        {
            _configuration = configuration;
            _userService = userService;
            //_userTokenService = userTokenService;
            //this._mapper = mapper;
        }

        public async Task<TokenResponseDto> GenerateRefreshToken(RefreshTokenRequestDto requestTokenDto)
        {
            //check if an access token is valid or not
            var principal = GetTokenClaimPrincipal(requestTokenDto.AcessToken);
            if (principal == null)
                throw new Exception("The access token is invalid");

            //check if refresh token is not expired and it was created by correct user to create refresh token.

            //generate new access token

            //generate refresh token
            var emailAddress = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
            var userDetails = await _userService.GetUserDetailsAsync(emailAddress);
            return GenerateToken(userDetails);
        }

        public async  Task<TokenResponseDto> GenerateTokenAsync(string userName, string password)
        {
            //1. Validate user based on credentials ---UserService ---- User@123 User@gmail.com

            var userDetails = await _userService.GetUserDetailsAsync(userName);

            if (userDetails == null)

              throw new InvalidCredentialsException($"The email address {userName} doesn't exists in database");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, userDetails.Password);

            if (!isPasswordValid)
                throw new InvalidCredentialsException($"The password {password} doesn't match with {userName}");

            // 2. If credentials are valid, generate token

             return GenerateToken(userDetails);

           
            //3 . we have to preserve token in my UserToken
        }


        private TokenResponseDto GenerateToken(UserDomain userDomain)
        {
            string secretKey = _configuration["Jwt:Secret"]!;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor =
                new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity([

                         new Claim(ClaimTypes.Name,userDomain.Name),
                         new Claim(ClaimTypes.Email,userDomain.Email),
                         new Claim("IsAdmin","True"),
                         new Claim("UserId",userDomain.Id.ToString())
                        ]),
                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:TokenExpiryInMinutes"])),
                    SigningCredentials = credentials,
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"]
                };

            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenResponseDto()
            {
                AccessToken = token,
                RefreshToken = GenerateRefreshToken()
            };
            
        }

        private ClaimsPrincipal? GetTokenClaimPrincipal(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!);
            var validationParameter = GetTokenValidationParameters(key);

            
            return tokenHandler.ValidateToken(accessToken, validationParameter ,out _);
            
        }

        private TokenValidationParameters? GetTokenValidationParameters(byte[] key)
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"]!,
                ValidAudience = _configuration["Jwt:Audience"]!,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new Byte[64];

            using var range = RandomNumberGenerator.Create();   
            range.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
