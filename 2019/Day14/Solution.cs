using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode.Y2019.Day14 {

    [ProblemName("Space Stoichiometry")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => Parse(input)(1);
        long PartTwo(string input) {
            var oreForFuel = Parse(input);

            var ore = 1000000000000L;

            var fuel = 1L;
            while (true) {
                // newFuel <= the amount we can produce with the given ore
                // since (double)ore / oreForFuel(fuel) >= 1, fuel becomes
                // a better estimation in each iteration until it reaches 
                // the maximum

                var newFuel = (int)((double)ore / oreForFuel(fuel) * fuel);

                if (newFuel == fuel) {
                    return newFuel;
                }
                fuel = newFuel;
            }
        }

        Func<long, long> Parse(string productionRules) {
            (string chemical, long amount) ParseReagent(string st) {
                var parts = st.Split(" ");
                return (parts[1], long.Parse(parts[0]));
            }

            var reactions = (
                from rule in productionRules.Split("\n")
                let inout = rule.Split(" => ")
                let input = inout[0].Split(", ").Select(ParseReagent).ToArray()
                let output = ParseReagent(inout[1])
                select (output, input)
            ).ToDictionary(inout => inout.output.chemical, inout => inout);

            return (fuel) => {

                var ore = 0L;
                var inventory = reactions.Keys.ToDictionary(chemical => chemical, _ => 0L);
                var productionList = new Queue<(string chemical, long amount)>();
                productionList.Enqueue(("FUEL", fuel));

                while (productionList.Any()) {
                    var (chemical, amount) = productionList.Dequeue();
                    if (chemical == "ORE") {
                        ore += amount;
                    } else {
                        var reaction = reactions[chemical];

                        var useFromInventory = Math.Min(amount, inventory[chemical]);
                        amount -= useFromInventory;
                        inventory[chemical] -= useFromInventory;

                        if (amount > 0) {
                            var multiplier = (long)Math.Ceiling((decimal)amount / reaction.output.amount);
                            inventory[chemical] = Math.Max(0, multiplier * reaction.output.amount - amount);

                            foreach (var reagent in reaction.input) {
                                productionList.Enqueue((reagent.chemical, reagent.amount * multiplier));
                            }
                        }
                    }
                }
                return ore;
            };
        }
    }
}