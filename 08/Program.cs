using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

Console.WriteLine(File.ReadAllLines("input.txt").Select(line => line.Split("|").Last().Split(" ").Select(segment => segment.Trim()).ToArray()).ToArray().Sum(array => array.Where(segment => new int[] { 2, 3, 4, 7 }.Contains(segment.Length)).Count()));
var sw = Stopwatch.StartNew();

var lines = File.ReadAllLines("input.txt");
var wires = lines.Select(line => line.Split(" | ").First().Split(" ").Select(segment => segment.Trim()).ToArray()).ToArray();
var displays = lines.Select(line => line.Split(" | ").Last().Split(" ").Select(segment => segment.Trim()).ToArray()).ToArray();
var mapping = new Dictionary<int, HashSet<char>>[lines.Length];
var sum = 0;
for (int i = 0; i < lines.Length; i++)
{
    var w = wires[i];
    var m = mapping[i];

    m = new();
    m.Add(1, new(w.First(s => s.Length == 2)));
    m.Add(7, new(w.First(s => s.Length == 3)));
    m.Add(4, new(w.First(s => s.Length == 4)));
    m.Add(8, new(w.First(s => s.Length == 7)));
    m.Add(3, new(w.First(s => s.Length == 5 && s.ToHashSet().IsProperSupersetOf(m[7]))));
    m.Add(9, new(w.First(s => s.Length == 6 && s.ToHashSet().IsProperSupersetOf(m[3]))));
    m.Add(5, new(w.First(s => s.Length == 5 && !s.ToHashSet().SetEquals(m[3]) && s.ToHashSet().IsProperSubsetOf(m[9]))));
    m.Add(2, new(w.First(s => s.Length == 5 && !s.ToHashSet().SetEquals(m[5]) && !s.ToHashSet().SetEquals(m[3]))));
    m.Add(6, new(w.First(s => s.Length == 6 && !s.ToHashSet().SetEquals(m[9]) && s.ToHashSet().IsProperSupersetOf(m[5]))));
    m.Add(0, new(w.First(s => s.Length == 6 && !s.ToHashSet().SetEquals(m[9]) && !s.ToHashSet().SetEquals(m[6]))));

    sum += displays[i].Select(s => m.First(kv => kv.Value.SetEquals(s.ToHashSet())).Key).Aggregate(0, (a, n) => a * 10 + n);
}
Console.WriteLine(sum);
Console.WriteLine(sw.Elapsed);