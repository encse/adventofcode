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
                return t switch {
                    JObject v when skipRed && v.Values().Contains("red") => 0,
                    JObject v => v.Values().Select(Traverse).Sum(),
                    JArray v => v.Select(Traverse).Sum(),
                    JValue v when v.Type == JTokenType.Integer => (int)v,
                    _ => 0
                };
            }
            return Traverse(JToken.Parse(input));
        }
    }
}