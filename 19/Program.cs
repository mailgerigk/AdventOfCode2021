using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace _19
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            var p = new Program();
            p.Part1And2();
            Console.WriteLine(sw.Elapsed);
        }

        public void Part1And2()
        {
            const float DegToRad = MathF.PI / 180;

            var ups = new Vector3[]
            {
                Vector3.UnitX,
                -Vector3.UnitX,
                Vector3.UnitY,
                -Vector3.UnitY,
                Vector3.UnitZ,
                -Vector3.UnitZ,
            };
            var rotations = new float[]
            {
                0,
                90 * DegToRad,
                180 * DegToRad,
                270 * DegToRad
            };

            var axisSwaps = new Func<Point3, Point3>[]
            {
                p => new Point3 { X= p.X, Y= p.Y, Z = p.Z },
                p => new Point3 { X= p.X, Y= p.Z, Z = p.Y },
                p => new Point3 { X= p.Y, Y= p.X, Z = p.Z },
                p => new Point3 { X= p.Y, Y= p.Z, Z = p.X },
                p => new Point3 { X= p.Z, Y= p.X, Z = p.Y },
                p => new Point3 { X= p.Z, Y= p.Y, Z = p.X },
            };

            var axisNegations = new Func<Point3, Point3>[]
            {
                p => new Point3 { X= p.X, Y= p.Y, Z = p.Z },
                p => new Point3 { X= p.X, Y= p.Y, Z = -p.Z },
                p => new Point3 { X= p.X, Y= -p.Y, Z = p.Z },
                p => new Point3 { X= p.X, Y= -p.Y, Z = -p.Z },
                p => new Point3 { X= -p.X, Y= p.Y, Z = p.Z },
                p => new Point3 { X= -p.X, Y= p.Y, Z = -p.Z },
                p => new Point3 { X= -p.X, Y= -p.Y, Z = p.Z },
                p => new Point3 { X= -p.X, Y= -p.Y, Z = -p.Z },
            };

            var lines = File.ReadAllLines("input.txt");
            var positions = new List<Point3>();
            var scanners = new List<Scanner>();
            foreach (var line in lines)
            {
                if (line.StartsWith("---"))
                {
                    if (positions.Any())
                    {
                        var scanner = new   Scanner();
                        foreach (var swap in axisSwaps)
                        {
                            foreach (var negation in axisNegations)
                            {
                                var c = new ScannerConfig();
                                foreach (var beacon in positions)
                                {
                                    c.Beacons.Add(swap(negation(beacon)));
                                }
                                scanner.Configs.Add(c);
                            }
                        }
                        scanners.Add(scanner);
                    }
                    positions.Clear();
                    continue;
                }
                if (string.IsNullOrEmpty(line))
                    continue;

                var parts = line.Split(",").Select(int.Parse).ToArray();
                var position = new Point3 { X =parts[0], Y= parts[1], Z= parts[2] };
                positions.Add(position);
            }
            if (positions.Any())
            {
                var scanner = new   Scanner();
                foreach (var swap in axisSwaps)
                {
                    foreach (var negation in axisNegations)
                    {
                        var c = new ScannerConfig();
                        foreach (var beacon in positions)
                        {
                            c.Beacons.Add(swap(negation(beacon)));
                        }
                        scanner.Configs.Add(c);
                    }
                }
                scanners.Add(scanner);
            }

            var map = new Map();
            foreach (var beacon in scanners[0].Configs[0].Beacons)
            {
                map.Beacons.Add(beacon);
            }
            scanners.RemoveAt(0);

            while (scanners.Any())
            {
                var lockObj = new object();
                var found = 0L;
                var scannerI = 0;
                var configI = 0;
                var mapI = 0;
                var beaconI = 0;

                Parallel.For(0, scanners.Count, i =>
                {
                    var scanner = scanners[i];
                    Parallel.For(0, scanner.Configs.Count, j =>
                    {
                        var config = scanner.Configs[j];
                        for (int k = 0; k < config.Beacons.Count; k++)
                        {
                            var configPoint = config.Beacons[k];
                            var configView = new ListView()
                            {
                                Perspektive = configPoint,
                                Points = config.Beacons,
                            };
                            for (int h = 0; h < map.Beacons.Count; h++)
                            {
                                var mapPoint = map.Beacons.ElementAt(h);
                                var mapView = new ListView()
                                {
                                    Perspektive = mapPoint,
                                    Points = map.Beacons,
                                };
                                var configViewArray = configView.ToArray();
                                var mapViewArray = mapView.ToArray();

                                int sum = 0;
                                foreach (var configViewPoint in configViewArray)
                                {
                                    foreach (var mapViewPoint in mapViewArray)
                                    {
                                        if (configViewPoint.X == mapViewPoint.X && configViewPoint.Y == mapViewPoint.Y && configViewPoint.Z == mapViewPoint.Z)
                                        {
                                            sum++;
                                            break;
                                        }
                                    }
                                }
                                if (Interlocked.Read(ref found) == 1)
                                {
                                    return;
                                }
                                if (sum >= 12)
                                {
                                    lock (lockObj)
                                    {
                                        found = 1;
                                        scannerI = i;
                                        configI = j;
                                        beaconI = k;
                                        mapI = h;
                                    }
                                    return;
                                }
                            }
                        }
                    });
                });

                Debug.Assert(found == 1);
                var scanner = scanners.ElementAt(scannerI);
                var config = scanner.Configs.ElementAt(configI);
                var mapPoint = map.Beacons.ElementAt(mapI);
                var configPoint = config.Beacons.ElementAt(beaconI);

                foreach (var configViewPoint in new ListView() { Perspektive = configPoint, Points = config.Beacons, })
                {
                    var pointOnMap = new Point3
                    {
                        X = mapPoint.X + configViewPoint.X,
                        Y = mapPoint.Y + configViewPoint.Y,
                        Z = mapPoint.Z + configViewPoint.Z,
                    };
                    map.Beacons.Add(pointOnMap);
                }
                map.Scanners.Add(new Point3 { X = mapPoint.X - configPoint.X, Y = mapPoint.Y - configPoint.Y, Z = mapPoint.Z - configPoint.Z });
                scanners.Remove(scanner);
            }
            Console.WriteLine(map.Beacons.Count);
            int maxDistance = 0;
            foreach (var a in map.Scanners)
            {
                foreach (var b in map.Scanners)
                {
                    maxDistance = Math.Max(maxDistance, a.Distance(b));
                }
            }
            Console.WriteLine(maxDistance);
        }
    }

    struct Point3
    {
        public int X, Y, Z;

        public int Distance(Point3 other)
        {
            return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
        }

        public override string ToString() => $"{X} {Y} {Z}";
    }

    class ScannerConfig
    {
        public List<Point3> Beacons = new();
    }

    class Scanner
    {
        public List<ScannerConfig> Configs = new();
    }

    class Map
    {
        public HashSet<Point3> Beacons = new();
        public HashSet<Point3> Scanners = new();
    }

    class ListView : IEnumerable<Point3>
    {
        public Point3 Perspektive;
        public IEnumerable<Point3> Points;

        public IEnumerator<Point3> GetEnumerator()
        {
            return new Enumerator
            {
                Perspektive = Perspektive,
                enumerator = Points.GetEnumerator(),
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator
            {
                Perspektive = Perspektive,
                enumerator = Points.GetEnumerator(),
            };
        }

        class Enumerator : IEnumerator<Point3>
        {
            public Point3 Perspektive;
            public IEnumerator<Point3> enumerator;

            public Point3 Current { get; private set; }

            public bool MoveNext()
            {
                var v = enumerator.MoveNext();
                if (v)
                {
                    Current = new Point3
                    {
                        X = enumerator.Current.X - Perspektive.X,
                        Y = enumerator.Current.Y - Perspektive.Y,
                        Z = enumerator.Current.Z - Perspektive.Z,
                    };
                }
                return v;
            }

            public void Reset()
            {
                enumerator.Reset();
            }

            object IEnumerator.Current => Current;

            public void Dispose()
            {

            }
        }
    }
}
