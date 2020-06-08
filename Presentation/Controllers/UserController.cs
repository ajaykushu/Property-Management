using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Presentation.ConstModal;
using Presentation.Utility.Interface;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection;

namespace Presentation.Controllers
{
    public class UserController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly IOptions<MenuMapperModel> _menuDetails;
        private readonly string _token;
        private readonly IDetection _detection;

        public UserController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IOptions<MenuMapperModel> menuDetails, IHttpContextAccessor httpContextAccessor, IDetection detection)
        {
            _httpClientHelper = httpClientHelper;
            _apiRoute = apiRoute;
            _menuDetails = menuDetails;
            if (httpContextAccessor.HttpContext.Session.TryGetValue("token", out var token))
            {
                _token = Encoding.UTF8.GetString(token);
            }
            _detection = detection;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterUser register)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _apiRoute.Value.Routes.TryGetValue("register", out string path);
                    var response = await _httpClientHelper.PostFileDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, register, this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        if (await response.Content.ReadAsStringAsync().ConfigureAwait(false) == "true")
                            return Ok(StringConstants.RegisterSuccess);
                        else
                            return Ok(StringConstants.RegisterFailed);
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                        return BadRequest(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return Content("<script language='javascript' type='text/javascript'>location.reload(true);</script>");
                    else
                        return StatusCode((int)response.StatusCode, StringConstants.Error);
                }
                catch (Exception)
                {
                    return StatusCode(500, StringConstants.Error);
                }
            }
            else
            {
                var msg = String.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).Select(y => y.Value.Errors.FirstOrDefault().ErrorMessage));
                return BadRequest(msg);
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserDetailView(long id)
        {
            UserDetail userDetail = new UserDetail();
            try
            {
                _apiRoute.Value.Routes.TryGetValue("userdetail", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?Id=" + id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    userDetail = JsonConvert.DeserializeObject<UserDetail>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/User/Mobile/UserDetailView.cshtml", userDetail);
            return View(userDetail);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            RegisterUser registerrequest = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getregistermodel", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    registerrequest = JsonConvert.DeserializeObject<RegisterUser>(await response.Content.ReadAsStringAsync());
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return Content("<script language='javascript' type='text/javascript'>location.reload(true);</script>");
            }
            catch (Exception)
            {
                registerrequest = new RegisterUser();
                TempData["Error"] = StringConstants.Error;
                return View(registerrequest);
            }
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/User/Mobile/Create.cshtml", registerrequest);
            return View(registerrequest);
        }

        [HttpGet]
        public async Task<ActionResult> EditUserView(long Id)
        {
            EditUser editrequestmodal = null;
            try
            {
                HttpContext.Session.TryGetValue("token", out var token);
                _apiRoute.Value.Routes.TryGetValue("geteditusermodel", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?Id=" + Id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    editrequestmodal = JsonConvert.DeserializeObject<EditUser>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
                editrequestmodal = new EditUser();
                return View(editrequestmodal);
            }
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/User/Mobile/EditUserView.cshtml", editrequestmodal);

            return View(editrequestmodal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(EditUser user)
        {
            string msg = string.Empty;
            if (ModelState.IsValid)
            {
                _apiRoute.Value.Routes.TryGetValue("updateuser", out string path);
                try
                {
                    var response = await _httpClientHelper.PostFileDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, user, this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        if (await response.Content.ReadAsStringAsync().ConfigureAwait(false) == "true")
                            return Ok(StringConstants.SuccessUpdate);
                        else
                            return Ok(StringConstants.UpdateFailed);
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                        return BadRequest(await response.Content.ReadAsStringAsync());
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return Content("<script language='javascript' type='text/javascript'>location.reload(true);</script>");
                    else
                        return StatusCode(500, StringConstants.Error);
                }
                catch (Exception)
                {
                    msg = StringConstants.Error;
                }
            }
            else
                msg = string.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).Select(y => y.Value.Errors.FirstOrDefault().ErrorMessage));
            return BadRequest(msg);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers(string matchString, int requestedPage, FilterEnum filter = FilterEnum.ByEmail)
        {
            ViewBag.searchString = matchString ?? "";
            ViewBag.filter = filter;
            Pagination<IList<UsersList>> usersLists = null;

            _apiRoute.Value.Routes.TryGetValue("getallusers", out string path);
            try
            {
                string parameters = string.Concat("?matchString=", matchString, "&filter=", filter, "&requestedPage=", requestedPage);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + parameters, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    usersLists = JsonConvert.DeserializeObject<Pagination<IList<UsersList>>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                usersLists = new Pagination<IList<UsersList>>
                {
                    Payload = new List<UsersList>()
                };
                TempData["Error"] = StringConstants.Error;
            }
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/User/Mobile/GetAllUsers.cshtml", usersLists);
            return View(usersLists);
        }

        [HttpGet]
        public async Task<IActionResult> DeAct_ActUser(long id, int operation, int page)
        {
            _apiRoute.Value.Routes.TryGetValue("deAct_actUser", out string path);
            try
            {
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?userId=" + id + "&operation=" + operation, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    if (operation == 0)
                        TempData["Success"] = StringConstants.DeactivateSuccessfull;
                    else
                        TempData["Success"] = StringConstants.ActivateSuccessfull;
                }
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }

            return RedirectToAction("GetAllUsers", page);
        }

        [HttpGet]
        public async Task<JsonResult> CheckEmail(string email)
        {
            Boolean result;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("checkemail", out var path);
                var res = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?email=" + email, this, _token);
                if (res.IsSuccessStatusCode)
                {
                    result = Convert.ToBoolean(await res.Content.ReadAsStringAsync());
                    if (result)
                        return Json("Email Already Present");
                }
            }
            catch (Exception) { }
            return Json(true);
        }

        [HttpGet]
        public async Task<JsonResult> CheckPhoneNumber(string phoneNumber)
        {
            Boolean result;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("checkphonenumber", out var path);
                var res = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?phone=" + phoneNumber, this, _token);
                if (res.IsSuccessStatusCode)
                {
                    result = Convert.ToBoolean(await res.Content.ReadAsStringAsync());
                    if (result)
                        return Json("Phone Number Already Present");
                }
            }
            catch (Exception) { }
            return Json(true);
        }

        [HttpGet]
        public async Task<JsonResult> CheckUserName(string userName)
        {
            Boolean result;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("checkusername", out var path);
                var res = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?userName=" + userName, this, _token);
                if (res.IsSuccessStatusCode)
                {
                    result = Convert.ToBoolean(await res.Content.ReadAsStringAsync());
                    if (result)
                        return Json("User Name Already Present");
                }
            }
            catch (Exception) { }
            return Json(true);
        }
    }
}