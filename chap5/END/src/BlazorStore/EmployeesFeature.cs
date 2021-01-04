using Fluxor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorStore
{
    public class EmployeesFeature : Feature<EmployeeState>
    {
        public override string GetName()
            => "Employees";

        protected override EmployeeState GetInitialState()
            => new EmployeeState(null);
    }
}
