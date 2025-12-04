namespace Day04;

public static class Solution
{
    private const int MaxNghbrs = 3;
    
    public static void Run()
    {
        var input = File.ReadAllLines("Day04/input.txt");
        var grid = Array.ConvertAll(input, s => s.ToCharArray());
        
        var n = grid.Length;
        var m = grid[0].Length;

        var accessibleSum = 0;
        var prevSum = -1;
        var it = 0;

        while (accessibleSum != prevSum)
        {
            Console.WriteLine($"Iteration {it}, sum: {accessibleSum}");
            it++;
            prevSum = accessibleSum;
            
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < m; j++)
                {
                    if (!IsRoll(grid, i, j))
                    {
                        continue;
                    }

                    var ii = new[] { i - 1, i, i + 1 };
                    var jj = new[] { j - 1, j, j + 1 };
                    var nghbrsCount = ii.SelectMany(_ => jj, (i, j) => (i, j)).Count(t => IsRoll(grid, t.i, t.j));

                    if (nghbrsCount <= MaxNghbrs + 1)
                    {
                        accessibleSum++;
                        grid[i][j] = '.';
                    }
                }
            }
        }

        Console.WriteLine($"Accessible sum: {accessibleSum}");
    }

    private static bool IsRoll(char[][] grid, int i, int j)
    {
        return grid.ElementAtOrDefault(i)?.ElementAtOrDefault(j) == '@';
    }
}

