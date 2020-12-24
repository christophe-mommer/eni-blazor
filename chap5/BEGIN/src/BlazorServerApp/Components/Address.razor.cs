using BlazorAppShared.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServerApp.Components
{
    public class AddressBase : ComponentBase
    {
        protected List<Country> Countries = new List<Country>
        {
            new Country{ Id = 1, Name = "France" },
            new Country{ Id = 2, Name = "Belgique" },
            new Country{ Id = 3, Name = "Suisse" },
            new Country{ Id = 4, Name = "Canada" },
            new Country{ Id = 5, Name = "Autre" }
        };

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
            base.OnInitialized();
        }
    }
}
