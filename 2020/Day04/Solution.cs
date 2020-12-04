using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.Day04 {

    [ProblemName("Passport Processing")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Ids(input).Count(HasRequiredKeys);
        int PartTwo(string input) => Ids(input).Count(id => HasRequiredKeys(id) && HasRequiredValues(id));

        bool Range(string st, string pattern, int min, int max) {
            var m = Regex.Match(st, "^" + pattern + "$");
            if (!m.Success) {
                return false;
            }
            var v = int.Parse(m.Groups[^1].Value);
            return v >= min && v <= max;
        }

        bool HasRequiredKeys(Dictionary<string, string> id) {
            return "byr iyr eyr hgt hcl ecl pid".Split(' ').All(key => id.ContainsKey(key));
        }

        bool HasRequiredValues(Dictionary<string, string> id) {
            foreach (var kvp in id) {
                var v = kvp.Key switch {
                    "byr" => Range(kvp.Value, @"[0-9]{4}", 1920, 2002),
                    "iyr" => Range(kvp.Value, @"[0-9]{4}", 2010, 2020),
                    "eyr" => Range(kvp.Value, @"[0-9]{4}", 2020, 2030),
                    "hgt" => Range(kvp.Value, @"([0-9]{3})cm", 150, 193) || Range(kvp.Value, @"([0-9]{2})in", 59, 76),
                    "hcl" => Regex.Match(kvp.Value, @"^#[0-9a-f]{6}$").Success,
                    "ecl" => "amb blu brn gry grn hzl oth".Split(" ").Contains(kvp.Value),
                    "pid" => Regex.Match(kvp.Value, @"^[0-9]{9}$").Success,
                    "cid" => true,
                    _ => false
                };

                if (!v) {
                    return false;
                }
            }
            return true;
        }

        IEnumerable<Dictionary<string, string>> Ids(string input) {
            var lines = input.Split("\n");
            for (var i = 0; i < lines.Length; i++) {
                var id = new Dictionary<string, string>();
                while (i < lines.Length && lines[i] != "") {
                    foreach (var item in lines[i].Split(" ")) {
                        var parts = item.Split(":");
                        id.Add(parts[0], parts[1]);
                    }
                    i++;
                }
                yield return id;
            }
        }
    }
}