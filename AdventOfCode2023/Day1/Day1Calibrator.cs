
namespace Day1;

public class Day1Calibrator
{
    static Dictionary<string, string> replacements = new Dictionary<string, string>
    {
        { "one", "1" },
        { "two", "2" },
        { "three", "3" },
        { "four", "4" },
        { "five", "5" },
        { "six", "6" },
        { "seven", "7" },
        { "eight", "8" },
        { "nine", "9" }
    };

    public IEnumerable<string> GetCalibrations(IEnumerable<string> input)
    {
        foreach (var inputItem in input)
        {
            var numbers = GetNumbers(inputItem);
            var firstChar = numbers.First();
            var lastChar = numbers.Last();
            Console.WriteLine($"From {inputItem}: {firstChar}-{lastChar}");
            
            yield return $"{firstChar}{lastChar}";
        }
    }

    public int GetCalibrationsSum(IEnumerable<string> input)
    {
        var calibrations = GetCalibrations(input);
        var sum = calibrations.Sum(x => int.Parse(x));
        return sum;
    }

    private List<string> GetNumbers(string inputItem)
    {
        var allDigits = inputItem.Select((c, index) => new { c, index })
            .Where(x => char.IsDigit(x.c))
            .ToDictionary(x => x.index, x => x.c.ToString());

        foreach (var replacement in replacements)
        {
            var indexes = inputItem.AllIndexesOf(replacement.Key);

            foreach (var index in indexes)
            {
                allDigits[index] = replacement.Value;
            }
        }

        var sorted = allDigits.OrderBy(x => x.Key).Select(x => x.Value);

        return sorted.ToList();
    }
}

public static class StringExtensions
{
    public static IEnumerable<int> AllIndexesOf(this string str, string value)
    {
        if (String.IsNullOrEmpty(value))
            throw new ArgumentException("the string to find may not be empty", "value");
        for (int index = 0; ; index += value.Length)
        {
            index = str.IndexOf(value, index);
            if (index == -1)
                break;
            yield return index;
        }
    }
}