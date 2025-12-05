namespace Day05;

public static class Solution
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day05/input.txt");
        var i = 0;
        var idRanges = new List<IdRange>();

        while (input[i] != "")
        {
            idRanges.Add(IdRange.Parse(input[i]));
            i++;
        }

        var ids = input[(i + 1)..].Select(long.Parse);
        var correctIdsCount = ids.Count(id => idRanges.Any(ir => ir.Contains(id)));

        Console.WriteLine($"Correct IDs count: {correctIdsCount}");
    }

    private readonly record struct IdRange(long Left, long Right)
    {
        public static IdRange Parse(string s)
        {
            var ss = s.Split('-');
            return new(long.Parse(ss[0]), long.Parse(ss[1]));
        }

        public bool Contains(long id) => id >= Left && id <= Right;
    }
}

