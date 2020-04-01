using AutoMapper;
using DataEntity;
using Models.RequestModels;

namespace Utilities
{
    public class Mappingprofile : Profile
    {
        public Mappingprofile()
        {
            CreateMap<PropertyOperationModel, Property>();
        }
    }
}
