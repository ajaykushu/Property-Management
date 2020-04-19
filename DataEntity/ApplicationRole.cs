using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DataEntity
{
    public class ApplicationRole : IdentityRole<long>
    {
        public virtual ICollection<RoleMenuMap> RoleMenuMaps { set; get; }
    }
}