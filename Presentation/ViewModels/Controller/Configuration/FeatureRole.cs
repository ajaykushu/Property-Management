using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class FeatureRole
    {
        public SelectItem Role { get; set; }
        public List<SelectItem> Feature { get; set; }
    }
}