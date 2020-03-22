using System.Collections.Generic;

namespace Models.ResponseModels
{
    public class TokenResponseModel
    {
        public string Token { set; get; }
        public IList<string> Roles { set; get; }
        public string FullName { get; set; }
        public Dictionary<string, HashSet<string>> MenuItems { set; get; }
        public string PhotoPath { set; get; }
    }
}
