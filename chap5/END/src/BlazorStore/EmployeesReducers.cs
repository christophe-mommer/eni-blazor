using BlazorStore.Actions;
using Fluxor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorStore
{
    public static class EmployeesReducers
    {
        [ReducerMethod]
        public static EmployeeState ReduceEmployeeLoaded(EmployeeState state, LoadEmployees action)
            => state with { ErrorCode = "" };

        [ReducerMethod]
        public static EmployeeState ReduceEmployeeLoaded(EmployeeState state, EmployeesLoaded action)
            => state with { Employees = action.Employees.ToArray(), ErrorCode = "" };

        [ReducerMethod]
        public static EmployeeState ReducerEmployeeLoadingFaield(EmployeeState state, EmployeesLoadingFailed action)
            => state with { ErrorCode = action.ErrorCode };
    }
}
