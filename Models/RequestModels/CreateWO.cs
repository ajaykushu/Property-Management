using System;
using System.Collections.Generic;
using System.Text;

namespace Models.RequestModels
{
    public class CreateWO
    {
        public string Location { get; set; }
        public string Area { get; set; }
        public int Issue { get; set; }
        public List<SelectItem> Issues { get; set; }
        public int Item { get; set; }
        public List<SelectItem> Items { get; set; }
        public string Description { get; set; }
        public long Property { get; set; }
        public List<SelectItem> Properties { get; set; }
    }
}
