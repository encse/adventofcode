using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day20 {

    class Solution : Solver {

        public string GetName() => "A Regular Map";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var grid = Positions(input).ToList().GroupBy(x => x.fromPos).ToDictionary(x => x.Key, x => x.Select(y => y.toPos).ToList());

            var q = new Queue<((int x, int y) pos, int d)>();
            q.Enqueue(((0, 0), 0));
            var seen = new HashSet<(int x, int y)>();
            var dMax = int.MinValue;
            while (q.Any()) {
                var (pos, d) = q.Dequeue();
                if (seen.Contains(pos)) {
                    continue;
                }
                dMax = Math.Max(dMax, d);

                seen.Add(pos);
                foreach (var nextPos in grid[pos]) {
                    q.Enqueue((nextPos, d + 1));
                }
            }

            return dMax;
        }

        IEnumerable<((int x, int y) fromPos, (int x, int y) toPos)> Positions(string input){
            var ich = 0;
            var p = ParseSeq(input, ref ich);
            return p.Traverse((0,0), ((int x, int y) pos) => { return Enumerable.Empty<((int x, int y) posFrom, (int x, int y) posTo)>();});
        }

        int PartTwo(string input) {
            var grid = Positions(input).ToList().GroupBy(x => x.fromPos).ToDictionary(x => x.Key, x => x.Select(y => y.toPos).ToList());


            var q = new Queue<((int x, int y) pos, int d)>();
            q.Enqueue(((0, 0), 0));
            var seen = new HashSet<(int x, int y)>();
            var res = 0;
            while (q.Any()) {
                var (pos, d) = q.Dequeue();
                if (seen.Contains(pos)) {
                    continue;
                }
                if(d >= 1000){
                    res ++;
                }

                seen.Add(pos);
                foreach (var nextPos in grid[pos]) {
                    q.Enqueue((nextPos, d + 1));
                }
            }

            return res;
        }


        Node ParseSeq(string input, ref int ich) {
            var nodes = new List<Node>();
            while (ich < input.Length) {
                var ch = input[ich];
                switch (ch) {
                    case '^':
                        ich++;
                        break;
                    case '(':
                        nodes.Add(ParseAlt(input, ref ich));
                        break;
                    case '|':
                    case ')':
                    case '$':
                        return new Seq { nodes = nodes.ToArray() };
                    default:
                        nodes.Add(ParseLiteral(input, ref ich));
                        break;
                }
            }
            throw new Exception();
        }

        Node ParseAlt(string input, ref int ich) {
            ich++;
            var nodes = new List<Node>();
            while (ich < input.Length) {
                nodes.Add(ParseSeq(input, ref ich));
                if(input[ich] == ')'){
                    ich++;
                    break;
                } else {
                    ich++;
                }
            }
            return new Alt{nodes=nodes.ToArray()};
        }

        Node ParseLiteral(string input, ref int ich) {
            var sb = new StringBuilder();
            while (ich < input.Length) {
                var ch = input[ich];
                switch (ch) {
                    case 'E':
                    case 'S':
                    case 'W':
                    case 'N':
                        sb.Append(ch);
                        ich++;
                        break;
                    default:
                        return new Literal { st = sb.ToString() };
                }
            }
            throw new Exception();
        }

    }

    abstract class Node {
        public abstract IEnumerable<((int x, int y) posFrom, (int x, int y) posTo)> Traverse((int x, int y) pos,
            Func<(int x, int y), IEnumerable<((int x, int y) posFrom, (int x, int y) posTo)>> cont);
    }

    class Literal : Node {
        public string st;

        static Dictionary<char, (int dx, int dy)> step = new Dictionary<char, (int dx, int dy)>{
            {'N',(0, -1)},
            {'E',(1,0)},
            {'W',(-1, 0)},
            {'S',(0, 1)},
        };

        public override IEnumerable<((int x, int y) posFrom, (int x, int y) posTo)> Traverse((int x, int y) pos,
            Func<(int x, int y), IEnumerable<((int x, int y) posFrom, (int x, int y) posTo)>> cont
        ) {
            foreach (var ch in st) {
                switch (ch) {
                    case 'E':
                    case 'N':
                    case 'W':
                    case 'S':
                        var posNew = (pos.x + step[ch].dx, pos.y + step[ch].dy);
                        yield return (pos, posNew);
                        yield return (posNew, pos);
                        pos = posNew;
                        break;
                }
            }

            foreach (var next in cont(pos))
                yield return next;
        }
    }

    class Seq : Node {
        public Node[] nodes;
        public override IEnumerable<((int x, int y) posFrom, (int x, int y) posTo)> Traverse(
            (int x, int y) pos,
            Func<(int x, int y), IEnumerable<((int x, int y) posFrom, (int x, int y) posTo)>> cont
        ) {
            Func<int, Func<(int x, int y), IEnumerable<((int x, int y) posFrom, (int x, int y) posTo)>>> step = null;
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
        public override IEnumerable<((int x, int y) posFrom, (int x, int y) posTo)> Traverse(
            (int x, int y) pos,
            Func<(int x, int y), IEnumerable<((int x, int y) posFrom, (int x, int y) posTo)>> cont
        ) {
            Func<int, Func<(int x, int y), IEnumerable<((int x, int y) posFrom, (int x, int y) posTo)>>> step = null;
            step = (int i) => {
                if (i == nodes.Length)
                    return cont;

                return (posT) => nodes[i].Traverse(pos, step(i + 1));
            };
            return step(0)(pos);
        }
    }
}