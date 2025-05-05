using System;
using System.Collections.Generic;

namespace JLox.Interpreter
{

    public abstract class Expr
     {
  public interface IVisitor<R> {
   R VisitAssignExpr(Assign expr) { throw new NotImplementedException(); }
   R VisitBinaryExpr(Binary expr) { throw new NotImplementedException(); }
   R VisitGroupingExpr(Grouping expr) { throw new NotImplementedException(); }
   R VisitLiteralExpr(Literal expr) { throw new NotImplementedException(); }
   R VisitLogicalExpr(Logical expr) { throw new NotImplementedException(); }
   R VisitUnaryExpr(Unary expr) { throw new NotImplementedException(); }
   R VisitVariableExpr(Variable expr) { throw new NotImplementedException(); }
  }
			 public class Assign : Expr
			{

				public Assign (Token name, Expr value)
				{
					this.name = name;
					this.value = value;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitAssignExpr(this);
				}
			public Token name { get; set; }
			public Expr value { get; set; }

			}
			 public class Binary : Expr
			{

				public Binary (Expr left, Token op, Expr right)
				{
					this.left = left;
					this.op = op;
					this.right = right;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitBinaryExpr(this);
				}
			public Expr left { get; set; }
			public Token op { get; set; }
			public Expr right { get; set; }

			}
			 public class Grouping : Expr
			{

				public Grouping (Expr expression)
				{
					this.expression = expression;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitGroupingExpr(this);
				}
			public Expr expression { get; set; }

			}
			 public class Literal : Expr
			{

				public Literal (object value)
				{
					this.value = value;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitLiteralExpr(this);
				}
			public object value { get; set; }

			}
			 public class Logical : Expr
			{

				public Logical (Expr left, Token op, Expr right)
				{
					this.left = left;
					this.op = op;
					this.right = right;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitLogicalExpr(this);
				}
			public Expr left { get; set; }
			public Token op { get; set; }
			public Expr right { get; set; }

			}
			 public class Unary : Expr
			{

				public Unary (Token op, Expr right)
				{
					this.op = op;
					this.right = right;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitUnaryExpr(this);
				}
			public Token op { get; set; }
			public Expr right { get; set; }

			}
			 public class Variable : Expr
			{

				public Variable (Token name)
				{
					this.name = name;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitVariableExpr(this);
				}
			public Token name { get; set; }

			}

    public abstract R Accept<R>(IVisitor<R> visitor);
     }

}
