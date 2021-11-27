using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Day15;

[ProblemName("Dueling Generators")]
class Solution : Solver {

    public object PartOne(string input) =>
        MatchCount(Combine(ParseGenerators(input)).Take(40000000));

    public object PartTwo(string input) {
        var generators = ParseGenerators(input);
        return MatchCount(Combine((generators.a.Where(a => (a & 3) == 0), generators.b.Where(a => (a & 7) == 0))).Take(5000000));
    }

    IEnumerable<(long, long)> Combine((IEnumerable<long> a, IEnumerable<long> b) items) =>
        Enumerable.Zip(items.a, items.b, (a, b) => (a, b));

    int MatchCount(IEnumerable<(long a, long b)> items) =>
        items.Count(item => (item.a & 0xffff) == (item.b & 0xffff));

    (IEnumerable<long> a, IEnumerable<long> b) ParseGenerators(string input) {
        var lines = input.Split('\n');
        var startA = int.Parse(lines[0].Substring("Generator A starts with ".Length));
        var startB = int.Parse(lines[1].Substring("Generator B starts with ".Length));

        return (Generator(startA, 16807), Generator(startB, 48271));
    }

    IEnumerable<long> Generator(int start, int mul) {
        var mod = 2147483647;

        long state = start;
        while (true) {
            state = (state * mul) % mod;
            yield return state;
        }
    }
}
