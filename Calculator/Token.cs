using Calculator.Interfaces;
using System;

namespace Calculator
{
    public struct Token : IOperationPriority, IMathOperation
    {
        public decimal Number;
        public Operation Operator;

        public bool IsNumber { get { return Operator == 0; } }

        public decimal PerformOperation(decimal firstValue, decimal secondValue, Operation op)
        {
            switch (op)
            {
                case Operation.Add:
                    return firstValue + secondValue;
                case Operation.Subtract:
                    return firstValue - secondValue;
                case Operation.Multiply:
                    return firstValue * secondValue;
                case Operation.Divide:
                    return secondValue != 0 ? firstValue / secondValue : throw new FormatException("Can't divide by zero!");
                default:
                    throw new FormatException("Invalid operation!");
            }
        }

        public Priority GetPriority(Operation op)
        {
            switch (op)
            {
                case Operation.Add:
                case Operation.Subtract:
                    return Priority.Low;
                case Operation.Multiply:
                case Operation.Divide:
                    return Priority.High;
                default:
                    return Priority.None;
            }
        }
    }
}
