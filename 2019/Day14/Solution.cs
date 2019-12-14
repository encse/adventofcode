using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode.Y2019.Day14 {

    class Solution : Solver {

        public string GetName() => "Space Stoichiometry";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => new NanoFactory(input).OreNeededForFuel(1);
        long PartTwo(string input) {
            var nanoFactory = new NanoFactory(input);
            var ore = 1000000000000;

            var hi = 1;
            while (nanoFactory.OreNeededForFuel(hi) < ore) {
                hi *= 2;
            }

            var lo = hi / 2;
            while (hi - lo > 1) {
                var m = (hi + lo) / 2;
                if (nanoFactory.OreNeededForFuel(m) > ore) {
                    hi = m;
                } else {
                    lo = m;
                }
            }

            return lo;
        }
    }

    class Reactions : Dictionary<string, (Chemical output, Chemical[] input)> { }

    class Chemical {
        public string name;
        public long amount;
    }

    class NanoFactory {
        private Reactions reactions = new Reactions();

        public NanoFactory(string productionRules) {

            Chemical ParseChemical(string st) {
                var parts = st.Split(" ");
                return new Chemical { name = parts[1], amount = long.Parse(parts[0]) };
            }

            foreach (var rule in productionRules.Split("\n")) {
                var inout = rule.Split(" => ");
                var input = inout[0].Split(", ").Select(ParseChemical).ToArray();
                var output = ParseChemical(inout[1]);
                reactions[output.name] = (output, input);
            }

        }

        public long OreNeededForFuel(long fuel) {
            var productionList = new Queue<Chemical>();
            productionList.Enqueue(new Chemical { name = "FUEL", amount = fuel });
            var ore = 0L;
            var inventory = new Dictionary<string, long>();
            while (productionList.Any()) {
                var chemical = productionList.Dequeue();
                if (chemical.name == "ORE") {
                    ore += chemical.amount;
                } else {
                    var rule = reactions[chemical.name];
                    
                    var amountNeeded = chemical.amount;
                    if (inventory.ContainsKey(chemical.name)) {
                        var use = Math.Min(amountNeeded, inventory[chemical.name]);
                        amountNeeded -= use;
                        inventory[chemical.name] -= use;
                    }

                    if (amountNeeded > 0) {
                        var multiplier = (long)Math.Ceiling((decimal)amountNeeded / rule.output.amount);
                        var chemicalsToProduce = (
                            from chemicalT in rule.input
                            select new Chemical { name = chemicalT.name, amount = chemicalT.amount * multiplier }
                        ).ToArray();

                        inventory[chemical.name] = Math.Max(0, multiplier * rule.output.amount - amountNeeded);

                        foreach (var chemicalT in chemicalsToProduce) {
                            productionList.Enqueue(chemicalT);
                        }
                    }
                }
            }
            return ore;
        }
    }
}