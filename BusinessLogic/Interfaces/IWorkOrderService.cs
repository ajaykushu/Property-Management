using Models;
using Models.RequestModels;
using Models.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IWorkOrderService
    {
        Task<CreateWO> GetCreateWOModel(long userId);

        Task<PropDetail> GetAreaLocation(long id);

        Task<List<SelectItem>> GetSection(long id);

        Task<WorkOrderDetail> CreateWO(CreateWO createWO, long userId);
    }
}