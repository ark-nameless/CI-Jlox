using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLox.Interpreter
{
    public class AstPrinter : Expr.IVisitor<string>
    {
        public string Print(Expr expr)
        {
            return expr.Accept(this);
        }

        public string VisitBinaryExpr(Expr.Binary expr)
        {
            return parenthesize(expr.op.Lexeme, expr.left, expr.right);
        }

        public string VisitGroupingExpr(Expr.Grouping expr)
        {
            return parenthesize("group", expr.expression);
        }

        public string VisitLiteralExpr(Expr.Literal expr)
        {
            if (expr == null || expr.value == null) return "nil";
            return expr.value.ToString() ?? string.Empty;
        }

        public string VisitUnaryExpr(Expr.Unary expr)
        {
            return parenthesize(expr.op.Lexeme, expr.right);
        }

        private string parenthesize(string name, params Expr[] exprs)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);
            foreach (Expr e in exprs)
            {
                builder.Append(" ");
                builder.Append(e.Accept(this));
            }
            builder.Append(")");

            return builder.ToString();
        }
    }
}
