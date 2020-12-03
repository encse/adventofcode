using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2018.Day16 {

    [ProblemName("Chronal Classification")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var res = 0;
            var (testCases, prg) = Parse(input);
            foreach (var testCase in testCases) {
                var match = 0;
                for (var i = 0; i < 16; i++) {
                    testCase.stm[0] = i;
                    var regsActual = Step(testCase.regsBefore, testCase.stm);
                    if (Enumerable.Range(0, 4).All(ireg => regsActual[ireg] == testCase.regsAfter[ireg])) {
                        match++;
                    }
                }
                if (match >= 3) {
                    res++;
                }
            }
            return res;
        }

        int PartTwo(string input) {

            var constraints = Enumerable.Range(0, 16).ToDictionary(i => i, i => Enumerable.Range(0, 16).ToList());
            var (testCases, prg) = Parse(input);
            foreach (var testCase in testCases) {
                var op = testCase.stm[0];
                var oldMapping = constraints[op];
                var newMapping = new List<int>();
                foreach (var i in oldMapping) {
                    testCase.stm[0] = i;
                    var regsActual = Step(testCase.regsBefore, testCase.stm);
                    if (Enumerable.Range(0, 4).All(ireg => regsActual[ireg] == testCase.regsAfter[ireg])) {
                        newMapping.Add(i);
                    }
                }
                constraints[op] = newMapping;
            }

            var mapping = WorkOutMapping(constraints, new bool[16], new Dictionary<int, int>());
            var regs = new int[4];
            foreach (var stm in prg) {
                stm[0] = mapping[stm[0]];
                regs = Step(regs, stm);
            }
            return regs[0];
        }

        Dictionary<int, int> WorkOutMapping(Dictionary<int, List<int>> constaints, bool[] used, Dictionary<int, int> res) {
            var op = res.Count;
            if (op == 16) {
                return res;
            }
            foreach (var i in constaints[op]) {
                if (!used[i]) {
                    used[i] = true;
                    res[op] = i;
                    var x = WorkOutMapping(constaints, used, res);
                    if (x != null) {
                        return x;
                    }
                    res.Remove(op);
                    used[i] = false;
                }
            }
            return null;
        }

        (List<TestCase> testCases, List<int[]> prg) Parse(string input) {
            var lines = input.Split("\n").ToList();
            var iline = 0;

            var testCases = new List<TestCase>();
            while (Ints(@"Before: \[(\d+), (\d+), (\d+), (\d+)\]", out var regsBefore)) {
                Ints(@"(\d+) (\d+) (\d+) (\d+)", out var stm);
                Ints(@"After:  \[(\d+), (\d+), (\d+), (\d+)\]", out var regsAfter);
                iline++;
                testCases.Add(new TestCase() { regsBefore = regsBefore, regsAfter = regsAfter, stm = stm });
            }
            iline++;
            iline++;
            var prg = new List<int[]>();
            while (Ints(@"(\d+) (\d+) (\d+) (\d+)", out var stm)) {
                prg.Add(stm);
            }

            bool Ints(string pattern, out int[] r) {
                r = null;
                if (iline >= lines.Count) {
                    return false;
                }
                var m = Regex.Match(lines[iline], pattern);
                if (m.Success) {
                    iline++;
                    r = m.Groups.Values.Skip(1).Select(x => int.Parse(x.Value)).ToArray();
                }
                return m.Success;
            }
            return (testCases, prg);
        }

        int[] Step(int[] regs, int[] stm) {
            regs = regs.ToArray();
            regs[stm[3]] = stm[0] switch {
                0 => regs[stm[1]] + regs[stm[2]],
                1 => regs[stm[1]] + stm[2],
                2 => regs[stm[1]] * regs[stm[2]],
                3 => regs[stm[1]] * stm[2],
                4 => regs[stm[1]] & regs[stm[2]],
                5 => regs[stm[1]] & stm[2],
                6 => regs[stm[1]] | regs[stm[2]],
                7 => regs[stm[1]] | stm[2],
                8 => regs[stm[1]],
                9 => stm[1],
                10 => stm[1] > regs[stm[2]] ? 1 : 0,
                11 => regs[stm[1]] > stm[2] ? 1 : 0,
                12 => regs[stm[1]] > regs[stm[2]] ? 1 : 0,
                13 => stm[1] == regs[stm[2]] ? 1 : 0,
                14 => regs[stm[1]] == stm[2] ? 1 : 0,
                15 => regs[stm[1]] == regs[stm[2]] ? 1 : 0,
                _ => throw new ArgumentException()
            };
            return regs;
        }
    }

    class TestCase {
        public int[] regsBefore;
        public int[] regsAfter;
        public int[] stm;
    }
}