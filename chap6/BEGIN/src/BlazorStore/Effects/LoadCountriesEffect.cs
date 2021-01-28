using BlazorServices.Abstractions;
using BlazorStore.Actions;
using Fluxor;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorStore.Effects
{
    public class LoadCountriesEffect : Effect<LoadCountries>
    {
        private readonly ICountryService service;
        private readonly ILogger<LoadJobsEffect> logger;

        public LoadCountriesEffect(
            ICountryService service,
            ILogger<LoadJobsEffect> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        protected override async Task HandleAsync(LoadCountries action, IDispatcher dispatcher)
        {
            try
            {
                var countries = await service.GetAll().ToListAsync();
                if (countries != null)
                {
                    dispatcher.Dispatch(new CountriesLoaded(countries));
                    return;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unable to load countries from service");
            }
            dispatcher.Dispatch(new CountriesLoadingFailed("LoadingFailed"));
        }
    }
}
