using BlazorAppShared.Models;
using BlazorServerApp.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Pages
{
    public class EmployeesListBase : ComponentBase
    {
        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        protected List<Employee> Employees { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Employees = await EmployeeService.GetAll().ToListAsync();
            await base.OnInitializedAsync();
        }
    }
}
