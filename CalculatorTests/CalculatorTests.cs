using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Calculator
{
    
    [TestClass()]
    public class CalculatorTests
    {

        [TestMethod]
        public void CalculateTesteCorrectValue()
        {
            var expression = "1+6/(2+4)";
            var expect = 2;

            Calculator calculator = new Calculator(expression);

            Assert.AreEqual(expect, calculator.Result);
        }

        [TestMethod]
        public void CalculateTesteCorrectValue2()
        {
            var expression = "1.2+5*(-2--4)";
            var expect = 11.2m;

            Calculator calculator = new Calculator(expression);

            Assert.AreEqual(expect, calculator.Result);
        }
        [TestMethod]
        public void CalculateTesteCorrectValue3()
        {
            var expression = "1,2+5*(-2--4)";
            var expect = 11.2m;

            Calculator calculator = new Calculator(expression);

            Assert.AreEqual(expect, calculator.Result);
        }

        [TestMethod]
        public void CalculateTesteCorrectValue4()
        {
            var expression = "2+-4";
            var expect = -2;

            Calculator calculator = new Calculator(expression);

            Assert.AreEqual(expect, calculator.Result);
        }

        [TestMethod]
        public void CalculateTesteNegativeValue()
        {
            var expression = "-1-2*(-2-4)";
            var expect = 11;

            Calculator calculator = new Calculator(expression);

            Assert.AreEqual(expect, calculator.Result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateTesteIncorrectValue()
        {
            Calculator calculator = new Calculator("5-a");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CalculateTesteIncorrectValue2()
        {
            Calculator calculator = new Calculator(")3+4");
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void CalculateTesteDividedByZero()
        {
            Calculator calculator = new Calculator("3/0");
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void CalculateTesteDividedByZero2()
        {
            Calculator calculator = new Calculator("-5/(2-2)");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateTesteNull()
        {
            string s = null;
            Calculator calculator = new Calculator(s);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CalculateTesteEmptyString()
        {
            string s = "";
            Calculator calculator = new Calculator(s);
        }

    }
}