using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorServices.Abstractions
{
    public interface IInternetChecker
    {
        Task<bool> IsInternetAvailable(int timeout = 2000);
    }
}
