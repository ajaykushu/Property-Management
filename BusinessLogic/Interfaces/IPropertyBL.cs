using Models;
using Models.Property.RequestModels;
using Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPropertyBL
    {
        Task<PropertyOperationDTO> GetProperty(long id);

        Task<bool> UpdateProperty(PropertyOperationDTO prop);

        Task<bool> AddProperty(PropertyOperationDTO modal);

        Task<List<PropertiesModel>> GetProperties();

        Task<bool> ActDeactProperty(int id, bool operation);

        PropertyOperationDTO GetPropertyType();

        //Task<bool> MarkPrimary(long id, long userId);

        Task<bool> CheckProperty(string userName);

        Task<PropertyConfigDTO> GetPropertyConfig(long id);

        Task<bool> SavePropertyConfig(PropertyConfigDTO propertyConfig);

        Task<List<SelectItem>> GetSubLocation(long id);
    }
}