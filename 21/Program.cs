using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _21
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

        void Play100(ref int p, ref int s, ref int dice)
        {
            var diceTotal = 0;

            diceTotal += dice++;
            dice = dice % 101 + dice / 101;

            diceTotal += dice++;
            dice = dice % 101 + dice / 101;

            diceTotal += dice++;
            dice = dice % 101 + dice / 101;

            p = p + diceTotal;
            while(p > 10)
                p = p % 11 + p / 11;
            s += p;
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void Part1()
        {
            var lines = File.ReadAllLines("input.txt");
            var p1 = int.Parse( lines[0].Substring("Player 1 starting position: ".Length));
            var p2 = int.Parse(lines[1].Substring("Player 1 starting position: ".Length));

            var s1 = 0;
            var s2 = 0;

            var dice = 1;
            var count = 0;

            var isP1 = true;
            while (s1 < 1000 && s2 < 1000)
            {
                count += 3;
                if (isP1)
                    Play100(ref p1, ref s1, ref dice);
                else
                    Play100(ref p2, ref s2, ref dice);
                isP1 = !isP1;
            }

            Console.WriteLine(Math.Min(s1, s2) * count);
        }

        (bool p1Turn, int p1Position, int p1Score, int p2Position, int p2Score) Play3((bool p1Turn, int p1Position, int p1Score, int p2Position, int p2Score) tuple, int dice)
        {
            var (p1Turn, p1Position, p1Score, p2Position, p2Score) = tuple;
            if (p1Turn)
                Play(ref p1Position, ref p1Score, dice);
            else
                Play(ref p2Position, ref p2Score, dice);
            return (!p1Turn, p1Position, p1Score, p2Position, p2Score);

            static void Play(ref int p, ref int s, int dice)
            {
                p = p + dice;
                while (p > 10)
                    p = p % 11 + p / 11;
                s += p;
            }
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void Part2()
        {
            var lines = File.ReadAllLines("input.txt");
            var p1 = int.Parse( lines[0].Substring("Player 1 starting position: ".Length));
            var p2 = int.Parse(lines[1].Substring("Player 1 starting position: ".Length));

            var universes = new Dictionary<(bool p1Turn, int p1Position, int p1Score, int p2Position, int p2Score), ulong>();
            universes.Add((true, p1, 0, p2, 0), 1);

            ulong p1Wins = 0;
            ulong p2Wins = 0;

            var diceRolls = new (int dice, ulong count)[]
            {
                (3, 1),
                (4, 3),
                (5, 6),
                (6, 7),
                (7, 6),
                (8, 3),
                (9, 1),
            };


            while (universes.Any())
            {
                var next = new Dictionary<(bool p1Turn, int p1Position, int p1Score, int p2Position, int p2Score), ulong>();
                foreach (var (tuple, amount) in universes)
                {
                    foreach (var roll in diceRolls)
                    {
                        var k = Play3(tuple, roll.dice);
                        var realAmount = amount * roll.count;

                        if (k.p1Score >= 21)
                        {
                            p1Wins += realAmount;
                            continue;
                        }

                        if (k.p2Score >= 21)
                        {
                            p2Wins += realAmount;
                            continue;
                        }

                        if (!next.ContainsKey(k))
                        {
                            next.Add(k, 0);
                        }
                        next[k] += realAmount;
                    }
                }
                universes = next;
            }

            Console.WriteLine(Math.Max(p1Wins, p2Wins));
        }
    }
}
