// Generated from ../Rybu4WS.g4 by ANTLR 4.9.2
import org.antlr.v4.runtime.tree.ParseTreeListener;

/**
 * This interface defines a complete listener for a parse tree produced by
 * {@link Rybu4WSParser}.
 */
public interface Rybu4WSListener extends ParseTreeListener {
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#file}.
	 * @param ctx the parse tree
	 */
	void enterFile(Rybu4WSParser.FileContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#file}.
	 * @param ctx the parse tree
	 */
	void exitFile(Rybu4WSParser.FileContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#process_declaration}.
	 * @param ctx the parse tree
	 */
	void enterProcess_declaration(Rybu4WSParser.Process_declarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#process_declaration}.
	 * @param ctx the parse tree
	 */
	void exitProcess_declaration(Rybu4WSParser.Process_declarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#instance_declaration}.
	 * @param ctx the parse tree
	 */
	void enterInstance_declaration(Rybu4WSParser.Instance_declarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#instance_declaration}.
	 * @param ctx the parse tree
	 */
	void exitInstance_declaration(Rybu4WSParser.Instance_declarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#instance_arguments}.
	 * @param ctx the parse tree
	 */
	void enterInstance_arguments(Rybu4WSParser.Instance_argumentsContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#instance_arguments}.
	 * @param ctx the parse tree
	 */
	void exitInstance_arguments(Rybu4WSParser.Instance_argumentsContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#instance_states}.
	 * @param ctx the parse tree
	 */
	void enterInstance_states(Rybu4WSParser.Instance_statesContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#instance_states}.
	 * @param ctx the parse tree
	 */
	void exitInstance_states(Rybu4WSParser.Instance_statesContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#instance_state_init}.
	 * @param ctx the parse tree
	 */
	void enterInstance_state_init(Rybu4WSParser.Instance_state_initContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#instance_state_init}.
	 * @param ctx the parse tree
	 */
	void exitInstance_state_init(Rybu4WSParser.Instance_state_initContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#server_declaration}.
	 * @param ctx the parse tree
	 */
	void enterServer_declaration(Rybu4WSParser.Server_declarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#server_declaration}.
	 * @param ctx the parse tree
	 */
	void exitServer_declaration(Rybu4WSParser.Server_declarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#argument_with_type_list}.
	 * @param ctx the parse tree
	 */
	void enterArgument_with_type_list(Rybu4WSParser.Argument_with_type_listContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#argument_with_type_list}.
	 * @param ctx the parse tree
	 */
	void exitArgument_with_type_list(Rybu4WSParser.Argument_with_type_listContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#argument_with_type}.
	 * @param ctx the parse tree
	 */
	void enterArgument_with_type(Rybu4WSParser.Argument_with_typeContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#argument_with_type}.
	 * @param ctx the parse tree
	 */
	void exitArgument_with_type(Rybu4WSParser.Argument_with_typeContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#variable_declaration}.
	 * @param ctx the parse tree
	 */
	void enterVariable_declaration(Rybu4WSParser.Variable_declarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#variable_declaration}.
	 * @param ctx the parse tree
	 */
	void exitVariable_declaration(Rybu4WSParser.Variable_declarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#variable_type_integer}.
	 * @param ctx the parse tree
	 */
	void enterVariable_type_integer(Rybu4WSParser.Variable_type_integerContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#variable_type_integer}.
	 * @param ctx the parse tree
	 */
	void exitVariable_type_integer(Rybu4WSParser.Variable_type_integerContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#variable_type_enum}.
	 * @param ctx the parse tree
	 */
	void enterVariable_type_enum(Rybu4WSParser.Variable_type_enumContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#variable_type_enum}.
	 * @param ctx the parse tree
	 */
	void exitVariable_type_enum(Rybu4WSParser.Variable_type_enumContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#action_declaration}.
	 * @param ctx the parse tree
	 */
	void enterAction_declaration(Rybu4WSParser.Action_declarationContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#action_declaration}.
	 * @param ctx the parse tree
	 */
	void exitAction_declaration(Rybu4WSParser.Action_declarationContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#action_condition}.
	 * @param ctx the parse tree
	 */
	void enterAction_condition(Rybu4WSParser.Action_conditionContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#action_condition}.
	 * @param ctx the parse tree
	 */
	void exitAction_condition(Rybu4WSParser.Action_conditionContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#condition}.
	 * @param ctx the parse tree
	 */
	void enterCondition(Rybu4WSParser.ConditionContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#condition}.
	 * @param ctx the parse tree
	 */
	void exitCondition(Rybu4WSParser.ConditionContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#condition_logic_operator}.
	 * @param ctx the parse tree
	 */
	void enterCondition_logic_operator(Rybu4WSParser.Condition_logic_operatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#condition_logic_operator}.
	 * @param ctx the parse tree
	 */
	void exitCondition_logic_operator(Rybu4WSParser.Condition_logic_operatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#condition_comparison_operator}.
	 * @param ctx the parse tree
	 */
	void enterCondition_comparison_operator(Rybu4WSParser.Condition_comparison_operatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#condition_comparison_operator}.
	 * @param ctx the parse tree
	 */
	void exitCondition_comparison_operator(Rybu4WSParser.Condition_comparison_operatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#statement}.
	 * @param ctx the parse tree
	 */
	void enterStatement(Rybu4WSParser.StatementContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#statement}.
	 * @param ctx the parse tree
	 */
	void exitStatement(Rybu4WSParser.StatementContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#statement_call}.
	 * @param ctx the parse tree
	 */
	void enterStatement_call(Rybu4WSParser.Statement_callContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#statement_call}.
	 * @param ctx the parse tree
	 */
	void exitStatement_call(Rybu4WSParser.Statement_callContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#statement_match}.
	 * @param ctx the parse tree
	 */
	void enterStatement_match(Rybu4WSParser.Statement_matchContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#statement_match}.
	 * @param ctx the parse tree
	 */
	void exitStatement_match(Rybu4WSParser.Statement_matchContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#statement_match_option}.
	 * @param ctx the parse tree
	 */
	void enterStatement_match_option(Rybu4WSParser.Statement_match_optionContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#statement_match_option}.
	 * @param ctx the parse tree
	 */
	void exitStatement_match_option(Rybu4WSParser.Statement_match_optionContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#statement_state_mutation}.
	 * @param ctx the parse tree
	 */
	void enterStatement_state_mutation(Rybu4WSParser.Statement_state_mutationContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#statement_state_mutation}.
	 * @param ctx the parse tree
	 */
	void exitStatement_state_mutation(Rybu4WSParser.Statement_state_mutationContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#statement_state_mutation_operator}.
	 * @param ctx the parse tree
	 */
	void enterStatement_state_mutation_operator(Rybu4WSParser.Statement_state_mutation_operatorContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#statement_state_mutation_operator}.
	 * @param ctx the parse tree
	 */
	void exitStatement_state_mutation_operator(Rybu4WSParser.Statement_state_mutation_operatorContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#statement_return}.
	 * @param ctx the parse tree
	 */
	void enterStatement_return(Rybu4WSParser.Statement_returnContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#statement_return}.
	 * @param ctx the parse tree
	 */
	void exitStatement_return(Rybu4WSParser.Statement_returnContext ctx);
	/**
	 * Enter a parse tree produced by {@link Rybu4WSParser#enum_value}.
	 * @param ctx the parse tree
	 */
	void enterEnum_value(Rybu4WSParser.Enum_valueContext ctx);
	/**
	 * Exit a parse tree produced by {@link Rybu4WSParser#enum_value}.
	 * @param ctx the parse tree
	 */
	void exitEnum_value(Rybu4WSParser.Enum_valueContext ctx);
}