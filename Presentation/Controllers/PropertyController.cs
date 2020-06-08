using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
    public class PropertyController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly string _token;
        private readonly IDetection _detection;

        public PropertyController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IHttpContextAccessor httpContextAccessor, IDetection detection)
        {
            _httpClientHelper = httpClientHelper;
            _apiRoute = apiRoute;
            if (httpContextAccessor.HttpContext.Session.TryGetValue("token", out var token))
            {
                _token = Encoding.UTF8.GetString(token);
            }
            _detection = detection;
        }

        [HttpGet]
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
                        return Ok(StringConstants.SuccessSaved);
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
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/Property/Mobile/ListProperties.cshtml", properties);
            return View(properties);
        }

        [HttpGet]
        public async Task<ActionResult> ActDeactProperty(int id, bool operation)
        {
            if (id == 0)
            {
                TempData["Error"] = "Unable to Activate/Deactivate property";
                return RedirectToAction("ListProperties");
            }
            try
            {
                _apiRoute.Value.Routes.TryGetValue("actdeactproperty", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id + "&operation=" + operation, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    if (operation)
                        TempData["Success"] = "Property Activate Successfull";
                    else
                        TempData["Success"] = "Property DeActivate Successfull";
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
                TempData["Error"] = string.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).Select(y => y.Value.Errors.FirstOrDefault().ErrorMessage));
            }

            return RedirectToAction("PropertyEditView", new { id = prop.Id });
        }

        [HttpGet]
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
                            TempData["Success"] = StringConstants.SuccessSaved;
                    }
                }
                catch (Exception)
                {
                    TempData["Error"] = StringConstants.Error;
                }
            }
            return RedirectToAction("PropertyConfig", new { Id = propertyConfig.PropertyId });
        }

        [HttpGet]
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
    }
}