using Microsoft.AspNetCore.Identity;

namespace DataEntity
{
    public class ApplicationUserRole : IdentityUserRole<long>
    {
        public virtual ApplicationRole Role { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}