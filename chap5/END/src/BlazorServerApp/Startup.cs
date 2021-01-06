using BlazorServices;
using BlazorServices.Abstractions;
using BlazorStore;
using Fluxor;
using MatBlazor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;

namespace BlazorServerApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddOptions();
            services.Configure<ApiOptions>(Configuration.GetSection("Api"));

            void ConfigureClient(IServiceProvider sp, HttpClient client)
            {
                var options = sp.GetRequiredService<IOptions<ApiOptions>>();
                client.BaseAddress = new Uri(options.Value.Url);
            }
            //services.AddSingleton<IEmployeeService, InMemoryEmployeeService>();
            services.AddHttpClient<IEmployeeService, HttpEmployeeService>(ConfigureClient);
            services.AddHttpClient<IJobService, HttpJobService>(ConfigureClient);
            services.AddHttpClient<ICountryService, HttpCountryService>(ConfigureClient);

            services.AddMatBlazor();

            services.AddFluxor(opts =>
            {
                opts.ScanAssemblies(typeof(EmployeeState).Assembly);
                opts.UseReduxDevTools();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
