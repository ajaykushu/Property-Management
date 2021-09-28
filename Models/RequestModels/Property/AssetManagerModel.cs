using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Property.RequestModels
{
    public class AssetManagerModel
    {
        
        public List<SelectItem> Property { get; set; }
        public int PropertyId { get; set; }
       
        public int LocationId { get; set; }
        public long? ItemId { get; set; }
        public string Issues { get; set; }
        public string NewAsset { get; set; }
    }
}
