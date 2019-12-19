using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System;

namespace AdventOfCode.Y2019.Day17 {

    class Solution : Solver {

        enum Dir {
            Up = '^',
            Down = 'v',
            Left = '<',
            Right = '>',
        }
        public string GetName() => "Set and Forget";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var mx = Screenshot(input);
           
            var crow = mx.Length;
            var ccol = mx[0].Length;
            var res = 0;
            var pattern = ".#.\n###\n.#.".Split("\n");

            var x = 0;
            for (var irow = 1; irow < crow - 1; irow++) {
                for (var icol = 1; icol < ccol - 1; icol++) {
                    var ok = true;
                    foreach (var drow in new[] { -1, 0, 1 }) {
                        foreach (var dcol in new[] { -1, 0, 1 }) {
                            if (pattern[1 + drow][1 + dcol] != mx[irow + drow][icol + dcol]) {
                                ok = false;
                            }
                        }
                    }

                    if (ok) {
                        x++;
                        res += icol * irow;
                    }
                }
            }
            return res;
        }

        
        int PartTwo(string prg) {
            var path = Path(prg);

            foreach (var res in Solve(path, ImmutableList<string>.Empty)) {

                var compressed = res.functions.Select(program => Compress(program)).ToArray();

                if (res.main.Count <= 20 && compressed.All(c => c.Length <= 20)) {
                    var mainC = string.Join(",", res.main);
                    var lines = $"{mainC}\n{compressed[0]}\n{compressed[1]}\n{compressed[2]}\nn\n";
                    
                    var icm = new IntCodeMachine(prg);
                    icm.memory[0] = 2;
                    var output = icm.Run(lines.Select(x=>(long)x).ToArray());

                    var q = string.Join("", output.Select(x => (char)x));
                    return (int)output.Last();
                }
            }
            throw new Exception();
        }

        string[] Screenshot(string input){
            var icm = new IntCodeMachine(input);
            var output = icm.Run();
            return string.Join("", output.Select(x => (char)x)).Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        }
        
        IEnumerable<(ImmutableList<char> main, string[] functions)> Solve(string path, ImmutableList<string> functions) {
            if (path.Length == 0) {
                yield return (ImmutableList<char>.Empty, functions.ToArray());
            }

            var functionNames = "ABC";

            for (var ifunction = 0; ifunction < functions.Count; ifunction++) {
                var function = functions[ifunction];
                var name = functionNames[ifunction];
                if (path.StartsWith(function)) {
                    foreach (var res in Solve(path.Substring(function.Length), functions)) {
                        yield return (res.main.Insert(0, name), res.functions);
                    }
                }
            }

            if (functions.Count < 3) {
                for (var length = 1; length <= path.Length; length++) {
                    var function = path[0..length].ToString();
                    var name = functionNames[functions.Count];
                    foreach (var (main, programs) in Solve(path.Substring(function.Length), functions.Add(function))) {
                        yield return (main.Insert(0, name), programs);
                    }
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

            var (pos, dir) = RobotPosition(mx);
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

        ((int irow, int icol) pos, (int drow, int dcol) dir) RobotPosition(string[] mx) {
            var crow = mx.Length;
            var ccol = mx[0].Length;

            var forward = new Dictionary<Dir, (int drow, int dcol)> {
                {Dir.Up, (-1, 0)},
                {Dir.Down, (1, 0)},
                {Dir.Left, (0, -1)},
                {Dir.Right, (0, 1)},
            };

            for (var irow = 0; irow < crow; irow++) {
                for (var icol = 0; icol < ccol; icol++) {
                    var dir = (Dir)mx[irow][icol];
                    if (Enum.IsDefined(typeof(Dir), dir)) {
                        return ((irow, icol), forward[dir]);
                    }
                }
            }

            throw new Exception();

        }
    }
}