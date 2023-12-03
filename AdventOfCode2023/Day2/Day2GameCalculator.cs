

using System.Text.RegularExpressions;

namespace Day2;

public static partial class Day2GameCalculator
{
    public static int Solve(string input, Dictionary<string, int> maximums)
    {
        var games = Parse(input);
        var idsOfBadGames = new List<int>();
        foreach (var maximum in maximums)
        {
            var color = maximum.Key;
            var max = maximum.Value;
            foreach (var game in games)
            {
                foreach (var round in game.Rounds)
                {
                    foreach (var hand in round.Hands)
                    {
                        if (hand.color == color && hand.number > max)
                        {
                            idsOfBadGames.Add(game.Id);
                        }
                    }
                }
            }
        }

        var idsOfGoodGames = games.Select(g => g.Id).Except(idsOfBadGames);

        return idsOfGoodGames.Sum();
    }

    public static int SolveP2(string input)
    {
        var games = Parse(input);
        return games.Select(g => g.CalculatePower()).ToList().Sum();
    }

    private static List<Game> Parse(string input)
    {
        var games = new List<Game>();
        foreach (var line in input.Split(Environment.NewLine))
        {
            games.Add(ParseGame(line));
        }
        return games;
    }

    private static Game ParseGame(string line)
    {
        Regex regex = GameRegex();
        Regex colorRegex = ColorRegex();
        var game = new Game();
        var firstMatch = regex.Match(line);
        game.Id = int.Parse(firstMatch.Groups[1].Value);
        var rest = firstMatch.Groups[2].Value;

        var rounds = rest.Split(';');
        foreach (var round in rounds)
        {
            var roundObj = new Round();
            var vals = round.Split(',');
            foreach (var val in vals)
            {
                var matchesColor = colorRegex.Match(val);
                roundObj.Hands.Add((matchesColor.Groups[2].Value, int.Parse(matchesColor.Groups[1].Value)));
            }
            game.Rounds.Add(roundObj);
        }
        return game;
    }

    [GeneratedRegex(@"Game (\d+):(.*)")]
    private static partial Regex GameRegex();
    [GeneratedRegex(@"(\d+) (\w+)")]
    private static partial Regex ColorRegex();


}


internal class Game
{
    public int Id { get; set; }
    public List<Round> Rounds { get; set; } = [];

    internal int CalculatePower()
    {
        return Rounds.SelectMany(r => r.Hands).GroupBy(h => h.color).Select(g => (color: g.Key, number: g.Max(h => h.number))).Aggregate(1, (total, next) => total * next.number);
    }
}

public class Round
{
    public List<(string color, int number)> Hands { get; set; } = [];
}


