using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Presentation.ViewModels.Controller.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigBL _configService;

        public ConfigurationController(IConfigBL configService)
        {

            _configService = configService;
        }
        /// <summary>
        /// Used to return features corresponding to role Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>List</returns>
        [HttpGet]
        [Route("getfeatues")]
        [FeatureBasedAuthorization(MenuEnum.Edit_Feature)]
        public async Task<ActionResult<List<FeatureRoleModel>>> GetAllFeatues(long id)
        {
            var data = await _configService.GetFeatureRoles(id);
            return Ok(data);
        }
        /// <summary>
        /// used to get all roles
        /// </summary>
        /// <returns>List</returns>
        [HttpGet]
        [Route("getrole")]
        [FeatureBasedAuthorization(MenuEnum.Edit_Feature)]
        public async Task<ActionResult<List<SelectItem>>> GetRoles()
        {
            var data = await _configService.GetRoles();
            return Ok(data);
        }
        /// <summary>
        /// used to update the feature corresponding to role
        /// </summary>
        /// <param name="valuePairs"></param>
        /// <returns>boolean</returns>
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