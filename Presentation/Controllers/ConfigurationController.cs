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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection;

namespace Presentation.Controllers
{
    [ReferrerAttribute]
    public class ConfigurationController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly string _token;
        private readonly IDetection _detection;

        public ConfigurationController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IHttpContextAccessor httpContextAccessor, IDetection detection, IDistributedCache _session)
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

        [Authorize]
        public async Task<IActionResult> Index()
        {
            List<SelectItem> roles = null;
            _apiRoute.Value.Routes.TryGetValue("getrole", out var path);
            var res = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token);
            if (res.IsSuccessStatusCode)
                roles = JsonConvert.DeserializeObject<List<SelectItem>>(await res.Content.ReadAsStringAsync());

            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/Configuration/Mobile/Index.cshtml", roles);
            return View(roles);
        }

        [Authorize]
        public async Task<IActionResult> FeaturesSelector(long id)
        {
            FeatureView features = new FeatureView();
            _apiRoute.Value.Routes.TryGetValue("getfeatures", out var path);
            var res = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id, this, _token);
            if (res.IsSuccessStatusCode)
                features.Features = JsonConvert.DeserializeObject<List<SelectItem>>(await res.Content.ReadAsStringAsync());
            features.Roleid = id;
            return PartialView(features);
        }

        public IActionResult RoleControl()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateFeature()
        {
            int val = 0;
            var x = HttpContext.Request.Form.Where(x => int.TryParse(x.Key, out val)).Select(x => x.Value).FirstOrDefault().ToList();
            KeyValuePair<int, List<string>> keyValuePair = new KeyValuePair<int, List<string>>(val, x);
            _apiRoute.Value.Routes.TryGetValue("updatefeature", out var path);
            var res = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, keyValuePair, this, _token);
            if (res.IsSuccessStatusCode)
                return Ok("Successfully Updated");
            else
            {
                var result = await FeaturesSelector(keyValuePair.Key) as PartialViewResult;
                HttpContext.Response.StatusCode = (int)res.StatusCode;
                if (res.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return Content("<script language='javascript' type='text/javascript'>location.reload(true);</script>");
                }

                return PartialView("FeaturesSelector", result.Model);
            }
        }
       
    }
   
}