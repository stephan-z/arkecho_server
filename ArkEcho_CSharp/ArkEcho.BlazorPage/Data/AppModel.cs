using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkEcho.BlazorPage
{
    public class AppModel
    {
        private int counter;
        public int Counter { get { return counter; } set { counter = value; CounterChanged?.Invoke(); } }

        public event Action CounterChanged;
        public AppModel()
        {
            Counter = 0;
        }
    }
}
