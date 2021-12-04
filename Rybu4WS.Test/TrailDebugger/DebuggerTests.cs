using FluentAssertions;
using Rybu4WS.TrailDebugger;
using Rybu4WS.TrailDebugger.TrailSchema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Rybu4WS.Test.TrailDebugger
{
    public class DebuggerTests
    {
        public DebuggerTests()
        {
        }

        [Fact]
        public void Bank()
        {
            var input = TestUtils.ReadResource("bank_termination_trail.xml");
            var result = Rybu4WS.TrailDebugger.Debugger.Parse(input);

            result.ServerVars.Should().HaveCount(5);
            result.ServerVars.Should().ContainSingle(x => x.ServerTag == 0 && x.ServerVarName == "Bank" && x.ServerVarState == "balanceA_1_balanceB_0");
            result.ServerVars.Should().ContainSingle(x => x.ServerTag == 1 && x.ServerVarName == "Atm" && x.ServerVarState == "state_None");
            result.ServerVars.Should().ContainSingle(x => x.ServerTag == 2 && x.ServerVarName == "BankingApp" && x.ServerVarState == "NONE");
            result.ServerVars.Should().ContainSingle(x => x.ServerTag == 3 && x.ServerVarName == "First" && x.ServerVarState == "NONE");
            result.ServerVars.Should().ContainSingle(x => x.ServerTag == 4 && x.ServerVarName == "Second" && x.ServerVarState == "NONE");

            result.AgentVars.Should().HaveCount(2);
            result.AgentVars.Should().ContainSingle(x => x.AgentTag == 0 && x.AgentVarName == "AgentFirst" && x.AgentIniServer == "First" && x.AgentIniService == "START_FROM_INIT");
            result.AgentVars.Should().ContainSingle(x => x.AgentTag == 1 && x.AgentVarName == "AgentSecond" && x.AgentIniServer == "Second" && x.AgentIniService == "START_FROM_INIT");

            result.AgentList.Should().HaveCount(2);
            result.AgentList.Should().ContainSingle(x => x.AgentListTag == 0 && x.AgentListName == "AgentFirst");
            result.AgentList.Should().ContainSingle(x => x.AgentListTag == 1 && x.AgentListName == "AgentSecond");

            result.Configurations.Should().HaveCount(22);

            var conf2 = result.Configurations.Single(x => x.ConfNr == 2);
            conf2.States.Should().HaveCount(5);
            conf2.States.Should().ContainSingle(x => x.ServerTag == 0 && x.Server == "Bank" && x.Value == "balanceA_1_balanceB_0" && x.Active == true);
            conf2.States.Should().ContainSingle(x => x.ServerTag == 1 && x.Server == "Atm" && x.Value == "state_None" && x.Active == true);
            conf2.States.Should().ContainSingle(x => x.ServerTag == 2 && x.Server == "BankingApp" && x.Value == "NONE" && x.Active == true);
            conf2.States.Should().ContainSingle(x => x.ServerTag == 3 && x.Server == "First" && x.Value == "NONE_FROM_INIT_PRE_SL29SC4EL29EC19" && x.Active == true);
            conf2.States.Should().ContainSingle(x => x.ServerTag == 4 && x.Server == "Second" && x.Value == "NONE" && x.Active == true);
            conf2.Messages.Should().HaveCount(2);
            conf2.Messages.Should().ContainSingle(x => x.AgentTag == 0 && x.Agent == "AgentFirst" && x.ServerTag == 3 && x.Server == "First" && x.Service == "ENTER_PRE_SL29SC4EL29EC19_FROM_INIT" && x.Active == true);
            conf2.Messages.Should().ContainSingle(x => x.AgentTag == 1 && x.Agent == "AgentSecond" && x.ServerTag == 4 && x.Server == "Second" && x.Service == "START_FROM_INIT" && x.Active == true);
            conf2.Actions.Should().HaveCount(2);
            conf2.Actions.Should().ContainSingle(x => x.ActionTag == 64 && x.AgentTag == 0 && x.ServerTag == 3 && x.State == "NONE_FROM_INIT_PRE_SL29SC4EL29EC19" && x.Service == "ENTER_PRE_SL29SC4EL29EC19_FROM_INIT" && x.NextServerTag == 1);
            conf2.Actions.Should().ContainSingle(x => x.ActionTag == 73 && x.AgentTag == 1 && x.ServerTag == 4 && x.State == "NONE" && x.Service == "START_FROM_INIT" && x.NextServerTag == 4);
        }
    }
}
