using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkOrderController : ControllerBase
    {
        private readonly IWorkOrderService _workOrderService;

        public WorkOrderController(IWorkOrderService workOrderService)
        {
            _workOrderService = workOrderService;
        }

        [HttpGet]
        [Route("getworkordermodel")]
        [FeatureBasedAuthorization(MenuEnum.Create_WO)]
        public async Task<ActionResult<CreateWO>> GetCreateWOModel()
        {
            CreateWO wo = null;
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
        public async Task<ActionResult> CreateWO([FromForm] CreateWO createWO, List<IFormFile> File)
         {
            var status = await _workOrderService.CreateWO(createWO, File);
            return Ok(status);
        }

        [HttpPost]
        [Route("createrecurringwo")]
        [FeatureBasedAuthorization(MenuEnum.Create_WO)]
        public async Task<ActionResult> CreateRecurringWO([FromForm] CreateRecurringWO createWO, List<IFormFile> File)
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
        public async Task<ActionResult<List<WorkOrderAssigned>>> GetWO(WOFilterModel wOFilterModel)
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
            EditRecurringWorkOrder editWorkOrder = await _workOrderService.GetEditRecurringWO(id);
            return Ok(editWorkOrder);
        }

        [HttpPost]
        [Route("editWO")]
        [FeatureBasedAuthorization(MenuEnum.Edit_WO)]
        public async Task<ActionResult<bool>> EditWO([FromForm] EditWorkOrder editWorkOrder, List<IFormFile> File)
        {
            bool status = await _workOrderService.EditWO(editWorkOrder, File);
            return Ok(status);
        }
        
            [HttpPost]
        [Route("editrecurringwo")]
        [FeatureBasedAuthorization(MenuEnum.Edit_WO)]
        public async Task<ActionResult<bool>> EditRecurringWO([FromForm] EditRecurringWorkOrder editWorkOrder, List<IFormFile> File)
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
        public async Task<ActionResult<Pagination<List<CommentDTO>>>> PostComment(Post post)
        {
            var res = await _workOrderService.PostComment(post);
            return Ok(res);
        }

        [HttpGet]
        [Route("workorderstatuschange")]
        [FeatureBasedAuthorization(MenuEnum.WO_Operation)]
        public async Task<ActionResult<bool>> WorkOrderStatusChange(string id, int statusId, string comment)
        {
            bool res = await _workOrderService.WorkOrderStatusChange(id, statusId, comment);
            return Ok(res);
        }

        [HttpPost]
        [Route("workordersexport")]
        [FeatureBasedAuthorization(MenuEnum.WO_Operation)]
        public async Task<ActionResult<List<WorkOrderDetail>>> WOExport(WOFilterModel wOFilterModel)
        {
            var res = await _workOrderService.WOExport(wOFilterModel);
            return Ok(res);
        }
        [HttpGet]
        [Route("gethistory")]
        [FeatureBasedAuthorization(MenuEnum.GetWO_Detail)]
        public async Task<ActionResult<List<HistoryDetail>>> GetHistory(string entity,string rowId)
        {
            List<HistoryDetail> res = await _workOrderService.GetHistory(entity,rowId);
            return Ok(res);
        }
    }
}