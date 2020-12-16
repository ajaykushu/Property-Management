﻿using System.Collections.Generic;

namespace Models.Property.RequestModels
{
    public class PropertyConfigDTO
    {
        public long PropertyId { get; set; }
        public List<SelectItem> Locations { set; get; }
        public int? LocationId { get; set; }
        public string NewLocation { get; set; }
        public string SubLocation { set; get; }
        public string Items { get; set; }
    }
}