using BlazorStore.Actions;
using Fluxor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorStore
{
    public static class ReferenceReducers
    {
        [ReducerMethod]
        public static ReferenceState ReduceLoadCountries(ReferenceState state, LoadCountries _)
             => state with { ErrorCode = "" };

        [ReducerMethod]
        public static ReferenceState ReduceLoadJobs(ReferenceState state, LoadJobs _)
            => state with { ErrorCode = "" };

        [ReducerMethod]
        public static ReferenceState ReduceCountriesLoaded(ReferenceState state, CountriesLoaded action)
            => state with { Countries = action.Countries.ToArray(), ErrorCode = "" };

        [ReducerMethod]
        public static ReferenceState ReduceCountriesLoadingFailed(ReferenceState state, CountriesLoadingFailed action)
            => state with { ErrorCode = action.ErrorCode };

        [ReducerMethod]
        public static ReferenceState ReduceJobsLoaded(ReferenceState state, JobsLoaded action)
            => state with { Jobs = action.Jobs.ToArray(), ErrorCode = "" };

        [ReducerMethod]
        public static ReferenceState ReduceJobsLoadingFailed(ReferenceState state, JobsLoadingFailed action)
            => state with { ErrorCode = action.ErrorCode };
    }
}
