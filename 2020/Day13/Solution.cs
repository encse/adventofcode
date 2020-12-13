using System.Linq;
using System.Numerics;

namespace AdventOfCode.Y2020.Day13 {

    [ProblemName("Shuttle Search")]
    class Solution : Solver {

        public object PartOne(string input) {
            var problem = Parse(input);
            return problem.buses.Aggregate(
                (wait: long.MaxValue, bus: long.MaxValue),
                (min, bus) => {
                    var wait = bus.period - (problem.earliestDepart % bus.period); 
                    return wait < min.wait ? (wait, bus.period) : min;
                },
                min => min.wait * min.bus
            );
        }

        public object PartTwo(string input) =>
            ChineseRemainderTheorem(
                Parse(input).buses
                    .Select(bus => (mod: bus.period, a: bus.period - bus.delay))
                    .ToArray()
            );

        (int earliestDepart, (long period, int delay)[] buses) Parse(string input) {
            var lines = input.Split("\n");
            var earliestDepart = int.Parse(lines[0]);
            var buses = lines[1].Split(",")
                .Select((part, idx) => (part, idx))
                .Where(item => item.part != "x")
                .Select(item => (period: long.Parse(item.part), delay: item.idx))
                .ToArray();
            return (earliestDepart, buses);
        }

        // https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
        long ChineseRemainderTheorem((long mod, long a)[] items) {
            var prod = items.Aggregate(1L, (acc, item) => acc * item.mod);
            var sum = items.Select((item, i) => {
                var p = prod / item.mod;
                return item.a * ModInv(p, item.mod) * p;
            }).Sum();

            return sum % prod;
        }

        long ModInv(long a, long m) => (long)BigInteger.ModPow(a, m - 2, m);
    }
}