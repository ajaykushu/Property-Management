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

        Task<Dictionary<string, List<SelectItem>>> GetDataByCategory(string category);

        Task<bool> CreateWO(CreateWO createWO, List<IFormFile> File);

        Task<WorkOrderDetail> GetWODetail(string id);

        Task<Pagination<List<WorkOrderAssigned>>> GetWO(WOFilterModel wOFilterModel);

        Task<EditWorkOrder> GetEditWO(string id);

        Task<bool> EditWO(EditWorkOrder editWorkOrder, List<IFormFile> File);

        Task<List<CommentDTO>> GetComment(string workorderId, int pageNumber);

        Task<bool> PostComment(Post post);

        Task<bool> WorkOrderStatusChange(string Id, int statusId, string comment);

        Task<List<SelectItem>> GetLocation(long id);

        Task<List<AllWOExport>> WOExport(WOFilterModel wOFilterModel);
        Task<List<HistoryDetail>> GetHistory(string entity,string rowId);
    }
}