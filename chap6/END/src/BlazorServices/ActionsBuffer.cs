using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorServices
{
    public class ActionsBuffer
    {
        private readonly Queue<object> actions = new Queue<object>();

        public void AddActionToBuffer(object action)
        {
            actions.Enqueue(action);
        }
        public IEnumerable<object> GetActions()
        {
            yield return actions.Dequeue();
        }
    }
}
