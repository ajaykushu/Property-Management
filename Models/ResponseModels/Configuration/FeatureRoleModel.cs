using Models;
using System.Collections.Generic;

namespace Presentation.ViewModels.Controller.Configuration
{
    public class FeatureRoleModel
    {
        public SelectItem Role { get; set; }
        public List<SelectItem> Feature { get; set; }
    }
}