using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Utilities.CustomException;

namespace BusinessLogic.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IRepo<Property> _property;
        private readonly IRepo<PropertyType> _proptype;
        private readonly IRepo<UserProperty> _userProperty;
        private readonly IRepo<ApplicationUser> _user;
        private readonly IRepo<Location> _loc;
        private readonly IRepo<SubLocation> _subloaction;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PropertyService(IRepo<Property> property, IRepo<PropertyType> proptype, IRepo<ApplicationUser> user, IRepo<Location> loc, IRepo<SubLocation> subloaction, IRepo<UserProperty> userProperty, IHttpContextAccessor httpContextAccessor)
        {
            _property = property;
            _proptype = proptype;
            _user = user;
            _loc = loc;
            _subloaction = subloaction;
            _userProperty = userProperty;
            _httpContextAccessor = httpContextAccessor;
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
                State = modal.State,
                StreetAddress1 = modal.StreetAddress1,
                PropertyName = modal.PropertyName,
                PropertyTypes = _proptype.Get(x => x.Id == modal.PropertyTypeId).FirstOrDefault(),
                StreetAddress2 = modal.StreetAddress2,
                ZipCode = modal.ZipCode
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
            
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                var username = _httpContextAccessor.HttpContext.User.Claims.Where(x=>x.Type==ClaimTypes.NameIdentifier).FirstOrDefault();
                if (username != null)
                {
                    var prop = await _userProperty.GetAll().Where(x => x.ApplicationUser.UserName == username.Value).Include(x=>x.Property).Select(
                   x => new PropertiesModel
                   {
                       City = x.Property.City,
                       Country = x.Property.Country,
                       Id = x.Id,
                       StreetAddress2 = x.Property.StreetAddress2,
                       ZipCode = x.Property.ZipCode,
                       PropertyName = x.Property.PropertyName,
                       PropertyType = x.Property.PropertyTypes.PropertyTypeName,
                       StreetAddress1 = x.Property.StreetAddress2,
                       State = x.Property.State,
                       IsActive = x.Property.IsActive
                   }
                   ).AsNoTracking().ToListAsync();
                   return prop;
                }
                else
                {
                    throw new BadRequestException("No Properties Asscociated");
                }
              
            }
            else
            {
                var prop =  await _property.GetAll().Select(
                   x => new PropertiesModel
                   {
                       City = x.City,
                       Country = x.Country,
                       Id = x.Id,
                       StreetAddress2 = x.StreetAddress2,
                       ZipCode = x.ZipCode,
                       PropertyName = x.PropertyName,
                       PropertyType = x.PropertyTypes.PropertyTypeName,
                       StreetAddress1 = x.StreetAddress2,
                       State = x.State,
                       IsActive = x.IsActive
                   }
                   ).AsNoTracking().ToListAsync();
                return prop;
            }
           
        }

        public async Task<bool> ActDeactProperty(int id, bool operation)
        {
            var prop = _property.Get(x => x.Id == id).Include(x => x.UserProperties).ThenInclude(x => x.ApplicationUser).FirstOrDefault();

            if (prop != null)
            {
                prop.IsActive = operation;
                int status = await _property.Update(prop);
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
                StreetAddress1 = x.StreetAddress1,
                ZipCode = x.ZipCode,
                PropertyName = x.PropertyName,
                PropertyTypeId = x.PropertyTypeId,
                StreetAddress2 = x.StreetAddress2,
                State = x.State,
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
                property.Country = prop.Country;
                property.City = prop.City;
                property.StreetAddress1 = prop.StreetAddress1;
                property.StreetAddress2 = prop.StreetAddress2;
                property.PropertyName = prop.PropertyName;
                property.PropertyTypeId = prop.PropertyTypeId;
                property.State = prop.State;
                property.ZipCode = prop.ZipCode;
                status = Convert.ToBoolean(await _property.Update(property));
            }
            else
                throw new BadRequestException("Property Not Found");
            return status;
        }

        //public async Task<bool> MarkPrimary(long Id, long userId)
        //{
        //    var user = await _user.Get(x => x.Id == userId).Include(x => x.UserProperties).FirstOrDefaultAsync();
        //    if (user != null && user.UserProperties != null)
        //    {
        //        foreach (var property in user.UserProperties)
        //        {
        //            if (property.PropertyId == Id)
        //                property.IsPrimary = true;
        //            else
        //                property.IsPrimary = false;
        //        }
        //        var updatestatus = await _user.Update(user);
        //        if (updatestatus > 0)
        //            return true;
        //    }

        //    return false;
        //}

        public async Task<bool> CheckProperty(string propertyName)
        {
            bool status;
            var res = await _property.Get(x => x.PropertyName.ToLower().Equals(propertyName.ToLower())).FirstOrDefaultAsync();
            status = res == null ? false : true;
            return status;
        }

        public async Task<PropertyConfig> GetPropertyConfig(long id)
        {
            var res = await _loc.Get(x => x.PropertyId == id).Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.LocationName
            }).ToListAsync();
            var proprtyconfig = new PropertyConfig
            {
                Locations = res,
                PropertyId = id
            };
            return proprtyconfig;
        }

        public async Task<bool> SavePropertyConfig(PropertyConfig propertyConfig)
        {
            var prop = await _property.Get(x => x.Id == propertyConfig.PropertyId).Include(x => x.Locations).ThenInclude(x => x.SubLocations).ThenInclude(x => x.WorkOrders).FirstOrDefaultAsync();
            HashSet<string> areas = null;
            if (!string.IsNullOrWhiteSpace(propertyConfig.SubLocation))
            {
                if (propertyConfig.SubLocation.Contains(','))
                    areas = propertyConfig.SubLocation.Split(',').ToHashSet();
                else
                    areas.Append(propertyConfig.SubLocation);
            }
            if (!string.IsNullOrWhiteSpace(propertyConfig.NewLocation))
            {
                if (prop.Locations == null) prop.Locations = new List<Location>();
                var location = new Location
                {
                    LocationName = propertyConfig.NewLocation,
                    SubLocations = new List<SubLocation>()
                };
                foreach (var item in areas)
                {
                    location.SubLocations.Add(new SubLocation
                    {
                        AreaName = item
                    });
                }
                prop.Locations.Add(location);
            }
            else if (propertyConfig.LocationId != 0)
            {
                var location = prop.Locations.Where(x => x.Id == propertyConfig.LocationId).FirstOrDefault();
                if (location != null)
                {
                    if (location.SubLocations == null)
                    {
                        location.SubLocations = new List<SubLocation>();
                        foreach (var item in areas)
                        {
                            location.SubLocations.Add(new SubLocation
                            {
                                AreaName = item
                            });
                        }
                    }
                    else
                    {
                        for (int i = 0; i < location.SubLocations.Count;)
                        {
                            if (!areas.Contains(location.SubLocations.ElementAt(i).AreaName))
                            {
                                if (location.SubLocations.ElementAt(i).WorkOrders.Count == 0)
                                    location.SubLocations.Remove(location.SubLocations.ElementAt(i));
                                else
                                    throw new BadRequestException(location.SubLocations.ElementAt(i).AreaName + " is assigned to workorder Ids: " + string.Join(",", location.SubLocations.ElementAt(i).WorkOrders.Select(x => x.Id).ToList()));
                            }
                            else
                            {
                                areas.Remove(location.SubLocations.ElementAt(i).AreaName);
                                i++;
                            }
                        }
                        foreach (var item in areas)
                        {
                            location.SubLocations.Add(new SubLocation
                            {
                                AreaName = item
                            });
                        }
                    }
                }
            }
            propertyConfig.Locations = prop.Locations.Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.LocationName
            }).ToList();
            var status = await _property.Update(prop);
            if (status > 0)
                return true;
            return false;
        }

        public async Task<string> GetArea(int id)
        {
            var res = await _subloaction.Get(x => x.LocationId == id).Select(x => x.AreaName).ToListAsync();
            return string.Join(',', res);
        }

        public async Task<List<SelectItem>> GetSubLocation(long id)
        {
            var res = await _subloaction.GetAll().Where(x => x.LocationId == id).Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.AreaName
            }).ToListAsync();
            return res;
        }
    }
}