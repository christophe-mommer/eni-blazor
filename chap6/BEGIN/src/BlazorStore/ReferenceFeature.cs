using Fluxor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorStore
{
    public class ReferenceFeature : Feature<ReferenceState>
    {
        public override string GetName()
            => "Reference";

        protected override ReferenceState GetInitialState()
            => new ReferenceState();
    }
}
