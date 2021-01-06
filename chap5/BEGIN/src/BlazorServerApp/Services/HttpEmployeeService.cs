using BlazorAppShared.Models;
using BlazorServerApp.Services.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorServerApp.Services
{
    public class HttpEmployeeService : IEmployeeService
    {
        private readonly HttpClient client;

        public HttpEmployeeService(
            HttpClient client)
        {
            this.client = client;
        }

        public Task AddOrUpdate(Employee employee)
        {
            if(employee.Id != Guid.Empty)
            {
                return client.PutAsJsonAsync($"api/employees/{employee.Id}", employee);
            }
            else
            {
                return client.PostAsJsonAsync("api/employees", employee);
            }
        }

        public Task Delete(Employee employee) => client.DeleteAsync($"api/employees/{employee.Id}");

        public async IAsyncEnumerable<Employee> GetAll()
        {
            var response = await client.GetAsync("api/employees");
            if (response.IsSuccessStatusCode)
            {
                var employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(await response.Content.ReadAsStringAsync());
                foreach (var e in employees)
                {
                    yield return e;
                }
            }
        }
    }
}
