using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace _16
{
    public class Program
    {
        enum TypeId
        {
            Operator = -1,
            Sum = 0,
            Product = 1,
            Minimum = 2,
            Maximum = 3,
            Literal = 4,
            GreaterThan = 5,
            LessThan = 6,
            EqualTo = 7,
        }

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
            var digits = File.ReadAllText("input.txt").Where(char.IsLetterOrDigit).Select(c => char.IsDigit(c) ? c - '0' : char.ToLower(c) - 'a' + 10).ToArray();
            //digits = "D2FE28".Where(char.IsLetterOrDigit).Select(c => char.IsDigit(c) ? c - '0' : char.ToLower(c) - 'a' + 10).ToArray();
            var bits = new BitArray(digits.Length * 4);
            for (int i = 0, j = 0; i < bits.Length; i += 4, j++)
            {
                bits[i + 3] = (digits[j] & 1) > 0;
                bits[i + 2] = (digits[j] & 2) > 0;
                bits[i + 1] = (digits[j] & 4) > 0;
                bits[i + 0] = (digits[j] & 8) > 0;
            }
            var bitsIndex = 0;
            var totalVersion = 0;

            ParsePacket();

            Console.WriteLine(totalVersion);

            TypeId ParseHeader()
            {
                var v2 = bits[bitsIndex++] ? 1 : 0;
                var v1 = bits[bitsIndex++] ? 1 : 0;
                var v0 = bits[bitsIndex++] ? 1 : 0;
                var version = v2 << 2 | v1 << 1 | v0;

                totalVersion += version;

                var ti2 = bits[bitsIndex++] ? 1 : 0;
                var ti1 = bits[bitsIndex++] ? 1 : 0;
                var ti0 = bits[bitsIndex++] ? 1 : 0;
                var typeId = ti2 << 2 | ti1 << 1 | ti0;

                if (typeId == (long)TypeId.Literal)
                    return TypeId.Literal;
                return TypeId.Operator;
            }

            long ParseLiteral()
            {
                long literal = 0;
            loop:
                var isLast = !bits[bitsIndex++];
                for (long i = 0; i < 4; i++)
                {
                    literal <<= 1;
                    literal |= bits[bitsIndex++] ? 1L : 0L;
                }
                if (!isLast)
                {
                    goto loop;
                }
                return literal;
            }

            void ParseOperator()
            {
                var isPacketCountLength = bits[bitsIndex++];
                if (isPacketCountLength)
                {
                    long packetCount = 0;
                    for (long i = 0; i < 11; i++)
                    {
                        packetCount <<= 1;
                        packetCount |= bits[bitsIndex++] ? 1L : 0L;
                    }

                    for (long i = 0; i < packetCount; i++)
                    {
                        ParsePacket();
                    }
                }
                else
                {
                    // 15 bit, bit count
                    long bitCount = 0;
                    for (long i = 0; i < 15; i++)
                    {
                        bitCount <<= 1;
                        bitCount |= bits[bitsIndex++] ? 1L : 0L;
                    }

                    var startIndex = bitsIndex;
                    while (startIndex + bitCount > bitsIndex)
                    {
                        ParsePacket();
                    }
                }
            }

            void ParsePacket()
            {
                var typeId = ParseHeader();
                if (typeId == TypeId.Literal)
                {
                    ParseLiteral();
                }
                else
                {
                    ParseOperator();
                }
            }
        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void Part2()
        {
            var digits = File.ReadAllText("input.txt").Where(char.IsLetterOrDigit).Select(c => char.IsDigit(c) ? c - '0' : char.ToLower(c) - 'a' + 10).ToArray();
            //digits = "9C0141080250320F1802104A08".Where(char.IsLetterOrDigit).Select(c => char.IsDigit(c) ? c - '0' : char.ToLower(c) - 'a' + 10).ToArray();
            var bits = new BitArray(digits.Length * 4);
            for (int i = 0, j = 0; i < bits.Length; i += 4, j++)
            {
                bits[i + 3] = (digits[j] & 1) > 0;
                bits[i + 2] = (digits[j] & 2) > 0;
                bits[i + 1] = (digits[j] & 4) > 0;
                bits[i + 0] = (digits[j] & 8) > 0;
            }
            var bitsIndex = 0;

            Console.WriteLine(ParsePacket());

            TypeId ParseHeader()
            {
                var v2 = bits[bitsIndex++] ? 1 : 0;
                var v1 = bits[bitsIndex++] ? 1 : 0;
                var v0 = bits[bitsIndex++] ? 1 : 0;
                var version = v2 << 2 | v1 << 1 | v0;


                var ti2 = bits[bitsIndex++] ? 1 : 0;
                var ti1 = bits[bitsIndex++] ? 1 : 0;
                var ti0 = bits[bitsIndex++] ? 1 : 0;
                var typeId = ti2 << 2 | ti1 << 1 | ti0;

                if (typeId == (long)TypeId.Sum) return TypeId.Sum;
                if (typeId == (long)TypeId.Product) return TypeId.Product;
                if (typeId == (long)TypeId.Minimum) return TypeId.Minimum;
                if (typeId == (long)TypeId.Maximum) return TypeId.Maximum;
                if (typeId == (long)TypeId.Literal) return TypeId.Literal;
                if (typeId == (long)TypeId.GreaterThan) return TypeId.GreaterThan;
                if (typeId == (long)TypeId.LessThan) return TypeId.LessThan;
                if (typeId == (long)TypeId.EqualTo) return TypeId.EqualTo;
                return TypeId.Operator;
            }

            long ParseLiteral()
            {
                long literal = 0;
            loop:
                var isLast = !bits[bitsIndex++];
                for (long i = 0; i < 4; i++)
                {
                    literal <<= 1;
                    literal |= bits[bitsIndex++] ? 1L : 0L;
                }
                if (!isLast)
                {
                    goto loop;
                }
                return literal;
            }

            long ParseOperator(TypeId id)
            {
                long? result = null;
                var isPacketCountLength = bits[bitsIndex++];
                if (isPacketCountLength)
                {
                    long packetCount = 0;
                    for (long i = 0; i < 11; i++)
                    {
                        packetCount <<= 1;
                        packetCount |= bits[bitsIndex++] ? 1L : 0L;
                    }

                    for (long i = 0; i < packetCount; i++)
                    {
                        if(result is null)
                        {
                            result = ParsePacket();
                            continue;
                        }
                        switch (id)
                        {
                            case TypeId.Sum:
                                result += ParsePacket();
                                break;
                            case TypeId.Product:
                                result *= ParsePacket();
                                break;
                            case TypeId.Minimum:
                                result = Math.Min(result.Value, ParsePacket());
                                break;
                            case TypeId.Maximum:
                                result = Math.Max(result.Value, ParsePacket());
                                break;
                            case TypeId.GreaterThan:
                                result = result.Value > ParsePacket() ? 1 : 0;
                                break;
                            case TypeId.LessThan:
                                result = result.Value < ParsePacket() ? 1 : 0;
                                break;
                            case TypeId.EqualTo:
                                result = result.Value == ParsePacket() ? 1 : 0;
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    // 15 bit, bit count
                    long bitCount = 0;
                    for (long i = 0; i < 15; i++)
                    {
                        bitCount <<= 1;
                        bitCount |= bits[bitsIndex++] ? 1L : 0L;
                    }

                    var startIndex = bitsIndex;
                    while (startIndex + bitCount > bitsIndex)
                    {
                        if (result is null)
                        {
                            result = ParsePacket();
                            continue;
                        }
                        switch (id)
                        {
                            case TypeId.Sum:
                                result += ParsePacket();
                                break;
                            case TypeId.Product:
                                result *= ParsePacket();
                                break;
                            case TypeId.Minimum:
                                result = Math.Min(result.Value, ParsePacket());
                                break;
                            case TypeId.Maximum:
                                result = Math.Max(result.Value, ParsePacket());
                                break;
                            case TypeId.GreaterThan:
                                result = result.Value > ParsePacket() ? 1 : 0;
                                break;
                            case TypeId.LessThan:
                                result = result.Value < ParsePacket() ? 1 : 0;
                                break;
                            case TypeId.EqualTo:
                                result = result.Value == ParsePacket() ? 1 : 0;
                                break;
                            default:
                                break;
                        }
                    }
                }
                return result.Value;
            }

            long ParsePacket()
            {
                var typeId = ParseHeader();
                if (typeId == TypeId.Literal)
                {
                    return ParseLiteral();
                }
                else
                {
                    return ParseOperator(typeId);
                }
            }
        }
    }
}
