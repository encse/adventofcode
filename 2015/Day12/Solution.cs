using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AdventOfCode.Y2015.Day12 {

    class Solution : Solver {

        public string GetName() => "JSAbacusFramework.io";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Solve(input, false);
        int PartTwo(string input) => Solve(input, true);

        int Solve(string input, bool skipRed) {
            int Traverse(JsonElement t) {
                return t.ValueKind switch
                {
                    JsonValueKind.Object when skipRed && t.EnumerateObject().Any(
                        p => p.Value.ValueKind == JsonValueKind.String && p.Value.GetString() == "red") => 0,
                    JsonValueKind.Object => t.EnumerateObject().Select(p => Traverse(p.Value)).Sum(),
                    JsonValueKind.Array => t.EnumerateArray().Select(Traverse).Sum(),
                    JsonValueKind.Number => t.GetInt32(),
                    _ => 0
                };
            }

            return Traverse(JsonDocument.Parse(input).RootElement);
        }
    }
}