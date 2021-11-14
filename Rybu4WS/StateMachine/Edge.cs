using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class Edge
    {
        public Node Source { get; set; }

        public Node Target { get; set; }

        public string ReceiveMessage { get; set; } // ?

        public string SendMessageServer { get; set; }

        public string SendMessage { get; set; } // !

        public bool IsSendingMessage() => SendMessageServer != null && SendMessage != null;
    }
}
