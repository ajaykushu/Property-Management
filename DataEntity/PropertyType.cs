using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class PropertyType : Log
    {
        public PropertyType()
        {
            this.Properties = new HashSet<Property>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string PropertyTypeName { set; get; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}