using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.Utility.Interface;
using System;
using System.Collections.Generic;
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
        private readonly string _scheme;

        public HttpHelper(IHttpClientFactory httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient.CreateClient();
            _httpContextAccessor = httpContextAccessor;
            _scheme = _httpContextAccessor.HttpContext.Request.IsHttps ? "https://" : "http://";
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

        public void RemoveHeader()
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

        public async Task<HttpResponseMessage> PostFileDataAsync(string url, object data, Controller controller, string token = null)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            if (!string.IsNullOrWhiteSpace(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            MultipartFormDataContent multiContent = new MultipartFormDataContent();
            foreach (var item in data.GetType().GetProperties())
            {
                if (item.Name.Equals("File") && item.GetValue(data) != null)
                {
                    if (item.PropertyType.Name.Equals("IFormFile"))
                    {
                        var file = item.GetValue(data) as IFormFile;
                        BinaryReader reader = new BinaryReader(file.OpenReadStream());
                        var imagebytes = reader.ReadBytes((int)file.Length);

                        multiContent.Add(new ByteArrayContent(imagebytes), "File", file.FileName);
                    }
                    else
                    {
                        foreach (var items in item.GetValue(data) as List<IFormFile>)
                        {
                            BinaryReader reader = new BinaryReader(items.OpenReadStream());
                            var imagebytes = reader.ReadBytes((int)items.Length);
                            multiContent.Add(new ByteArrayContent(imagebytes), "File", items.FileName);
                        }
                    }
                }
                else if (item.PropertyType.Name.Equals("List`1") && item.GetValue(data) != null)
                {
                    foreach (var val in item.GetValue(data) as List<string>)
                        multiContent.Add(new StringContent(val), item.Name);
                }
                else if (item.GetValue(data) != null)
                    multiContent.Add(new StringContent(Convert.ToString(item.GetValue(data)), Encoding.UTF8, "application/json"), item.Name);
            }
            var res = await _httpClient.PostAsync(url, multiContent).ConfigureAwait(false);
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
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)message.StatusCode;
                _httpContextAccessor.HttpContext.Response.Redirect(_scheme + _httpContextAccessor.HttpContext.Request.Host.Value + "/Login/LogOut");
            }
            else if (message.StatusCode == HttpStatusCode.Forbidden)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)message.StatusCode;
                _httpContextAccessor.HttpContext.Response.Redirect(
                   _scheme + _httpContextAccessor.HttpContext.Request.Host.Value + "/Home/Forbidden");
            }
            else if (message.StatusCode == HttpStatusCode.InternalServerError)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = (int)message.StatusCode;
                controller.TempData["Error"] = "Some Error Occured Contact Administrator";
            }
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
        }

        public string SerializeToString(object toBeSerializedObj)
        {
            return JsonConvert.SerializeObject(toBeSerializedObj);
        }
    }
}