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
using Presentation.ViewModels.Controller.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection;

namespace Presentation.Controllers
{
    
    public class PropertyController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly string _token;
        private readonly IDetection _detection;

        public PropertyController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IHttpContextAccessor httpContextAccessor, IDetection detection,IDistributedCache _session)
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

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> AddPropertyView()
        {
            PropertyOperation addProperty = new PropertyOperation();
            try
            {
                HttpContext.Session.TryGetValue("token", out var token);
                _apiRoute.Value.Routes.TryGetValue("getPropertyTypes", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    addProperty = JsonConvert.DeserializeObject<PropertyOperation>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/Property/Mobile/AddPropertyView.cshtml", addProperty);
            return View("AddPropertyView", addProperty);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProperty(PropertyOperation model)
        {
            if (ModelState.IsValid)
            {
                _apiRoute.Value.Routes.TryGetValue("addproperty", out string path);
                try
                {
                    var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, model, this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode && await response.Content.ReadAsStringAsync() == "true")
                        return Ok(StringConstants.CreatedSuccess);
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return Content("<script language='javascript' type='text/javascript'>location.reload(true);</script>");
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
               string msg = String.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
                return StatusCode(500, msg);
            }
        }

        [HttpGet]
        
        [Authorize]
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
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/Property/Mobile/ListProperties.cshtml", properties);
            return View(properties);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> ActDeactProperty(int id, bool operation)
        {
            if (id == 0)
            {
                //TempData["Error"] = "Unable to Activate/Deactivate property";
                return RedirectToAction("ListProperties");
            }
            try
            {
                _apiRoute.Value.Routes.TryGetValue("actdeactproperty", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id + "&operation=" + operation, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    
                }
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            return RedirectToAction("ListProperties");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> UpdateProperty(PropertyOperation prop)
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
                TempData["Error"] = String.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
            }

            return RedirectToAction("ListProperties", new { id = prop.Id });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> PropertyEditView(long id)
        {
            _apiRoute.Value.Routes.TryGetValue("getproperty", out string path);
            PropertyOperation prop = new PropertyOperation();
            try
            {
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    prop = JsonConvert.DeserializeObject<PropertyOperation>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/Property/Mobile/PropertyEditView.cshtml", prop);
            return View(prop);
        }

        [HttpGet]
        [Authorize]
        public async Task<JsonResult> CheckProperty(string propertyName)
        {
            Boolean result;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("checkproperty", out var path);
                var res = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?propertyName=" + propertyName, this, _token);
                if (res.IsSuccessStatusCode)
                {
                    result = Convert.ToBoolean(await res.Content.ReadAsStringAsync());
                    if (result)
                        return Json("Property Name Already Present");
                }
            }
            catch (Exception) { }
            return Json(true);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> PropertyConfig(long Id)
        {
            PropertyConfig result = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("propertyconfig", out var path);
                var res = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?Id=" + Id, this, _token);
                if (res.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<PropertyConfig>(await res.Content.ReadAsStringAsync());
                }
            }
            catch (Exception) { }
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/Property/Mobile/PropertyConfig.cshtml", result);
            return View("PropertyConfig", result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PropertyConfig(PropertyConfig propertyConfig)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _apiRoute.Value.Routes.TryGetValue("propertyconfig", out var path);
                    var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, propertyConfig, this, _token);
                    if (response.IsSuccessStatusCode)
                    {
                        var status = await response.Content.ReadAsStringAsync();
                        if (status == "true")
                            TempData["Success"] = StringConstants.CreatedSuccess;
                    }
                }
                catch (Exception)
                {
                    TempData["Error"] = StringConstants.Error;
                }
            }
            else
            {
                TempData["Error"] = String.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
            }
            return RedirectToAction("PropertyConfig", new { Id = propertyConfig.PropertyId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSubLocation(long id)
        {
            List<SelectItem> result = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getsublocation", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    result = JsonConvert.DeserializeObject<List<SelectItem>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
            }
            return Ok(result);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetPropertyData(long id)
        {
            string result = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getpropertydata", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception)
            {
            }
            return Ok(result);
        }

        [HttpPost]
        
        public async Task<IActionResult> DeleteLocation(long Id)
        {
            string result = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("deleteloc", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?Id=" + Id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    result = await response.Content.ReadAsStringAsync();
                else
                {
                    return BadRequest("Delete Failed");
                }
            }
            catch (Exception)
            {
            }
            return Ok(result);
        } 
        [HttpGet]
        public async Task<IActionResult> AssetManager()
        {
            AssetManagerModel assetManagerModel = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("assetmanager", out var path);
                var res = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path , this, _token);
                if (res.IsSuccessStatusCode)
                {
                    assetManagerModel = JsonConvert.DeserializeObject<AssetManagerModel>(await res.Content.ReadAsStringAsync());
                }
            }
            catch (Exception) { }
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/Property/Mobile/AssetManager.cshtml", assetManagerModel);
           
            return View(assetManagerModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssetManager(AssetManagerModel assetManagerModel)
        {
            bool result = false;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("createasset", out var path);
                var res = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path ,assetManagerModel, this, _token);
                if (res.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<bool>(await res.Content.ReadAsStringAsync());
                }
            }
            catch (Exception) { }
            return RedirectToAction("AssetManager");
        }
        [HttpGet]
        public async Task<IActionResult> GetAsset(int locId)
        {
            List<SelectItem> result= new List<SelectItem>();
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getasset", out var path);
                var res = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path+"?location="+locId, this, _token);
                if (res.IsSuccessStatusCode)
                {
                    result = JsonConvert.DeserializeObject<List<SelectItem>>(await res.Content.ReadAsStringAsync());
                }
            }
            catch (Exception) { }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsset(long Id)
        {
            string result = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("deleteasset", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?Id=" + Id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    result = await response.Content.ReadAsStringAsync();
                else
                {
                    return BadRequest("Delete Failed");
                }
            }
            catch (Exception)
            {
            }
            return Ok(result);

        }
        [HttpGet]
        public async Task<IActionResult> Assets()
        {
            List<Assets> result = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("assets", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    result  = JsonConvert.DeserializeObject<List<Assets>>(await response.Content.ReadAsStringAsync());
               
               
            }
            catch (Exception)
            {
            }
            return View(result);

        }



    }
}