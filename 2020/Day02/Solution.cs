using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2020.Day02 {

    record PasswordEntry(int a, int b, char ch, string password);

    class Solution : Solver {

        public string GetName() => "Password Philosophy";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => ValidCount(input, (PasswordEntry pe) => {
            var count = pe.password.Count(ch => ch == pe.ch);
            return pe.a <= count && count <= pe.b;
        });

        int PartTwo(string input) => ValidCount(input, (PasswordEntry pe) => {
            return (pe.password[pe.a - 1] == pe.ch) ^ (pe.password[pe.b - 1] == pe.ch);
        });

        int ValidCount(string input, Func<PasswordEntry, bool> isValid) {
            return (
                from line in input.Split("\n")
                    let parts = line.Split(' ')
                    let range = parts[0].Split('-').Select(int.Parse).ToArray()
                    let ch = parts[1][0]
                    let pe = new PasswordEntry(range[0], range[1], ch, parts[2])
                where
                    isValid(pe)
                select
                    pe
            ).Count();
        }
    }
}