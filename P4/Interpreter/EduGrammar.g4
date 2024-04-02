grammar EduGrammar;

program: line* EOF;
line: statement | ifBlock | whileBlock;

statement: (assignment | print) ';';

assignment: id '=' expr;

print: 'print' '(' expr ')';

ifBlock: 'if' expr '{' line* '}'elseBlock?;

elseBlock: 'else' '{' line* '}';

whileBlock: 'while' expr '{' line* '}';

expr: 
    constant                # constantExpr
    | id                    # idExpr
    | expr multiOp expr     # multiExpr
    | expr addSubOp expr    # addSubExpr
    | expr compareOp expr   # compareExpr
    | expr boolOp expr      # boolExpr
    | '(' expr ')'          # parenExpr
    | '!' expr              # notExpr;

constant: Num | String | Bool | Null ;
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
