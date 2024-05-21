﻿grammar EduGrammar;

program: functionDeclaration* statement* EOF;

statement: 
      variableDeclaration
    | assignment 
    | print  
    | ifBlock 
    | whileBlock 
    | functionCallStatement
    | returnStatement
    | forLoop
    | gameObjectDeclaration
    | gameObjectMethodCall;
    
variableDeclaration:
    type id '=' expr ';' | type id ';';

functionDeclaration:
    'function' type id '(' parameterList? ')' block;    
parameterList:
    parameter (',' parameter)*;
parameter:
    type id;
    
functionCallStatement:
    id '(' argumentList? ')' ';';
functionCallExpr:
    id '(' argumentList? ')' ';';
    
argumentList:
    expr (',' expr)*;    

assignment: id '=' expr ';';

print: 'print' '(' expr ')' ';';

ifBlock: 'if' '(' expr ')' block  elseBlock?;
elseBlock: 'else' block;

whileBlock: 'while' '(' expr ')' block;

block: '{' statement* '}';

returnStatement: 'return' expr ';';
forLoop: 'for' '(' variableDeclaration ';' expr ';' assignment ')' block;


gameObjectDeclaration: objectType id '=' 'new' objectType '(' argumentList? ')' ';';
gameObjectMethodCall: id '.' ID '(' argumentList? ')' ';';

objectType: 'Tower' | 'Hero';

expr: boolExpr;
boolExpr           : comparisonExpr ( boolOp comparisonExpr )* ;
comparisonExpr     : additionExpr ( compareOp additionExpr )* ;
additionExpr       : multiplicationExpr ( addSubOp multiplicationExpr )* ;
multiplicationExpr : unaryExpr ( multiOp unaryExpr )* ;
unaryExpr          : ( unOP)* ternaryExpr ;
ternaryExpr        : term('?' term ':' term)* ;
term               : id | constant | parenExpr | functionCallExpr;

//binOP: addSubOp | multiOp | boolOp | compareOp | ternaryOp;

unOP: '!' | '-';
parenExpr: '(' expr ')' ;
type: 'Num' | 'String' | 'Bool' | 'Void';
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
ID: [a-zA-Z_]+[a-zA-Z0-9_]*;
WS: [ \t]+ -> skip; // Skip spaces and tabs but not newlines
NL: [\r\n]+ -> channel(HIDDEN); // Handle newlines separately

Num: [0-9]+('.'[0-9]+)?;
String: '"' ~'"'* '"';
Bool: 'true' | 'false';
Null: 'null';
