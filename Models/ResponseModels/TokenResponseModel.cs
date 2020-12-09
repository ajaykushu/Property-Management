using System.Collections.Generic;

namespace Models.ResponseModels
{
    public class TokenResponseModel
    {
        public string Token { set; get; }
        public long UId { set; get; }
        public HashSet<string> Roles { set; get; }
        public string FullName { get; set; }
        public HashSet<string> MenuItems { set; get; }
        public bool  IsEffortVisible { get; set; }
        public string PhotoPath { set; get; }
    }
}