using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Presentation.Utility.Interface;
using Presentation.ViewModels;
using System.Collections.Generic;

namespace Presentation.Controllers
{

    public class WorkOrderController : Controller
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IOptions<RouteConstModel> _apiRoute;

        public WorkOrderController(IHttpClientHelper httpClientHelper, IOptions<RouteConstModel> apiRoute)
        {
            _httpClientHelper = httpClientHelper;
            _apiRoute = apiRoute;
        }


        [HttpGet]

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult WorkOrderOverview()
        {
            return PartialView("_WorkOrderOverview");
        }
        [HttpGet]
        // [Authorize]
        public IActionResult CreateWorkOrder()
        {
            CreateWorkOrder createWorkOrder = new CreateWorkOrder
            {
                Areas = new List<SelectItem>() { new SelectItem() {
             Id=1,
             PropertyName="X"
            }},
                Departments = new List<SelectItem>() { new SelectItem() {
             Id=1,
             PropertyName="X"
            }},
                Enginnerings = new List<SelectItem>() { new SelectItem() {
             Id=1,
             PropertyName="X"
            }},
                Issues = new List<SelectItem>() { new SelectItem() {
             Id=1,
             PropertyName="X"
            }},
                Items = new List<SelectItem>() { new SelectItem() {
             Id=1,
             PropertyName="X"
            }},
                Locations = new List<SelectItem>() { new SelectItem() {
             Id=1,
             PropertyName="X"
            }},
                RequestedBy = new List<SelectItem>() { new SelectItem() {
             Id=1,
             PropertyName="X"
            }},

            };
            return PartialView("_CreateWorkOrder", createWorkOrder);
        }

        [HttpPost]
        public IActionResult AddWorkOrder(CreateWorkOrder workOrder)
        {

            if (ModelState.IsValid)
            {
                return Ok("hello");
            }
            return null;
        }
    }
}
