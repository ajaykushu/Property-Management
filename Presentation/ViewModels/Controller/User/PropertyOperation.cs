using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class PropertyOperation
    {
        public long Id { get; set; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Must between than 5-50 characters")]
        [Required]
        public string PropertyName { get; set; }

        [Required(ErrorMessage = "Please Choose Property Type")]
        public int PropertyTypeId { get; set; }

        public List<SelectItem> PropertyTypes { get; set; }

        [Display(ShortName = "Property Number")]
        [Required(ErrorMessage = "Please Enter Property Number")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Must between than 1-10 characters")]
        public string HouseNumber { set; get; }

        [StringLength(50, MinimumLength = 0, ErrorMessage = "Must between than 0-50 characters")]
        public string Locality { set; get; }

        [Required(ErrorMessage = "Please Enter Street")]
        [StringLength(50, MinimumLength = 10, ErrorMessage = "Must between than 5-50 characters")]
        public string Street { set; get; }

        [StringLength(100, MinimumLength = 0, ErrorMessage = "Must between than 0-100 characters")]
        public string StreetLine2 { set; get; }

        [Required(ErrorMessage = "Please Enter PinCode")]
        [RegularExpression(@"^[1-9]{1}[0-9]{5,9}$", ErrorMessage = "Please Enter Valid Zip Code")]
        [StringLength(10, MinimumLength = 6, ErrorMessage = "Please Enter Valid Zip Code")]
        public string PinCode { set; get; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Must between than 5-50 characters")]
        [Required(ErrorMessage = "Please Enter City")]
        public string City { set; get; }

        [StringLength(50, MinimumLength = 5, ErrorMessage = "Must between than 4-50 characters")]
        [Required(ErrorMessage = "Please Enter Country")]
        public string Country { set; get; }
    }
}