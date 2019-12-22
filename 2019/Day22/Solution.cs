using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Numerics;

namespace AdventOfCode.Y2019.Day22 {

    class Solution : Solver {

        public string GetName() => "Slam Shuffle";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        BigInteger PartOne(string input) {
            var m = 10007;
            var iter = 1;
            var (a, b) = Parse(input, m, iter);
            return Mod(a * 2019 + b, m);
        }

        BigInteger PartTwo(string input) {
            var m = 119315717514047;
            var iter = 101741582076661;
            var (a, b) = Parse(input, m, iter);

            return Mod(ModInv(a, m) * (2020 - b), m);
        }

        BigInteger Mod(BigInteger a, BigInteger m) => ((a % m) + m) % m;
        BigInteger ModInv(BigInteger a, BigInteger m) => BigInteger.ModPow(a, m - 2, m);

        (BigInteger a, BigInteger big) Parse(string input, long m, long n) {
            var a = new BigInteger(1);
            var b = new BigInteger(0);

            foreach (var line in input.Split('\n')) {
                if (line.Contains("into new stack")) {
                    a = -a;
                    b = m - b - 1;
                } else if (line.Contains("cut")) {
                    var i = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    b = m + b - i;
                } else if (line.Contains("increment")) {
                    var i = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    a *= i;
                    b *= i;
                } else {
                    throw new Exception();
                }
            }

            var resA = BigInteger.One;
            var resB = BigInteger.Zero;

            // resA = a^n
            resA = BigInteger.ModPow(a, n, m);
            // resB = b * (1 + a + a^2 + ... a^n) = b * (a^n - 1) / (a - 1);
            resB = b * (BigInteger.ModPow(a, n, m) - 1) * ModInv(a - 1, m) % m;

            return (resA, resB);
        }
    }
}