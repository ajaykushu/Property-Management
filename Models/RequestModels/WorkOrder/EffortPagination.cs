using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.WorkOrder.RequestModels
{
    public class EffortPagination
    {
        public List<EffortDTO> EffortDTOs { get; set; }
        public int TotalEffortOfWeek {
            get
            {
                return this.EffortDTOs.Sum(x => x.TotalForDay);
            }
        }

        public int TotalBOTaxEffortOfWeek
        {
            get { return this.EffortDTOs.Sum(x => x.BOTax); }
        }
        public int TotalRepairEffortOfWeek
        {
            get { return this.EffortDTOs.Sum(x => x.Repair); }
        }
        public DateTime? LastDay { get {
                if (this.EffortDTOs != null && this.EffortDTOs.Any())
                    return this.EffortDTOs.Last().DateTime;
                else
                    return null;
            }
        }

        public DateTime? FirstDay { get {
                if (this.EffortDTOs != null && this.EffortDTOs.Any())
                    return this.EffortDTOs.First().DateTime;
                else
                    return null;
            } }


    }
}
