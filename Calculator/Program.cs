using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Calculator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("Choose mod \n1: input expressions from a keyboard \n2: expressions from a file \n3: if you want to exit\nmod: ");
                string mod = Console.ReadLine();

                if (mod == "1")
                {
                    KeyboardMod();
                }
                if (mod == "2")
                {
                    FileMod();
                }
                if (mod == "3")
                {
                    break;
                }
            }
        }

        private static bool CheckCorrect(string inputString)
        {
            int openBracket = 0, closeBracket = 0;
            string correctSymbols;

            inputString = Convert(inputString);

            if (CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator[0] == '.')
            {
                correctSymbols = "+-*/.()";
            }
            else
            {
                correctSymbols = "+-*/,()";
            }

            if (inputString == null)
            {
                return false;
            }
            if (inputString.Length == 0 || inputString.Length == 1)
            {
                return false;
            }
            if (inputString[0] == ')' || inputString[inputString.Length - 1] == '(')
            {
                return false;
            }

            for (int i = 0; i < inputString.Length; i++)
            {
                if (!char.IsDigit(inputString[i]) && !correctSymbols.Contains(inputString[i]))
                {
                    return false;
                }
                if (inputString[i] == '(')
                {
                    openBracket++;
                }
                if (inputString[i] == ')')
                {
                    closeBracket++;
                }
            }

            if (openBracket != closeBracket)
            {
                return false;
            }

            return true;
        }

        private static void KeyboardMod()
        {
            while (true)
            {
                Console.WriteLine("Enter the sentence. if you want exit put ENTER");
                string input = Console.ReadLine();

                if (input.Length == 0)
                {
                    Console.WriteLine("The application finishing its work");
                    break;
                }
                Console.WriteLine(Calculate(input));
            }
        }

        private static void FileMod(string path)
        {
            List<string> expressions = new List<string>();

            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        expressions.Add(line);
                    }
                }

                for (int i = 0; i < expressions.Count; i++)
                {
                    expressions[i] = $"{Calculate(expressions[i])}";
                }

                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    foreach (var item in expressions)
                    {
                        sw.WriteLine(item);
                    }
                }

                Console.WriteLine($"\n{path}");
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("file doesn't exist");
            }
        }

        private static void FileMod()
        {
            Console.WriteLine("Enter the path to file");
            FileMod(Console.ReadLine());
        }

        private static string Calculate(string input)
        {
            Calculator calculator;

            if (CheckCorrect(input))
            {
                try
                {
                    calculator = new Calculator(input);
                    return $"{input} = {calculator.Result}";
                }
                catch (DivideByZeroException)
                {
                    return $"{input} = divided by zero";
                }
                catch (ArgumentOutOfRangeException)
                {
                    return $"{input} = incorrect expression";
                }
            }
            else
            {
                return $"{input} = the expression has a mistake";
            }
        }

        private static string Convert(string inputString)
        {
            inputString = inputString.Replace(',', CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator[0]);
            inputString = inputString.Replace('.', CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator[0]);
            return inputString;
        }
    }
}
