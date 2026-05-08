using ePizza.Application.DTOs.Request;
using ePizza.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizza.Application.Contracts
{
    public interface ITokenGeneratorService
    {
        Task<TokenResponseDto> GenerateTokenAsync(string userName, string password);


        Task<TokenResponseDto> GenerateRefreshToken(RefreshTokenRequestDto requestTokenDto);
    }
}
