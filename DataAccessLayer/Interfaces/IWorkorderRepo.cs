using DataEntity;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public interface IWorkorderRepo:IRepo<WorkOrder>
    {
        Task<WorkOrderAssigned> FilterData();
        
    }
}
