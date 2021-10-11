using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Property.RequestModels;
using Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyBL _propertyService;

        public PropertyController(IPropertyBL propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        [FeatureBasedAuthorization(MenuEnum.Add_Property)]
        [Route("getproperty")]
        public async Task<PropertyOperationDTO> GetPropertyType(long id) => await _propertyService.GetProperty(id);

        [HttpPost]
        [FeatureBasedAuthorization(MenuEnum.Edit_Property)]
        [Route("updateproperty")]
        public async Task<IActionResult> UpdateProperty(PropertyOperationDTO prop)
        {
            var status = await _propertyService.UpdateProperty(prop);
            return Ok(status);
        }

        [HttpGet]
        [Route("listproperties")]
        [ResponseCache(NoStore = true, Duration = 0)]
        [FeatureBasedAuthorization(MenuEnum.View_Property)]
        public async Task<ActionResult<List<PropertiesModel>>> GetProperties()
        {
            List<PropertiesModel> properties = await _propertyService.GetProperties();
            return properties;
        }

        [FeatureBasedAuthorization(MenuEnum.Add_Property)]
        [HttpPost]
        [Route("addproperty")]
        public async Task<ActionResult<bool>> AddProperty([FromBody] PropertyOperationDTO modal)
        {
            var status = await _propertyService.AddProperty(modal);
            return Ok(status);
        }

        [HttpGet]
        [Route("getPropertyTypes")]
        [FeatureBasedAuthorization(MenuEnum.Add_Property)]
        public ActionResult<PropertyOperationDTO> GetPropertyTypes()
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

        //[HttpGet]
        //[Route("markprimary")]
        //public async Task<ActionResult<bool>> MarkPrimary(long id, long userId)
        //{
        //    bool status = await _propertyService.MarkPrimary(id, userId);
        //    return Ok(status);
        //}

        [HttpGet]
        [Route("checkproperty")]
        public async Task<ActionResult<bool>> CheckProperty(string propertyName)
        {
            var res = await _propertyService.CheckProperty(propertyName);
            return Ok(res);
        }

        [HttpGet]
        [Route("propertyconfig")]
        public async Task<ActionResult<PropertyConfigDTO>> PropertyConfig(long Id)
        {
            var res = await _propertyService.GetPropertyConfig(Id);
            return Ok(res);
        }

        [HttpPost]
        [Route("propertyconfig")]
        public async Task<ActionResult<bool>> PropertyConfig(PropertyConfigDTO propertyConfig)
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

        [HttpGet]
        [Route("getpropertydata")]
        [FeatureBasedAuthorization(MenuEnum.Create_WO)]
        public async Task<ActionResult<SelectItem>> GetPropertyData(long id)
        {
            string prop = await _propertyService.GetPropertyData(id);
            return Ok(prop);
        }

       [HttpGet]
       [Route("deleteloc")]
       public async Task<ActionResult<bool>> DeleteLocation(long Id)
        {
            bool status = await _propertyService.DeleteLocation(Id);
            return Ok(status);
        }
        [HttpGet]
        [Route("deleteasset")]
        public async Task<ActionResult<bool>> DeleteAsset(long Id)
        {
            bool status = await _propertyService.DeleteAsset(Id);
            return Ok(status);
        }
        [HttpGet]
        [Route("assetmanager")]
        public async Task<ActionResult<PropertyConfigDTO>> AssetManager()
        {
             AssetManagerModel asset = await _propertyService.GetAssetManager();
            return Ok(asset);
        }
        [HttpGet]
        [Route("getasset")]
        public async Task<ActionResult<List<SelectItem>>> GetAssset(int location)
        {
            List<SelectItem> asset = await _propertyService.GetAssset(location);
            return Ok(asset);
        }

        [HttpPost]
        [Route("createasset")]
        public async Task<ActionResult<bool>> AssetManager(AssetManagerModel asset)
        {
            bool res = await _propertyService.SaveAsset(asset);
            return Ok(res);
        }
        [HttpGet]
        [Route("assets")]
        public async Task<ActionResult<bool>> Assets()
        {
            List<Assets> res = await _propertyService.Assets();
            return Ok(res);
        }

    }
}