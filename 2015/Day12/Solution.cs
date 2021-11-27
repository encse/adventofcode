using System.Linq;
using System.Text.Json;

namespace AdventOfCode.Y2015.Day12;

[ProblemName("JSAbacusFramework.io")]
class Solution : Solver {

    public object PartOne(string input) => Solve(input, false);
    public object PartTwo(string input) => Solve(input, true);

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
