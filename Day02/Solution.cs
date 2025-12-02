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
        var halfSize = id.Length / 2;

        return id[..halfSize] == id[halfSize..];
    }
}

