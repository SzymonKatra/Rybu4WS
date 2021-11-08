// Template generated code from Antlr4BuildTasks.Template v 8.14

grammar Rybu4WS;

file:
    (server_declaration)*
    (process_declaration)*
    EOF;

process_declaration:
    PROCESS
    ID
    LBRACE
    (statement)*
    RBRACE;

server_declaration: 
    SERVER ID
    LBRACE
    (variable_declaration)*
    (action_declaration)*
    RBRACE;

variable_declaration:
    VAR
    ID
    COLON
    (variable_type_integer | variable_type_enum)
    ASSIGNMENT
    variable_initial_value
    SEMICOLON;

variable_type_integer: variable_type_integer_min VAR_RANGE variable_type_integer_max;
variable_type_integer_min: NUMBER;
variable_type_integer_max: NUMBER;
variable_type_enum: LBRACE ID (COMMA ID)* RBRACE;
variable_initial_value: NUMBER | enum_value;

action_declaration:
    LBRACE
    ID
    (action_condition)?
    RBRACE
    ACTION_ARROW
    LBRACE
    (statement)*
    RBRACE;

action_condition: ACTION_CONDITION condition (condition_logic_operator condition)*;

condition: ID condition_comparison_operator condition_value;
condition_value: NUMBER | enum_value;
condition_logic_operator: CONDITION_AND | CONDITION_OR;
condition_comparison_operator: CONDITION_EQUAL | CONDITION_NOT_EQUAL | CONDITION_GREATER_THAN | CONDITION_LESS_THAN | CONDITION_GREATER_OR_EQUAL_THAN | CONDITION_LESS_OR_EQUAL_THAN;

statement: statement_call | statement_match | statement_state_mutation | statement_return;

statement_call: call_server_name DOT call_action_name LPAREN RPAREN SEMICOLON;
statement_match:
    MATCH call_server_name DOT call_action_name LPAREN RPAREN
    LBRACE
    (statement_match_option)*
    RBRACE;
statement_match_option:
    enum_value ACTION_ARROW
    LBRACE
    ((statement)* | (MATCH_SKIP SEMICOLON))
    RBRACE;
statement_state_mutation: ID statement_state_mutation_operator (NUMBER | enum_value) SEMICOLON;
statement_state_mutation_operator: ASSIGNMENT | OPERATOR_INCREMENT | OPERATOR_DECREMENT;
statement_return: RETURN enum_value SEMICOLON;

call_server_name: ID;
call_action_name: ID;

enum_value: COLON ID;

DOT: '.';
LBRACE: '{';
RBRACE: '}';
LPAREN: '(';
RPAREN: ')';
COLON: ':';
COMMA: ',';
SEMICOLON: ';';
VAR: 'var';
ACTION_CONDITION: '|';
ACTION_ARROW: '->';
VAR_RANGE: '..';
ASSIGNMENT: '=';
OPERATOR_INCREMENT: '+=';
OPERATOR_DECREMENT: '-=';
CONDITION_EQUAL: '==';
CONDITION_NOT_EQUAL: '!=';
CONDITION_GREATER_THAN: '>';
CONDITION_LESS_THAN: '<';
CONDITION_GREATER_OR_EQUAL_THAN: '>=';
CONDITION_LESS_OR_EQUAL_THAN: '<=';
CONDITION_AND: '&&';
CONDITION_OR: '||';
MATCH_SKIP: 'skip';
MATCH: 'match';
RETURN: 'return';
SERVER: 'server';
PROCESS: 'process';
NUMBER: [0-9]+;
ID: [a-zA-Z][a-zA-Z0-9]+;
WHITESPACE: [ \t\r\n]+ -> skip;