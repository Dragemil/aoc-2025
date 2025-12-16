using System.Collections.Immutable;

namespace Day10;

public static class Solution
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day10/input.txt");
        var machines = input.Select(Machine.Parse).ToList();

        var minButtonPressSum = 0;
        foreach (var machine in machines)
        {
            var buttonCombinations = new List<ButtonCombination> { new(machine) };
            while (!buttonCombinations[0].IsFinal)
            {
                var newButtonCombinations = new List<ButtonCombination>();
                foreach (var buttonCombination in buttonCombinations)
                {
                    for (var i = buttonCombination.LastClickedIdx + 1; i < machine.ButtonSchematics.Length; i++)
                    {
                        var newButtonCombination = buttonCombination.AddClick(i);
                        if (newButtonCombination.IsCorrect)
                        {
                            minButtonPressSum += newButtonCombination.ClickCount;
                            goto Found; // 😌
                        }
                        
                        newButtonCombinations.Add(newButtonCombination);
                    }
                }
                newButtonCombinations.Sort((bc1, bc2) => bc1.DistanceFromDesired.CompareTo(bc2.DistanceFromDesired));
                buttonCombinations = newButtonCombinations;
            }

            throw new InvalidOperationException("No combination found.");
            Found: {}
        }

        Console.WriteLine($"Minimum button press sum {minButtonPressSum}");
        // pt. 1 == 390
    }

    private class ButtonCombination
    {
        public Machine Machine { get; private init; }
        public bool[] Lights { get; private init; }
        public HashSet<int> ButtonsClicked { get; private init; }
        public int LastClickedIdx { get; private init; }
        public int DistanceFromDesired { get; private init; }
        public bool IsCorrect => DistanceFromDesired == 0;
        public bool IsFinal => ButtonsClicked.Count == Machine.ButtonSchematics.Length;
        public int ClickCount => ButtonsClicked.Count;

        private ButtonCombination()
        { }

        public ButtonCombination(Machine machine)
        {
            Machine = machine;
            Lights = new bool[machine.DesiredLights.Length];
            ButtonsClicked = [];
            LastClickedIdx = -1;
            DistanceFromDesired = GetDistance(Lights);
        }

        public ButtonCombination AddClick(int schematicIdx)
        {
            var newLights = Lights.ToArray();
            foreach (var lightSwitch in Machine.ButtonSchematics[schematicIdx])
            {
                newLights[lightSwitch] = !newLights[lightSwitch];
            }

            return new()
            {
                Machine = Machine,
                Lights = newLights,
                ButtonsClicked = [ ..ButtonsClicked, schematicIdx],
                LastClickedIdx = schematicIdx,
                DistanceFromDesired = GetDistance(newLights),
            };
        }

        private int GetDistance(bool[] lights)
            => Machine.DesiredLights.Zip(lights, (l1, l2) => l1 ^ l2).Count(l => l);
    }
    
    private class Machine
    {
        public ImmutableArray<bool> DesiredLights { get; private init; }
        public ImmutableArray<ImmutableHashSet<int>> ButtonSchematics { get; private init; }

        public static Machine Parse(string s)
        {
            var parts = s.Split(' ');
            return new()
            {
                DesiredLights = [..parts[0][1..^1].Select(l => l == '#')],
                ButtonSchematics = [..parts[1..^1]
                    .Select(p => p[1..^1].Split(',').Select(int.Parse).ToImmutableHashSet())],
            };
        }
    }
}

