using Models.RequestModels;
using Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IWorkOrderService
    {
        Task<CreateWO> GetCreateWOModel(long userId);
        Task<PropDetail> GetAreaLocation(long id);
    }
}
