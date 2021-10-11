using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceDesignerDedan
{
    public static class DedanConversion
    {
        public static string ToDedan(this SystemDescriptor system)
        {
            var sb = new StringBuilder();

            foreach (var server in system.Servers)
            {
                sb.AppendLine(server.ToDedan(system, system.GetOtherServerNames(server)));
            }

            sb.AppendLine();
            sb.AppendLine($"servers {string.Join(", ", system.Servers.Select(x => x.Name))};");
            sb.AppendLine($"agents A");
            sb.AppendLine();
            sb.AppendLine("init -> {");
            foreach (var server in system.Servers)
            {
                sb.AppendLine($"{Indent(1)}{server.Name}(A, {string.Join(", ", system.GetOtherServerNames(server))}).{server.InitialState}");
            }
            sb.AppendLine();
            sb.AppendLine($"{Indent(1)}A.{system.EntryPointCallServerName}.{system.EntryPointCallServiceName},");
            sb.AppendLine("}.");

            return sb.ToString();
        }

        public static string[] GetOtherServerNames(this SystemDescriptor system, ServerDescriptor serverExcept)
        {
            return system.Servers.Except(new[] { serverExcept }).Select(x => x.Name).ToArray();
        }

        public static string ToDedan(this ServerDescriptor server, SystemDescriptor system, string[] otherServerNames)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"server: {server.Name}(agents A; servers {string.Join(", ", otherServerNames)}),");

            var allCalleers = system.GENCallsList.Where(x => x.CallDescriptor.CallServerName == server.Name).ToList();
            var allCalled = system.GENCallsList.Where(x => x.CallerServerName == server.Name).ToList();

            sb.AppendLine("services {");
            foreach (var callEntry in allCalleers)
            {
                sb.AppendLine($"{Indent(1)}{callEntry.CallServiceName()},");
                foreach (var stateName in server.States)
                {
                    sb.AppendLine($"{Indent(1)}{callEntry.PerformServiceName(stateName)},");
                }
            }
            sb.AppendLine();
            foreach (var callEntry in allCalled)
            {
                foreach (var retVal in callEntry.FindAllReturnValues(system))
                {
                    sb.AppendLine($"{Indent(1)}{callEntry.ReturnServiceName(retVal)},");

                }
            }
            sb.AppendLine("},");


            var servicePendingStates = server.Services.Select(x => $"SERVING_{x.Name}").ToList(); ;

            sb.AppendLine("states {");
            foreach (var stateName in server.States.Concat(servicePendingStates))
            {
                sb.AppendLine($"{Indent(1)}{stateName},");
            }
            sb.AppendLine("},");

            sb.AppendLine("actions {");
            foreach (var service in server.Services)
            {
                var serviceCallers = allCalleers.Where(x => x.CallDescriptor.CallServiceName == service.Name);

                foreach (var action in service.Actions)
                {
                    var requiredState = action.Key;
                    var handler = action.Value;
                    if (handler is HandlerDescriptorMutation)
                    {
                        var handlerMutation = (HandlerDescriptorMutation)handler;
                        foreach (var callEntry in serviceCallers)
                        {
                            sb.AppendLine($"{Indent(1)}{{ A.{server.Name}.{callEntry.CallServiceName()}, {server.Name}.{requiredState} }} -> {{ A.{server.Name}.{callEntry.PerformServiceName(requiredState)}, {server.Name}.SERVING_{service.Name} }},");
                            sb.AppendLine($"{Indent(1)}{{ A.{server.Name}.{callEntry.PerformServiceName(requiredState)}, {server.Name}.SERVING_{service.Name} }} -> {{ A.{callEntry.CallerServerName}.{callEntry.ReturnServiceName(handlerMutation.ReturnValue)}, {server.Name}.{handlerMutation.PostState} }},");
                        }
                    }
                    else if (handler is HandlerDescriptorCall)
                    {
                        var handlerCall = (HandlerDescriptorCall)handler;
                        var callEntry = system.GENCallsList.Single(x => x.CallDescriptor == handlerCall);
                        sb.AppendLine($"{Indent(1)}{{ A.{server.Name}.{service.Name}, {server.Name}.{requiredState} }} -> {{ A.{handlerCall.CallServerName}.{callEntry.CallServiceName()}, {server.Name}.CALLING_{handlerCall.CallServerName}_{handlerCall.CallServiceName} }},");
                        foreach (var retsHandler in handlerCall.ReturnValueHandlers)
                        {
                            var retVal = retsHandler.Key;
                            sb.AppendLine($"{Indent(1)}{{ A.{server.Name}.{callEntry.ReturnServiceName(retsHandler.Key)}, {server.Name}.CALLING_{handlerCall.CallServerName}_{handlerCall.CallServiceName} }} -> {{ }}");
                        }
                    }
                }
            }
            sb.AppendLine("}");

            return sb.ToString();
        }

        private static string Indent(int level)
        {
            return new string(' ', level * 4);
        }
    }
}
