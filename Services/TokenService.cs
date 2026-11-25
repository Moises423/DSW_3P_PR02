using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DSW_3P_PR02.Dtos;
using Microsoft.IdentityModel.Tokens;

namespace DSW_3P_PR02.Services;

public sealed class TokenService(JwtSettings settings) : ITokenService
{
    public LoginResponse GenerarToken(string usuario)
    {
        var ahora = DateTimeOffset.UtcNow;
        var expira = ahora.AddMinutes(settings.ExpireMinutes);
        var credencialesFirma = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
            SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario),
            new Claim(ClaimTypes.Name, usuario),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var jwt = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            notBefore: ahora.UtcDateTime,
            expires: expira.UtcDateTime,
            signingCredentials: credencialesFirma);

        return new LoginResponse(
            new JwtSecurityTokenHandler().WriteToken(jwt),
            "Bearer",
            expira);
    }
}
