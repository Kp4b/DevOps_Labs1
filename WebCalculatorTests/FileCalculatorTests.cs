using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCalculator.Models;

namespace WebCalculatorTests
{
    public class FileCalculatorTests
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

        [Fact]
        public void ArrayCalculate_SetArrayAllTypeStrings_GetArrayWithMathResultAndErrorsLines()
        {
            string[] testArray = {"45*2+55",
                                 "",
                                 "  34.54+87",
                                 "34+54/0",
                                 "some text",
                                 "-(34+43)/3.5+(34.54-20)",
                                 "43.54+-54",
                                 "45/(54+23))",
                                 "43-13+100/-0.5*(1-0.5)",
                                 "56*()",
                                 "-(-43*((34.5+87.9)/4*(-43.45/0.4+65-(76/2+54))))"};
            string[] expected = {"45*2+55 = 145",
                                 " = expresion error.",
                                 "  34.54+87 = expresion error.",
                                 "34+54/0 = expresion error.",
                                 "some text = expresion error.",
                                 "-(34+43)/3.5+(34.54-20) = -7.46",
                                 "43.54+-54 = expresion error.",
                                 "45/(54+23)) = expresion error.",
                                 "43-13+100/-0.5*(1-0.5) = -70.0",
                                 "56*() = expresion error.",
                                 "-(-43*((34.5+87.9)/4*(-43.45/0.4+65-(76/2+54)))) = -178455.3750"};
            var test = new FileCalculator(testArray);

            Assert.Equal(expected, test.GetAnswerArray());
        }

        [Fact]
        public void GetAnswerFilePath_SetEmptyStringArray_GetExpression()
        {
            var expected = Array.Empty<string>();
            var test = new FileCalculator(expected);

            Assert.Throws<FormatException>(() => test.GetAnswerFilePath());
        }
    }
}
