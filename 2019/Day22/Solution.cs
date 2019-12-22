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
            var p = 10007;
            var iter = 1;
            var (a, b) = Parse(input, p, iter);
            return Mod(a * 2019 + b, p);
        }

        BigInteger PartTwo(string input) {
            var p = 119315717514047;
            var iter = 101741582076661;
            var (a, b) = Parse(input, p, iter);

            var a_i = BigInteger.ModPow(a, p - 2, p);

            // modular inverse a'la little Fermat: a_i * a ≡ 1 (m) 
            return Mod(a_i * (2020 - b) , p); 
        }

        BigInteger Mod(BigInteger a, BigInteger m) => ((a % m) + m) % m;

        (BigInteger a, BigInteger big) Parse(string input, long m, long iter) {
            var a = new BigInteger(1);
            var b = new BigInteger(0);

            foreach (var line in input.Split('\n')) {
                if (line.Contains("into new stack")) {
                    a *= -1;
                    b = m - b - 1;
                } else if (line.Contains("cut")) {
                    var n = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    b = b - n + m;
                } else if (line.Contains("increment")) {
                    var n = long.Parse(Regex.Match(line, @"-?\d+").Value);
                    a *= n;
                    b *= n;
                } else {
                    throw new Exception();
                }
            }

            var resA = BigInteger.One;
            var resB = BigInteger.Zero;

            while (iter > 0) {
                if (iter % 2 == 1) {
                    (resA, resB) = ((resA * a) % m, (resB * a + b) % m);
                }
                (a, b) = ((a * a) % m, (b * a + b) % m);
                iter >>= 1;
            }

            return (resA, resB);
        }
    }
}