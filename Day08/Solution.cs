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

        var orderedDistances = distances.OrderBy(d => d.Distance).Take(ConnectionCount);
        foreach (var (jBox1, jBox2, _) in orderedDistances)
        {
            if (jBox1.Circuit == jBox2.Circuit)
            {
                continue;
            }

            var (bigCircuit, smallCircuit) = jBox1.Circuit.Count >= jBox2.Circuit.Count
                ? (jBox1.Circuit, jBox2.Circuit)
                : (jBox2.Circuit, jBox1.Circuit);
            
            bigCircuit.UnionWith(smallCircuit);
            foreach (var jBox in smallCircuit)
            {
                jBox.Circuit = bigCircuit;
            }
        }

        var circuitMul = jBoxes
            .Select(jb => jb.Circuit)
            .Distinct()
            .OrderByDescending(c => c.Count)
            .Take(3)
            .Aggregate(1, (mul, c) => mul * c.Count);

        Console.WriteLine($"Top 3 circuits size product: {circuitMul}");
    }

    private readonly record struct JBoxDistance(JBox J1, JBox J2, float Distance);

    [DebuggerDisplay("{Vec}")]
    private class JBox
    {
        private JBox(Vector3 vec)
        {
            Vec = vec;
            Circuit = [this];
        }

        public Vector3 Vec { get; }
        public HashSet<JBox> Circuit { get; set; }

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

