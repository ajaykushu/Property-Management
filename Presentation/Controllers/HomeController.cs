using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Presentation.ConstModal;
using Presentation.Utility;
using Presentation.Utility.Interface;
using Presentation.ViewModels;
using Presentation.ViewModels.Controller.Home;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection;

namespace Presentation.Controllers
{
    [ReferrerAttribute]
    public class HomeController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly string _token;
        private readonly IDetection _detection;

        public HomeController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IHttpContextAccessor httpContextAccessor, IDetection detection,IDistributedCache _session)
        {
            _httpClientHelper = httpClientHelper;
            _apiRoute = apiRoute;
            if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var id = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid).Value;
                var returnval = ObjectByteConverter.Deserialize<SessionStore>(_session.Get(id));
                if (returnval != null)
                    _token = returnval.Token;

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
        [Authorize]
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

        public async Task<IActionResult> GetUserEmail()
        {
            List<UserList> allNotification = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getUserEmail", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode) {
                    allNotification = JsonConvert.DeserializeObject<List<UserList>>(await response.Content.ReadAsStringAsync());
                    return StatusCode((int)HttpStatusCode.OK,allNotification);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
            //getting all notification
            
            return BadRequest("Unable to fetch");

        }

        [HttpGet]
        [Authorize]
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
        public  IActionResult SchedularView()
        {
            return PartialView("~/Views/Shared/SchedulerView.cshtml");
        }
        [HttpGet]
        public IActionResult ValidateDateEqualOrGreater(DateTime duedate)
        {
            if (duedate.Date.CompareTo(DateTime.Now.Date) >0)
            {
                return Json(true);
            }
            return Json(false);
        } 
        [HttpGet]
        public async Task<IActionResult> DashBoard()
        {
            //getting the proprtyList
            DashBoard dashBoard = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("GetDashboard", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    dashBoard = JsonConvert.DeserializeObject<DashBoard>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            //getting all notification
            if (_detection.Device.Type == DeviceType.Mobile)
            {
                return View("~/Views/Home/Mobile/Dashboard.cshtml", dashBoard);
            }
           
            return View(dashBoard);
        }
        [HttpGet]
        public async Task<IActionResult> Locations(long Id)
        {
            //getting the proprtyList
            List<LoctionDetail> dashBoard = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("locationView", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path+"?Id="+Id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    dashBoard = JsonConvert.DeserializeObject<List<LoctionDetail>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            //getting all notification

            return PartialView("DashboardRoomView",dashBoard);
        }

        [HttpGet]
        public async Task<IActionResult> Sublocation(long Id)
        {
            //getting the proprtyList
            List<SubLocationModel> dashBoard = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("sublocation", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?Id=" + Id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    dashBoard = JsonConvert.DeserializeObject<List<SubLocationModel>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            //getting all notification
            return PartialView("Sublocation", dashBoard);
        }

        [HttpGet]
        public async Task<IActionResult>  WorkOrderList(long sublocId)
        {
            List<string> wo = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("wolist", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + sublocId, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    wo = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            //getting all notification
            return PartialView(wo);
        }


    }
}