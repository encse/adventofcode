using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day21 {

    record Problem(
        HashSet<string> allergens, 
        HashSet<string> ingredients, 
        (HashSet<string> ingredients, HashSet<string> allergens)[] mapping);

    [ProblemName("Allergen Assessment")]
    class Solution : Solver {

        public object PartOne(string input) {
            var problem = Parse(input);
            var suspiciousIngredients = UnionAll(GetIngredientsByAllergene(problem).Values);
            return problem.mapping.Select(entry => entry.ingredients.Except(suspiciousIngredients).Count()).Sum();
        }

        public object PartTwo(string input) {
            var problem = Parse(input);
            var ingredientsByAllergene = GetIngredientsByAllergene(problem);
            
            // The problem is set up in a way that we identify the allergene - ingredient pairs one by one. 
            while (ingredientsByAllergene.Values.Any(ingredients => ingredients.Count > 1)) {
                foreach (var allergen in problem.allergens) {
                    var candidates = ingredientsByAllergene[allergen];
                    if (candidates.Count == 1) {
                        foreach (var allergenT in problem.allergens) {
                            if (allergen != allergenT) {
                                ingredientsByAllergene[allergenT].Remove(candidates.Single());
                            }
                        }
                    }
                }
            }

            return string.Join(",", problem.allergens.OrderBy(a => a).Select(a => ingredientsByAllergene[a].Single()));
        }

        private Problem Parse(string input) {
            var mapping = (
                from line in input.Split("\n")
                    let parts = line.Trim(')').Split(" (contains ")
                    let ingredients = parts[0].Split(" ").ToHashSet()
                    let allergens = parts[1].Split(", ").ToHashSet()
                select (ingredients, allergens)
            ).ToArray();

            return new Problem(
                UnionAll(from line in mapping select line.allergens),
                UnionAll(from line in mapping select line.ingredients),
                mapping
            );
        }

        private Dictionary<string, HashSet<string>> GetIngredientsByAllergene(Problem problem) =>
            problem.allergens.ToDictionary(
                allergene => allergene, 
                allergene => problem.mapping
                    .Where(entry => entry.allergens.Contains(allergene))
                    .Aggregate(
                        problem.ingredients as IEnumerable<string>,
                        (res, entry) => res.Intersect(entry.ingredients))
                    .ToHashSet());

        private HashSet<string> UnionAll(IEnumerable<HashSet<string>> sets) =>
            sets.Aggregate<IEnumerable<string>>((result, set) => result.Union(set)).ToHashSet();
    }
}