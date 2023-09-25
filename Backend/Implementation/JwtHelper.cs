using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

public class JwtHelper
{
    private readonly string _key;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtHelper(string key, string issuer, string audience)
    {
        _key = key;
        _issuer = issuer;
        _audience = audience;
    }

    public bool VerifyToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return principal.Identity.IsAuthenticated;
        }
        catch
        {
            return false;
        }
    }

    private TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = _issuer,
            ValidAudience = _audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key))
        };
    }

    public Claim GetClaim(string token, string claimType)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);
            return claim;
        }
        catch
        {
            return null;
        }
    }
}
