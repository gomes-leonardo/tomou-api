using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tomou.Domain.Entities;
using Tomou.Domain.Security.Tokens;

namespace Tomou.Infrastructure.Security.Tokens;
internal class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly int _expirationMinutes;
    private readonly string _secret;
    public JwtTokenGenerator(int expirationMinutes, string secret)
    {
        _expirationMinutes = expirationMinutes;
        _secret = secret;
    }

    public string Generate(long userId, string name, string email, bool isCaregiver)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, name),
            new(ClaimTypes.Email, email),
            new("isCaregiver", isCaregiver.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            signingCredentials: creds,
            claims: claims
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
