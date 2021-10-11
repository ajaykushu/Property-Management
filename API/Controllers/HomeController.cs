using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.WorkOrder.RequestModels;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {

        private  IHomeBL _homeBL { get; set; }
        public HomeController(IHomeBL homeBL)
        {
            _homeBL = homeBL;
        }

        [HttpGet("getdashboard")]
       public async Task<ActionResult<DashBoard>> Dashboard()
        {
            var obj =await _homeBL.GetDashboard();
            return Ok(obj);
        }
        [HttpGet("locationView")]
        public async Task<ActionResult<List<LoctionDetail>>> LocationView(long Id)
        {
            var obj = await _homeBL.LocationView(Id);
            return Ok(obj);

        }
        [HttpGet("sublocationview")]
        public async Task<ActionResult<List<LoctionDetail>>> Sublocation(long Id)
        {
            var obj = await _homeBL.Sublocation(Id);
            return Ok(obj);

        }
    }
}