using BlazorAppShared.Models;
using BlazorStore;
using BlazorStore.Actions;
using Fluxor;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorComponents
{
    public class EmployeeDetailsBase : ComponentBase, IDisposable
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }
        private Task<IJSObjectReference> _module;
        private Task<IJSObjectReference> Module =>
            _module ??= JSRuntime.InvokeAsync<IJSObjectReference>("import", "./js/map.js").AsTask();

        [Inject]
        protected NavigationManager Navigation { get; set; }
        [Inject]
        protected IDispatcher Dispatcher { get; set; }

        [Inject]
        protected IState<EmployeeState> State { get; set; }
        [Inject]
        protected IState<ReferenceState> RefState { get; set; }
        [Inject]
        private IMatToaster Toaster { get; set; }

        [Parameter]
        public Guid Id { get; set; }

        

        protected Employee Employee { get; set; }
        private IEnumerable<Job> jobs { get; set; }
        protected List<Job> Jobs
        {
            get => jobs?.ToList() ?? new List<Job>();
            set
            {
                jobs = value;
                if (value != null)
                {
                    SelectedJobId = SelectedJobId; // To trigger Employee.Job affectation
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
                    Employee.Job = Jobs?.Find(j => j.Id == value);
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

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await CheckAddressAndDisplayMap();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnInitializedAsync()
        {
            State.StateChanged += State_StateChanged;
            RefState.StateChanged += RefState_StateChanged;
            if (Id == Guid.Empty)
            {
                Employee = new Employee
                {
                    Address = new AddressModel()
                };
            }
            else
            {
                if (State.Value.Employees != null)
                {
                    Employee = State.Value.Employees.FirstOrDefault(e => e.Id == Id);
                }
                else
                {
                    Dispatcher.Dispatch(new LoadEmployees());
                }
            }
            if (RefState.Value.Jobs == null)
            {
                Dispatcher.Dispatch(new LoadJobs());
            }
            else
            {
                Jobs = RefState.Value.Jobs.ToList();
            }
            await base.OnInitializedAsync();
        }

        private void RefState_StateChanged(object sender, ReferenceState e)
        {
            if (e.Jobs != null)
            {
                Jobs = RefState.Value.Jobs.ToList();
            }
            _ = InvokeAsync(StateHasChanged);
        }

        private void State_StateChanged(object sender, EmployeeState e)
        {
            if (e.Employees != null)
            {
                Employee = e.Employees.FirstOrDefault(e => e.Id == Id);
            }
            if (!e.LastSaveOnApi)
            {
                Toaster.Add("Les données ont été sauvegardées dans une zone temporaire et seront envoyées vers le serveur dès lors qu'une connexion internet sera disponible", MatToastType.Warning);
            }
            else if (e.LastSaveOnApi)
            {
                Toaster.Add("Les données ont bien été sauveardées", MatToastType.Success);
            }
            _ = InvokeAsync(StateHasChanged);
        }

        protected void Save() => Dispatcher.Dispatch(new SaveEmployeeAction(Employee));

        public void Dispose()
        {
            try
            {
                State.StateChanged -= State_StateChanged;
                RefState.StateChanged -= RefState_StateChanged;
            }
            catch { }
        }
    }
}
