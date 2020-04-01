using System.Collections.Generic;

namespace Presentation.ViewModels
{
    public class RouteConstModel
    {
        public string ApplicationBaseUrl { get; set; }
        public Dictionary<string, string> Routes { get; set; }
    }
}