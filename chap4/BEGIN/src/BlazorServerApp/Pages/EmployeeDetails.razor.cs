using BlazorAppShared.Models;
using BlazorServerApp.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Pages
{
    public class EmployeeDetailsBase : ComponentBase
    {
        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        protected List<Country> Countries = new List<Country>
        {
            new Country{ Id = 1, Name = "France" },
            new Country{ Id = 2, Name = "Belgique" },
            new Country{ Id = 3, Name = "Suisse" },
            new Country{ Id = 4, Name = "Canada" },
            new Country{ Id = 5, Name = "Autre" }
        };
        protected List<Job> Jobs = new List<Job>
        {
            new Job { Id = 1, Title = "Manager" },
            new Job { Id = 2, Title = "Journaliste" },
            new Job { Id = 3, Title = "Développeur" },
            new Job { Id = 4, Title = "Directeur" },
            new Job { Id = 5, Title = "Secrétaire" }
        };
        protected Employee Employee { get; set; }
        protected int? SelectedCountryId
        {
            get => Employee?.Country?.Id;
            set
            {
                if (value.HasValue)
                {
                    Employee.Country = Countries.Find(j => j.Id == value.Value);
                }
            }
        }
        protected int? SelectedJobId
        {
            get => Employee?.Job?.Id;
            set
            {
                if (value.HasValue)
                {
                    Employee.Job = Jobs.Find(j => j.Id == value.Value);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            Employee = await EmployeeService.GetAll().FirstOrDefaultAsync(e => e.Id == Id);
            await base.OnInitializedAsync();
        }
            
        protected Task Save()
        {
            return EmployeeService.AddOrUpdate(Employee);
        }
    }
}
