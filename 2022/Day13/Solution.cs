using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace AdventOfCode.Y2022.Day13;

[ProblemName("Distress Signal")]
class Solution : Solver {

    public object PartOne(string input) =>
        GetPackets(input)
            .Chunk(2)
            .Select((pair, index) => Compare(pair[0], pair[1]) < 0 ? index + 1 : 0)
            .Sum();

    public object PartTwo(string input) {
        var p1 = JsonNode.Parse("[[2]]");
        var p2 = JsonNode.Parse("[[6]]");
        var packets = GetPackets(input).Append(p1).Append(p2).ToList();
        packets.Sort(Compare);
        return (packets.IndexOf(p1) + 1) * (packets.IndexOf(p2) + 1);
    }

    IEnumerable<JsonNode> GetPackets(string input) =>
         input
            .Split("\n")
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(line => JsonNode.Parse(line));

    int Compare(JsonNode left, JsonNode right) {
        if (left is JsonValue leftVal && right is JsonValue rightVal){
            return left.GetValue<int>() - right.GetValue<int>();
        }

        var leftArray = left switch { JsonArray a => a, _ => new JsonArray(left.GetValue<int>())};
        var rightArray = right switch { JsonArray a => a, _ => new JsonArray(right.GetValue<int>())};
        foreach (var (leftItem, rightItem) in Enumerable.Zip(leftArray, rightArray)) {
            var c = Compare(leftItem, rightItem);
            if (c != 0) {
                return c;
            }
        }
        return leftArray.Count - rightArray.Count;
    }
}