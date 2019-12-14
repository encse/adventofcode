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
        private Dictionary<string, int> stepsToProduce = new Dictionary<string, int>();

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

            stepsToProduce["ORE"] = 0;
        }

        public long OreNeededForFuel(long fuel) {
            var productionList = new ProductionList(StepsToProduce);
            productionList.Add(new Chemical { name = "FUEL", amount = fuel });
            var ore = 0L;
            while (productionList.Any()) {
                var chemical = productionList.Next();
                if (chemical.name == "ORE") {
                    ore += chemical.amount;
                } else {
                    foreach (var chemicalT in ChemicalsToProduce(chemical)) {
                        productionList.Add(chemicalT);
                    }
                }
            }
            return ore;
        }

        private Chemical[] ChemicalsToProduce(Chemical chemical) {
            var rule = reactions[chemical.name];
            var multiplier = (long)Math.Ceiling((decimal)chemical.amount / rule.output.amount);
            return (
                from chemicalT in rule.input
                select new Chemical { name = chemicalT.name, amount = chemicalT.amount * multiplier }
            ).ToArray();
        }

        private int StepsToProduce(string name) {

            if (!stepsToProduce.ContainsKey(name)) {
                var inputChemicals = reactions[name].input;
                stepsToProduce[name] = inputChemicals.Select(checmical => StepsToProduce(checmical.name)).Max() + 1;
            }

            return stepsToProduce[name];
        }

    }

    class ProductionList {
        private Func<string, int> getPriority;
        private SortedDictionary<long, Dictionary<string, long>> tasksByPriority =
            new SortedDictionary<long, Dictionary<string, long>>();

        public ProductionList(Func<string, int> getPriority) {
            this.getPriority = getPriority;
        }

        public void Add(Chemical chemical) {
            var priority = getPriority(chemical.name);
            if (!tasksByPriority.ContainsKey(priority)) {
                tasksByPriority[priority] = new Dictionary<string, long>();
            }
            var tasks = tasksByPriority[priority];
            tasks[chemical.name] = tasks.GetValueOrDefault(chemical.name) + chemical.amount;
        }

        public Chemical Next() {
            var task = tasksByPriority.Last();
            var chemicalsByName = task.Value;
            var name = chemicalsByName.Keys.First();
            var amount = chemicalsByName[name];

            chemicalsByName.Remove(name);
            if (chemicalsByName.Count == 0) {
                tasksByPriority.Remove(task.Key);
            }
            return new Chemical { name = name, amount = amount };
        }

        public bool Any() => tasksByPriority.Any();
    }

}