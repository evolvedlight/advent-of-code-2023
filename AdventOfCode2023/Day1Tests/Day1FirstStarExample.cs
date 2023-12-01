using Day1;

namespace Day1Tests
{
    public class Day1FirstStar
    {
        [Fact]
        public void Example()
        {
            var inputString = @"1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet";
            var lines = inputString.Split(Environment.NewLine);
            int expected = 142;

            var calc = new Day1Calibrator();
            var actual = calc.GetCalibrationsSum(lines);

            Assert.Equal(expected, actual);
        }
    }
}