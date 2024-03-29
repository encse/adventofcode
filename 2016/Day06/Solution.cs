using System.Linq;

namespace AdventOfCode.Y2016.Day06;

[ProblemName("Signals and Noise")]
class Solution : Solver {

    public object PartOne(string input) => Decode(input).mostFrequent;
    public object PartTwo(string input) => Decode(input).leastFrequent;

    (string mostFrequent, string leastFrequent) Decode(string input) {
        var lines = input.Split('\n');
        string mostFrequent = "";
        string leastFrequent = "";
        for (int i = 0; i < lines[0].Length; i++) {
            var items = (from line in lines group line by line[i] into g orderby g.Count() select g.Key);
            mostFrequent += items.Last();
            leastFrequent += items.First();
        }
        return (mostFrequent: mostFrequent, leastFrequent: leastFrequent);
    }
}
