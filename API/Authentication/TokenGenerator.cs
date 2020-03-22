using API.Authentication.Interfaces;
using DataEntity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Authentication
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;
        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Claim[] GetClaims(ApplicationUser applicationuser, IList<string> roles, List<string> features)
        {

            List<Claim> authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, applicationuser.UserName),
                new Claim(ClaimTypes.Email, applicationuser.Email),
                new Claim(ClaimTypes.GivenName,applicationuser.FirstName+" "+applicationuser.LastName+" "+applicationuser.Suffix),
                new Claim(ClaimTypes.Sid,applicationuser.Id.ToString())
            };
            foreach (var role in roles)
                authClaims.Add(
                    new Claim(ClaimTypes.Role, role));

            foreach (var feature in features)
                authClaims.Add(new Claim("Feature", feature));

            return authClaims.ToArray();
        }

        public string GetToken(Claim[] claims)
        {
            JwtSecurityToken token;

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("token").GetSection("key").Value));

            token = new JwtSecurityToken(
            issuer: _configuration.GetSection("token").GetSection("issuer").Value,
            audience: _configuration.GetSection("token").GetSection("audience").Value,
            expires: DateTime.UtcNow.AddHours(2),
            claims: claims,
            signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
