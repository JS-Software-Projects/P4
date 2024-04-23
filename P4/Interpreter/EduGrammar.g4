grammar EduGrammar;

program: line* EOF;
line: statement;

statement: 
      variableDeclaration
    | assignment 
    | print  
    | ifBlock 
    | whileBlock 
    | functionDeclaration
    | functionCall
    | returnStatement
    | forLoop;

variableDeclaration:
    type id '=' expr ';' | type id ';';

functionDeclaration:
    'function' type id '(' parameterList? ')' block;    
parameterList:
    parameter (',' parameter)*;
parameter:
    type id;
    
functionCall:
    id '(' argumentList? ')' ';';
argumentList:
    expr (',' expr)*;    

assignment: id '=' expr ';';

print: 'print' '(' expr ')' ';';

ifBlock: 'if' expr block elseBlock?;
elseBlock: 'else' block;

whileBlock: 'while' expr block;

block: '{' line* '}';

returnStatement: 'return' expr ';';
forLoop: 'for' '(' variableDeclaration ';' expr ';' assignment ')' block;

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
