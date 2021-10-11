using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransferObjects.RequestModels
{
    public class SubLocationModel
    {
        public long Id { get; set; }
        public string SublocationName { get; set; }
        public bool HasPendingWorkder { get; set; }
        public List<string> Workorder { get; set; }

    }

    
}
