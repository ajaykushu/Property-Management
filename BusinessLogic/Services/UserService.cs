using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities.CustomException;
using Utilities.Interface;

namespace BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRepo<Languages> _langrepo;
        private readonly IRepo<Property> _property;
        private readonly IRepo<UserProperty> _userproperty;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IImageUploadInFile _imageUploadInFile;
        private readonly ICache _cache;
        public UserService(UserManager<ApplicationUser> userManager,
              RoleManager<ApplicationRole> roleManager, IRepo<Languages> langrepo, IRepo<Property> property,  IRepo<UserProperty> userproperty, IHttpContextAccessor httpContextAccessor, IImageUploadInFile imageUploadInFile, ICache cache)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _langrepo = langrepo;
            _property = property;
            _userproperty = userproperty;
            _httpContextAccessor = httpContextAccessor;
            _imageUploadInFile = imageUploadInFile;
            _cache = cache;


        }

        public async Task<bool> RegisterUser(RegisterUser model)
        {

            var language = _langrepo.Get(x => x.Id == model.Language).FirstOrDefault();
            var prop = _userproperty.GetAll().Include(x => x.Property).ToList();
            IdentityResult identityResult;
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Language = language,
                Suffix = model.Suffix,
                TimeZone = model.TimeZone,
                OfficeExt = model.OfficeExt ?? null
            };
            identityResult = await _userManager.CreateAsync(applicationUser, model.Password);
            foreach (var item in prop)
                if (model.SelectedProperty != null && model.SelectedProperty.Contains(item.Property.PropertyName))
                    applicationUser.UserProperties.Add(item);


            if (!identityResult.Succeeded)
            {
                throw new BadRequestException(identityResult.Errors.Select(x => x.Description).Aggregate((i, j) => i + ", " + j));
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(applicationUser.Email);

                var roleresult = await _userManager.AddToRoleAsync(user, model.Role);
                if (roleresult.Succeeded)
                    return true;
                else
                    throw new BadRequestException(roleresult.Errors.Select(x => x.Description).Aggregate((i, j) => i + ", " + j));
            }

        }
       
        public RegisterUser GetRegisterModel()
        {
            RegisterUser registerRequest = new RegisterUser
            {
                Roles = _roleManager.Roles.Select(x => new SelectItem { Id = x.Id, PropertyName = x.Name }).AsNoTracking().ToList(),
                Languages = _langrepo.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.Language }).AsNoTracking().ToList(),
                TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(x => new SelectItem { Id = 1, PropertyName = x.DisplayName }).ToList(),
                Properties = _property.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.PropertyName }).AsNoTracking().ToList()
            };
            var langId = registerRequest.Languages.Where(x => x.PropertyName.ToLower() == "english").FirstOrDefault();
            var roleid = registerRequest.Roles.Where(x => x.PropertyName.ToLower() == "user").FirstOrDefault();
            registerRequest.Language = langId != null ? langId.Id : 0;
            registerRequest.Role = roleid?.PropertyName;
            return registerRequest;
        }
        public async Task<EditUserModel> GetEditUserModelAsync(long Id)
        {

            ApplicationUser applicationUser = await _userManager.Users.Where(x => x.Id == Id).Include(x => x.UserProperties).ThenInclude(x => x.Property).AsNoTracking().FirstOrDefaultAsync();
            if (applicationUser == null)
                throw new BadRequestException("User not Found");
            var roles = await _userManager.GetRolesAsync(applicationUser);
            EditUserModel editusermodel = new EditUserModel
            {
                Properties = _property.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.PropertyName }).AsNoTracking().ToList(),
                Roles = _roleManager.Roles.Select(x => new SelectItem { Id = x.Id, PropertyName = x.Name }).AsNoTracking().ToList(),
                Languages = _langrepo.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.Language }).AsNoTracking().ToList(),
                TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(x => new SelectItem { Id = 1, PropertyName = x.DisplayName }).ToList(),
                Email = applicationUser.Email,
                UserName = applicationUser.UserName,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Suffix = applicationUser.Suffix,
                SMSAlert = applicationUser.SMSAltert,
                TimeZone = applicationUser.TimeZone,
                Language = applicationUser.LanguageId,
                Role = roles.Count > 0 ? roles[0] : "",
                ClockType = applicationUser.ClockType,
                OfficeExt = applicationUser.OfficeExt,
                PhoneNumber = applicationUser.PhoneNumber,
                SelectedProperty = applicationUser.UserProperties.Select(x => x.Property.PropertyName).ToList(),
                Id = applicationUser.Id
            };
            return editusermodel;
        }
        public async Task<bool> UpdateUser(EditUserModel editUser)
        {
            IdentityResult identityResult;
            ApplicationUser applicationUser = await _userManager.Users.Where(x => x.Id == editUser.Id).Include(x => x.Language).Include(x => x.UserProperties).FirstOrDefaultAsync();
            var prop = _property.GetAll().Include(x => x.UserProperties).ThenInclude(x => x.Property).ToList();
            applicationUser.Email = editUser.Email;
            applicationUser.UserName = editUser.UserName;
            applicationUser.TimeZone = editUser.TimeZone;
            applicationUser.Suffix = editUser.Suffix;
            applicationUser.FirstName = editUser.FirstName;
            applicationUser.Language.Id = editUser.Language;
            applicationUser.LastName = editUser.LastName;
            applicationUser.SMSAltert = editUser.SMSAlert;
            applicationUser.TimeZone = editUser.TimeZone;
            applicationUser.OfficeExt = editUser.OfficeExt;
            applicationUser.PhoneNumber = editUser.PhoneNumber;
            applicationUser.ClockType = editUser.ClockType;
            applicationUser.UserProperties.Clear();
            foreach (var item in prop)
            {
                if (editUser.SelectedProperty != null && editUser.SelectedProperty.Contains(item.PropertyName))
                    applicationUser.UserProperties.Add(new UserProperty()
                    {
                        ApplicationUser = applicationUser,
                        Property = item
                    });
            }
            if (!String.IsNullOrEmpty(editUser.Password))
                applicationUser.PasswordHash = _userManager.PasswordHasher.HashPassword(applicationUser, editUser.Password);

            identityResult = await _userManager.UpdateAsync(applicationUser);
            if (identityResult.Succeeded)
            {
                if (editUser.Password != null) _cache.RemoveItem(applicationUser.Id + "");
                var roles = await _userManager.GetRolesAsync(applicationUser);
                var roleDeleted = await _userManager.RemoveFromRolesAsync(applicationUser, roles);
                var roleAdded = await _userManager.AddToRoleAsync(applicationUser, editUser.Role);
                if (roleDeleted.Succeeded && roleAdded.Succeeded)
                {
                    return true;
                }
                else
                {
                    throw new BadRequestException(roleDeleted.Errors.Select(x => x.Description).Aggregate((i, j) => i + ", " + j) + " " + roleAdded.Errors.Select(x => x.Description).Aggregate((i, j) => i + ", " + j));
                }
            }
            else
            {
                throw new BadRequestException(identityResult.Errors.Select(x => x.Description).Aggregate((i, j) => i + ", " + j));
            }
        }
       
        public async Task<bool> UploadAvtar(string path, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user.PhotoPath != null)
                _imageUploadInFile.Delete(user.PhotoPath);
            user.PhotoPath = path;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                throw new BadRequestException(result.Errors.Select(x => x.Description).Aggregate((i, j) => i + ", " + j));
            }
        }

        public async Task<Pagination<IList<UsersListModel>>> GetAllUsers(int pageNumber, FilterEnum filter, string matchStr)
        {
            var count = _userManager.Users.Count();
            List<ApplicationUser> user;
            if (matchStr != null && filter == FilterEnum.ByEmail)
                user = await _userManager.Users.Where(x => x.Email.ToLower().StartsWith(matchStr.ToLower())).Skip(pageNumber * 10).Take(10).AsNoTracking().ToListAsync();
            else if (matchStr != null && filter == FilterEnum.ByFirstName)
                user = await _userManager.Users.Where(x => x.FirstName.ToLower().StartsWith(matchStr.ToLower())).Skip(pageNumber * 10).Take(10).AsNoTracking().ToListAsync();
            else
                user = await _userManager.Users.Skip(pageNumber * 10).Take(10).AsNoTracking().ToListAsync();
            List<UsersListModel> users = new List<UsersListModel>();
            foreach (var item in user)
            {
                var roles = await _userManager.GetRolesAsync(item);
                if (roles != null)
                    users.Add(new UsersListModel
                    {
                        Email = item.Email,
                        FullName =item.FirstName + " " + item.LastName,
                        Id = item.Id,
                        UserName = item.UserName,
                        IsActive = item.IsActive,
                        Roles = string.Join(", ", roles)
                    });
            }
            var pagination = new Pagination<IList<UsersListModel>>
            {
                ItemsPerPage = user.Count < 10 ? user.Count : 10,
                PageCount = count < 10 ? 1 : (count / 10) + 1,
                Payload = users,
                CurrentPage = pageNumber
            };

            return pagination;
        }

        public async Task<bool> Deact_Actuser(long userId, int operation)
        {

            var iduser = await _userManager.FindByIdAsync(userId + "");
            iduser.IsActive = Convert.ToBoolean(operation);
            var identityresult = await _userManager.UpdateAsync(iduser);
            if (identityresult.Succeeded)
            {
                if(operation==0)
                    _cache.RemoveItem(userId + "");
                return true;
            }
            else
            {
                throw new BadRequestException(identityresult.Errors.Select(x => x.Description).Aggregate((i, j) => i + ", " + j));
            }
        }

        public async Task<UserDetailModel> GetUserDetail(long id)
        {
            var user = _userManager.Users.Where(x => x.Id == id).Include(x => x.UserProperties).Select(x => new
            {
                applicationUser = x,
                UserModel = new UserDetailModel
                {
                    EmailAddress = x.Email,
                    FullName = string.Concat(x.FirstName, " ", x.LastName, " ", x.Suffix ?? ""),
                    Id = x.Id,
                    PhotoPath = "https://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/" + x.PhotoPath,
                    ListProperties = x.UserProperties.Select(x => new PropertiesModel
                    {
                        Id = x.Property.Id,
                        City = x.Property.City,
                        Country = x.Property.Country,
                        HouseNumber = x.Property.HouseNumber,
                        StreetLine2 = x.Property.StreetLine2,
                        Locality = x.Property.Locality,
                        PinCode = x.Property.PinCode,
                        PropertyName = x.Property.PropertyName,
                        PropertyType = x.Property.PropertyTypes.PropertyTypeName,
                        Street = x.Property.Street
                    }).ToList(),
                    PhoneNumber = x.PhoneNumber,
                    UserId = x.UserName,
                    OfficeExtension = x.OfficeExt,
                    IsActive = x.IsActive,
                    SMSAlert = x.SMSAltert,
                    TimeZone = x.TimeZone
                }
            }).AsNoTracking().FirstOrDefault();
            if (user != null)
                user.UserModel.Roles = await _userManager.GetRolesAsync(user.applicationUser);
            else
                throw new BadRequestException("User Not Found");
            return user.UserModel;
        }

       
        public async Task<bool> CheckEmail(string email)
        {
            bool status;
            var res = await _userManager.FindByEmailAsync(email);
            status = res == null ? true : false;
            return status;
        }

        public async Task<bool> CheckPhoneNumber(string phoneNumber)
        {
            bool status = false;
            var res = await _userManager.Users.Where(x => x.PhoneNumber.Equals(phoneNumber)).FirstOrDefaultAsync();
            status = res == null ? true : false;
            return status;
        }

        public async Task<bool> CheckUserName(string userName)
        {
            bool status = false;
            var res = await _userManager.Users.Where(x => x.UserName.ToLower().Equals(userName.Trim().ToLower())).FirstOrDefaultAsync();
            status = res == null ? true : false;
            return status;
        }
    }
}
