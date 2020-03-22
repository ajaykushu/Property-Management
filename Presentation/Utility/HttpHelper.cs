using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.Utility.Interface;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Utility
{
    public class HttpHelper : IHttpClientHelper
    {
        private readonly HttpClient _httpClient;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public HttpHelper(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient.CreateClient();
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<HttpResponseMessage> GetDataAsync(string url, Controller controller, string token = null)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var res = await _httpClient.GetAsync(url).ConfigureAwait(false);
            await SetTempData(res, controller);
            
            return res;
        }
        public  void RemoveHeader()
        {
            if (_httpClient.DefaultRequestHeaders.Contains("Authorization"))
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }

        public async Task<HttpResponseMessage> PostDataAsync(string url, object paramObj, Controller controller, string token = null)
        {
            HttpResponseMessage res = null;
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var jsonstring = SerializeToString(paramObj);
            using (StringContent content = new StringContent(jsonstring, Encoding.UTF8, "application/json"))
            {
                res = await _httpClient.PostAsync(url, content).ConfigureAwait(false);
            };
            await SetTempData(res, controller);
            return res;
        }

        public async Task<HttpResponseMessage> PostFileDataAsync(string url, string email, IFormFile paramObjs, Controller controller, string token = null)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            BinaryReader reader = new BinaryReader(paramObjs.OpenReadStream());
            var imagebytes = reader.ReadBytes((int)paramObjs.Length);
            multiContent.Add(new ByteArrayContent(imagebytes), "files", paramObjs.FileName);
            var res = await _httpClient.PostAsync(url + "?email=" + email, multiContent).ConfigureAwait(false);
            await SetTempData(res, controller);
            return res;
        }
        public async Task SetTempData(HttpResponseMessage message, Controller controller)
        {

            if (message.StatusCode == HttpStatusCode.BadRequest)
                controller.TempData["Error"] = await message.Content.ReadAsStringAsync();
            else if (message.StatusCode == HttpStatusCode.Unauthorized)
            {
                controller.TempData["Error"] = "Please Login";
                _httpContextAccessor.HttpContext.Session.Clear();
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)message.StatusCode;
                _httpContextAccessor.HttpContext.Response.Redirect("https://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/Login/LogOut");
            }
            else if (message.StatusCode == HttpStatusCode.Forbidden)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)message.StatusCode;
                _httpContextAccessor.HttpContext.Response.Redirect(
                    "https://" + _httpContextAccessor.HttpContext.Request.Host.Value + "/Home/Forbidden");

            }
            else if (message.StatusCode == HttpStatusCode.InternalServerError)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)message.StatusCode;
                controller.TempData["Error"] = "Some Error Occured Contact Administrator";
            }
           

        }

        public string SerializeToString(object toBeSerializedObj)
        {
            return JsonConvert.SerializeObject(toBeSerializedObj);
        }

    }
}
