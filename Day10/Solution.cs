using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2017.Day10 {

    class Solution : Solver {

        public void Solve(string input) {
            Console.WriteLine(PartOne(input));
            Console.WriteLine(PartTwo(input));
        }

        int PartOne(string input) {
            var chars = input.Split(',')
                .Select(int.Parse)
                .Select(x => (char)x);

            var hash = Hash(chars, 1);
            return hash[0] * hash[1];
        }

        string PartTwo(string input) {
            var suffix = new[] { 17, 31, 73, 47, 23 }.Select(x => (char)x);
            var chars = input.ToArray().Concat(suffix);

            var hash = Hash(chars, 64);

            var res = "";
            for (var blockIdx = 0; blockIdx < 16; blockIdx++) {
                var block = hash.Skip(16 * blockIdx).Take(16);
                var w = block.Aggregate(0, (acc, ch) => acc ^ ch);
                res += w.ToString("x2");
            }
            return res;
        }

        char[] Hash(IEnumerable<char> input, int rounds) {
            var output = Enumerable.Range(0, 256).Select(x => (char)x).ToArray();

            var current = 0;
            var skip = 0;
            for (var round = 0; round < rounds; round++) {
                foreach (var len in input) {
                    for (int i = 0; i < len / 2; i++) {
                        var from = (current + i) % output.Length;
                        var to = (current + len - 1 - i) % output.Length;
                        var t = output[from];
                        output[from] = output[to];
                        output[to] = t;

                    }
                    current += len + skip;
                    skip++;
                }
            }
            return output;
        }
    }
}
