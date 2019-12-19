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
            var icm = new IntCodeMachine(input);
            var output = icm.Run();
            var mx = string.Join("", output.Select(x => (char)x)).Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
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


        int PartTwo(string input) {
            var icm = new IntCodeMachine(input);
            var output = icm.Run();
            var mx = string.Join("", output.Select(x => (char)x)).Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            var path = Path(mx);
            foreach (var (main, programs) in Solve(path, 0, ImmutableList<string>.Empty)) {
                var compiled = programs.Select(program => compile(program)).ToArray();
                if (main.Length <= 20 && compiled.All(c => c.Length <= 20)) {
                    var mainC = main.Replace(" ", ",").Replace("0", "A").Replace("1","B").Replace("2","C").TrimEnd(',');
                    var lines = $"{mainC}\n{compiled[0]}\n{compiled[1]}\n{compiled[2]}\nn\n";
                    
                    icm.Reset();
                    icm.memory[0] = 2;
                    output = icm.Run(lines.Select(x=>(long)x).ToArray());

                    var q = string.Join("", output.Select(x => (char)x));
                    return (int)output.Last();
                }
            }
            throw new Exception();
        }

        IEnumerable<(string, string[])> Solve(string path, int ich, ImmutableList<string> parts) {
            if (ich == path.Length) {
                yield return ("", parts.ToArray());
            }

            for (var ipart = 0; ipart < parts.Count; ipart++) {
                var part = parts[ipart];
                if (ich + part.Length <= path.Length && path.Substring(ich, part.Length) == part) {
                    foreach (var (main, programs) in Solve(path, ich + part.Length, parts)) {
                        yield return (ipart + " " + main, programs);
                    }
                }
            }
            if (parts.Count < 3) {
                for (var l = 1; ich + l <= path.Length; l++) {
                    var newPart = path.Substring(ich, l);
                    var newParts = parts.Add(newPart);
                    foreach (var (main, programs) in Solve(path, ich + l, newParts)) {
                        yield return ((newParts.Count - 1) + " " + main, programs);
                    }
                }
            }
        }

        // IEnumerable<string> main(string path, string a, string b, string c) {
        //     if (path == "") {
        //         yield return "";
        //     }
        //     if (path.StartsWith(a)) {
        //         foreach (var x in main(path.Substring(a.Length), a, b, c)) {
        //             if (x == "") {
        //                 yield return "a";
        //             } else {
        //                 yield return "a," + x;
        //             }
        //         }
        //     }
        //     if (path.StartsWith(b)) {
        //         foreach (var x in main(path.Substring(b.Length), a, b, c)) {
        //             if (x == "") {
        //                 yield return "b";
        //             } else {
        //                 yield return "b," + x;
        //             }
        //         }
        //     }
        //     if (path.StartsWith(c)) {
        //         foreach (var x in main(path.Substring(c.Length), a, b, c)) {
        //             if (x == "") {
        //                 yield return "c";
        //             } else {
        //                 yield return "c," + x;
        //             }
        //         }
        //     }
        // }

        // string[] Program2(string[] mx) {
        //     var path = Path(mx);
        //     var s = new Dictionary<string, int>();
        //     for (var ich = 0; ich < path.Length; ich++) {
        //         for (var cch = 1; ich + cch <= path.Length; cch++) {
        //             var key = path.Substring(ich, cch);
        //             if (!s.ContainsKey(key)) {
        //                 s[key] = 1;
        //             } else {
        //                 s[key]++;
        //             }
        //         }
        //     }
        //     var compiled = new Dictionary<string, string>();
        //     foreach (var substring in s.Keys) {
        //         if (!compiled.ContainsKey(substring)) {
        //             compiled[substring] = compile(substring);
        //         }
        //     }



        //     var substrings = s.Keys
        //         .Where(st => compiled[st].Length <= 20)
        //         .OrderBy(substring => -(substring.Length - compiled[substring].Length) * s[substring])
        //         .ToList();

        //     foreach (var stA in substrings) {
        //         foreach (var stB in substrings) {
        //             foreach (var stC in substrings) {
        //                 if (stA != stB && stA != stC && stB != stC) {
        //                     foreach (var m in main(path, stA, stB, stC)) {
        //                         Console.WriteLine(m.Length);
        //                         // Console.WriteLine(m);
        //                         // Console.WriteLine(stA);
        //                         // Console.WriteLine(stB);
        //                         // Console.WriteLine(stC);
        //                     }
        //                 }
        //             }
        //         }
        //     }
        //     return new string[] { };
        // }

        string compile(string st) {
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

        // string[] Program(string[] mx) {
        //     var path = Path(mx);

        //     string decompile(string st) {
        //         if (st == "") {
        //             return "";
        //         }
        //         var res = "";
        //         foreach (var step in st.Split(",")) {
        //             if (step == "L" || step == "R") {
        //                 res += step;
        //             } else {
        //                 res += new string('F', int.Parse(step));
        //             }

        //         }
        //         return res;
        //     }
        //     var p = 0;

        //     Console.WriteLine(compile(path));
        //     var ichA = 0;
        //     for (var cchA = 1; ichA + cchA <= path.Length; cchA++) {
        //         var stA = path.Substring(ichA, cchA);
        //         var cA = compile(stA);
        //         if (cA.Length > 20) {
        //             continue;
        //         }

        //         var ichB = ichA + cchA;
        //         for (var cchB = 1; ichB + cchB <= path.Length; cchB++) {
        //             var stB = path.Substring(ichB, cchB);
        //             var cB = compile(stB);
        //             if (cB.Length > 20) {
        //                 continue;
        //             }

        //             for (var ichC = ichB + cchB; ichC < path.Length; ichC++) {
        //                 for (var cchC = 1; ichC + cchC <= path.Length; cchC++) {
        //                     var stC = path.Substring(ichC, cchC);

        //                     var cC = compile(stC);

        //                     if (cC.Length > 20) {
        //                         continue;
        //                     }
        //                     var sanity = stA + stB + stC;

        //                     // if(!sanity.Contains("F")|| !sanity.Contains("L")|| !sanity.Contains("R")){
        //                     //     continue;
        //                     // }
        //                     foreach (var m in main(path, stA, stB, stC)) {
        //                         // if (m.Length <= 20) {
        //                         //Console.WriteLine(path);
        //                         Console.WriteLine(m.Length);
        //                         // Console.WriteLine(m);
        //                         // Console.WriteLine(cA);
        //                         // Console.WriteLine(cB);
        //                         // Console.WriteLine(cC);
        //                         p++;
        //                         // }
        //                     }
        //                     // Console.Write(".");
        //                 }
        //             }
        //         }
        //     }

        //     Console.WriteLine(p);
        //     return new string[] { };
        // }
        string Path(string[] mx) {
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