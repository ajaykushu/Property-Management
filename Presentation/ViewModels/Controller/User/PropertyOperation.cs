using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class PropertyOperation
    {
        public long Id { get; set; }

        [DisplayName("Property Name")]
        [StringLength(50, ErrorMessage = "Must less than 50 characters")]
        [Required]
        public string PropertyName { get; set; }

        [Required(ErrorMessage = "Please Choose Property Type")]
        public int PropertyTypeId { get; set; }

        public List<SelectItem> PropertyTypes { get; set; }

        [DisplayName("Street Address 1")]
        [StringLength(100, ErrorMessage = "Must less than 100 characters")]
        public string StreetAddress1 { set; get; }

        [DisplayName("Street Address 2")]
        [StringLength(100, ErrorMessage = "Must less than 100 characters")]
        public string StreetAddress2 { set; get; }

        [DisplayName("Zip Code")]
        [StringLength(50, ErrorMessage = "Must less than 50 characters")]
        public string ZipCode { set; get; }

        [StringLength(50, ErrorMessage = "Must less than 50 characters")]
        public string City { set; get; }

        [StringLength(50, ErrorMessage = "Must less than 50 characters")]
        public string State { set; get; }

        [StringLength(50, ErrorMessage = "Must less than 50 characters")]
        public string Country { set; get; }
    }
}