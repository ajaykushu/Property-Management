﻿using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DataEntity
{
    public class ApplicationRole : IdentityRole<long>
    {
        public virtual ICollection<RoleMenuMap> RoleMenuMaps { set; get; }
        public int? DepartmentId { set; get; }
        public Department Department { set; get; }
        public virtual ICollection<WorkOrder> WorkOrdersAssigned { get; set; }
    }
}