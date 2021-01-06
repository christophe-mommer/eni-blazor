using BlazorAppShared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace BlazorServices
{
    public abstract class ServiceBase<T>
    {
        protected readonly HttpClient client;

        public ServiceBase(
            HttpClient client)
        {
            this.client = client;
        }

        public async IAsyncEnumerable<T> GetAll()
        {
            var response = await client.GetAsync(GetUri());
            if (response.IsSuccessStatusCode)
            {
                var data = JsonConvert.DeserializeObject<IEnumerable<T>>(await response.Content.ReadAsStringAsync());
                foreach (var e in data)
                {
                    yield return e;
                }
            }
        }

        private string GetUri()
            => typeof(T).Name switch
            {
                nameof(Job) => "api/jobs",
                nameof(Country) => "api/countries",
                _ => "api/employees"
            };
    }
}
