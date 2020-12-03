using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace AdventOfCode.Y2015.Day04 {

    [ProblemName("The Ideal Stocking Stuffer")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);

        }

        int PartOne(string input) => ParallelFind(input, "00000");
        int PartTwo(string input) => ParallelFind(input, "000000");

        int ParallelFind(string input, string prefix) {
            var q = new ConcurrentQueue<int>();

            Parallel.ForEach(
                Enumerable.Range(0, int.MaxValue), 
                () => MD5.Create(), 
                (i, state, md5) => {
                    var hashBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(input + i));
                    var hash = string.Join("", hashBytes.Select(b => b.ToString("x2")));

                    if (hash.StartsWith(prefix)) {
                        q.Enqueue(i);
                        state.Stop();
                    }
                    return md5;
                 }, 
                 (_) => {}
            );
            return q.Min();
        }

    }
}