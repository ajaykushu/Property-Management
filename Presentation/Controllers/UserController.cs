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

namespace Presentation.Controllers
{

    public class UserController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly IOptions<MenuMapperModel> _menuDetails;
        private readonly string _token;

        public UserController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IOptions<MenuMapperModel> menuDetails, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientHelper = httpClientHelper;
            _apiRoute = apiRoute;
            _menuDetails = menuDetails;
            if (httpContextAccessor.HttpContext.Session.TryGetValue("token", out var token))
            {
                _token = Encoding.UTF8.GetString(token);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterRequest register)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _apiRoute.Value.Routes.TryGetValue("register", out string path);
                    var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, register, this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode && await response.Content.ReadAsStringAsync().ConfigureAwait(false) == "true")
                    {
                        if (register.Photo != null)
                        {
                            _apiRoute.Value.Routes.TryGetValue("uploadimage", out string imageuploadpath);
                            var res = await _httpClientHelper.PostFileDataAsync(_apiRoute.Value.ApplicationBaseUrl + imageuploadpath, register.Email, register.Photo, this, _token);
                            if (res.IsSuccessStatusCode)
                                return Ok(StringConstants.RegisterSuccess);
                            else
                                return Ok(StringConstants.ImageFailed);
                        }
                        else
                            return Ok(StringConstants.RegisterSuccess);

                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                        return BadRequest(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
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
            UserDetailModel userDetail = new UserDetailModel();
            try
            {
                _apiRoute.Value.Routes.TryGetValue("userdetail", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?Id=" + id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    userDetail = JsonConvert.DeserializeObject<UserDetailModel>(await response.Content.ReadAsStringAsync());

            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            return View(userDetail);
        }

        [HttpGet]
        public async Task<ActionResult> Create()
        {
            RegisterRequest registerrequest = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getregistermodel", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    registerrequest = JsonConvert.DeserializeObject<RegisterRequest>(await response.Content.ReadAsStringAsync());

            }
            catch (Exception)
            {
                registerrequest = new RegisterRequest();
                TempData["Error"] = StringConstants.Error;
                return View(registerrequest);
            }
            return View(registerrequest);
        }
        [HttpGet]
        public async Task<ActionResult> AddPropertyView()
        {
            AddProperty addProperty = new AddProperty();
            try
            {
                HttpContext.Session.TryGetValue("token", out var token);
                _apiRoute.Value.Routes.TryGetValue("getPropertyTypes", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    addProperty = JsonConvert.DeserializeObject<AddProperty>(await response.Content.ReadAsStringAsync());


            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            return PartialView("_AddPropertyView", addProperty);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProperty(AddProperty model)
        {
            if (ModelState.IsValid)
            {
                _apiRoute.Value.Routes.TryGetValue("addproperty", out string path);
                try
                {
                    var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, model, this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode && await response.Content.ReadAsStringAsync() == "true")
                        return Ok(StringConstants.SuccessSaved);
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                    else
                        return StatusCode((int)HttpStatusCode.InternalServerError, StringConstants.Error);
                }
                catch (Exception)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, StringConstants.Error);
                }
            }
            else
            {
                var msg = String.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).Select(y => y.Value.Errors.FirstOrDefault().ErrorMessage));
                return StatusCode(500, msg);
            }
        }
        [HttpGet]
        public async Task<ActionResult> ListProperties()
        {
            _apiRoute.Value.Routes.TryGetValue("listproperties", out string path);
            List<Properties> properties = new List<Properties>();
            try
            {
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    properties = JsonConvert.DeserializeObject<List<Properties>>(await response.Content.ReadAsStringAsync());

            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }

            return View(properties);
        }
        [HttpGet]
        public async Task<ActionResult> DeleteProperty(int id)
        {
            if (id == 0)
            {
                return BadRequest("invalid property");
            }
            try
            {
                _apiRoute.Value.Routes.TryGetValue("deleteproperty", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    TempData["Success"] = StringConstants.DeleteSuccess;

            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            return RedirectToAction("ListProperties");
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
                    HttpContext.Session.TryGetValue("token", out var token);
                    var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, user, this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode && await response.Content.ReadAsStringAsync().ConfigureAwait(false) == "true")
                    {
                        if (user.Photo != null)
                        {
                            _apiRoute.Value.Routes.TryGetValue("uploadimage", out string imageuploadpath);
                            var res = await _httpClientHelper.PostFileDataAsync(_apiRoute.Value.ApplicationBaseUrl + imageuploadpath, user.Email, user.Photo, this, _token);
                            if (res.IsSuccessStatusCode)
                                return Ok(StringConstants.ImageUpload);
                            else
                                return Ok(StringConstants.ImageFailed);
                        }
                        else
                            return Ok(StringConstants.SuccessUpdate);
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                        return BadRequest(await response.Content.ReadAsStringAsync());
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
                string parameters = "?matchString=" + matchString + "&filter=" + filter + "&requestedPage=" + requestedPage;
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
            return View(usersLists);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProperty(AddProperty prop)
        {
            _apiRoute.Value.Routes.TryGetValue("updateproperty", out string path);
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, prop, this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                        TempData["Success"] = StringConstants.SuccessUpdate;

                }
                catch (Exception)
                {
                    TempData["Error"] = StringConstants.Error;
                }
            }
            else
            {
                TempData["Error"] = string.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).Select(y => y.Value.Errors.FirstOrDefault().ErrorMessage));
            }

            return RedirectToAction("PropertyEditView", new { id = prop.Id });

        }
        [HttpGet]
        public async Task<IActionResult> PropertyEditView(long id)
        {
            _apiRoute.Value.Routes.TryGetValue("getproperty", out string path);
            AddProperty prop = new AddProperty();
            try
            {
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    prop = JsonConvert.DeserializeObject<AddProperty>(await response.Content.ReadAsStringAsync());


            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            return View(prop);
        }

        [HttpGet]
        public async Task<IActionResult> DeAct_ActUser(long userId, int operation, int page)
        {

            _apiRoute.Value.Routes.TryGetValue("deAct_actUser", out string path);
            try
            {
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?userId=" + userId + "&operation=" + operation, this, _token).ConfigureAwait(false);
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


    }
}
