using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _20
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

        static void DoWork(int count)
        {
            var lines = File.ReadAllLines("input.txt");
            var zoom = lines[0];
            var points = new HashSet<(int x, int y)>();
            for (int y = 2; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    if (lines[y][x] == '#')
                        points.Add((x, y - 2));
                }
            }

            var kernel = new (int x, int y)[]
            {
                (-1, -1), (0, -1), (1, -1),
                (-1,  0), (0,  0), (1,  0),
                (-1,  1), (0,  1), (1,  1),
            };

            var min = (x:-1, y:-1 );
            var max = (x: lines[2].Length + 5, y: lines.Length + 3);

            var bounds = '.';

            var bag = new ConcurrentBag<(int, int)>();
            for (int i = 0; i < count; i++)
            {
                bag.Clear();
                System.Threading.Tasks.Parallel.For(min.y - 1, max.y + 1, y =>
                {
                    for (int x = min.x - 1; x < max.x + 1; x++)
                    {
                        var index = 0;
                        foreach (var offset in kernel)
                        {
                            index <<= 1;
                            if (min.x <= x + offset.x && x + offset.x < max.x && min.y <= y + offset.y && y + offset.y < max.y)
                            {
                                index |= points.Contains((x + offset.x, y + offset.y)) ? 1 : 0;
                            }
                            else
                            {
                                index |= bounds == '#' ? 1 : 0;
                            }
                        }
                        if (zoom[index] == '#')
                        {
                            bag.Add((x, y));
                        }
                    }
                });

                points.Clear();
                foreach (var point in bag)
                    points.Add(point);

                if (bounds == '.')
                {
                    bounds = zoom.First();
                }
                else if (bounds == '#')
                {
                    bounds = zoom.Last();
                }


                min.x -= 1;
                min.y -= 1;
                max.x += 1;
                max.y += 1;
            }

            Console.WriteLine(points.Count);
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void Part1()
        {
            DoWork(2);
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void Part2()
        {
            DoWork(50);
        }
    }
}
