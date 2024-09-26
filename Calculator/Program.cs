using System;
using System.IO;
using System.Configuration;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = true;
            var input = new ConsoleKeyInfo();

            Console.WriteLine("Welcome! This program has two modes:");

            while (result)
            {
                var consoleMode = false;
                var fileMode = false;
                var exit = false;

                while (!consoleMode & !fileMode & !exit)
                {
                    Console.WriteLine("For console calculation without parentheses press 1");
                    Console.WriteLine("For file calculation press 2");
                    Console.WriteLine("For exit program press <WhiteSpace>...\n");

                    input = Console.ReadKey(true);

                    consoleMode = input.Key == ConsoleKey.D1 || input.Key == ConsoleKey.NumPad1;
                    fileMode = input.Key == ConsoleKey.D2 || input.Key == ConsoleKey.NumPad2;
                    exit = input.Key == ConsoleKey.Spacebar;

                    if (!(consoleMode || fileMode || exit))
                        Console.WriteLine($"Key {input.Key} is bad choiсe,try againe...\n");
                }
                if (exit)
                {
                    result = Repeat();
                }
                else
                {
                    try
                    {
                        if (consoleMode)
                        {
                            Console.WriteLine("You chose console mode.");
                            Console.WriteLine("Write math expression and press <Enter>...");

                            var expression = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(expression))
                                throw new FormatException("Empty line.");

                            var consoleExp = new Calculator(expression);
                            Console.WriteLine($"Answer : {consoleExp.ConsoleCalculate()}".Replace(',', '.'));
                        }
                        else if (fileMode)
                        {
                            Console.WriteLine("You chose file mode.");
                            Console.WriteLine("Write file path and press <Enter>...");

                            var filePath = Console.ReadLine();

                            if (string.IsNullOrWhiteSpace(filePath))
                                throw new FormatException("Empty file path.");

                            var file = new FileCalculator(filePath, GetConfig("OutFileName"));
                            Console.WriteLine($"Calculated file path : {file.GetAnswerFilePath()}");
                        }
                    }
                    catch (ConfigurationErrorsException)
                    {
                        Console.WriteLine("Have configuration error.");
                        Console.ReadKey();
                        break;
                    }
                    catch (FileNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    result = Repeat();
                }
            }
        }

        private static bool Repeat()
        {
            Console.WriteLine("Try again? Y/N :");
            return Console.ReadLine().ToUpper() == "Y";
        }

        private static string GetConfig(string key)
        {
            var val = ConfigurationManager.AppSettings[key];
            return string.IsNullOrEmpty(val)
                   ? throw new ConfigurationErrorsException()
                   : val;
        }
    }
}