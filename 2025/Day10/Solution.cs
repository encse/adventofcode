namespace AdventOfCode.Y2025.Day10;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;
using AdventOfCode.Model;
using System.Security.Cryptography;
using AngleSharp.Html.Dom.Events;

record Problem(int target, int[] buttons, int[] jolts);

record Equation(int[] buttonIndices, int sum);

[ProblemName("Factory")]
class Solution : Solver {

    public object PartOne(string input) {
        var res = 0;
        foreach (var p in Parse(input)) {
            var limit = 1 << p.buttons.Length;
            var tries = Enumerable.Range(0, limit).OrderBy(BitCount).ToArray();

            var q = -1;
            foreach (var n in tries) {
                if ((Xor(p.buttons, n) ^ p.target) == 0) {
                    q = n;
                    break;
                }
            }
            if (q == -1) {
                throw new Exception();
            }
            res += BitCount(q);
        }
        return res;
    }


    public object PartTwo(string input) {
        var res = 0L;
        foreach (var p in Parse(input)) {
            var s = Solve(p);
            Console.WriteLine(s);
            res += s;
        }
        return 0;
    }

    int Xor(int[] buttons, int mask) {
        var res = 0;
        var i = 0;
        while (mask != 0) {
            if ((mask & 1) != 0) {
                res ^= buttons[i];
            }
            mask >>= 1;
            i++;
        }
        return res;
    }

    Dictionary<string, int> cache;

    int Solve(Problem p) {
        var equations = new List<Equation>();
        for(int i = 0; i < p.jolts.Length; i++) {
            var jolt = p.jolts[i];
            var buttonIndices = new List<int>();
            for(var buttonIndex = 0; buttonIndex < p.buttons.Length; buttonIndex++) {
                var button = p.buttons[buttonIndex];
                if ((button & 1 << i) != 0) {
                    buttonIndices.Add(buttonIndex);
                }
            }
            equations.Add(new Equation(buttonIndices.ToArray(), jolt));
        }
        Console.WriteLine(p.target + " " + equations.Count);

        return SolveEquations(equations);
    }

    int SolveEquations(List<Equation> equations) {
        // [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}

        if (equations.Count == 0) {
            return 0;
        }

        var res = int.MaxValue / 2;
        var eq = equations.MinBy(eq => eq.buttonIndices.Length);
        var q = Choose(eq.sum, eq.buttonIndices, new int[20]).ToArray();

        foreach (var xs in q) {

            var substitutedEquations =
                equations.Select(eqT => Substitute(eqT, eq.buttonIndices, xs)).ToArray();

            if (substitutedEquations.Any(eq => eq.sum < 0) || 
                substitutedEquations.Any(eq => eq.sum > 0 && !eq.buttonIndices.Any())
            ) {
                continue;
            }

            var remainingEquations = substitutedEquations.Where(eq =>
                eq.sum != 0 || eq.buttonIndices.Any()
            ).ToArray();

            var cur = xs.Sum() + SolveEquations(remainingEquations.ToList());
            if (cur < res) {
                res = cur;
            }
        }
        return res;
    }

    Equation Substitute(Equation eq, int[] indices, int[] values) {
        var sum = eq.sum;
        var remainingIndices = eq.buttonIndices.ToList();
        for (int i = 0; i < indices.Length; i++) {
            var index = indices[i];
            var value = values[index];
            if (remainingIndices.Contains(index)) {
                remainingIndices.Remove(index);
                sum -= value;
            }
        }
        return new Equation(remainingIndices.ToArray(), sum);
    }


    bool TooMuch(Problem p, int[] state) {
        return Enumerable.Range(0, p.jolts.Length).Any(i => state[i] > p.jolts[i]);
    }

    int[] Push(int[] state, int button, int n) {
        var res = state.ToArray();
        for (int i = 0; i < state.Length; i++) {
            if ((button & (1 << i)) != 0) {
                res[i] += n;
            }
        }
        return res;
    }

    int BitCount(int n) {
        int res = 0;
        while (n != 0) {
            n &= n - 1;
            res++;
        }
        return res;
    }

    // [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
    IEnumerable<Problem> Parse(string input) {
        var lines = input.Split("\n");
        foreach (var line in lines) {
            var parts = line.Split(" ").ToArray();
            var num = Convert.ToInt32(
                string.Join("",
                    parts.First()
                        .Replace("[", "")
                        .Replace("]", "")
                        .Replace('.', '0')
                        .Replace('#', '1')
                        .Reverse()
                ),
                2);

            var buttons =
                from part in parts[1..^1]
                let digits = Regex.Matches(part, @"\d").Select(m => int.Parse(m.Value))
                let mask = (from d in digits select 1 << d).Sum()
                select mask;

            var jolts =
                    parts.Last()
                        .Replace("{", "")
                        .Replace("}", "")
                        .Split(",")
                        .Select(int.Parse);
            ;
            yield return new Problem(num, buttons.ToArray(), jolts.ToArray());
        }
    }

    IEnumerable<int[]> Choose(int s, int[] indices, int[] acc) {
        if (indices.Length == 1) {
            acc = acc.ToArray();
            acc[indices[0]] = s;
            yield return acc;
            yield break;
        }
        for (int i = 0; i <= s; i++) {
            foreach (var v in Choose(s - i, indices[1..].ToArray(), acc)) {
                var vT = v.ToArray();
                vT[indices[0]] = i;
                yield return vT;
            }
        }
    }

}