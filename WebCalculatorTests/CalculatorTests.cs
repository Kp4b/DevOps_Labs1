using System;
using Xunit;
using WebCalculator.Models;

namespace WebCalculator.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void ConsoleCalculate_SetOnePositiveIntegerNamber_GetSameNum()
        {
            var expected = 65;
            var testExpression = "65";
            var test = new Calculator(testExpression);

            Assert.True(expected == test.ConsoleCalculate());
        }

        [Fact]
        public void ConsoleCalculate_SetOneNegativeNamberWithPointSeparator_GetSameNum()
        {
            var expected = -34.543m;
            var testExpression = "-34.543";
            var test = new Calculator(testExpression);

            Assert.True(expected == test.ConsoleCalculate());
        }

        [Fact]
        public void ConsoleCalculate_SetExpressionOnlyPositiveIntegerNums_GetRightResult()
        {
            var testExpression = "43+54/2-3*4";
            var expected = 58;
            var test = new Calculator(testExpression);

            Assert.True(expected == test.ConsoleCalculate());
        }

        [Fact]
        public void ConsoleCalculate_SetExpresionPositiveNumsWithPointSeparator_GetRightResult()
        {
            var testExpression = "40.56+49.44";
            var expected = 90m;
            var test = new Calculator(testExpression);

            Assert.True(expected == test.ConsoleCalculate());
        }

        [Fact]
        public void ConsoleCalculate_SetExpressionAllTypeNums_GetRightResult()
        {
            var testExpression = "-34.567*-3+54.34/0.5*-4+-453";
            var expected = -784.019m;
            var test = new Calculator(testExpression);

            Assert.True(expected == test.ConsoleCalculate());
        }

        [Fact]
        public void ConsoleCalculate_SetExpressionWithDivideByZero_GetExpression()
        {
            var testExpression = "34+23/0";
            var test = new Calculator(testExpression);

            Assert.Throws<FormatException>(() => test.ConsoleCalculate());
        }

        [Fact]
        public void ConsoleCalculate_SetExpressionWithManyMinuses_GetRightResult()
        {
            var testExpression = "-3-65+-7-5+20-7"; // Failed tests, cost half a day!!!
            var expected = -67;
            var test = new Calculator(testExpression);

            Assert.True(expected == test.ConsoleCalculate());
        }

        [Fact]
        public void ConsoleCalculate_SetLineWithLetters_GetExeption()
        {
            var expected = "34+p78";
            var test = new Calculator(expected);
            Assert.Throws<FormatException>(() => test.ConsoleCalculate());
        }

        [Fact]
        public void ConsoleCalculate_SetLineWithBraces_GetExeption()
        {
            var expected = "4+(-5-8)";
            var test = new Calculator(expected);

            Assert.Throws<FormatException>(() => test.ConsoleCalculate());
        }

        [Fact]
        public void ConsoleCalculate_SetLineWithSomeOperators_GetExeption()
        {
            var expected = "5*+5";
            var test = new Calculator(expected);

            Assert.Throws<FormatException>(() => test.ConsoleCalculate());
        }

        [Fact]
        public void Constructor_SetEmptyConsoleString_GetExeption()
        {
            var expected = "";

            Assert.Throws<FormatException>(() => new Calculator(expected));
        }

        [Fact]
        public void Constructor_SetWhitespacesConsoleString_GetExeption()
        {
            var expected = "    ";

            Assert.Throws<FormatException>(() => new Calculator(expected));
        }
    }
}
