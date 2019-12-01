using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace AdventOfCode.Y2015.Day04 {

    class Solution : Solver {

        public string GetName() => "The Ideal Stocking Stuffer";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Hashes(input).First(p => p.hash.StartsWith("00000")).idx;

        int PartTwo(string input) => Hashes(input).First(p => p.hash.StartsWith("000000")).idx;

        IEnumerable<(string hash, int idx)> Hashes(string st) {
            var md5 = MD5.Create();
            return from i in Enumerable.Range(0, int.MaxValue)
                let hash = md5.ComputeHash(Encoding.ASCII.GetBytes(st + i))
                select (string.Join("", hash.Select(b => b.ToString("x2"))), i);
        }
    }
}