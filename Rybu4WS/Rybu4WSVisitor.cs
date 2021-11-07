using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using Rybu4WS;
using Rybu4WS.Logic;

namespace Rybu4WS
{
    public class Rybu4WSVisitor : Rybu4WSBaseVisitor<object>
    {
        public Logic.System Result { get; private set; }

        public Rybu4WSVisitor()
        {
            Result = new Logic.System();
        }

        public override object VisitServer_declaration([NotNull] Rybu4WSParser.Server_declarationContext context)
        {
            var server = new Logic.Server() { Name = context.ID().GetText() };
            
            foreach (var item in context.server_dependency_list()?.server_dependency() ?? Enumerable.Empty<Rybu4WSParser.Server_dependencyContext>())
            {
                server.Dependencies.Add(new ServerDependency()
                {
                    Name = item.server_dependency_name().ID().GetText(),
                    Type = item.server_dependency_type().ID().GetText()
                });
            }

            foreach (var item in context.variable_declaration() ?? Enumerable.Empty<Rybu4WSParser.Variable_declarationContext>())
            {
                var variable = new ServerVariable() { Name = item.ID().GetText() };

                var contextInteger = item.variable_type_integer();
                var contextEnum = item.variable_type_enum();

                if (contextInteger != null)
                {
                    variable.Type = VariableType.Integer;
                    var minValue = int.Parse(contextInteger.variable_type_integer_min().NUMBER().GetText());
                    var maxValue = int.Parse(contextInteger.variable_type_integer_max().NUMBER().GetText());
                    for (int i = minValue; i <= maxValue; i++)
                    {
                        variable.AvailableValues.Add(i.ToString());
                    }
                }
                else if (contextEnum != null)
                {
                    variable.Type = VariableType.Enum;
                    variable.AvailableValues.AddRange(contextEnum.ID().Select(x => x.GetText()));
                }

                server.Variables.Add(variable);
            }

            foreach (var item in context.action_declaration() ?? Enumerable.Empty<Rybu4WSParser.Action_declarationContext>())
            {
                var action = new ServerAction() { Name = item.ID().GetText() };

                action.Condition = item.action_condition() != null ? BuildCondition(item.action_condition()) : null;

                foreach (var statementItem in item.statement() ?? Enumerable.Empty<Rybu4WSParser.StatementContext>())
                {
                    action.Statements.Add(BuildStatement(statementItem));
                } 

                server.Actions.Add(action);
            }

            Result.Servers.Add(server);

            return base.VisitServer_declaration(context);
        }

        public override object VisitInstance_declaration([NotNull] Rybu4WSParser.Instance_declarationContext context)
        {
            var instance = new ServerInstance()
            {
                Name = context.instance_name().ID().GetText(),
                ServerType = context.instance_server_type().ID().GetText()
            };
            
            if (context.instance_dependencies() != null)
            {
                instance.DependencyInstancesNames.AddRange(context.instance_dependencies().ID().Select(x => x.GetText()));
            }

            if (context.instance_states() != null)
            {
                foreach (var item in context.instance_states().instance_state_init())
                {
                    string varName = item.ID().GetText();
                    string value = null;
                    if (item.NUMBER() != null)
                    {
                        value = item.NUMBER().GetText();
                    }
                    else if (item.enum_value() != null)
                    {
                        value = item.enum_value().ID().GetText();
                    }
                    instance.InitialStates.Add(varName, value);
                }
            }

            Result.Instances.Add(instance);

            return base.VisitInstance_declaration(context);
        }

        public override object VisitProcess_declaration([NotNull] Rybu4WSParser.Process_declarationContext context)
        {
            var process = new Process() { Name = context.ID().GetText() };
            foreach (var item in context.statement())
            {
                process.Statements.Add(BuildStatement(item));
            }
            Result.Processes.Add(process);

            return base.VisitProcess_declaration(context);
        }

        public void FillLocation(Antlr4.Runtime.ParserRuleContext context, BaseStatement target)
        {
            target.StartLine = context.Start.Line;
            target.StartColumn = context.Start.Column;

            target.EndLine = context.Stop.Line;
            target.EndColumn = context.Stop.Column;
        }

        public BaseStatement BuildStatement(Rybu4WSParser.StatementContext statementContext)
        {
            if (statementContext.statement_call() != null)
            {
                var statementCall = new StatementCall()
                {
                    ServerName = statementContext.statement_call().call_server_name().ID().GetText(),
                    ActionName = statementContext.statement_call().call_action_name().ID().GetText(),
                };
                FillLocation(statementContext, statementCall);

                return statementCall;
            }
            else if (statementContext.statement_match() != null)
            {
                var matchContext = statementContext.statement_match();

                var statementMatch = new StatementMatch()
                {
                    ServerName = matchContext.call_server_name().ID().GetText(),
                    ActionName = matchContext.call_action_name().ID().GetText(),
                };
                FillLocation(statementContext, statementMatch);

                foreach (var matchOptionItem in matchContext.statement_match_option() ?? Enumerable.Empty<Rybu4WSParser.Statement_match_optionContext>())
                {
                    var statementMatchOption = new StatementMatchOption()
                    {
                        HandledValue = matchOptionItem.enum_value().ID().GetText() 
                    };
                    if (matchOptionItem.MATCH_SKIP() == null)
                    {
                        foreach (var handlerStatementContext in matchOptionItem.statement() ?? Enumerable.Empty<Rybu4WSParser.StatementContext>())
                        {
                            statementMatchOption.HandlerStatements.Add(BuildStatement(handlerStatementContext));
                        }
                    }
                    statementMatch.Handlers.Add(statementMatchOption);
                }

                return statementMatch;
            }
            else if (statementContext.statement_state_mutation() != null)
            {
                var mutationContext = statementContext.statement_state_mutation();

                var statementStateMutation = new StatementStateMutation()
                {
                    VariableName = mutationContext.ID().GetText(),
                    Operator = ToStateMutationOperator(mutationContext.statement_state_mutation_operator())
                };
                FillLocation(statementContext, statementStateMutation);

                if (mutationContext.NUMBER() != null)
                {
                    statementStateMutation.Value = mutationContext.NUMBER().GetText();
                }
                else if (mutationContext.enum_value() != null)
                {
                    statementStateMutation.Value = mutationContext.enum_value().ID().GetText();
                }

                return statementStateMutation;
            }
            else if (statementContext.statement_return() != null)
            {
                var statementReturn = new StatementReturn()
                {
                    Value = statementContext.statement_return().enum_value().ID().GetText()
                };
                FillLocation(statementContext, statementReturn);

                return statementReturn;
            }

            throw new Exception("Unknown statement type");
        }

        public StateMutationOperator ToStateMutationOperator(Rybu4WSParser.Statement_state_mutation_operatorContext context)
        {
            if (context.ASSIGNMENT() != null) return StateMutationOperator.Assignment;
            else if (context.OPERATOR_INCREMENT() != null) return StateMutationOperator.Increment;
            else if (context.OPERATOR_DECREMENT() != null) return StateMutationOperator.Decrement;
            
            throw new Exception("Unknown state mutation operator");
        }

        public ICondition BuildCondition(Rybu4WSParser.Action_conditionContext conditionContext)
        {
            ICondition result = null;

            var conditions = conditionContext.condition();
            var conditionOperators = conditionContext.condition_logic_operator();
            if (conditionOperators.Length != conditions.Length - 1) throw new Exception("Wrong number of logical operators in condition");
            for (int i = 0; i < conditions.Length; i++)
            {
                var condition = conditions[i];
                var leaf = new ConditionLeaf()
                {
                    VariableName = condition.ID().GetText(),
                    Operator = ToConditionOperator(condition.condition_comparison_operator()),
                };
                if (condition.condition_value().NUMBER() != null)
                {
                    leaf.Value = condition.condition_value().NUMBER().GetText();
                }
                else if (condition.condition_value().enum_value() != null)
                {
                    leaf.Value = condition.condition_value().enum_value().ID().GetText();
                }

                if (result != null)
                {
                    var conditionNode = new ConditionNode()
                    {
                        Left = result,
                        Right = leaf,
                        Operator = ToConditionLogicalOperator(conditionOperators[i - 1])
                    };
                    result = conditionNode;
                }
                else
                {
                    result = leaf;
                }
            }

            return result;
        }
        
        public ConditionOperator ToConditionOperator(Rybu4WSParser.Condition_comparison_operatorContext operatorContext)
        {
            if (operatorContext.CONDITION_EQUAL() != null) return ConditionOperator.Equal;
            else if (operatorContext.CONDITION_NOT_EQUAL() != null) return ConditionOperator.NotEqual;
            else if (operatorContext.CONDITION_GREATER_THAN() != null) return ConditionOperator.GreaterThan;
            else if (operatorContext.CONDITION_LESS_THAN() != null) return ConditionOperator.LessThan;
            else if (operatorContext.CONDITION_GREATER_OR_EQUAL_THAN() != null) return ConditionOperator.GreaterOrEqualThan;
            else if (operatorContext.CONDITION_LESS_OR_EQUAL_THAN() != null) return ConditionOperator.LessOrEqualThan;

            throw new Exception("Unknown condition operator");
        }

        public ConditionLogicalOperator ToConditionLogicalOperator(Rybu4WSParser.Condition_logic_operatorContext operatorContext)
        {
            if (operatorContext.CONDITION_AND() != null) return ConditionLogicalOperator.And;
            else if (operatorContext.CONDITION_OR() != null) return ConditionLogicalOperator.Or;

            throw new Exception("Unknown condition logical operator");
        }
    }
}
