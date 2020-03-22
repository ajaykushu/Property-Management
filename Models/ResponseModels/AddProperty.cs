using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.ResponseModels
{
    public class AddProperty
    {
        [Required]
        public long Id { get; set; }
        public string PropertyName { get; set; }
        [Required]
        public int PropertyTypeId { get; set; }
        public List<SelectItem> PropertyTypes { get; set; }
        [Required]
        public string HouseNumber { set; get; }
        public string Locality { set; get; }
        [Required]
        public string Street { set; get; }

        public string LandMark { set; get; }
        [Required]
        [DataType(DataType.PostalCode)]
        public string PinCode { set; get; }
        [Required]
        public string City { set; get; }
        [Required]
        public string Country { set; get; }
    }
}
