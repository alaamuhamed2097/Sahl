namespace ConsoleAppMSTest
{
    [TestClass]
    public sealed class CalculatorMsTests
    {
        [TestMethod]
        public void AddNumbers_ReturnsSum()
        {
            // Arrange
            var calculator = new ConsoleApp1.Calculator();
            int a = 7;
            int b = 8;

            // Act
            int result = calculator.Add(a, b);

            // Assert
            Assert.AreEqual(result, 15); // Use Assert.That instead of Assert.AreEqual
        }

        [TestMethod]
        public void SubtractNumbers_ReturnsDifference()
        {
            // Arrange
            var calculator = new ConsoleApp1.Calculator();
            int a = 10;
            int b = 4;

            // Act
            int result = calculator.Subtract(a, b);

            // Assert
            Assert.AreEqual(result, 6);
        }

        [TestMethod]
        public void MultiplyNumbers_ReturnsDifference()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 10;
            int b = 4;
            int result = calculator.Multiply(a, b);
            Assert.AreEqual(result, 40);
        }

        [TestMethod]
        public void DivideNumbers_ReturnsQuotient()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 20;
            int b = 4;
            int result = calculator.Divide(a, b);
            Assert.AreEqual(result, 5);
        }

        [TestMethod]
        public void DivideByZero_ThrowsException()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 10;
            int b = 0;
            Assert.Throws<DivideByZeroException>(() => calculator.Divide(a, b));
        }

        [TestMethod]
        public void IsEven_ReturnsTrueForEvenNumber()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = 6;
            bool result = calculator.IsEven(number);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsOdd_ReturnsTrueForOddNumber()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = 7;
            bool result = calculator.IsOdd(number);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Square_ReturnsSquaredValue()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = 5;
            int result = calculator.Square(number);
            Assert.AreEqual(result, 25);
        }

        [TestMethod]
        public void SquareOfNegativeNumber_ReturnsPositiveSquaredValue()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = -4;
            int result = calculator.Square(number);
            Assert.AreEqual(result, 16);
        }

        [TestMethod]
        public void Add_NegativeAndPositiveNumber_ReturnsCorrectSum()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = -3;
            int b = 7;
            int result = calculator.Add(a, b);
            Assert.AreEqual(result, 4);
        }

        [TestMethod]
        public void Subtract_LargerFromSmallerNumber_ReturnsNegativeResult()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 5;
            int b = 10;
            int result = calculator.Subtract(a, b);
            Assert.AreEqual(result, -5);
        }

        [TestMethod]
        public void Multiply_ByZero_ReturnsZero()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 15;
            int b = 0;
            int result = calculator.Multiply(a, b);
            Assert.AreEqual(result, 0);
        }

        [TestMethod]
        public void Divide_NegativeNumbers_ReturnsPositiveQuotient()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = -20;
            int b = -4;
            int result = calculator.Divide(a, b);
            Assert.AreEqual(result, 5);
        }

        [TestMethod]
        public void IsEven_ReturnsFalseForOddNumber()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = 9;
            bool result = calculator.IsEven(number);
            Assert.IsFalse(result);
        }
    }
}
