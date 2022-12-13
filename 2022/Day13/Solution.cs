using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace AdventOfCode.Y2022.Day13;

// NOTES: I don't use C# during the year, so I didn't know which Json parser to use 
// and first went with System.Text.Json, then found a solution on reddit which uses 
// System.Text.Json.Nodes and could improve my coding a bit.
// 
// For part2: I couldn't find a version of OrderBy() that would take a simple comparator 
// function, and I didn't want to implement a full blown IComparer<T> interface. Then 
// realised that List<T> has a Sort function which works with just a simple delegate. 
// Unfortunately it's a void function so there is no way to chain the result further. 
// So much about using just one expression for Part2.
// 
// I didn't have a great idea to deal with the 1 based indexing, but I'm satisfied with 
// how this looks in general. Well mostly. I managed to overgolf the compare function 
// at the end...
// 
[ProblemName("Distress Signal")]
class Solution : Solver {

    public object PartOne(string input) =>
        GetPackets(input)
            .Chunk(2)
            .Select((pair, index) => Compare(pair[0], pair[1]) < 0 ? index + 1 : 0)
            .Sum();

    public object PartTwo(string input) {
        var divider = GetPackets("[[2]]\n[[6]]").ToList();
        var packets = GetPackets(input).Concat(divider).ToList();
        packets.Sort(Compare);
        return (packets.IndexOf(divider[0]) + 1) * (packets.IndexOf(divider[1]) + 1);
    }

    IEnumerable<JsonNode> GetPackets(string input) =>
        from line in input.Split("\n") 
        where !string.IsNullOrEmpty(line) 
        select JsonNode.Parse(line);

    int Compare(JsonNode nodeA, JsonNode nodeB) {
        if (nodeA is JsonValue && nodeB is JsonValue) {
            return (int)nodeA - (int)nodeB;
        } else {
            // It's AoC time, let's exploit FirstOrDefault! 
            // 😈 if all items are equal, compare the length of the arrays 
            var arrayA = nodeA as JsonArray ?? new JsonArray((int)nodeA);
            var arrayB = nodeB as JsonArray ?? new JsonArray((int)nodeB);
            return Enumerable.Zip(arrayA, arrayB)
                .Select(p => Compare(p.First, p.Second))
                .FirstOrDefault(c => c != 0, arrayA.Count - arrayB.Count);
        }
    }
}