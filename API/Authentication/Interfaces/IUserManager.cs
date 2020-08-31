using Models.Login.RequestModels;
using Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Authentication.Interfaces
{
    public interface IUserManager
    {
        Task<TokenResponseModel> DoLogin(LoginUserDTO loginDTO);

        Task<bool> GetPasswordChangeTokenAsync(string email, string verificationPath);

        Task<bool> ChangePassowrd(string email, string token, string password);
        Task<HashSet<string>> GetMenuData();
    }
}