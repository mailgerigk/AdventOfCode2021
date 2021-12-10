using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

var sw = Stopwatch.StartNew();
var input = File.ReadAllText("input.txt").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
input = new int[] { 16, 1, 2, 0, 4, 2, 7, 1, 2, 14 };
var (mn, mx) = (input.Min(), input.Max());
var part1 = Enumerable.Range(input.Min(), input.Max()).Select(i => (i, input.Select(n => Math.Abs(n - i)).Sum())).OrderBy(t => t.Item2).First();
Console.WriteLine(part1);

Func<int, int, int> a = (int target, int current) =>
{
    var abs = Math.Abs(target - current);
    var sum = 0;
    for (int i = 0; i < abs; i++)
    {
        sum += i + 1;
    }
    return sum;
};

var part2 = Enumerable.Range(input.Min(), input.Max()).Select(i => (i, input.Select(n => a(i, n)).Sum())).OrderBy(t => t.Item2).First();
Console.WriteLine(part2);
Console.WriteLine(sw.Elapsed);

Console.WriteLine(
    Enumerable.Range(
        File.ReadAllText("input.txt")
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .Min(),
        File.ReadAllText("input.txt")
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .Max())
    .Select(i => (
        i,
        File.ReadAllText("input.txt")
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .Select(n => Math.Abs(n - i))
            .Sum())
        )
    .OrderBy(t => t.Item2)
    .First()
);

Console.WriteLine(
    Enumerable.Range(
        File.ReadAllText("input.txt")
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .Min(),
        File.ReadAllText("input.txt")
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .Max())
    .Select(i => (
        i,
        File.ReadAllText("input.txt")
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .Select(n =>
            {
                var abs = Math.Abs(i - n);
                var sum = 0;
                for (int j = 0; j < abs; j++)
                {
                    sum += j + 1;
                }
                return sum;
            })
            .Sum())
        )
    .OrderBy(t => t.Item2)
    .First()
);

Console.WriteLine(Enumerable.Range(File.ReadAllText("input.txt").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Min(), File.ReadAllText("input.txt").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Max()).Select(i => (i, File.ReadAllText("input.txt").Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Select(n => { var abs = Math.Abs(i - n); var sum = 0; for (int j = 0; j < abs; j++) { sum += j + 1; } return sum; }).Sum())).OrderBy(t => t.Item2).First());