using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Models.CheckList.RequestModels
{
    public class CheckListDTO
    {

        List<SelectItem> Locations { get; set; }
        List<SelectItem> Sublocations { get; set; }
        public string InspectionId { set; get; }
        public int LocationId { set; get; }
        public string Description { get; set; }
        public int? SubLocationId { get; set; }
        

    }

    
}
