
namespace TestCalculator
{
    using Calculator;
    public class TestCalculatorConstructor
    {
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