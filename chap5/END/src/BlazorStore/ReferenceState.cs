using BlazorAppShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorStore
{
    public record ReferenceState(
        IEnumerable<Country>? Countries = null,
        IEnumerable<Job>? Jobs = null,
        string ErrorCode = "");
}
