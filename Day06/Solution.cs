namespace Day06;

public static class Solution
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day06/input.txt");
        var problems = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(n => new Problem{ Numbers = [long.Parse(n)]}).ToList();
        foreach (var (number, problem) in input[1..^1]
            .SelectMany(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Zip(problems)))
        {
            problem.Numbers.Add(long.Parse(number));
        }
        
        foreach (var (op, problem) in input[^1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Zip(problems))
        {
            if (op is not [var opc])
            {
                throw new InvalidOperationException("Too long sign");
            }

            problem.Operator = opc;
        }

        var problemSum = problems.Sum(p => p.Compute());

        Console.WriteLine($"Sum of problems: {problemSum}");
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

