using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rybu4WS.Logic
{
    public class Server
    {
        public string Name { get; set; }

        public List<ServerDependency> Dependencies { get; set; } = new List<ServerDependency>();

        public List<ServerVariable> Variables { get; set; } = new List<ServerVariable>();

        public List<ServerAction> Actions { get; set; } = new List<ServerAction>();


        public List<string> GetCartesianStates(ICondition condition = null)
        {
            if (condition == null) return GetCartesianStatesLeaf(null);
            else if (condition is ConditionNode) return GetCartesianStates(condition as ConditionNode);
            else if (condition is ConditionLeaf) return GetCartesianStatesLeaf(condition as ConditionLeaf);

            throw new Exception("Unsupported condition type");
        }

        public List<string> GetCartesianStates(ConditionNode conditionNode)
        {
            var leftStates = GetCartesianStates(conditionNode.Left);
            var rightStates = GetCartesianStates(conditionNode.Right);

            if (conditionNode.Operator == ConditionLogicalOperator.And) return leftStates.Intersect(rightStates).ToList();
            else if (conditionNode.Operator == ConditionLogicalOperator.Or) return leftStates.Union(rightStates).Distinct().ToList();

            throw new Exception("Unsupported condition logic operator");
        }

        public List<string> GetCartesianStatesLeaf(ConditionLeaf condition)
        {
            var states = new List<string>() { "" };

            foreach (var variable in Variables)
            {
                var newStates = new List<string>();
                foreach (var state in states)
                {
                    foreach (var value in variable.AvailableValues)
                    {
                        bool conditionSatisfied = true;
                        if (condition != null && condition.VariableName == variable.Name)
                        {
                            if (condition.VariableType != variable.Type) throw new Exception("Incorrect condition variable type");
                            conditionSatisfied = CheckCondition(condition, value);
                        }

                        if (conditionSatisfied)
                        {
                            newStates.Add($"{state}{(state != "" ? "_" : "")}{variable.Name}_{value}");
                        }
                    }
                }
                states = newStates;
            }

            return states;
        }

        private bool CheckCondition(ConditionLeaf condition, string variableValue)
        {
            if (condition == null) return true;

            if (condition.VariableType == VariableType.Integer)
            {
                var intVariableValue = int.Parse(variableValue);
                var conditionIntValue = int.Parse(condition.Value);
                
                switch (condition.Operator)
                {
                    case ConditionOperator.Equal: return intVariableValue == conditionIntValue;
                    case ConditionOperator.NotEqual: return intVariableValue != conditionIntValue;
                    case ConditionOperator.GreaterThan: return intVariableValue > conditionIntValue;
                    case ConditionOperator.GreaterOrEqualThan: return intVariableValue >= conditionIntValue;
                    case ConditionOperator.LessThan: return intVariableValue < conditionIntValue;
                    case ConditionOperator.LessOrEqualThan: return intVariableValue <= conditionIntValue;
                    default: throw new Exception("Operator not supported");
                }
            }
            else if (condition.VariableType == VariableType.Enum)
            {
                switch (condition.Operator)
                {
                    case ConditionOperator.Equal: return variableValue == condition.Value;
                    case ConditionOperator.NotEqual: return variableValue != condition.Value;
                    default: throw new Exception("Operator not supported");
                }
            }
            else
            {
                throw new Exception("Unknown VariableType");
            }
        }
    }
}
