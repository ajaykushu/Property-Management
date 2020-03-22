using Models;
using Presentation.ViewModels.Controller.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces

{
    public interface IConfigService
    {
        Task<List<SelectItem>> GetRoles();
        Task<List<SelectItem>> GetFeatureRoles(long roleId);
        Task<bool> UpdateFeature(KeyValuePair<int, List<string>> valuePairs);
    }
}
