using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        [FeatureBasedAuthorization("Add Property")]
        [Route("getproperty")]
        public async Task<PropertyOperationModel> GetPropertyType(long id) => await _propertyService.GetProperty(id);

        [HttpPost]
        [FeatureBasedAuthorization("Edit Property")]
        [Route("updateproperty")]
        public async Task<IActionResult> UpdateProperty(PropertyOperationModel prop)
        {
            var status = await _propertyService.UpdateProperty(prop);
            return Ok(status);
        }
        [HttpGet]
        [Route("listproperties")]
        [FeatureBasedAuthorization("View Property")]
        public async Task<ActionResult<List<PropertiesModel>>> GetProperties()
        {
            List<PropertiesModel> properties = await _propertyService.GetProperties();
            return properties;

        }
        [FeatureBasedAuthorization("Add Property")]
        [HttpPost]
        [Route("addproperty")]
        public async Task<ActionResult<bool>> AddProperty([FromBody] PropertyOperationModel modal)
        {
            var status = await _propertyService.AddProperty(modal);
            return Ok(status);

        }
        [HttpGet]
        [Route("getPropertyTypes")]
        [FeatureBasedAuthorization("Add Property")]
        public ActionResult<PropertyOperationModel> GetPropertyTypes()
        {
            return _propertyService.GetPropertyType();
        }
        [HttpGet]
        [Route("deleteproperty")]
        [FeatureBasedAuthorization("Delete Property")]
        public async Task<ActionResult<bool>> DeleteProperty(int id)
        {
            var status = await _propertyService.DeleteProperty(id);
            return Ok(status);
        }
        [HttpGet]
        [Route("markprimary")]
        public async Task<ActionResult<bool>> MarkPrimary(long id, long userId)
        {
            bool status = await _propertyService.MarkPrimary(id, userId);
            return Ok(status);
        }
    }
}