using AutoMapper;
using DataEntity;
using Models.RequestModels;

namespace Utilities
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<EditWorkOrder, WorkOrder>();
        }

    }
}
