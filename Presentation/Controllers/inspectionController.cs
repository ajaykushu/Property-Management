using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Presentation.ConstModal;
using Presentation.Utility;
using Presentation.Utility.Interface;
using Presentation.ViewModels;
using Wangkanai.Detection;

namespace Presentation.Controllers
{
    public class InspectionController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly string _token;
        private readonly IDetection _detection;

        public InspectionController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IHttpContextAccessor httpContextAccessor, IDetection detection, IDistributedCache _session)
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
        [HttpPost]
        
        public async Task<IActionResult> Inspection(Inspection inspect)
        {
            string sys_id="" ;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("createinspection", out string path);
                var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path ,inspect ,this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    sys_id = await response.Content.ReadAsStringAsync();
            }
            catch (Exception )
            {
                TempData["Error"] = StringConstants.Error;
            }
            return RedirectToAction("CheckList",new {id= sys_id,propertyId=inspect.PropertyId });
        }
        [HttpGet]
        public async Task<IActionResult> Inspection()
        {
            Inspection list = new Inspection();
            try
            {
                _apiRoute.Value.Routes.TryGetValue("inspection", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    list = JsonConvert.DeserializeObject<Inspection>(await response.Content.ReadAsStringAsync());
            }

            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> CheckList(CheckList check)
        {
            var ret = false;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("checklist", out string path);
                var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path,check, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    ret = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }

            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> CheckList(string id, long propertyId)
        {
            CheckList chkl = new CheckList();
            List<SelectItem> result= new List<SelectItem>();
            chkl.InspectionId = id;
            
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getlocation", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + propertyId, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    chkl.Locations = JsonConvert.DeserializeObject<List<SelectItem>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
            }
            
            return View(chkl);
        }
        [HttpGet]
        public async Task<IActionResult> Item(string Id)
        {
            List<Grouped> grp = new List<Grouped>();
            try
            {
                _apiRoute.Value.Routes.TryGetValue("checklist", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "/" + Id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    grp = JsonConvert.DeserializeObject<List<Grouped>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            return PartialView(grp);
        }
        [HttpPost]
        public async Task<IActionResult> Item(CheckList check)
        {
            List<Grouped> grp = new List<Grouped>();
            try
            {
                _apiRoute.Value.Routes.TryGetValue("checklist", out string path);
                var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path,check,this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    grp = JsonConvert.DeserializeObject<List<Grouped>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            return PartialView(grp);
        }

        public async Task<IActionResult> Inspections()
        {
            List<Inspections> grp = new List<Inspections>();
            try
            {
                _apiRoute.Value.Routes.TryGetValue("inspections", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    grp = JsonConvert.DeserializeObject<List<Inspections>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            return View(grp);
            
        }


    }
}