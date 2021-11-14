using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Antlr4.Runtime;
using Rybu4WS.StateMachine;
using System.IO;

namespace Rybu4WS.Test.IntegrationTests
{
    public class EndToEnd
    {
        [Fact]
        public void Bank()
        {
            var input = TestUtils.ReadResource("bank.txt");
            var stateMachine = ParseAndConvert(input);

            stateMachine.Graphs.Should().HaveCount(5);
            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "Bank");
            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "Atm");
            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "BankingApp");
            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "First");
            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "Second");

            var bankGraph = stateMachine.Graphs.Single(x => x.Name == "Bank");
            bankGraph.InitNode.ToString().Should().Be("balanceA_1_balanceB_0");
            bankGraph.Nodes.Should().HaveCount(15);
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_0");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_1");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_0");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_1");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_0_FROM_BankingApp_PRE_SL4SC50EL4EC63");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_0_FROM_BankingApp_PRE_SL4SC65EL4EC77");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_1_FROM_BankingApp_PRE_SL4SC79EL4EC89");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_0_FROM_BankingApp_PRE_SL5SC34EL5EC46");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_0_FROM_BankingApp_PRE_SL5SC34EL5EC46");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_1_FROM_Atm_PRE_SL6SC39EL6EC52");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_0_FROM_Atm_PRE_SL6SC54EL6EC64");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_1_FROM_Atm_PRE_SL6SC39EL6EC52");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_0_FROM_Atm_PRE_SL6SC54EL6EC64");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_0_FROM_Atm_PRE_SL7SC40EL7EC52");
            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_0_FROM_Atm_PRE_SL7SC40EL7EC52");
            bankGraph.Edges.Should().HaveCount(18);
            // todo: test

            var atmGraph = stateMachine.Graphs.Single(x => x.Name == "Atm");
            var bankingAppGraph = stateMachine.Graphs.Single(x => x.Name == "BankingApp");
            var firstGraph = stateMachine.Graphs.Single(x => x.Name == "First");
            var secondGraph = stateMachine.Graphs.Single(x => x.Name == "Second");

            var dedanCode = stateMachine.ToDedan();
        }

        private StateMachineSystem ParseAndConvert(string input)
        {
            var languageSystem = Language.Parser.Rybu4WS.Parse(input, new MemoryStream());

            var converter = new Converter();
            var smsystem = converter.Convert(languageSystem);

            return smsystem;
        }
    }
}
