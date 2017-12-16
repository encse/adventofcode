using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode2017.Day10 {

    class Solution : Solver {

        public string GetName() => "Knot Hash"; 

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var chars = input.Split(',').Select(int.Parse);
            var hash = KnotHash(chars, 1);
            return hash[0] * hash[1];
        }

        string PartTwo(string input) {
            var suffix = new [] { 17, 31, 73, 47, 23 };
            var chars = input.ToCharArray().Select(b => (int)b).Concat(suffix);

            var hash = KnotHash(chars, 64);

            return string.Join("", 
                from blockIdx in Enumerable.Range(0, 16)
                let block = hash.Skip(16 * blockIdx).Take(16)
                select block.Aggregate(0, (acc, ch) => acc ^ ch).ToString("x2"));
        }

        int[] KnotHash(IEnumerable<int> input, int rounds) {
            var output = Enumerable.Range(0, 256).ToArray();

            var current = 0;
            var skip = 0;
            for (var round = 0; round < rounds; round++) {
                foreach (var len in input) {
                    for (int i = 0; i < len / 2; i++) {
                        var from = (current + i) % output.Length;
                        var to = (current + len - 1 - i) % output.Length;
                        (output[from], output[to]) = (output[to], output[from]);
                    }
                    current += len + skip;
                    skip++;
                }
            }
            return output;
        }
    }
}
