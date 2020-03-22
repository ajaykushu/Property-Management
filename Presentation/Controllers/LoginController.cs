using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Presentation.ConstModal;
using Presentation.Utility.Interface;
using Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentation.Controllers
{

    public class LoginController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly IOptions<MenuMapperModel> _menuDetails;


        public LoginController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IOptions<MenuMapperModel> menuDetails)
        {
            _httpClientHelper = httpClientHelper;
            _apiRoute = apiRoute;
            _menuDetails = menuDetails;

        }
        /// <summary>
        /// This Controller Action returns View to LogIn.
        /// </summary>
        /// <param name=""></param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            HttpContext.Session.TryGetValue("token", out var token);
            if (token != null)
            {
                return RedirectToAction("Index", "WorkOrder");
            }
            //to test if the Browser had enabled cookie.
            HttpContext.Session.SetString("cookie", "t@st");
            return View();
        }
        /// <summary>
        /// This Controller Action is Post method for Login.
        /// </summary>
        /// <param name="login">Login</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginExist(LoginReq login)
        {
           
            var consentFeature = HttpContext.Features.Get<ITrackingConsentFeature>();
            if (!consentFeature.HasConsent)
            {
                TempData["Info"] = "accept cookie";
                return RedirectToAction("Privacy", "Home");
            }
            else if (ModelState.IsValid)
            {
                _apiRoute.Value.Routes.TryGetValue("userlogin", out string path);
                var response = await _httpClientHelper.PostDataAsync(
                    _apiRoute.Value.ApplicationBaseUrl + path
                    , login, this).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {

                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(
                        await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    HttpContext.Session.SetString("token",
                                                  tokenResponse.Token);
                    HttpContext.Session.SetString("name",
                                                  tokenResponse.FullName);
                    HttpContext.Session.SetString("imagePath",
                                                  tokenResponse.PhotoPath);

                    Dictionary<string, HashSet<MenuProperty>> menuView = new Dictionary<string, HashSet<MenuProperty>>();
                    MakeDictionaryFormenuView(tokenResponse, menuView);
                    HttpContext.Session.SetString(
                    "menu",
                    JsonConvert.SerializeObject(menuView)
                    );
                    if (tokenResponse.Roles.Contains("User"))
                        return RedirectToAction("Index", "WorkOrder");
                    else
                        return RedirectToAction("GetAllUsers", "User");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    TempData["Error"] = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = StringConstants.Error;
                    return RedirectToAction("Index");
                }

            }

            else
            {
                TempData["Error"] = ModelState.Where(x => x.Value.Errors.Count > 0).Select(y => y.Value.Errors.FirstOrDefault().ErrorMessage);
                return RedirectToAction("Index");
            }
        }

        private void MakeDictionaryFormenuView(TokenResponse tokenResponse, Dictionary<string, HashSet<MenuProperty>> menuView)
        {
            foreach (var items in tokenResponse.MenuItems)
                foreach (var submenu in items.Value)
                    if (_menuDetails.Value.Menus.ContainsKey(submenu) && _menuDetails.Value.Menus[submenu].Enabled == false)
                        if (!menuView.ContainsKey(items.Key))
                        {
                            var list = new HashSet<MenuProperty>
                            {
                                _menuDetails.Value.Menus[submenu]
                            };
                            _menuDetails.Value.Menus[submenu].Enabled = true;
                            menuView.Add(items.Key, list);
                        }
                        else
                        {
                            menuView[items.Key].Add(_menuDetails.Value.Menus[submenu]);
                            _menuDetails.Value.Menus[submenu].Enabled = true;
                        }
            foreach (var item in _menuDetails.Value.Menus)
                item.Value.Enabled = false;
        }

        /// <summary>
        /// This Controller Action Logs Out and returns View to LogIn.
        /// </summary>
        /// <param name=""></param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            _httpClientHelper.RemoveHeader();
            return RedirectToAction("Index", "Login", null);
        }
        /// <summary>
        /// This Controller Action returns View to Forget Password Screen.
        /// </summary>
        /// <param name=""></param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            HttpContext.Session.Clear();
            return View();
        }
        /// <summary>
        /// This Controller Action Post Method for Email based token generation for Password Change.
        /// </summary>
        /// <param name="login">Login</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SendEmailForPasswordReset(LoginReq login)
        {
            if (!String.IsNullOrWhiteSpace(login.Email))
            {

                string verPath = "https://" + HttpContext.Request.Host.Value + "/Login/PasswordResetView";
                _apiRoute.Value.Routes.TryGetValue("sendforgotpwdemail", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?email=" + login.Email + "&verificationPath=" + verPath, this).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = StringConstants.EmailSent;
                }
                else
                {
                    TempData["Error"] = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                return View("ForgotPassword");
            }
            else
            {
                TempData["error"] = ModelState.Where(x => x.Value.Errors.Count > 0).Select(y => y.Value.Errors.FirstOrDefault().ErrorMessage);
                return View();
            }

        }
        /// <summary>
        /// This Controller Action returns Post method for token recognition and return View to ForgetPassword.
        /// </summary>
        /// <param name="token">string</param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public IActionResult PasswordResetView(string token, string email)
        {
            return View(new PasswordChange() { Email = email, Token = token });
        }
        /// <summary>
        /// This Controller Action updates the new Password of User.
        /// </summary>
        /// <param name="resetPassword">ResetPassword</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(PasswordChange resetPassword)
        {

            if (ModelState.IsValid)
            {
                if (resetPassword.Token.Contains(" ")) resetPassword.Token = resetPassword.Token.Replace(" ", "+");
                _apiRoute.Value.Routes.TryGetValue("changepassword", out string path);
                var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, resetPassword, this).ConfigureAwait(false);
                if (response.IsSuccessStatusCode && await response.Content.ReadAsStringAsync().ConfigureAwait(false) == "true")
                {
                    TempData["Success"] = StringConstants.PwdChanged;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = StringConstants.PwdNotChangedError;
                    return RedirectToAction("ForgotPassword", "Login");
                }
            }
            else
            {
                TempData["Error"] = ModelState.Where(x => x.Value.Errors.Count > 0).Select(y => y.Value.Errors.FirstOrDefault().ErrorMessage);
                return RedirectToAction("PasswordResetView", "Login");
            }

        }


    }
}
