using DataTransferObjects.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Models;
using Models.ResponseModels;
using Models.WorkOrder.RequestModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IWorkOrderBL
    {
        Task<CreateNormalWO> GetCreateWOModel(long userId);

        Task<Dictionary<string, List<SelectItem>>> GetDataByCategory(string category);

        Task<bool> CreateWO(CreateNormalWO createWO, List<IFormFile> File);

        Task<WorkOrderDetail> GetWODetail(string id);

        Task<Pagination<List<WorkOrderAssigned>>> GetWO(WOFilterDTO wOFilterModel);

        Task<EditNormalWorkOrder> GetEditWO(string id);

        Task<bool> EditWO(EditNormalWorkOrder editWorkOrder, List<IFormFile> File);

        Task<List<CommentDTO>> GetComment(string workorderId, int pageNumber);

        Task<bool> PostComment(PostCommentDTO post);

        Task<bool> WorkOrderStatusChange(WorkOrderDetail workOrderDetail, IList<IFormFile> files);

        Task<List<SelectItem>> GetLocation(long id);

        Task<List<AllWOExport>> WOExport(WOFilterDTO wOFilterModel);
        Task<List<HistoryDetail>> GetHistory(string entity,string rowId);
        Task<bool> CreateRecurringWO(CreateRecurringWODTO createWO, List<IFormFile> file);
        Task<EditRecurringWorkOrderDTO> GetEditRecurringWO(string id);
        Task<bool> EditRecurringWO(EditRecurringWorkOrderDTO editWorkOrder, List<IFormFile> file);
        Task<Pagination<List<RecurringWOs>>> GetRecurringWO(WOFilterDTO wOFilterDTO);
        Task<Pagination<List<ChildWo>>> GetChildWO(int pageNumber,string search, string rwoId);
        Task<WorkOrderDetail> GetRecurringWODetail(string rwoId);
        Task<List<AllWOExportRecurring>> WOExportRecurring(WOFilterDTO wOFilterModel);
        Task<bool> AddEffort(EffortPagination effortDTOs, string id);
        Task<EffortPagination> GetEffort(string id,bool prev);
        Task<List<SelectItem>> GetIssues( long id);
        Task<List<SelectItem>> GetItem(long id);
        Task<List<string>> GetWoList(long id);
    }
}