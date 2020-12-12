using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2018.Day20 {
    [ProblemName("A Regular Map")]
    class Solution : Solver {

        public object PartOne(string input) => Solver(input).dMax;
        public object PartTwo(string input) => Solver(input).distantRooms;

        (int dMax, int distantRooms) Solver(string input) {
            var grid = Doors(input)
                .ToList()
                .GroupBy(x => x.posFrom)
                .ToDictionary(x=>x.Key, x=> x.Select(y => y.posTo).ToList());
                       
            var queue = new Queue<((int x, int y) pos, int d)>();
            queue.Enqueue(((0, 0), 0));

            var seen = new HashSet<(int x, int y)>();
            var (dMax, distantRooms) = (int.MinValue, 0);

            while (queue.Any()) {
                var (pos, d) = queue.Dequeue();
                if (seen.Contains(pos)) {
                    continue;
                }

                dMax = Math.Max(dMax, d);
                if (d >= 1000) {
                    distantRooms++;
                }

                seen.Add(pos);
                foreach (var nextPos in grid[pos]) {
                    queue.Enqueue((nextPos, d + 1));
                }
            }

            return (dMax, distantRooms);
        }

        IEnumerable<((int x, int y) posFrom, (int x, int y) posTo)> Doors(string input) {
            var s = new Stack<(int x, int y)>();
            (int x, int y) pos = (0, 0);
            foreach (var ch in input) {
                var prev = pos;
                switch (ch) {
                    case 'N': pos = (pos.x, pos.y - 1); break;
                    case 'S': pos = (pos.x, pos.y + 1); break;
                    case 'E': pos = (pos.x + 1, pos.y); break;
                    case 'W': pos = (pos.x - 1, pos.y); break;
                    case '(': s.Push(pos); break;
                    case '|': pos = s.Peek(); break;
                    case ')': pos = s.Pop(); break;
                }

                if ("NSEW".IndexOf(ch) >= 0) {
                    yield return (prev, pos);
                    yield return (pos, prev);
                }
            }
        }
    }
}