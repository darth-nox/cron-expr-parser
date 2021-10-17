using System;
using System.Linq;
using Xunit;

namespace CronExprParser.UnitTests
{
    public class CronExpressionParserTests
    {
        [Fact]
        public void ParseMinutes_ValidValues_Success()
        {
            var parser = new CronExpressionParser();

            Assert.Equal("10", parser.Parse("10 * * * *")[0]);
            Assert.Equal(string.Join(' ', Enumerable.Range(0, 60)), parser.Parse("* * * * *")[0]);
            Assert.Equal("0 5 10 15 20 25 30 35 40 45 50 55", parser.Parse("*/5 * * * *")[0]);
            Assert.Equal("17 27 37 47 57", parser.Parse("17/10 * * * *")[0]);
            Assert.Equal("1 5 27", parser.Parse("1,5,27 * * * *")[0]);
            Assert.Equal("0 1 2 3 4 5 6 7 8 9 10", parser.Parse("0-10 * * * *")[0]);
        }

        [Fact]
        public void ParseMinutes_ValueOutOfRange_Success()
        {
            var parser = new CronExpressionParser();

            Assert.Throws<Exception>(() => parser.Parse("60 * * * *"));
            Assert.Throws<Exception>(() => parser.Parse("-1 * * * *"));
            Assert.Throws<Exception>(() => parser.Parse("A * * * *"));
            Assert.Throws<Exception>(() => parser.Parse("*/60 * * * *"));
            Assert.Throws<Exception>(() => parser.Parse("-1/3 * * * *"));
        }

        [Fact]
        public void ParseHours_ValidValues_Success()
        {
            var parser = new CronExpressionParser();

            Assert.Equal("10", parser.Parse("* 10 * * *")[1]);
            Assert.Equal(string.Join(' ', Enumerable.Range(0, 24)), parser.Parse("* * * * *")[1]);
            Assert.Equal("0 5 10 15 20", parser.Parse("* */5 * * *")[1]);
            Assert.Equal("3 13 23", parser.Parse("* 3/10 * * *")[1]);
            Assert.Equal("1 5 18", parser.Parse("* 1,5,18 * * *")[1]);
            Assert.Equal("0 1 2 3 4 5 6 7 8 9 10", parser.Parse("* 0-10 * * *")[1]);
        }

        [Fact]
        public void ParseHours_ValueOutOfRange_Success()
        {
            var parser = new CronExpressionParser();

            Assert.Throws<Exception>(() => parser.Parse("* 60 * * *"));
            Assert.Throws<Exception>(() => parser.Parse("* -1 * * *"));
            Assert.Throws<Exception>(() => parser.Parse("* A * * *"));
            Assert.Throws<Exception>(() => parser.Parse("* */60 * * *"));
            Assert.Throws<Exception>(() => parser.Parse("* -1/3 * * *"));
        }

        [Fact]
        public void ParseDayOfMonth_ValidValues_Success()
        {
            var parser = new CronExpressionParser();

            Assert.Equal("10", parser.Parse("* * 10 * *")[2]);
            Assert.Equal(string.Join(' ', Enumerable.Range(1, 31)), parser.Parse("* * * * *")[2]);
            Assert.Equal("1 6 11 16 21 26 31", parser.Parse("* * */5 * *")[2]);
            Assert.Equal("3 13 23", parser.Parse("* * 3/10 * *")[2]);
            Assert.Equal("1 5 18 31", parser.Parse("* * 1,5,18,31 * *")[2]);
            Assert.Equal("21 22 23 24 25 26 27 28 29 30", parser.Parse("* * 21-30 * *")[2]);
            Assert.Equal(string.Join(' ', Enumerable.Range(1, 31)), parser.Parse("* * ? * *")[2]);
        }

        [Fact]
        public void ParseDayOfMonth_ValueOutOfRange_Success()
        {
            var parser = new CronExpressionParser();

            Assert.Throws<Exception>(() => parser.Parse("* * 60 * *"));
            Assert.Throws<Exception>(() => parser.Parse("* * -1 * *"));
            Assert.Throws<Exception>(() => parser.Parse("* * A * *"));
            Assert.Throws<Exception>(() => parser.Parse("* * */0 * *"));
            Assert.Throws<Exception>(() => parser.Parse("* * -1/3 * *"));
        }

        [Fact]
        public void ParseMonth_ValidValues_Success()
        {
            var parser = new CronExpressionParser();

            Assert.Equal("JUN", parser.Parse("* * * JUN *")[3]);
            Assert.Equal("FEB AUG", parser.Parse("* * * FEB,AUG *")[3]);
            Assert.Equal("JAN FEB MAR APR MAY JUN JUL AUG SEP", parser.Parse("* * * JAN-SEP *")[3]);
            Assert.Equal("10", parser.Parse("* * * 10 *")[3]);
            Assert.Equal(string.Join(' ', Enumerable.Range(1, 12)), parser.Parse("* * * * *")[3]);
            Assert.Equal("1 6 11", parser.Parse("* * * */5 *")[3]);
            Assert.Equal("3", parser.Parse("* * * 3/10 *")[3]);
            Assert.Equal("1 5 12", parser.Parse("* * * 1,5,12 *")[3]);
            Assert.Equal("2 3 4 5 6 7 8", parser.Parse("* * * 2-8 *")[3]);
        }

        [Fact]
        public void ParseMonth_ValueOutOfRange_Success()
        {
            var parser = new CronExpressionParser();

            Assert.Throws<Exception>(() => parser.Parse("* * * 60 *"));
            Assert.Throws<Exception>(() => parser.Parse("* * * -1 *"));
            Assert.Throws<Exception>(() => parser.Parse("* * * A *"));
            Assert.Throws<Exception>(() => parser.Parse("* * * */0 *"));
            Assert.Throws<Exception>(() => parser.Parse("* * * -1/3 *"));
            Assert.Throws<Exception>(() => parser.Parse("* * * */FEB *"));
            Assert.Throws<Exception>(() => parser.Parse("* * * UUU *"));
        }

        [Fact]
        public void ParseDayOfWeek_ValidValues_Success()
        {
            var parser = new CronExpressionParser();

            Assert.Equal("SAT", parser.Parse("* * * * SAT")[4]);
            Assert.Equal("MON WED", parser.Parse("* * * * MON,WED")[4]);
            Assert.Equal("WED THU FRI", parser.Parse("* * * * WED-FRI")[4]);
            Assert.Equal("6", parser.Parse("* * * * 6")[4]);
            Assert.Equal(string.Join(' ', Enumerable.Range(0, 7)), parser.Parse("* * * * *")[4]);
            Assert.Equal("0 2 4 6", parser.Parse("* * * * */2")[4]);
            Assert.Equal("1 4", parser.Parse("* * * * 1/3")[4]);
            Assert.Equal("0 3 6", parser.Parse("* * * * 0,3,6")[4]);
            Assert.Equal("0 1 2 3 4 5 6", parser.Parse("* * * * 0-6")[4]);
            Assert.Equal(string.Join(' ', Enumerable.Range(0, 7)), parser.Parse("* * * * ?")[4]);
        }

        [Fact]
        public void ParseDayOfWeek_ValueOutOfRange_Success()
        {
            var parser = new CronExpressionParser();

            Assert.Throws<Exception>(() => parser.Parse("* * * * 60"));
            Assert.Throws<Exception>(() => parser.Parse("* * * * -1"));
            Assert.Throws<Exception>(() => parser.Parse("* * * * A"));
            Assert.Throws<Exception>(() => parser.Parse("* * * * */0"));
            Assert.Throws<Exception>(() => parser.Parse("* * * * -1/3"));
            Assert.Throws<Exception>(() => parser.Parse("* * * * */SAT"));
            Assert.Throws<Exception>(() => parser.Parse("* * * * HHH"));
        }
    }
}