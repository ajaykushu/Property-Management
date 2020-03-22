using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class FeatureRole
    {
        public SelectItem Role { get; set; }
        public List<SelectItem> Feature { get; set; }

    }
}
