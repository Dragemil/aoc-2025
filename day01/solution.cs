#!/usr/bin/dotnet run

var input = File.ReadAllLines("input.txt");

var dial = 50;
var zeros = 0;

foreach (var rotation in input)
{
    var count = int.Parse(rotation[1..]);
    var positive = rotation[0] == 'R' ? 1 : -1;
    dial = (dial + count * positive) % 100;

    Console.WriteLine(dial);

    if (dial == 0)
    {
        zeros++;
    }
}

Console.WriteLine($"Password: {zeros}");