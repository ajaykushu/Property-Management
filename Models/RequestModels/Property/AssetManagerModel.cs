using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.Property.RequestModels
{
    public class AssetManagerModel
    {
        public List<SelectItem> Assets { get; set; }
        public long? AssetId { get; set; }
        public string Issues { get; set; }
        public string NewAsset { get; set; }
    }
}
