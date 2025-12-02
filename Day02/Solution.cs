namespace Day02;

public static class Solution
{
    public static void Run()
    {
        var input = File.ReadAllText("Day02/input.txt");

        var ranges = input.Split(',');

        long seqSum = 0;

        foreach (var range in ranges)
        {
            var (left, right) = ParseRange(range);

            for (var i = left; i <= right; i++)
            {
                if (IsSequential(i.ToString()))
                {
                    Console.WriteLine($"Sequential: {i}");
                    seqSum += i;
                }
            }
        }

        Console.WriteLine($"ID sum: {seqSum}");
    }

    private static (long Left, long Right) ParseRange(string range)
    {
        var left = new string(range.TakeWhile(char.IsDigit).ToArray());
        var right = range[(left.Length + 1) ..];

        return (long.Parse(left), long.Parse(right));
    }

    private static bool IsSequential(string id)
    {
        var firstSize = id.Length / 2;

        for (var i = firstSize; i >= 1; i--)
        {
            var isSequential = id.Chunk(i).Select(p => new string(p)).Distinct().Take(2).Count() == 1;

            if (isSequential)
            {
                return true;
            }
        }

        return false;
    }
}

