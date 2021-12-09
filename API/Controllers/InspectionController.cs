using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using DataEntity;
using DataTransferObjects.ResponseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.CheckList.RequestModels;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InspectionController : ControllerBase
    {
        private readonly IInspect _ins;
        public InspectionController(IInspect ins)
        {
            _ins = ins;
        }
        [HttpPost]
        [Route("createinspection")]
        public async Task<ActionResult<string>> Inspection(InspectionDTO inspect)
        {
            string status = await _ins.AddInspection(inspect);
            return Ok(status);
        }
        [HttpGet]

        [Route("inspection")]
        public async Task<ActionResult<InspectionDTO>> Inspection()
        {
            InspectionDTO insp = new InspectionDTO();
             insp = await _ins.GetInspection();
            return Ok(insp);
        }
        [HttpPost]
        [Route("checklist")]
        public async Task<IActionResult> CheckList( CheckListDTO check)
        {
            bool test= await _ins.AddList(check);
            return Ok(test);
        }
       
        [HttpGet]
        [Route("checklist/{Id}")]
        public async Task<ActionResult<GroupedDTO>> CheckList(string Id)
        {
            List<GroupedDTO> grp =  _ins.GetCheckList(Id);
            return Ok(grp); 
        }

        [HttpGet]
        [Route("Inspections")]
        public async Task<ActionResult<List<InspectionsDTO>>> GetAllInspections()
        {
            List<InspectionsDTO> list = await _ins.GetInspections();
            return Ok(list);
        }
    }
}