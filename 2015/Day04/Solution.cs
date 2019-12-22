using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace AdventOfCode.Y2015.Day04 {

    class Solution : Solver {

        public string GetName() => "The Ideal Stocking Stuffer";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);

        }

        int PartOne(string input) => ParallelFind(input, "00000");
        int PartTwo(string input) => ParallelFind(input, "000000");

        int ParallelFind(string input, string prefix) {
            var lck = new object();
            int res = int.MaxValue;

            var step = Environment.ProcessorCount;
            var pres = Parallel.For(0, step, (i, state) => {
                foreach (var (hash, idx) in Hashes(input, i, step)) {
                    if (state.ShouldExitCurrentIteration) {
                        lock (lck) {
                            if (idx > res) {
                                return;
                            }
                        }
                    }

                    if (hash.StartsWith(prefix)) {
                        lock (lck) {
                            res = Math.Min(res, idx);
                        }
                        state.Stop();
                        return;
                    }
                }
            });
            return res;
        }
        IEnumerable<(string hash, int idx)> Hashes(string st, int start, int step) {
            var md5 = MD5.Create();
            for (var i = start; ; i += step) {
                var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(st + i));
                yield return (string.Join("", hash.Select(b => b.ToString("x2"))), i);
            }
        }
    }
}