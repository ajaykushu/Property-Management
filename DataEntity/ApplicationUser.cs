using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            this.WorkerTyperUserMaps = new HashSet<WorkerTyperUserMap>();
        }

        [Required]
        [Column(TypeName = "varchar(4)")]
        public string Title { set; get; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string FirstName { set; get; }

        [Column(TypeName = "varchar(50)")]
        [Required]
        public string LastName { set; get; }

        [Column(TypeName = "varchar(10)")]
        public string Suffix { set; get; }

        [Required]
        public bool SMSAltert { get; set; }
        public Languages Language { get; set; }

        [Required]
        public int LanguageId { get; set; }
        [Required]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string TimeZone { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string ClockType { set; get; }
        [Column(TypeName = "varchar(10)")]
        public string OfficeExt { set; get; }
        public string PhotoPath { set; get; }
        public virtual ICollection<UserProperty> UserProperties { set; get; }
        //for worker types
        public virtual ICollection<WorkerTyperUserMap> WorkerTyperUserMaps { set; get; }
        //for manager
        public long? ManagerId { set; get; }
        public virtual ApplicationUser Manager { get; set; }
        public bool IsActive { get; set; }
    }

    public class ApplicationUserPhoneValidator : IUserValidator<ApplicationUser>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            var appuser = await manager.FindByIdAsync(user.Id.ToString());
            if (appuser != null)
            {
                if (manager.Users.Where(x => x.PhoneNumber == user.PhoneNumber && !appuser.Id.Equals(x.Id)).FirstOrDefault() != null)
                    return IdentityResult.Failed(new IdentityError { Description = "Phone number is already present" });
            }
            else
                if (manager.Users.Where(x => x.PhoneNumber == user.PhoneNumber).FirstOrDefault() != null)
                return IdentityResult.Failed(new IdentityError { Description = "Phone number is already present" });


            return IdentityResult.Success;
        }
    }
}
