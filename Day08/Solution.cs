using System.Diagnostics;
using System.Numerics;

namespace Day08;

public static class Solution
{
    private const int ConnectionCount = 1000;
    
    public static void Run()
    {
        var input = File.ReadAllLines("Day08/input.txt");
        var jBoxes = input.Select(JBox.Parse).ToList();
        var distances = new List<JBoxDistance>(jBoxes.Count * (jBoxes.Count - 1) / 2);
        for (var i = 0; i < jBoxes.Count; i++)
        {
            for (var j = i + 1; j < jBoxes.Count; j++)
            {
                distances.Add(new(jBoxes[i], jBoxes[j], JBox.Distance(jBoxes[i], jBoxes[j])));
            }
        }

        var jBoxToCircuit = jBoxes.ToDictionary(j => j, j => new HashSet<JBox> { j });
        var orderedDistances = distances.OrderBy(d => d.Distance).Take(ConnectionCount);
        foreach (var (jBox1, jBox2, _) in orderedDistances)
        {
            var circuit1 = jBoxToCircuit[jBox1];
            var circuit2 = jBoxToCircuit[jBox2];

            if (circuit1 == circuit2)
            {
                continue;
            }

            var (bigCircuit, smallCircuit) =
                circuit1.Count >= circuit2.Count ? (circuit1, circuit2) : (circuit2, circuit1);
            
            bigCircuit.UnionWith(smallCircuit);
            foreach (var jBox in smallCircuit)
            {
                jBoxToCircuit[jBox] = bigCircuit;
            }
        }

        var circuitMul = jBoxToCircuit.Values
            .Distinct()
            .OrderByDescending(c => c.Count)
            .Take(3)
            .Aggregate(1, (mul, c) => mul * c.Count);

        Console.WriteLine($"Top 3 circuits size product: {circuitMul}");
    }

    private readonly record struct JBoxDistance(JBox J1, JBox J2, double Distance);

    [DebuggerDisplay("X = {X}, Y = {Y}, Z = {Z}")]
    private class JBox(Vector3 vec)
    {
        public Vector3 Vec { get; } = vec;

        public static JBox Parse(string str)
        {
            Span<float> coords = stackalloc float[3];
            var span = str.AsSpan();
            var ranges = span.Split(',');

            var i = 0;
            foreach (var range in ranges)
            {
                coords[i++] = float.Parse(span[range]);
            }

            return new(new(coords));
        }

        public static float Distance(JBox left, JBox right)
        {
            return Vector3.Distance(left.Vec, right.Vec);
        }
    }
}

