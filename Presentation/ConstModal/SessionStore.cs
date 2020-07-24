using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.ConstModal
{
    [Serializable]
    public class SessionStore
    {
        public string Token { get; set; }
        public HashSet<string> MenuList { get; set; }
        public Dictionary<string,List<MenuProperty>> MenuView { get; set; }
    }
}
