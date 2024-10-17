using System;
using System.IO;
using System.Linq;

namespace WebCalculator
{
    public class FileCalculator : Calculator
    {
        private readonly string _answerFilePath;
        private readonly string[] _expressions;

        public FileCalculator(string filePath, string outFileName)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found.");

            if (string.IsNullOrWhiteSpace(outFileName))
                throw new FormatException("Set name for answer file!");

            _answerFilePath = $"{Path.GetDirectoryName(filePath)}/{outFileName}";
            _expressions = File.ReadAllLines(filePath);
        }

        public FileCalculator(string[] test)
        {
            _expressions = test;
        }

        public string[] GetAnswerArray() => ArrayCalculate(_expressions);

        public string GetAnswerFilePath()
        {
            if (_expressions == null || !_expressions.Any())
                throw new FormatException("File is empty.");

            var answerArray = ArrayCalculate(_expressions);
            File.WriteAllLines(_answerFilePath, answerArray);

            return _answerFilePath;
        }

        private string[] ArrayCalculate(string[] fileArray)
        {
            var length = fileArray.Length;
            var answerArray = new string[length];

            for (var i = 0; i < length; i++)
            {
                var line = fileArray[i];
                try
                {
                    if (string.IsNullOrWhiteSpace(line))
                        throw new FormatException();
                    Func<Operation, bool> unaryMinusCheck = symbol =>
                    {
                        return symbol == Operation.OpenBrace || symbol == Operation.Multiply || symbol == Operation.Divide;
                    };

                    ParseLine(line, unaryMinusCheck);
                    answerArray[i] = $"{line} = {Calculate().ToString().Replace(',', '.')}";
                }
                catch (FormatException)
                {
                    answerArray[i] = $"{line} = expresion error.";
                    continue;
                }
            }
            return answerArray;
        }
    }
}
