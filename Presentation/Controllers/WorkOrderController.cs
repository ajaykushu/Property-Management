using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Presentation.ConstModal;
using Presentation.Utiliity.Interface;
using Presentation.Utility.Interface;
using Presentation.ViewModels;
using Presentation.ViewModels.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection;

namespace Presentation.Controllers
{
    public class WorkOrderController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly string _token;
        private readonly IExport<WorkOrderDetail> _export;
        private readonly IExport<AllWOExport> _allwoexport;
        private readonly IDetection _detection;

        public WorkOrderController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IHttpContextAccessor httpContextAccessor, IExport<WorkOrderDetail> export, IExport<AllWOExport> allwoexport, IDetection detection)
        {
            _httpClientHelper = httpClientHelper;
            _apiRoute = apiRoute;
            if (httpContextAccessor.HttpContext.Session.TryGetValue("token", out var token))
            {
                _token = Encoding.UTF8.GetString(token);
            }
            _export = export;
            _allwoexport = allwoexport;
            _detection = detection;
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
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/WorkOrder/Mobile/Index.cshtml", wOFilterModel);
            return View(wOFilterModel);
        }

        [HttpGet]
        public async Task<IActionResult> CreateWorkOrder()
        {
            CreateWorkOrder createWorkOrder = new CreateWorkOrder();

            _apiRoute.Value.Routes.TryGetValue("getworkordermodel", out string path);
            var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                createWorkOrder = JsonConvert.DeserializeObject<CreateWorkOrder>(await response.Content.ReadAsStringAsync());


            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/WorkOrder/Mobile/CreateWorkOrder.cshtml", createWorkOrder);
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
        public async Task<IActionResult> GetDataByCategory(string category)
        {
            Dictionary<string, List<SelectItem>> result = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("getdatabycategory", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?category=" + category, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    result = JsonConvert.DeserializeObject<Dictionary<string, List<SelectItem>>>(await response.Content.ReadAsStringAsync());
            }
            catch (Exception)
            {
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<WorkOrderDetail>> GetWODetail(string id)
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
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/WorkOrder/Mobile/WorkOrderDetail.cshtml", workOrderDetail);
            return View("WorkOrderDetail", workOrderDetail);
        }

        [HttpGet]
        public async Task<IActionResult> EditWOView(string id)
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
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/WorkOrder/Mobile/EditWOView.cshtml", editWorkOrder);
            return View(editWorkOrder);
        }

        [HttpPost]
        public async Task<IActionResult> EditWO(EditWorkOrder editWorkOrder)
        {
            string msg = String.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    _apiRoute.Value.Routes.TryGetValue("editWO", out string path);
                    var response = await _httpClientHelper.PostFileDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, editWorkOrder, this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        if (await response.Content.ReadAsStringAsync().ConfigureAwait(false) == "true")
                            return Ok(StringConstants.SuccessUpdate);
                        else
                            return Ok(StringConstants.UpdateFailed);
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                        return BadRequest(await response.Content.ReadAsStringAsync());
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return Content("<script language='javascript' type='text/javascript'>location.reload(true);</script>");
                    else
                        return StatusCode(500, StringConstants.Error);
                }
                catch (Exception)
                {
                    msg = StringConstants.Error;
                }
            }
            else
                msg = String.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
            return BadRequest(msg);
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
                            return Ok(StringConstants.CreatedSuccess);
                        }
                        else
                            return BadRequest("Unable To Create");
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        return Content("<script language='javascript' type='text/javascript'>location.reload(true);</script>");
                }
                catch (Exception)
                {
                }
                return StatusCode(500, StringConstants.Error);
            }
            else
            {
                var msg = String.Join(", ", ModelState.Where(x => x.Value.Errors.Count > 0).SelectMany(x => x.Value.Errors.Select(y => y.ErrorMessage)).ToList());
                return BadRequest(msg);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetComment(string workOrderId, int requestedPage)
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
                }
            }
            catch (Exception)
            {
            }
            return Redirect("~/WorkOrder/GetWODetail?id=" + post.WorkOrderId + "#CommentSection");
        }

        [HttpPost]
        public async Task<IActionResult> WorkOrderStatusChange(string id, int statusId, string comment)
        {
            try
            {
                var urlpayload = string.Concat("?id=", id, "&statusId=", statusId, "&comment=", comment);
                _apiRoute.Value.Routes.TryGetValue("workorderstatuschange", out string path);
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
            return RedirectToAction("GetWODetail", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> DownloadWO(string woId)
        {
            WorkOrderDetail workOrderDetail = null;
            string filename = "File.csv";
            string contentType = "text/csv";
            byte[] file = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("wodetail", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + woId, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    workOrderDetail = JsonConvert.DeserializeObject<WorkOrderDetail>(await response.Content.ReadAsStringAsync());
                }

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };

                HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
                file = await _export.CreateCSV(workOrderDetail);
            }
            catch (Exception)
            {
            }
            return File(file, contentType);
        }

        public async Task<IActionResult> ExportWO(WOFilterModel wo)
        {
            List<AllWOExport> workOrderDetail = null;
            string filename = "File.csv";
            string contentType = "text/csv";
            byte[] file = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("workordersexport", out string path);
                var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, wo, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    workOrderDetail = JsonConvert.DeserializeObject<List<AllWOExport>>(await response.Content.ReadAsStringAsync());
                }

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };

                HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
                file = await _allwoexport.CreateListCSV(workOrderDetail);
            }
            catch (Exception)
            {
            }
            return File(file, contentType);
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory(string entity,string id=null)
        {
            List<HistoryDetail> historyDetails = null;
            ViewBag.Id = id;
            _apiRoute.Value.Routes.TryGetValue("gethistory", out string path);
            var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path+"?entity="+entity , this, _token).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                historyDetails = JsonConvert.DeserializeObject<List<HistoryDetail>>(await response.Content.ReadAsStringAsync());
            }
            if (_detection.Device.Type == DeviceType.Mobile) {
                return View("~/Views/WorkOrder/Mobile/GetHistory.cshtml", historyDetails);
            }
            return PartialView(historyDetails);
        }
    }
}