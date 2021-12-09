using System;
using System.Collections.Generic;
using System.Text;

namespace DataTransferObjects.ResponseModels
{
    public class InspectionsDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Property { get; set; }
        public string Inspector { get; set; }
        public DateTime  StartDate { get; set; }
        public int Items { get; set; }
        public int  IsComplete { get; set; }
        public string InspectionId { get; set; }
        public long PropertyId { get; set; }
    }
}
