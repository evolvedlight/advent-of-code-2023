namespace Day4;

public static class Day4LotteryCalculator
{
    public static int CalculatePoints(string input)
    {
        var cards = input.Split(Environment.NewLine).Select(l => ParseToCard(l)).ToList();

        var sum = 0;

        foreach (var card in cards)
        {
            var points = 0;
            foreach (var number in card.ActualNumbers)
            {
                if (card.WinningNumbers.Contains(number))
                {
                    if (points == 0)
                    {
                        points = 1;
                    }
                    else
                    {
                        points *= 2;
                    }
                }
            }

            sum += points;
        }

        return sum;
    }

    private static Card ParseToCard(string l)
    {
        var card = new Card();

        var cardText = l.Split(":");
        var cardString = cardText[0];
        var winningString = cardText[1].Split("|")[0];
        var actualString = cardText[1].Split("|")[1];
        var winningNumbers = winningString.Split(" ").Where(n => !string.IsNullOrEmpty(n)).Select(n => int.Parse(n.Trim())).ToList();
        var actualNumbers = actualString.Split(" ").Where(n => !string.IsNullOrEmpty(n)).Select(n => int.Parse(n.Trim())).ToList();

        card.WinningNumbers = winningNumbers;
        card.ActualNumbers = actualNumbers;

        return card;
    }
}

public class Card()
{
    public List<int> WinningNumbers = [];
    public List<int> ActualNumbers = [];
}