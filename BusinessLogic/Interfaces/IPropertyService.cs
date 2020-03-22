﻿using Models.RequestModels;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
   public interface IPropertyService
    {
        Task<PropertyOperationModel> GetProperty(long id);
        Task<bool> UpdateProperty(PropertyOperationModel prop);
        Task<bool> AddProperty(PropertyOperationModel modal);
        Task<List<PropertiesModel>> GetProperties();
        Task<bool> DeleteProperty(int id);
        PropertyOperationModel GetPropertyType();
    }
}
