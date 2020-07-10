using Microsoft.AspNetCore.Authorization;
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
using System.Threading.Tasks;
using Wangkanai.Detection;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Presentation.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly IOptions<MenuMapperModel> _menuDetails;
        private readonly ISessionStorage _sessionStorage;
        private readonly IDetection _detection;

        public LoginController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IOptions<MenuMapperModel> menuDetails, ISessionStorage sessionStorage, IDetection detection)
        {
            _httpClientHelper = httpClientHelper;
            _apiRoute = apiRoute;
            _menuDetails = menuDetails;
            _sessionStorage = sessionStorage;
            _detection = detection;
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

            return View();
        }

        public IActionResult Mobile()
        {
            ViewBag.SubTitle = "My Property";
            ViewBag.isEnabled = true;
            return View("~/Views/Shared/Mobile/Menu.cshtml");
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
            //var consentFeature = HttpContext.Features.Get<ITrackingConsentFeature>();
            //if (!consentFeature.HasConsent)
            //{
            //    TempData["Info"] = "accept cookie";
            //    return RedirectToAction("Privacy", "Home");
            //}
            if (ModelState.IsValid)
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
                    HttpContext.Session.SetString("UId",
                                                  tokenResponse.UId + "");

                    Dictionary<string, List<MenuProperty>> menuView = new Dictionary<string, List<MenuProperty>>();
                    MakeDictionaryFormenuView(tokenResponse, menuView);
                    _sessionStorage.AddItem(tokenResponse.UId, tokenResponse.MenuItems);
                    HttpContext.Session.SetString(
                    "menu",
                    JsonConvert.SerializeObject(menuView)
                    );
                    HttpContext.Session.SetString(
                   "role",
                   tokenResponse.Roles.First()
                   );

                    if (tokenResponse.Roles.Contains("Master Admin") && _detection.Device.Type != DeviceType.Mobile)
                        return RedirectToAction("GetAllUsers", "User");
                    else
                        return RedirectToAction("Index", "WorkOrder");
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
                TempData["Error"] = String.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
                return RedirectToAction("Index");
            }
        }

        private void MakeDictionaryFormenuView(TokenResponse tokenResponse, Dictionary<string, List<MenuProperty>> menuView)
        {
            foreach (var menu in _menuDetails.Value.Menus)
            {
                if (tokenResponse.MenuItems.Contains(menu.Key))
                {
                    menu.Value.Enabled = true;
                    if (!menuView.ContainsKey(menu.Value.MainMenuName))
                    {
                        var templist = new List<MenuProperty>
                        {
                            menu.Value
                        };
                        menuView.Add(menu.Value.MainMenuName, templist);
                    }
                    else
                        menuView[menu.Value.MainMenuName].Add(menu.Value);
                }
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
            var Id = Convert.ToInt64(HttpContext.Session.GetString("UId"));
            _sessionStorage.RemoveItem(Id);
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
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/Login/Mobile/ForgotPassword.cshtml");
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
            }
            else
            {
                TempData["Error"] = String.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
            }
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/Login/Mobile/ForgotPassword.cshtml");
            return View("ForgotPassword");
        }

        /// <summary>
        /// This Controller Action returns Post method for token recognition and return View to ForgetPassword.
        /// </summary>
        /// <param name="token">string</param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        public IActionResult PasswordResetView(string token, string email)
        {
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/Login/Mobile/PasswordResetView.cshtml", new PasswordChange() { Email = email, Token = token });
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
                TempData["Error"] = String.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
                return RedirectToAction("PasswordResetView", "Login");
            }
        }
    }
}