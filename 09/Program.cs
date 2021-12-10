using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

var sw = Stopwatch.StartNew();
var lines = File.ReadAllLines("input.txt").Where(line => !string.IsNullOrEmpty(line)).ToArray();
var heights = lines.Select(line => line.Select(c => c - '0').ToArray()).ToArray();
var riskLevel = 0;
var lowPoints = new List<(int x, int y)>();
for (int y = 0; y < heights.Length; y++)
{
    for (int x = 0; x < heights[y].Length; x++)
    {
        var height = heights[y][x];

        var neededForLowpoint = 4;
        neededForLowpoint -= (x == 0 || x == heights[y].Length - 1) ? 1 : 0;
        neededForLowpoint -= (y == 0 || y == heights.Length - 1) ? 1 : 0;

        var heigherNeighbours = 0;
        heigherNeighbours += (0 < x && heights[y][x - 1] > height) ? 1 : 0;
        heigherNeighbours += (x < heights[y].Length - 1 && heights[y][x + 1] > height) ? 1 : 0;
        heigherNeighbours += (0 < y && heights[y - 1][x] > height) ? 1 : 0;
        heigherNeighbours += (y < heights.Length - 1 && heights[y + 1][x] > height) ? 1 : 0;

        riskLevel += heigherNeighbours >= neededForLowpoint ? 1 + height : 0;

        if (heigherNeighbours >= neededForLowpoint)
            lowPoints.Add((x, y));
    }
}
Console.WriteLine(riskLevel);
var elapsed1 = sw.Elapsed;
Console.WriteLine(elapsed1);

sw.Start();
var basinSizes = new List<int>();
foreach (var lowPoint in lowPoints)
{
    var set = new HashSet<(int, int)>();
    var queue = new Queue<(int x, int y)>();
    queue.Enqueue(lowPoint);

    var basinSize = 0;
    while (queue.Any())
    {
        var (x, y) = queue.Dequeue();
        set.Add((x, y));

        var isInBounds = 0 <= y && y <= heights.Length - 1 && 0 <= x && x <= heights[y].Length - 1;
        if (!isInBounds || heights[y][x] == 9)
            continue;

        if (set.Add((x + 1, y)))
            queue.Enqueue((x + 1, y));
        if (set.Add((x - 1, y)))
            queue.Enqueue((x - 1, y));
        if (set.Add((x, y + 1)))
            queue.Enqueue((x, y + 1));
        if (set.Add((x, y - 1)))
            queue.Enqueue((x, y - 1));

        basinSize++;
    }

    basinSizes.Add(basinSize);
}
basinSizes.Sort();
basinSizes.Reverse();
Console.WriteLine(basinSizes.Take(3).Aggregate(1, (aggregate, item) => aggregate * item));
var elapsed2 = sw.Elapsed;
Console.WriteLine(elapsed2);
