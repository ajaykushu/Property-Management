using Microsoft.AspNetCore.Http;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IRecurringBL
    {
        Task<bool> WorkOrderStatusChange(WorkOrderDetail workOrderDetail, IList<IFormFile> files);
    }
}
