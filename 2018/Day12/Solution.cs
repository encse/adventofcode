using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day12 {

    class Solution : Solver {

        public string GetName() => "Subterranean Sustainability";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        long PartOne(string input) => Iterate(input, 20);

        long PartTwo(string input) => Iterate(input, 50000000000);

        long Iterate(string input, long iterations) {
            var (state, rules) = Parse(input);

            var dLeftPos = 0L;

            while (iterations > 0) {
                var prevState = state;
                state = Step(state, rules);
                iterations--;
                dLeftPos = state.left - prevState.left;
                if (state.pots == prevState.pots) {
                    state = new State { left = state.left + iterations * dLeftPos, pots = state.pots };
                    break;
                }
            }

            return Enumerable.Range(0, state.pots.Length).Select(i => state.pots[i] == '#' ? i + state.left : 0).Sum();
        }

        State Step(State state, Dictionary<string, string> rules) {
            var pots = "....." + state.pots + ".....";
            var newPots = "";
            for (var i = 2; i < pots.Length - 2; i++) {
                var x = pots.Substring(i - 2, 5);
                newPots += rules.TryGetValue(x, out var ch) ? ch : ".";
            }

            var firstFlower = newPots.IndexOf("#");
            var newLeft = firstFlower + state.left - 3;

            newPots = newPots.Substring(firstFlower);
            newPots = newPots.Substring(0, newPots.LastIndexOf("#") + 1);
            var res = new State { left = newLeft, pots = newPots };

            return res;
        }

        (State state, Dictionary<string, string> rules) Parse(string input) {
            var lines = input.Split("\n");
            var state = new State { left = 0, pots = lines[0].Substring("initial state: ".Length) };
            var rules = (from line in lines.Skip(2) let parts = line.Split(" => ") select new { key = parts[0], value = parts[1] }).ToDictionary(x => x.key, x => x.value);
            return (state, rules);
        }
    }

    class State {
        public long left;
        public string pots;
    }

}