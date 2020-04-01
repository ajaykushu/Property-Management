using DataEntity;
using System.Collections.Generic;
using System.Security.Claims;

namespace API.Authentication.Interfaces
{
    public interface ITokenGenerator
    {
        Claim[] GetClaims(ApplicationUser applicationuser, HashSet<string> features);

        string GetToken(Claim[] claims);
    }
}