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

        var buyingOptions = new Dictionary<string, int>();
        foreach (var num in GetNums(input)) {
            var optionsBySeller = BuyingOptions(num);
            foreach(var seq in optionsBySeller.Keys){
                buyingOptions[seq] = buyingOptions.GetValueOrDefault(seq) + optionsBySeller[seq];
            }
        }
        return buyingOptions.Values.Max();
    }

    Dictionary<string, int> BuyingOptions(int seed) {
        var bananasSold = Bananas(seed).ToArray();

        var buyOptions = new Dictionary<string, int>();

        // a sliding window of 5 elements over the sold bananas defines the sequence the monkey 
        // will recognize. add the first occurrence of each sequence to the buyOptions dictionary 
        // with the corresponding banana count
        for (var i = 5;i < bananasSold.Length; i++) {
            var slice = bananasSold[(i-5) .. i];
            var seq = string.Join(",", Diff(slice)); 
            if (!buyOptions.ContainsKey(seq)) {
                buyOptions[seq] = slice.Last();
            }
        }
        return buyOptions;
    }
    int[] Bananas(int seed)  => SecretNumbers(seed).Select(n => n % 10).ToArray();

    int[] Diff(IEnumerable<int> x) => x.Zip(x.Skip(1)).Select(p => p.Second - p.First).ToArray();

    IEnumerable<int> SecretNumbers(int seed) {
        var mixAndPrune = (int a, long b) => (int)((a ^ b) % 16777216);
        
        yield return seed;
        for(var i = 0;i< 2000;i++) {
            seed = mixAndPrune(seed, seed * 64L);
            seed = mixAndPrune(seed, seed / 32L);
            seed = mixAndPrune(seed, seed * 2048L);
            yield return seed;
        }
    }
    IEnumerable<int> GetNums(string input) => input.Split("\n").Select(int.Parse);
}