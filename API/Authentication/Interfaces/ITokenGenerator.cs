using DataEntity;
using System.Collections.Generic;
using System.Security.Claims;

namespace API.Authentication.Interfaces
{
    public interface ITokenGenerator
    {
        Claim[] GetClaims(ApplicationUser applicationuser, IList<string> roles, HashSet<string> features);
        string GetToken(Claim[] claims);
    }
}
