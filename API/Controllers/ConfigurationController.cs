﻿using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Presentation.ViewModels.Controller.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigService _configService;

        public ConfigurationController(IConfigService configService)
        {
            _configService = configService;
        }

        [HttpGet]
        [Route("getfeatues")]
        [FeatureBasedAuthorization(MenuEnum.Edit_Feature)]
        public async Task<ActionResult<List<FeatureRoleModel>>> GetAllRolesAndFeatues(long id)
        {
            var data = await _configService.GetFeatureRoles(id);
            return Ok(data);
        }

        [HttpGet]
        [Route("getrole")]
        [FeatureBasedAuthorization(MenuEnum.Edit_Feature)]
        public async Task<ActionResult<List<SelectItem>>> GetRoles()
        {
            var data = await _configService.GetRoles();
            return Ok(data);
        }

        [HttpPost]
        [Route("updatefeature")]
        [FeatureBasedAuthorization(MenuEnum.Edit_Feature)]
        public async Task<ActionResult<bool>> UpdateFeature(KeyValuePair<int, List<string>> valuePairs)
        {
            var data = await _configService.UpdateFeature(valuePairs);
            return Ok(data);
        }
    }
}