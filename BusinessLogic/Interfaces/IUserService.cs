using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUser(RegisterUser model);

        Task<RegisterUser> GetRegisterModel();

        Task<EditUserModel> GetEditUserModelAsync(long Id);

        Task<bool> UpdateUser(EditUserModel editUser);

        Task<Pagination<IList<UsersListModel>>> GetAllUsers(int pageNumber, FilterEnum filter, string matchString);

        Task<bool> Deact_Actuser(long userId, int operation);

        Task<UserDetailModel> GetUserDetail(long id);

        Task<bool> CheckEmail(string email);

        Task<bool> CheckPhoneNumber(string phoneNumber);

        Task<bool> CheckUserName(string userName);
        Task<List<AllNotification>> GetAllNotification();
        Task<int> GetNotificationCount();
        Task<bool> MarkAsRead(int id);
    }
}