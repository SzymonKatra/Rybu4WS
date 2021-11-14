using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using Antlr4.Runtime;
using Rybu4WS.StateMachine;

namespace Rybu4WS.Test.IntegrationTests
{
    public class EndToEnd
    {
        [Fact]
        public void Bank()
        {
            var input = TestUtils.ReadResource("bank.txt");

            var str = new AntlrInputStream(input);
            var lexer = new Rybu4WSLexer(str);
            var tokens = new CommonTokenStream(lexer);
            var parser = new Rybu4WSParser(tokens);
            var listener_lexer = new ErrorListener<int>();
            var listener_parser = new ErrorListener<IToken>();
            lexer.AddErrorListener(listener_lexer);
            parser.AddErrorListener(listener_parser);
            var tree = parser.file();
            var vis = new Rybu4WSVisitor();
            vis.Visit(tree);
            var res = vis.Result;
            var postProcessor = new PostProcessor();
            postProcessor.Process(res);

            var converter = new Converter();
            var smsystem = converter.Convert(res);
            var dedanCode = smsystem.ToDedan();
        }
    }
}
