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

        int PartOne(string input) {
            int Traverse(JToken v) {
                switch (v) {
                    case JObject t: return t.Properties().Select(p => Traverse(p.Value)).Sum();
                    case JArray t: return t.Select(p => Traverse(p)).Sum();
                    case JValue t when v.Type == JTokenType.Integer: return (int)t;
                    default: return 0;
                }
            }
            return Traverse(JToken.Parse(input));
        }

        int PartTwo(string input) {
           int Traverse(JToken v){
               switch (v) {
                    case JObject t when t.Properties().Any(p => p.Value.Type == JTokenType.String && (string)p.Value == "red"): return 0;
                    case JObject t: return t.Properties().Select(p => Traverse(p.Value)).Sum();
                    case JArray t: return t.Select(p => Traverse(p)).Sum();
                    case JValue t when v.Type == JTokenType.Integer: return (int)t;
                    default: return 0;
                }
            }
            return Traverse(JToken.Parse(input));
        }
    }
}