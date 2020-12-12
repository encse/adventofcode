using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Y2016.Day14 {

    [ProblemName("One-Time Pad")]
    class Solution : Solver {

        public object PartOne(string input) => Solve(Hashes(input, 0));
        public object PartTwo(string input) => Solve(Hashes(input, 2016));

        int Solve(IEnumerable<string> hashes) {
            var found = 0;
            var nextIdx = Enumerable.Range(0, 16).Select(_ => new Queue<int>()).ToArray();
            var res = 0;
            var hashQueue = new Queue<string>();
            var idx = 0;
            var idxEnd = 0;
            foreach (var hashEnd in hashes) {

                hashQueue.Enqueue(hashEnd);

                for (int i = 0; i < hashEnd.Length - 5; i++) {
                    if (hashEnd[i] == hashEnd[i + 1] &&
                        hashEnd[i + 1] == hashEnd[i + 2] &&
                        hashEnd[i + 2] == hashEnd[i + 3] &&
                        hashEnd[i + 3] == hashEnd[i + 4]
                    ) {
                        var c = hashEnd[i] <= '9' ? hashEnd[i] - '0' : hashEnd[i] - 'a' + 10;
                        nextIdx[c].Enqueue(idxEnd);
                    }
                }
                idxEnd++;

                if (hashQueue.Count() == 1001) {
                    var hash = hashQueue.Dequeue();
                    for (int i = 0; i < hash.Length - 2; i++) {
                        if (hash[i] == hash[i + 1] && hash[i + 2] == hash[i + 1]) {
                            var iq = hash[i] <= '9' ? hash[i] - '0' : hash[i] - 'a' + 10;
                            var q = nextIdx[iq];
                            while (q.Any() && q.First() <= idx) {
                                q.Dequeue();
                            }
                            if (q.Any() && q.First() - idx <= 1000) {
                                found++;
                                res = idx;
                                if (found == 64) {
                                    return res;
                                }
                            }
                            break;
                        }
                    }
                    idx++;
                }
            }

            throw new Exception();
        }

        public IEnumerable<string> Hashes(string input, int rehash) {

            for (var i = 0; i < int.MaxValue; i++) {
                var q = new ConcurrentQueue<(int i, string hash)>();

                Parallel.ForEach(
                    Enumerable.Range(i, 1000),
                    () => MD5.Create(),
                    (i, state, md5) => {
                        var newInput = new byte[32];
                        var btoh = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

                        var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(input + i));

                        for (var r = 0; r < rehash; r++) {
                            for (int ib = 0; ib < 16; ib++) {
                                newInput[2 * ib] = (byte)btoh[(hash[ib] >> 4) & 15];
                                newInput[2 * ib + 1] = (byte)btoh[hash[ib] & 15];
                            }
                            hash = md5.ComputeHash(newInput);
                        }

                        q.Enqueue((i, string.Join("", hash.Select(b => b.ToString("x2")))));
                        return md5;
                    },
                    (_) => { }
                );
                foreach (var item in q.OrderBy(x => x.i)) {
                    i = item.i;
                    yield return item.hash;
                }
            }
        }
    }
}