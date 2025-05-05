using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLox.Interpreter
{
    [Serializable]
    public class ParseException : Exception
    {
        public ParseException() : base() { }
        public ParseException(string message) : base(message) { }
        public ParseException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class RuntimeException : Exception
    {
        public Token token;

        public RuntimeException() : base() { }
        public RuntimeException(Token token, string message) : base(message)
        {
            this.token = token;
        }
        public RuntimeException(string message, Exception inner) : base(message, inner) { }
    }
}
