using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Y2020.Day13 {

    [ProblemName("Shuttle Search")]
    class Solution : Solver {

        public object PartOne(string input) {
            var lines = input.Split("\n");
            var t0 = int.Parse(lines[0]);
            var minWait = int.MaxValue;
            var res = 0;
            foreach (var bus in lines[1].Replace("x,", "").Split(",").Select(int.Parse)) {
                var wait = (bus - (t0 % bus)) % bus;
                if (wait < minWait) {
                    res = bus * wait;
                    minWait = wait;
                }

            }
            return res;
        }

        public object PartTwo(string input) {
            var lines = input.Split("\n");
            var buses = new List<long>();
            var waits = new List<long>();
            var parts = lines[1].Split(",");
            for (var i = 0; i < parts.Length; i++) {
                if (parts[i] != "x") {
                    var bus = long.Parse(parts[i]);
                    buses.Add(bus);
                    waits.Add(bus - i);
                }
            }

            return ChineseRemainderTheorem(buses.ToArray(), waits.ToArray());
        }

        // https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
        long ChineseRemainderTheorem(long[] mods, long[] bs) {
            var prod = mods.Aggregate(1L, (acc, j) => acc * j);
            var sm = mods.Select((n, i) => {
                var p = prod / n;
                return bs[i] * ModInv(p, n) * p;
            }).Sum();

            return sm % prod;
        }

        long ModInv(long a, long m) => (long)BigInteger.ModPow(a, m - 2, m);
    }
}