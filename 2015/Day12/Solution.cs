using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

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
            int Traverse(JToken t) {
                switch (t) {
                    case JObject v when skipRed && v.Values().Contains("red"): return 0;
                    case JObject v: return v.Values().Select(Traverse).Sum();
                    case JArray v: return v.Select(Traverse).Sum();
                    case JValue v when v.Type == JTokenType.Integer: return (int)v;
                    default: return 0;
                }
            }
            return Traverse(JToken.Parse(input));
        }
    }
}