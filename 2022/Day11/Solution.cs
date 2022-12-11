using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day11;

[ProblemName("Monkey in the Middle")]
class Solution : Solver {

    public object PartOne(string input) {
        var monkeys = Parse(input).ToList();
        return Run(20, monkeys, w => w / 3);
    }

    public object PartTwo(string input) {
        var monkeys = Parse(input).ToList();
        var mod = monkeys.Aggregate(1, (mod, monkey) => mod * monkey.mod);
        return Run(10_000, monkeys, w => w % mod);
    }

    IEnumerable<Monkey> Parse(string input) => 
        from block in input.Split("\n\n") select ParseMonkey(block);

    Monkey ParseMonkey(string input) {
        var monkey = new Monkey();

        foreach (var match in input.Split("\n").Select(LineParser)) {
            match("Starting items: (.*)", (arg) => {
                monkey.items = new Queue<long>(arg.Split(", ").Select(long.Parse));
            });
            match(@"Operation: new = old \* old", _ => {
                monkey.operation = old => old * old;
            });
            match(@"Operation: new = old \* (\d+)", arg => {
                monkey.operation = old => old * int.Parse(arg);
            });
            match(@"Operation: new = old \+ (\d+)", arg => {
                monkey.operation = old => old + int.Parse(arg);
            });
            match(@"Test: divisible by (\d+)", arg => {
                monkey.mod = int.Parse(arg);
            });
            match(@"If true: throw to monkey (\d+)", arg => {
                monkey.passToMonkeyIfDivides = int.Parse(arg);
            });
            match(@"If false: throw to monkey (\d+)", arg => {
                monkey.passToMonkeyOtherwise = int.Parse(arg);
            });
        }
        return monkey;
    }

    Action<string, Action<string>> LineParser(string line) {
        // if the line matches pattern, takes the value of the last match group and calls the given action with it.
        void match(string pattern, Action<string> action) {
            var m = Regex.Match(line, pattern);
            if (m.Success) {
                action(m.Groups[m.Groups.Count - 1].Value);
            }
        }
        return match;
    }

    long Run(int rounds, List<Monkey> monkeys, Func<long, long> updateWorryLevel) {
        for (var i = 0; i < rounds; i++) {
            Round(monkeys, updateWorryLevel);
        }
        var activeMonkeys = (
            from monkey in monkeys 
            orderby monkey.inspectedItems descending 
            select monkey
        ).ToArray();

        return (long)activeMonkeys[0].inspectedItems * activeMonkeys[1].inspectedItems;
    }

    void Round(List<Monkey> monkeys, Func<long, long> updateWorryLevel) {
        foreach (var monkey in monkeys) {
            while (monkey.items.Any()) {
                monkey.inspectedItems++;

                var item = monkey.items.Dequeue();
                item = monkey.operation(item);
                item = updateWorryLevel(item);

                int targetMonkey = item % monkey.mod == 0 ? 
                    monkey.passToMonkeyIfDivides : 
                    monkey.passToMonkeyOtherwise;

                monkeys[targetMonkey].items.Enqueue(item);
            }
        }
    }

    class Monkey {
        public Queue<long> items;
        public Func<long, long> operation;
        public int mod;
        public int passToMonkeyIfDivides;
        public int passToMonkeyOtherwise;
        public int inspectedItems;
    }
}
