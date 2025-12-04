namespace Day04;

public static class Solution
{
    private const int MaxNghbrs = 3;
    
    public static void Run()
    {
        var input = File.ReadAllLines("Day04/input.txt");
        var n = input.Length;
        var m = input[0].Length;

        var accessibleSum = 0;

        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < m; j++)
            {
                if (!IsRoll(input, i, j))
                {
                    continue;
                }

                var ii = new[] { i - 1, i, i + 1 };
                var jj = new[] { j - 1, j, j + 1 };
                var nghbrsCount = ii.SelectMany(_ => jj, (i, j) => (i, j)).Count(t => IsRoll(input, t.i, t.j));

                if (nghbrsCount <= MaxNghbrs + 1)
                {
                    accessibleSum++;
                }
            }
        }

        Console.WriteLine($"Accessible sum: {accessibleSum}");
    }

    private static bool IsRoll(string[] grid, int i, int j)
    {
        return grid.ElementAtOrDefault(i)?.ElementAtOrDefault(j) == '@';
    }
}

