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
    public class LoadEmployeesEffect : Effect<LoadEmployees>
    {
        private readonly IEmployeeService service;
        private readonly ILogger<LoadEmployeesEffect> logger;

        public LoadEmployeesEffect(
            IEmployeeService service,
            ILogger<LoadEmployeesEffect> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        protected override async Task HandleAsync(LoadEmployees action, IDispatcher dispatcher)
        {
            try
            {
                var employees = await service.GetAll().ToListAsync();
                if(employees != null)
                {
                    dispatcher.Dispatch(new EmployeesLoaded(employees));
                    return;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unable to load employees from service");
            }
            dispatcher.Dispatch(new EmployeesLoadingFailed("LoadingFailed"));
        }
    }
}
