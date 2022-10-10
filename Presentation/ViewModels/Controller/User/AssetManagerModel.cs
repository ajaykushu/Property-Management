using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class AssetManagerModel
    {
       
        public List<SelectItem> Property { get; set; }
        [Required(ErrorMessage = "Please Select Property")]
        public int PropertyId { get; set; }
        [Required(ErrorMessage ="Please Select Location")]
        public int LocationId { get; set; }
        [Required(ErrorMessage = "Please Select Item")]
        public long? ItemId { get; set; }
        public string Issues { get; set; }
        [Required(ErrorMessage = "Please Enter New Asset")]
        public string NewAsset { get; set; }
    }
}
