using DataTransferObjects.RequestModels;
using Models.WorkOrder.RequestModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IHomeBL
    {
        public  Task<DashBoard> GetDashboard();
        public Task<List<LoctionDetail>> LocationView(long Id);
        Task<List<SubLocationModel>> Sublocation(long loc);
    }
}
