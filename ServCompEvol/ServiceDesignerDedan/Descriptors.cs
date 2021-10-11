using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceDesignerDedan
{
    public class CallEntry
    {
        public string CallerServerName { get; set; }

        public HandlerDescriptorCall CallDescriptor { get; set; }

        public string CallServiceName() => $"{CallerServerName}_CALL_{CallDescriptor.CallServiceName}";

        public string PerformServiceName(string stateName) => $"{CallerServerName}_PERFORM_{CallDescriptor.CallServiceName}_{stateName}";

        public string ReturnServiceName(string returnValue) => $"RETURN_{CallDescriptor.CallServerName}_{CallDescriptor.CallServiceName}_{returnValue}";

        public IEnumerable<string> FindAllReturnValues(SystemDescriptor system)
        {
            foreach (var server in system.Servers)
            {
                foreach (var service in server.Services)
                {
                    if (server.Name == CallDescriptor.CallServerName && service.Name == CallDescriptor.CallServiceName)
                    {
                        return service.GENReturnValues;
                    }
                }
            }

            return Enumerable.Empty<string>();
        }
    }


    public class SystemDescriptor
    {
        public string Name { get; set; }

        public List<ServerDescriptor> Servers { get; set; }

        public string EntryPointCallServerName { get; set; }

        public string EntryPointCallServiceName { get; set; }

        public List<CallEntry> GENCallsList { get; set; }

        public void Process()
        {
            GENCallsList = new List<CallEntry>();
            foreach (var server in Servers)
            {
                server.Process(this);
            }
        }
    }

    public class ServerDescriptor
    {
        public string Name { get; set; }

        public List<string> States { get; set; }

        public string InitialState { get; set; }

        public List<ServiceDescriptor> Services { get; set; }

        public void Process(SystemDescriptor system)
        {
            foreach (var service in Services)
            {
                service.Process(this, system);
            }
        }
    }

    public class ServiceDescriptor
    {
        public string Name { get; set; }

        public Dictionary<string, HandlerDescriptorBase> Actions { get; set; } // key = pre state

        public IReadOnlyList<string> GENReturnValues; 

        public void Process(ServerDescriptor server, SystemDescriptor system)
        {
            var returnVals = new List<string>();

            foreach (var action in Actions)
            {
                var handler = action.Value;
                if (handler is HandlerDescriptorMutation)
                {
                    var handlerMutation = (HandlerDescriptorMutation)handler;
                    returnVals.Add(handlerMutation.ReturnValue);
                }
                else if (handler is HandlerDescriptorCall)
                {
                    var handlerCall = (HandlerDescriptorCall)handler;
                    system.GENCallsList.Add(new CallEntry()
                    {
                        CallerServerName = server.Name,
                        CallDescriptor = handlerCall
                    });
                }
            }

            GENReturnValues = returnVals.Distinct().ToList();
        }
    }

    public class HandlerDescriptorBase
    {
    }

    public class HandlerDescriptorMutation : HandlerDescriptorBase
    {
        public string PostState { get; set; }

        public string ReturnValue { get; set; }
    }

    public class HandlerDescriptorCall : HandlerDescriptorBase
    {
        public string CallServerName { get; set; }

        public string CallServiceName { get; set; }

        public Dictionary<string, HandlerDescriptorBase> ReturnValueHandlers { get; set; }
    }

    public class HandlerDescriptorTermination : HandlerDescriptorBase
    {

    }
}
