using System;
using System.Collections.Generic;

namespace JLox.Interpreter
{

    public abstract class Stmt
     {
  public interface IVisitor<R> {
   R VisitBlockStmt(Block stmt) { throw new NotImplementedException(); }
   R VisitExpressionStmt(Expression stmt) { throw new NotImplementedException(); }
   R VisitIfStmt(If stmt) { throw new NotImplementedException(); }
   R VisitPrintStmt(Print stmt) { throw new NotImplementedException(); }
   R VisitVarStmt(Var stmt) { throw new NotImplementedException(); }
   R VisitWhileStmt(While stmt) { throw new NotImplementedException(); }
  }
			 public class Block : Stmt
			{

				public Block (List<Stmt> statements)
				{
					this.statements = statements;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitBlockStmt(this);
				}
			public List<Stmt> statements { get; set; }

			}
			 public class Expression : Stmt
			{

				public Expression (Expr expression)
				{
					this.expression = expression;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitExpressionStmt(this);
				}
			public Expr expression { get; set; }

			}
			 public class If : Stmt
			{

				public If (Expr condition, Stmt thenBranch, Stmt elseBranch)
				{
					this.condition = condition;
					this.thenBranch = thenBranch;
					this.elseBranch = elseBranch;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitIfStmt(this);
				}
			public Expr condition { get; set; }
			public Stmt thenBranch { get; set; }
			public Stmt elseBranch { get; set; }

			}
			 public class Print : Stmt
			{

				public Print (Expr expression)
				{
					this.expression = expression;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitPrintStmt(this);
				}
			public Expr expression { get; set; }

			}
			 public class Var : Stmt
			{

				public Var (Token name, Expr initializer)
				{
					this.name = name;
					this.initializer = initializer;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitVarStmt(this);
				}
			public Token name { get; set; }
			public Expr initializer { get; set; }

			}
			 public class While : Stmt
			{

				public While (Expr condition, Stmt body)
				{
					this.condition = condition;
					this.body = body;
				}
				public override R Accept<R>(IVisitor<R> visitor)
				{
					return visitor.VisitWhileStmt(this);
				}
			public Expr condition { get; set; }
			public Stmt body { get; set; }

			}

    public abstract R Accept<R>(IVisitor<R> visitor);
     }

}
