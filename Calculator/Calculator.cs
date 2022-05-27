using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Calculator
{
    public class Calculator
    {
        public decimal Result { get; }
       
        private Dictionary<char, byte> signs = new Dictionary<char, byte> { {'(',1 }, {'+',2 }, { '-', 2 }, { '*', 3 }, { '/', 3 }, { ')', 4 } };
        
        private readonly List<string> _listInputString = new List<string>();
        private readonly List<string> _stackNumbers = new List<string>();
        private readonly List<string> _stackSigns = new List<string>();
        private readonly List<string> _reverseString = new List<string>();
        private readonly List<decimal> _resultStack = new List<decimal>();

        public Calculator(string inputString)
        {
            if (inputString == null)
            {
                throw new ArgumentNullException("the input string is null"); 
            }
            if (inputString == "")
            {
                throw new ArgumentException("the input string is empty");
            }

            inputString = ConvertInputString(inputString);
            InputStringToList(inputString);
            ConvertListString();
            CreateReverseString();
            Result = CalculateReverseString();
        }

        private decimal CalculateReverseString()
        {
            int i = 0;

            foreach (var item in _reverseString)
            {
                if (decimal.TryParse(item, out decimal a))
                {
                    _resultStack.Add(a);
                    i++;
                }
                if (item == "+")
                { 
                    _resultStack[i-2] += _resultStack[i-1];
                    _resultStack.RemoveAt(i-1);
                    i--;
                }
                if (item == "-")
                {
                    _resultStack[i - 2] -= _resultStack[i - 1];
                    _resultStack.RemoveAt(i - 1);
                    i--;
                }
                if (item == "*")
                {
                    _resultStack[i - 2] *= _resultStack[i - 1];
                    _resultStack.RemoveAt(i - 1);
                    i--;
                }
                if (item == "/")
                {
                    _resultStack[i - 2] /= _resultStack[i - 1];
                    _resultStack.RemoveAt(i - 1);
                    i--;
                }
            }
            return _resultStack[0];
        }

        private void CreateReverseString()
        {
            decimal digit;

            foreach (var item in _listInputString)
            {
                if (decimal.TryParse(item, NumberStyles.Float, CultureInfo.InvariantCulture, out digit) && CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator == ".")
                {
                    _stackNumbers.Add(Convert.ToString(digit));
                }
                else if (decimal.TryParse(item, NumberStyles.Float, CultureInfo.InvariantCulture, out digit) && CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator == ",")
                {
                    _stackNumbers.Add(Convert.ToString(digit));
                }
                else
                {
                    if (_stackSigns.Count == 0)
                    {
                        _stackSigns.Add(item);
                        continue;
                    }
                    if (item == ")")
                    {
                        for (int i = _stackSigns.Count - 1; _stackSigns[i] != "("; i--)
                        {
                            _stackNumbers.Add(_stackSigns[i]);
                            _stackSigns.RemoveAt(i);
                        }
                        _stackSigns.RemoveAt(_stackSigns.Count - 1);
                        continue;
                    }
                    if (GetPriority(_stackSigns[_stackSigns.Count - 1]) < GetPriority(item) || item == "(")
                    {
                        _stackSigns.Add(item);
                    }
                    else
                    {
                        _stackNumbers.Add(item);
                    }
                }
            }

            _stackSigns.Reverse();

            foreach (var item in _stackNumbers)
            {
                _reverseString.Add(item);
            }
            foreach (var item in _stackSigns)
            {
                _reverseString.Add(item);
            }
        }

        private void InputStringToList(string inputString)
        {
            string digit;

            for (int i = 0; i < inputString.Length; i++)
            {
                if (char.IsDigit(inputString[i]))
                {
                    digit = TakeDigit(inputString, i);
                    _listInputString.Add(digit);
                    i += digit.Length - 1;
                }
                else if (signs.ContainsKey(inputString[i]))
                {
                    _listInputString.Add(inputString[i].ToString());
                }
                else
                {
                    throw new ArgumentException("there is a mistake in the sentence " + inputString);
                }
            }
        }

        private string TakeDigit(string inputString, int position)
        {
            var result = new StringBuilder();

            result.Append(inputString[position]);

            if (position == inputString.Length)
            { 
                return result.ToString();
            }

            for (int i = position+1; i < inputString.Length; i++)
            {
                if (!char.IsDigit(inputString[i]) && inputString[i] != '.')
                {
                    return result.ToString();
                }
                result.Append(inputString[i]);
            }

            return result.ToString();
        }

        private byte GetPriority(string sign)
        {
            return signs[sign[0]];
        }

        private void ConvertListString()
        {
            string correctSymbols = "+-*/()";

            if (_listInputString[0] == "-" && decimal.TryParse(_listInputString[1], out _))
            {
                _listInputString[1] = "-" + _listInputString[1];
                _listInputString.RemoveAt(0);
            }

            for (int i = 1; i < _listInputString.Count-1; i++)
            {
                if (correctSymbols.Contains(_listInputString[i-1]) && _listInputString[i] == "-" && decimal.TryParse(_listInputString[i + 1], out _))
                {
                    _listInputString[i + 1] = "-" + _listInputString[i + 1];
                    _listInputString.RemoveAt(i);
                }
                
            }
        }

        private string ConvertInputString(string inputString)
        {
            inputString = inputString.Replace(',', CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator[0]);
            inputString = inputString.Replace('.', CultureInfo.InvariantCulture.NumberFormat.NumberDecimalSeparator[0]);
            return inputString;
        }
    }
}
