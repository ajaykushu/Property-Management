using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Property : Log
    {
        public Property()
        {
            this.UserProperties = new HashSet<UserProperty>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }

        [Column(TypeName = "varchar(50)")]
        public string PropertyName { get; set; }

        [Column(TypeName = "int")]
        public int PropertyTypeId { get; set; }

        public virtual PropertyType PropertyTypes { get; set; }
        
       
        [Column(TypeName = "varchar(100)")]
        public string StreetAddress1 { set; get; }

        [Column(TypeName = "varchar(100)")]
        public string StreetAddress2 { set; get; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string ZipCode { set; get; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string City { set; get; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string State { set; get; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Country { set; get; }
        [Required]
        [Column(TypeName = "bit")]
        public bool IsActive { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<UserProperty> UserProperties { get; set; }
    }
}