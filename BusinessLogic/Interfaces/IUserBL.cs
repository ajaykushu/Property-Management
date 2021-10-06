using DataTransferObjects.ResponseModels;
using Models;
using Models.ResponseModels;
using Models.ResponseModels.User;
using Models.User.RequestModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IUserBL
    {
        Task<bool> RegisterUser(RegisterUserDTO model);

        Task<RegisterUserDTO> GetRegisterModel();

        Task<EditUserDTO> GetEditUserModelAsync(long Id);

        Task<bool> UpdateUser(EditUserDTO editUser);

        Task<Pagination<IList<UsersListModel>>> GetAllUsers(int pageNumber, FilterEnum filter, string matchString);

        Task<bool> Deact_Actuser(long userId, int operation);

        Task<UserDetailModel> GetUserDetail(long id);

        Task<bool> CheckEmail(string email);

        Task<bool> CheckPhoneNumber(string phoneNumber);

        Task<bool> CheckUserName(string userName);

        Task<List<AllNotification>> GetAllNotification();

        Task<int> GetNotificationCount();

        Task<bool> MarkAsRead(int id);
        Task<List<UserList>> GetUserEmail();
        Task<List<TimeSheet>> GetTimeSheet();
        Task<List<TimesheetBreakDown>> GetTimeSheet(string id);
        Task<bool> SaveEffort(List<TimesheetBreakDown> timesheetBreakDown);
        Task<bool> ChangeTZ(string timeZone,long Id);
    }
}