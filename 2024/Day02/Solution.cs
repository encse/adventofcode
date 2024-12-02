namespace AdventOfCode.Y2024.Day02;

using System;
using System.Collections.Generic;
using System.Linq;

[ProblemName("Red-Nosed Reports")]
class Solution : Solver {

    public object PartOne(string input) => 
        ParseSamples(input).Count(Valid);

    public object PartTwo(string input) => 
        ParseSamples(input).Count(samples => Attenuate(samples).Any(Valid));

    IEnumerable<int[]> ParseSamples(string input) => 
        from line in input.Split("\n")
        let samples = line.Split(" ").Select(int.Parse)
        select samples.ToArray();

    // Generates all possible variations of the input sequence by omitting 
    // either zero or one element from it.
    IEnumerable<int[]> Attenuate(int[] samples) =>
        from i in Enumerable.Range(0, samples.Length+1)
        let before = samples.Take(i - 1)
        let after = samples.Skip(i)
        select Enumerable.Concat(before, after).ToArray();

    // Checks the monothinicity condition by examining consecutive elements
    bool Valid(int[] samples) {
        var pairs = Enumerable.Zip(samples, samples.Skip(1));
        return
            pairs.All(p => 1 <= p.Second - p.First && p.Second - p.First <= 3) ||
            pairs.All(p => 1 <= p.First - p.Second && p.First - p.Second <= 3);
    }
}
