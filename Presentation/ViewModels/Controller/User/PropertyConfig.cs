using System.Collections.Generic;
using System.ComponentModel;

namespace Presentation.ViewModels
{
    public class PropertyConfig
    {
        public long PropertyId { get; set; }
        public  string Name { get; set; }
        public List<SelectItem> Locations { set; get; }
        public int? LocationId { get; set; }
        public string NewLocation { get; set; }

        [DisplayName("Sub Location")]
        public string SubLocation { set; get; }
        
    }
}