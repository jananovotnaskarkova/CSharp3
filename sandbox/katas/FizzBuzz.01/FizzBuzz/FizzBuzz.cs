public class FizzBuzz
{
    public static void CountTo(int lastNumber)
    {
        for (int number = 1; number <= lastNumber; number++)
        {
            if ((number % 3 == 0 && number % 5 == 0) || (number.ToString().Contains('3') && number.ToString().Contains('5')))
            {
                Console.WriteLine("FizzBuzz");
            }
            else if (number % 3 == 0 || number.ToString().Contains('3'))
            {
                Console.WriteLine("Fizz");
            }
            else if (number % 5 == 0 || number.ToString().Contains('5'))
            {
                Console.WriteLine("Buzz");
            }
            else
            {
                Console.WriteLine(number);
            }
        }
    }
}
