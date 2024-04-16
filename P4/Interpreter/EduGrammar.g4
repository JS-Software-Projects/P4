grammar EduGrammar;

program: line* EOF;
line: statement | ifBlock | whileBlock | functionDeclaration;

statement: variableDeclaration | assignment | print ';';

variableDeclaration:
    type id '=' expr ';'| parameter;

functionDeclaration:
    'function' type id '(' parameterList? ')' '{' line* '}';
parameterList:
    parameter (',' parameter)*;
parameter:
    type id;

assignment: id '=' expr;

print: 'print' '(' expr ')';

ifBlock: 'if' expr '{' line* '}' elseBlock?;
elseBlock: 'else' '{' line* '}';

whileBlock: 'while' expr '{' line* '}';

expr: 
     constant         # constantExpr
    | id              # identifier
    | expr binOP expr # binaryExpr
    | unOP  expr      # unaryExpr
    | '(' expr ')'    # parenExpr
    | expr '?' expr ':' expr # ternaryExpr;

binOP: addSubOp | multiOp | boolOp | compareOp;
unOP: '!' | '-';



type: 'Num' | 'String' | 'Bool';
constant: Num | String | Bool | Null;
addSubOp: ADD | SUB;
multiOp: MULT | DIV;
boolOp: AND | OR;
compareOp: '==' | '!=' | '<' | '<=' | '>' | '>=';

ADD: '+';
SUB: '-';
MULT: '*';
DIV: '/';
AND: '&&';
OR: '||';
id : ID;
ID: [a-zA-Z]+;
WS: [ \t]+ -> skip; // Skip spaces and tabs but not newlines
NL: [\r\n]+ -> channel(HIDDEN); // Handle newlines separately

Num: '-'?[0-9]+('.'[0-9]+)?;
String: '"' ~'"'* '"';
Bool: 'true' | 'false';
Null: 'null';
