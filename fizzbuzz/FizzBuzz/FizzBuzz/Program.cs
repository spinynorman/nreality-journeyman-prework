using System;

namespace FizzBuzz
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var i = 1; i <= 100; i++)
            {
                if (i%3 == 0 && i%5 == 0)
                {
                    Console.WriteLine("FizzBuzz");
                }
                else if (i%3 == 0)
                {
                    Console.WriteLine("Fizz");
                }
                else if (i%5 == 0)
                {
                    Console.WriteLine("Buzz");
                }
                else
                {
                    Console.WriteLine(i);
                }

            }
            Console.WriteLine("Press any key to quit...");
            Console.ReadLine();
        }
    }
}
