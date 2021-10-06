using API.Authentication.Interfaces;
using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Login.RequestModels;
using Models.ResponseModels;
using Models.User.RequestModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using Utilities.CustomException;

namespace API.Authentication
{
    public class User : IUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly IEmailSender _emailSender;
        private readonly IRepo<RoleMenuMap> _roleMenuMap;
        private readonly IUserBL _user;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _scheme;

        public User(UserManager<ApplicationUser> userManager, ITokenGenerator tokenGenerator, IEmailSender emailSender,
               IRepo<RoleMenuMap> roleMenuMap, IUserBL user, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _emailSender = emailSender;
            _roleMenuMap = roleMenuMap;
            _user = user;
            _httpContextAccessor = httpContextAccessor;
            _scheme = _httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://";
        }

        public async Task<bool> GetPasswordChangeTokenAsync(string email, string verificationPath)
        {
            ApplicationUser user;

            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new BadRequestException("Email not Found");
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<html> <h1>Click on change to change Password</h1> <p this link is valid for limited peroid of time please change as soon as possible></p><a href='").Append(verificationPath).Append("?token=").Append(token).Append("&email=").Append(email).Append("'>change</a></html>");
            var message = stringBuilder.ToString();
            await _emailSender.SendAsync(email, "Password Change Token", message, true);
            return true;
        }

        public async Task<bool> ChangePassowrd(string email, string token, string password)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, token, password);
                if (identityResult.Succeeded)
                {
                    return true;
                }
                else
                {
                    throw new BadRequestException(identityResult.Errors.Select(x => x.Description).Aggregate((i, j) => i + ", " + j));
                }
            }
            else
            {
                throw new BadRequestException("User Not Found");
            }
        }

        public async Task<TokenResponseModel> DoLogin(LoginUserDTO loginDTO)
        {
            if (_userManager.Users.Count() == 0)
            {
                var newuser = new RegisterUserDTO
                {
                   
                    Email = "test@test.com",
                    PhoneNumber = "7777777777",
                    FirstName = "Test",
                    LastName = "Last",
                    Language = 1,
                    Role = "MASTER ADMIN",
                    DepartmentId = 1,

                    Password = "Testuser123@",
                    File = null,
                    OfficeExt = "234",
                };
                await _user.RegisterUser(newuser);
            }
            ApplicationUser identityUser;
            TokenResponseModel returnToken;

            identityUser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (identityUser != null)
            {
                if (!await _userManager.CheckPasswordAsync(identityUser, loginDTO.Password))
                    throw new UnAuthorizedException("Invalid Password");
                else
                {
                    if (!identityUser.IsActive)
                    {
                        throw new UnAuthorizedException("Your Account Has been Suspended");
                    }
                    var roles = await _userManager.GetRolesAsync(identityUser) ?? new List<string>();

                    var submenu = _roleMenuMap.GetAll().Include(x => x.Role).Where(x => roles.Contains(x.Role.Name)).Include(x => x.Menu).Select(x => x.Menu.MenuName).ToHashSet();

                    var claims = _tokenGenerator.GetClaims(identityUser, submenu, roles);

                    returnToken = new TokenResponseModel()
                    {
                        FullName = identityUser.FirstName + " " + identityUser.LastName + " " + identityUser.Suffix,
                        UId = identityUser.Id,
                        IsEffortVisible=identityUser.IsEffortVisible,
                        Roles = roles.ToHashSet(),
                        Token = _tokenGenerator.GetToken(claims),
                        MenuItems = submenu,
                        PhotoPath = !string.IsNullOrWhiteSpace(identityUser.PhotoPath) ? _scheme + _httpContextAccessor.HttpContext.Request.Host.Value + "/api/" + identityUser.PhotoPath : ""
                    };
                }
            }
            else
                throw new UnAuthorizedException("Email Id not Registered. Please Contact Admin");

            return returnToken;
        }

        public async Task<HashSet<string>> GetMenuData()
        {
           var userId= _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user) ?? new List<string>();
            var submenu = _roleMenuMap.GetAll().Include(x => x.Role).Where(x => roles.Contains(x.Role.Name)).Include(x => x.Menu).Select(x => x.Menu.MenuName).ToHashSet();
            return submenu;
        }
    }
}