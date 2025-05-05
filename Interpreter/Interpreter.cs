using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JLox.Interpreter
{
    public class Interpreter : Expr.IVisitor<object>, Stmt.IVisitor<object?>
    {
        private Environment env = new(); 

        public void interpret(List<Stmt> statements)
        {
            try
            {
                foreach (Stmt statement in statements)
                {
                    execute(statement);
                }
            }
            catch (RuntimeException ex)
            {
                Lox.runtimeError(ex);
            }
        }
        public object VisitBinaryExpr(Expr.Binary expr)
        {
            object left = evaluate(expr.left);
            object right = evaluate(expr.right);

            switch (expr.op.Type)
            {
                case TokenType.GREATER:
                    checkNumberOperands(expr.op, left, right);
                    return (double)left > (double)right;
                case TokenType.GREATER_EQUAL:
                    checkNumberOperands(expr.op, left, right);
                    return (double)left >= (double)right;
                case TokenType.LESS:
                    checkNumberOperands(expr.op, left, right);
                    return (double)left < (double)right;
                case TokenType.LESS_EQUAL:
                    checkNumberOperands(expr.op, left, right);
                    return (double)left <= (double)right;
                case TokenType.MINUS:
                    checkNumberOperands(expr.op, left, right);
                    return (double)left - (double)right;
                case TokenType.PLUS:
                    if (left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }
                    if (left is string && right is string)
                    {
                        return (string)left + (string)right;
                    }
                    throw new RuntimeException(expr.op, "Operants must be two number or two strings.");
                case TokenType.SLASH:
                    checkNumberOperands(expr.op, left, right);
                    return (double)left / (double)right;
                case TokenType.STAR:
                    checkNumberOperands(expr.op, left, right);
                    return (double)left * (double)right;
                case TokenType.BANG_EQUAL:
                    return !isEqual(left, right);
                case TokenType.BANG:
                    return isEqual(left, right);
            }

            // Unreachable
            return null;
        }

        public object VisitGroupingExpr(Expr.Grouping expr)
        {
            return evaluate(expr.expression);
        }

        public object VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.value;
        }

        public object VisitlogicalExpr(Expr.Logical expr)
        {
            object left = evaluate(expr.left);

            if (expr.op.Type == TokenType.OR)
            {
                if (isTruthy(left)) return left;
            }
            else
            {
                if (!isTruthy(left)) return left;
            }

            return evaluate(expr.right);
        }

        public object VisitUnaryExpr(Expr.Unary expr)
        {
            object right = evaluate(expr.right);

            switch (expr.op.Type) 
            {
                case TokenType.BANG:
                    return !isTruthy(right);
                case TokenType.MINUS:
                    checkNumberOperand(expr.op, right);
                    return -(double)right;
            }

            // Unreachable
            return null;
        }

        public object VisitVariableExpr(Expr.Variable expr)
        {
            return env.get(expr.name);
        }

        private void checkNumberOperand(Token op, object operand)
        {
            if (operand is double) return;
            throw new RuntimeException(op, "Operand must be a number.");
        }
        private void checkNumberOperands(Token op, object left, object right)
        {
            if (left is double && right is double) return;
            throw new RuntimeException(op, "Operant must be numbers.");
        }

        private bool isTruthy(Object obj)
        {
            if (obj == null) return false;
            if (obj is bool) return (bool)obj;
            if (obj is string) return string.IsNullOrEmpty(obj.ToString());
            return true;
        }

        private object evaluate(Expr expr)
        {
            return expr.Accept(this);
        }
        private void execute(Stmt stmt)
        {
            stmt.Accept(this);
        }
        public object? VisitBlockStmt(Stmt.Block stmt)
        {
            executeBlock(stmt.statements, new Environment(env));
            return null;
        }
        private void executeBlock(List<Stmt> statements, Environment env)
        {
            Environment previous = this.env;
            try
            {
                this.env = env;

                foreach (Stmt statement in statements)
                {
                    execute(statement);
                }
            }
            finally
            {
                this.env = previous;
            }
        }
        private bool isEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }
        private string stringify(object obj)
        {
            if (obj == null) return "nil";
            if (obj is double)
            {
                string text = obj.ToString();
                if(text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }

            return obj?.ToString() ?? "nil";
        }

        public object? VisitExpressionStmt(Stmt.Expression stmt)
        {
            evaluate(stmt.expression);
            return null;
        }

        public object? VisitIfStmt(Stmt.If stmt)
        {
            if (isTruthy(evaluate(stmt.condition)))
            {
                execute(stmt.thenBranch);
            }
            else if (stmt.elseBranch != null)
            {
                execute(stmt.elseBranch);
            }
            return null;
        }

        public object? VisitPrintStmt(Stmt.Print stmt)
        {
            object value = evaluate(stmt.expression);
            Console.WriteLine(stringify(value));
            return null;
        }

        public object? VisitVarStmt(Stmt.Var stmt)
        {
            object value = null;
            if (stmt.initializer != null)
            {
                value = evaluate(stmt.initializer);
            }

            env.define(stmt.name.Lexeme, value);
            return null;
        }

        public object? VisitWhileStmt(Stmt.While stmt)
        {
            while (isTruthy(evaluate(stmt.condition)))
            {
                execute(stmt.body);
            }
            return null;
        } 

        public object VisitAssignExpr(Expr.Assign expr)
        {
            object value = evaluate(expr.value);
            env.assign(expr.name, value);
            return value;
        }
    }
}
