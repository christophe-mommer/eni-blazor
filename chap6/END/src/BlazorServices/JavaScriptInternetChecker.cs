using BlazorServices.Abstractions;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorServices
{
    public class JavaScriptInternetChecker : IInternetChecker
    {
        private readonly IJSRuntime jsRuntime;

        public JavaScriptInternetChecker(
            IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public Task<bool> IsInternetAvailable(int timeout = 2000)
            => jsRuntime.InvokeAsync<bool>("isOnline", TimeSpan.FromMilliseconds(timeout)).AsTask();
    }
}
