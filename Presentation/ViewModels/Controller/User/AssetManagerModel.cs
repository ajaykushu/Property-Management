using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class AssetManagerModel
    {
        public List<SelectItem> Assets { get; set; }
        public long? AssetId { get; set; }
        public string Issues { get; set; }
        public string NewAsset { get; set; }
    }
}
