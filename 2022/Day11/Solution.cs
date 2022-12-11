using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day11;

[ProblemName("Monkey in the Middle")]
class Solution : Solver {

    public object PartOne(string input) {
        var monkeys = Parse(input);
        return Run(20, monkeys, w => w / 3);
    }

    public object PartTwo(string input) {
        var monkeys = Parse(input);

        var mod = 1;
        foreach (var m in monkeys) {
            mod *= m.mod;
        }

        return Run(10_000, monkeys, w => w % mod);
    }

    List<Monkey> Parse(string input) {
        var monkeys = new List<Monkey>();

        foreach (var block in input.Split("\n\n")) {
            var lines = block.Split("\n");

            var monkey = new Monkey();
            foreach (var line in lines) {
                Match(line, "Starting items: (.*)", (arg) => {
                    monkey.items = new Queue<long>(arg.Split(", ").Select(long.Parse));
                });
                Match(line, @"Operation: new = old \* old", _ => {
                    monkey.operation = old => old * old;
                });
                Match(line, @"Operation: new = old \* (\d+)", arg => {
                    monkey.operation = old => old * int.Parse(arg);
                });
                Match(line, @"Operation: new = old \+ (\d+)", arg => {
                    monkey.operation = old => old + int.Parse(arg);
                });
                Match(line, @"Test: divisible by (\d+)", arg => {
                    monkey.mod = int.Parse(arg);
                });
                Match(line, @"If true: throw to monkey (\d+)", arg => {
                    monkey.passToMonkeyIfTrue = int.Parse(arg);
                });
                Match(line, @"If false: throw to monkey (\d+)", arg => {
                    monkey.passToMonkeyIfFalse = int.Parse(arg);
                });
            }
            monkeys.Add(monkey);
        }
        return monkeys;
    }

    void Match(string line, string pattern, Action<string> action) {
        var m = Regex.Match(line, pattern);
        if (m.Success) {
            action(m.Groups[m.Groups.Count - 1].Value);
        }
    }

    long Run(int rounds, List<Monkey> monkeys, Func<long, long> updateWorryLevel) {
        for (var i = 0; i < rounds; i++) {
            Round(monkeys, updateWorryLevel);
        }
        var activeMonkeys = monkeys.OrderByDescending(m => m.inspectedItems).Take(2).ToArray();
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
                    monkey.passToMonkeyIfTrue : 
                    monkey.passToMonkeyIfFalse;

                monkeys[targetMonkey].items.Enqueue(item);
            }
        }
    }

    class Monkey {
        public Queue<long> items;
        public Func<long, long> operation;
        public int mod;
        public int passToMonkeyIfTrue;
        public int passToMonkeyIfFalse;
        public int inspectedItems;
    }
}
