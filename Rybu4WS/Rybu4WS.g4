grammar Rybu4WS;

file:
    (const_declaration | type_definition)*
    (interface_declaration)*
    (server_declaration)*
    (server_definition)*
    (process_declaration | group_declaration)*
    EOF;

const_declaration: CONST ID ASSIGNMENT NUMBER SEMICOLON;

type_definition: TYPE ID ASSIGNMENT variable_type SEMICOLON;

interface_declaration:
    INTERFACE
    ID
    LBRACE
    (interface_action)*
    RBRACE;

interface_action:
    LBRACE
    ID
    RBRACE
    ACTION_ARROW
    LBRACE
    (interface_action_possible_return_values)?
    RBRACE;

interface_action_possible_return_values: enum_value (COMMA enum_value)*;

group_declaration:
    GROUP
    ID
    LBRACE
    (variable_declaration_with_value)*
    (process)*
    RBRACE;

process_declaration: process;

process:
    (process_indexer)?
    PROCESS
    ID
    LBRACE
    (statement)*
    RBRACE;

process_indexer:
    LCHEVRON
    ID
    ASSIGNMENT
    variable_type_integer
    RCHEVRON;

server_definition:
    VAR
    server_definition_name
    (array_access | array_range)?
    ASSIGNMENT
    server_definition_type
    LPAREN
    (server_definition_dependency_list)?
    RPAREN
    (server_definition_variable_list)?
    SEMICOLON;

server_definition_name: ID;
server_definition_type: ID;
server_definition_dependency_list: server_definition_dependency (COMMA server_definition_dependency)*;
server_definition_dependency: ID (array_access | array_range)?;

server_definition_variable_list:
    LBRACE
    server_definition_variable (COMMA server_definition_variable)*
    RBRACE;

server_definition_variable: server_definition_variable_name (array_access | array_range)? ASSIGNMENT server_definition_variable_value;
server_definition_variable_name: ID;
server_definition_variable_value: NUMBER | enum_value | ID;

server_declaration: 
    SERVER ID
    (server_dependency_list)?
    (server_implemented_interfaces)?
    LBRACE
    (variable_declaration)*
    (action_declaration)*
    RBRACE;

server_dependency_list:
    LPAREN
    (server_dependency (COMMA server_dependency)*)?
    RPAREN;
server_dependency: server_dependency_name COLON server_dependency_type (array_declaration)?;
server_dependency_name: ID;
server_dependency_type: ID;

server_implemented_interfaces:
    IMPLEMENTS
    ID (COMMA ID)*;

variable_declaration:
    VAR
    ID
    COLON
    variable_type
    (array_declaration)?
    SEMICOLON;

variable_declaration_with_value:
    VAR
    ID
    COLON
    variable_type
    (array_declaration)?
    ASSIGNMENT
    variable_value
    SEMICOLON;

variable_type: variable_type_integer | variable_type_enum | ID;
variable_type_integer: variable_type_integer_min VAR_RANGE variable_type_integer_max;
variable_type_integer_min: NUMBER | ID;
variable_type_integer_max: NUMBER | ID;
variable_type_enum: LBRACE ID (COMMA ID)* RBRACE;
variable_value: NUMBER | enum_value | ID;

action_declaration:
    LBRACE
    ID
    (action_condition)?
    RBRACE
    ACTION_ARROW
    (timed_delay)?
    LBRACE
    (statement)*
    RBRACE;

action_condition: ACTION_CONDITION condition_list;

statement: statement_call | statement_match | statement_state_mutation | statement_return | statement_terminate | statement_loop | statement_wait | statement_if;

statement_call: call_server_name (array_access)? DOT call_action_name LPAREN RPAREN SEMICOLON;
statement_match:
    statement_match_call
    LBRACE
    (statement_match_option)*
    RBRACE;
statement_match_call: MATCH call_server_name (array_access)? DOT call_action_name LPAREN RPAREN;
statement_match_option:
    enum_value ACTION_ARROW
    LBRACE
    ((statement)* | (MATCH_SKIP SEMICOLON))
    RBRACE;
statement_state_mutation: ID (array_access)? statement_state_mutation_operator statement_state_mutation_value SEMICOLON;
statement_state_mutation_value: NUMBER | enum_value | ID;
statement_state_mutation_operator: ASSIGNMENT | OPERATOR_INCREMENT | OPERATOR_DECREMENT | OPERATOR_MODULO;
statement_return: RETURN enum_value SEMICOLON;
statement_terminate: TERMINATE SEMICOLON;
statement_loop:
    statement_loop_identifier
    LBRACE
    (statement)*
    RBRACE;
statement_loop_identifier: LOOP;
statement_wait:
    WAIT
    LPAREN condition_list RPAREN
    SEMICOLON;
statement_if:
    statement_if_header
    LBRACE
    (statement)*
    RBRACE;
statement_if_header: statement_if_identifier LPAREN condition_list RPAREN;
statement_if_identifier: IF;

condition_list: condition (condition_logic_operator condition)*;
condition: ID (array_access)? condition_comparison_operator condition_value;
condition_value: NUMBER | enum_value | ID;
condition_logic_operator: CONDITION_AND | CONDITION_OR;
condition_comparison_operator: CONDITION_EQUAL | CONDITION_NOT_EQUAL | LCHEVRON | RCHEVRON | CONDITION_GREATER_OR_EQUAL_THAN | CONDITION_LESS_OR_EQUAL_THAN;

call_server_name: ID;
call_action_name: ID;

enum_value: COLON ID;
array_access: LBRACKET (NUMBER | ID) RBRACKET;
array_range: LBRACKET array_range_min VAR_RANGE array_range_max RBRACKET;
array_range_min: NUMBER | ID;
array_range_max: NUMBER | ID;
array_declaration: LBRACKET (NUMBER | ID) RBRACKET;

timed_delay: (LPAREN | LCHEVRON) (timed_delay_value | (timed_delay_min COMMA timed_delay_max)) (RPAREN | RCHEVRON);
timed_delay_min: timed_delay_value;
timed_delay_max: timed_delay_value;
timed_delay_value: NUMBER | ID;

DOT: '.';
LBRACE: '{';
RBRACE: '}';
LPAREN: '(';
RPAREN: ')';
LBRACKET: '[';
RBRACKET: ']';
LCHEVRON: '<';
RCHEVRON: '>';
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
OPERATOR_MODULO: '%=';
CONDITION_EQUAL: '==';
CONDITION_NOT_EQUAL: '!=';
CONDITION_GREATER_OR_EQUAL_THAN: '>=';
CONDITION_LESS_OR_EQUAL_THAN: '<=';
CONDITION_AND: '&&';
CONDITION_OR: '||';
LOOP: 'loop';
MATCH_SKIP: 'skip';
MATCH: 'match';
RETURN: 'return';
TERMINATE: 'terminate';
WAIT: 'wait';
IF: 'if';
SERVER: 'server';
PROCESS: 'process';
INTERFACE: 'interface';
IMPLEMENTS: 'implements';
GROUP: 'group';
CONST: 'const';
TYPE: 'type';
NUMBER: [0-9]+;
ID: [a-zA-Z][a-zA-Z0-9]*;
COMMENT: '--' ~[\r\n]* -> skip;
WHITESPACE: [ \t\r\n]+ -> skip;