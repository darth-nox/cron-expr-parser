using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CronExprParser
{
    public class CronExpressionParser
    {
        private readonly string[] _months = { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        private readonly List<Func<string, string>> _parsers = new();
        private readonly string[] _weekdays = { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" };

        public CronExpressionParser()
        {
            _parsers.Add(ParseMinute);
            _parsers.Add(ParseHour);
            _parsers.Add(ParseDayOfMonth);
            _parsers.Add(ParseMonth);
            _parsers.Add(ParseDayOfWeek);
        }

        public IList<string> Parse(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) throw new ArgumentException("Please provide non-empty CRON expression");

            var parts = expression.Split(' ');
            if (parts.Length == 0)
                throw new ArgumentException($"CRON expression should contain 5 parts: minutes hours dom month dow, got {expression}");

            return parts.Select((s, i) => _parsers[i](s)).ToList();
        }

        private static string ParseMinute(string value) => ParseNumeric(value, 0, 59);

        private static string ParseHour(string value) => ParseNumeric(value, 0, 23);

        private static string ParseDayOfMonth(string value)
        {
            const int min = 1;
            const int max = 31;
            return value == "?" ? string.Join(' ', GenerateSequence(min, max)) : ParseNumeric(value, min, max);
        }

        private string ParseMonth(string value)
        {
            var month = value.ToUpperInvariant();
            return TryParseName(month, _months, out var res) ? res : ParseNumeric(month, 1, 12);
        }

        private string ParseDayOfWeek(string value)
        {
            const int min = 0;
            const int max = 6;
            if (value == "?") return string.Join(' ', GenerateSequence(min, max));

            var dayOfWeek = value.ToUpperInvariant();
            return TryParseName(dayOfWeek, _weekdays, out var res) ? res : ParseNumeric(dayOfWeek, min, max);
        }

        private static bool TryParseName(string value, string[] allowedNames, out string result)
        {
            // Single name
            if (allowedNames.Contains(value))
            {
                result = value;
                return true;
            }

            // List of values
            if (Regex.IsMatch(value, @"^([A-Z]{3},)+[A-Z]{3}$"))
            {
                result = string.Join(' ', value.Split(','));
                return true;
            }

            // Range of values
            if (Regex.IsMatch(value, @"^[A-Z]{3}-[A-Z]{3}$"))
            {
                var parts = value.Split('-');
                if (allowedNames.Contains(parts[0]) && allowedNames.Contains(parts[1]))
                {
                    var left = Array.IndexOf(allowedNames, parts[0]);
                    var right = Array.IndexOf(allowedNames, parts[1]);
                    result = string.Join(' ', allowedNames[left..(right + 1)]);
                    return true;
                }
            }

            result = null;
            return false;
        }

        private static string ParseNumeric(string value, int min, int max)
        {
            // Number
            if (int.TryParse(value, out var m) && m >= min && m <= max) return value;

            // Any value
            if (value == "*") return string.Join(' ', GenerateSequence(min, max));

            // Repetition with step
            if (Regex.IsMatch(value!, @"^(\d+|\*)/\d+$"))
            {
                var parts = value.Split('/');
                var start = parts[0] == "*" ? min : int.Parse(parts[0]);
                var step = int.Parse(parts[1]);
                if (start >= min && start <= max && step > 0 && step <= max) return string.Join(' ', GenerateSequence(start, max, step));
            }

            // List of values
            if (Regex.IsMatch(value!, @"^(\d+,)+\d+$")) return string.Join(' ', value.Split(','));

            // Range of values
            if (Regex.IsMatch(value!, @"^\d+-\d+$"))
            {
                var parts = value.Split('-');
                var start = int.Parse(parts[0]);
                var end = int.Parse(parts[1]);
                if (start >= min && start <= max && end >= min && end <= max && end > start) return string.Join(' ', GenerateSequence(start, end));
            }

            throw new Exception($"{value} isn't valid expression");
        }

        private static IEnumerable<int> GenerateSequence(int min, int max, int step = 1)
        {
            for (var i = min; i <= max; i += step) yield return i;
        }
    }
}