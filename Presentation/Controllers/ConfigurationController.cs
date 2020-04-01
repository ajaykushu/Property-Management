using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Presentation.Utility.Interface;
using Presentation.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly string _token;

        public ConfigurationController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientHelper = httpClientHelper;
            _apiRoute = apiRoute;
            if (httpContextAccessor.HttpContext.Session.TryGetValue("token", out var token))
            {
                _token = Encoding.UTF8.GetString(token);
            }
        }

        /// <s
        public async Task<IActionResult> Index()
        {
            List<SelectItem> roles = null;
            _apiRoute.Value.Routes.TryGetValue("getrole", out var path);
            var res = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token);
            if (res.IsSuccessStatusCode)
                roles = JsonConvert.DeserializeObject<List<SelectItem>>(await res.Content.ReadAsStringAsync());

            return View(roles);
        }

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
        // [ValidateAntiForgeryToken]
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