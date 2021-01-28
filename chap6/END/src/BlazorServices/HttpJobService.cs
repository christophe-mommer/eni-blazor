using BlazorAppShared.Models;
using BlazorServices.Abstractions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorServices
{
    public class HttpJobService : ServiceBase<Job>, IJobService
    {
        public HttpJobService(
            HttpClient client)
            : base(client)
        {
        }
    }
}
