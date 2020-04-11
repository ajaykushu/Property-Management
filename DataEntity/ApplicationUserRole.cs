using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataEntity
{
    public class ApplicationUserRole:IdentityUserRole<long>
    {
        public virtual ApplicationRole Role { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
