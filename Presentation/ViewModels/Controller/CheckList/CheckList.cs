using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class CheckList
    {

        public List<SelectItem> Locations { get; set; }
        public string InspectionId { set; get; }
        [DisplayName("Location")]
        [Required(ErrorMessage = "Select Location")]
        public int LocationId { get; set; }
        [DisplayName("Sub Location")]
        public int? SublocationId { get; set; }
        [Required(ErrorMessage ="Enter description")]
       public string Description { get; set; }


    }

    
}
