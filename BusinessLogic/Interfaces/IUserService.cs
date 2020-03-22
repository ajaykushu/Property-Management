using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterUser(RegisterRequest model);
        RegisterRequest GetRegisterModel();
        Task<EditUser> GetEditUserModelAsync(long Id);
        Task<bool> UpdateUser(EditUser editUser);
        Task<bool> UploadAvtar(string path, string email);
        AddProperty GetPropertyType();
        Task<bool> AddProperty(AddProperty modal);
        Task<List<Properties>> GetProperties();
        Task<bool> DeleteProperty(int id);
        //Task<bool> MarkasPrimaryProperty(int id, string email);
        Task<bool> CheckUser(RegisterRequest model);
        Task<Pagination<IList<UsersList>>> GetAllUsers(int pageNumber, FilterEnum filter, string matchString);
        Task<bool> Deact_Actuser(long userId, int operation);
        Task<UserDetailModel> GetUserDetail(long id);
        Task<AddProperty> GetProperty(long id);
        Task<bool> UpdateProperty(AddProperty prop);
    }
}
