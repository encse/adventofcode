using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.Day04 {

    [ProblemName("Passport Processing")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Credentials(input).Count(HasRequiredKeys);
        int PartTwo(string input) => Credentials(input).Count(cred => HasRequiredKeys(cred) && HasRequiredValues(cred));

        Dictionary<string, string> rxs = new Dictionary<string, string>(){
            {"byr", "19[2-9][0-9]|200[0-2]"},
            {"iyr", "201[0-9]|2020"},
            {"eyr", "202[0-9]|2030"},
            {"hgt", "1[5-8][0-9]cm|19[0-3]cm|59in|6[0-9]in|7[0-6]in"},
            {"hcl", "#[0-9a-f]{6}"},
            {"ecl", "amb|blu|brn|gry|grn|hzl|oth"},
            {"pid", "[0-9]{9}"},
        };

        bool HasRequiredKeys(Dictionary<string, string> id) =>
            rxs.Keys.All(key => id.ContainsKey(key));

        bool HasRequiredValues(Dictionary<string, string> id) =>
            id.All(kvp =>
                !rxs.ContainsKey(kvp.Key) ||
                Regex.IsMatch(kvp.Value, "^(" + rxs[kvp.Key] + ")$")
            );

        IEnumerable<Dictionary<string, string>> Credentials(string input) =>
            from block in input.Split("\n\n") 
            select 
                block
                    .Split("\n ".ToCharArray())
                    .Select(part => part.Split(":"))
                    .ToDictionary(parts => parts[0], parts => parts[1]);
    }
}