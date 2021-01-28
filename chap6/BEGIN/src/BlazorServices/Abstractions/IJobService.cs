using BlazorAppShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorServices.Abstractions
{
    public interface IJobService
    {
        IAsyncEnumerable<Job> GetAll();
    }
}
