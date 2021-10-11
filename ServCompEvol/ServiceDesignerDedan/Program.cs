using System;
using System.Collections.Generic;

namespace ServiceDesignerDedan
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new ServerDescriptor()
            {
                Name = "user",
                States = new List<string>() { "start", "end" },
                InitialState = "start",
                Services = new List<ServiceDescriptor>()
                {
                    new ServiceDescriptor()
                    {
                        Name = "begin",
                        Actions = new Dictionary<string, HandlerDescriptorBase>()
                        {
                            { 
                                "start", 
                                new HandlerDescriptorCall()
                                {
                                    CallServerName = "store",
                                    CallServiceName = "buy",
                                    ReturnValueHandlers = new Dictionary<string, HandlerDescriptorBase>()
                                    {
                                        { "ok", new HandlerDescriptorTermination() },
                                        { "fail", new HandlerDescriptorTermination() }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var store = new ServerDescriptor()
            {
                Name = "store",
                States = new List<string>() { "in-stock", "not-in-stock" },
                InitialState = "in-stock",
                Services = new List<ServiceDescriptor>()
                {
                    new ServiceDescriptor()
                    {
                        Name = "buy",
                        Actions = new Dictionary<string, HandlerDescriptorBase>()
                        {
                            { "in-stock", new HandlerDescriptorMutation() { PostState = "not-in-stock", ReturnValue = "ok" } },
                            { "not-in-stock", new HandlerDescriptorMutation() { PostState = "not-in-stock", ReturnValue = "fail" } }
                        }
                    }
                }
            };

            var system = new SystemDescriptor()
            {
                Name = "book-store",
                Servers = new List<ServerDescriptor>() { user, store },
                EntryPointCallServerName = "user",
                EntryPointCallServiceName = "begin"
            };

            system.Process();
            Console.WriteLine(system.ToDedan());
        }
    }
}
