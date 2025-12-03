namespace Day03;

public static class Solution
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day03/input.txt");

        var joltageSum = input.Sum(GetMaxJoltage);

        Console.WriteLine($"Joltage sum: {joltageSum}");
    }

    private static int GetMaxJoltage(string bank)
    {
        var maxFirst = bank[..^1].Index().MaxBy(b => b.Item);
        var maxSecond = bank[(maxFirst.Index+1) ..].Max();

        var joltage = int.Parse(new([maxFirst.Item, maxSecond]));
        
        Console.WriteLine($"Joltage: {joltage}");

        return joltage;
    }
}

