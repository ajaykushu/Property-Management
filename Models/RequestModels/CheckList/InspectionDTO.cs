using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models.CheckList.RequestModels
{
    public class InspectionDTO
    {
        
        public string Id { get; set; }
       
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public int Type { get; set; } //1 for inspection and 2 for checklist

        
        public int DepartmentID { get; set; }
        
        public string CronExpression { get; set; }
        public int Status { get; set; }//1 in progress 2 complete  3 pause
        public List<SelectItem> Departments { get; set; }
        public List<SelectItem> Properties { get; set; }
       
        public long PropertyId { get; set; }

    }
}
