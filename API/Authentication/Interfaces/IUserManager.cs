using Models.RequestModels;
using Models.ResponseModels;
using System.Threading.Tasks;

namespace API.Authentication.Interfaces
{
    public interface IUserManager
    {
        Task<TokenResponseModel> DoLogin(LoginUserModel loginDTO);
        Task<bool> GetPasswordChangeTokenAsync(string email, string verificationPath);
        Task<bool> ChangePassowrd(string email, string token, string password);

    }
}
