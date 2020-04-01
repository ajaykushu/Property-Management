using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.RequestModels;
using Models.ResponseModels;
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
        //[FeatureBasedAuthorization("Edit User")]
        public async Task<ActionResult<CreateWO>> GetCreateWOModel()
        {
            CreateWO wo = null;
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
            if (userId != null)
               wo  = await _workOrderService.GetCreateWOModel(Convert.ToInt64(userId));
            return Ok(wo);
        }
        [HttpGet]
        [Route("getarealocation")]
        public async Task<ActionResult<CreateWO>> GetAreaLocation(long id)
        {
            PropDetail prop= await _workOrderService.GetAreaLocation(id);
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
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Sid).Value;
            if(userId!=null)
                workOrderDetail = await _workOrderService.CreateWO(createWO,Convert.ToInt64(userId));
            return Ok(workOrderDetail);
        }

    }
}