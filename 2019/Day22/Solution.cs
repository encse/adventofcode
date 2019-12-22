using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

namespace AdventOfCode.Y2019.Day22 {

    class Solution : Solver {

        public string GetName() => "Slam Shuffle";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) {
            return Shuffle2(input, 10007)(2019);
        }

        Func<long, long> Shuffle(string input, long m, long iter = 1) {
            var steps = new List<Func<long, long>>();

            foreach (var line in input.Split('\n')) {
                if (line.Contains("into new stack")) {
                    steps.Add((x) => (m - x - 1) % m);
                } else if (line.Contains("cut")) {
                    var n = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    steps.Add((x) => {
                        return (x - n + m) % m;
                    });
                } else if (line.Contains("increment")) {
                    var n = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    steps.Add((x) => {
                        return (long)BigInteger.ModPow(BigInteger.Multiply(x, n), 1, m);
                    });
                } else {
                    throw new Exception();
                }
            }

            return (long z) => {
                for (var i = 0; i < iter; i++) {
                    foreach (var step in steps) {
                        z = step(z);
                    }
                }
                return z;
            };
        }


        Func<long, long> Shuffle2(string input, long m) {
            var a = new BigInteger(1);
            var b = new BigInteger(0);

            foreach (var line in input.Split('\n')) {
                if (line.Contains("into new stack")) {
                    // y = (ax + b)
                    // (m - y - 1) % m)
                    a *= -1;
                    b = m - b - 1;
                } else if (line.Contains("cut")) {
                    var n = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    // a = a;
                    b = b - n + m;
                } else if (line.Contains("increment")) {
                    var n = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    a *= n;
                    b *= n;
                } else {
                    throw new Exception();
                }
            }


            return (long z) => {
                var x = new BigInteger(z);
                var y = a * x;
                return (long)(((a * x + b) % m) + m) % m;
            };
        }

        Func<long, long> InvertShuffle2(string input, long m) {

            var a = new BigInteger(1);
            var b = new BigInteger(0);

            foreach (var line in input.Split('\n')) {
                if (line.Contains("into new stack")) {
                    // y = (ax + b)
                    // (m - y - 1) % m)
                    a *= -1;
                    b = m - b - 1;
                } else if (line.Contains("cut")) {
                    var n = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    // a = a;
                    b = b - n + m;
                } else if (line.Contains("increment")) {
                    var n = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    a *= n;
                    b *= n;
                } else {
                    throw new Exception();
                }
            }

            return (long z) => {
                var r = new BigInteger(z);
                r -= b;
                var inverse = (long)BigInteger.ModPow(a, m - 2, m);
                return (long)BigInteger.ModPow(BigInteger.Multiply(r, inverse), 1, m);
            };
        }


        Func<long, long> InvertShuffleT(string input, long m, long t) {

            var a = new BigInteger(1);
            var b = new BigInteger(0);

            foreach (var line in input.Split('\n')) {
                if (line.Contains("into new stack")) {
                    // y = (ax + b)
                    // (m - y - 1) % m)
                    a *= -1;
                    b = m - b - 1;
                } else if (line.Contains("cut")) {
                    var n = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    // a = a;
                    b = b - n + m;
                } else if (line.Contains("increment")) {
                    var n = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    a *= n;
                    b *= n;
                } else {
                    throw new Exception();
                }
            }


            var resA = new BigInteger(1);
            var resB = new BigInteger(0);

            var a2 = a;
            var b2 = b;

            while (t > 0) {
                if (t % 2 == 1) {
                    (resA, resB) = ((resA * a2) % m, (resB * a2 + b2) % m);
                }
                (a2, b2) = ((a2 * a2) % m, (b2 * a2 + b2) % m);
                t >>= 1;
            }

            return (long z) => {
                var r = new BigInteger(z);
                r -= resB;
                var inverse = (long)BigInteger.ModPow(resA, m - 2, m);
                return (long)BigInteger.ModPow(BigInteger.Multiply(r, inverse), 1, m);
            };
        }

        Func<long, long> InvertShuffle(string input, long m) {
            var steps = new List<Func<long, long>>();

            foreach (var line in input.Split('\n')) {
                if (line.Contains("into new stack")) {
                    steps.Add((i) => m - i - 1);
                } else if (line.Contains("cut")) {
                    var n = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    steps.Add((i) => {
                        return (i + n + m) % m;
                    });
                } else if (line.Contains("increment")) {
                    var n = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    var inverse = (long)BigInteger.ModPow(n, m - 2, m);
                    steps.Add((i) => {
                        return (long)BigInteger.ModPow(BigInteger.Multiply(i, inverse), 1, m);
                    });
                } else {
                    throw new Exception();
                }
            }
            steps.Reverse();

            return (long z) => {
                foreach (var step in steps) {
                    z = step(z);
                }
                return z;
            };
        }
        long PartTwo(string input) {
            var m = 119315717514047;
            var t = 101741582076661;
            var iter = t;
            // var shuffle = Shuffle(input, m, iter);
            // var invertShuffle = InvertShuffle2(input, m);



            var invertShuffle = InvertShuffleT(input, m, iter);

            // for (var i = 0; i < 1000; i++) {

            //     var x = shuffle(i);
            //     var q = InvertShuffleT(input, m, iter)(x);
            //     if (q != i) {
            //         throw new Exception();
            //     }
            // }
            Console.WriteLine("test opk");
            return invertShuffle(2020L);
        }
    }
}