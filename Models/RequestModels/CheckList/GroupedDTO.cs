using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.CheckList.RequestModels
{
    public class GroupedDTO
    {
        public string LocationName { set; get; }
        public int LocationId { set; get; }
        public string InspectionId { get; set; }
        public int Order { get; set; }
        public List<ItemDTO> Items { get; set; }
    }

    public class ItemDTO
    {
        
            public string Id { get; set; }
            public string Description { get; set; }
            public int LocationId { get; set; }
            public int Order { get; set; }
            public bool Status { get; set; }
        
    }
}
