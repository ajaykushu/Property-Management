using API.Authentication.Interfaces;
using DataEntity;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly IDistributedCache _cache;

        public TokenGenerator(IConfiguration configuration, IDistributedCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

        public Claim[] GetClaims(ApplicationUser applicationuser, HashSet<string> features, IList<string> roles)
        {
            var jti = Guid.NewGuid().ToString().Replace("-", "");
            List<Claim> authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, applicationuser.UserName),
                new Claim(ClaimTypes.Email, applicationuser.Email),
                new Claim(ClaimTypes.GivenName,applicationuser.FirstName+" "+applicationuser.LastName+" "+applicationuser.Suffix),
                new Claim(ClaimTypes.Sid,applicationuser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,jti),
                new Claim("TimeZone",applicationuser.TimeZone??TimeZoneInfo.Utc.Id),
                new Claim("Clock",applicationuser.ClockType)
            };
            foreach (var feature in features)
                authClaims.Add(new Claim("Feature", feature));
            _cache.SetAsync(applicationuser.Id + "", Encoding.UTF8.GetBytes(jti), new DistributedCacheEntryOptions() {
                SlidingExpiration = TimeSpan.FromDays(30)
            }).Wait();
            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            return authClaims.ToArray();
        }

        public string GetToken(Claim[] claims)
        {
            JwtSecurityToken token;

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("token").GetSection("key").Value));

            token = new JwtSecurityToken(
            issuer: _configuration.GetSection("token").GetSection("issuer").Value,
            audience: _configuration.GetSection("token").GetSection("audience").Value,
            expires: DateTime.UtcNow.AddDays(30),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}