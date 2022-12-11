using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day11;

[ProblemName("Monkey in the Middle")]
class Solution : Solver {

    public object PartOne(string input) {
        var monkeys = ParseMonkeys(input);
        Run(20, monkeys, w => w / 3);
        return GetMonkeyBusinessLevel(monkeys);
    }

    public object PartTwo(string input) {
        var monkeys = ParseMonkeys(input);
        var mod = monkeys.Aggregate(1, (mod, monkey) => mod * monkey.mod);
        Run(10_000, monkeys, w => w % mod);
        return GetMonkeyBusinessLevel(monkeys);
    }

    Monkey[] ParseMonkeys(string input) =>
        input.Split("\n\n").Select(ParseMonkey).ToArray();

    Monkey ParseMonkey(string input) {
        var monkey = new Monkey();

        foreach (var line in input.Split("\n")) {
            var tryParse = LineParser(line);
            if (tryParse(@"Monkey (\d+)", out var arg)) {
                // pass
            } else if (tryParse("Starting items: (.*)", out arg)) {
                monkey.items = new Queue<long>(arg.Split(", ").Select(long.Parse));
            } else if (tryParse(@"Operation: new = old \* old", out _)) {
                monkey.operation = old => old * old;
            } else if (tryParse(@"Operation: new = old \* (\d+)", out arg)) {
                monkey.operation = old => old * int.Parse(arg);
            } else if (tryParse(@"Operation: new = old \+ (\d+)", out arg)) {
                monkey.operation = old => old + int.Parse(arg);
            } else if (tryParse(@"Test: divisible by (\d+)", out arg)) {
                monkey.mod = int.Parse(arg);
            } else if (tryParse(@"If true: throw to monkey (\d+)", out arg)) {
                monkey.passToMonkeyIfDivides = int.Parse(arg);
            } else if (tryParse(@"If false: throw to monkey (\d+)", out arg)) {
                monkey.passToMonkeyOtherwise = int.Parse(arg);
            } else {
                throw new ArgumentException(line);
            }
        }
        return monkey;
    }

    long GetMonkeyBusinessLevel(IEnumerable<Monkey> monkeys) => 
        monkeys
            .OrderByDescending(monkey => monkey.inspectedItems)
            .Take(2)
            .Aggregate(1L, (res, monkey) => res * monkey.inspectedItems);

    void Run(int rounds, Monkey[] monkeys, Func<long, long> updateWorryLevel) {
        for (var i = 0; i < rounds; i++) {
            foreach (var monkey in monkeys) {
                while (monkey.items.Any()) {
                    monkey.inspectedItems++;

                    var item = monkey.items.Dequeue();
                    item = monkey.operation(item);
                    item = updateWorryLevel(item);

                    var targetMonkey = item % monkey.mod == 0 ?
                        monkey.passToMonkeyIfDivides :
                        monkey.passToMonkeyOtherwise;

                    monkeys[targetMonkey].items.Enqueue(item);
                }
            }
        }
    }

    class Monkey {
        public Queue<long> items;
        public Func<long, long> operation;
        public int inspectedItems;
        public int mod;
        public int passToMonkeyIfDivides, passToMonkeyOtherwise;
    }

    // converts a line into a tryParse-style parser function
    TryParse LineParser(string line) {
        bool match(string pattern, out string arg) {
            var m = Regex.Match(line, pattern);
            if (m.Success) {
                arg = m.Groups[m.Groups.Count - 1].Value;
                return true;
            } else {
                arg = "";
                return false;
            }
        }
        return match;
    }
    delegate bool TryParse(string pattern, out string arg);
}
