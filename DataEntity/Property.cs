﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataEntity
{
    public class Property
    {
        public Property()
        {
            this.UserProperties = new HashSet<UserProperty>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { set; get; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string PropertyName { get; set; }
        [Column(TypeName = "int")]
        //which type of property it is like hotel,mall,restrurent
        public int PropertyTypeId { get; set; }
        public virtual PropertyType PropertyTypes { get; set; }
        [Required]
        [Column(TypeName = "varchar(10)")]
        public string HouseNumber { set; get; }
        [Column(TypeName = "varchar(50)")]
        public string Locality { set; get; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Street { set; get; }
        [Column(TypeName = "varchar(100)")]
        public string LandMark { set; get; }
        [Required]
        [Column(TypeName = "varchar(8)")]
        public string PinCode { set; get; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string City { set; get; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Country { set; get; }

        public virtual ICollection<UserProperty> UserProperties { get; set; }

    }
}
