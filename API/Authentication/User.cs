﻿using API.Authentication.Interfaces;
using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.RequestModels;
using Models.ResponseModels;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IUserService _user;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public User(UserManager<ApplicationUser> userManager, ITokenGenerator tokenGenerator, IEmailSender emailSender,
               IRepo<RoleMenuMap> roleMenuMap, IUserService user, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _emailSender = emailSender;
            _roleMenuMap = roleMenuMap;
            _user = user;
            _httpContextAccessor = httpContextAccessor;




        }
        public async Task<bool> GetPasswordChangeTokenAsync(string email, string verificationPath)
        {
            ApplicationUser user;

            user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new BadRequestException("Email not Found");
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailSender.SendAsync(email,
                "Password Change Token",
                "<html>" +
                "<h1>Click on change to change Password</h1>" +
                "<p this link is valid for limited peroid of time please change as soon as possible></p>" +
                "<a href='" + verificationPath + "?token=" + token + "&email=" + email + "'>change</a>" +
                "</html>",
                true
                );
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
        public async Task<TokenResponseModel> DoLogin(LoginUserModel loginDTO)
        {
            if (_userManager.Users.Count() == 0)
            {
                await _user.RegisterUser(new RegisterUser
                {
                    UserName = "TestUser",
                    Email = "test@test.com",
                    PhoneNumber = "7777777777",
                    FirstName = "Test",
                    LastName = "Last",
                    Language = 1,
                    Role = "Admin",
                    Password = "Testuser123@",
                    TimeZone = "India +5:30"
                });
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


                    var claims = _tokenGenerator.GetClaims(identityUser, submenu);

                    returnToken = new TokenResponseModel()
                    {
                        FullName = identityUser.FirstName + " " + identityUser.LastName + " " + identityUser.Suffix,
                        UId = identityUser.Id,
                        Roles = roles.ToHashSet(),
                        Token = _tokenGenerator.GetToken(claims),
                        MenuItems = submenu,
                        PhotoPath = "https://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/" + identityUser.PhotoPath
                    };

                }

            }
            else
                throw new UnAuthorizedException("Email Id not Registered. Please Contact Admin");

            return returnToken;
        }


    }
}
