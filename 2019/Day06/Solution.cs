using System.Collections.Generic;
using System.Linq;
using ChildToParent = System.Collections.Generic.Dictionary<string, string>;

namespace AdventOfCode.Y2019.Day06;

[ProblemName("Universal Orbit Map")]
class Solution : Solver {

    public object PartOne(string input) {
        var childToParent = ParseTree(input);
        return (
            from node in childToParent.Keys
            select GetAncestors(childToParent, node).Count()
        ).Sum();
    }

    public object PartTwo(string input) {
        var childToParent = ParseTree(input);
        var ancestors1 = new Stack<string>(GetAncestors(childToParent, "YOU"));
        var ancestors2 = new Stack<string>(GetAncestors(childToParent, "SAN"));
        while (ancestors1.Peek() == ancestors2.Peek()) {
            ancestors1.Pop();
            ancestors2.Pop();
        }
        return ancestors1.Count + ancestors2.Count;
    }

    ChildToParent ParseTree(string input) =>
        input
            .Split("\n")
            .Select(line => line.Split(")"))
            .ToDictionary(
                parent_child => parent_child[1],
                parent_child => parent_child[0]
            );

    IEnumerable<string> GetAncestors(ChildToParent childToParent, string node) {
        for (
            var parent = childToParent[node];
            parent != null;
            parent = childToParent.GetValueOrDefault(parent, null)
        ) {
            yield return parent;
        }

    }
}
