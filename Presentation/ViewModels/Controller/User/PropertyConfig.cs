using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class PropertyConfig
    {
        public long PropertyId { get; set; }
        public List<SelectItem> Locations { set; get; }
        public int LocationId { get; set; }
        public string NewLocation { get; set; }
        public string Area { set; get; }
    }
}
