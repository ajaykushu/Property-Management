using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Property.RequestModels;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Utilities.CustomException;
using Utilities.Interface;

namespace BusinessLogic.Services
{
    public class PropertyBL : IPropertyBL
    {
        private readonly IRepo<Property> _property;
        private readonly IRepo<PropertyType> _proptype;
        private readonly IRepo<UserProperty> _userProperty;
        private readonly IRepo<Location> _loc;
        private readonly IRepo<SubLocation> _subloaction;
        private readonly IRepo<Item> _item;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly INotifier _notifier;

        public PropertyBL(IRepo<Property> property, IRepo<PropertyType> proptype, IRepo<Location> loc, IRepo<SubLocation> subloaction, IRepo<UserProperty> userProperty, IHttpContextAccessor httpContextAccessor, INotifier notifier, IRepo<Item> item)
        {
            _property = property;
            _proptype = proptype;
            _loc = loc;
            _subloaction = subloaction;
            _userProperty = userProperty;
            _httpContextAccessor = httpContextAccessor;
            _notifier = notifier;
            _item = item;
        }

        public PropertyOperationDTO GetPropertyType()
        {
            var res = _proptype.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.PropertyTypeName }).AsNoTracking().ToList();
            if (res == null)
                throw new BadRequestException("Property type is not available");
            PropertyOperationDTO prop = new PropertyOperationDTO
            {
                PropertyTypes = res
            };
            return prop;
        }

        public async Task<bool> AddProperty(PropertyOperationDTO modal)
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
                throw new BadRequestException("Add Property failed");
            }
        }

        public async Task<List<PropertiesModel>> GetProperties()
        {
            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin") || _httpContextAccessor.HttpContext.User.IsInRole("User"))
            {
                var username = _httpContextAccessor.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
                if (username != null)
                {
                    var prop = await _userProperty.GetAll().Where(x => x.ApplicationUser.UserName == username.Value).Include(x => x.Property).Select(
                   x => new PropertiesModel
                   {
                       City = x.Property.City,
                       Country = x.Property.Country,
                       Id = x.PropertyId,
                       StreetAddress2 = x.Property.StreetAddress2,
                       ZipCode = x.Property.ZipCode,
                       PropertyName = x.Property.PropertyName,
                       PropertyType = x.Property.PropertyTypes.PropertyTypeName,
                       StreetAddress1 = x.Property.StreetAddress1,
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
                var prop = await _property.GetAll().Select(
                   x => new PropertiesModel
                   {
                       City = x.City,
                       Country = x.Country,
                       Id = x.Id,
                       StreetAddress2 = x.StreetAddress2,
                       ZipCode = x.ZipCode,
                       PropertyName = x.PropertyName,
                       PropertyType = x.PropertyTypes.PropertyTypeName,
                       StreetAddress1 = x.StreetAddress1,
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
                string message;
                if (prop.IsActive)
                    message = prop.PropertyName + " is activate now";
                else
                    message = prop.PropertyName + " is deactive now";
                int status = await _property.Update(prop);
                if (status > 0)
                {
                    var users = prop.UserProperties.Select(x => x.ApplicationUserId).ToList();
                    await _notifier.CreateNotification(message, users, prop.Id + "", "PE");
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

        public async Task<PropertyOperationDTO> GetProperty(long id)
        {
            var prop = await _property.Get(x => x.Id == id).Select(x => new PropertyOperationDTO
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

        public async Task<bool> UpdateProperty(PropertyOperationDTO prop)
        {
            var property = _property.Get(x => x.Id == prop.Id).FirstOrDefault();
            var status = false;
            if (property != null)
            {
                //property.Country = prop.Country;
                property.City = prop.City;
                property.StreetAddress1 = prop.StreetAddress1;
                property.StreetAddress2 = prop.StreetAddress2;
                property.PropertyName = prop.PropertyName;
                property.PropertyTypeId = prop.PropertyTypeId;
                property.State = prop.State;
                property.ZipCode = prop.ZipCode;
                status = Convert.ToBoolean(await _property.Update(property));
                if (status)
                {
                    var users = _userProperty.Get(x => x.PropertyId == prop.Id).Select(x => x.ApplicationUserId).ToList();
                    await _notifier.CreateNotification("Property Updated" + prop.Id, users, prop.Id + "", "PE");
                }
            }
            else
                throw new BadRequestException("Property Not Found");
            return status;
        }

        public async Task<bool> CheckProperty(string propertyName)
        {
            bool status;
            var res = await _property.Get(x => x.PropertyName.ToLower().Equals(propertyName.ToLower())).FirstOrDefaultAsync();
            status = res == null ? false : true;
            return status;
        }

        public async Task<PropertyConfigDTO> GetPropertyConfig(long id)
        {
            var res = await _loc.Get(x => x.PropertyId == id).Where(x=>x.Active).Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.LocationName
            }).ToListAsync();
            var prop = await _property.Get(x => x.Id == id).FirstOrDefaultAsync();
            var proprtyconfig = new PropertyConfigDTO
            {
                Locations = res,
                PropertyId = id,
                Name = prop.PropertyName,
            };
            return proprtyconfig;
        }

        public async Task<bool> SavePropertyConfig(PropertyConfigDTO propertyConfig)
        {
            Location location = null;
            if (string.IsNullOrWhiteSpace(propertyConfig.NewLocation) && !propertyConfig.LocationId.HasValue)
            {
                throw new BadRequestException("Choose Location or Enter New Location");
            }

            var prop = await _property.Get(x => x.Id == propertyConfig.PropertyId).Include(x => x.Locations).ThenInclude(x => x.SubLocations).ThenInclude(x => x.WorkOrders).Include(x=>x.Locations).ThenInclude(x => x.WorkOrders).FirstOrDefaultAsync();

            
            HashSet<string> areas = null;
            if (!string.IsNullOrWhiteSpace(propertyConfig.SubLocation))
            {
                if (propertyConfig.SubLocation.Contains(','))
                    areas = propertyConfig.SubLocation.Split(',').ToHashSet();
                else
                {
                    areas = new HashSet<string>
                    {
                        propertyConfig.SubLocation
                    };
                }
            }
            
            if (!string.IsNullOrWhiteSpace(propertyConfig.NewLocation))
            {
                if (prop.Locations == null) prop.Locations = new List<Location>();
                else
                {
                    if (prop.Locations.Where(x => x.LocationName.ToLower() == propertyConfig.NewLocation.ToLower()).FirstOrDefault() != null)
                        throw new BadRequestException(propertyConfig.NewLocation + " location aready there");
                }

                 location = new Location
                {
                    LocationName = propertyConfig.NewLocation,
                    Active=true,
                    SubLocations = new List<SubLocation>()
                };
                if (areas != null)
                {
                    foreach (var item in areas)
                    {
                        location.SubLocations.Add(new SubLocation
                        {
                            AreaName = item
                        });
                    }
                }
                prop.Locations.Add(location);
            }
            else if (propertyConfig.LocationId.HasValue)
            {
                 location = prop.Locations.Where(x => x.Id == propertyConfig.LocationId).FirstOrDefault();

                if (location != null)
                {
                    if (location.SubLocations == null)
                    {
                        location.SubLocations = new List<SubLocation>();
                        if (areas != null)
                        {
                            foreach (var area in areas)
                            {
                                location.SubLocations.Add(new SubLocation
                                {
                                    AreaName = area
                                });

                            }

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
                        if (areas != null)
                        {
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
                //itms
               
                
            }
            var status = await _property.Update(prop);
            if (status > 0)
            {
                return true;
            }
            return false;
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

        public async Task<string> GetPropertyData(long id)
        {
            var res = await _subloaction.Get(x => x.LocationId == id).Select(x => x.AreaName).ToListAsync();
            return string.Join(',', res);
        }

        public async Task<bool> DeleteLocation(long id)
        {
            //for loc with wo
           var obj= _loc.Get(x => x.Id == id).FirstOrDefault();
            if(obj!=null)
            {
                obj.Active = false;
               var status= await _loc.Update(obj); 
                if (status > 0)
                    return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsset(long id)
        {
           var obj= _item.Get(x => x.Id == id).FirstOrDefault();
            if (obj != null)
            {
                obj.Active = false;
                var status = await _item.Update(obj);
                if (status > 0)
                    return true;
            }
            return false;
        }

        public async Task<AssetManagerModel> GetAssetManager()
        {
            var AssetManagerModel = new AssetManagerModel();
            AssetManagerModel.Property = await _property.Get(x => x.IsActive).Select(x => new SelectItem
            {
                Id=x.Id,
                PropertyName=x.PropertyName
            }).ToListAsync();
            //var loc=await _loc.Get(x=>x.PropertyId==).ThenInclude(y=>y.Items).ToListAsync();
            //AssetManagerModel.Location=loc.Select(x=>x.)

            return AssetManagerModel;
        }

        public async Task<bool> SaveAsset(AssetManagerModel asset)
        {
            var issues = asset.Issues.Split(",").ToHashSet<string>();
            if (!asset.ItemId.HasValue)
            {
                var assetobj = new Item();
                assetobj.ItemName = asset.NewAsset;
                assetobj.LocationId = asset.LocationId;
                assetobj.Active = true;
                assetobj.Issues = new List<Issue>();
                
                foreach(var issue in issues)
                {
                    assetobj.Issues.Add(new Issue
                    {
                        IssueName=issue,
                       
                    });
                }
               var status= await _item.Add(assetobj);
                if (status > 0)
                    return true;
                return false;


            }
            else
            {
                var item = _item.Get(x => x.Id == asset.ItemId).Include(x=>x.Issues).ThenInclude(y=>y.WorkOrders).FirstOrDefault();
                if(item!=null)
                {
                    if(item.Issues==null|| item.Issues.Count == 0)
                    {
                        item.Issues = new List<Issue>();
                        //add as above
                        
                        foreach (var issue in issues)
                        {
                            item.Issues.Add(new Issue
                            {
                                IssueName = issue,

                            });
                        }
                    }
                    else
                    {
                       
                        for (int i = 0; i < item.Issues.Count;)
                        {
                            if (!issues.Contains(item.Issues.ElementAt(i).IssueName))
                            {
                                if (item.Issues.ElementAt(i).WorkOrders.Count == 0)
                                    item.Issues.Remove(item.Issues.ElementAt(i));
                                else
                                    throw new BadRequestException(item.Issues.ElementAt(i).IssueName + " is assigned to workorder Ids: " + string.Join(",", item.Issues.ElementAt(i).WorkOrders.Select(x => x.Id).ToList()));
                            }
                            else
                            {
                                issues.Remove(item.Issues.ElementAt(i).IssueName);
                                i++;
                            }
                        }
                        if (issues != null)
                        {
                            foreach (var issue in issues)
                            {
                                item.Issues.Add(new Issue
                                {
                                    IssueName = issue
                                });
                            }
                        }
                    }

                  
                }
                var status = await _item.Update(item);
                if (status > 0)
                    return true;
                return false;

            }
        }

        public async  Task<List<SelectItem>> GetAssset(int loc)
        {
            return await _item.Get(x => x.LocationId == loc).Select(x => new SelectItem {Id=x.Id,PropertyName=x.ItemName }).ToListAsync();
        }

        public Task<List<Assets>> Assets()
        {
            throw new NotImplementedException();

        }
    }
}