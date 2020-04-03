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
        //[FeatureBasedAuthorization("Edit User")]
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
        public async Task<ActionResult<CreateWO>> GetAreaLocation(long id)
        {
            PropDetail prop = await _workOrderService.GetAreaLocation(id);
            return Ok(prop);
        }

        [HttpGet]
        [Route("getsection")]
        public async Task<ActionResult<List<SelectItem>>> GetSection(long id)
        {
            List<SelectItem> prop = await _workOrderService.GetSection(id);
            return Ok(prop);
        }

        [HttpPost]
        [Route("createwo")]
        public async Task<ActionResult> CreateWO(CreateWO createWO)
        {
            WorkOrderDetail workOrderDetail = null;
            workOrderDetail = await _workOrderService.CreateWO(createWO);
            return Ok(workOrderDetail);
        }

        [HttpGet]
        [Route("getallworkorder")]
        public async Task<ActionResult<List<WorkOrderAssigned>>> GetWO(string matchString, FilterEnumWO filter, int requestedPage)
        {
            Pagination<List<WorkOrderAssigned>> workorderassigned = null;
            workorderassigned = await _workOrderService.GetWO(requestedPage, filter, matchString);
            return Ok(workorderassigned);
        }
    }
}