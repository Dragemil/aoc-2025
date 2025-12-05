namespace Day05;

public static class Solution
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day05/input.txt");
        var idRanges = new LinkedList<IdRange>();

        foreach (var s in input.TakeWhile(s => s != ""))
        {
            var idRange = IdRange.Parse(s);
            var node = idRanges.First;

            while (node is not null)
            {
                var prevNode = node;
                node = node.Next;
                if (idRange.Intersects(prevNode.Value))
                {
                    idRange = idRange.Merge(prevNode.Value);
                    idRanges.Remove(prevNode);
                }
            }

            idRanges.AddFirst(idRange);
        }

        var correctIdsCount = idRanges.Sum(ir => ir.Size);

        Console.WriteLine($"Correct IDs count: {correctIdsCount}");
    }

    private readonly record struct IdRange(long Left, long Right)
    {
        public static IdRange Parse(string s)
        {
            var ss = s.Split('-');
            return new(long.Parse(ss[0]), long.Parse(ss[1]));
        }

        public IdRange Merge(IdRange other) => new(Math.Min(Left, other.Left), Math.Max(Right, other.Right));

        public bool Intersects(IdRange other)
        {
            return Contains(other.Left)
                || Contains(other.Right)
                || other.Contains(Left)
                || other.Contains(Right);
        }

        public bool Contains(long id) => id >= Left && id <= Right;

        public long Size => Right - Left + 1;
    }
}

