using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.JWT
{
    public class JWTTokenManager:IJWTTokenManager
    {
        private readonly IConfiguration _Configuration;
        public JWTTokenManager(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public string GetUserToken(long Id, string UserName)
        {
            try
            {
                if (Id == 0 || string.IsNullOrWhiteSpace(UserName))
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

        public string GetUserToken(long Id, string UserName, string Role)
        {
            try
            {
                if (Id == 0 || string.IsNullOrWhiteSpace(UserName))
                {
                    return null;
                }

                var Claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                    new Claim(ClaimTypes.Name, UserName),
                    new Claim(ClaimTypes.Role, Role)
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

        public string GetUserToken(long Id, string UserName, IList<string> Roles)
        {
            try
            {
                if (Id == 0 || string.IsNullOrWhiteSpace(UserName))
                {
                    return null;
                }

                var Claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                    new Claim(ClaimTypes.Name, UserName),
                };
                if (Roles != null
                    && Roles.Count > 0)
                {
                    foreach (var item in Roles)
                    {
                        Claims.Add(new Claim(ClaimTypes.Role, item));
                    }
                }
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
