define type142



results: matched list<resulttype>, missing list<sourcetype> 

select *
from environment
where
	when target.id match "daemon"
	then target.id match glob entry.id as "daemon"



<eval-statement> ::= when | <if-statement> 
<if-statement> ::= when <expression> 


grouping → "(" expression ")" ;

unary → ( "-" | "!" ) expression ;

binary → expression operator expression ;

expression → literal | unary | binary | grouping ;

literal → NUMBER | STRING | "true" | "false" | "nil"; 

operator → "==" | "!=" | "<" | "<=" | ">" | ">=" | "+" | "-" | "*" | "/" ;