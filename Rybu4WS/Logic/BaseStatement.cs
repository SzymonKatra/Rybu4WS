using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Logic
{
    public abstract class BaseStatement
    {
        public int StartLine { get; set; }
        public int StartColumn { get; set; }

        public int EndLine { get; set; }
        public int EndColumn { get; set; }

        public string GetStringLocation()
        {
            return $"L{StartLine}C{StartColumn}L{EndLine}C{EndColumn}";
        }
    }
}
