using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class PayScale : Log
    {
        public PayScale()
        {
            this.WorkerTypes = new HashSet<ApplicationUser>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Code { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Billingtype { get; set; }

        [Column(TypeName = "money")]
        public decimal HourlyCost { get; set; }

        public ICollection<ApplicationUser> WorkerTypes { get; set; }
    }
}