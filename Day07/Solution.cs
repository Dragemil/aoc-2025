namespace Day07;

public static class Solution
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day07/input.txt");
        var beamStartIdx = input[0].IndexOf('S');
        var beams = new HashSet<int> { beamStartIdx };
        var splitCount = 0;
        
        foreach (var layer in input[1..])
        {
            var toSplit = beams.Where(b => layer[b] == '^').ToArray();
            splitCount += toSplit.Length;
            
            foreach (var b in toSplit)
            {
                beams.Remove(b);
                beams.Add(b - 1);
                beams.Add(b + 1);
            }
        }

        Console.WriteLine($"Tachyon beam splits count: {splitCount}");
    }
}

