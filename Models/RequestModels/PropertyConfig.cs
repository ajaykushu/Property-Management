using System.Collections.Generic;

namespace Models.RequestModels
{
    public class PropertyConfig
    {
        public long PropertyId { get; set; }
        public List<SelectItem> Locations { set; get; }
        public int LocationId { get; set; }
        public string NewLocation { get; set; }
        public string SubLocation { set; get; }
    }
}