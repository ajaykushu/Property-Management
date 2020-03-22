using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Country
    {
        public Country()
        {
            this.ApplicationUsers = new HashSet<ApplicationUser>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(2)")]
        public string ISO2 { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string Name { get; set; }

        [Column(TypeName = "varchar(80)")]
        public string Nicename { get; set; }

        [Column(TypeName = "varchar(3)")]
        public string ISO3 { get; set; }
        public int? Numcode { get; set; }

        public int? PhoneCode { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }

    }
}
