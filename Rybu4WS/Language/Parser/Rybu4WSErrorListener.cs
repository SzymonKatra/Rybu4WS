// Template generated code from Antlr4BuildTasks.Template v 8.14
namespace Rybu4WS
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Rybu4WSErrorListener<S> : IAntlrErrorListener<S>
    {
        public bool HadError { get; private set; }
        private TextWriter _errorTextWriter;

        public Rybu4WSErrorListener(TextWriter errorTextWriter)
        {
            _errorTextWriter = errorTextWriter;
        } 

        public void SyntaxError(TextWriter output, IRecognizer recognizer, S offendingSymbol, int line,
            int charPositionInLine, string msg, RecognitionException e)
        {
            HadError = true;
            _errorTextWriter.WriteLine($"L: {line} C: {charPositionInLine + 1} - {msg}");
        }
    }
}