using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using API.Interface;
using API.Entities;


namespace API.Service
{
    //config : lay cac gia tri dang ki trong configuration
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(AppUser user){
            var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings");

              if (tokenKey.Length < 64)
                throw new Exception("Your TokenKey needs to be at least 64 characters long.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
            
        }
    }
}