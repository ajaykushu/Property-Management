using API.Authentication.Interfaces;
using DataEntity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utilities.Interface;

namespace API.Authentication
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly ICache _cache;
        public TokenGenerator(IConfiguration configuration, ICache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }
        public Claim[] GetClaims(ApplicationUser applicationuser, HashSet<string> features)
        {
            var jti = Guid.NewGuid().ToString().Replace("-", "");
            List<Claim> authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, applicationuser.UserName),
                new Claim(ClaimTypes.Email, applicationuser.Email),
                new Claim(ClaimTypes.GivenName,applicationuser.FirstName+" "+applicationuser.LastName+" "+applicationuser.Suffix),
                new Claim(ClaimTypes.Sid,applicationuser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,jti)
            };
            foreach (var feature in features)
                authClaims.Add(new Claim("Feature", feature));
            _cache.AddItem(applicationuser.Id + "", jti, TimeSpan.FromMinutes(10).Ticks);
            return authClaims.ToArray();
        }

        public string GetToken(Claim[] claims)
        {
            JwtSecurityToken token;

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("token").GetSection("key").Value));

            token = new JwtSecurityToken(
            issuer: _configuration.GetSection("token").GetSection("issuer").Value,
            audience: _configuration.GetSection("token").GetSection("audience").Value,
            expires: DateTime.UtcNow.AddMinutes(10),
            claims: claims,
            signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
