namespace Day01;

public static class Solution
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day01/input.txt");

        var dial = 50;
        var zeros = 0;
        const int b = 100;

        foreach (var rotation in input)
        {
            var prevDial = dial;
            if (!(rotation is [char dir, .. var rest] && int.TryParse(rest, out var count)))
            {
                throw new Exception();
            }

            if (count == 0)
            {
                continue;
            }

            var isPositive = dir == 'R';
            var positive = isPositive ? 1 : -1;
            var rot = count * positive;
            Console.WriteLine($"Rotation: {rot}");
            dial += rot;

            if (dial == 0)
            {
                zeros++;
                Console.WriteLine($"Dial: {dial}");
                Console.WriteLine($"Zeros: {zeros}");
                continue;
            }

            var fixup = dial < 0 ? b : -b;
            while (!(dial is (>= 0) and (< b)))
            {
                dial += fixup;
                zeros++;
            }

            if (!isPositive)
            {
                if (prevDial == 0)
                {
                    zeros--;
                }

                if (dial == 0)
                {
                    zeros++;
                }
            }

            Console.WriteLine($"Dial: {dial}");
            Console.WriteLine($"Zeros: {zeros}");
        }

        Console.WriteLine($"Password: {zeros}");
    }
}