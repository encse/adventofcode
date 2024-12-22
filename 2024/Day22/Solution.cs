namespace AdventOfCode.Y2024.Day22;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Monkey Market")]
class Solution : Solver {

    public object PartOne(string input) {
       return GetNums(input).Select(x => (long)Step(x).Last()).Sum();
    }

    public object PartTwo(string input) {
        var mainDict = new Dictionary<(int a, int b, int c, int d), int>();
        foreach(var num in GetNums(input)) {
            var banana = GetBanana(num);
            foreach(var k in banana.Keys){
                mainDict[k] = mainDict.GetValueOrDefault(k) + banana[k];
            }
        }
        return mainDict.Values.Max();
    }

    Dictionary<(int a, int b, int c, int d), int> GetBanana(int num) {
        var res = new Dictionary<(int a, int b, int c, int d), int>();

        var prices = Step(num).Select(n => n % 10).ToArray();
        for(var i = 5;i < prices.Length; i++) {
            var slice = prices[(i-5) .. i];
            var diff = Diff(slice);
            var key = (diff[0], diff[1], diff[2], diff[3]);
            if (!res.ContainsKey(key)) {
                res[key] = slice.Last();
            }
        }
        return res;
        // var diff = Diff(steps).ToArray();

        // var res = 0;
        // for(var i = seq.Length-1; i<diff.Length;i++) {
        //     if (diff[(i-(seq.Length-1))..i].SequenceEqual(seq)) {
        //         res += steps[i];
        //     }
        // }
        return res;
    }
    IEnumerable<int[]> Quads(int[] nums) {
        for(var i = 4;i<=nums.Length;i++) {
            yield return nums[(i-4) .. i];
        }
        
    }


    int[] Diff(IEnumerable<int> x) {
        return x.Zip(x.Skip(1)).Select(p => p.Second - p.First).ToArray();
    }
    IEnumerable<int> Step(int a) {
        yield return a;
        for(var i = 0;i< 2000;i++) {
            var b = (long)a;
            b = Mix(b, b * 64);
            b = Prune(b);
            b = Mix(b, b/32);
            b = Prune(b);
            b = Mix(b, b*2048);
            b = Prune(b);
            a = (int)b;
            yield return a;
        }
    }
    long Mix(long a, long b) => a ^ b;
    long Prune(long a) => a % 16777216;

    IEnumerable<int> GetNums(string input) =>
        input.Split("\n").Select(int.Parse);
}