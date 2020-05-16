using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
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
        private readonly IRepo<UserNotification> _userNotification;
        private readonly IRepo<Languages> _langrepo;
        private readonly IRepo<Property> _property;
        private readonly IRepo<Department> _department;
        private readonly IRepo<UserProperty> _userProperty;
        private readonly IRepo<Notification> _notification;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IImageUploadInFile _imageUploadInFile;
        private readonly ICache _cache;
        private  readonly string _scheme;
        private readonly long userId;

        public UserService(UserManager<ApplicationUser> userManager,
              RoleManager<ApplicationRole> roleManager, IRepo<Languages> langrepo, IRepo<Property> property, IHttpContextAccessor httpContextAccessor, IImageUploadInFile imageUploadInFile, ICache cache, IRepo<Department> department, IRepo<UserProperty> userProperty, IRepo<Notification> notification, IRepo<UserNotification> userNotification)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _langrepo = langrepo;
            _property = property;
            _userProperty = userProperty;
            _httpContextAccessor = httpContextAccessor;
            _imageUploadInFile = imageUploadInFile;
            _cache = cache;
            _department = department;
            _notification = notification;
            _scheme = _httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://";
            var sid = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (sid != null)
                userId = Convert.ToInt64(sid);
            _userNotification = userNotification;
        }

        public async Task<bool> RegisterUser(RegisterUser model)
        {
            var filepath = await _imageUploadInFile.UploadAsync(model.File);
            var prop = _property.GetAll().ToList();
            IdentityResult identityResult;
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                LanguageId = model.Language,
                Suffix = model.Suffix,
                TimeZone = model.TimeZone,
                OfficeExt = model.OfficeExt ?? null,
                PhotoPath = filepath,
                DepartmentId = model.DepartmentId
            };
            if (model.SelectedProperty != null && !model.Role.Equals("Master Admin"))
            {
                foreach (var item in prop)
                {
                    var userprop = new UserProperty
                    {
                        PropertyId = item.Id
                        
                    };
                   
                    if (item.PropertyName == model.PrimaryProperty)
                        userprop.IsPrimary = true;
                    if (model.SelectedProperty.Contains(item.PropertyName))
                        applicationUser.UserProperties.Add(userprop);
                }
            }
            identityResult = await _userManager.CreateAsync(applicationUser, model.Password);
            if (!identityResult.Succeeded)
            {
                _imageUploadInFile.Delete(filepath);
                throw new BadRequestException(identityResult.Errors.Select(x => x.Description).Aggregate((i, j) => i + ", " + j));
            }
            else
            {
                var roleresult = await _userManager.AddToRoleAsync(applicationUser, model.Role);
                if (roleresult.Succeeded)
                    return true;
                else
                    throw new BadRequestException(roleresult.Errors.Select(x => x.Description).Aggregate((i, j) => i + ", " + j));
            }
        }

        public async Task<RegisterUser> GetRegisterModel()
        {
            RegisterUser registerRequest = new RegisterUser
            {
                
                Departments = await _department.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.DepartmentName }).AsNoTracking().ToListAsync(),
                Languages =await  _langrepo.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.Language }).AsNoTracking().ToListAsync(),
                TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(x => new SelectItem { Id = 1, PropertyName = x.DisplayName }).ToList()
               
            };
            // finding roles to be displayed a/c to usertype
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin")) {
                registerRequest.Roles = new List<SelectItem> { new SelectItem { Id = 1, PropertyName = "User" } };
                registerRequest.Properties = await _userProperty.GetAll().Where(x => x.ApplicationUserId == userId).Include(x => x.Property).Where(x => x.Property.IsActive).Select(x => new SelectItem { Id = x.Id, PropertyName = x.Property.PropertyName }).AsNoTracking().ToListAsync();
                registerRequest.Role = registerRequest.Roles.Where(x => x.PropertyName.ToLower() == "user").Select(x=>x.PropertyName).FirstOrDefault();
            }
            else
            {
                registerRequest.Roles = _roleManager.Roles.Select(x => new SelectItem { Id = x.Id, PropertyName = x.Name }).AsNoTracking().ToList();
                registerRequest.Properties = await _property.GetAll().Where(x => x.IsActive).Select(x => new SelectItem { Id = x.Id, PropertyName = x.PropertyName }).AsNoTracking().ToListAsync();
                registerRequest.Role = registerRequest.Roles.Where(x => x.PropertyName.ToLower() == "admin").Select(x => x.PropertyName).FirstOrDefault();
            }

            var langId = registerRequest.Languages.Where(x => x.PropertyName.ToLower().Equals("english")).FirstOrDefault();

           

            registerRequest.Language = (int)langId.Id;
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
                
                Languages = _langrepo.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.Language }).AsNoTracking().ToList(),
                TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(x => new SelectItem { Id = 1, PropertyName = x.DisplayName }).ToList(),
                Departments = _department.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.DepartmentName }).AsNoTracking().ToList(),
                Email = applicationUser.Email,
                UserName = applicationUser.UserName,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                Suffix = applicationUser.Suffix,
                SMSAlert = applicationUser.SMSAltert,
                TimeZone = applicationUser.TimeZone,
                Language = applicationUser.LanguageId.GetValueOrDefault(),
                Role = roles.Count > 0 ? roles[0] : "",
                ClockType = applicationUser.ClockType,
                OfficeExt = applicationUser.OfficeExt,
                PhoneNumber = applicationUser.PhoneNumber,
                DepartmentId = applicationUser.DepartmentId.GetValueOrDefault(),
                SelectedProperty = applicationUser.UserProperties.Select(x => x.Property.PropertyName).ToList(),
                Id = applicationUser.Id,
                PrimaryProperty = applicationUser.UserProperties.Where(x => x.IsPrimary).Select(x => x.Property.PropertyName).FirstOrDefault()
            };
            // finding roles to be displayed a/c to usertype
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                editusermodel.Roles = new List<SelectItem> { new SelectItem { Id = 1, PropertyName = "User" } };
                editusermodel.Properties = await _userProperty.GetAll().Where(x => x.ApplicationUserId == userId).Include(x => x.Property).Where(x => x.Property.IsActive).Select(x => new SelectItem { Id = x.Id, PropertyName = x.Property.PropertyName }).AsNoTracking().ToListAsync();
                
            }
            else
            {
                editusermodel.Roles = _roleManager.Roles.Select(x => new SelectItem { Id = x.Id, PropertyName = x.Name }).AsNoTracking().ToList();
                editusermodel.Properties = await _property.GetAll().Where(x => x.IsActive).Select(x => new SelectItem { Id = x.Id, PropertyName = x.PropertyName }).AsNoTracking().ToListAsync();
               
            }
            return editusermodel;
        }

        public async Task<bool> UpdateUser(EditUserModel editUser)
        {
            IdentityResult identityResult;

            ApplicationUser applicationUser = await _userManager.Users.Where(x => x.Id == editUser.Id).Include(x => x.Language).Include(x => x.UserProperties).FirstOrDefaultAsync();
            var prop = _property.GetAll().Include(x => x.UserProperties).ThenInclude(x => x.Property).ToList();

            applicationUser.Email = editUser.Email;
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
            applicationUser.DepartmentId = editUser.DepartmentId;

            //handling image update
            var filepath = await _imageUploadInFile.UploadAsync(editUser.File);
            var prevpath = applicationUser.PhotoPath;

            if (filepath != null)
                applicationUser.PhotoPath = filepath;

            if (editUser.Role == "Master Admin" || editUser.SelectedProperty==null)
                applicationUser.UserProperties.Clear();

            else if (editUser.SelectedProperty != null && !editUser.Role.Equals("Master Admin"))
            {
                applicationUser.UserProperties.Clear();
                foreach (var item in prop)
                {
                    if (editUser.SelectedProperty.Contains(item.PropertyName))
                    {
                        var userprop = new UserProperty
                        {
                            ApplicationUser = applicationUser,
                            Property = item
                        };
                       
                        if (editUser.PrimaryProperty == item.PropertyName)
                            userprop.IsPrimary = true;
                        applicationUser.UserProperties.Add(userprop);
                    }

                }
            }
            if (!String.IsNullOrEmpty(editUser.Password))
                applicationUser.PasswordHash = _userManager.PasswordHasher.HashPassword(applicationUser, editUser.Password);

            identityResult = await _userManager.UpdateAsync(applicationUser);

            if (identityResult.Succeeded)
            {
                if (filepath != null)
                    _imageUploadInFile.Delete(prevpath);
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

        public async Task<Pagination<IList<UsersListModel>>> GetAllUsers(int pageNumber, FilterEnum filter, string matchStr)
        {
            int iteminpage = 20;
            var query = _userManager.Users;
            if (matchStr != null && filter == FilterEnum.ByEmail)
                 query= query.Where(x => x.NormalizedEmail.Contains(matchStr.ToUpper()));
            else if (matchStr != null && filter == FilterEnum.ByFirstName)
                query = query.Where(x => x.FirstName.ToLower().StartsWith(matchStr.ToLower()));

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {   
                var propIds = await _userProperty.GetAll().Include(x => x.ApplicationUser).Where(x => x.ApplicationUserId == userId).AsNoTracking().Select(x=>x.PropertyId).Distinct().ToListAsync();
                var userIds = await _userProperty.GetAll().Where(x => propIds.Contains(x.PropertyId)).AsNoTracking().Select(x => x.ApplicationUserId).ToListAsync();
                query = query.Where(x=>userIds.Contains(x.Id));
            }

            var count = query.Count();
            var user = await query.Skip(pageNumber * iteminpage).Take(iteminpage).AsNoTracking().ToListAsync();
            
            List<UsersListModel> users = new List<UsersListModel>();
            foreach (var item in user)
            {
                var roles = await _userManager.GetRolesAsync(item);
                if (roles != null)
                    users.Add(new UsersListModel
                    {
                        Email = item.Email,
                        FullName = item.FirstName + " " + item.LastName,
                        Id = item.Id,
                        UserName = item.UserName,
                        IsActive = item.IsActive,
                        Roles = string.Join(", ", roles)
                    });
            }
            var pagination = new Pagination<IList<UsersListModel>>
            {
                ItemsPerPage = count > iteminpage ? iteminpage : count,
                PageCount = count <= iteminpage ? 1 : count % iteminpage == 0 ? count / iteminpage : count / iteminpage + 1,
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
                if (operation == 0)
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
            var user = _userManager.Users.Where(x => x.Id == id).Include(x => x.UserProperties).Include(x => x.Department).Select(x => new
            {
                applicationUser = x,
                UserModel = new UserDetailModel
                {
                    EmailAddress = x.Email,
                    FullName = string.Concat(x.FirstName, " ", x.LastName, " ", x.Suffix ?? ""),
                    Id = x.Id,
                    PhotoPath = string.Concat(_scheme, _httpContextAccessor.HttpContext.Request.Host.Value, "/", x.PhotoPath),
                    ListProperties = x.UserProperties.Select(x => new PropertiesModel
                    {
                        Id = x.Property.Id,
                        City = x.Property.City,
                        Country = x.Property.Country,
                        StreetAddress1 = x.Property.StreetAddress1,
                        ZipCode = x.Property.ZipCode,
                        PropertyName = x.Property.PropertyName,
                        PropertyType = x.Property.PropertyTypes.PropertyTypeName,
                        StreetAddress2 = x.Property.StreetAddress2,
                        IsPrimary = x.IsPrimary,
                        State = x.Property.State,
                    }).ToList(),
                    PhoneNumber = x.PhoneNumber,
                    Department = x.Department.DepartmentName,
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
            status = res == null ? false : true;
            return status;
        }

        public async Task<bool> CheckPhoneNumber(string phoneNumber)
        {
            bool status = false;
            var res = await _userManager.Users.Where(x => x.PhoneNumber.Equals(phoneNumber)).FirstOrDefaultAsync();
            status = res == null ? false : true;
            return status;
        }

        public async Task<bool> CheckUserName(string userName)
        {
            bool status = false;
            var res = await _userManager.Users.Where(x => x.UserName.ToLower().Equals(userName.ToLower())).FirstOrDefaultAsync();
            status = res == null ? false : true;
            return status;
        }

        public async Task<List<AllNotification>> GetAllNotification()
        {

            var data = await _notification.GetAll().Include(x => x.UserNotification).Where(x => x.UserNotification.Where(x => x.ApplicationUserId == userId && !x.IsRead).Any()).Select(x => new AllNotification
            {
                Id=x.NId,
                CreationTime=x.CreatedTime.ToString("dd-MMM-yy hh:mm:ss tt"),
                Message=x.Message,
                NotificationType=x.NotificationType,
                NavigatorId=x.NavigatorId
            }).AsNoTracking().ToListAsync();

            return data;
        }

        public async Task<int> GetNotificationCount()
        {
            var data = await _notification.GetAll().Include(x => x.UserNotification).Where(x => x.UserNotification.Where(x => x.ApplicationUserId == userId && !x.IsRead).Any()).CountAsync();
            return data;
        }

        public async Task<bool> MarkAsRead(int id)
        {
            var data = await _userNotification.Get(x => x.ApplicationUserId == userId && x.NotificationId == id).FirstOrDefaultAsync();
            if (data != null)
            {
                data.IsRead = true;
                var count=await _userNotification.Get(x => x.NotificationId == id).Where(x => !x.IsRead).CountAsync();
                if (count == 0)
                    await _userNotification.Delete(data);
                else
                {
                    if (await _userNotification.Update(data) > 1)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}