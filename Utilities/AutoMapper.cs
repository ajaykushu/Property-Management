using AutoMapper;
using DataEntity;
using Models.RequestModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<EditWorkOrder, WorkOrder>();
        }
       
    }
}
