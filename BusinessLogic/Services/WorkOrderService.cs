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
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IRepo<Issue> _issueRepo;
        private readonly IRepo<Item> _itemRepo;
        private readonly IRepo<UserProperty> _userProperty;
        public WorkOrderService(IRepo<Issue> issueRepo, IRepo<Item> itemRepo, IRepo<UserProperty> userProperty)
        {
            _issueRepo = issueRepo;
            _itemRepo = itemRepo;
            _userProperty = userProperty;
        }

        public async Task<PropDetail> GetAreaLocation(long id)
        {
            var primaryprop = await _userProperty.Get(x => x.PropertyId == id).Include(x=>x.Property).AsNoTracking().FirstOrDefaultAsync();
            StringBuilder sb = new StringBuilder();
            if (primaryprop != null)
            {
                if (!string.IsNullOrWhiteSpace(primaryprop.Property.Locality))
                {
                    sb.Append(primaryprop.Property.Locality);
                    sb.Append(", ");
                }
                sb.Append(primaryprop.Property.City);

            }
            PropDetail propDetail = new PropDetail
            {
                Area = primaryprop.Property.Street,
                Location = sb.ToString()
            };
            return propDetail;
        }

        public async Task<CreateWO> GetCreateWOModel(long userId)
        {
            var property = await _userProperty.GetAll().Where(x => x.ApplicationUserId == userId).Include(x => x.Property).AsNoTracking().ToListAsync();
            var primaryprop = property.Where(x => x.isPrimary == true).FirstOrDefault();
            StringBuilder sb = new StringBuilder();
            if (primaryprop != null)
            {
                if (!string.IsNullOrWhiteSpace(primaryprop.Property.Locality))
                {
                    sb.Append(primaryprop.Property.Locality);
                    sb.Append(", ");
                }
                sb.Append(primaryprop.Property.City);
            }

            CreateWO wo = new CreateWO()
            {
                Properties = property.Select(x => new SelectItem
                {
                    Id = x.Property.Id,
                    PropertyName = x.Property.PropertyName
                }).ToList(),
                Property = primaryprop != null ? primaryprop.PropertyId : 0,
                Issues = await _issueRepo.GetAll().Select(x => new SelectItem
                {
                    Id = x.Id,
                    PropertyName = x.IssueName
                }).ToListAsync(),
                Items = await _itemRepo.GetAll().Select(x => new SelectItem
                {
                    Id = x.Id,
                    PropertyName = x.ItemName
                }).ToListAsync(),

                Area = primaryprop != null ? primaryprop.Property.Street : "",
                Location = sb.ToString()
            };
            
            return wo;
        }
    }
}
