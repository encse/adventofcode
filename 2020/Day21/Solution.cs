using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2020.Day21 {

    [ProblemName("Allergen Assessment")]
    class Solution : Solver {

        public object PartOne(string input) {
            var allIngredients = new HashSet<string>();
            var allAllergens = new HashSet<string>();

            foreach (var line in input.Split("\n")) {
                var parts = line.Trim(')').Split(" (contains ");
                allIngredients.UnionWith(parts[0].Split(" "));
                allAllergens.UnionWith(parts[1].Split(", "));
            }

            var map = new Dictionary<string, HashSet<string>>();

            foreach (var allergen in allAllergens) {
                var candidates = allIngredients.ToHashSet();
                foreach (var line in input.Split("\n")) {
                    var parts = line.Trim(')').Split(" (contains ");
                    var ingerients = parts[0].Split(" ").ToHashSet();
                    var allergens = parts[1].Split(", ").ToHashSet();

                    if (allergens.Contains(allergen)) {
                        candidates.IntersectWith(ingerients);
                    }
                }
                map[allergen] = candidates;
            }

            var suspicious = new HashSet<string>();
            foreach (var ingredients in map.Values) {
                suspicious.UnionWith(ingredients);
            }

            var c = 0;
            foreach (var line in input.Split("\n")) {
                var parts = line.Trim(')').Split(" (contains ");
                var ingerients = parts[0].Split(" ").ToHashSet();
                c += ingerients.Count(ingredient => !suspicious.Contains(ingredient));
            }

            return c;
        }

        public object PartTwo(string input) {
            var allIngredients = new HashSet<string>();
            var allAllergens = new HashSet<string>();

            foreach (var line in input.Split("\n")) {
                var parts = line.Trim(')').Split(" (contains ");
                allIngredients.UnionWith(parts[0].Split(" "));
                allAllergens.UnionWith(parts[1].Split(", "));
            }

            var map = new Dictionary<string, HashSet<string>>();

            foreach (var allergen in allAllergens) {
                map[allergen] = allIngredients.ToHashSet();
            }

            while (map.Values.Any(v => v.Count > 1)) {
                foreach (var allergen in allAllergens) {
                    var candidates = map[allergen];
                    if (candidates.Count == 1) {
                        foreach(var allergenT in allAllergens){
                            if(allergen != allergenT){
                                map[allergenT].Remove(candidates.Single());
                            }
                        }
                    } else {
                        foreach (var line in input.Split("\n")) {
                            var parts = line.Trim(')').Split(" (contains ");
                            var ingerients = parts[0].Split(" ").ToHashSet();
                            var allergens = parts[1].Split(", ").ToHashSet();

                            if (allergens.Contains(allergen)) {
                                candidates.IntersectWith(ingerients);
                            }
                        }
                    }
                }

            }

            return string.Join(",", allAllergens.OrderBy(a => a).Select(a => map[a].Single()));
        }
    }
}