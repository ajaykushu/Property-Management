using System;
using System.Collections.Generic;

namespace Presentation.ConstModal
{
    public class MenuMapperModel
    {
        public Dictionary<string, MenuProperty> Menus { get; set; }
    }

    [Serializable]
    public class MenuProperty
    {
        public string SubMenuName { get; set; }
        public string Action { get; set; }
        public string Controller { set; get; }
        public string Class { get; set; }
        public bool Enabled { get; set; }
        public string MainMenuName { get; set; }
    }
}