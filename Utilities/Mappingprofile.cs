using AutoMapper;
using DataEntity;
using Models.ResponseModels;

namespace Utilities
{
    public class Mappingprofile : Profile
    {
        public Mappingprofile()
        {
            CreateMap<AddProperty, Property>();
        }
    }
}
