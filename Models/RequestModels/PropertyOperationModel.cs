using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.RequestModels
{
    public class PropertyOperationModel
    {
        public long Id { get; set; }

        [StringLength(50, ErrorMessage = "Must less than 50 characters")]
        [Required]
        public string PropertyName { get; set; }

        [Required(ErrorMessage = "Please Choose Property Type")]
        public int PropertyTypeId { get; set; }

        public List<SelectItem> PropertyTypes { get; set; }

        [Required(ErrorMessage = "Please Enter Street")]
        [StringLength(100, ErrorMessage = "Must less than 100 characters")]
        public string StreetAddress1 { set; get; }

        [StringLength(100, ErrorMessage = "Must less than 100 characters")]
        public string StreetAddress2 { set; get; }


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