using System;
using System.Collections.Generic;


namespace Presentation.ViewModels
{
    public class TokenResponse
    {
        public string Token { set; get; }
        public IList<String> Roles { set; get; }
        public string FullName { get; set; }
        public Dictionary<string, HashSet<string>> MenuItems { set; get; }
        public string PhotoPath { set; get; }


    }
}
