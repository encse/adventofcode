namespace AdventOfCode.Y2024.Day22;

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

[ProblemName("Monkey Market")]
class Solution : Solver {

    public object PartOne(string input) {
        return GetNums(input).Select(x => (long)SecretNumbers(x).Last()).Sum();
    }

    public object PartTwo(string input) {
        // create a dictionary of all buying options then select the one with the most banana:

        var buyingOptions = new Dictionary<(int,int,int,int), int>();

        foreach (var num in GetNums(input)) {
            var optionsBySeller = BuyingOptions(num);
            foreach (var seq in optionsBySeller.Keys) {
                buyingOptions[seq] = buyingOptions.GetValueOrDefault(seq) + optionsBySeller[seq];
            }
        }
        return buyingOptions.Values.Max();
    }

    Dictionary<(int,int,int,int), int> BuyingOptions(int seed) {
        var bananasSold = Bananas(seed).ToArray();
        var buyingOptions = new Dictionary<(int,int,int,int), int>();

        // a sliding window of 5 elements over the sold bananas defines the sequence the monkey 
        // will recognize. add the first occurrence of each sequence to the buyingOptions dictionary 
        // with the corresponding banana count
        var diff = Diff(bananasSold);
        for (var i = 0; i < bananasSold.Length - 4; i++) {
            var seq = (diff[i], diff[i+1], diff[i+2], diff[i+3]);
            if (!buyingOptions.ContainsKey(seq)) {
                buyingOptions[seq] = bananasSold[i+4];
            }
        }
        return buyingOptions;
    }
    int[] Bananas(int seed) => SecretNumbers(seed).Select(n => n % 10).ToArray();

    int[] Diff(IEnumerable<int> x) => x.Zip(x.Skip(1)).Select(p => p.Second - p.First).ToArray();

    IEnumerable<int> SecretNumbers(int seed) {
        var mixAndPrune = (int a, long b) => (int)((a ^ b) % 16777216);

        yield return seed;
        for (var i = 0; i < 2000; i++) {
            seed = mixAndPrune(seed, seed * 64L);
            seed = mixAndPrune(seed, seed / 32L);
            seed = mixAndPrune(seed, seed * 2048L);
            yield return seed;
        }
    }
    IEnumerable<int> GetNums(string input) => input.Split("\n").Select(int.Parse);
}