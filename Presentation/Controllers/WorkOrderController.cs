using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Presentation.ConstModal;
using Presentation.Utiliity.Interface;
using Presentation.Utility;
using Presentation.Utility.Interface;
using Presentation.ViewModels;
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
    public class WorkOrderController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;
        private readonly string _token;
        private readonly IExport<WorkOrderDetail> _export;
        private readonly IExport<AllWOExport> _allwoexport;
        private readonly IExport<AllWOExportRecurring> _allrecurringwoexport;
        private readonly IDetection _detection;

        public WorkOrderController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute, IHttpContextAccessor httpContextAccessor, IDistributedCache _session, IExport<WorkOrderDetail> export, IExport<AllWOExport> allwoexport, IDetection detection, IExport<AllWOExportRecurring> allrecurringwoexport)
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
            _export = export;
            _allwoexport = allwoexport;
            _detection = detection;
            _allrecurringwoexport = allrecurringwoexport;
        }

        [HttpGet]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> CreateWorkOrderView()
        {

            CreateWorkOrder createWorkOrder = new CreateWorkOrder();

            _apiRoute.Value.Routes.TryGetValue("getworkordermodel", out string path);
            var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                createWorkOrder = JsonConvert.DeserializeObject<CreateWorkOrder>(await response.Content.ReadAsStringAsync());


            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/WorkOrder/Mobile/CreateWorkOrder.cshtml", createWorkOrder);
           else
            return View("CreateWorkOrder", createWorkOrder);
           
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateWorkOrderRecurringView()
        {

            CreateWorkOrderRecurring createWorkOrder = new CreateWorkOrderRecurring();

            _apiRoute.Value.Routes.TryGetValue("getworkordermodel", out string path);
            var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, this, _token).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
                createWorkOrder = JsonConvert.DeserializeObject<CreateWorkOrderRecurring>(await response.Content.ReadAsStringAsync());


            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/WorkOrder/Mobile/CreateWorkOrderRecurring.cshtml", createWorkOrder);
           
            else
                return View("CreateWorkOrderRecurring", createWorkOrder);
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
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult<WorkOrderDetail>> GetWODetail(string id,int type=1)
        {
            WorkOrderDetail workOrderDetail = null;

            try
            {
                string path;
                if (type == 2)
                {
                    _apiRoute.Value.Routes.TryGetValue("getrecwodetail", out path);
                }
                else {
                    _apiRoute.Value.Routes.TryGetValue("wodetail", out path);
                }
            
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
        [Authorize]
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditRecurringWOView(string id)
        {
            EditRecurringWorkOrder editWorkOrder = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("editRecurringwomodel", out string path);
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?id=" + id, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    editWorkOrder = JsonConvert.DeserializeObject<EditRecurringWorkOrder>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception)
            {
            }
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/WorkOrder/Mobile/EditWOViewRecurring.cshtml", editWorkOrder);
            return View(editWorkOrder);
        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
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
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateWORecurring(CreateWorkOrderRecurring workOrder)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _apiRoute.Value.Routes.TryGetValue("createrecurringwo", out string path);
                    var response = await _httpClientHelper.PostFileDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, workOrder, this, _token).ConfigureAwait(false);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
                        if (result)
                        {
                            var href= Url.Action("GetRecurringWO");
                            return StatusCode((int)HttpStatusCode.Redirect,href+"@"+StringConstants.CreatedSuccess);
                           
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
        [Authorize]
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
        [Authorize]
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
            if (!post.WorkOrderId.Contains("R"))
            {
                return Redirect("~/WorkOrder/GetWODetail?id=" + post.WorkOrderId + "#CommentSection");
            }
            else
            {
                return Redirect("~/WorkOrder/GetWODetail?id=" + post.WorkOrderId + "&type=2#CommentSection");
            }
        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> DownloadWO(string woId,int type)
        {
            WorkOrderDetail workOrderDetail = null;
            string filename = String.Empty;
            string contentType = String.Empty;
            if (type == 1)
            {
                filename = "File.csv";
                 contentType = "text/csv";
            }
            else
            {
                filename = "File.xlsx";
                contentType = "text/xlsx";
            }
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
                if(type==1)
                file = await _export.CreateCSV(workOrderDetail);
                else
                    file = await _export.CreateExcel(workOrderDetail);
            }
            catch (Exception)
            {
            }
            return File(file, contentType);
        }

        [Authorize]
        public async Task<IActionResult> ExportWO(WOFilterModel wo,int type)
        {
            List<AllWOExport> workOrderDetail = null;
            string filename = String.Empty;
            string contentType = String.Empty;
            if (type == 1)
            {
                filename = "File.csv";
                contentType = "text/csv";
            }
            else
            {
                filename = "File.xlsx";
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
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
                if (type == 1)
                    file = await _allwoexport.CreateListCSV(workOrderDetail);
                else
                    file = await _allwoexport.CreateListExcel(workOrderDetail);
                
            }
            catch (Exception)
            {
            }
            return File(file, contentType);
        }
        [Authorize]
        public async Task<IActionResult> ExportRecurringWO(WOFilterModel wo,int type)
        {
            List<AllWOExportRecurring> workOrderDetail = null;
            string filename = String.Empty;
            string contentType = String.Empty;
            if (type == 1)
            {
                filename = "File.csv";
                contentType = "text/csv";
            }
            else
            {
                filename = "File.xlsx";
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            byte[] file = null;
            try
            {
                _apiRoute.Value.Routes.TryGetValue("workordersrecurringexport", out string path);
                var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, wo, this, _token).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    workOrderDetail = JsonConvert.DeserializeObject<List<AllWOExportRecurring>>(await response.Content.ReadAsStringAsync());
                }

                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = true,
                };

                HttpContext.Response.Headers.Add("Content-Disposition", cd.ToString());
                if (type == 1)
                    file = await _allrecurringwoexport.CreateListCSV(workOrderDetail);
                else
                    file = await _allrecurringwoexport.CreateListExcel(workOrderDetail);
                //file = await _allrecurringwoexport.CreateListCSV(workOrderDetail);
            }
            catch (Exception)
            {
            }
            return File(file, contentType);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetHistory(string entity,string rowId)
        {
            List<HistoryDetail> historyDetails = null;
            ViewBag.Id = rowId;
            _apiRoute.Value.Routes.TryGetValue("gethistory", out string path);
            var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path+"?entity="+entity+"&rowId="+rowId , this, _token).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                historyDetails = JsonConvert.DeserializeObject<List<HistoryDetail>>(await response.Content.ReadAsStringAsync());
            }
            if (_detection.Device.Type == DeviceType.Mobile) {
                return View("~/Views/WorkOrder/Mobile/GetHistory.cshtml", historyDetails);
            }
            return PartialView(historyDetails);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRecurringWO(WOFilterModel wOFilterModel)
        {
            Pagination<List<RecurringWOs>> recWo = null;
            _apiRoute.Value.Routes.TryGetValue("getRecurringWO", out string path);
            var response = await _httpClientHelper.PostDataAsync(_apiRoute.Value.ApplicationBaseUrl + path, wOFilterModel, this, _token).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                recWo = JsonConvert.DeserializeObject<Pagination<List<RecurringWOs>>>(await response.Content.ReadAsStringAsync());
                ViewBag.Response = recWo;
            }
           
           if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/WorkOrder/Mobile/GetRecurringWO.cshtml", wOFilterModel);
            return View(wOFilterModel);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetChildWO(string rwoId,string search, int pageNumber = 0)
        {
            Pagination<List<ChildWo>> recWo = null;
            _apiRoute.Value.Routes.TryGetValue("getchildwos", out string path);
            var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path + "?pageNumber=" + pageNumber+"&search="+search+"&rwoId="+rwoId, this, _token).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                recWo = JsonConvert.DeserializeObject<Pagination<List<ChildWo>>>(await response.Content.ReadAsStringAsync());
            }
            if (_detection.Device.Type == DeviceType.Mobile)
                return View("~/Views/WorkOrder/Mobile/GetChildWO.cshtml", recWo);

            return PartialView(recWo);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditRecurringWO(EditRecurringWorkOrder editWorkOrder)
        {
            string msg = String.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    _apiRoute.Value.Routes.TryGetValue("editrecurringwo", out string path);
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


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCompletedWO(int pageNumber)
        {  
            try
            {
                _apiRoute.Value.Routes.TryGetValue("completedwo", out string path);
                StringBuilder query = new StringBuilder();
                var response = await _httpClientHelper.GetDataAsync(_apiRoute.Value.ApplicationBaseUrl + path+"?pageNumber="+pageNumber, this, _token).ConfigureAwait(false);
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
                return View("~/Views/WorkOrder/Mobile/GetCompletedWO.cshtml");
            return View();
        }

    }
}