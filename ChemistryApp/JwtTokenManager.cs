using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChemistryApp.Models;
using Microsoft.IdentityModel.Tokens;

namespace ChemistryApp;

public class JwtTokenManager : IJwtTokenManager
{
    private readonly IConfiguration _configuration;
    private readonly ChemistryAppContext _entities;

    public JwtTokenManager(IConfiguration configuration)
    {
        _configuration = configuration;
        _entities = new ChemistryAppContext();
    }
    public string? Authenticate(string login, string password)
    {
        var user = _entities.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
        if (user == null)
        {
            return null;
        }

        var key = _configuration.GetValue<string>("JwtConfig:Key");
        var keyBytes = Encoding.ASCII.GetBytes(key);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = new SigningCredentials
            (
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}