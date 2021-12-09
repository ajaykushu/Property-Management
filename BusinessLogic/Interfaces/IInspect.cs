using DataEntity;
using DataTransferObjects.ResponseModels;
using Models.CheckList.RequestModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IInspect
    {
        Task<string> AddInspection(InspectionDTO inspect);
        Task<InspectionDTO> GetInspection();
        List<GroupedDTO> GetCheckList(string id);
        Task<bool> AddList(CheckListDTO check);
        Task<List<InspectionsDTO>> GetInspections();
    }
}
