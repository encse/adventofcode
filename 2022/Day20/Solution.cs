using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Y2022.Day20;

[ProblemName("Grove Positioning System")]
class Solution : Solver {

    record Data(int idx, long num);

    public object PartOne(string input) =>
         GetGrooveCoordinates(Mix(Parse(input, 1)));

    public object PartTwo(string input) {
        var data = Parse(input, 811589153L);
        for (var i = 0; i < 10; i++) {
            data = Mix(data);
        }
        return GetGrooveCoordinates(data);
    }

    List<Data> Parse(string input, long m) =>
        input
            .Split("\n")
            .Select((line, idx) => new Data(idx, long.Parse(line) * m))
            .ToList();

    List<Data> Mix(List<Data> numsWithIdx) {
        var mod = numsWithIdx.Count - 1;
        for (var idx = 0; idx < numsWithIdx.Count; idx++) {
            var srcIdx = numsWithIdx.FindIndex(x => x.idx == idx);
            var num = numsWithIdx[srcIdx];

            var dstIdx = (srcIdx + num.num) % mod;
            if (dstIdx < 0) {
                dstIdx += mod;
            }

            numsWithIdx.RemoveAt(srcIdx);
            numsWithIdx.Insert((int)dstIdx, num);
        }
        return numsWithIdx;
    }

    long GetGrooveCoordinates(List<Data> numsWithIdx) {
        var idx = numsWithIdx.FindIndex(x => x.num == 0);
        return (
            numsWithIdx[(idx + 1000) % numsWithIdx.Count].num +
            numsWithIdx[(idx + 2000) % numsWithIdx.Count].num +
            numsWithIdx[(idx + 3000) % numsWithIdx.Count].num
        );
    }
}
