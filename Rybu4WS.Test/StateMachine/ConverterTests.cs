using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Rybu4WS.StateMachine;
using Rybu4WS.Language;
using FluentAssertions;

namespace Rybu4WS.Test.StateMachine
{
    public class ConverterTests
    {
        private Converter _converter;
        private Variable _intVariable;
        private Variable _enumVariable;

        public ConverterTests()
        {
            _converter = new Converter();
            _intVariable = new Variable() { Name = "int1", Type = VariableType.Integer, AvailableValues = new List<string>() { "0", "1", "2" }, InitialValue = "0" };
            _enumVariable = new Variable() { Name = "enum1", Type = VariableType.Enum, AvailableValues = new List<string>() { "first", "second", "third" }, InitialValue = "first" };
        }

        [Fact]
        public void Mutate_EnumAssignment()
        {
            var states = CreateStates();

            var newStates = _converter.Mutate(states, new StatementStateMutation()
            {
                VariableName = "enum1",
                Operator = StateMutationOperator.Assignment,
                Value = "second"
            });

            newStates.Should().ContainSingle(x => x.Name == "int1" && x.Value == "0");
            newStates.Should().ContainSingle(x => x.Name == "enum1" && x.Value == "second");
        }

        [Fact]
        public void Mutate_EnumAssignment_InvalidValue()
        {
            var states = CreateStates();

            Action a = () => _converter.Mutate(states, new StatementStateMutation()
            {
                VariableName = "enum1",
                Operator = StateMutationOperator.Assignment,
                Value = "invalid"
            });

            a.Should().Throw<Exception>();
        }

        [Fact]
        public void Mutate_EnumIncrement()
        {
            var states = CreateStates();

            Action a = () => _converter.Mutate(states, new StatementStateMutation()
            {
                VariableName = "enum1",
                Operator = StateMutationOperator.Increment,
                Value = "1"
            });

            a.Should().Throw<Exception>();
        }

        [Fact]
        public void Mutate_EnumDecrement()
        {
            var states = CreateStates();

            Action a = () => _converter.Mutate(states, new StatementStateMutation()
            {
                VariableName = "enum1",
                Operator = StateMutationOperator.Decrement,
                Value = "1"
            });

            a.Should().Throw<Exception>();
        }

        [Fact]
        public void Mutate_IntIncrement_1()
        {
            var states = CreateStates();

            var newStates = _converter.Mutate(states, new StatementStateMutation()
            {
                VariableName = "int1",
                Operator = StateMutationOperator.Increment,
                Value = "1"
            });

            newStates.Should().ContainSingle(x => x.Name == "int1" && x.Value == "1");
            newStates.Should().ContainSingle(x => x.Name == "enum1" && x.Value == "first");
        }

        [Fact]
        public void Mutate_IntIncrement_2()
        {
            var states = CreateStates();

            var newStates = _converter.Mutate(states, new StatementStateMutation()
            {
                VariableName = "int1",
                Operator = StateMutationOperator.Increment,
                Value = "2"
            });

            newStates.Should().ContainSingle(x => x.Name == "int1" && x.Value == "2");
            newStates.Should().ContainSingle(x => x.Name == "enum1" && x.Value == "first");
        }

        [Fact]
        public void Mutate_IntIncrement_OutOfRange()
        {
            var states = CreateStates();

            Action a = () => _converter.Mutate(states, new StatementStateMutation()
            {
                VariableName = "int1",
                Operator = StateMutationOperator.Increment,
                Value = "3"
            });

            a.Should().Throw<Exception>();
        }

        [Fact]
        public void Mutate_IntDecrement_OutOfRange()
        {
            var states = CreateStates();

            Action a = () => _converter.Mutate(states, new StatementStateMutation()
            {
                VariableName = "int1",
                Operator = StateMutationOperator.Decrement,
                Value = "1"
            });

            a.Should().Throw<Exception>();
        }

        [Fact]
        public void Mutate_IntDecrement_1()
        {
            var states = CreateStates();

            var varStateIndex = states.FindIndex(x => x.Name == "int1");
            var varState = states[varStateIndex];
            varState.Value = "2";
            states[varStateIndex] = varState;

            var newStates = _converter.Mutate(states, new StatementStateMutation()
            {
                VariableName = "int1",
                Operator = StateMutationOperator.Decrement,
                Value = "1"
            });

            newStates.Should().ContainSingle(x => x.Name == "int1" && x.Value == "1");
            newStates.Should().ContainSingle(x => x.Name == "enum1" && x.Value == "first");
        }

        [Fact]
        public void Mutate_IntDecrement_2()
        {
            var states = CreateStates();

            var varStateIndex = states.FindIndex(x => x.Name == "int1");
            var varState = states[varStateIndex];
            varState.Value = "2";
            states[varStateIndex] = varState;

            var newStates = _converter.Mutate(states, new StatementStateMutation()
            {
                VariableName = "int1",
                Operator = StateMutationOperator.Decrement,
                Value = "2"
            });

            newStates.Should().ContainSingle(x => x.Name == "int1" && x.Value == "0");
            newStates.Should().ContainSingle(x => x.Name == "enum1" && x.Value == "first");
        }

        [Fact]
        public void Mutate_Assignment()
        {
            var states = CreateStates();

            var newStates = _converter.Mutate(states, new StatementStateMutation()
            {
                VariableName = "int1",
                Operator = StateMutationOperator.Assignment,
                Value = "2"
            });

            newStates.Should().ContainSingle(x => x.Name == "int1" && x.Value == "2");
            newStates.Should().ContainSingle(x => x.Name == "enum1" && x.Value == "first");
        }

        [Fact]
        public void Mutate_Assignment_OutOfRange()
        {
            var states = CreateStates();

            Action a = () => _converter.Mutate(states, new StatementStateMutation()
            {
                VariableName = "int1",
                Operator = StateMutationOperator.Assignment,
                Value = "3"
            });

            a.Should().Throw<Exception>();
        }

        [Fact]
        public void GetCartesianStates_ConditionNode_And()
        {
            var server = CreateServerWithVariables();

            var left = new ConditionLeaf()
            {
                Operator = ConditionOperator.GreaterOrEqualThan,
                VariableName = "int",
                VariableType = VariableType.Integer,
                Value = "3"
            };
            var right = new ConditionLeaf()
            {
                Operator = ConditionOperator.Equal,
                VariableName = "enum",
                VariableType = VariableType.Enum,
                Value = "second"
            };

            var result = _converter.GetCartesianStates(server.Variables, new ConditionNode()
            {
                Left = left,
                Operator = ConditionLogicalOperator.And,
                Right = right
            });

            result.Should().BeEquivalentTo(new List<List<StatePair>>
            {
                new List<StatePair>() { new StatePair("int", "3"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "4"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "5"), new StatePair("enum", "second") },
            });
        }

        [Fact]
        public void GetCartesianStates_ConditionNode_Or()
        {
            var server = CreateServerWithVariables();

            var left = new ConditionLeaf()
            {
                Operator = ConditionOperator.GreaterOrEqualThan,
                VariableName = "int",
                VariableType = VariableType.Integer,
                Value = "3"
            };
            var right = new ConditionLeaf()
            {
                Operator = ConditionOperator.Equal,
                VariableName = "enum",
                VariableType = VariableType.Enum,
                Value = "second"
            };

            var result = _converter.GetCartesianStates(server.Variables, new ConditionNode()
            {
                Left = left,
                Operator = ConditionLogicalOperator.Or,
                Right = right
            });

            result.Should().BeEquivalentTo(new List<List<StatePair>>
            {
                new List<StatePair>() { new StatePair("int", "0"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "1"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "2"), new StatePair("enum", "second") },

                new List<StatePair>() { new StatePair("int", "3"), new StatePair("enum", "first") },
                new List<StatePair>() { new StatePair("int", "3"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "3"), new StatePair("enum", "third") },

                new List<StatePair>() { new StatePair("int", "4"), new StatePair("enum", "first") },
                new List<StatePair>() { new StatePair("int", "4"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "4"), new StatePair("enum", "third") },

                new List<StatePair>() { new StatePair("int", "5"), new StatePair("enum", "first") },
                new List<StatePair>() { new StatePair("int", "5"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "5"), new StatePair("enum", "third") },
            });
        }

        [Fact]
        public void GetCartesianStates_ConditionLeaf_Int()
        {
            var server = CreateServerWithVariables();

            var result = _converter.GetCartesianStatesLeaf(server.Variables, new ConditionLeaf()
            {
                Operator = ConditionOperator.GreaterOrEqualThan,
                VariableName = "int",
                VariableType = VariableType.Integer,
                Value = "3"
            });

            result.Should().BeEquivalentTo(new List<List<StatePair>>
            {
                new List<StatePair>() { new StatePair("int", "3"), new StatePair("enum", "first") },
                new List<StatePair>() { new StatePair("int", "3"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "3"), new StatePair("enum", "third") },

                new List<StatePair>() { new StatePair("int", "4"), new StatePair("enum", "first") },
                new List<StatePair>() { new StatePair("int", "4"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "4"), new StatePair("enum", "third") },

                new List<StatePair>() { new StatePair("int", "5"), new StatePair("enum", "first") },
                new List<StatePair>() { new StatePair("int", "5"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "5"), new StatePair("enum", "third") },
            });
        }

        [Fact]
        public void GetCartesianStates_ConditionLeaf_Enum()
        {
            var server = CreateServerWithVariables();

            var result = _converter.GetCartesianStatesLeaf(server.Variables, new ConditionLeaf()
            {
                Operator = ConditionOperator.Equal,
                VariableName = "enum",
                VariableType = VariableType.Enum,
                Value = "second"
            });

            result.Should().BeEquivalentTo(new List<List<StatePair>>
            {
                new List<StatePair>() { new StatePair("int", "0"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "1"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "2"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "3"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "4"), new StatePair("enum", "second") },
                new List<StatePair>() { new StatePair("int", "5"), new StatePair("enum", "second") },
            });
        }

        [Fact]
        public void IsConditionSatisfied_True_ConditionLeaf_Integer()
        {
            var server = CreateServerWithVariables();

            var states = new List<StatePair>()
            {
                new StatePair(server.Variables.Single(x => x.Name == "int")) { Value = "2"},
                new StatePair(server.Variables.Single(x => x.Name == "enum"))
            };

            var conditionLeaf = new ConditionLeaf()
            {
                VariableType = VariableType.Integer,
                VariableName = "int",
                Operator = ConditionOperator.GreaterThan,
                Value = "1",
            };
            var result = _converter.IsConditionSatisfied(conditionLeaf, states);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsConditionSatisfied_False_ConditionLeaf_Integer()
        {
            var server = CreateServerWithVariables();

            var states = new List<StatePair>()
            {
                new StatePair(server.Variables.Single(x => x.Name == "int")) { Value = "0"},
                new StatePair(server.Variables.Single(x => x.Name == "enum"))
            };

            var conditionLeaf = new ConditionLeaf()
            {
                VariableType = VariableType.Integer,
                VariableName = "int",
                Operator = ConditionOperator.GreaterThan,
                Value = "1",
            };
            var result = _converter.IsConditionSatisfied(conditionLeaf, states);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsConditionSatisfied_True_ConditionLeaf_Enum()
        {
            var server = CreateServerWithVariables();

            var states = new List<StatePair>()
            {
                new StatePair(server.Variables.Single(x => x.Name == "int")),
                new StatePair(server.Variables.Single(x => x.Name == "enum")) { Value = "first" }
            };

            var conditionLeaf = new ConditionLeaf()
            {
                VariableType = VariableType.Enum,
                VariableName = "enum",
                Operator = ConditionOperator.Equal,
                Value = "first",
            };
            var result = _converter.IsConditionSatisfied(conditionLeaf, states);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsConditionSatisfied_False_ConditionLeaf_Enum()
        {
            var server = CreateServerWithVariables();

            var states = new List<StatePair>()
            {
                new StatePair(server.Variables.Single(x => x.Name == "int")),
                new StatePair(server.Variables.Single(x => x.Name == "enum")) { Value = "second" }
            };

            var conditionLeaf = new ConditionLeaf()
            {
                VariableType = VariableType.Enum,
                VariableName = "enum",
                Operator = ConditionOperator.Equal,
                Value = "first",
            };
            var result = _converter.IsConditionSatisfied(conditionLeaf, states);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsConditionSatisfied_True_ConditionNode_And()
        {
            var server = CreateServerWithVariables();

            var states = new List<StatePair>()
            {
                new StatePair(server.Variables.Single(x => x.Name == "int")) { Value = "2" },
                new StatePair(server.Variables.Single(x => x.Name == "enum")) { Value = "first" }
            };

            var conditionNode = new ConditionNode()
            {
                Left = new ConditionLeaf()
                {
                    VariableType = VariableType.Integer,
                    VariableName = "int",
                    Operator = ConditionOperator.GreaterThan,
                    Value = "1",
                },
                Operator = ConditionLogicalOperator.And,
                Right = new ConditionLeaf()
                {
                    VariableType = VariableType.Enum,
                    VariableName = "enum",
                    Operator = ConditionOperator.Equal,
                    Value = "first",
                }
            };

            var result = _converter.IsConditionSatisfied(conditionNode, states);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsConditionSatisfied_False_ConditionNode_And()
        {
            var server = CreateServerWithVariables();

            var states = new List<StatePair>()
            {
                new StatePair(server.Variables.Single(x => x.Name == "int")) { Value = "2" },
                new StatePair(server.Variables.Single(x => x.Name == "enum")) { Value = "second" }
            };

            var conditionNode = new ConditionNode()
            {
                Left = new ConditionLeaf()
                {
                    VariableType = VariableType.Integer,
                    VariableName = "int",
                    Operator = ConditionOperator.GreaterThan,
                    Value = "1",
                },
                Operator = ConditionLogicalOperator.And,
                Right = new ConditionLeaf()
                {
                    VariableType = VariableType.Enum,
                    VariableName = "enum",
                    Operator = ConditionOperator.Equal,
                    Value = "first",
                }
            };

            var result = _converter.IsConditionSatisfied(conditionNode, states);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsConditionSatisfied_True_ConditionNode_Or()
        {
            var server = CreateServerWithVariables();

            var states = new List<StatePair>()
            {
                new StatePair(server.Variables.Single(x => x.Name == "int")) { Value = "2" },
                new StatePair(server.Variables.Single(x => x.Name == "enum")) { Value = "second" }
            };

            var conditionNode = new ConditionNode()
            {
                Left = new ConditionLeaf()
                {
                    VariableType = VariableType.Integer,
                    VariableName = "int",
                    Operator = ConditionOperator.GreaterThan,
                    Value = "1",
                },
                Operator = ConditionLogicalOperator.Or,
                Right = new ConditionLeaf()
                {
                    VariableType = VariableType.Enum,
                    VariableName = "enum",
                    Operator = ConditionOperator.Equal,
                    Value = "first",
                }
            };

            var result = _converter.IsConditionSatisfied(conditionNode, states);

            result.Should().BeTrue();
        }

        [Fact]
        public void IsConditionSatisfied_False_ConditionNode_Or()
        {
            var server = CreateServerWithVariables();

            var states = new List<StatePair>()
            {
                new StatePair(server.Variables.Single(x => x.Name == "int")) { Value = "0" },
                new StatePair(server.Variables.Single(x => x.Name == "enum")) { Value = "second" }
            };

            var conditionNode = new ConditionNode()
            {
                Left = new ConditionLeaf()
                {
                    VariableType = VariableType.Integer,
                    VariableName = "int",
                    Operator = ConditionOperator.GreaterThan,
                    Value = "1",
                },
                Operator = ConditionLogicalOperator.Or,
                Right = new ConditionLeaf()
                {
                    VariableType = VariableType.Enum,
                    VariableName = "enum",
                    Operator = ConditionOperator.Equal,
                    Value = "first",
                }
            };

            var result = _converter.IsConditionSatisfied(conditionNode, states);

            result.Should().BeFalse();
        }

        [Fact]
        public void IsConditionSatisfied_ConditionNot()
        {
            var server = CreateServerWithVariables();

            var states = new List<StatePair>()
            {
                new StatePair(server.Variables.Single(x => x.Name == "int")) { Value = "0" },
                new StatePair(server.Variables.Single(x => x.Name == "enum")) { Value = "second" }
            };

            var conditionNode = new ConditionNode()
            {
                Left = new ConditionLeaf()
                {
                    VariableType = VariableType.Integer,
                    VariableName = "int",
                    Operator = ConditionOperator.GreaterThan,
                    Value = "1",
                },
                Operator = ConditionLogicalOperator.Or,
                Right = new ConditionLeaf()
                {
                    VariableType = VariableType.Enum,
                    VariableName = "enum",
                    Operator = ConditionOperator.Equal,
                    Value = "first",
                }
            };
            var conditionNot = new ConditionNot()
            {
                Condition = conditionNode
            };

            var result = _converter.IsConditionSatisfied(conditionNot, states);

            result.Should().BeTrue();
        }

        [Fact]
        public void Combinations()
        {
            var graphs = new List<Graph>()
            {
                new Graph(),
                new Graph(),
                new Graph()
            };

            graphs[0].Nodes.Add(new Node() { Caller = "11" });
            graphs[0].Nodes.Add(new Node() { Caller = "12" });
            graphs[0].Nodes.Add(new Node() { Caller = "13" });

            graphs[1].Nodes.Add(new Node() { Caller = "21" });
            graphs[1].Nodes.Add(new Node() { Caller = "22" });

            graphs[2].Nodes.Add(new Node() { Caller = "31" });
            graphs[2].Nodes.Add(new Node() { Caller = "32" });
            graphs[2].Nodes.Add(new Node() { Caller = "33" });

            var combinations = _converter
                .GetNodeCombinations(graphs)
                .Select(x => string.Join("", x.Select(n => n.Caller)))
                .ToList();
            combinations.Should().HaveCount(3 * 2 * 3);

            int i = 0;
            combinations[i++].Should().Be("112131");
            combinations[i++].Should().Be("112132");
            combinations[i++].Should().Be("112133");

            combinations[i++].Should().Be("112231");
            combinations[i++].Should().Be("112232");
            combinations[i++].Should().Be("112233");

            //

            combinations[i++].Should().Be("122131");
            combinations[i++].Should().Be("122132");
            combinations[i++].Should().Be("122133");

            combinations[i++].Should().Be("122231");
            combinations[i++].Should().Be("122232");
            combinations[i++].Should().Be("122233");

            //

            combinations[i++].Should().Be("132131");
            combinations[i++].Should().Be("132132");
            combinations[i++].Should().Be("132133");

            combinations[i++].Should().Be("132231");
            combinations[i++].Should().Be("132232");
            combinations[i++].Should().Be("132233");
        }

        private Server CreateServerWithVariables()
        {
            var server = new Server();
            server.Variables = new List<Variable>()
            {
                new Variable()
                {
                    Name = "int",
                    Type = VariableType.Integer,
                    AvailableValues = new List<string>() { "0", "1", "2", "3", "4", "5" }
                },
                new Variable()
                {
                    Name = "enum",
                    Type = VariableType.Enum,
                    AvailableValues = new List<string>() { "first", "second", "third" }
                }
            };
            return server;
        }

        private List<StatePair> CreateStates()
        {
            return new List<StatePair>()
            {
                new StatePair(_intVariable),
                new StatePair(_enumVariable)
            };
        }
    }
}
