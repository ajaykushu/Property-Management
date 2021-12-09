using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class Grouped
    {
        public string LocationName { set; get; }
        public int LocationId { set; get; }
        public int SubLocationId { get; set; }
        public string InspectionId { get; set; }
        public int Order { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        
            public string Id { get; set; }
            public string Description { get; set; }
            public int LocationId { get; set; }
            public int Order { get; set; }
            public bool Status { get; set; }
        
    }
}
