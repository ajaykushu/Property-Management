using Microsoft.AspNetCore.Http;
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

        Task<List<SelectItem>> GetUsersByDepartment(long id);

        Task<bool> CreateWO(CreateWO createWO, List<IFormFile> File);

        Task<WorkOrderDetail> GetWODetail(long id);

        Task<Pagination<List<WorkOrderAssigned>>> GetWO(WOFilterModel wOFilterModel);

        Task<EditWorkOrder> GetEditWO(long id);

        Task<bool> EditWO(EditWorkOrder editWorkOrder, List<IFormFile> File);

        Task<Pagination<List<CommentDTO>>> GetPaginationComment(long workorderId, int pageNumber);

        Task<bool> PostComment(Post post);

        Task<bool> WorkOrderStageChange(long Id, int stageId);

        Task<List<SelectItem>> GetLocation(long id);
    }
}