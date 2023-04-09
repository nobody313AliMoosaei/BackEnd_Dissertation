using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dissertation_Project.Core.Utlities.JWT
{
    public class JWTBearer : IJWTBearer
    {
        private readonly IConfiguration _Configuration;
        public JWTBearer(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public string GetUserToken(ulong Id, string UserName)
        {
            try
            {
                if(Id==0 || string.IsNullOrWhiteSpace(UserName))
                {
                    return null;
                }

                var Claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                    new Claim(ClaimTypes.Name, UserName)
                };
                
                string Key = _Configuration["JWTConfiguration:key"];
                var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
                var Credential = new SigningCredentials(SecretKey, SecurityAlgorithms.HmacSha256);
               
                var Token =
                new JwtSecurityToken(
                    issuer: _Configuration["JWTConfiguration:issuer"],
                    audience: _Configuration["JWTConfiguration:audience"],
                    expires: DateTime.Now.AddDays(10),
                    claims: Claims,
                    signingCredentials: Credential
                    );

                string jwt_Token = new JwtSecurityTokenHandler().WriteToken(Token);
                return jwt_Token;
            }
            catch
            {
                return null;
            }
        }
    }
}
