using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

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
            var nics = (
               from i in Enumerable.Range(0, 50)
               select (machine: new IntCodeMachine(input), queue: new Queue<long>(new long[] { i }))
           ).ToArray();


            var (x, y, yLast) = (0L, 0L, 0L);
            var idle = false;
            while (true) {

                idle = true;

                for (var i = 0; i < 50; i++) {
                    var (machine, queue) = nics[i];
                    var data = queue.Any() ? queue.ToArray() : new[] { -1L };
                    if (queue.Any()) {
                        idle = false;
                    }
                    queue.Clear();
                    var output = machine.Run(data);
                    for (var d = 0; d < output.Length; d += 3) {
                        var packet = output.Skip(d).Take(3).ToArray();
                        idle = false;
                        var recipient = packet[0];
                        if (recipient == 255) {

                            (x, y) = (packet[1], packet[2]);
                            if (part1) {
                                return y;
                            }

                        } else {
                            var recipientQ = nics[recipient];
                            recipientQ.queue.Enqueue(packet[1]);
                            recipientQ.queue.Enqueue(packet[2]);
                        }
                    }
                }

                if (idle) {

                    idle = false;
                    nics[0].queue.Enqueue(x);
                    nics[0].queue.Enqueue(y);
                    if (y == yLast) {
                        return y;
                    }
                    yLast = y;
                }
            }
        }

    }
}