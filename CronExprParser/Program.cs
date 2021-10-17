using System;

namespace CronExprParser
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args is null || args.Length == 0)
            {
                Console.WriteLine("Please provide the expression");
                return;
            }

            var input = args[0];
            var commandDelimiterIndex = input.LastIndexOf(' ');
            var headers = new[] { "minute", "hour", "day of month", "month", "day of week", "command" };

            var parser = new CronExpressionParser();
            var values = parser.Parse(input[..commandDelimiterIndex]);
            values.Add(input[(commandDelimiterIndex + 1)..]);

            for (var i = 0; i < headers.Length; i++) Console.WriteLine("{0,-14}{1}", headers[i], values[i]);
        }
    }
}