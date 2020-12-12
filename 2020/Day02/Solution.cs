using System;
using System.Linq;

namespace AdventOfCode.Y2020.Day02 {

    record PasswordEntry(int a, int b, char ch, string password);

    [ProblemName("Password Philosophy")]
    class Solution : Solver {

        public object PartOne(string input) => ValidCount(input, (PasswordEntry pe) => {
            var count = pe.password.Count(ch => ch == pe.ch);
            return pe.a <= count && count <= pe.b;
        });

        public object PartTwo(string input) => ValidCount(input, (PasswordEntry pe) => {
            return (pe.password[pe.a - 1] == pe.ch) ^ (pe.password[pe.b - 1] == pe.ch);
        });

        int ValidCount(string input, Func<PasswordEntry, bool> isValid) =>
            input
                .Split("\n")
                .Select(line => {
                    var parts = line.Split(' ');
                    var range = parts[0].Split('-').Select(int.Parse).ToArray();
                    var ch = parts[1][0];
                    return new PasswordEntry(range[0], range[1], ch, parts[2]);
                })
                .Count(isValid);
    }
}