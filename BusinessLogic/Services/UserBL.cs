using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using DataTransferObjects.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Models;
using Models.ResponseModels;
using Models.ResponseModels.User;
using Models.User.RequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities.CustomException;
using Utilities.Interface;

namespace BusinessLogic.Services
{
    public class UserBL : IUserBL
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IRepo<UserNotification> _userNotification;
        private readonly IRepo<WorkOrder> _wo;
        private readonly IRepo<Languages> _langrepo;
        private readonly IRepo<Property> _property;
        private readonly IRepo<Department> _department;
        private readonly IRepo<UserProperty> _userProperty;
        private readonly IRepo<Notification> _notification;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IImageUploadInFile _imageUploadInFile;
        private readonly IDistributedCache _cache;
        private readonly string _scheme;
        private readonly long userId;
        private readonly IRepo<Effort> _effort;

        public UserBL(IRepo<WorkOrder> wo,UserManager<ApplicationUser> userManager,
              RoleManager<ApplicationRole> roleManager, IRepo<Languages> langrepo, IRepo<Property> property, IHttpContextAccessor httpContextAccessor, IImageUploadInFile imageUploadInFile, IDistributedCache cache, IRepo<Department> department, IRepo<UserProperty> userProperty, IRepo<Notification> notification, IRepo<UserNotification> userNotification, IRepo<Effort> effort)
        {
            _userManager = userManager;
            _wo = wo;
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
            _effort = effort;
        }

        public async Task<bool> RegisterUser(RegisterUserDTO model)
        {
            var filepath = await _imageUploadInFile.UploadAsync(model.File);
            var prop = _property.GetAll().ToList();
            IdentityResult identityResult;
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = model.Email,
            Email = model.Email,
                IsEffortVisible=model.IsEffortVisible,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ClockType = "12",
                LanguageId = model.Language,
                Suffix = model.Suffix,
                TimeZone = "Pacific Standard Time",
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

        public async Task<RegisterUserDTO> GetRegisterModel()
        {
            RegisterUserDTO registerRequest = new RegisterUserDTO
            {
                Departments = await _department.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.DepartmentName }).AsNoTracking().ToListAsync(),
                Languages = await _langrepo.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.Language }).AsNoTracking().ToListAsync(),
            };
             registerRequest.Roles = await _roleManager.Roles.Select(x => new SelectItem { Id = x.Id, PropertyName = x.Name }).AsNoTracking().ToListAsync();
            // finding roles to be displayed a/c to usertype
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                registerRequest.Roles= registerRequest.Roles.Where(x=>x.PropertyName!="Master Admin").ToList();
                registerRequest.Properties = await _userProperty.GetAll().Where(x => x.ApplicationUserId == userId).Include(x => x.Property).Where(x => x.Property.IsActive).Select(x => new SelectItem { Id = x.Id, PropertyName = x.Property.PropertyName }).AsNoTracking().ToListAsync();
                registerRequest.Role = "User";
            }
            else if (_httpContextAccessor.HttpContext.User.IsInRole("Master Admin"))
            {
                registerRequest.Properties = await _property.GetAll().Where(x => x.IsActive).Select(x => new SelectItem { Id = x.Id, PropertyName = x.PropertyName }).AsNoTracking().ToListAsync();
                registerRequest.Role = "Admin";
            }
            else if (_httpContextAccessor.HttpContext.User.IsInRole("User"))
            {
                registerRequest.Roles = registerRequest.Roles.Where(x => x.PropertyName.ToLower().Equals("user")).ToList();
                registerRequest.Properties = await _userProperty.GetAll().Where(x => x.ApplicationUserId == userId).Include(x => x.Property).Where(x => x.Property.IsActive).Select(x => new SelectItem { Id = x.Id, PropertyName = x.Property.PropertyName }).AsNoTracking().ToListAsync();
                registerRequest.Role = registerRequest.Roles[0].PropertyName;
            }

            var langId = registerRequest.Languages.Where(x => x.PropertyName.ToLower().Equals("english")).FirstOrDefault();

            registerRequest.Language = (int)langId.Id;
            return registerRequest;
        }

        public async Task<EditUserDTO> GetEditUserModelAsync(long Id)
        {
            ApplicationUser applicationUser = await _userManager.Users.Where(x => x.Id == Id).Include(x => x.UserProperties).ThenInclude(x => x.Property).AsNoTracking().FirstOrDefaultAsync();

            if (applicationUser == null)
                throw new BadRequestException("User not Found");

            var roles = await _userManager.GetRolesAsync(applicationUser);

            EditUserDTO editusermodel = new EditUserDTO
            {
                Languages = _langrepo.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.Language }).AsNoTracking().ToList(),
                TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(x => new SelectItem { Id = x.Id, PropertyName = x.DisplayName }).ToList(),
                Departments = _department.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.DepartmentName }).AsNoTracking().ToList(),
                Email = applicationUser.Email,
                IsEffortVisible=applicationUser.IsEffortVisible,
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
            // finding roles to be displayed a/c to usertype
            editusermodel.Roles = await _roleManager.Roles.Select(x => new SelectItem { Id = x.Id, PropertyName = x.Name }).AsNoTracking().ToListAsync();
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                editusermodel.Roles = editusermodel.Roles.Where(x => x.PropertyName != "Master Admin").ToList();
                editusermodel.Properties = await _userProperty.GetAll().Where(x => x.ApplicationUserId == userId).Include(x => x.Property).Where(x => x.Property.IsActive).Select(x => new SelectItem { Id = x.Id, PropertyName = x.Property.PropertyName }).AsNoTracking().ToListAsync();
            }
            else if (_httpContextAccessor.HttpContext.User.IsInRole("Master Admin"))
            {
                editusermodel.Properties = await _property.GetAll().Where(x => x.IsActive).Select(x => new SelectItem { Id = x.Id, PropertyName = x.PropertyName }).AsNoTracking().ToListAsync();
               
            }
            else if (_httpContextAccessor.HttpContext.User.IsInRole("User"))
            {
                editusermodel.Roles = editusermodel.Roles.Where(x => x.PropertyName.ToLower().Equals("user")).ToList();
                editusermodel.Properties = await _userProperty.GetAll().Where(x => x.ApplicationUserId == userId).Include(x => x.Property).Where(x => x.Property.IsActive).Select(x => new SelectItem { Id = x.Id, PropertyName = x.Property.PropertyName }).AsNoTracking().ToListAsync();
               
            }
     
            return editusermodel;
        }

        public async Task<bool> UpdateUser(EditUserDTO editUser)
        {
            IdentityResult identityResult;

            ApplicationUser applicationUser = await _userManager.Users.Where(x => x.Id == editUser.Id).Include(x => x.Language).Include(x => x.UserProperties).FirstOrDefaultAsync();
            var prop = _property.GetAll().Include(x => x.UserProperties).ThenInclude(x => x.Property).ToList();

            applicationUser.Email = editUser.Email;
            applicationUser.TimeZone = editUser.TimeZone;
            applicationUser.Suffix = editUser.Suffix;
            applicationUser.FirstName = editUser.FirstName;
            applicationUser.IsEffortVisible = editUser.IsEffortVisible;
            applicationUser.Language.Id = editUser.Language;
            applicationUser.LastName = editUser.LastName;
            applicationUser.SMSAltert = editUser.SMSAlert;
            applicationUser.OfficeExt = editUser.OfficeExt;
            applicationUser.PhoneNumber = editUser.PhoneNumber;
            applicationUser.ClockType = editUser.ClockType;
            applicationUser.DepartmentId = editUser.DepartmentId;

            //handling image update
            var filepath = await _imageUploadInFile.UploadAsync(editUser.File);
            var prevpath = applicationUser.PhotoPath;

            if (filepath != null)
                applicationUser.PhotoPath = filepath;

            if (editUser.Role == "Master Admin" || editUser.SelectedProperty == null)
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
                if (editUser.Password != null) await _cache.RefreshAsync(applicationUser.Id + "");
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
            var quey2 = _roleManager.Roles;
            if (matchStr != null && filter == FilterEnum.Email)
                query = query.Where(x => x.NormalizedEmail.StartsWith(matchStr.ToUpper()));
            else if (matchStr != null && filter == FilterEnum.FirstName)
                query = query.Where(x => x.FirstName.ToLower().StartsWith(matchStr.ToLower()));
            else if (matchStr != null && filter == FilterEnum.Property)
                query = query.Where(x => x.UserProperties.Where(x => x.IsPrimary && x.Property.PropertyName.ToLower().Contains(matchStr.ToLower())).Any());

            if (!_httpContextAccessor.HttpContext.User.IsInRole("Master Admin"))
            {
                var propIds = await _userProperty.GetAll().Include(x => x.ApplicationUser).Where(x => x.ApplicationUserId == userId).AsNoTracking().Select(x => x.PropertyId).Distinct().ToListAsync();
                var userIds = await _userProperty.GetAll().Where(x => propIds.Contains(x.PropertyId)).AsNoTracking().Select(x => x.ApplicationUserId).ToListAsync();
                if (_httpContextAccessor.HttpContext.User.IsInRole("User"))
                {
                    //only select users roles
                    var usersWithRole = await _userManager.GetUsersInRoleAsync("User");
                    userIds = usersWithRole.Where(x => userIds.Contains(x.Id)).Select(x => x.Id).ToList();
                }
                query = query.Where(x => userIds.Contains(x.Id));
            }
            var count = query.Count();


            var tempquery = await query.Include(x => x.UserProperties).ThenInclude(x => x.Property).Skip(pageNumber * iteminpage).Take(iteminpage).AsNoTracking().ToListAsync();

            List<UsersListModel> users = new List<UsersListModel>();
            foreach (var item in tempquery)
            {
                var roles = await _userManager.GetRolesAsync(item);
                if (roles != null)
                    users.Add(new UsersListModel
                    {
                        Email = item.Email,
                        FullName = item.FirstName + " " + item.LastName,
                        Id = item.Id,
                        
                        IsActive = item.IsActive,
                        PrimaryProperty = item.UserProperties.Where(x => x.IsPrimary).Select(x => x.Property.PropertyName).FirstOrDefault(),
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
                    await _cache.RemoveAsync(userId + "");
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
                    IsEffortVisible = x.IsEffortVisible,
                    FullName = string.Concat(x.FirstName, " ", x.LastName, " ", x.Suffix ?? ""),
                    Id = x.Id,
                    PhotoPath = !string.IsNullOrWhiteSpace(x.PhotoPath) ? string.Concat(_scheme, _httpContextAccessor.HttpContext.Request.Host.Value, "/api/", x.PhotoPath) : "",
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
                    
                    OfficeExtension = x.OfficeExt,
                    IsActive = x.IsActive,
                    SMSAlert = x.SMSAltert,
                    TimeZone = x.TimeZone,
                    TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(x => new SelectItem { Id = x.Id, PropertyName = x.DisplayName }).ToList()

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
                Id = x.NId,
                CreationTime = x.CreatedTime.ToString("dd-MMM-yy hh:mm:ss tt"),
                Message = x.Message,
                NotificationType = x.NotificationType,
                NavigatorId = x.NavigatorId
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
                var count = await _userNotification.Get(x => x.NotificationId == id).Where(x => !x.IsRead).CountAsync();
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

        public async Task<List<UserList>> GetUserEmail()
        {
           var res= await _userManager.Users.Select(x => new UserList {  Email = x.Email,  DisplayName = x.FirstName + " " + x.LastName }).AsNoTracking().ToListAsync();
            return res;
        }

        public async Task<List<TimeSheet>> GetTimeSheet()
        {

            var query = _effort.GetAll() ;
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                var propIds = await _userProperty.GetAll().Include(x => x.ApplicationUser).Where(x => x.ApplicationUserId == userId).AsNoTracking().Select(x => x.PropertyId).Distinct().ToListAsync();
                var userIds = await _userProperty.GetAll().Where(x => propIds.Contains(x.PropertyId)).AsNoTracking().Select(x => x.ApplicationUserId).ToListAsync();
                query=query.Where(x => userIds.Contains(x.UserId));
            }
            if (_httpContextAccessor.HttpContext.User.IsInRole("User"))
            {
                query=query.Where(x => x.UserId == userId);
            }
            var res = await query.Include(x => x.User).ToListAsync();
            //dynamic grp = null;
            List<TimeSheet> sh = new List<TimeSheet>();
            if (!_httpContextAccessor.HttpContext.User.IsInRole("User"))
            {
                var grp = res.GroupBy(x => new { x.WOId, x.UserId });
                foreach (var item in grp)
                {
                    var x = new TimeSheet();
                    x.WoId = item.Key.WOId;
                    x.UserName = item.Select(x => String.Concat(x.User.FirstName," ",x.User.LastName)).First();
                    x.Updated = item.Select(x => x.UpdatedTime.ToString("dd-MMM-yyyy")).First();
                    x.TotalHours = item.Sum(x => x.Repair).ToString();
                    sh.Add(x);
                }
            }
            else
            {
                var grp = res.GroupBy(x => x.WOId);
                foreach (var item in grp)
                {
                    var x = new TimeSheet();
                    x.WoId = item.Key;
                    x.Updated = item.Select(x => x.UpdatedTime.ToString("dd-MMM-yyyy")).First();
                    x.TotalHours = item.Sum(x => x.Repair).ToString();
                    sh.Add(x);
                }
            }
            

             
            return sh;
        }

        public async Task<List<TimesheetBreakDown>> GetTimeSheet(string id)
        {
           var query= _effort.GetAll().Include(x => x.User).Where(x => x.WOId == id);
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                var propIds = await _userProperty.GetAll().Include(x => x.ApplicationUser).Where(x => x.ApplicationUserId == userId).AsNoTracking().Select(x => x.PropertyId).Distinct().ToListAsync();
                var userIds = await _userProperty.GetAll().Where(x => propIds.Contains(x.PropertyId)).AsNoTracking().Select(x => x.ApplicationUserId).ToListAsync();
                query = query.Where(x => userIds.Contains(x.UserId)).OrderBy(x => x.Date);
            }
            if (_httpContextAccessor.HttpContext.User.IsInRole("User"))
            {
                query = query.Where(x => x.UserId == userId && x.Date>=x.Date.AddMonths(-1)).OrderBy(x => x.Date);
            }
            return await query.Select(x => new TimesheetBreakDown
            {
                
                Repair = x.Repair.ToString(),
                Date = x.Date.ToString("dd-MMM-yyyy"),
                Service = x.Service.ToString(),
                Updated = x.UpdatedTime.ToString("dd-MMM-yyyy"),
                UserName = x.User.FirstName + " " + x.User.LastName,
                
                WoId = x.WOId.ToString()
            }).ToListAsync();
        }

        public async Task<bool> SaveEffort(List<TimesheetBreakDown> timesheetBreakDown)
        {
            if (timesheetBreakDown.Count > 0)
            {
                var wo = await _wo.Get(x => x.Id == timesheetBreakDown[0].WoId).Include(x => x.Efforts).FirstOrDefaultAsync();
                foreach(var item in timesheetBreakDown)
                {
                    var effitem=wo.Efforts.Where(x => x.Date.ToString("yyyy-MM-dd") == item.Date).FirstOrDefault();
                    if (effitem != null && int.TryParse(item.Service, out _) && int.TryParse(item.Repair, out _))
                    {
                        effitem.Service = Convert.ToInt32(item.Service);
                        effitem.Repair = Convert.ToInt32(item.Repair);
                    }
                }
                var res=await _wo.Update(wo);
                if (res >= 1) return true;
            }
           
                return false;
        }
        public async  Task<bool> ChangeTZ(string timeZone,long Id)
        {
            var user = await _userManager.Users.Where(x=>x.Id==Id).FirstOrDefaultAsync();
            user.TimeZone = timeZone;
            var res=await  _userManager.UpdateAsync(user);
            if (res.Succeeded)
                return true;
            return false;

        }
    }
}