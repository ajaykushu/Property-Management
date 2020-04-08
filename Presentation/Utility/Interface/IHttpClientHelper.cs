using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Presentation.Utility.Interface
{
    public interface IHttpClientHelper
    {
        Task<HttpResponseMessage> GetDataAsync(String urlstring, Controller controller, string token = null);

        Task<HttpResponseMessage> PostDataAsync(string urlstring, object data, Controller controller, string token = null);

        Task<HttpResponseMessage> PostFileDataAsync(string urlstring, object payload, Controller controller, string token = null);

        void RemoveHeader();

        //void SendByte(object data, IFormFile file);
    }
}