namespace ConsoleAppNUnitTest
{
    [TestFixture]
    public class CalculatorNUnitTests
    {
        [Test]
        public void AddNumbers_ReturnsSum()
        {
            // Arrange
            var calculator = new ConsoleApp1.Calculator();
            int a = 7;
            int b = 8;

            // Act
            int result = calculator.Add(a, b);

            // Assert
            Assert.That(result, Is.EqualTo(15)); // Use Assert.That instead of Assert.AreEqual
        }

        [Test]
        public void SubtractNumbers_ReturnsDifference()
        {
            // Arrange
            var calculator = new ConsoleApp1.Calculator();
            int a = 10;
            int b = 4;

            // Act
            int result = calculator.Subtract(a, b);

            // Assert
            Assert.That(result, Is.EqualTo(6));
        }

        [Test]
        public void MultiplyNumbers_ReturnsDifference()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 10;
            int b = 4;
            int result = calculator.Multiply(a, b);
            Assert.That(result, Is.EqualTo(40));
        }

        [Test]
        public void DivideNumbers_ReturnsQuotient()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 20;
            int b = 4;
            int result = calculator.Divide(a, b);
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void DivideByZero_ThrowsException()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 10;
            int b = 0;
            Assert.Throws<DivideByZeroException>(() => calculator.Divide(a, b));
        }

        [Test]
        public void IsEven_ReturnsTrueForEvenNumber()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = 6;
            bool result = calculator.IsEven(number);
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsOdd_ReturnsTrueForOddNumber()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = 7;
            bool result = calculator.IsOdd(number);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Square_ReturnsSquaredValue()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = 5;
            int result = calculator.Square(number);
            Assert.That(result, Is.EqualTo(25));
        }

        [Test]
        public void SquareOfNegativeNumber_ReturnsPositiveSquaredValue()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = -4;
            int result = calculator.Square(number);
            Assert.That(result, Is.EqualTo(16));
        }

        [Test]
        public void Add_NegativeAndPositiveNumber_ReturnsCorrectSum()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = -3;
            int b = 7;
            int result = calculator.Add(a, b);
            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        public void Subtract_LargerFromSmallerNumber_ReturnsNegativeResult()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 5;
            int b = 10;
            int result = calculator.Subtract(a, b);
            Assert.That(result, Is.EqualTo(-5));
        }

        [Test]
        public void Multiply_ByZero_ReturnsZero()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 15;
            int b = 0;
            int result = calculator.Multiply(a, b);
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Divide_NegativeNumbers_ReturnsPositiveQuotient()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = -20;
            int b = -4;
            int result = calculator.Divide(a, b);
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void IsEven_ReturnsFalseForOddNumber()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = 9;
            bool result = calculator.IsEven(number);
            Assert.That(result, Is.False);
        }
    }
}