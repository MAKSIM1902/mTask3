using System;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string key;
            using (var generator = RandomNumberGenerator.Create())
            {
                var salt = new byte[16];
                generator.GetBytes(salt);
                key = String.Concat<byte>(salt);

            }
            if (args.Length % 2 == 0 || args.Length < 3)
            {
                Console.WriteLine("Please enter an odd number of arguments");
                Environment.Exit(0);
            }
            Random r = new Random();
            int g = r.Next(0, args.Length - 1);
            Console.WriteLine($"HMAC: {HMACHASH(args[g], key)}");
            int userChoice;
            Console.WriteLine("Available moves:");
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + args[i]);
            }
            Console.Write("Enter your move: ");
            userChoice = Int32.Parse(Console.ReadLine());
            userChoice--;
            Console.WriteLine($"Your move: {args[userChoice]}");
            Console.WriteLine("Computer move: " + args[g]);
            Middle(args, userChoice);
            if (userChoice == g)
            {
                Console.WriteLine("Draw");
            }
            else if (userChoice > g)
            {
                Console.WriteLine("You win!");
            }
            else
            {
                Console.WriteLine("You lose!");
            }
            Console.WriteLine($"HMAC key: {HMACHASH(key,"")}");
        }
        static string[] Middle(string[] arr, int index)
        {
            int z = arr.Length / 2 + 1 - index;
            if (z == index) return arr;
            if (z < 0)
            {
                z += index;
            }

            for (int j = 0; j < z; j++)
            {
                string b = arr[arr.Length - 1];
                for (int i = arr.Length - 1; i > 0; i--)
                {
                    arr[i] = arr[i - 1];
                }
                arr[0] = b;
            }
            return arr;
        }

        static string HMACHASH(string str, string key)
        {
            byte[] bkey = Encoding.Default.GetBytes(key);
            using (var hmac = new HMACSHA256(bkey))
            {
                byte[] bstr = Encoding.Default.GetBytes(str);
                var bhash = hmac.ComputeHash(bstr);
                return BitConverter.ToString(bhash).Replace("-", string.Empty);
            }
        }
    }
}
