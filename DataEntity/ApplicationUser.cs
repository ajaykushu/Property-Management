using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataEntity
{
    public class ApplicationUser : IdentityUser<long>
    {
        public ApplicationUser()
        {
            this.UserProperties = new HashSet<UserProperty>();
            this.WorkOrdersAssigned = new HashSet<WorkOrder>();
            this.Leaves = new HashSet<Leave>();
        }
        [Column(TypeName = "varchar(50)")]
        public string FirstName { set; get; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { set; get; }

        [Column(TypeName = "varchar(50)")]
        public string Suffix { set; get; }

        public bool SMSAltert { get; set; }

        public Languages Language { get; set; }
        public int? LanguageId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string TimeZone { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string ClockType { set; get; }

        [Column(TypeName = "varchar(50)")]
        public string OfficeExt { set; get; }
        public bool IsEffortVisible { get; set; }
        public string PhotoPath { set; get; }

        public int? DepartmentId { set; get; }
        public Department Department { set; get; }

        public bool IsActive { get; set; }
        public virtual ICollection<UserProperty> UserProperties { set; get; }
        public virtual ICollection<WorkOrder> WorkOrdersAssigned { get; set; }
        public virtual ICollection<Effort> Efforts { get; set; }
        public virtual ICollection<Leave> Leaves { get; set; }
    }

    public class ApplicationUserPhoneValidator : IUserValidator<ApplicationUser>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            var appuser = await manager.FindByIdAsync(user.Id.ToString());
            if (appuser != null)
            {
                if (manager.Users.Where(x => x.PhoneNumber == user.PhoneNumber && appuser.Id != x.Id).FirstOrDefault() != null)
                    return IdentityResult.Failed();
            }
            else
                if (manager.Users.Where(x => x.PhoneNumber == user.PhoneNumber).FirstOrDefault() != null)
                return IdentityResult.Failed();

            return IdentityResult.Success;
        }
    }
}