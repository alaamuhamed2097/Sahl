namespace ConsoleAppNUnitTest
{
    public class CalculatorXUnitTests
    {
        [Fact]
        public void AddNumbers_ReturnsSum()
        {
            // Arrange
            var calculator = new ConsoleApp1.Calculator();
            int a = 7;
            int b = 8;

            // Act
            int result = calculator.Add(a, b);

            // Assert
            Assert.Equal(15, result);
        }

        [Fact]
        public void SubtractNumbers_ReturnsDifference()
        {
            // Arrange
            var calculator = new ConsoleApp1.Calculator();
            int a = 10;
            int b = 4;

            // Act
            int result = calculator.Subtract(a, b);

            // Assert
            Assert.Equal(6, result);
        }

        [Fact]
        public void MultiplyNumbers_ReturnsDifference()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 10;
            int b = 4;
            int result = calculator.Multiply(a, b);
            Assert.Equal(40, result);
        }

        [Fact]
        public void DivideNumbers_ReturnsQuotient()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 20;
            int b = 4;
            int result = calculator.Divide(a, b);
            Assert.Equal(5, result);
        }

        [Fact]
        public void DivideByZero_ThrowsException()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 10;
            int b = 0;
            Assert.Throws<DivideByZeroException>(() => calculator.Divide(a, b));
        }

        [Fact]
        public void IsEven_ReturnsTrueForEvenNumber()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = 6;
            bool result = calculator.IsEven(number);
            Assert.True(result);
        }

        [Fact]
        public void IsOdd_ReturnsTrueForOddNumber()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = 7;
            bool result = calculator.IsOdd(number);
            Assert.True(result);
        }

        [Fact]
        public void Square_ReturnsSquaredValue()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = 5;
            int result = calculator.Square(number);
            Assert.Equal(25, result);
        }

        [Fact]
        public void SquareOfNegativeNumber_ReturnsPositiveSquaredValue()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = -4;
            int result = calculator.Square(number);
            Assert.Equal(16, result);
        }

        [Fact]
        public void Add_NegativeAndPositiveNumber_ReturnsCorrectSum()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = -3;
            int b = 7;
            int result = calculator.Add(a, b);
            Assert.Equal(4, result);
        }

        [Fact]
        public void Subtract_LargerFromSmallerNumber_ReturnsNegativeResult()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 5;
            int b = 10;
            int result = calculator.Subtract(a, b);
            Assert.Equal(-5, result);
        }

        [Fact]
        public void Multiply_ByZero_ReturnsZero()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = 15;
            int b = 0;
            int result = calculator.Multiply(a, b);
            Assert.Equal(0, result);
        }

        [Fact]
        public void Divide_NegativeNumbers_ReturnsPositiveQuotient()
        {
            var calculator = new ConsoleApp1.Calculator();
            int a = -20;
            int b = -4;
            int result = calculator.Divide(a, b);
            Assert.Equal(5, result);
        }

        [Fact]
        public void IsEven_ReturnsFalseForOddNumber()
        {
            var calculator = new ConsoleApp1.Calculator();
            int number = 9;
            bool result = calculator.IsEven(number);
            Assert.False(result);
        }
    }
}