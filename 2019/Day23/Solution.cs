using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day23 {


    class Solution : Solver {

        public string GetName() => "Category Six";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => Solve(input, true);

        long PartTwo(string input) => Solve(input, false);

        long Solve(string input, bool part1) {

            var addresses = Enumerable.Range(0, 50).ToList();
            var nics = new IntCodeMachine[50];
            foreach (var address in addresses) {
                nics[address] = new IntCodeMachine(input);
                nics[address].Run(address);
            }

            var queue = new List<(long address, long x, long y)>();

            addresses.Add(255);
            long yLast = 0;
            while (true) {

                foreach (var address in addresses) {
                    var filteredQueue = new List<(long, long, long)>();
                    var data = new List<long>();
                    foreach (var packet in queue) {
                        if (packet.address == address) {
                            data.Add(packet.x);
                            data.Add(packet.y);
                        } else {
                            filteredQueue.Add(packet);
                        }
                    }
                    queue = filteredQueue;
                    if (address == 255) {
                        if (data.Any()) {
                            if (part1) {
                                return data[1];
                            }
                            if (queue.Count == 0) {
                                var (x, y) = (data[^2], data[^1]);
                                if (y != yLast) {
                                    queue.Add((0, x, y));
                                    yLast = y;
                                } else {
                                    return yLast;
                                }
                            }
                        }
                    } else {
                        if (!data.Any()) {
                            data.Add(-1);
                        }
                        var output = nics[address].Run(data.ToArray());
                        for (var d = 0; d < output.Length; d += 3) {
                            queue.Add((output[d], output[d + 1], output[d + 2]));
                        }
                    }
                }
            }
        }

    }
}