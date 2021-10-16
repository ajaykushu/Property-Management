using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.WorkOrder.RequestModels;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities;
using DataTransferObjects.ResponseModels;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    
    public class WorkOrderController : ControllerBase
    {
        private readonly IWorkOrderBL _workOrderService;
        private readonly IRecurringBL _recurringBL;

        public WorkOrderController(IWorkOrderBL workOrderService, IRecurringBL recurringBL)
        {
            _workOrderService = workOrderService;
            _recurringBL = recurringBL;
        }

        [HttpGet]
        [Route("getworkordermodel")]
        [FeatureBasedAuthorization(MenuEnum.Create_WO)]
        public async Task<ActionResult<CreateNormalWO>> GetCreateWOModel()
        {
            CreateNormalWO wo = null;
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
            if (userId != null)
                wo = await _workOrderService.GetCreateWOModel(Convert.ToInt64(userId));
            return Ok(wo);
        }

        [HttpGet]
        [Route("getlocation")]
        [FeatureBasedAuthorization(MenuEnum.Create_WO)]
        public async Task<ActionResult<SelectItem>> GetLocation(long id)
        {
            List<SelectItem> prop = await _workOrderService.GetLocation(id);
            return Ok(prop);
        }

        [HttpGet]
        [Route("getdatabycategory")]
        [FeatureBasedAuthorization(MenuEnum.Create_WO)]
        public async Task<ActionResult<Dictionary<string, List<SelectItem>>>> GetDataByCategory(string category)
        {
            Dictionary<string, List<SelectItem>> prop = await _workOrderService.GetDataByCategory(category);
            return Ok(prop);
        }

        [HttpPost]
        [Route("createwo")]
        [FeatureBasedAuthorization(MenuEnum.Create_WO)]
        public async Task<ActionResult> CreateWO([FromForm] CreateNormalWO createWO, List<IFormFile> File)
         {
            var status = await _workOrderService.CreateWO(createWO, File);
            return Ok(status);
        }

        [HttpPost]
        [Route("createrecurringwo")]
        [FeatureBasedAuthorization(MenuEnum.Create_WO)]
        public async Task<ActionResult> CreateRecurringWO([FromForm] CreateRecurringWODTO createWO, List<IFormFile> File)
        {
            var status = await _workOrderService.CreateRecurringWO(createWO, File);
            return Ok(status);
        }

        [HttpGet]
        [Route("wodetail")]
        [FeatureBasedAuthorization(MenuEnum.GetWO_Detail)]
        public async Task<ActionResult<WorkOrderDetail>> GetWODetail(string id)
        {
            var workOrderDetail = await _workOrderService.GetWODetail(id);
            return Ok(workOrderDetail);
        }

        [HttpPost]
        [Route("getallworkorder")]
        [FeatureBasedAuthorization(MenuEnum.Get_WO)]
        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<ActionResult<List<WorkOrderAssigned>>> GetWO(WOFilterDTO wOFilterModel)
        {
            var workorderassigned = await _workOrderService.GetWO(wOFilterModel);
            return Ok(workorderassigned);
        }

        [HttpGet]
        [Route("editWOModel")]
        [FeatureBasedAuthorization(MenuEnum.Edit_WO)]
        public async Task<ActionResult> GetEditWO(string id)
        {
            var editWorkOrder = await _workOrderService.GetEditWO(id);
            return Ok(editWorkOrder);
        }
        [HttpGet]
        [Route("editRecurringwomodel")]
        [FeatureBasedAuthorization(MenuEnum.Edit_WO)]
        public async Task<ActionResult> GetEditRecurringWO(string id)
        {
            EditRecurringWorkOrderDTO editWorkOrder = await _workOrderService.GetEditRecurringWO(id);
            return Ok(editWorkOrder);
        }

        [HttpPost]
        [Route("editWO")]
        [FeatureBasedAuthorization(MenuEnum.Edit_WO)]
        public async Task<ActionResult<bool>> EditWO([FromForm] EditNormalWorkOrder editWorkOrder, List<IFormFile> File)
        {
            bool status = await _workOrderService.EditWO(editWorkOrder, File);
            return Ok(status);
        }
        
            [HttpPost]
        [Route("editrecurringwo")]
        [FeatureBasedAuthorization(MenuEnum.Edit_WO)]
        public async Task<ActionResult<bool>> EditRecurringWO([FromForm] EditRecurringWorkOrderDTO editWorkOrder, List<IFormFile> File)
        {
            bool status = await _workOrderService.EditRecurringWO(editWorkOrder, File);
            return Ok(status);
        }

        [HttpGet]
        [Route("getcomment")]
        public async Task<ActionResult<List<CommentDTO>>> GetComment(string workorderId, int pageNumber)
        {
            var res = await _workOrderService.GetComment(workorderId, pageNumber);
            return Ok(res);
        }

        [HttpPost]
        [Route("postcomment")]
        [FeatureBasedAuthorization(MenuEnum.Post_Comment)]
        public async Task<ActionResult<Pagination<List<CommentDTO>>>> PostComment(PostCommentDTO post)
        {
            var res = await _workOrderService.PostComment(post);
            return Ok(res);
        }

        [HttpPost]
        [Route("workorderstatuschange")]
        [FeatureBasedAuthorization(MenuEnum.WO_Operation)]
        public async Task<ActionResult<bool>> WorkOrderStatusChange([FromForm] WorkOrderDetail workOrderDetail,IList<IFormFile> File)
        {
            bool res = false;
            if (!workOrderDetail.Id.Contains("RWO",comparisonType:StringComparison.InvariantCultureIgnoreCase)) {
                 res = await _workOrderService.WorkOrderStatusChange(workOrderDetail, File);
            }
            else
                 res = await _recurringBL.WorkOrderStatusChange(workOrderDetail, File);
            return Ok(res);
        }
        

        [HttpPost]
        [Route("workordersexport")]
        [FeatureBasedAuthorization(MenuEnum.WO_Operation)]
        public async Task<ActionResult<List<WorkOrderDetail>>> WOExport(WOFilterDTO wOFilterModel)
        {
            var res = await _workOrderService.WOExport(wOFilterModel);
            return Ok(res);
        }
        [HttpPost]
        [Route("workordersrecurringexport")]
        [FeatureBasedAuthorization(MenuEnum.WO_Operation)]
        public async Task<ActionResult<List<WorkOrderDetail>>> WOExportRecurring(WOFilterDTO wOFilterModel)
        {
            List <AllWOExportRecurring> res = await _workOrderService.WOExportRecurring(wOFilterModel);
            return Ok(res);
        }

        [HttpPost]
        [ResponseCache(NoStore = true, Duration = 0)]
        [Route("getRecurringWO")]
        [FeatureBasedAuthorization(MenuEnum.GetWO_Detail)]
        public async Task<ActionResult<Pagination<List<RecurringWOs>>>> GetRecurringWO(WOFilterDTO wOFilterDTO)
        {
            Pagination<List<RecurringWOs>> res = await _workOrderService.GetRecurringWO(wOFilterDTO);
            return Ok(res);
        }
        [HttpGet]
        [Route("gethistory")]
        [FeatureBasedAuthorization(MenuEnum.Recurring_WO)]
        public async Task<ActionResult<List<HistoryDetail>>> GetHistory(string entity,string rowId)
        {
            List<HistoryDetail> res = await _workOrderService.GetHistory(entity,rowId);
            return Ok(res);
        }
        [HttpGet]
        [Route("getchildwos")]
        [ResponseCache(NoStore = true, Duration = 0)]
        [FeatureBasedAuthorization(MenuEnum.Recurring_WO)]
        public async Task<ActionResult<Pagination<List<ChildWo>>>> GetChildWO( string rwoId, string search,int pageNumber)
        {
            Pagination<List<ChildWo>> res = await _workOrderService.GetChildWO(pageNumber,search, rwoId);
            return Ok(res);
        }

        [HttpGet]
        [Route("getrecwodetail")]
        [FeatureBasedAuthorization(MenuEnum.Recurring_WO)]
        public async Task<ActionResult<WorkOrderDetail>> GetRecurringWODetail(string id)
        {
            WorkOrderDetail res = await _workOrderService.GetRecurringWODetail(id);
            return Ok(res);
        }

       
        [HttpPost]
        [Route("addeffort")]
        public async Task<ActionResult<bool>> AddEffort(EffortPagination effortDTOs, string Id)
        {
            bool res = await _workOrderService.AddEffort(effortDTOs,Id);
            return Ok(res);
        }
        [HttpGet]
        [ResponseCache(NoStore = true, Duration = 0)]
        [Route("geteffort")]
        public async Task<ActionResult<EffortPagination>> GetEffort(string id, bool prev)
        {
            EffortPagination res = await _workOrderService.GetEffort(id, prev);
            return Ok(res);
        }

        [HttpGet]
        [Route("getissue")]
        public async Task<ActionResult<List<SelectItem>>> GetIssues(long id)
        {
            List<SelectItem> res = await _workOrderService.GetIssues(id);
            return Ok(res);
        }
        [HttpGet]
        [Route("getitem")]
        public async Task<ActionResult<List<SelectItem>>> GetItem(long id)
        {
            List<SelectItem> res = await _workOrderService.GetItem(id);
            return Ok(res);
        }
        [HttpGet]
        [Route("wolist")]
        public async Task<ActionResult<List<SelectItem>>> GetWoList(long id)
        {
            List<string> res = await _workOrderService.GetWoList(id);
            return Ok(res);
        }

    }
}