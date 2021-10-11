using BusinessLogic.Interfaces;
using DataAccessLayer.Interfaces;
using DataEntity;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class ConfigBL : IConfigBL
    {
        private readonly IRepo<ApplicationRole> _applicationRole;
        private readonly IRepo<Menu> _menu;
        private readonly IRepo<RoleMenuMap> _roleMenuMap;

        public ConfigBL(IRepo<ApplicationRole> applicationRole, IRepo<Menu> menu, IRepo<RoleMenuMap> roleMenuMap)
        {
            _applicationRole = applicationRole;
            _menu = menu;
            _roleMenuMap = roleMenuMap;
        }

        public async Task<List<SelectItem>> GetRoles()
        {
            var roles = await _applicationRole.GetAll().Select(x => new SelectItem { Id = x.Id, PropertyName = x.Name }).AsNoTracking().ToListAsync();
            return roles;
        }

        public async Task<List<SelectItem>> GetFeatureRoles(long roleId)
        {
            var menus = await _menu.GetAll().AsNoTracking().ToListAsync();
            var rolemenumap = await _roleMenuMap.GetAll().Include(x => x.Role).Include(x => x.Menu).Where(x => x.RoleId == roleId).AsNoTracking().Select(x => x.MenuId).ToListAsync();
            //create mapping
            var ret = menus.Select(x => new SelectItem
            {
                Id = x.Id,
                PropertyName = x.MenuName,
                Selected = rolemenumap.Contains(x.Id) ? "true" : "false"
            }).ToList();
            return ret;
        }

        public async Task<bool> UpdateFeature(KeyValuePair<int, List<string>> valuePairs)
        {
            var role = await _applicationRole.GetAll().Include(x => x.RoleMenuMaps).Where(x => x.Id == valuePairs.Key).FirstOrDefaultAsync();
            if (role != null)
            {
                if (role.RoleMenuMaps == null)
                    role.RoleMenuMaps = new HashSet<RoleMenuMap>();
                role.RoleMenuMaps.Clear();
                foreach (var item in valuePairs.Value)
                {
                    if (long.TryParse(item, out long val))
                    {
                        RoleMenuMap roleMenuMap = new RoleMenuMap
                        {
                            MenuId = val
                        };
                        role.RoleMenuMaps.Add(roleMenuMap);
                    }
                }
                var row = await _applicationRole.Update(role);
                if (row > 0)
                    return true;
            }
            return false;
        }
    }
}