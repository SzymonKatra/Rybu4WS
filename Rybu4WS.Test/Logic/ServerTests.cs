using System;
using Xunit;
using Rybu4WS;
using System.Collections.Generic;
using FluentAssertions;
using Rybu4WS.Logic;

namespace Rybu4WS.Test
{
    public class ServerTests
    {
        [Fact]
        public void GetCartesianStates_ConditionNode_And()
        {
            var server = CreateWithVariables();

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

            var result = server.GetCartesianStates(new ConditionNode()
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
            var server = CreateWithVariables();

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

            var result = server.GetCartesianStates(new ConditionNode()
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
            var server = CreateWithVariables();

            var result = server.GetCartesianStatesLeaf(new ConditionLeaf()
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
            var server = CreateWithVariables();

            var result = server.GetCartesianStatesLeaf(new ConditionLeaf()
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

        private Server CreateWithVariables()
        {
            var server = new Server();
            server.Variables = new List<ServerVariable>()
            {
                new ServerVariable()
                {
                    Name = "int",
                    Type = VariableType.Integer,
                    AvailableValues = new List<string>() { "0", "1", "2", "3", "4", "5" }
                },
                new ServerVariable()
                {
                    Name = "enum",
                    Type = VariableType.Enum,
                    AvailableValues = new List<string>() { "first", "second", "third" }
                }
            };
            return server;
        }
    }
}
