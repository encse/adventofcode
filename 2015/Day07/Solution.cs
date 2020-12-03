using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2015.Day07 {

    [ProblemName("Some Assembly Required")]
    class Solution : Solver {

        class State : Dictionary<string, int> { }
        class Calc : Dictionary<string, Func<State, int>> { }


        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Parse(input)["a"](new State());

        int PartTwo(string input) {
            var calc = Parse(input);
            return calc["a"](new State() { ["b"] = calc["a"](new State()) });
        }

        Calc Parse(string input) =>
            input.Split('\n').Aggregate(new Calc(), (calc, line) =>
                Gate(calc, line, @"(\w+) AND (\w+) -> (\w+)", pin => pin[0] & pin[1]) ??
                Gate(calc, line, @"(\w+) OR (\w+) -> (\w+)", pin => pin[0] | pin[1]) ??
                Gate(calc, line, @"(\w+) RSHIFT (\w+) -> (\w+)", pin => pin[0] >> pin[1]) ??
                Gate(calc, line, @"(\w+) LSHIFT (\w+) -> (\w+)", pin => pin[0] << pin[1]) ??
                Gate(calc, line, @"NOT (\w+) -> (\w+)", pin => ~pin[0]) ??
                Gate(calc, line, @"(\w+) -> (\w+)", pin => pin[0]) ??
                throw new Exception(line)
            );

        Calc Gate(Calc calc, string line, string pattern, Func<int[], int> op) {
            var match = Regex.Match(line, pattern);
            if (!match.Success) {
                return null;
            }
            var parts = match.Groups.Cast<Group>().Skip(1).Select(g => g.Value).ToArray();
            var pinOut = parts.Last();
            var pins = parts.Take(parts.Length - 1).ToArray();
            calc[pinOut] = (state) => {
                if (!state.ContainsKey(pinOut)) {
                    var args = pins.Select(pin => int.TryParse(pin, out var i) ? i : calc[pin](state)).ToArray();
                    state[pinOut] = op(args);
                }
                return state[pinOut];
            };
            return calc;
        }
    }
}