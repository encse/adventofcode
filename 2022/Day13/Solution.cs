using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace AdventOfCode.Y2022.Day13;

[ProblemName("Distress Signal")]
class Solution : Solver {

    public object PartOne(string input) =>
        GetPackets(input)
            .Chunk(2)
            .Select((pair, index) => Compare(pair[0], pair[1]) < 0 ? index + 1 : 0)
            .Sum();

    public object PartTwo(string input) {
        var sorted = GetPackets("[[2]]\n[[6]]\n" + input)
            .OrderBy(Compare)
            .Select(packet => packet.ToString())
            .ToList();
        return (sorted.IndexOf("[[2]]") + 1) * (sorted.IndexOf("[[6]]") + 1);
    }

    IEnumerable<JsonElement> GetPackets(string input) =>
         input
            .Split("\n")
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(line => JsonDocument.Parse(line).RootElement);

    int Compare(JsonElement left, JsonElement right) =>
        (left.ValueKind, right.ValueKind) switch {
            (JsonValueKind.Number, JsonValueKind.Number) => left.GetInt32() - right.GetInt32(),
            (JsonValueKind.Array, JsonValueKind.Array) => Compare(left.EnumerateArray().ToArray(), right.EnumerateArray().ToArray()),
            (JsonValueKind.Number, JsonValueKind.Array) => Compare(new[] { left }, right.EnumerateArray().ToArray()),
            (JsonValueKind.Array, JsonValueKind.Number) => Compare(left.EnumerateArray().ToArray(), new[] { right }),
            _ => throw new ArgumentException()
        };

    int Compare(JsonElement[] leftArray, JsonElement[] rightArray) {
        foreach (var (left, right) in Enumerable.Zip(leftArray, rightArray)) {
            var c = Compare(left, right);
            if (c != 0) {
                return c;
            }
        }
        return leftArray.Length - rightArray.Length;
    }
}

file static class Extensions {
    public static List<T> OrderBy<T>(this IEnumerable<T> items, Comparison<T> comparison){
        var list = items.ToList();
        list.Sort(comparison);
        return list;
    }
}