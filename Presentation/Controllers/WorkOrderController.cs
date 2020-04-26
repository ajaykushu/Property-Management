using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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
        public async Task<IActionResult> Index(WOFilterModel wOFilterModel)
        {
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getallworkorder", out string path);
                StringBuilder query = new StringBuilder();
                var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, wOFilterModel, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var WorkOrderAssigned = JsonConvert.DeserializeObject<Pagination<List<WorkOrderAssigned>>>(await response.Content.ReadAsStringAsync());
                    ViewBag.Response = WorkOrderAssigned;
                }
            }
            catch (Exception)
            {
            }
            return View(wOFilterModel);
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
        public async Task<IActionResult> GetUserByDept(long id)
        {
            List<SelectItem> result = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getusersbydepartment", out string path);
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
        public async Task<IActionResult> GetComment(long workOrderId, int requestedPage)
        {
            ViewBag.workorderId = workOrderId;
            List<Comment> comments = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getcomment", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?workorderId=" + workOrderId + "&pageNumber=" + requestedPage, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    comments = JsonConvert.DeserializeObject<List<Comment>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {
            }
            return PartialView("CommentPartial", comments);
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

                    // TempData["Success"] = "Posted Successfully";
                }
            }
            catch (Exception)
            {
            }
            return RedirectToAction("GetWODetail", new { id = post.WorkOrderId });
        }

        [HttpPost]
        public async Task<IActionResult> WorkOrderStageChange(long Id, int stageId)
        {
            try
            {
                var urlpayload = string.Concat("?Id=", Id, "&stageId=", stageId);
                _apiRoute.Value.Routes.TryGetValue("workorderstagechange", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + urlpayload, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var status = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                    if (status)
                        TempData["Success"] = "Status Changed Sucessfully";
                }
            }
            catch (Exception)
            {
            }
            return RedirectToAction("GetWODetail", new { id = Id });
        }
    }
}