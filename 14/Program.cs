using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Data;

namespace _14
{
    public class Program
    {
        static void Main(string[] args)
        {
            BenchmarkDotNet.Running.BenchmarkRunner.Run<Program>();

            var p = new Program();
            p.Part1();
            p.Part2();
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void Part1()
        {
            var lines = File.ReadAllLines("input.txt");
            //            lines = @"NNCB

            //CH -> B
            //HH -> N
            //CB -> H
            //NH -> C
            //HB -> C
            //HC -> B
            //HN -> C
            //NN -> C
            //BH -> H
            //NC -> B
            //NB -> B
            //BN -> B
            //BB -> N
            //BC -> B
            //CC -> N
            //CN -> C".Split(Environment.NewLine);

            var polymer = lines[0];
            var rules = new List<(string src, char dst)>();
            for (int i = 2; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrEmpty(line))
                    continue;
                var parts = line.Split(" -> ");
                var src = parts[0];
                var dst = parts[1];
                rules.Add((src, dst.First()));
            }
            for (int i = 0; i < 10; i++)
            {
                var insertions = new List<(char value, int index)>();
                foreach (var rule in rules)
                {
                    for (int j = -1; ;)
                    {
                        j = polymer.IndexOf(rule.src, j + 1);
                        if (j == -1)
                        {
                            break;
                        }
                        insertions.Add((rule.dst, j + 1));
                    }
                }
                var offset = 0;
                var list = polymer.ToList();
                insertions = insertions.OrderBy(t => t.index).ToList();
                foreach (var insertion in insertions)
                {
                    list.Insert(insertion.index + offset, insertion.value);
                    offset++;
                }
                polymer = new string(list.ToArray());
            }
            var set = polymer.ToHashSet();
            var counts = set.Select(c => (c, polymer.Count(c2 => c2 == c))).OrderBy(t => t.Item2).ToList();
            Console.WriteLine(counts.Last().Item2 - counts.First().Item2);
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void Part2()
        {
            var lines = File.ReadAllLines("input.txt");
            var polymer = lines[0];
            var rules = new Dictionary<int, int>();
            var counts = new Dictionary<int, ulong>();
            var totalCount = new Dictionary<char, ulong>();
            for (int i = 2; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrEmpty(line))
                    continue;
                var parts = line.Split(" -> ");
                var a = parts[0][0];
                var b = parts[0][1];
                var c = parts[1][0];
                rules.Add(a << 16 | b, c);
                counts.Add(a << 16 | b, 0);
            }

            foreach (var rule in rules)
            {
                var a = (char)(rule.Key >> 16);
                var b = (char)(rule.Key);
                for (int i = 0; i < polymer.Length - 1; i++)
                {
                    if (polymer[i] == a && polymer[i + 1] == b)
                    {
                        counts[rule.Key]++;
                    }
                }
            }

            for (int i = 0; i < polymer.Length; i++)
            {
                if (!totalCount.ContainsKey(polymer[i]))
                    totalCount.Add(polymer[i], 0);
                totalCount[polymer[i]]++;
            }

            for (int i = 0; i < 40; i++)
            {
                var oldCount = new Dictionary<int, ulong>(counts);
                foreach (var rule in rules)
                {
                    var a = (char)(rule.Key >> 16);
                    var b = (char)(rule.Key);
                    var c = (char)rule.Value;
                    var amount = oldCount[rule.Key];
                    if (!totalCount.ContainsKey(c))
                        totalCount.Add(c, 0);
                    totalCount[c] += amount;
                    counts[a << 16 | c] += amount;
                    counts[c << 16 | b] += amount;
                    counts[rule.Key] -= amount;
                }
            }

            Console.WriteLine(totalCount.Max(kv => kv.Value) - totalCount.Min(kv => kv.Value));
        }
    }
}
