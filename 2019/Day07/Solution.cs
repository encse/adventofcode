using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2019.Day07;

[ProblemName("Amplification Circuit")]
class Solution : Solver {

    public object PartOne(string prg) => Solve(prg, false, new[] { 0, 1, 2, 3, 4 });
    public object PartTwo(string prg) => Solve(prg, true, new[] { 5, 6, 7, 8, 9 });

    long Solve(string prg, bool loop, int[] prgids) {
        var amps = Enumerable.Range(0, 5).Select(x => new IntCodeMachine(prg)).ToArray();
        var max = 0L;

        foreach (var perm in Permutations(prgids)) {
            max = Math.Max(max, ExecAmps(amps, perm, loop));
        }
        return max;
    }

    long ExecAmps(IntCodeMachine[] amps, int[] prgid, bool loop) {

        for (var i = 0; i < amps.Length; i++) {
            amps[i].Reset();
            amps[i].input.Enqueue(prgid[i]);
        }

        var data = new[] { 0L };

        while (true) {
            for (var i = 0; i < amps.Length; i++) {
                data = amps[i].Run(data).ToArray();
            }
            if (amps.All(amp => amp.Halted())) {
                return data.Last();
            }
            if (!loop) {
                data = new long[0];
            }
        }
    }

    IEnumerable<T[]> Permutations<T>(T[] rgt) {
       
        IEnumerable<T[]> PermutationsRec(int i) {
            if (i == rgt.Length) {
                yield return rgt.ToArray();
            }

            for (var j = i; j < rgt.Length; j++) {
                (rgt[i], rgt[j]) = (rgt[j], rgt[i]);
                foreach (var perm in PermutationsRec(i + 1)) {
                    yield return perm;
                }
                (rgt[i], rgt[j]) = (rgt[j], rgt[i]);
            }
        }

        return PermutationsRec(0);
    }
}
