using Calculator;

namespace TestCalculator
{
    public class TestFileCalculatorMethods
    {
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

