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
            var smsystem = ParseAndConvert(input);
            var dedanCode = smsystem.ToDedan();
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
