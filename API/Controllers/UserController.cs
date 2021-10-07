using BusinessLogic.Interfaces;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.User.RequestModels;
using Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;
using Utilities.Interface;
using DataTransferObjects.ResponseModels;
using Models.ResponseModels.User;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IConfiguration Configuration;
        private readonly IUserBL _userService;
        private readonly INotifier _notifier;

        public UserController(IUserBL userService, INotifier notifier)
        {
            _userService = userService;
            _notifier = notifier;
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
        public async Task<ActionResult> Register([FromForm] RegisterUserDTO user)
        {
            var status = await _userService.RegisterUser(user);
            return Ok(status);
        }

        [HttpGet]
        [Route("getregisterrequestmodel")]
        [FeatureBasedAuthorization(MenuEnum.Add_User)]
        public async Task<ActionResult<RegisterUserDTO>> GetRegisterRequestModel()
        {
            return await _userService.GetRegisterModel();
        }

        [HttpGet]
        [Route("getedituserrequestmodel")]
        [FeatureBasedAuthorization(MenuEnum.Edit_User)]
        public async Task<ActionResult<EditUserDTO>> GeUserEditRequestModel(long Id)
        {
            return await _userService.GetEditUserModelAsync(Id);
        }

        [HttpPost]
        [Route("updateuser")]
        [FeatureBasedAuthorization(MenuEnum.Edit_User)]
        public async Task<ActionResult<EditUserDTO>> UpdateUser([FromForm] EditUserDTO user)
        {
            var status = await _userService.UpdateUser(user);
            return Ok(status);
        }

        [Route("getallusers")]
       
        [FeatureBasedAuthorization(MenuEnum.View_Users)]
        [ResponseCache(NoStore = true, Duration = 0)]
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

        [HttpGet]
        [Route("getallnotification")]
        public async Task<ActionResult<List<AllNotification>>> GetAllNotification()
        {
            var res = await _userService.GetAllNotification();
            return res;
        }

        //creating server events
        [HttpGet]
        [Route("getnotificationcount")]
        public async Task<ActionResult<int>> GetNotificationCount()
        {
            int res = await _userService.GetNotificationCount();
            return Ok(res);
        }

        [HttpGet]
        [Route("markasread")]
        public async Task<ActionResult<int>> MarkAsRead(int id)
        {
            bool res = await _userService.MarkAsRead(id);
            return Ok(res);
        }

        [HttpGet]
        [Route("getUserEmail")]
        public async Task<ActionResult<List<UserList>>> GetUserEmail()
        {
            var res = await _userService.GetUserEmail();
            return Ok(res);
        }

        [HttpGet]
        [Route("gettimesheet")]
        public async Task<ActionResult<List<TimeSheet>>> GetTimeSheet()
        {
            List<TimeSheet> res = await _userService.GetTimeSheet();
            return Ok(res);
        }
        [HttpGet]
        [Route("gettimesheet/{id}")]
        public async Task<ActionResult<List<TimeSheet>>> GetTimeSheet(string id)
        {
            var res = await _userService.GetTimeSheet( id);
            return Ok(res);
        }

        [HttpPost]
        [Route("saveeffort")]
        public async Task<ActionResult<bool>> SaveEffort(List<TimesheetBreakDown> timesheetBreakDown)
        {
            bool res = await _userService.SaveEffort(timesheetBreakDown);
            return Ok(res);
        }

        [HttpPost]
        [Route("changetz")]
        public async Task<ActionResult<bool>> ChangeTZ([FromForm] UserDetailModel udm)
        {
            bool res = await _userService.ChangeTZ(udm.TimeZone, udm.Id);
            return Ok(res);
        }

    }
}