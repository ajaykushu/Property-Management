using BusinessLogic.Interfaces;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Http;
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
        private readonly IImageUploadInFile _imageUploadInFile;

        public UserController(IUserService userService, IImageUploadInFile imageUploadInFile)
        {
            _userService = userService;
            _imageUploadInFile = imageUploadInFile;
        }

        /// <summary>
        /// This Controller Action fot Register User
        /// </summary>
        /// <param name="register">Object</param>
        /// <returns>IActionResult</returns>
        ///

        [HttpPost]
        [Route("register")]
        [FeatureBasedAuthorization("Add User")]
        public async Task<ActionResult> Register([FromBody] RegisterUser register)
        {
            //var managerId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            var status = await _userService.RegisterUser(register);
            return Ok(status);
        }

        [HttpGet]
        [Route("getregisterrequestmodel")]
        [FeatureBasedAuthorization("Add User")]
        public ActionResult<RegisterUser> GetRegisterRequestModel()
        {
            return _userService.GetRegisterModel();
        }

        [HttpGet]
        [Route("getedituserrequestmodel")]
        [FeatureBasedAuthorization("Edit User")]
        public async Task<ActionResult<EditUserModel>> GeUserEditRequestModel(long Id)
        {
            return await _userService.GetEditUserModelAsync(Id);
        }

        [HttpPost]
        [Route("updateuser")]
        [FeatureBasedAuthorization("Edit User")]
        public async Task<ActionResult<EditUserModel>> UpdateUser([FromBody] EditUserModel edit)
        {
            var status = await _userService.UpdateUser(edit);
            return Ok(status);
        }

        [HttpPost]
        [Route("uploadAvtar")]
        [FeatureBasedAuthorization("Add User,Edit User")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> UploadAvtar(IFormFile files, string email)
        {
            if (files == null)
            {
                return BadRequest();
            }
            var path = await _imageUploadInFile.UploadAsync(files);
            if (path != null)
            {
                var status = await _userService.UploadAvtar(path, email);
                return Ok(status);
            }
            return StatusCode(500, "Upload Failed");
        }

        [Route("getallusers")]
        [FeatureBasedAuthorization("View Users")]
        [HttpGet]
        public async Task<ActionResult<Pagination<IList<UsersListModel>>>> GetAllUsers(string matchString, FilterEnum filter, int requestedPage)
        {
            //var managerId = HttpContext.User.FindFirstValue(ClaimTypes.Sid);
            var usersList = await _userService.GetAllUsers(requestedPage, filter, matchString);
            return Ok(usersList);
        }

        [HttpGet]
        [FeatureBasedAuthorization("ActDct User")]
        [Route("deact_actuser")]
        public async Task<IActionResult> Deact_Actuser(long userId, int operation)
        {
            if (operation > 1 && operation < 0)
                return BadRequest("Invalid Operation");
            bool status = await _userService.Deact_Actuser(userId, operation);
            return Ok(status);
        }

        [HttpGet]
        [FeatureBasedAuthorization("View User Detail")]
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