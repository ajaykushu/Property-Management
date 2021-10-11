using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.WorkOrder.RequestModels
{
    public class DashBoard
    {
        public List<SelectItem> Properties { get; set; }
        public List<LoctionDetail> Locations { get; set; }
    }

    public class LoctionDetail
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public bool HasPendingWorkorder { get; set; }
        public  int? WorkOrderCount { get; set; }
        public List<KeyValuePair<string,int>> StateDistribution { get; set; }

    }
}
