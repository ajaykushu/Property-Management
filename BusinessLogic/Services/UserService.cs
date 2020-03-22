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
        private readonly IRepo<PropertyType> _proprepo;
        private readonly IRepo<Property> _property;
        private readonly IRepo<UserProperty> _userproperty;
        private readonly IRepo<Country> _country;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IImageUploadInFile _imageUploadInFile;
        public UserService(UserManager<ApplicationUser> userManager,
              RoleManager<ApplicationRole> roleManager, IRepo<Languages> langrepo, IRepo<PropertyType> proprepo, IRepo<Property> property, IRepo<Country> country, IRepo<UserProperty> userproperty, IHttpContextAccessor httpContextAccessor, IImageUploadInFile imageUploadInFile)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _langrepo = langrepo;
            _proprepo = proprepo;
            _property = property;
            _userproperty = userproperty;
            _country = country;
            _httpContextAccessor = httpContextAccessor;
            _imageUploadInFile = imageUploadInFile;


        }

        public async Task<bool> RegisterUser(RegisterRequest model)
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
                CountryId = model.CountryCode,
                Title = model.Title,
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
        public async Task<bool> CheckUser(RegisterRequest model)
        {
            ApplicationUser identityUser;

            identityUser = await _userManager.FindByIdAsync(model.Email);
            if (identityUser != null)
            {
                return true;
            }
            return false;
        }
        public RegisterRequest GetRegisterModel()
        {
            RegisterRequest registerRequest = new RegisterRequest
            {
                Roles = _roleManager.Roles.Select(x => new SelectItem { Id = x.Id, PropertyName = x.Name }).AsNoTracking().ToList(),
                Languages = _langrepo.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.Language }).AsNoTracking().ToList(),
                CountryCodes = _country.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.Name + " (" + x.PhoneCode + ")" }).AsNoTracking().ToList(),
                TimeZones = TimeZoneInfo.GetSystemTimeZones().Select(x => new SelectItem { Id = 1, PropertyName = x.DisplayName }).ToList(),
                Properties = _property.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.PropertyName }).AsNoTracking().ToList()
            };
            var langId = registerRequest.Languages.Where(x => x.PropertyName.ToLower() == "english").FirstOrDefault();
            var roleid = registerRequest.Roles.Where(x => x.PropertyName.ToLower() == "user").FirstOrDefault();
            registerRequest.Language = langId != null ? langId.Id : 0;
            registerRequest.Role = roleid != null ? roleid.PropertyName : null;
            return registerRequest;
        }
        public async Task<EditUser> GetEditUserModelAsync(long Id)
        {

            ApplicationUser applicationUser = await _userManager.Users.Where(x => x.Id == Id).Include(x => x.UserProperties).ThenInclude(x => x.Property).Include(x => x.Country).AsNoTracking().FirstOrDefaultAsync();
            if (applicationUser == null)
                throw new BadRequestException("User not Found");
            var roles = await _userManager.GetRolesAsync(applicationUser);
            EditUser editusermodel = new EditUser
            {
                Properties = _property.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.PropertyName }).AsNoTracking().ToList(),
                Roles = _roleManager.Roles.Select(x => new SelectItem { Id = x.Id, PropertyName = x.Name }).AsNoTracking().ToList(),
                Languages = _langrepo.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.Language }).AsNoTracking().ToList(),
                CountryCodes = _country.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.Name + " (" + x.PhoneCode + ")" }).AsNoTracking().ToList(),
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
                OfficeExt=applicationUser.OfficeExt,
                PhoneNumber = applicationUser.PhoneNumber,
                CountryCode = applicationUser.Country.Id,
                SelectedProperty = applicationUser.UserProperties.Select(x => x.Property.PropertyName).ToList(),
                Id = applicationUser.Id,
                Title = applicationUser.Title
            };


            return editusermodel;
        }
        public async Task<bool> UpdateUser(EditUser editUser)
        {


            IdentityResult identityResult;
            ApplicationUser applicationUser = await _userManager.Users.Where(x => x.Id == editUser.Id).Include(x => x.Language).Include(x => x.UserProperties).FirstOrDefaultAsync();
            var prop = _property.GetAll().Include(x => x.UserProperties).ThenInclude(x => x.Property).ToList();
            //making prop primary
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
            applicationUser.Title = editUser.Title;
            applicationUser.CountryId = editUser.CountryCode;
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
        public AddProperty GetPropertyType()
        {
            var res = _proprepo.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.PropertyTypeName }).AsNoTracking().ToList();
            if (res == null)
                throw new BadRequestException("Property type is not available");
            AddProperty prop = new AddProperty
            {
                PropertyTypes = res
            };
            return prop;
        }
        public async Task<bool> AddProperty(AddProperty modal)
        {

            var property = _property.Get(x => x.PropertyName.ToLower().Equals(modal.PropertyName.ToLower())).FirstOrDefault();
            if (property != null)
            {
                throw new BadRequestException("Property Name already available");
            }
            Property prop = new Property()
            {
                City = modal.City,
                Country = modal.Country,
                HouseNumber = modal.HouseNumber,
                LandMark = modal.LandMark,
                Locality = modal.Locality,
                PropertyName = modal.PropertyName,
                PropertyTypes = _proprepo.Get(x => x.Id == modal.PropertyTypeId).FirstOrDefault(),
                Street = modal.Street,
                PinCode = modal.PinCode

            };
            var res = await _property.Add(prop);
            if (res > 0)
            {
                return true;
            }
            else
            {
                throw new BadRequestException("Add user failed");
            }
        }



        public async Task<List<Properties>> GetProperties()
        {
            var prop = await _property.GetAll().Select(
                x => new Properties
                {
                    City = x.City,
                    Country = x.Country,
                    HouseNumber = x.HouseNumber,
                    Id = x.Id,
                    LandMark = x.LandMark,
                    Locality = x.Locality,
                    PinCode = x.PinCode,
                    PropertyName = x.PropertyName,
                    PropertyType = x.PropertyTypes.PropertyTypeName,
                    Street = x.Street

                }
                ).AsNoTracking().ToListAsync();

            return prop;


        }
        public async Task<bool> DeleteProperty(int id)
        {
            var prop = _property.Get(x => x.Id == id).Include(x => x.UserProperties).ThenInclude(x => x.ApplicationUser).FirstOrDefault();

            if (prop != null)
            {
                if (prop.UserProperties != null && prop.UserProperties.Count != 0)
                {
                    throw new BadRequestException("Unable to delete as this is propery is allocated to [ " + string.Join(",", prop.UserProperties.Select(x => x.ApplicationUser.UserName).ToList()) + " ]");
                }
                int status = await _property.Delete(prop);
                if (status > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new BadRequestException("Property not found");
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

        public async Task<Pagination<IList<UsersList>>> GetAllUsers(int pageNumber, FilterEnum filter, string matchStr)
        {
           // long Id = Convert.ToInt64(managerId);
            var count = _userManager.Users.Count();
            List<ApplicationUser> user;
            if (matchStr != null && filter == FilterEnum.ByEmail)
                user = await _userManager.Users.Where(x => x.Email.ToLower().StartsWith(matchStr.ToLower())).Skip(pageNumber * 10).Take(10).AsNoTracking().ToListAsync();
            else if (matchStr != null && filter == FilterEnum.ByFirstName)
                user = await _userManager.Users.Where(x => x.FirstName.ToLower().StartsWith(matchStr.ToLower())).Skip(pageNumber * 10).Take(10).AsNoTracking().ToListAsync();
            else
                user = await _userManager.Users.Skip(pageNumber * 10).Take(10).AsNoTracking().ToListAsync();
            List<UsersList> users = new List<UsersList>();
            foreach (var item in user)
            {
                var roles = await _userManager.GetRolesAsync(item);
                if (roles != null)
                    users.Add(new UsersList
                    {
                        Email = item.Email,
                        FullName = item.Title + " " + item.FirstName + " " + item.LastName,
                        Id = item.Id,
                        UserName = item.UserName,
                        IsActive = item.IsActive,
                        Roles = string.Join(", ", roles)
                    });
            }
            var pagination = new Pagination<IList<UsersList>>
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
                return true;
            }
            else
            {
                throw new BadRequestException(identityresult.Errors.Select(x => x.Description).Aggregate((i, j) => i + ", " + j));
            }
        }

        public async Task<UserDetailModel> GetUserDetail(long id)
        {
            var user = _userManager.Users.Where(x => x.Id == id).Include(x => x.UserProperties).Include(x => x.Country).Select(x => new
            {
                applicationUser = x,
                UserModel = new UserDetailModel
                {
                    CountryCode = x.Country.Name + " (" + x.Country.PhoneCode + ")",
                    EmailAddress = x.Email,
                    FullName = string.Concat(x.Title ?? "", " ", x.FirstName, " ", x.LastName, " ", x.Suffix ?? ""),
                    Id = x.Id,
                    PhotoPath = "https://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/" + x.PhotoPath,
                    ListProperties = x.UserProperties.Select(x => new Properties
                    {
                        Id = x.Property.Id,
                        City = x.Property.City,
                        Country = x.Property.Country,
                        HouseNumber = x.Property.HouseNumber,
                        LandMark = x.Property.LandMark,
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

        public async Task<AddProperty> GetProperty(long id)
        {
            var prop = await _property.Get(x => x.Id == id).Select(x => new AddProperty
            {
                City = x.City,
                Country = x.Country,
                HouseNumber = x.HouseNumber,
                LandMark = x.LandMark,
                Locality = x.Locality,
                PinCode = x.PinCode,
                PropertyName = x.PropertyName,
                PropertyTypeId = x.PropertyTypeId,
                Street = x.Street,
                Id = x.Id
            }).AsNoTracking().FirstOrDefaultAsync();
            if (prop != null)
                prop.PropertyTypes = await _proprepo.GetAll().Select(x => new SelectItem
                {
                    Id = x.Id,
                    PropertyName = x.PropertyTypeName
                }).ToListAsync();
            else
                throw new BadRequestException("Property Not Found");
            return prop;
        }

        public async Task<bool> UpdateProperty(AddProperty prop)
        {
            var property = _property.Get(x => x.Id == prop.Id).FirstOrDefault();
            var status = false;
            if (property != null)
            {
                property.HouseNumber = prop.HouseNumber;
                property.Country = prop.Country;
                property.City = prop.City;
                property.LandMark = prop.LandMark;
                property.Locality = prop.Locality;
                property.PropertyName = prop.PropertyName;
                property.PropertyTypeId = prop.PropertyTypeId;
                property.Street = prop.Street;
                property.PinCode = prop.PinCode;
                status = Convert.ToBoolean(await _property.Update(property));
            }
            else
                throw new BadRequestException("Property Not Found");
            return status;

        }
    }
}
