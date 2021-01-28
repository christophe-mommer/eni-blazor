using BlazorAppShared.Models;
using BlazorStore.Actions;
using Fluxor;
using System.Collections.Generic;
using System.Linq;

namespace BlazorStore
{
    public static class EmployeesReducers
    {
        [ReducerMethod]
        public static EmployeeState ReduceLoadEmployees(EmployeeState state, LoadEmployees action)
            => state with { ErrorCode = "" };

        [ReducerMethod]
        public static EmployeeState ReduceEmployeeLoaded(EmployeeState state, EmployeesLoaded action)
            => state with { Employees = action.Employees.ToArray(), ErrorCode = "" };

        [ReducerMethod]
        public static EmployeeState ReducerEmployeeLoadingFailed(EmployeeState state, EmployeesLoadingFailed action)
            => state with { ErrorCode = action.ErrorCode };

        [ReducerMethod]
        public static EmployeeState ReduceEmployeeSaved(EmployeeState state, EmployeeSaved action)
        {
            var employees = state.Employees?.ToList() ?? new List<Employee>();
            var existingEmployee = employees.Find(e => e.Id == action.Employee.Id);
            if (existingEmployee != null)
            {
                employees.Remove(existingEmployee);
            }
            employees.Add(action.Employee);
            return state with { Employees = employees, LastSaveOnApi = action.RemoteSaved };
        }
    }
}
