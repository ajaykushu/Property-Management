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
        public async Task<IActionResult> Index(string matchString, int requestedPage, string endDate, FilterEnumWO filter = FilterEnumWO.ByAssigned, FilterEnumWOStage stage = FilterEnumWOStage.INITWO)
        {
            ViewBag.searchString = matchString ?? "";
            ViewBag.filter = filter;
            ViewBag.stage = stage;
            ViewBag.enddate = endDate;
            Pagination<List<WorkOrderAssigned>> WorkOrderAssigned = null;
            try
            {
                StringBuilder routeBuilder = new StringBuilder();
                _apiRoute.Value.Routes.TryGetValue("getallworkorder", out string path);
                string parameters = string.Concat("?matchString=" , matchString , "&filter=" , filter , "&requestedPage=" , requestedPage , "&stage=" , stage , "&endDate=" , endDate);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + parameters, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    WorkOrderAssigned = JsonConvert.DeserializeObject<Pagination<List<WorkOrderAssigned>>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {
            }
            return View(WorkOrderAssigned);
        }

        [HttpGet]
        public async Task<IActionResult> CreateWorkOrder()
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
            return View("CreateWorkOrder", createWorkOrder);
        }

        [HttpGet]
        public async Task<IActionResult> GetArea(long id)
        {
            List<SelectItem> result = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getarea", out string path);
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
        public async Task<IActionResult> GetLocation(long id)
        {
            List<SelectItem> result = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getlocation", out string path);
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

        [HttpGet]
        public async Task<ActionResult<WorkOrderDetail>> GetWODetail(long id)
        {
            WorkOrderDetail workOrderDetail = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("wodetail", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    workOrderDetail = JsonConvert.DeserializeObject<WorkOrderDetail>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {
            }
            return View("WorkOrderDetail", workOrderDetail);
        }

        [HttpGet]
        public async Task<IActionResult> EditWOView(long id)
        {
            EditWorkOrder editWorkOrder = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("editWOModel", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    editWorkOrder = JsonConvert.DeserializeObject<EditWorkOrder>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {
            }
            return View(editWorkOrder);
        }

        [HttpPost]
        public async Task<IActionResult> EditWO(EditWorkOrder editWorkOrder)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _apiRoute.Value.Routes.TryGetValue("editWO", out string path);
                    var response = await _httpClientHelper.PostFileDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, editWorkOrder, this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        bool status = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                        if (status)
                        {
                            TempData["Success"] = "Successfully Updated";
                            return RedirectToAction("GetWODetail", new { id = editWorkOrder.Id });
                        }
                        else
                        {
                            TempData["Error"] = "Unable to Update";
                        }
                    }
                }
                catch (Exception)
                {
                    TempData["Error"] = StringConstants.Error;
                }
            }
            else
            {
                var msg = string.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).Select(y => y.Value.Errors.FirstOrDefault().ErrorMessage));
                TempData["Error"] = msg;
            }
            return RedirectToAction("EditWOView", new { id = editWorkOrder.Id });
        }

        [HttpPost]
        public async Task<IActionResult> CreateWO(CreateWorkOrder workOrder)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _apiRoute.Value.Routes.TryGetValue("createwo", out string path);
                    var response = await _httpClientHelper.PostFileDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, workOrder, this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                        if (result)
                        {
                            return Ok(StringConstants.SuccessSaved);
                        }
                        else
                            return BadRequest("Unable To Create");
                    }
                }
                catch (Exception)
                {
                }
                return StatusCode(500, StringConstants.Error);
            }
            else
            {
                var msg = string.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).Select(y => y.Value.Errors.FirstOrDefault().ErrorMessage));
                return BadRequest(msg);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetComment(long Id, int requestedPage)
        {
            ViewBag.workorderId = Id;
            Pagination<List<Comment>> comments = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getcomment", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?workorderId=" + Id + "&pageNumber=" + requestedPage, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    comments = JsonConvert.DeserializeObject<Pagination<List<Comment>>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {
            }
            return View("CommentOperation", comments);
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(Post post)
        {
            try
            {
                _apiRoute.Value.Routes.TryGetValue("postcomment", out string path);
                var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, post, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var status = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                    if (status)
                       // TempData["Success"] = "Posted Successfully";
                }
            }
            catch (Exception)
            {
            }
            return RedirectToAction("GetComment", new { id = post.WorkOrderId });
        }

        [HttpGet]
        public async Task<IActionResult> WorkOrderOperation(long workorderId, ProcessEnumWOStage process)
        {
           
            try
            {
                var urlpayload = string.Concat("?workOrderId=" , workorderId , "&process=" , process);
                _apiRoute.Value.Routes.TryGetValue("workorderoperation", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + urlpayload, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                   var status = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                    if (status)
                        TempData["Success"] = process + " Completed Sucessfully";
                }
            }
            catch (Exception)
            {
            }
            return RedirectToAction("GetWODetail", new { id = workorderId });
        }

        [HttpPost]
        public async Task<IActionResult> AssignToUser(long userId, long workOrderId)
        {
            
            if (userId == 0 || workOrderId == 0)
            {
                TempData["Error"] = StringConstants.Error;
            }
            else
            {
                try
                {
                    var urlpayload = string.Concat("?userId=" , userId , "&workOrderId=" , workOrderId);
                    _apiRoute.Value.Routes.TryGetValue("assigntouser", out string path);
                    var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + urlpayload, new { }, this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                       var status = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                        if (status)
                            TempData["Success"] = " Assigned Completed Sucessfully";
                    }
                }
                catch (Exception)
                {
                }
            }
            return RedirectToAction("GetWODetail", new { id = workOrderId });
        }
    }
}