using BlazorServices;
using BlazorServices.Abstractions;
using BlazorStore.Actions;
using Fluxor;
using System.Threading.Tasks;

namespace BlazorStore.Effects
{
    public class SaveEmployeeEffect : Effect<SaveEmployeeAction>
    {
        private readonly IEmployeeService employeeService;
        private readonly IInternetChecker internetChecker;
        private readonly ActionsBuffer buffer;

        public SaveEmployeeEffect(
            IEmployeeService employeeService,
            IInternetChecker internetChecker,
            ActionsBuffer buffer)
        {
            this.employeeService = employeeService;
            this.internetChecker = internetChecker;
            this.buffer = buffer;
        }

        protected override async Task HandleAsync(SaveEmployeeAction action, IDispatcher dispatcher)
        {
            bool sendToApi = false;
            if (await internetChecker.IsInternetAvailable())
            {
                sendToApi = true;
                await employeeService.AddOrUpdate(action.Employee);
            }
            else
            {
                buffer.AddActionToBuffer(action);
            }
            dispatcher.Dispatch(new EmployeeSaved(action.Employee, sendToApi));
        }
    }
}
