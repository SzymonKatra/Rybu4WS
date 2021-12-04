using Rybu4WS.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class BaseEdge<T>
    {
        public T Source { get; set; }

        public T Target { get; set; }

        public string ReceiveMessage { get; set; } // ?

        public string SendMessageServer { get; set; }

        public string SendMessage { get; set; } // !

        public bool IsSendingMessage() => SendMessageServer != null && SendMessage != null;
    }

    public class Edge : BaseEdge<Node>
    {
        public BaseStatement StatementReference { get; set; }

        public ICondition Condition { get; set; }
    }
}
