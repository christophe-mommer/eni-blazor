using BlazorAppShared.Models;
using BlazorServices.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorServices
{
    public class HttpEmployeeService : ServiceBase<Employee>, IEmployeeService
    {
        public HttpEmployeeService(
            HttpClient client)
            : base(client)
        {
        }

        public Task AddOrUpdate(Employee employee)
        {
            if (employee.Id != Guid.Empty)
            {
                return client.PutAsJsonAsync($"api/employees/{employee.Id}", employee);
            }
            else
            {
                return client.PostAsJsonAsync("api/employees", employee);
            }
        }

        public Task Delete(Employee employee) => client.DeleteAsync($"api/employees/{employee.Id}");

    }
}
