using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JLox.Tool
{
    public class GenerateAst
    {
        public static void main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: generate_ast <output_dir>");
                return;
            }
            string outputDir = args[0];
            defineAst(outputDir, "Expr", new List<string>
            {
                "Assign     : Token name, Expr value",
                "Binary     : Expr left, Token op, Expr right",
                "Grouping   : Expr expression",
                "Literal    : object value",
                "Logical    : Expr left, Token op, Expr right",
                "Unary      : Token op, Expr right",
                "Variable   : Token name",
            });

            defineAst(outputDir, "Stmt", new List<string>()
            {
                "Block      : List<Stmt> statements",
                "Expression : Expr expression",
                "If         : Expr condition, Stmt thenBranch, Stmt elseBranch",
                "Print      : Expr expression",
                "Var        : Token name, Expr initializer",
                "While      : Expr condition, Stmt body",
            });
        }

        private static void defineAst(string outputDir, string baseName, List<string> types)
        {
            Directory.CreateDirectory(outputDir);

            var path = Path.Combine(outputDir, baseName + ".cs");

            using (var writer = new StreamWriter(path)) 
            {
                writer.WriteLine("using System;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine();
                writer.WriteLine("namespace JLox.Interpreter");
                writer.WriteLine("{");
                writer.WriteLine();
                writer.WriteLine($"    public abstract class {baseName}");
                writer.WriteLine("     {");
                defineVisitor(writer, baseName, types);
                foreach(var type in types)
                {
                    string className = type.Split(":").First().Trim();
                    string fields = type.Split(":").Last().Trim();

                    defineType(writer, baseName, className, fields);
                }
                writer.WriteLine();
                writer.WriteLine($"    public abstract R Accept<R>(IVisitor<R> visitor);");
                writer.WriteLine("     }");
                writer.WriteLine();
                writer.WriteLine("}");
            }
        }

        private static void defineType(StreamWriter writer, string baseName, string className, string fieldList)
        {
            writer.WriteLine($"\t\t\t public class {className} : {baseName}");
            writer.WriteLine($"\t\t\t{{");
            writer.WriteLine($"");
            #region Constructor
            writer.WriteLine($"\t\t\t\tpublic {className} ({fieldList})");
            writer.WriteLine($"\t\t\t\t{{");
            string[] fields = fieldList.Split(", ");
            foreach (string field in fields) {
                var name = field.Split(" ").Last();
                writer.WriteLine($"\t\t\t\t\tthis.{name} = {name};");
            }
            writer.WriteLine($"\t\t\t\t}}");
            #endregion

            #region Visitor
            writer.WriteLine($"\t\t\t\tpublic override R Accept<R>(IVisitor<R> visitor)");
            writer.WriteLine($"\t\t\t\t{{");
            writer.WriteLine($"\t\t\t\t\treturn visitor.Visit{className}{baseName}(this);");
            writer.WriteLine($"\t\t\t\t}}");
            #endregion

            #region Fields
            foreach (var field in fields)
            {
                writer.WriteLine($"\t\t\tpublic {field} {{ get; set; }}");
            }
            #endregion
            writer.WriteLine($"");
            writer.WriteLine($"\t\t\t}}");
        }

        private static void defineVisitor(StreamWriter writer, string baseName, List<string> types)
        {
            writer.WriteLine($"  public interface IVisitor<R> {{");

            foreach(string type in types)
            {
                string typeName = type.Split(":").First().Trim();
                writer.WriteLine($"   R Visit{typeName}{baseName}({typeName} {baseName.ToLower()}) {{ throw new NotImplementedException(); }}");
            }
            writer.WriteLine($"  }}");
        }
    }
}
