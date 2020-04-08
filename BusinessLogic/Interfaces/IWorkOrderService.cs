﻿using Models;
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

        Task<bool> CreateWO(CreateWO createWO);

        Task<WorkOrderDetail> GetWODetail(long id);

        Task<Pagination<List<WorkOrderAssigned>>> GetWO(int pageNumber, FilterEnumWO filter, string matchStr, FilterEnumWOStage stage, string enddate);

        Task<EditWorkOrder> GetEditWO(long id);

        Task<bool> EditWO(EditWorkOrder editWorkOrder);

        Task<Pagination<List<CommentDTO>>> GetPaginationComment(long workorderId, int pageNumber);

        Task<bool> PostComment(Post post);

        Task<bool> WorkOrderOperation(long workOrderId, ProcessEnumWOStage command);

        Task<bool> AssignToUser(long userId, long workOrderId);
    }
}