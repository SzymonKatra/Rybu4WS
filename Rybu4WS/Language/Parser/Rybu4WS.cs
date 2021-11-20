using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Language.Parser
{
    public static class Rybu4WS
    {
        public static Language.System Parse(string input, Stream parseOutputMessages)
        {
            var textWriter = new StreamWriter(parseOutputMessages);

            var str = new AntlrInputStream(input);
            var lexer = new Rybu4WSLexer(str);
            var tokens = new CommonTokenStream(lexer);
            var parser = new Rybu4WSParser(tokens);
            var listener_lexer = new Rybu4WSErrorListener<int>(textWriter);
            var listener_parser = new Rybu4WSErrorListener<IToken>(textWriter);
            lexer.AddErrorListener(listener_lexer);
            parser.AddErrorListener(listener_parser);
            var tree = parser.file();

            if (listener_lexer.HadError || listener_parser.HadError)
            {
                textWriter.WriteLine("!!! Some errors occurred during parsing !!!");
                textWriter.Flush();
                return null;
            }

            var visitor = new Rybu4WSVisitor(textWriter);
            visitor.Visit(tree);

            var postProcessor = new Rybu4WSPostProcessor(textWriter);
            postProcessor.Process(visitor.Result);

            textWriter.Flush();

            return visitor.Result;
        }
    }
}
