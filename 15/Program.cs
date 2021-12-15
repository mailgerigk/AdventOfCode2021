using System;
using System.IO;
using System.Linq;

namespace _15
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
            var lines = File.ReadAllLines("input.txt").Where(line => !string.IsNullOrEmpty(line));
            //            lines = @"1163751742
            //1381373672
            //2136511328
            //3694931569
            //7463417111
            //1319128137
            //1359912421
            //3125421639
            //1293138521
            //2311944581".Split(Environment.NewLine);
            var grid = lines.Select(line => line.Select(c => c - '0').ToArray()).ToArray();
            var cost = new (int x, int y, int cost)[grid.Length][];
            for (int i = 0; i < grid.Length; i++)
            {
                cost[i] = new (int x, int y, int cost)[grid[i].Length];
                for (int k = 0; k < cost[i].Length; k++)
                {
                    cost[i][k] = (0, 0, int.MaxValue - 10);
                }
            }
            cost[cost.Length - 1][cost[cost.Length - 1].Length - 1] = (0, 0, 0);

            var changed = false;
            do
            {
                changed = false;
                for (int y = 0; y < grid.Length; y++)
                {
                    for (int x = 0; x < grid[y].Length; x++)
                    {
                        var currentCost = cost[y][x].cost;
                        for (int dy = -1; dy < 2; dy++)
                        {
                            for (int dx = -1; dx < 2; dx++)
                            {
                                if (!(Math.Abs(dy) == 0 ^ Math.Abs(dx) == 0))
                                    continue;

                                if (0 <= y + dy && y + dy < grid.Length && 0 <= x + dx && x + dx < grid[y].Length && currentCost > cost[y + dy][x + dx].cost + grid[y + dy][x + dx])
                                {
                                    cost[y][x] = (dx, dy, cost[y + dy][x + dx].cost + grid[y + dy][x + dx]);
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            } while (changed);

            Console.WriteLine(cost[0][0].cost);
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void Part2()
        {
            var lines = File.ReadAllLines("input.txt").Where(line => !string.IsNullOrEmpty(line));
            //            lines = @"1163751742
            //1381373672
            //2136511328
            //3694931569
            //7463417111
            //1319128137
            //1359912421
            //3125421639
            //1293138521
            //2311944581".Split(Environment.NewLine);
            //lines = new string[] { "8" };
            var grid = lines.Select(line => line.Select(c => c - '0').ToArray()).ToArray();
            var bigGrid = new int[grid.Length * 5][];
            for (int y = 0; y < grid.Length; y++)
            {
                for (int i = 0; i < 5; i++)
                {
                    bigGrid[y + grid.Length * i] = new int[grid[y].Length * 5];
                }
            }
            for (int y = 0; y < grid.Length; y++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int x = 0; x < grid[y].Length; x++)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            var risk = grid[y][x] + i + j;
                            while (risk > 9)
                            {
                                risk -= 9;
                            }
                            bigGrid[y + grid.Length * j][x + grid[y].Length * i] = risk;
                        }
                    }
                }
            }
            grid = bigGrid;

            var cost = new (int x, int y, int cost)[grid.Length][];
            for (int i = 0; i < grid.Length; i++)
            {
                cost[i] = new (int x, int y, int cost)[grid[i].Length];
                for (int k = 0; k < cost[i].Length; k++)
                {
                    cost[i][k] = (0, 0, int.MaxValue - 10);
                }
            }
            cost[cost.Length - 1][cost[cost.Length - 1].Length - 1] = (0, 0, 0);

            var changed = false;
            do
            {
                changed = false;
                for (int y = grid.Length - 1; y >= 0; y--)
                {
                    for (int x = grid[y].Length - 1; x >= 0; x--)
                    {
                        var currentCost = cost[y][x].cost;
                        for (int dy = -1; dy < 2; dy++)
                        {
                            for (int dx = -1; dx < 2; dx++)
                            {
                                if (!(Math.Abs(dy) == 0 ^ Math.Abs(dx) == 0))
                                    continue;

                                if (0 <= y + dy && y + dy < grid.Length && 0 <= x + dx && x + dx < grid[y].Length && currentCost > cost[y + dy][x + dx].cost + grid[y + dy][x + dx])
                                {
                                    cost[y][x] = (dx, dy, cost[y + dy][x + dx].cost + grid[y + dy][x + dx]);
                                    changed = true;
                                }
                            }
                        }
                    }
                }
            } while (changed);

            Console.WriteLine(cost[0][0].cost);
        }
    }
}
