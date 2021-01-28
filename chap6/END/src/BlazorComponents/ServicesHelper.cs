using BlazorServices;
using BlazorServices.Abstractions;
using BlazorStore;
using Fluxor;
using MatBlazor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace BlazorComponents
{
    public static class ServicesHelper
    {
        public static IServiceCollection RegisterAppServices(this IServiceCollection services)
        {
            IAsyncPolicy<HttpResponseMessage> GetPolicy()
            {
                return HttpPolicyExtensions
                           .HandleTransientHttpError()
                           .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(1 + (1 * retryAttempt)));
            };

            void ConfigureClient(IServiceProvider sp, HttpClient client)
            {
                var options = sp.GetRequiredService<ApiOptions>();
                client.BaseAddress = new Uri(options.Url);
            }
            //services.AddSingleton<IEmployeeService, InMemoryEmployeeService>();
            services.AddHttpClient<IEmployeeService, HttpEmployeeService>(ConfigureClient).AddPolicyHandler(GetPolicy());
            services.AddHttpClient<IJobService, HttpJobService>(ConfigureClient).AddPolicyHandler(GetPolicy());
            services.AddHttpClient<ICountryService, HttpCountryService>(ConfigureClient).AddPolicyHandler(GetPolicy());

            services.AddSingleton(provider =>
            {
                var config = provider.GetService<IConfiguration>();
                return config.GetSection("Api").Get<ApiOptions>();
            });

            services.AddScoped<IInternetChecker, JavaScriptInternetChecker>();
            services.AddScoped<ActionsService>();
            services.AddSingleton<ActionsBuffer>();

            services.AddMatBlazor(); 
            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });

            services.AddFluxor(opts =>
            {
                opts.ScanAssemblies(typeof(EmployeeState).Assembly);
                opts.UseReduxDevTools();
            });

            return services;
        }
    }
}
