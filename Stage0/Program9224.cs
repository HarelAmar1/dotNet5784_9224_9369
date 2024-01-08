using System;
namespace Stage0
{
    partial class Program
    {
        private static void Main(string[] args)
        {
            Welcome9224();
            Welcome9369();
            Console.ReadKey();
        }
        static partial void Welcome9369();
        private static void Welcome9224()
        {
            Console.WriteLine("Enter your name please:");
            string? name = Console.ReadLine();
            Console.WriteLine("welcome {0}!", name);
        }
    }
}
