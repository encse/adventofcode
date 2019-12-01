using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2016.Day04 {

    class Solution : Solver {

        public string GetName() => "Security Through Obscurity";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => (
            from i in Parse(input)
            let name = i.name.Replace("-", "")
            let computedChecksum = string.Join("", (from ch in name group ch by ch into g orderby -g.Count(), g.Key select g.Key).Take(5))
            where computedChecksum == i.checksum
            select i.sectorid
        ).Sum();

        int PartTwo(string input) => (
            from i in Parse(input)
            let name = string.Join("", from ch in i.name select ch == '-' ? ' ' : (char)('a' + (ch - 'a' + i.sectorid) % 26))
            where name.Contains("northpole")
            select i.sectorid
        ).Single();

        IEnumerable<(string name, int sectorid, string checksum)> Parse(string input){
            var rx = new Regex(@"([^\d]+)\-(\d+)\[(.*)\]");
            
            return from line in input.Split('\n')
                let m = rx.Match(line)
                select (m.Groups[1].Value, int.Parse(m.Groups[2].Value), m.Groups[3].Value);
             
        }
    }
}