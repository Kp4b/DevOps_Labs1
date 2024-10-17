using System;
using System.Globalization;
using System.Collections.Generic;


namespace WebCalculator
{
    public class Calculator
    {
        private const char PointSeparator = '.';
        private const char UnaryMinus = '-';
        private readonly string _consoleExpression;
        private readonly List<Token> _tokens = new List<Token>();

        public Calculator() { }

        public Calculator(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new FormatException("Wrong input!");

            _consoleExpression = input;
        }

        public decimal ConsoleCalculate()
        {

            Func<Operation, bool> unaryMinusCheck = symbol =>
            {
                return symbol == Operation.Add || symbol == Operation.Multiply || symbol == Operation.Divide;
            };
            ParseLine(_consoleExpression, unaryMinusCheck);
            return Calculate();
        }

        protected decimal Calculate()
        {
            var numbers = new Stack<decimal>();
            var operators = new Stack<Operation>();

            for (var i = 0; i < _tokens.Count; i++)
            {
                var token = _tokens[i];

                if (token.IsNumber)
                {
                    numbers.Push(token.Number);

                    if (operators.Count == 0 || (new Token().GetPriority(operators.Peek()) != Priority.High
                                             & operators.Peek() != Operation.UnaryMinus))
                        continue;
                    else
                        while (operators.Count != 0 && (operators.Peek() == Operation.UnaryMinus
                                                       || new Token().GetPriority(operators.Peek()) == Priority.High))
                            numbers.Push(StackCalc(numbers, operators));
                }
                else if (token.Operator != Operation.CloseBrace)
                    operators.Push(token.Operator);
                else
                {
                    while (operators.Count != 0 && operators.Peek() != Operation.OpenBrace)
                    {
                        numbers.Push(StackCalc(numbers, operators));
                    }
                    if (operators.Count != 0 && operators.Peek() == Operation.OpenBrace)
                    {
                        operators.Pop();
                        if (operators.Count != 0 & operators.Peek() == Operation.UnaryMinus)
                            numbers.Push(StackCalc(numbers, operators));
                    }
                    else
                        throw new FormatException("Wrong line!");
                }
            }
            while (operators.Count != 0)
            {
                if (operators.Contains(Operation.OpenBrace))
                    throw new FormatException("Wrong line!");
                else
                    numbers.Push(StackCalc(numbers, operators));
            }
            return numbers.Pop();
        }

        protected void ParseLine(string line, Func<Operation, bool> unaryMinusControl)
        {
            var lineLen = line.Length;
            _tokens.Clear();

            for (var i = 0; i < lineLen; i++)
            {
                if (char.IsDigit(line[i]))
                {
                    var temporalyNum = string.Empty;

                    while (char.IsDigit(line[i]) || (line[i].Equals(PointSeparator) & !temporalyNum.Contains(PointSeparator)))
                    {
                        temporalyNum += line[i];
                        i++;
                        if (i == lineLen) break;
                    }
                    _tokens.Add(new Token { Number = decimal.Parse(temporalyNum, CultureInfo.InvariantCulture) });
                    i--;
                }
                else if (line[i] == UnaryMinus & i < lineLen
                                               & (i == 0 || unaryMinusControl((Operation)line[i - 1]))
                                               && (char.IsDigit(line[i + 1]) || ((Operation)line[i + 1] == Operation.OpenBrace)))
                    _tokens.Add(new Token { Operator = Operation.UnaryMinus });
                else if (Enum.IsDefined(typeof(Operation), (Operation)line[i]))
                    _tokens.Add(new Token { Operator = (Operation)line[i] });
                else throw new FormatException("Wrong line format!");
            }
        }

        private decimal StackCalc(Stack<decimal> numStack, Stack<Operation> opStack)
        {
            if (numStack.Count != 0 & opStack.Count != 0 & opStack.Peek() == Operation.UnaryMinus)
            {
                opStack.Pop();
                return -numStack.Pop();
            }
            else if (numStack.Count > 1 & opStack.Count != 0)
            {
                var secondVal = numStack.Pop();
                var firstVal = numStack.Pop();
                var operation = opStack.Pop();

                if (opStack.Count != 0 && opStack.Peek() == Operation.Subtract)
                {
                    firstVal = -firstVal;
                    opStack.Pop();
                    opStack.Push(Operation.Add);
                }
                return new Token().PerformOperation(firstVal, secondVal, operation);
            }
            else
                throw new FormatException("Wrong line!");
        }
    }
}
