﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Rybu4WS;
using Rybu4WS.Language;

namespace Rybu4WS.Language.Parser
{
    public class Rybu4WSVisitor : Rybu4WSBaseVisitor<object>
    {
        public Language.System Result { get; private set; }

        private TextWriter _errorTextWriter;

        public Rybu4WSVisitor(TextWriter errorTextWriter)
        {
            Result = new Language.System();
            _errorTextWriter = errorTextWriter;
        }

        public override object VisitServer_declaration([NotNull] Rybu4WSParser.Server_declarationContext context)
        {
            var serverDeclaration = new Language.ServerDeclaration() { TypeName = context.ID().GetText() };
            FillLocation(context, serverDeclaration);

            if (context.server_dependency_list() != null)
            {
                foreach (var item in context.server_dependency_list().server_dependency() ?? Enumerable.Empty<Rybu4WSParser.Server_dependencyContext>())
                {
                    var serverDependency = new ServerDependency()
                    {
                        Name = item.server_dependency_name().ID().GetText(),
                        Type = item.server_dependency_type().ID().GetText()
                    };

                    serverDeclaration.Dependencies.Add(serverDependency);
                }
            }

            if (context.server_implemented_interfaces() != null)
            {
                foreach (var item in context.server_implemented_interfaces().ID() ?? Enumerable.Empty<Antlr4.Runtime.Tree.ITerminalNode>())
                {
                    serverDeclaration.ImplementedInterfaces.Add(new ServerImplementedInterface()
                    {
                        InterfaceTypeName = item.GetText()
                    });
                }
            }

            foreach (var variableCtx in context.variable_declaration() ?? Enumerable.Empty<Rybu4WSParser.Variable_declarationContext>())
            {
                var variable = new Variable() { Name = variableCtx.ID().GetText() };

                var contextInteger = variableCtx.variable_type_integer();
                var contextEnum = variableCtx.variable_type_enum();

                if (contextInteger != null)
                {
                    variable.Type = VariableType.Integer;
                    
                    int minValue, maxValue;

                    if (contextInteger.variable_type_integer_min().ID() != null)
                    {
                        var minConstName = contextInteger.variable_type_integer_min().ID().GetText();
                        if (!GetConstValue(minConstName, null, contextInteger, out minValue)) continue;
                    }
                    else
                    {
                        minValue = int.Parse(contextInteger.variable_type_integer_min().NUMBER().GetText());
                    }

                    if (contextInteger.variable_type_integer_max().ID() != null)
                    {
                        var maxConstName = contextInteger.variable_type_integer_max().ID().GetText();
                        if (!GetConstValue(maxConstName, null, contextInteger, out maxValue)) continue;
                    }
                    else
                    {
                        maxValue = int.Parse(contextInteger.variable_type_integer_max().NUMBER().GetText());
                    }

                    if (minValue > maxValue)
                    {
                        WriteError(contextInteger, $"Integer range minimum value ({minValue}) cannot be grater than range maximum value ({maxValue})");
                        continue;
                    }

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

                if (variableCtx.variable_declaration_array() != null)
                {
                    int arraySize = 0;
                    if (variableCtx.variable_declaration_array().NUMBER() != null)
                    {
                        arraySize = int.Parse(variableCtx.variable_declaration_array().NUMBER().GetText());
                    }
                    else if (variableCtx.variable_declaration_array().ID() != null)
                    {
                        if (!GetConstValue(variableCtx.variable_declaration_array().ID().GetText(), null, variableCtx, out arraySize))
                        {
                            continue;
                        }
                    }

                    for (int i = 0; i < arraySize; i++)
                    {
                        serverDeclaration.Variables.Add(new Variable()
                        {
                            Name = GetIndexedName(variable.Name, i),
                            Type = variable.Type,
                            AvailableValues = new List<string>(variable.AvailableValues),
                            InitialValue = variable.InitialValue
                        });
                    }
                }
                else
                {
                    serverDeclaration.Variables.Add(variable);
                }
            }

            foreach (var item in context.action_declaration() ?? Enumerable.Empty<Rybu4WSParser.Action_declarationContext>())
            {
                var actionName = item.ID().GetText();
                var action = serverDeclaration.Actions.SingleOrDefault(x => x.Name == actionName);
                if (action == null)
                {
                    action = new ServerAction() { Name = actionName };
                    serverDeclaration.Actions.Add(action);
                }

                var actionBranch = new ServerActionBranch();

                actionBranch.Condition = item.action_condition() != null ? BuildCondition(item.action_condition().condition_list()) : null;

                foreach (var statementItem in item.statement() ?? Enumerable.Empty<Rybu4WSParser.StatementContext>())
                {
                    actionBranch.Statements.Add(BuildStatement(statementItem));
                }

                action.Branches.Add(actionBranch);
            }

            Result.ServerDeclarations.Add(serverDeclaration);

            return base.VisitServer_declaration(context);
        }

        private string GetIndexedName(string name, int? arrayIndex = null)
        {
            if (arrayIndex.HasValue)
            {
                name += arrayIndex.Value.ToString().PadLeft(4, '0');
            }

            return name;
        }

        public override object VisitProcess_declaration([NotNull] Rybu4WSParser.Process_declarationContext context)
        {
            var process = BuildProcess(context.process());
            Result.Processes.Add(process);

            return base.VisitProcess_declaration(context);
        }

        public override object VisitGroup_declaration([NotNull] Rybu4WSParser.Group_declarationContext context)
        {
            var group = new Group() { Name = context.ID().GetText() };
            foreach (var variableCtx in context.variable_declaration_with_value() ?? Enumerable.Empty<Rybu4WSParser.Variable_declaration_with_valueContext>())
            {
                var variable = new Variable() { Name = variableCtx.ID().GetText() };

                var contextInteger = variableCtx.variable_type_integer();
                var contextEnum = variableCtx.variable_type_enum();

                if (contextInteger != null)
                {
                    variable.Type = VariableType.Integer;
                    int minValue = 0;
                    int maxValue = 0;

                    if (contextInteger.variable_type_integer_min().ID() != null)
                    {
                        var minConstName = contextInteger.variable_type_integer_min().ID().GetText();
                        if (!GetConstValue(minConstName, null, variableCtx, out minValue)) continue;
                    }
                    else
                    {
                        minValue = int.Parse(contextInteger.variable_type_integer_min().NUMBER().GetText());
                    }

                    if (contextInteger.variable_type_integer_max().ID() != null)
                    {
                        var maxConstName = contextInteger.variable_type_integer_max().ID().GetText();
                        if (!GetConstValue(maxConstName, null, variableCtx, out maxValue)) continue;
                    }
                    else
                    {
                        maxValue = int.Parse(contextInteger.variable_type_integer_max().NUMBER().GetText());
                    }

                    if (minValue > maxValue)
                    {
                        WriteError(variableCtx, $"Integer range minimum value ({minValue}) cannot be grater than range maximum value ({maxValue})");
                        continue;
                    }

                    for (int i = minValue; i <= maxValue; i++)
                    {
                        variable.AvailableValues.Add(i.ToString());
                    }
                    if (variableCtx.variable_value().NUMBER() == null)
                    {
                        WriteError(variableCtx, "Integer initial value for this variable must be defined");
                        continue;
                    }
                    variable.InitialValue = variableCtx.variable_value().NUMBER().GetText();

                }
                else if (contextEnum != null)
                {
                    variable.Type = VariableType.Enum;
                    variable.AvailableValues.AddRange(contextEnum.ID().Select(x => x.GetText()));
                    if (variableCtx.variable_value().enum_value() == null)
                    {
                        WriteError(variableCtx, "Enum initial value for this variable must be defined");
                        continue;
                    }
                    variable.InitialValue = variableCtx.variable_value().enum_value().ID().GetText();
                }

                if (variableCtx.variable_declaration_array() != null)
                {
                    int arraySize = 0;
                    if (variableCtx.variable_declaration_array().NUMBER() != null)
                    {
                        arraySize = int.Parse(variableCtx.variable_declaration_array().NUMBER().GetText());
                    }
                    else if (variableCtx.variable_declaration_array().ID() != null)
                    {
                        if (!GetConstValue(variableCtx.variable_declaration_array().ID().GetText(), null, variableCtx, out arraySize))
                        {
                            continue;
                        }
                    }

                    for (int i = 0; i < arraySize; i++)
                    {
                        group.Variables.Add(new Variable()
                        {
                            Name = GetIndexedName(variable.Name, i),
                            Type = variable.Type,
                            AvailableValues = new List<string>(variable.AvailableValues),
                            InitialValue = variable.InitialValue
                        });
                    }
                }
                else
                {
                    group.Variables.Add(variable);
                }
            }
            foreach (var processCtx in context.process() ?? Enumerable.Empty<Rybu4WSParser.ProcessContext>())
            {
                group.Processes.Add(BuildProcess(processCtx));
            }
            Result.Groups.Add(group);

            return base.VisitGroup_declaration(context);
        }

        private bool GetConstValue(string name, Dictionary<string, int> indexerContext, ParserRuleContext onErrorContext, out int result)
        {
            result = 0;
            var constDecl = Result.ConstDeclarations.FirstOrDefault(x => x.Name == name);
            if (constDecl != null)
            {
                result = constDecl.Value;
                return true;
            }
            if (indexerContext != null && indexerContext.TryGetValue(name, out result))
            {
                return true;
            }

            WriteError(onErrorContext, $"Cannot find const or indexer named {name}");
            return false;
        }

        private Process BuildProcess(Rybu4WSParser.ProcessContext context)
        {
            var process = new Process() { Name = context.ID().GetText() };
            foreach (var item in context.statement())
            {
                process.Statements.Add(BuildStatement(item));
            }
            return process;
        }

        public override object VisitServer_definition([NotNull] Rybu4WSParser.Server_definitionContext context)
        {
            var serverDefinition = new ServerDefinition()
            {
                Name = context.server_definition_name().ID().GetText(),
                Type = context.server_definition_type().ID().GetText()
            };
            FillLocation(context, serverDefinition);

            if (context.server_definition_dependencies() != null)
            {
                foreach (var dependencyName in context.server_definition_dependencies().ID() ?? Enumerable.Empty<Antlr4.Runtime.Tree.ITerminalNode>())
                {
                    serverDefinition.DependencyNameList.Add(dependencyName.GetText());
                }
            }
            
            if (context.server_definition_variable_list() != null)
            {
                foreach (var variable in context.server_definition_variable_list().server_definition_variable() ?? Enumerable.Empty<Rybu4WSParser.Server_definition_variableContext>())
                {
                    var varName = variable.server_definition_variable_name().ID().GetText();

                    var varValue = variable.server_definition_variable_value().NUMBER()?.GetText() ??
                        variable.server_definition_variable_value().enum_value().ID().GetText();

                    serverDefinition.VariablesInitialValues.Add(varName, varValue);
                }
            }

            Result.ServerDefinitions.Add(serverDefinition);

            return base.VisitServer_definition(context);
        }

        public override object VisitInterface_declaration([NotNull] Rybu4WSParser.Interface_declarationContext context)
        {
            var interfaceDeclaration = new InterfaceDeclaration()
            {
                TypeName = context.ID().GetText()
            };

            foreach (var item in context.interface_action() ?? Enumerable.Empty<Rybu4WSParser.Interface_actionContext>())
            {
                var interfaceAction = new InterfaceAction()
                {
                    Name = item.ID().GetText()
                };

                if (item.interface_action_possible_return_values() != null)
                {
                    foreach (var possibleRetVal in item.interface_action_possible_return_values().enum_value() ?? Enumerable.Empty<Rybu4WSParser.Enum_valueContext>())
                    {
                        interfaceAction.PossibleReturnValues.Add(possibleRetVal.ID().GetText());
                    }
                }

                interfaceDeclaration.RequiredActions.Add(interfaceAction);
            }

            Result.InterfaceDeclarations.Add(interfaceDeclaration);

            return base.VisitInterface_declaration(context);
        }

        public override object VisitConst_declaration([NotNull] Rybu4WSParser.Const_declarationContext context)
        {
            var constDeclaration = new ConstDeclaration()
            {
                Name = context.ID().GetText(),
                Value = int.Parse(context.NUMBER().GetText())
            };

            Result.ConstDeclarations.Add(constDeclaration);

            return base.VisitConst_declaration(context);
        }

        public void FillLocation(Antlr4.Runtime.ParserRuleContext context, IWithCodeLocation target)
        {
            target.CodeLocation = new CodeLocation()
            {
                StartLine = context.Start.Line,
                StartColumn = context.Start.Column,
                EndLine = context.Stop.Line,
                EndColumn = context.Stop.Column,
                StartIndex = context.Start.StartIndex,
                EndIndex = context.Stop.StopIndex
            };
        }

        public void FillPostLocation(Antlr4.Runtime.ParserRuleContext context, BaseStatement target)
        {
            target.PostCodeLocation = new CodeLocation()
            {
                StartLine = context.Stop.Line,
                StartColumn = context.Stop.Column,
                EndLine = context.Stop.Line,
                EndColumn = context.Stop.Column,
                StartIndex = context.Stop.StopIndex,
                EndIndex = context.Stop.StopIndex
            };
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
                var matchCallContext = matchContext.statement_match_call();

                var statementMatch = new StatementMatch()
                {
                    ServerName = matchCallContext.call_server_name().ID().GetText(),
                    ActionName = matchCallContext.call_action_name().ID().GetText(),
                };
                FillLocation(matchCallContext, statementMatch);
                FillPostLocation(matchContext, statementMatch);

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

                if (mutationContext.statement_state_mutation_value().NUMBER() != null)
                {
                    statementStateMutation.Value = mutationContext.statement_state_mutation_value().NUMBER().GetText();
                }
                else if (mutationContext.statement_state_mutation_value().enum_value() != null)
                {
                    statementStateMutation.Value = mutationContext.statement_state_mutation_value().enum_value().ID().GetText();
                }
                else if (mutationContext.statement_state_mutation_value().ID() != null)
                {
                    if (GetConstValue(mutationContext.statement_state_mutation_value().ID().GetText(), mutationContext, out var result))
                    {
                        statementStateMutation.Value = result.ToString();
                    }
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
            else if (statementContext.statement_terminate() != null)
            {
                var statementTerminate = new StatementTerminate();
                FillLocation(statementContext, statementTerminate);

                return statementTerminate;
            }
            else if (statementContext.statement_loop() != null)
            {
                var loopContext = statementContext.statement_loop();

                var statementLoop = new StatementLoop();
                FillLocation(loopContext.statement_loop_identifier(), statementLoop);
                FillPostLocation(loopContext, statementLoop);

                foreach (var item in loopContext.statement() ?? Enumerable.Empty<Rybu4WSParser.StatementContext>())
                {
                    statementLoop.LoopStatements.Add(BuildStatement(item));
                }

                return statementLoop;
            }
            else if (statementContext.statement_wait() != null)
            {
                var waitContext = statementContext.statement_wait();
                var statementWait = new StatementWait()
                {
                    Condition = BuildCondition(waitContext.condition_list())
                };
                FillLocation(waitContext, statementWait);

                return statementWait;
            }
            else if (statementContext.statement_if() != null)
            {
                var ifContext = statementContext.statement_if();
                var statementIf = new StatementIf()
                {
                    Condition = BuildCondition(ifContext.statement_if_header().condition_list())
                };
                FillLocation(ifContext.statement_if_header(), statementIf);
                FillPostLocation(ifContext, statementIf);

                foreach (var item in ifContext.statement() ?? Enumerable.Empty<Rybu4WSParser.StatementContext>())
                {
                    statementIf.ConditionStatements.Add(BuildStatement(item));
                }

                return statementIf;
            }

            throw new NotImplementedException(BuildMessage(statementContext, "Unknown statement type"));
        }

        public StateMutationOperator ToStateMutationOperator(Rybu4WSParser.Statement_state_mutation_operatorContext context)
        {
            if (context.ASSIGNMENT() != null) return StateMutationOperator.Assignment;
            else if (context.OPERATOR_INCREMENT() != null) return StateMutationOperator.Increment;
            else if (context.OPERATOR_DECREMENT() != null) return StateMutationOperator.Decrement;
            else if (context.OPERATOR_MODULO() != null) return StateMutationOperator.Modulo;

            throw new NotImplementedException(BuildMessage(context, "Unknown state mutation operator"));
        }

        public ICondition BuildCondition(Rybu4WSParser.Condition_listContext conditionContext)
        {
            ICondition result = null;

            var conditions = conditionContext.condition();
            var conditionOperators = conditionContext.condition_logic_operator();
            if (conditionOperators.Length != conditions.Length - 1)
            {
                throw new Exception(BuildMessage(conditionContext, "Condition incorrectly formatted - wrong number of logical operators in condition."));
            }
            for (int i = 0; i < conditions.Length; i++)
            {
                var condition = conditions[i];
                var leaf = new ConditionLeaf()
                {
                    VariableName = condition.ID().GetText(),
                    Operator = ToConditionOperator(condition.condition_comparison_operator()),
                };
                FillLocation(condition, leaf);
                if (condition.condition_value().NUMBER() != null)
                {
                    leaf.Value = condition.condition_value().NUMBER().GetText();
                    leaf.VariableType = VariableType.Integer;
                }
                else if (condition.condition_value().enum_value() != null)
                {
                    leaf.Value = condition.condition_value().enum_value().ID().GetText();
                    leaf.VariableType = VariableType.Enum;
                }
                else if (condition.condition_value().ID() != null)
                {
                    if (!GetConstValue(condition.condition_value().ID().GetText(), conditionContext, out var constValue)) continue;
                    leaf.Value = constValue.ToString();
                    leaf.VariableType = VariableType.Integer;
                }

                if (leaf.VariableType == VariableType.Enum && leaf.Operator != ConditionOperator.Equal && leaf.Operator != ConditionOperator.NotEqual)
                {
                    WriteError(conditionContext, "Enum variables must be compared with = or != only");
                    continue;
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
            else if (operatorContext.RCHEVRON() != null) return ConditionOperator.GreaterThan;
            else if (operatorContext.LCHEVRON() != null) return ConditionOperator.LessThan;
            else if (operatorContext.CONDITION_GREATER_OR_EQUAL_THAN() != null) return ConditionOperator.GreaterOrEqualThan;
            else if (operatorContext.CONDITION_LESS_OR_EQUAL_THAN() != null) return ConditionOperator.LessOrEqualThan;

            throw new NotImplementedException(BuildMessage(operatorContext, "Unknown condition operator"));
        }

        public ConditionLogicalOperator ToConditionLogicalOperator(Rybu4WSParser.Condition_logic_operatorContext operatorContext)
        {
            if (operatorContext.CONDITION_AND() != null) return ConditionLogicalOperator.And;
            else if (operatorContext.CONDITION_OR() != null) return ConditionLogicalOperator.Or;

            throw new NotImplementedException(BuildMessage(operatorContext, "Unknown condition logical operator"));

        }

        private void WriteError(ParserRuleContext context, string message)
        {
            _errorTextWriter.WriteLine(BuildMessage(context, message));
        }

        private string BuildMessage(ParserRuleContext context, string message)
        {
            return $"VISITOR ERROR AT (Start {context.Start.Line}:{context.Start.Column}, Stop: {context.Stop.Line}:{context.Stop.Line}) - {message}";
        }
    }
}
