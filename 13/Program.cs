using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

var lines = File.ReadAllLines("input.txt");
var points = new List<(int x, int y)>();
int i = 0;
for (; i < lines.Length; i++)
{
    if (string.IsNullOrEmpty(lines[i]))
    {
        i++;
        break;
    }

    var numbers = lines[i].Split(",").Select(int.Parse);
    points.Add((numbers.First(), numbers.Last()));
}

var first = true;
for (; i < lines.Length; i++)
{
    var parts = lines[i].Split("=");
    var axis = parts.First().Last();
    var number = int.Parse(parts.Last());

    switch (axis)
    {
        case 'x':
            for (int j = 0; j < points.Count; j++)
            {
                points[j] = (points[j].x < number ? points[j].x : number - (points[j].x - number), points[j].y);
            }
            break;
        case 'y':
            for (int j = 0; j < points.Count; j++)
            {
                points[j] = (points[j].x, points[j].y < number ? points[j].y : number - (points[j].y - number));
            }
            break;
    }

    if (first)
    {
        for (int j = points.Count; j > 0; j--)
        {
            if (points.Count(p => p == points[j - 1]) > 1)
            {
                points.RemoveAt(j - 1);
            }
        }
        Console.WriteLine(points.Count);
        first = false;
    }
}

for (int y = 0; y <= points.Max(p => p.y); y++)
{
    for (int x = 0; x <= points.Max(p => p.x); x++)
    {
        Console.Write(points.Contains((x, y)) ? "#" : ".");
    }
    Console.WriteLine();
}
