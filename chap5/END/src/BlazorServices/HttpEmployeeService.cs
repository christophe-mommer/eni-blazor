﻿using BlazorAppShared.Models;
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
            if (employee.Id != Guid.Empty)
            {
                return client.PutAsJsonAsync($"api/employee/{employee.Id}", employee);
            }
            else
            {
                return client.PostAsJsonAsync("api/employee", employee);
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
