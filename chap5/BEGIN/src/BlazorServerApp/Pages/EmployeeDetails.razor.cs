using BlazorAppShared.Models;
using BlazorServerApp.Services.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorServerApp.Pages
{
    public class EmployeeDetailsBase : ComponentBase
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        private Task<IJSObjectReference> _module;
        private Task<IJSObjectReference> Module =>
            _module ??= JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/map.js").AsTask();

        [Inject]
        protected NavigationManager Navigation { get; set; }

        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        public IEmployeeService EmployeeService { get; set; }

        protected List<Job> Jobs = new List<Job>
        {
            new Job { Id = 1, Title = "Manager" },
            new Job { Id = 2, Title = "Journaliste" },
            new Job { Id = 3, Title = "Développeur" },
            new Job { Id = 4, Title = "Directeur" },
            new Job { Id = 5, Title = "Secrétaire" }
        };
        private Employee employee;
        protected Employee Employee
        {
            get => employee; 
            set
            {
                employee = value;
                if (value != null)
                {
                    _ = CheckAddressAndDisplayMap();
                }
            }
        }
        protected int SelectedJobId
        {
            get => Employee?.Job?.Id ?? -1;
            set
            {
                if (value != -1)
                {
                    Employee.Job = Jobs.Find(j => j.Id == value);
                }
            }
        }

        protected async Task AddressChanged(AddressModel newValue)
        {
            Employee.Address = newValue;
            await CheckAddressAndDisplayMap();
        }

        private async Task CheckAddressAndDisplayMap()
        {
            if (Employee != null
             && !string.IsNullOrWhiteSpace(Employee.Address?.Street)
             && !string.IsNullOrWhiteSpace(Employee.Address?.Zipcode)
             && !string.IsNullOrWhiteSpace(Employee.Address?.Country?.Name))
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:83.0) Gecko/20100101 Firefox/83.0");
                    var data = await httpClient.GetStringAsync($"https://nominatim.openstreetmap.org/search?street={Employee.Address.Street}&postalcode={Employee.Address.Zipcode}&country={Employee.Address.Country.Name}&format=json");
                    if (!string.IsNullOrWhiteSpace(data))
                    {
                        var jsonData = JArray.Parse(data);
                        var latitude = jsonData[0]["lat"];
                        var longitude = jsonData[0]["lon"];
                        var jsModule = await Module;
                        await jsModule.InvokeVoidAsync("displayMap", latitude.Value<string>(), longitude.Value<string>());
                    }
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (Id == Guid.Empty)
            {
                Employee = new Employee
                {
                    Address = new AddressModel()
                };
            }
            else
            {
                Employee = (await EmployeeService.GetAll().ToListAsync()).Find(e => e.Id == Id);
            }
            await base.OnInitializedAsync();
        }

        protected async Task Save()
        {
            await EmployeeService.AddOrUpdate(Employee);
            Navigation.NavigateTo("/employees");
        }
    }
}
