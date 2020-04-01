using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.CustomException;

namespace BusinessLogic.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IRepo<Property> _property;
        private readonly IRepo<PropertyType> _proptype;
        private readonly IRepo<ApplicationUser> _user;
        public PropertyService(IRepo<Property> property, IRepo<PropertyType> proptype, IRepo<ApplicationUser> user)
        {
            _property = property;
            _proptype = proptype;
            _user = user;
        }
        public PropertyOperationModel GetPropertyType()
        {
            var res = _proptype.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.PropertyTypeName }).AsNoTracking().ToList();
            if (res == null)
                throw new BadRequestException("Property type is not available");
            PropertyOperationModel prop = new PropertyOperationModel
            {
                PropertyTypes = res
            };
            return prop;
        }
        public async Task<bool> AddProperty(PropertyOperationModel modal)
        {

            var property = _property.Get(x => x.PropertyName.ToLower().Equals(modal.PropertyName.ToLower())).FirstOrDefault();
            if (property != null)
            {
                throw new BadRequestException("Property Name already available");
            }
            Property prop = new Property()
            {
                City = modal.City,
                Country = modal.Country,
                HouseNumber = modal.HouseNumber,
                StreetLine2 = modal.StreetLine2,
                Locality = modal.Locality,
                PropertyName = modal.PropertyName,
                PropertyTypes = _proptype.Get(x => x.Id == modal.PropertyTypeId).FirstOrDefault(),
                Street = modal.Street,
                PinCode = modal.PinCode

            };
            var res = await _property.Add(prop);
            if (res > 0)
            {
                return true;
            }
            else
            {
                throw new BadRequestException("Add user failed");
            }
        }



        public async Task<List<PropertiesModel>> GetProperties()
        {
            var prop = await _property.GetAll().Select(
                x => new PropertiesModel
                {
                    City = x.City,
                    Country = x.Country,
                    HouseNumber = x.HouseNumber,
                    Id = x.Id,
                    StreetLine2 = x.StreetLine2,
                    Locality = x.Locality,
                    PinCode = x.PinCode,
                    PropertyName = x.PropertyName,
                    PropertyType = x.PropertyTypes.PropertyTypeName,
                    Street = x.Street

                }
                ).AsNoTracking().ToListAsync();

            return prop;


        }
        public async Task<bool> DeleteProperty(int id)
        {
            var prop = _property.Get(x => x.Id == id).Include(x => x.UserProperties).ThenInclude(x => x.ApplicationUser).FirstOrDefault();

            if (prop != null)
            {
                if (prop.UserProperties != null && prop.UserProperties.Count != 0)
                {
                    throw new BadRequestException("Unable to delete as this is propery is allocated to [ " + string.Join(",", prop.UserProperties.Select(x => x.ApplicationUser.UserName).ToList()) + " ]");
                }
                int status = await _property.Delete(prop);
                if (status > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new BadRequestException("Property not found");
            }
        }
        public async Task<PropertyOperationModel> GetProperty(long id)
        {
            var prop = await _property.Get(x => x.Id == id).Select(x => new PropertyOperationModel
            {
                City = x.City,
                Country = x.Country,
                HouseNumber = x.HouseNumber,
                StreetLine2 = x.StreetLine2,
                Locality = x.Locality,
                PinCode = x.PinCode,
                PropertyName = x.PropertyName,
                PropertyTypeId = x.PropertyTypeId,
                Street = x.Street,
                Id = x.Id
            }).AsNoTracking().FirstOrDefaultAsync();
            if (prop != null)
                prop.PropertyTypes = await _proptype.GetAll().Select(x => new SelectItem
                {
                    Id = x.Id,
                    PropertyName = x.PropertyTypeName
                }).ToListAsync();
            else
                throw new BadRequestException("Property Not Found");
            return prop;
        }

        public async Task<bool> UpdateProperty(PropertyOperationModel prop)
        {
            var property = _property.Get(x => x.Id == prop.Id).FirstOrDefault();
            var status = false;
            if (property != null)
            {
                property.HouseNumber = prop.HouseNumber;
                property.Country = prop.Country;
                property.City = prop.City;
                property.StreetLine2 = prop.StreetLine2;
                property.Locality = prop.Locality;
                property.PropertyName = prop.PropertyName;
                property.PropertyTypeId = prop.PropertyTypeId;
                property.Street = prop.Street;
                property.PinCode = prop.PinCode;
                status = Convert.ToBoolean(await _property.Update(property));
            }
            else
                throw new BadRequestException("Property Not Found");
            return status;

        }

        public async Task<bool> MarkPrimary(long Id, long userId)
        {


            var user = await _user.Get(x => x.Id == userId).Include(x => x.UserProperties).FirstOrDefaultAsync();
            if (user != null && user.UserProperties != null)
            {
                foreach (var property in user.UserProperties)
                {
                    if (property.PropertyId == Id)
                        property.IsPrimary = true;
                    else
                        property.IsPrimary = false;
                }
                var updatestatus = await _user.Update(user);
                if (updatestatus > 0)
                    return true;
                
            }
            
            return false;

        }
    }
}
