using BlazorAppShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorStore
{
    public record EmployeeState(IEnumerable<Employee> Employees = null, string ErrorCode = "");
}
