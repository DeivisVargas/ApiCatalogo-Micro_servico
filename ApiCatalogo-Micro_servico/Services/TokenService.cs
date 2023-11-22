using ApiCatalogo_Micro_servico.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiCatalogo_Micro_servico.Services
{
    public class TokenService : ITokenService
    {
        public string GetToken(string Key, string issuer, string audience, UserModel user)
        {

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                 new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            };


            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    issuer : issuer ,
                    audience : audience,
                    claims   : claims,
                    expires  : DateTime.Now.AddMinutes(120),
                    signingCredentials : credentials
                );

            var tokenHandler    = new JwtSecurityTokenHandler();
            var strinToken      = tokenHandler.WriteToken(token);
            return strinToken;
        }
    }
}
