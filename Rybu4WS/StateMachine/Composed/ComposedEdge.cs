using System;
using System.Collections.Generic;
using System.Text;

namespace Rybu4WS.StateMachine.Composed
{
    public class ComposedEdge : BaseEdge<ComposedNode>
    {
        public int AgentIndex { get; set; }
    }
}
