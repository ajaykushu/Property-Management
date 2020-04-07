using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.RequestModels;
using Models.ResponseModels;
using Newtonsoft.Json;
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
        [FeatureBasedAuthorization("Create WO")]
        public async Task<ActionResult<CreateWO>> GetCreateWOModel()
        {
            CreateWO wo = null;
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
            if (userId != null)
                wo = await _workOrderService.GetCreateWOModel(Convert.ToInt64(userId));
            return Ok(wo);
        }

        [HttpGet]
        [Route("getarealocation")]
        [FeatureBasedAuthorization("Create WO")]
        public async Task<ActionResult<CreateWO>> GetAreaLocation(long id)
        {
            PropDetail prop = await _workOrderService.GetAreaLocation(id);
            return Ok(prop);
        }

        [HttpGet]
        [Route("getsection")]
        [FeatureBasedAuthorization("Create WO")]
        public async Task<ActionResult<List<SelectItem>>> GetSection(long id)
        {
            List<SelectItem> prop = await _workOrderService.GetSection(id);
            return Ok(prop);
        }

        [HttpPost]
        [Route("createwo")]
        [FeatureBasedAuthorization("Create WO")]
        public async Task<ActionResult> CreateWO()
        {
            var form = HttpContext.Request.Form;
            if (form != null && form.Keys.Contains("model"))
            {
                var file = form.Files != null && form.Files.Count == 1 ? form.Files[0] : null;
                CreateWO createWO = JsonConvert.DeserializeObject<CreateWO>(form["model"]);
                var status = await _workOrderService.CreateWO(createWO,file);
                return Ok(status);
            }
            else
                return BadRequest("No Data Send");
        }

        [HttpGet]
        [Route("wodetail")]
        [FeatureBasedAuthorization("GetWO Detail")]
        public async Task<ActionResult<WorkOrderDetail>> GetWODetail(long id)
        {
            var workOrderDetail = await _workOrderService.GetWODetail(id);
            return Ok(workOrderDetail);
        }

        [HttpGet]
        [Route("getallworkorder")]
        [FeatureBasedAuthorization("Get WO")]
        public async Task<ActionResult<List<WorkOrderAssigned>>> GetWO(string matchString, int requestedPage, FilterEnumWOStage stage, string endDate, FilterEnumWO filter = FilterEnumWO.ByAssigned)
        
        {
            Pagination<List<WorkOrderAssigned>> workorderassigned = null;
            workorderassigned = await _workOrderService.GetWO(requestedPage, filter, matchString,stage,endDate);
            return Ok(workorderassigned);
        }
        [HttpGet]
        [Route("editWOModel")]
        [FeatureBasedAuthorization("Edit WO")]
        public async Task<ActionResult<EditWorkOrder>> GetEditWO(long id)
        {
            EditWorkOrder editWorkOrder = null;
            editWorkOrder = await _workOrderService.GetEditWO(id);
            return Ok(editWorkOrder);
        }
        [HttpPost]
        [Route("editWO")]
        [FeatureBasedAuthorization("Edit WO")]
        public async Task<ActionResult<bool>> EditWO(EditWorkOrder editWorkOrder)
        {
            bool status = await _workOrderService.EditWO(editWorkOrder);
            return Ok(status);
        }
        [HttpGet]
        [Route("getcomment")]
        public async Task<ActionResult<Pagination<List<CommentDTO>>>> GetComment(long workorderId, int pageNumber)
        {
           
            var res = await _workOrderService.GetPaginationComment(workorderId,pageNumber);
            return Ok(res);
        }
        [HttpPost]
        [Route("postcomment")]
        public async Task<ActionResult<Pagination<List<CommentDTO>>>> PostComment(Post post)
        {

            var res = await _workOrderService.PostComment(post);
            return Ok(res);
        }
       

    }
}