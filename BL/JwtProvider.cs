namespace WebAuth.BL;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    
    public string GenerateJwtToken(string value)
    {
        if (_options.SecretKey != null)
        {
            Claim[] claims = [new("username", value)];
            
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), SecurityAlgorithms.HmacSha256);
            
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddHours(_options.ExpiresHours));
            
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }
        else
        {
            throw new UnauthorizedAccessException();
        }
    }
}

public class JwtOptions
{
    public string? SecretKey { get; set; }
    public int ExpiresHours { get; set; }
}