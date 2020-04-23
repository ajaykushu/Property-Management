using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        [FeatureBasedAuthorization(MenuEnum.Add_Property)]
        [Route("getproperty")]
        public async Task<PropertyOperationModel> GetPropertyType(long id) => await _propertyService.GetProperty(id);

        [HttpPost]
        [FeatureBasedAuthorization(MenuEnum.Edit_Property)]
        [Route("updateproperty")]
        public async Task<IActionResult> UpdateProperty(PropertyOperationModel prop)
        {
            var status = await _propertyService.UpdateProperty(prop);
            return Ok(status);
        }

        [HttpGet]
        [Route("listproperties")]
        [FeatureBasedAuthorization(MenuEnum.View_Property)]
        public async Task<ActionResult<List<PropertiesModel>>> GetProperties()
        {
            List<PropertiesModel> properties = await _propertyService.GetProperties();
            return properties;
        }

        [FeatureBasedAuthorization(MenuEnum.Add_Property)]
        [HttpPost]
        [Route("addproperty")]
        public async Task<ActionResult<bool>> AddProperty([FromBody] PropertyOperationModel modal)
        {
            var status = await _propertyService.AddProperty(modal);
            return Ok(status);
        }

        [HttpGet]
        [Route("getPropertyTypes")]
        [FeatureBasedAuthorization(MenuEnum.Add_Property)]
        public ActionResult<PropertyOperationModel> GetPropertyTypes()
        {
            var res = _propertyService.GetPropertyType();
            return Ok(res);
        }

        [HttpGet]
        [Route("actdeactproperty")]
        [FeatureBasedAuthorization(MenuEnum.Act_Deact_Property)]
        public async Task<ActionResult<bool>> ActDeactProperty(int id, bool operation)
        {
            var status = await _propertyService.ActDeactProperty(id, operation);
            return Ok(status);
        }

        [HttpGet]
        [Route("markprimary")]
        public async Task<ActionResult<bool>> MarkPrimary(long id, long userId)
        {
            bool status = await _propertyService.MarkPrimary(id, userId);
            return Ok(status);
        }

        [HttpGet]
        [Route("checkproperty")]
        public async Task<ActionResult<bool>> CheckProperty(string propertyName)
        {
            var res = await _propertyService.CheckProperty(propertyName);
            return Ok(res);
        }

        [HttpGet]
        [Route("propertyconfig")]
        public async Task<ActionResult<PropertyConfig>> PropertyConfig(long Id)
        {
            var res = await _propertyService.GetPropertyConfig(Id);
            return Ok(res);
        }

        [HttpPost]
        [Route("propertyconfig")]
        public async Task<ActionResult<bool>> PropertyConfig(PropertyConfig propertyConfig)
        {
            bool res = await _propertyService.SavePropertyConfig(propertyConfig);
            return Ok(res);
        }

        [HttpGet]
        [Route("getsublocation")]
        [FeatureBasedAuthorization(MenuEnum.Create_WO)]
        public async Task<ActionResult<SelectItem>> GetSubLocation(long id)
        {
            List<SelectItem> prop = await _propertyService.GetSubLocation(id);
            return Ok(prop);
        }
    }
}