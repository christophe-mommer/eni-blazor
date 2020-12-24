using BlazorAppShared.Models;
using BlazorServerApp.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Pages
{
    public class EmployeesListBase : ComponentBase
    {
        [Inject]
        public IEmployeeService EmployeeService { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        protected List<Employee> Employees { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Employees = await EmployeeService.GetAll().ToListAsync();
            await base.OnInitializedAsync();
        }

        protected void CreateEmployee()
        {
            NavigationManager.NavigateTo("/employe/" + Guid.Empty);
        }
    }
}
