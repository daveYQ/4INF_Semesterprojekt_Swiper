using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace SwiperModeration.Services
{
    internal class APIService
    {
        public string URL { get; } = "https://localhost:7281";
        public UserModDTO CurrentUser { get; set; }

        public APIService() { }

        public async Task<List<UserModDTO>?> GetUsersAsync()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            var res = await client.GetFromJsonAsync<List<UserModDTO>>("/User");

            return res;
        }

        public async Task<UserModDTO?> LoginAsync(string email, string password, bool rememberMe)
        {
            HttpClient client = new HttpClient();

            var uriBuilder = new UriBuilder(URL + "/User/LogIn");

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["email"] = email;
            query["password"] = password;
            query["rememberMe"] = rememberMe + "";
            uriBuilder.Query = query.ToString();

            var res = await client.GetFromJsonAsync<UserModDTO>(uriBuilder.Uri.AbsoluteUri);

            return res;
        }

        public async Task<UserModDTO?> BlockUserAsync(string id)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            var res = await client.GetFromJsonAsync<UserModDTO>("/User/Block/" + id);

            return res;
        }
    }
}
