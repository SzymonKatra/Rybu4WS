using Rybu4WS.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.StateMachine
{
    public class BaseAction<T>
    {
        public T Source { get; set; }

        public T Target { get; set; }

        public string ReceiveMessage { get; set; } // ?

        public string SendMessageServer { get; set; }

        public string SendMessage { get; set; } // !

        public bool IsSendingMessage() => SendMessageServer != null && SendMessage != null;

        public TimedDelay Delay { get; set; }
    }

    public class Action : BaseAction<State>
    {
        public StatementStateMutation Mutation { get; set; }

        public ICondition Condition { get; set; }
    }
}
