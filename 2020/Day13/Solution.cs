using System.Linq;
using System.Numerics;

namespace AdventOfCode.Y2020.Day13 {

    [ProblemName("Shuttle Search")]
    class Solution : Solver {

        public object PartOne(string input) {
            var lines = input.Split("\n");
            var t = int.Parse(lines[0]);
            return lines[1].Split(",")
                .Where(x => x != "x")
                .Select(int.Parse)
                .Aggregate(
                    (wait: int.MaxValue, bus: int.MaxValue),
                    (min, bus) => {
                        var busArrivesIn = (bus - (t % bus)) % bus; 
                        return busArrivesIn < min.wait ? (busArrivesIn, bus) : min;
                    },
                    min => min.wait * min.bus
                );
        }

        public object PartTwo(string input) =>
            ChineseRemainderTheorem(
                input.Split("\n").ElementAt(1).Split(",")
                    .Select((part, i) => (part, i))
                    .Where(x => x.part != "x")
                    .Select(x => {
                        var bus = long.Parse(x.part);
                        return (mod: bus, a: bus - x.i);
                    })
                    .ToArray()
            );

        // https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
        long ChineseRemainderTheorem((long mod, long a)[] items) {
            var prod = items.Aggregate(1L, (acc, item) => acc * item.mod);
            var sm = items.Select((item, i) => {
                var p = prod / item.mod;
                return item.a * ModInv(p, item.mod) * p;
            }).Sum();

            return sm % prod;
        }

        long ModInv(long a, long m) => (long)BigInteger.ModPow(a, m - 2, m);
    }
}