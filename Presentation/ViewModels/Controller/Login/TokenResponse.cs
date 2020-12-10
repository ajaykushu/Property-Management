using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class TokenResponse
    {
        public string Token { set; get; }
        public long UId { set; get; }
        public HashSet<string> Roles { set; get; }
        public string FullName { get; set; }
        public HashSet<string> MenuItems { set; get; }
        public string PhotoPath { set; get; }
        public bool IsEffortVisible { get; set; }
    }
}