using BlazorServices;
using Fluxor;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorStore
{
    public class ActionsService
    {
        private readonly IJSRuntime runtime;
        private readonly IDispatcher dispatcher;
        private readonly ActionsBuffer buffer;

        public ActionsService(
            IJSRuntime runtime,
            IDispatcher dispatcher,
            ActionsBuffer buffer)
        {
            this.runtime = runtime;
            this.dispatcher = dispatcher;
            this.buffer = buffer;
        }

        public Task RegisterServiceToListenJS()
        {
            var thisRef = DotNetObjectReference.Create(this);
            return runtime.InvokeVoidAsync("storeActionService", thisRef).AsTask();
        }

        [JSInvokable]
        public void SetOnline(bool online)
        {
            if (online)
            {
                foreach (var action in buffer.GetActions())
                {
                    dispatcher.Dispatch(action);
                }
            }
        }
    }
}
