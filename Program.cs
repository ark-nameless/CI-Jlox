using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using JLox.Interpreter;

namespace JLox
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 1) {
                Console.WriteLine("Usage: jlox [script]");
                return;
            } 
            else if (args.Length == 1) {
                Lox.runFile(args[0]);
            } else {
                Lox.runPrompt();
            }
        }

    }
}
