using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataEntity
{
    public class HolidayLookup:Log
    {
        [Key]
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public bool Optional { get; set; }
    }
}
