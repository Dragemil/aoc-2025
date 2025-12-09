namespace Day09;

public static class Solution
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day09/input.txt");
        var points = input.Select(Point.Parse).Index().ToList();
        var maxArea = points
            .SelectMany(p => points.Skip(p.Index + 1), (p1, p2) => Point.Area(p1.Item, p2.Item))
            .Max();

        Console.WriteLine($"Max rectangle area: {maxArea}");
    }

    private readonly record struct Point(long X, long Y)
    {
        public static Point Parse(string s)
        {
            var coords = s.Split(',');
            return new(long.Parse(coords[0]), long.Parse(coords[1]));
        }
        
        public static long Area(Point p1, Point p2)
            => (Math.Abs(p1.X - p2.X) + 1) * (Math.Abs(p1.Y - p2.Y) + 1);
    }
}

