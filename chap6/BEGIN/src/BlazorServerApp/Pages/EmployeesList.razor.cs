using BlazorAppShared.Models;
using BlazorServices.Abstractions;
using BlazorStore;
using BlazorStore.Actions;
using Fluxor;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Pages
{
    public class EmployeesListBase : ComponentBase, IDisposable
    {
        [Inject] protected IState<EmployeeState> State { get; set; }
        [Inject] protected IDispatcher Dispatcher { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }

        protected string ErrorMessage = "";

        protected override void OnInitialized()
        {
            State.StateChanged += State_StateChanged;
            if (State.Value.Employees == null)
            {
                Dispatcher.Dispatch(new LoadEmployees());
            }
            base.OnInitialized();
        }
        
        private void State_StateChanged(object sender, EmployeeState e)
        {
            if(e.ErrorCode == "LoadingFailed")
            {
                ErrorMessage = "Les employés n'ont pas pu être chargés.";
            }
            InvokeAsync(StateHasChanged);
        }

        protected void CreateEmployee()
        {
            NavigationManager.NavigateTo("/employe/" + Guid.Empty);
        }

        public void Dispose()
        {
            try
            {
                State.StateChanged -= State_StateChanged;
            }
            catch { }
        }
    }
}
