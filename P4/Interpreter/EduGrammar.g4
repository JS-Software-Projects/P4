﻿grammar EduGrammar;

program: line* EOF;
line: functionDeclaration* statement;

statement: 
      variableDeclaration
    | assignment 
    | print  
    | ifBlock 
    | whileBlock 
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

expr : comparisonExpr ( boolOp comparisonExpr )* ;
comparisonExpr : additionExpr ( compareOp additionExpr )* ;
additionExpr : multiplicationExpr ( addSubOp multiplicationExpr )* ;
multiplicationExpr : unaryExpr ( multiOp unaryExpr )* ;
unaryExpr : ( unOP)* ternaryExpr ;
ternaryExpr : term('?' term ':' term)* ;
term : id | constant | 'true' | 'false' | '(' expr ') ' ;

binOP: addSubOp | multiOp | boolOp | compareOp | ternaryOp;
unOP: '!' | '-';

type: 'Num' | 'String' | 'Bool';
constant: Num | String | Bool | Null;
addSubOp: ADD | SUB;
multiOp: MULT | DIV;
boolOp: AND | OR;
ternaryOp: '?' | ':';
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
