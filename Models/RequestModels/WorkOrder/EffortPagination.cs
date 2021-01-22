using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.WorkOrder.RequestModels
{
    public class EffortPagination
    {
        public List<EffortDTO> EffortDTOs { get; set; }

        public DateTime Lastday { get; set; }
        public DateTime FistDay { get; set; }

        public int TotalEffort { get{
                return this.EffortDTOs.Sum(x => x.Repair+x.Service);
            }
        }


    }
}
