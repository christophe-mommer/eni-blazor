using BlazorAppShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Services.Abstractions
{
    public interface IEmployeeService
    {
        IAsyncEnumerable<Employee> GetAll();
        Task AddOrUpdate(Employee employee);
        Task Delete(Employee employee);
    }
}
