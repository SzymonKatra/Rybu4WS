using System;
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
                    
                    if (!GetMinMaxRangeValues(contextInteger, out int minValue, out int maxValue)) continue;

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

                actionBranch.Condition = item.action_condition() != null ? BuildCondition(item.action_condition().condition_list(), null) : null;

                foreach (var statementItem in item.statement() ?? Enumerable.Empty<Rybu4WSParser.StatementContext>())
                {
                    actionBranch.Statements.Add(BuildStatement(statementItem, null));
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
            Result.Processes.AddRange(BuildProcesses(context.process()));

            return base.VisitProcess_declaration(context);
        }

        private bool GetMinMaxRangeValues(Rybu4WSParser.Variable_type_integerContext context, out int minValue, out int maxValue)
        {
            minValue = 0;
            maxValue = 0;

            if (context.variable_type_integer_min().ID() != null)
            {
                var minConstName = context.variable_type_integer_min().ID().GetText();
                if (!GetConstValue(minConstName, null, context, out minValue))
                {
                    return false;
                }
            }
            else
            {
                minValue = int.Parse(context.variable_type_integer_min().NUMBER().GetText());
            }

            if (context.variable_type_integer_max().ID() != null)
            {
                var maxConstName = context.variable_type_integer_max().ID().GetText();
                if (!GetConstValue(maxConstName, null, context, out maxValue))
                {
                    return false;
                }
            }
            else
            {
                maxValue = int.Parse(context.variable_type_integer_max().NUMBER().GetText());
            }

            if (minValue > maxValue)
            {
                WriteError(context, $"Integer range minimum value ({minValue}) cannot be grater than range maximum value ({maxValue})");
                return false;
            }

            return true;
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
                    if (!GetMinMaxRangeValues(contextInteger, out int minValue, out int maxValue)) continue;

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

                    // ARRAYS NUMBERED FROM 1 !!!!
                    for (int i = 1; i <= arraySize; i++)
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
                group.Processes.AddRange(BuildProcesses(processCtx));
            }
            Result.Groups.Add(group);

            return base.VisitGroup_declaration(context);
        }

        private bool GetArrayAccess(Rybu4WSParser.Array_accessContext arrayAccessContext, IReadOnlyDictionary<string, int> indexerContext, out int result)
        {
            if (arrayAccessContext.NUMBER() != null)
            {
                result = int.Parse(arrayAccessContext.NUMBER().GetText());
                return true;
            }
            else if (arrayAccessContext.ID() != null)
            {
                return GetConstValue(arrayAccessContext.ID().GetText(), indexerContext, arrayAccessContext, out result);
            }

            throw new NotImplementedException();
        }

        private bool GetArrayRange(Rybu4WSParser.Array_rangeContext arrayRangeContext, IReadOnlyDictionary<string, int> indexerContext, out int minValue, out int maxValue)
        {
            minValue = 0;
            maxValue = 0;

            var minContext = arrayRangeContext.array_range_min();
            if (minContext.NUMBER() != null)
            {
                minValue = int.Parse(minContext.NUMBER().GetText());
            }
            else if (minContext.ID() != null)
            {
                return GetConstValue(minContext.ID().GetText(), null, arrayRangeContext, out minValue);
            }

            var maxContext = arrayRangeContext.array_range_max();
            if (maxContext.NUMBER() != null)
            {
                maxValue = int.Parse(maxContext.NUMBER().GetText());
            }
            else if (maxContext.ID() != null)
            {
                return GetConstValue(maxContext.ID().GetText(), null, arrayRangeContext, out maxValue);
            }

            return true;
        }

        private bool GetConstValue(string name, IReadOnlyDictionary<string, int> indexerContext, ParserRuleContext onErrorContext, out int result)
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

        private IEnumerable<Process> BuildProcesses(Rybu4WSParser.ProcessContext context)
        {
            if (context.process_indexer() != null)
            {
                var indexerCtx = context.process_indexer();
                string indexerName = indexerCtx.ID().GetText();
                if (!GetMinMaxRangeValues(indexerCtx.variable_type_integer(), out int minValue, out int maxValue))
                {
                    yield break;
                }

                for (int i = minValue; i <= maxValue; i++)
                {
                    var indexerContext = new Dictionary<string, int>();
                    indexerContext.Add(indexerName, i);
                    var process = new Process() { Name = GetIndexedName(context.ID().GetText(), i) };
                    foreach (var item in context.statement())
                    {
                        process.Statements.Add(BuildStatement(item, indexerContext));
                    }
                    yield return process;
                }
            }
            else
            {
                var process = new Process() { Name = context.ID().GetText() };
                foreach (var item in context.statement())
                {
                    process.Statements.Add(BuildStatement(item, null));
                }
                yield return process;
            }
        }

        public override object VisitServer_definition([NotNull] Rybu4WSParser.Server_definitionContext context)
        {
            var serverName = context.server_definition_name().ID().GetText();

            if (context.array_access() != null)
            {
                if (GetArrayAccess(context.array_access(), null, out int index))
                {
                    Result.ServerDefinitions.Add(CreateServerDefinition(GetIndexedName(serverName, index), context));

                }
            }
            else if (context.array_range() != null)
            {
                if (GetArrayRange(context.array_range(), null, out int minValue, out int maxValue))
                {
                    for (int i = minValue; i <= maxValue; i++)
                    {
                        Result.ServerDefinitions.Add(CreateServerDefinition(GetIndexedName(serverName, i), context));
                    }
                }
            }
            else
            {
                Result.ServerDefinitions.Add(CreateServerDefinition(serverName, context));
            }

            return base.VisitServer_definition(context);
        }

        private ServerDefinition CreateServerDefinition(string name, Rybu4WSParser.Server_definitionContext context)
        {
            var serverDefinition = new ServerDefinition()
            {
                Name = name,
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
                foreach (var variableCtx in context.server_definition_variable_list().server_definition_variable() ?? Enumerable.Empty<Rybu4WSParser.Server_definition_variableContext>())
                {
                    var varName = variableCtx.server_definition_variable_name().ID().GetText();
                    string varValue = null;
                    if (variableCtx.server_definition_variable_value().NUMBER() != null)
                    {
                        varValue = variableCtx.server_definition_variable_value().NUMBER().GetText();
                    }
                    else if (variableCtx.server_definition_variable_value().enum_value() != null)
                    {
                        varValue = variableCtx.server_definition_variable_value().enum_value().ID().GetText();
                    }
                    else if (variableCtx.server_definition_variable_value().ID() != null)
                    {
                        if (GetConstValue(variableCtx.server_definition_variable_value().ID().GetText(), null, context, out var intValue))
                        {
                            varValue = intValue.ToString();
                        }
                    }

                    if (variableCtx.array_access() != null)
                    {
                        if (!GetArrayAccess(variableCtx.array_access(), null, out var index)) continue;
                        serverDefinition.VariablesInitialValues[GetIndexedName(varName, index)] = varValue;
                    }
                    else if (variableCtx.array_range() != null)
                    {
                        if (!GetArrayRange(variableCtx.array_range(), null, out int minValue, out int maxValue)) continue;

                        for (int i = minValue; i <= maxValue; i++)
                        {
                            serverDefinition.VariablesInitialValues[GetIndexedName(varName, i)] = varValue;
                        }
                    }
                    else
                    {
                        serverDefinition.VariablesInitialValues[varName] = varValue;
                    }
                }
            }

            return serverDefinition;
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

        public BaseStatement BuildStatement(Rybu4WSParser.StatementContext statementContext, IReadOnlyDictionary<string, int> indexerContext)
        {
            if (statementContext.statement_call() != null)
            {
                var serverName = statementContext.statement_call().call_server_name().ID().GetText();

                if (statementContext.statement_call().array_access() != null)
                {
                    if (GetArrayAccess(statementContext.statement_call().array_access(), indexerContext, out int index))
                    {
                        serverName = GetIndexedName(serverName, index);
                    }
                }

                var statementCall = new StatementCall()
                {
                    ServerName = serverName,
                    ActionName = statementContext.statement_call().call_action_name().ID().GetText(),
                };
                FillLocation(statementContext, statementCall);

                return statementCall;
            }
            else if (statementContext.statement_match() != null)
            {
                var matchContext = statementContext.statement_match();
                var matchCallContext = matchContext.statement_match_call();

                var serverName = matchCallContext.call_server_name().ID().GetText();

                if (matchCallContext.array_access() != null)
                {
                    if (GetArrayAccess(matchCallContext.array_access(), indexerContext, out int index))
                    {
                        serverName = GetIndexedName(serverName, index);
                    }
                }

                var statementMatch = new StatementMatch()
                {
                    ServerName = serverName,
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
                            statementMatchOption.HandlerStatements.Add(BuildStatement(handlerStatementContext, indexerContext));
                        }
                    }
                    statementMatch.Handlers.Add(statementMatchOption);
                }

                return statementMatch;
            }
            else if (statementContext.statement_state_mutation() != null)
            {
                var mutationContext = statementContext.statement_state_mutation();
                var variableName = mutationContext.ID().GetText();
                if (mutationContext.array_access() != null)
                {
                    if (!GetArrayAccess(mutationContext.array_access(), indexerContext, out var indexValue))
                    {
                        return null;
                    }
                    variableName = GetIndexedName(variableName, indexValue);
                }

                var statementStateMutation = new StatementStateMutation()
                {
                    VariableName = variableName,
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
                    if (GetConstValue(mutationContext.statement_state_mutation_value().ID().GetText(), indexerContext, mutationContext, out var result))
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
                    statementLoop.LoopStatements.Add(BuildStatement(item, indexerContext));
                }

                return statementLoop;
            }
            else if (statementContext.statement_wait() != null)
            {
                var waitContext = statementContext.statement_wait();
                var statementWait = new StatementWait()
                {
                    Condition = BuildCondition(waitContext.condition_list(), indexerContext)
                };
                FillLocation(waitContext, statementWait);

                return statementWait;
            }
            else if (statementContext.statement_if() != null)
            {
                var ifContext = statementContext.statement_if();
                var statementIf = new StatementIf()
                {
                    Condition = BuildCondition(ifContext.statement_if_header().condition_list(), indexerContext)
                };
                FillLocation(ifContext.statement_if_header(), statementIf);
                FillPostLocation(ifContext, statementIf);

                foreach (var item in ifContext.statement() ?? Enumerable.Empty<Rybu4WSParser.StatementContext>())
                {
                    statementIf.ConditionStatements.Add(BuildStatement(item, indexerContext));
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

        public ICondition BuildCondition(Rybu4WSParser.Condition_listContext conditionListContext, IReadOnlyDictionary<string, int> indexerContext)
        {
            ICondition result = null;

            var conditions = conditionListContext.condition();
            var conditionOperators = conditionListContext.condition_logic_operator();
            if (conditionOperators.Length != conditions.Length - 1)
            {
                throw new Exception(BuildMessage(conditionListContext, "Condition incorrectly formatted - wrong number of logical operators in condition."));
            }
            for (int i = 0; i < conditions.Length; i++)
            {
                var conditionCtx = conditions[i];
                var variableName = conditionCtx.ID().GetText();
                if (conditionCtx.array_access() != null)
                {
                    if (!GetArrayAccess(conditionCtx.array_access(), indexerContext, out var arrayIndex))
                    {
                        return null;
                    }
                    variableName = GetIndexedName(variableName, arrayIndex);
                }
                var leaf = new ConditionLeaf()
                {
                    VariableName = variableName,
                    Operator = ToConditionOperator(conditionCtx.condition_comparison_operator()),
                };
                FillLocation(conditionCtx, leaf);
                if (conditionCtx.condition_value().NUMBER() != null)
                {
                    leaf.Value = conditionCtx.condition_value().NUMBER().GetText();
                    leaf.VariableType = VariableType.Integer;
                }
                else if (conditionCtx.condition_value().enum_value() != null)
                {
                    leaf.Value = conditionCtx.condition_value().enum_value().ID().GetText();
                    leaf.VariableType = VariableType.Enum;
                }
                else if (conditionCtx.condition_value().ID() != null)
                {
                    if (!GetConstValue(conditionCtx.condition_value().ID().GetText(), indexerContext, conditionCtx, out var constValue)) continue;
                    leaf.Value = constValue.ToString();
                    leaf.VariableType = VariableType.Integer;
                }

                if (leaf.VariableType == VariableType.Enum && leaf.Operator != ConditionOperator.Equal && leaf.Operator != ConditionOperator.NotEqual)
                {
                    WriteError(conditionCtx, "Enum variables must be compared with = or != only");
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
