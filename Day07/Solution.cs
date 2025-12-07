namespace Day07;

public static class Solution
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day07/input.txt");
        var beamStartIdx = input[0].IndexOf('S');
        var beamTimelines = new Dictionary<int, long> { [beamStartIdx] = 1 };
        long timelinesCount = 1;
        
        foreach (var layer in input[1..])
        {
            var toSplit = beamTimelines.Where(b => layer[b.Key] == '^').ToArray();
            timelinesCount += toSplit.Sum(b => b.Value);
            
            foreach (var (beam, timelines) in toSplit)
            {
                beamTimelines.Remove(beam);
                beamTimelines.AddOrUpdate(beam - 1, timelines, t => t + timelines);
                beamTimelines.AddOrUpdate(beam + 1, timelines, t => t + timelines);
            }
        }

        Console.WriteLine($"Tachyon beam timelines count: {timelinesCount}");
    }

    private static void AddOrUpdate<K, V>(this IDictionary<K, V> dict, K key, V toAdd, Func<V, V> toUpdate)
    {
        if (dict.TryGetValue(key, out var value))
        {
            dict[key] = toUpdate(value);
        }
        else
        {
            dict[key] = toAdd;
        }
    }
}

