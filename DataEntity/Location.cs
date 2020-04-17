using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataEntity
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string LocationName { get; set; }
        public long PropertyId { get; set; }
        public Property Property { get; set; }
        public virtual ICollection<Area> Areas { get; set; }
    }
}
