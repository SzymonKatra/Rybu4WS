// Template generated code from Antlr4BuildTasks.Template v 8.14
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.IO;
using System.Text;
using System.Linq;
using Rybu4WS.Language.Parser;

namespace Rybu4WS
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Try("1 + 2 + 3");
            //Try("1 2 + 3");
            //Try("1 + +");
            Try(File.ReadAllText("antlrtest/file2.txt"));
        }

        static void Try(string input)
        {
            //var str = new AntlrInputStream(input);
            //System.Console.WriteLine(input);
            //var lexer = new Rybu4WSLexer(str);
            //var tokens = new CommonTokenStream(lexer);
            //var parser = new Rybu4WSParser(tokens);
            //var listener_lexer = new Rybu4WSErrorListener<int>();
            //var listener_parser = new Rybu4WSErrorListener<IToken>();
            //lexer.AddErrorListener(listener_lexer);
            //parser.AddErrorListener(listener_parser);
            //var tree = parser.file();
            //var vis = new Rybu4WSVisitor();
            //vis.Visit(tree);
            //var res = vis.Result;
            //var postProcessor = new Rybu4WSPostProcessor();
            //postProcessor.Process(res);

            //if (listener_lexer.HadError || listener_parser.HadError)
            //    System.Console.WriteLine("error in parse.");
            //else
            //    System.Console.WriteLine("parse completed.");
        }

        static string ReadAllInput(string fn)
        {
            var input = System.IO.File.ReadAllText(fn);
            return input;
        }
    }
}
