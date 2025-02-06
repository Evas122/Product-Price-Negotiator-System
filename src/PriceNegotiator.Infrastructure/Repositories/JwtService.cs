using Microsoft.IdentityModel.Tokens;
using PriceNegotiator.Application.Common.Interfaces;
using PriceNegotiator.Application.Common.Options;
using PriceNegotiator.Domain.Entities.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PriceNegotiator.Infrastructure.Repositories;

public class JwtService : IJwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly IDateTimeProvider _dateTimeProvider;

    public JwtService(JwtSettings jwtSettings, IDateTimeProvider dateTimeProvider)
    {
        _jwtSettings = jwtSettings;
        _dateTimeProvider = dateTimeProvider;
    }
    public string GenerateJwtToken(User user)
    {
        var claims = GetClaims(user);

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private List<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        return claims;
    }
}