using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.Day07 {

    [ProblemName("Handy Haversacks")]
    class Solution : Solver {

        public object PartOne(string input) {
            var parentsOf = new Dictionary<string, HashSet<string>>();
            foreach (var line in input.Split("\n")) {
                var descr = ParseLine(line);

                foreach (var (_, bag) in descr.children) {
                    if (!parentsOf.ContainsKey(bag)) {
                        parentsOf[bag] = new HashSet<string>();
                    }
                    parentsOf[bag].Add(descr.bag);
                }
            }

            IEnumerable<string> PathsToRoot(string bag) {
                yield return bag;

                if (parentsOf.ContainsKey(bag)) {
                    foreach (var container in parentsOf[bag]) {
                        foreach (var bagT in PathsToRoot(container)) {
                            yield return bagT;
                        }
                    }
                }
            }

            return PathsToRoot("shiny gold bag").ToHashSet().Count - 1;
        }

        public object PartTwo(string input) {
            var childrenOf = new Dictionary<string, List<(int count, string bag)>>();
            foreach (var line in input.Split("\n")) {
                var descr = ParseLine(line);
                childrenOf[descr.bag] = descr.children;
            }

            long CountWithChildren(string bag) =>
                1 + (from child in childrenOf[bag] select child.count * CountWithChildren(child.bag)).Sum();

            return CountWithChildren("shiny gold bag") - 1;
        }

        (string bag, List<(int count, string bag)> children) ParseLine(string line){
            var bag = Regex.Match(line, "^[a-z]+ [a-z]+ bag").Value;

            var children =
                Regex
                    .Matches(line, "(\\d+) ([a-z]+ [a-z]+ bag)")
                    .Select(x => (count: int.Parse(x.Groups[1].Value), bag: x.Groups[2].Value))
                    .ToList();
            
            return (bag, children);
        }

    }
}