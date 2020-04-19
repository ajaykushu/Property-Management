﻿using BusinessLogic.Interfaces;
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
        [Route("getsublocation")]
        [FeatureBasedAuthorization(MenuEnum.Create_WO)]
        public async Task<ActionResult<SelectItem>> GetSubLocation(long id)
        {
            List<SelectItem> prop = await _workOrderService.GetSubLocation(id);
            return Ok(prop);
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
        [Route("getusersbydepartment")]
        [FeatureBasedAuthorization(MenuEnum.Create_WO)]
        public async Task<ActionResult<List<SelectItem>>> GetUsersByDepartment(long id)
        {
            List<SelectItem> prop = await _workOrderService.GetUsersByDepartment(id);
            return Ok(prop);
        }

        [HttpPost]
        [Route("createwo")]
        [FeatureBasedAuthorization(MenuEnum.Create_WO)]
        public async Task<ActionResult> CreateWO([FromForm] CreateWO createWO)
        {
            var status = await _workOrderService.CreateWO(createWO);
            return Ok(status);
        }

        [HttpGet]
        [Route("wodetail")]
        [FeatureBasedAuthorization(MenuEnum.GetWO_Detail)]
        public async Task<ActionResult<WorkOrderDetail>> GetWODetail(long id)
        {
            var workOrderDetail = await _workOrderService.GetWODetail(id);
            return Ok(workOrderDetail);
        }

        [HttpGet]
        [Route("getallworkorder")]
        [FeatureBasedAuthorization(MenuEnum.Get_WO)]
        public async Task<ActionResult<List<WorkOrderAssigned>>> GetWO(string matchString, int requestedPage, FilterEnumWOStage stage, string endDate, FilterEnumWO filter = FilterEnumWO.ByAssigned)
        {
           var workorderassigned = await _workOrderService.GetWO(requestedPage, filter, matchString, stage, endDate);
            return Ok(workorderassigned);
        }

        [HttpGet]
        [Route("editWOModel")]
        [FeatureBasedAuthorization(MenuEnum.Edit_WO)]
        public async Task<ActionResult<EditWorkOrder>> GetEditWO(long id)
        {
            var editWorkOrder = await _workOrderService.GetEditWO(id);
            return Ok(editWorkOrder);
        }

        [HttpPost]
        [Route("editWO")]
        [FeatureBasedAuthorization(MenuEnum.Edit_WO)]
        public async Task<ActionResult<bool>> EditWO([FromForm] EditWorkOrder editWorkOrder)
        {
            bool status = await _workOrderService.EditWO(editWorkOrder);
            return Ok(status);
        }

        [HttpGet]
        [Route("getcomment")]
        public async Task<ActionResult<Pagination<List<CommentDTO>>>> GetComment(long workorderId, int pageNumber)
        {
            var res = await _workOrderService.GetPaginationComment(workorderId, pageNumber);
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
        [Route("workorderstagechange")]
        [FeatureBasedAuthorization(MenuEnum.WO_Operation)]
        public async Task<ActionResult<bool>> WorkOrderStageChange(long Id, int stageId)
        {
            bool res = await _workOrderService.WorkOrderStageChange(Id, stageId);
            return Ok(res);
        }

        
    }
}