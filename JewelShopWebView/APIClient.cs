using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace JewelShopWebView
{
    public static class APIClient
    {
        private static HttpClient client = new HttpClient();

        static APIClient()
        {

            client.BaseAddress = new Uri("http://localhost:20494/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
        }
        

        public static Task<HttpResponseMessage> GetRequest(string requestUrl)
        {
            return client.GetAsync(requestUrl);
        }

        public static Task<HttpResponseMessage> PostRequest<T>(string requestUrl, T model)
        {
            return client.PostAsJsonAsync(requestUrl, model);
        }

        public static T GetElement<T>(Task<HttpResponseMessage> response)
        {
            return response.Result.Content.ReadAsAsync<T>().Result;
        }

        public static string GetError(Task<HttpResponseMessage> response)
        {
            return response.Result.Content.ReadAsStringAsync().Result;
        }
    }
}
