using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Rybu4WS.StateMachine;
using Rybu4WS.Logic;
using FluentAssertions;

namespace Rybu4WS.Test.StateMachine
{
    public class ConverterTests
    {
        private Converter _converter;
        private ServerVariable _intVariable;
        private ServerVariable _enumVariable;

        public ConverterTests()
        {
            _converter = new Converter();
            _intVariable = new ServerVariable() { Name = "int1", Type = VariableType.Integer, AvailableValues = new List<string>() { "0", "1", "2" }, InitialValue = "0" };
            _enumVariable = new ServerVariable() { Name = "enum1", Type = VariableType.Enum, AvailableValues = new List<string>() { "first", "second", "third" }, InitialValue = "first" };
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
