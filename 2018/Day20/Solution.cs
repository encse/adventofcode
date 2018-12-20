using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;


namespace AdventOfCode.Y2018.Day20 {
    using Doors = IEnumerable<((int x, int y) posFrom, (int x, int y) posTo)>;
    delegate Doors Cont((int x, int y) Pos);

    class Solution : Solver {

        public string GetName() => "A Regular Map";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Solver(input).dMax;
        int PartTwo(string input) => Solver(input).distantRooms;

        (int dMax, int distantRooms) Solver(string input) {

            var grid = Parse(input);
           
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

        Dictionary<(int x, int y), List<(int x, int y)>> Parse(string input) {
            var ich = 0;

            bool accept(char ch) {
                if (input[ich] == ch) {
                    ich++;
                    return true;
                } else {
                    return false;
                }
            };

            Action<char> except = (char ch) => {
                if (!accept(ch)) throw new Exception();
            };

            bool parseLiteral(out Literal literal) {
                var sb = new StringBuilder();
                while (true) {
                    if (accept('E')) sb.Append('E');
                    else if (accept('S')) sb.Append('S');
                    else if (accept('W')) sb.Append('W');
                    else if (accept('N')) sb.Append('N');
                    else break;
                }

                literal = sb.Length > 0 ? new Literal { st = sb.ToString() } : null;
                return literal != null;
            };

            bool parseBody(out Node seq) {
                var nodes = new List<Node>();
                while (true) {
                    if (parseAlt(out var alt)) {
                        nodes.Add(alt);
                    } else if (parseLiteral(out var lit)) {
                        nodes.Add(lit);
                    } else {
                        break;
                    }
                }
                seq = nodes.Any() ? new Seq { nodes = nodes.ToArray() } : null;
                return seq != null;
            };

            bool parseAlt(out Alt alt) {
                if (accept('(')) {
                    var nodes = new List<Node>();
                    while (parseBody(out var seq)) {
                        nodes.Add(seq);
                        accept('|');
                    }
                    except(')');
                    alt = new Alt { nodes = nodes.ToArray() };
                    return true;
                } else {
                    alt = null;
                    return false;
                }
            };

            except('^');
            parseBody(out var rootNode);
            except('$');

            return rootNode
                .Traverse((0, 0), ((int x, int y) pos) => Enumerable.Empty<((int x, int y) posFrom, (int x, int y) posTo)>())
                .GroupBy(x => x.posFrom)
                .ToDictionary(x => x.Key, x => x.Select(y => y.posTo).ToList());
        }

    }

    abstract class Node {
        public abstract Doors Traverse((int x, int y) pos, Cont cont);
    }

    class Literal : Node {
        public string st;

        static Dictionary<char, (int dx, int dy)> step = new Dictionary<char, (int dx, int dy)>{
            {'N',(0, -1)},
            {'E',(1,0)},
            {'W',(-1, 0)},
            {'S',(0, 1)},
        };

        public override Doors Traverse((int x, int y) pos, Cont cont) {
            foreach (var ch in st) {
                var posNew = (pos.x + step[ch].dx, pos.y + step[ch].dy);
                yield return (pos, posNew);
                yield return (posNew, pos);
                pos = posNew;
            }

            foreach (var next in cont(pos)) {
                yield return next;
            }
        }
    }

    class Seq : Node {
        public Node[] nodes;
        public override Doors Traverse((int x, int y) pos, Cont cont) {
            Func<int, Cont> step = null;
            step = (int i) => {
                if (i == nodes.Length)
                    return cont;

                return (posT) => nodes[i].Traverse(posT, step(i + 1));
            };
            return step(0)(pos);
        }
    }

    class Alt : Node {
        public Node[] nodes;
        public override Doors Traverse((int x, int y) pos, Cont cont) {
            Func<int, Cont> step = null;
            step = (int i) => {
                if (i == nodes.Length)
                    return cont;

                return (posT) => nodes[i].Traverse(pos, step(i + 1));
            };
            return step(0)(pos);
        }
    }
}