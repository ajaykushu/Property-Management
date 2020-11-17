using System;
using System.Collections.Generic;
using System.Text;

namespace Models.WorkOrder.RequestModels
{
    public class EffortDTO
    {
        public int BOTax { get; set; }
        public int Repair { get; set; }
        public DateTime DateTime { get; set; }
        public DayOfWeek Day { get {
                return this.DateTime.DayOfWeek;
            }
        }
        public int TotalForDay { get {
                return  this.BOTax + this.Repair;
            }
        }
        public bool IsEditable { get {
                var dt = DateTime.Now.Date;
                if (this.DateTime >= dt)
                    return false;
                else
                    return true;
            }
           
        }

    }
}
