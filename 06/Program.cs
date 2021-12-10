using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

public class P
{
    static void Main()
    {
        //BenchmarkRunner.Run<T>();
        var t = new T();
        t.Part2();
    }

    public class T
    {
        [Benchmark]
        public void Part2()
        {
            var start = Stopwatch.GetTimestamp();
            var input = File.ReadAllLines("input.txt")[0].Split(",").Select(int.Parse).ToArray();
            var dict0 = BigInteger.Zero;
            var dict1 = BigInteger.Zero;
            var dict2 = BigInteger.Zero;
            var dict3 = BigInteger.Zero;
            var dict4 = BigInteger.Zero;
            var dict5 = BigInteger.Zero;
            var dict6 = BigInteger.Zero;
            var dict7 = BigInteger.Zero;
            var dict8 = BigInteger.Zero;
            foreach (var number in input)
            {
                switch (number)
                {
                    case 0: dict0++; break;
                    case 1: dict1++; break;
                    case 2: dict2++; break;
                    case 3: dict3++; break;
                    case 4: dict4++; break;
                    case 5: dict5++; break;
                    case 6: dict6++; break;
                    case 7: dict7++; break;
                    case 8: dict8++; break;
                    default:
                        break;
                }
            }    
            for (int d = 0; d < 20_000; d++)
            {
                var new_fish = dict0;
                dict0 = dict1;
                dict1 = dict2;
                dict2 = dict3;
                dict3 = dict4;
                dict4 = dict5;
                dict5 = dict6;
                dict6 = dict7 + new_fish;
                dict7 = dict8;
                dict8 = new_fish;
            }
            Console.WriteLine(dict0 + dict1 + dict2 + dict3 + dict4 + dict5 + dict6 + dict7 + dict8);
            var end = Stopwatch.GetTimestamp();
            Console.WriteLine(TimeSpan.FromTicks(end -start));
        }

        [Benchmark]
        public void Part1()
        {
            var input = File.ReadAllLines("input.txt")[0].Split(",").Select(int.Parse).ToArray();
            var dict = new Dictionary<int, long>();
            for (int i = -1; i < 9; i++)
                dict.Add(i, 0);
            foreach (var number in input)
                dict[number]++;
            for (int d = 0; d < 80; d++)
            {
                for (int i = 0; i < 9; i++)
                    dict[i - 1] = dict[i];
                dict[8] = dict[-1];
                dict[6] += dict[-1];
                dict[-1] = 0;
            }
            Console.WriteLine(dict.Sum(kv => kv.Value));
        }
    }
}


