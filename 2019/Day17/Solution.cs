using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2019.Day17 {

    [ProblemName("Set and Forget")]
    class Solution : Solver {

        public object PartOne(string input) {
            var mx = Screenshot(input);

            var crow = mx.Length;
            var ccol = mx[0].Length;
            var cross = ".#.\n###\n.#.".Split("\n");

            bool crossing(int irow, int icol) => (
                from drow in new[] { -1, 0, 1 }
                from dcol in new[] { -1, 0, 1 }
                select cross[1 + drow][1 + dcol] == mx[irow + drow][icol + dcol]
            ).All(x => x);

            return (
                from irow in Enumerable.Range(1, crow - 2)
                from icol in Enumerable.Range(1, ccol - 2)
                where crossing(irow, icol)
                select icol * irow
            ).Sum();
        }

        public object PartTwo(string input) {
            var program = GeneratePrograms(Path(input)).First();

            var icm = new IntCodeMachine(input);
            icm.memory[0] = 2;
            return icm.Run(program).Last();
        }

        string[] Screenshot(string input) {
            var icm = new IntCodeMachine(input);
            var output = icm.Run();
            return output.ToAscii().Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }

        IEnumerable<string> GeneratePrograms(string path) {

            IEnumerable<(ImmutableList<int> indices, ImmutableList<string> functions)> GenerateRec(string path, ImmutableList<string> functions) {
                if (path.Length == 0) {
                    yield return (ImmutableList<int>.Empty, functions);
                }

                for (var i = 0; i < functions.Count; i++) {
                    var function = functions[i];

                    if (path.StartsWith(function)) {

                        var pathT = path.Substring(function.Length);
                        foreach (var res in GenerateRec(pathT, functions)) {
                            yield return (res.indices.Insert(0, i), res.functions);
                        }
                    }
                }

                if (functions.Count < 3) {
                    for (var length = 1; length <= path.Length; length++) {
                        var function = path[0..length].ToString();
                        var functionsT = functions.Add(function);
                        var idx = functions.Count;
                        var pathT = path.Substring(function.Length);
                        foreach (var res in GenerateRec(pathT, functionsT)) {
                            yield return (res.indices.Insert(0, idx), res.functions);
                        }
                    }
                }
            }

            foreach (var (indices, functions) in GenerateRec(path, ImmutableList<string>.Empty)) {

                var compressed = functions.Select(Compress).ToArray();
                if (indices.Count <= 20 && compressed.All(c => c.Length <= 20)) {

                    var main = string.Join(",", indices.Select(i => "ABC"[i]));
                    yield return $"{main}\n{compressed[0]}\n{compressed[1]}\n{compressed[2]}\nn\n";
                }

            }
        }

        string Compress(string st) {
            var steps = new List<string>();
            var l = 0;
            for (var i = 0; i < st.Length; i++) {
                var ch = st[i];

                if (l > 0 && ch != 'F') {
                    steps.Add(l.ToString());
                    l = 0;
                }
                if (ch == 'R' || ch == 'L') {
                    steps.Add(ch.ToString());
                } else {
                    l++;
                }
            }
            if (l > 0) {
                steps.Add(l.ToString());
            }
            return string.Join(",", steps);
        }

        string Path(string input) {
            var mx = Screenshot(input);
            var crow = mx.Length;
            var ccol = mx[0].Length;

            var (pos, dir) = FindRobot(mx);
            char look((int irow, int icol) pos) {
                var (irow, icol) = pos;
                return irow < 0 || irow >= crow || icol < 0 || icol >= ccol ? '.' : mx[irow][icol];
            }

            var path = "";
            var finished = false;
            while (!finished) {
                finished = true;
                foreach (var (nextDir, step) in new[]{
                    ((drow:  dir.drow, dcol:  dir.dcol), "F"),
                    ((drow: -dir.dcol, dcol:  dir.drow), "LF"),
                    ((drow:  dir.dcol, dcol: -dir.drow), "RF")
                }) {
                    var nextPos = (pos.irow + nextDir.drow, pos.icol + nextDir.dcol);
                    if (look(nextPos) == '#') {
                        path += step;
                        pos = nextPos;
                        dir = nextDir;
                        finished = false;
                        break;
                    }
                }
            }
            return path;
        }

        ((int irow, int icol) pos, (int drow, int dcol) dir) FindRobot(string[] mx) => (
            from irow in Enumerable.Range(0, mx.Length)
            from icol in Enumerable.Range(0, mx[0].Length)
            let ch = mx[irow][icol]
            where "^v<>".Contains(ch)
            let dir = mx[irow][icol] switch
            {
                '^' => (-1, 0),
                'v' => (1, 0),
                '<' => (0, -1),
                '>' => (0, 1),
                _ => throw new Exception()
            }
            select ((irow, icol), dir)
        ).First();
    }
}