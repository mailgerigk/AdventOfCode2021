using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace _18
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
            var lines = File.ReadAllLines("input.txt").Select(Node.Parse);
            Node node;
            node = lines.First();
            for (int i = 1; i < lines.Count(); i++)
            {
                node = Node.Add(node, lines.ElementAt(i));
            }
            Console.WriteLine(node.GetMagnitude());
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void Part2()
        {
            var lines = File.ReadAllLines("input.txt").Select(Node.Parse).ToArray();

            var max = 0;
            for (int a = 0; a < lines.Length; a++)
            {
                for (int b = 0; b < lines.Length; b++)
                {
                    max = Math.Max(max, GetMagnitude(a, b));
                }
            }
            Console.WriteLine(max);

            static int GetMagnitude(int a, int b)
            {
                var array = File.ReadAllLines("input.txt").Select(Node.Parse).ToArray();
                return Node.Add(array[a], array[b]).GetMagnitude();
            }
        }
    }

    abstract class Node
    {
        public Node Parent;

        public Literal AsLiteral => (Literal)this;
        public Pair AsPair => (Pair)this;

        public int Depth => 1 + (Parent == null ? 0 : Parent.Depth);

        public static Node Add(Node left, Node right)
        {
            var pair = new Pair{ Left = left, Right = right};
            left.Parent = pair;
            right.Parent = pair;

            var changed = false;
            do
            {
                changed = false;

                // explode
                var iterator = 1;
                Pair nextPair;
                do
                {
                    var iteratorRef = iterator++;
                    nextPair = pair.NextPair(ref iteratorRef);
                    if (nextPair != null && nextPair.Depth == 5)
                    {
                        var leftLiteral = pair.IteratorOfLiteral( nextPair.Left);
                        var rightLiteral = pair.IteratorOfLiteral( nextPair.Right);
                        Debug.Assert(leftLiteral + 1 == rightLiteral || leftLiteral < 0 || rightLiteral < 0);

                        leftLiteral--;
                        var leftLiteralNode = pair.NextLiteral(ref leftLiteral);

                        rightLiteral++;
                        var rightLiteralNode = pair.NextLiteral(ref rightLiteral);

                        if (leftLiteralNode != null)
                            leftLiteralNode.Value += nextPair.Left.AsLiteral.Value;
                        if (rightLiteralNode != null)
                            rightLiteralNode.Value += nextPair.Right.AsLiteral.Value;

                        var parent = nextPair.Parent;
                        if (parent.AsPair.Left == nextPair)
                        {
                            parent.AsPair.Left = new Literal { Value = 0 };
                            parent.AsPair.Left.Parent = parent;
                        }
                        else
                        {
                            parent.AsPair.Right = new Literal { Value = 0 };
                            parent.AsPair.Right.Parent = parent;
                        }

                        changed = true;
                        break;
                    }
                } while (nextPair != null);

                if (changed)
                    continue;

                // split
                iterator = 1;
                Literal nextLiteral;
                do
                {
                    var iteratorRef = iterator++;
                    nextLiteral = pair.NextLiteral(ref iteratorRef);
                    if (nextLiteral != null && nextLiteral.Value > 9)
                    {
                        var parent = nextLiteral.Parent;
                        if (parent.AsPair.Left == nextLiteral)
                        {
                            parent.AsPair.Left = new Pair { Left = new Literal { Value = nextLiteral.Value / 2 }, Right = new Literal { Value = (nextLiteral.Value + 1) / 2 } };
                            parent.AsPair.Left.Parent = parent;
                            parent.AsPair.Left.AsPair.Left.Parent = parent.AsPair.Left;
                            parent.AsPair.Left.AsPair.Right.Parent = parent.AsPair.Left;
                        }
                        else
                        {
                            parent.AsPair.Right = new Pair { Left = new Literal { Value = nextLiteral.Value / 2 }, Right = new Literal { Value = (nextLiteral.Value + 1) / 2 } };
                            parent.AsPair.Right.Parent = parent;
                            parent.AsPair.Right.AsPair.Left.Parent = parent.AsPair.Right;
                            parent.AsPair.Right.AsPair.Right.Parent = parent.AsPair.Right;
                        }

                        changed = true;
                        break;
                    }
                } while (nextLiteral != null);

            } while (changed);

            return pair;
        }

        public abstract int GetMagnitude();

        public abstract Literal NextLiteral(ref int iterator);

        public abstract Pair NextPair(ref int iterator);

        public abstract int IteratorOfLiteral(Node node);

        public abstract int IteratorOfPair(Node node);

        public static Node Parse(string s)
        {
            var offset = 0;
            return Parse(s, ref offset);
        }

        static Node Parse(string s, ref int offset)
        {
            if (s[offset] == '[')
            {
                offset++;
                var left = Parse(s, ref offset);
                Debug.Assert(s[offset] == ',');
                offset++;
                var right = Parse(s, ref offset);
                Debug.Assert(s[offset] == ']');
                offset++;
                var pair = new Pair { Left = left, Right = right };
                left.Parent = pair;
                right.Parent = pair;
                return pair;
            }
            else if (char.IsNumber(s[offset]))
            {
                var value = s[offset++] - '0';
                return new Literal { Value = value };
            }
            throw new NotSupportedException();
        }
    }

    class Pair : Node
    {
        public Node Left;
        public Node Right;

        public override int GetMagnitude() => 3 * Left.GetMagnitude() + 2 * Right.GetMagnitude();

        public override Literal NextLiteral(ref int iterator)
        {
            var left = Left.NextLiteral(ref iterator);
            if (left == null)
                return Right.NextLiteral(ref iterator);
            return left;
        }

        public override Pair NextPair(ref int iterator)
        {
            iterator--;
            if (iterator == 0)
                return this;
            var left = Left.NextPair(ref iterator);
            if (left == null)
                return Right.NextPair(ref iterator);
            return left;
        }

        public override string ToString() => $"[{Left},{Right}]";

        public override int IteratorOfLiteral(Node node)
        {
            var left = Left.IteratorOfLiteral(node);
            if (left < 0)
            {
                var right = Right.IteratorOfLiteral(node);
                if (right < 0)
                    return left + right;
                return right + Math.Abs(left);
            }
            return left;
        }

        public override int IteratorOfPair(Node node)
        {
            if (node == this)
            {
                return 1;
            }
            var left = Left.IteratorOfPair(node);
            if (left < 0)
            {
                var right = Right.IteratorOfPair(node);
                if (right < 0)
                    return left + right + -1;
                return 1 + right + Math.Abs(left);
            }
            return 1 + left;
        }
    }

    class Literal : Node
    {
        public int Value;

        public override int GetMagnitude() => Value;

        public override Literal NextLiteral(ref int iterator)
        {
            iterator--;
            if (iterator == 0)
                return this;
            return null;
        }

        public override Pair NextPair(ref int iterator)
        {
            return null;
        }

        public override string ToString() => $"{Value}";

        public override int IteratorOfLiteral(Node node)
        {
            if (node == this)
                return 1;
            return -1;
        }

        public override int IteratorOfPair(Node node)
        {
            return 0;
        }
    }
}
