using BlazorAppShared.Models;
using BlazorServices.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServices
{
    public class InMemoryEmployeeService : IEmployeeService
    {
        private readonly ConcurrentDictionary<Guid, Employee> employees = new ConcurrentDictionary<Guid, Employee>();

        public InMemoryEmployeeService()
        {
            var emp1Id = Guid.NewGuid();
            var emp2Id = Guid.NewGuid();
            employees.TryAdd(emp1Id, new Employee
            {
                Id = emp1Id,
                Name = "John",
                Lastname = "Doe",
                Job = new Job
                {
                    Id = 1,
                    Title = "Manager"
                },
                Address = new AddressModel
                {
                    Street = "370 route de Puechabon",
                    Zipcode = "34380",
                    Country = new Country { Id = 1, Name = "France" }
                }
            });
            employees.TryAdd(emp2Id, new Employee
            {
                Id = emp2Id,
                Name = "Jean",
                Lastname = "Valjean",
                Job = new Job
                {
                    Id = 2,
                    Title = "Journaliste"
                }
            });
        }

        public Task AddOrUpdate(Employee employee)
        {
            employees[employee.Id] = employee;
            return Task.CompletedTask;
        }

        public Task Delete(Employee employee)
        {
            if (employees.ContainsKey(employee.Id))
            {
                employees.TryRemove(employee.Id, out Employee _);
            }
            return Task.CompletedTask;
        }

        public IAsyncEnumerable<Employee> GetAll()
        {
            return employees.Values.ToAsyncEnumerable();
        }
    }

}
