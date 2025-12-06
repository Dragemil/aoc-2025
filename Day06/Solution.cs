using System.Text;

namespace Day06;

public static class Solution
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day06/input.txt");
        var transposed = Transpose(input);

        var problems = new List<Problem>();
        var createNew = true;
        foreach (var s in transposed)
        {
            if (createNew)
            {
                createNew = false;
                problems.Add(new()
                {
                    Numbers = [int.Parse(s[..^1])],
                    Operator = s[^1],
                });
            }
            else if (!string.IsNullOrWhiteSpace(s))
            {
                problems[^1].Numbers.Add(int.Parse(s));
            }
            else
            {
                createNew = true;
            }
        }

        var problemSum = problems.Sum(p => p.Compute());

        Console.WriteLine($"Sum of problems: {problemSum}");
    }

    private static string[] Transpose(string[] input)
    {
        var transposed = Enumerable.Range(0, input[0].Length).Select(_ => new StringBuilder()).ToArray();

        foreach (var t in input)
        {
            for (int j = 0; j < input[0].Length; j++)
            {
                transposed[j].Append(t[j]);
            }
        }

        return transposed.Select(sb => sb.ToString()).ToArray();
    }

    private class Problem
    {
        public required List<long> Numbers { get; set; }

        public char Operator { get; set; }

        public long Compute()
        {
            return Operator switch
            {
                '+' => Numbers.Sum(),
                '*' => Numbers.Aggregate((n1, n2) => n1 * n2),
                _ => throw new InvalidOperationException("Unknown sign"),
            };
        }
    }
}

