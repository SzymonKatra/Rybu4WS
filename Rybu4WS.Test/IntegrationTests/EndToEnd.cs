//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;
//using FluentAssertions;
//using Antlr4.Runtime;
//using Rybu4WS.StateMachine;
//using System.IO;

//namespace Rybu4WS.Test.IntegrationTests
//{
//    public class EndToEnd
//    {
//        [Fact]
//        public void Bank()
//        {
//            var input = TestUtils.ReadResource("bank.txt");
//            var stateMachine = ParseAndConvert(input);

//            stateMachine.Graphs.Should().HaveCount(5);
//            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "bank");
//            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "atm");
//            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "bankingApp");
//            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "ProcessFirst");
//            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "ProcessSecond");

//            var bankGraph = stateMachine.Graphs.Single(x => x.Name == "bank");
//            bankGraph.InitNode.ToString().Should().Be("balanceA_1_balanceB_0");
//            bankGraph.Nodes.Should().HaveCount(15);
//            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_0");
//            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_1");
//            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_0");
//            bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_1");
//            //bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_0_FROM_BankingApp_PRE_SL4SC50EL4EC63");
//            //bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_0_FROM_BankingApp_PRE_SL4SC65EL4EC77");
//            //bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_1_FROM_BankingApp_PRE_SL4SC79EL4EC89");
//            //bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_0_FROM_BankingApp_PRE_SL5SC34EL5EC46");
//            //bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_0_FROM_BankingApp_PRE_SL5SC34EL5EC46");
//            //bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_1_FROM_Atm_PRE_SL6SC39EL6EC52");
//            //bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_0_FROM_Atm_PRE_SL6SC54EL6EC64");
//            //bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_1_FROM_Atm_PRE_SL6SC39EL6EC52");
//            //bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_0_FROM_Atm_PRE_SL6SC54EL6EC64");
//            //bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_0_balanceB_0_FROM_Atm_PRE_SL7SC40EL7EC52");
//            //bankGraph.Nodes.Should().ContainSingle(x => x.ToString() == "balanceA_1_balanceB_0_FROM_Atm_PRE_SL7SC40EL7EC52");
//            bankGraph.Edges.Should().HaveCount(18);
//            // todo: test

//            var atmGraph = stateMachine.Graphs.Single(x => x.Name == "atm");
//            var bankingAppGraph = stateMachine.Graphs.Single(x => x.Name == "bankingApp");
//            var firstGraph = stateMachine.Graphs.Single(x => x.Name == "ProcessFirst");
//            var secondGraph = stateMachine.Graphs.Single(x => x.Name == "ProcessSecond");

//            var dedanCode = stateMachine.ToDedan();
//        }

//        [Fact]
//        public void Deadlock()
//        {
//            var input = TestUtils.ReadResource("deadlock.txt");
//            var stateMachine = ParseAndConvert(input);
//            stateMachine.Graphs.Should().HaveCount(4);
//            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "res1");
//            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "res2");
//            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "ProcessFirst");
//            stateMachine.Graphs.Should().ContainSingle(x => x.Name == "ProcessSecond");
//            var dedanCode = stateMachine.ToDedan();
//        }

//        [Fact]
//        public void Composing_Test()
//        {
//            var input = TestUtils.ReadResource("composing_test.txt");
//            var stateMachine = ParseAndConvert(input);
//            var dedanCode = stateMachine.ToDedan();
//        }


//        private StateMachineSystem ParseAndConvert(string input)
//        {
//            var errorStream = new MemoryStream();
//            var languageSystem = Language.Parser.Rybu4WS.Parse(input, errorStream);

//            if (errorStream.Length > 0)
//            {
//                errorStream.Seek(0, SeekOrigin.Begin);
//                using (var reader = new StreamReader(errorStream))
//                {
//                    var str = reader.ReadToEnd();
//                    throw new Exception(str);
//                }
//            }

//            var errorStreamConvert = new MemoryStream();
//            var smsystem = Converter.ConvertToStateMachine(languageSystem, errorStreamConvert);
//            if (errorStreamConvert.Length > 0)
//            {
//                errorStreamConvert.Seek(0, SeekOrigin.Begin);
//                using (var reader = new StreamReader(errorStreamConvert))
//                {
//                    var str = reader.ReadToEnd();
//                    throw new Exception(str);
//                }
//            }

//            return smsystem;
//        }
//    }
//}
