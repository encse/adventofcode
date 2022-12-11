using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

namespace AdventOfCode.Y2022.Day11;

[ProblemName("Monkey in the Middle")]
class Solution : Solver {

    public object PartOne(string input) {
        var monkeys = Parse(input);
        for(var i =0;i<20;i++) {
            Round(monkeys, worryLevel => worryLevel /3);
        }
        var res = monkeys.OrderByDescending(m => m.inspected).Take(2).ToArray();
        return res[0].inspected * res[1].inspected;
    }

    public object PartTwo(string input) {
        var monkeys = Parse(input);

        var mod = 1;
        foreach(var m in monkeys){
            mod *= m.test;
        }

        for(var i =0;i<10000;i++) {
            Round(monkeys, worryLevel => worryLevel % mod);
        }
        var res = monkeys.OrderByDescending(m => m.inspected).Take(2).ToArray();
        return (long)res[0].inspected * (long)res[1].inspected;
    }


    List<Monkey> Parse(string input) {
        var monkeys = new List<Monkey>();
        foreach (var block in input.Split("\n\n")) {
            var lines = block.Split("\n");
            var items = new Queue<long>(lines[1].Split("Starting items: ")[1].Split(", ").Select(long.Parse));
            Func<long,long> operation;
            if (lines[2].Contains("Operation: new = old * old")) {
                operation = (long old) => old * old;
            } else if (lines[2].Contains("Operation: new = old * ")) {
                var arg = int.Parse(lines[2].Split("Operation: new = old * ")[1]);
                operation = (long old) => old * arg;
            } else {
                var arg = int.Parse(lines[2].Split("Operation: new = old + ")[1]);
                operation = (long old) => old + arg;
            }

            var test = int.Parse(lines[3].Split("Test: divisible by ")[1]);
            var ifTrue = int.Parse(lines[4].Split("If true: throw to monkey ")[1]);
            var ifFalse =int.Parse(lines[5].Split("If false: throw to monkey ")[1]);
            var monkey = new Monkey(
                worryLevels:items,
                operation: operation,
                test: test,
                ifTrue: ifTrue,
                ifFalse: ifFalse
            );
            monkeys.Add(monkey);
        }
        return monkeys;
    }
    void Round(List<Monkey> monkeys, Func<long, long> updateWorryLevel) {
       

        foreach (var monkey in monkeys) {
            while (monkey.worryLevels.Any()) {
                var worryLevel = monkey.worryLevels.Dequeue();
                worryLevel = monkey.operation(worryLevel);
                monkey.inspected++;

                worryLevel = updateWorryLevel(worryLevel);

                if (worryLevel % monkey.test == 0) {
                    monkeys[monkey.ifTrue].worryLevels.Enqueue(worryLevel);
                } else {
                    monkeys[monkey.ifFalse].worryLevels.Enqueue(worryLevel);
                }
            }
        }
    }

    record Monkey(Queue<long> worryLevels, Func<long, long> operation, int test, int ifTrue, int ifFalse) {
        public int inspected { get; set; }
    }
}
