# Grammar 

## Expression

*Expression* -> LogicOperation;

*LogicOperation* -> Equality (("&&" | "||") Equality)*;

*Equality* -> StringOperation (("==" | "!=")) StringOperation*;

*StringOperation* -> Comparison (("@" | "@@")Comparison)*;

*Comparison* -> Term (("<"| "<=" | ">"| ">=") Term)*;

*Term* -> Factor (("+" | "-") Factor) *;

*Factor* -> Power (("*" | "/") Power)*;

*Power* -> Unary ("^" Unary | "!" Unary)*;

*Unary* -> ("-")? Primary ;

*Primary* -> "BooleanLiteral" | "StringLiteral" | "NumericLiteral" | "("Expression")" | DotPattern;

*DotPattern* -> "Identifier" ("," "Identifier" ("("Expression")")?)* | ("["Expressiom"]")*

## Statemments
