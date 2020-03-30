using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Presentation.Utility.Interface;
using Presentation.ViewModels;

namespace Presentation.Controllers
{
    public class PropertyController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly string _token;
        public PropertyController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientHelper = httpClientHelper;
            _apiRoute = apiRoute;
            if (httpContextAccessor.HttpContext.Session.TryGetValue("token", out var token))
            {
                _token = Encoding.UTF8.GetString(token);
            }

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
            return PartialView("_AddPropertyView", addProperty);
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
            return View(prop);
        }
        [HttpGet]
        public async Task<IActionResult> MarkPrimary(long id,long userId)
        {
            _apiRoute.Value.Routes.TryGetValue("markprimary", out string path);
            try
            {
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id+"&userId="+userId, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    TempData["Success"] = "Marked as Primary Propery";

            }
            catch (Exception)
            {
                TempData["Error"] = StringConstants.Error;
            }

            return RedirectToAction("UserDetailView", "User", new { id=userId });
        }


    }
}