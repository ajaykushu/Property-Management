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
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{

    public class WorkOrderController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly string _token;

        public WorkOrderController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientHelper = httpClientHelper;
            _apiRoute = apiRoute;
            if (httpContextAccessor.HttpContext.Session.TryGetValue("token", out var token))
            {
                _token = Encoding.UTF8.GetString(token);
            }

        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult WorkOrderOverview()
        {
            return PartialView("_WorkOrderOverview");
        }
        [HttpGet]
        // [Authorize]
        public async Task<IActionResult> CreateWorkOrderAsync()
        {
            CreateWorkOrder createWorkOrder = new CreateWorkOrder();
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getworkordermodel", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    createWorkOrder = JsonConvert.DeserializeObject<CreateWorkOrder>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
            }
            return PartialView("_CreateWorkOrder", createWorkOrder);
        }
        [HttpGet]
        public async Task<IActionResult> GetAreaLocation(long id)
        {
            PropDetail result=null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getarealocation", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path+"?id="+id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    result = JsonConvert.DeserializeObject<PropDetail>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
               
            }
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSection(long id)
        {
            List<SelectItem> result = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getsection", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    result = JsonConvert.DeserializeObject<List<SelectItem>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {

            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWO(CreateWorkOrder workOrder)
        {

            if (ModelState.IsValid)
            {
                WorkOrderDetail result = null;
                try
                {
                    _apiRoute.Value.Routes.TryGetValue("createwo", out string path);
                    var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path,workOrder,this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        result = JsonConvert.DeserializeObject<WorkOrderDetail>(await response.Content.ReadAsStringAsync());
                        return View("WorkOrderDetail", result);
                    }
                }
                catch (Exception)
                {

                }
                return Ok(result);
            }
            else
            {
              var  msg = string.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).Select(y => y.Value.Errors.FirstOrDefault().ErrorMessage));
                return BadRequest(msg);
            }
            
        }
    }
}
