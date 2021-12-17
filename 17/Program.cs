using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace _17
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

        static bool IsBetween(int value, int min, int max) => value <= max && min <= value;
        static int Gauss(int n) => n * (n + 1) / 2;

        [BenchmarkDotNet.Attributes.Benchmark]
        public void Part1()
        {
            var input = File.ReadAllText("input.txt");
            //input = "target area: x=20..30, y=-10..-5";
            input = input.Substring("target area:".Length);
            var parts = input.Split(",", StringSplitOptions.TrimEntries);
            parts[0] = parts[0].Substring("x=".Length);
            parts[1] = parts[1].Substring("y=".Length);
            var xparts = parts.First().Split("..");
            var xmin = int.Parse(xparts.First());
            var xmax = int.Parse(xparts.Last());
            (xmin, xmax) = (Math.Min(xmin, xmax), Math.Max(xmin, xmax));
            var yparts = parts.Last().Split("..");
            var ymin = int.Parse(yparts.First());
            var ymax = int.Parse(yparts.Last());
            (ymin, ymax) = (Math.Min(ymin, ymax), Math.Max(ymin, ymax));

            var heights = Enumerable.Range(0, 1000).Select(Gauss).ToArray();
            var heighest = 0;
            for (int startVelocity = 0; startVelocity < heights.Length; startVelocity++)
            {
                for (int time = 0; time < heights.Length; time++)
                {
                    var difference = heights[startVelocity] - heights[time];
                    if (difference < ymin)
                    {
                        break;
                    }
                    if (IsBetween(difference, ymin, ymax))
                    {
                        heighest = Math.Max(heights[startVelocity], heighest);
                        break;
                    }

                }
            }
            Console.WriteLine(heighest);
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void Part2()
        {
            var input = File.ReadAllText("input.txt");
            //input = "target area: x=20..30, y=-10..-5";
            input = input.Substring("target area:".Length);
            var parts = input.Split(",", StringSplitOptions.TrimEntries);
            parts[0] = parts[0].Substring("x=".Length);
            parts[1] = parts[1].Substring("y=".Length);
            var xparts = parts.First().Split("..");
            var xmin = int.Parse(xparts.First());
            var xmax = int.Parse(xparts.Last());
            (xmin, xmax) = (Math.Min(xmin, xmax), Math.Max(xmin, xmax));
            var yparts = parts.Last().Split("..");
            var ymin = int.Parse(yparts.First());
            var ymax = int.Parse(yparts.Last());
            (ymin, ymax) = (Math.Min(ymin, ymax), Math.Max(ymin, ymax));

            var timeX = new List<Range?>();
            var timeY = new List<Range?>();

            var min = Math.Min(ymin, xmin);
            var max = Math.Max(ymax, xmax) * 2;

            var gaussLookup = Enumerable.Range(0, max).Select(Gauss).ToArray();

            for (int i = min; i < max; i++)
            {
                var vY = i;
                var pY = 0;
                int? beginY = null;
                int? endY = null;
                for (int time = 0; time < max; time++)
                {
                    if (pY < ymin)
                    {
                        if (beginY is not null)
                            endY = time;
                        break;
                    }
                    if (beginY is null && IsBetween(pY, ymin, ymax))
                    {
                        beginY = time;
                    }
                    pY += vY--;
                }
                timeY.Add(beginY is not null ? new Range(beginY.Value, endY.Value) : null);

                int? beginX = null;
                int? endX = null;
                var iX = Math.Max(i, 0);
                int staticX = gaussLookup[iX];
                if (staticX >= xmin)
                {
                    for (int time = 0; time <= iX; time++)
                    {
                        var position = staticX - gaussLookup[Math.Max(iX - time, 0)];
                        beginX ??= IsBetween(position, xmin, xmax) ? time : null;
                        if (beginX is not null && endX is null && !IsBetween(position, xmin, xmax))
                            endX = time;
                    }
                    if(beginX is not null && endX is null)
                    {
                        endX = int.MaxValue;
                    }
                }
                timeX.Add(beginX is not null ? new Range(beginX.Value, endX.Value) : null);
            }

            var set = new HashSet<(int x, int y)>();
            for (int y = 0; y < timeY.Count; y++)
            {
                if (timeY[y] is Range ry)
                {
                    for (int x = 0; x < timeX.Count; x++)
                    {
                        if(timeX[x] is Range rx)
                        {
                            if(ry.Overlap(rx) > 0)
                            {
                                set.Add((x, y));
                            }
                        }
                    }
                }
            }
            Console.WriteLine(set.Count);
        }

        struct Range
        {
            public int Begin;
            public int End;

            public Range(int begin, int end) => (Begin, End) = (begin, end);

            public int Overlap(Range? r) => r is null ? 0 : Math.Max(Math.Min(End, r.Value.End) - Math.Max(Begin, r.Value.Begin), 0);

            public override string ToString() => $"{Begin}..{End}";
        }
    }
}
