using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ViewModels.Controller.Configuration
{
    public class FeatureRoleModel
    {
        public  SelectItem Role { get; set; }
        public List<SelectItem> Feature { get; set; }
    }
}
