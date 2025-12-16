namespace Day09;

public static class Solution
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day09/input.txt");
        var points = input.Select(Point.Parse).ToList();
        var shiftedPoints = points.ZipShift();

        var (horizontalRanges, verticalRanges) = InitRangeDicts(shiftedPoints);
        BuildRangeSetDicts(shiftedPoints, horizontalRanges, verticalRanges);

        var eligiblePointPairs = points
            .Index()
            .SelectMany(p => points.Skip(p.Index + 1), (tuple, point) => (p1: tuple.Item, p2: point))
            .Where(ps => ContainsPeriphery(ps.p1, ps.p2))
            .OrderByDescending(ps => Point.Area(ps.p1, ps.p2))
            .ToArray();

        var maxArea = Point.Area(eligiblePointPairs[0].p1, eligiblePointPairs[0].p2);
        
        Console.WriteLine($"Max rectangle area: {maxArea}");

        bool ContainsPeriphery(Point p1, Point p2)
        {
            var horRange = new Range(p1.X, p2.X);
            var vertRange = new Range(p1.Y, p2.Y);
            return horizontalRanges[vertRange.Small].Contains(horRange)
                && horizontalRanges[vertRange.Big].Contains(horRange)
                && verticalRanges[horRange.Small].Contains(vertRange)
                && verticalRanges[horRange.Big].Contains(vertRange);
        }
    }

    private static void BuildRangeSetDicts(
        List<(Point Prev, Point Next)> shiftedPoints,
        Dictionary<long, RangeSet> horizontalRanges,
        Dictionary<long, RangeSet> verticalRanges)
    {
        var (lowestIdx, lowestHorRange) = shiftedPoints
            .Index()
            .Where(p => p.Item.Prev.Y == p.Item.Next.Y)
            .MaxBy(p => p.Item.Prev.Y);
        shiftedPoints =
        [
            ..shiftedPoints[(lowestIdx+1)..],
            ..shiftedPoints[..(lowestIdx+1)],
        ];

        var directedRanges = new List<DirectedRange>
        {
            new()
            {
                IsVertical = false,
                Facing = true,
                Hook = lowestHorRange.Prev.Y,
                Range = Range.Create(lowestHorRange.Prev, lowestHorRange.Next),
            },
        };
        Console.WriteLine(directedRanges[^1]);

        foreach (var (prev, next) in shiftedPoints[..^1])
        {
            var directedRange = directedRanges[^1].CreateNext(prev, next);
            directedRanges.Add(directedRange);
            
            Console.WriteLine($"P1: {prev}, P2: {next}");
            Console.WriteLine(directedRanges[^1]);
        }

        var dirRangesLookup = directedRanges.ToLookup(dr => dr.Direction);
        foreach (var directedRange in directedRanges)
        {
            var dict = directedRange.IsVertical ? horizontalRanges : verticalRanges;
            var oppositeRanges = dirRangesLookup[directedRange.OppositeDirection()];
            Func<DirectedRange, bool> correctSide =
                directedRange.Direction is Direction.Up or Direction.Left
                    ? other => directedRange.Hook > other.Hook
                    : other => directedRange.Hook < other.Hook;
            foreach (var rangeEnd in new[] { directedRange.Range.Small, directedRange.Range.Big })
            {
                var closestRange = oppositeRanges
                    .Where(dr => dr.Range.Contains(rangeEnd) && correctSide(dr))
                    .Select(dr => new Range(directedRange.Hook, dr.Hook))
                    .MinBy(r => r.Size);
                dict[rangeEnd].Add(closestRange);
            }
        }
    }

    private static (Dictionary<long, RangeSet> horizontalRanges, Dictionary<long, RangeSet> verticalRanges)
        InitRangeDicts(List<(Point, Point)> shiftedPoints)
    {
        var horizontalRanges = new Dictionary<long, RangeSet>();
        var verticalRanges = new Dictionary<long, RangeSet>();
        foreach (var (prevPoint, nextPoint) in shiftedPoints)
        {
            var range = Range.Create(prevPoint, nextPoint);
            
            if (prevPoint.X == nextPoint.X)
            {
                if (verticalRanges.TryGetValue(prevPoint.X, out var rangeSet))
                {
                    rangeSet.Add(range);
                }
                else
                {
                    verticalRanges[prevPoint.X] = new(range);
                }
            }
            else
            {
                if (horizontalRanges.TryGetValue(prevPoint.Y, out var rangeSet))
                {
                    rangeSet.Add(range);
                }
                else
                {
                    horizontalRanges[prevPoint.Y] = new(range);
                }
            }
        }

        return (horizontalRanges, verticalRanges);
    }

    private static List<(T Prev, T Next)> ZipShift<T>(this List<T> list)
    {
        var res = list.Zip(list[1..]).ToList();
        res.Add((list[^1], list[0]));
        return res;
    }


    
    private class DirectedRange
    {
        public long Hook { get; set; }
        public Range Range { get; set; }
        public bool IsVertical { get; set; } // Y are equal
        
        /// <summary>
        /// (Vert and true) -> Right
        /// (Hor and true) -> Up
        /// </summary>
        public bool Facing { get; set; }

        public Direction Direction => GetDirection(IsVertical, Facing);

        public DirectedRange CreateNext(Point prev, Point next)
        {
            var range = Range.Create(prev, next);

            return new()
            {
                Range = range,
                Hook = IsVertical ? prev.Y : prev.X,
                IsVertical = !IsVertical,
                // Loop goes clockwise
                Facing = Direction switch
                {
                    // Down -> Left
                    Direction.Down when Hook == range.Small => false,
                    // Down -> Right
                    Direction.Down => true,
                    // Up -> Right
                    Direction.Up when Hook == range.Small => false,
                    // Up -> Left
                    Direction.Up => true,
                    // Left -> Down
                    Direction.Left when Hook == range.Small => false,
                    // Left -> Up
                    Direction.Left => true,
                    // Right -> Up
                    Direction.Right when Hook == range.Small => false,
                    // Right -> Down
                    Direction.Right => true,
                },
            };
        }

        public Direction OppositeDirection() => GetDirection(IsVertical, !Facing);

        public override string ToString() =>
            $"""Hook: {(IsVertical ? 'X' : 'Y')}{Hook}, {(IsVertical ? 'Y' : 'X')}Range: {Range}, Facing {Direction}""";
        
        private static Direction GetDirection(bool isVertical, bool facing) => (isVertical, facing) switch
        {
            (false, false) => Direction.Down,
            (false, true) => Direction.Up,
            (true, false) => Direction.Left,
            (true, true) => Direction.Right,
        };
    }

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }

    private class RangeSet
    {
        private List<Range> ranges;

        public RangeSet(Range range)
        {
            ranges = [range];
        }

        public void Add(Range range)
        {
            foreach (var (index, other) in ranges.Index().Where(r => r.Item.Touches(range)).Reverse().ToArray())
            {
                range = other.Merge(range);
                ranges.RemoveAt(index);
            }
            
            ranges.Add(range);
        }
        
        public bool Contains(Range v) => ranges.Any(r => r.Contains(v));
        
        public override string ToString() => $"[{string.Join(", ", ranges.Select(r => r.ToString()))}]";
    }

    private readonly record struct Range
    {
        public Range()
        { }
        
        public Range(long v1, long v2)
        {
            (Small, Big) = v1 <= v2 ? (v1, v2) : (v2, v1);
        }

        public static Range Create(Point p1, Point p2)
        {
            if (p1.X == p2.X)
            {
                return new(p1.Y, p2.Y);
            }
            else if (p1.Y == p2.Y)
            {
                return new(p1.X, p2.X);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public long Small { get; init; }
        public long Big { get; init; }
        public long Size => Big - Small + 1;

        public bool Contains(long v) => v >= Small && v <= Big;
        public bool Contains(Range other) => other.Small >= Small && other.Big <= Big;

        public bool Intersects(Range other) =>
            Contains(other.Small) || Contains(other.Big) || other.Contains(Small) || other.Contains(Big);
        
        public bool Touches(Range other) =>
            other.Small + 1 == Big || other.Big - 1 == Small || Intersects(other);

        public Range Merge(Range other) => new()
        {
            Small = Math.Min(Small, other.Small),
            Big = Math.Max(Big, other.Big),
        };
        
        public override string ToString() => $"<{Small}, {Big}>";
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

        public override string ToString() => $"({X}, {Y})";
    }
}

