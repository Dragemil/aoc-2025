using System.Text;

namespace Day03;

public static class Solution
{
    private const int BatteriesCount = 12;
    
    public static void Run()
    {
        var input = File.ReadAllLines("Day03/input.txt");

        var joltageSum = input.Sum(GetMaxJoltage);

        Console.WriteLine($"Joltage sum: {joltageSum}");
    }

    private static long GetMaxJoltage(string bank)
    {
        var jb = new StringBuilder(BatteriesCount);
        var lastBatteryIdx = -1;
        for (var i = 0; i < BatteriesCount; i++)
        {
            var from = lastBatteryIdx + 1;
            var to = bank.Length - BatteriesCount + i + 1;
            (var battery, lastBatteryIdx) = bank[from..to]
                .Zip(Enumerable.Range(from, to - from))
                .MaxBy(b => b.First);
            
            jb.Append(battery);
        }

        var joltage = long.Parse(jb.ToString());
        
        Console.WriteLine($"Joltage: {joltage}");
        return joltage;
    }
}

