namespace ConsoleApp1
{
    public interface ICalculator
    {
        int Add(int a, int b);
        int Divide(int a, int b);
        bool IsOdd(int number);
        int Square(int number);
        int Subtract(int a, int b);
    }
}