using BusinessLogic.Interfaces;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;
using Utilities.Interface;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IConfiguration Configuration;
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// This Controller Action fot Register User
        /// </summary>
        /// <param name="register">Object</param>
        /// <returns>IActionResult</returns>
        ///

        [HttpPost]
        [Route("register")]
        [FeatureBasedAuthorization(MenuEnum.Add_User)]
        public async Task<ActionResult> Register([FromForm] RegisterUser user)
        {
            var status = await _userService.RegisterUser(user);
            return Ok(status);
        }

        [HttpGet]
        [Route("getregisterrequestmodel")]
        [FeatureBasedAuthorization(MenuEnum.Add_User)]
        public ActionResult<RegisterUser> GetRegisterRequestModel()
        {
            return _userService.GetRegisterModel();
        }

        [HttpGet]
        [Route("getedituserrequestmodel")]
        [FeatureBasedAuthorization(MenuEnum.Edit_User)]
        public async Task<ActionResult<EditUserModel>> GeUserEditRequestModel(long Id)
        {
            return await _userService.GetEditUserModelAsync(Id);
        }

        [HttpPost]
        [Route("updateuser")]
        [FeatureBasedAuthorization(MenuEnum.Edit_User)]
        public async Task<ActionResult<EditUserModel>> UpdateUser([FromForm] EditUserModel user)
        {
            var status = await _userService.UpdateUser(user);
            return Ok(status);
        }

        [Route("getallusers")]
        [FeatureBasedAuthorization(MenuEnum.View_Users)]
        [HttpGet]
        public async Task<ActionResult<Pagination<IList<UsersListModel>>>> GetAllUsers(string matchString, FilterEnum filter, int requestedPage)
        {
            //var managerId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            var usersList = await _userService.GetAllUsers(requestedPage, filter, matchString);
            return Ok(usersList);
        }

        [HttpGet]
        [FeatureBasedAuthorization(MenuEnum.ActDct_User)]
        [Route("deact_actuser")]
        public async Task<IActionResult> Deact_Actuser(long userId, int operation)
        {
            if (operation > 1 && operation < 0)
                return BadRequest("Invalid Operation");
            bool status = await _userService.Deact_Actuser(userId, operation);
            return Ok(status);
        }

        [HttpGet]
        [FeatureBasedAuthorization(MenuEnum.View_User_Detail)]
        [Route("userdetail")]
        public async Task<IActionResult> GetUserDetail(long Id)
        {
            UserDetailModel userDetailModel = await _userService.GetUserDetail(Id);
            return Ok(userDetailModel);
        }

        [HttpGet]
        [Route("checkemail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            var res = await _userService.CheckEmail(email);
            return res;
        }

        [HttpGet]
        [Route("checkphonenumber")]
        public async Task<ActionResult<bool>> CheckPhoneNumber(string phone)
        {
            var res = await _userService.CheckPhoneNumber(phone);
            return res;
        }

        [HttpGet]
        [Route("checkusername")]
        public async Task<ActionResult<bool>> CheckUserName(string userName)
        {
            var res = await _userService.CheckUserName(userName);
            return res;
        }
    }
}