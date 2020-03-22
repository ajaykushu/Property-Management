using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels
{
    public class AddProperty
    {

        public long Id { get; set; }
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Please Keep length less than 5-50 charcters")]
        [Required]
        public string PropertyName { get; set; }
        [Required(ErrorMessage = "Please Choose Property Type")]
        public int PropertyTypeId { get; set; }
        public List<SelectItem> PropertyTypes { get; set; }
        [Display(ShortName = "Property Number")]
        [Required(ErrorMessage = "Please Enter Property Number")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Please Keep length less than 1-10 charcters")]
        public string HouseNumber { set; get; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Please Keep length less than 0-50 charcters")]
        public string Locality { set; get; }
        [Required(ErrorMessage = "Please Enter Street")]
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Please Keep length less than 0-50 charcters")]
        public string Street { set; get; }
        [StringLength(100, MinimumLength = 0, ErrorMessage = "Please Keep length less than 0-100 charcters")]
        public string LandMark { set; get; }
        [Required(ErrorMessage = "Please Enter PinCode")]
        [DataType(DataType.PostalCode,ErrorMessage ="Please Enter Valid Zip Code")]
        public string PinCode { set; get; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Please Keep length less than 0-50 charcters")]
        [Required(ErrorMessage = "Please Enter City")]
        public string City { set; get; }
        [StringLength(50, MinimumLength = 0, ErrorMessage = "Please Keep length less than 0-50 charcters")]
        [Required(ErrorMessage = "Please Enter Country")]
        public string Country { set; get; }
    }
}
