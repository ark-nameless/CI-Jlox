using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace JLox.Interpreter
{
    public static class Lox
    {
        private static Interpreter interpreter = new Interpreter();
        public static bool hadError = false;
        public static bool hadRuntimeError = false;

        public static void runFile(String path)
        {
            byte[] bytes = File.ReadAllBytes(path);
            if (hadError) return;
            if (hadRuntimeError) return;
            run(Encoding.UTF8.GetString(bytes));
        }

        public static void runPrompt()
        {
            var stream = Console.OpenStandardInput(1024 * 8);
            Console.SetIn(new StreamReader(stream, Encoding.ASCII));

            while(true)
            {
                Console.Write("> ");
                string line = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrEmpty(line)) break;
                run(line);
                hadError = false;
            }
        }

        public static void run(String source) {
            Scanner scanner = new Scanner(source);
            List<Token> tokens = scanner.scanTokens();

            Parser parser = new Parser(tokens);
            List<Stmt> statements = parser.parse();

            if (hadError) return;

            interpreter.interpret(statements);
            //Console.WriteLine(new AstPrinter().Print(expression));
        }
        
        public static void error(int line, string message)
        {
            report(line, "", message);
        }

        public static void report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error {where}: {message}");
            hadError = true;
        }

        public static void error(Token token, string message)
        {
            if (token.Type == TokenType.EOF)
            {
                report(token.Line, " at end", message);
            }
            else
            {
                report(token.Line, " at '" + token.Lexeme + "'", message);
            }
        }

        public static void runtimeError(RuntimeException e)
        {
            Console.WriteLine(e.Message + "\n[line " + e.token.Line + "]");
            hadRuntimeError = true;
        }
    }
}
