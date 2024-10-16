using Calculator;

namespace TestCalculator
{
    public class TestFileCalculatorConstructor
    {
        private const string OutFileName = "TestAnswer.txt";

        [Fact]
        public void Constructor_SetEmptyFilePath_GetExpression()
        {
            var expected = "";

            Assert.Throws<FileNotFoundException>(() => new FileCalculator(expected, OutFileName));
        }

        [Fact]
        public void Constructor_SetWrongFilePath_GetExpression()
        {
            var expected = "England - Ukraine = 4:0";

            Assert.Throws<FileNotFoundException>(() => new FileCalculator(expected, OutFileName));
        }

        [Fact]
        public void Constructor_EmptyOutFileName_ShouldThrowFormatException()
        {
            var directory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(directory, "Test.txt");
            File.WriteAllText(filePath, "1 + 1");

            Assert.Throws<FormatException>(() => new FileCalculator(filePath, ""));
            File.Delete(filePath);
        }

        [Fact]
        public void GetAnswerFilePath_ValidFile_ShouldReturnCorrectPath()
        {
            var directory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(directory, "Test.txt");
            File.WriteAllText(filePath, "1 + 1");

            var calculator = new FileCalculator(filePath, OutFileName);
            var answerFilePath = calculator.GetAnswerFilePath();

            Assert.True(File.Exists(answerFilePath));
            File.Delete(filePath);
            File.Delete(answerFilePath);
        }

        [Fact]
        public void GetAnswerArray_ValidExpressions_ShouldReturnCorrectResults()
        {
            var expressions = new[] { "1+1", "2*2", "3/3" };
            var calculator = new FileCalculator(expressions);

            var results = calculator.GetAnswerArray();

            Assert.Equal("1+1 = 2", results[0]);
            Assert.Equal("2*2 = 4", results[1]);
            Assert.Equal("3/3 = 1", results[2]);
        }

        [Fact]
        public void GetAnswerArray_InvalidExpressions_ShouldReturnErrorMessages()
        {
            var expressions = new[] { "1 +", "2 *", "3 /" };
            var calculator = new FileCalculator(expressions);

            var results = calculator.GetAnswerArray();

            Assert.Equal("1 + = expresion error.", results[0]);
            Assert.Equal("2 * = expresion error.", results[1]);
            Assert.Equal("3 / = expresion error.", results[2]);
        }
    }
}

