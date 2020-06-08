using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Presentation.Utility.Interface;
using Presentation.ViewModels;
using Presentation.ViewModels.Controller.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly string _token;
        private readonly IDetection _detection;

        public HomeController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IHttpContextAccessor httpContextAccessor, IDetection detection)
        {
            _httpClientHelper = httpClientHelper;
            _apiRoute = apiRoute;
            if (httpContextAccessor.HttpContext.Session.TryGetValue("token", out var token))
            {
                _token = Encoding.UTF8.GetString(token);
            }
            _detection = detection;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Forbidden()
        {
            return View("AccessDenied");
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationCount()
        {
            int count = 0;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getnotificationcount", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    count = JsonConvert.DeserializeObject<int>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                return Ok(0);
            }
            //getting all notification

            return Ok(count);
        }

        [HttpGet]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            bool status = false;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("markasread", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    status = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                return Ok(false);
            }
            //getting all notification

            return Ok(status);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotificationAsync()
        {
            List<AllNotification> allNotification = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getallnotification", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    allNotification = JsonConvert.DeserializeObject<List<AllNotification>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            //getting all notification
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/Home/Mobile/GetAllNotification.cshtml", allNotification);
            return View(allNotification);
        }
    }
}