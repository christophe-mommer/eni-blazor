using BlazorAppShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorStore.Actions
{
    public record CountriesLoaded(IEnumerable<Country> Countries);
}
