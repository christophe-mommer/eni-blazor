using BlazorAppShared.Models;
using BlazorStore;
using BlazorStore.Actions;
using Fluxor;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerApp.Components
{
    public class AddressBase : ComponentBase, IDisposable
    {
        protected List<Country> Countries = new List<Country>();

        [Inject]
        protected IState<ReferenceState> RefState { get; set; }

        [Inject]
        protected IDispatcher Dispatcher { get; set; }

        [Parameter]
        public AddressModel Value
        {
            get;
            set;
        }
        [Parameter]
        public EventCallback<AddressModel> ValueChanged { get; set; }

        protected int SelectedCountryId
        {
            get => Value.Country?.Id ?? -1;
            set
            {
                if (value != -1)
                {
                    Value.Country = Countries.Find(j => j.Id == value);
                }
            }
        }

        protected Task AddressUpdated()
        {
            return ValueChanged.InvokeAsync(Value);
        }

        protected override void OnInitialized()
        {
            Value ??= new AddressModel();
            if (RefState.Value.Countries != null)
            {
                Countries = RefState.Value.Countries.ToList();
            }
            else
            {
                RefState.StateChanged += RefState_StateChanged;
                Dispatcher.Dispatch(new LoadCountries());
            }
            base.OnInitialized();
        }

        private void RefState_StateChanged(object sender, ReferenceState e)
        {
            if (e.Countries != null)
            {
                Countries = e.Countries.ToList();
            }
        }

        public void Dispose()
        {
            try
            {
                RefState.StateChanged -= RefState_StateChanged;
            }
            catch { }
        }
    }
}
