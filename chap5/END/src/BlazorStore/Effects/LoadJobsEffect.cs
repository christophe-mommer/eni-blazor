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
    public class LoadJobsEffect : Effect<LoadJobs>
    {
        private readonly IJobService service;
        private readonly ILogger<LoadJobsEffect> logger;

        public LoadJobsEffect(
            IJobService service,
            ILogger<LoadJobsEffect> logger)
        {
            this.service = service;
            this.logger = logger;
        }

        protected override async Task HandleAsync(LoadJobs action, IDispatcher dispatcher)
        {
            try
            {
                var jobs = await service.GetAll().ToListAsync();
                if (jobs != null)
                {
                    dispatcher.Dispatch(new JobsLoaded(jobs));
                    return;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unable to load jobs from service");
            }
            dispatcher.Dispatch(new JobsLoadingFailed("LoadingFailed"));
        }
    }
}
