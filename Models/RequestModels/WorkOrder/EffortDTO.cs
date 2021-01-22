using System;
using System.Collections.Generic;
using System.Text;

namespace Models.WorkOrder.RequestModels
{
    public class EffortDTO
    {

        public DateTime DateTime { get; set; }
        public int Repair { get; set; }
        public int Service { get; set; }
        public bool Iseditable { get; set; }

    }
}
