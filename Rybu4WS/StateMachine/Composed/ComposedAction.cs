using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.StateMachine.Composed
{
    public class ComposedAction : BaseAction<ComposedState>
    {
        public int AgentIndex { get; set; }
    }
}
