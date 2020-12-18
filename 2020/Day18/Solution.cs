using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Y2020.Day18 {

    [ProblemName("Operation Order")]
    class Solution : Solver {

        public object PartOne(string input) => Solve(input, true);
        public object PartTwo(string input) => Solve(input, false);

        long Solve(string input, bool part1) {
            var sum = 0L;
            foreach (var line in input.Split("\n")) {
                // https://en.wikipedia.org/wiki/Shunting-yard_algorithm

                var opStack = new Stack<char>();
                var valStack = new Stack<long>();

                void evalUntil(string ops) {
                    while (!ops.Contains(opStack.Peek())) {
                        if (opStack.Pop() == '+') {
                            valStack.Push(valStack.Pop() + valStack.Pop());
                        } else {
                            valStack.Push(valStack.Pop() * valStack.Pop());
                        }
                    }
                }

                opStack.Push('(');

                foreach (var ch in line) {
                    switch (ch) {
                        case ' ':
                            break;
                        case '*':
                            evalUntil("(");
                            opStack.Push('*');
                            break;
                        case '+':
                            evalUntil(part1 ? "(" : "(*");
                            opStack.Push('+');
                            break;
                        case '(':
                            opStack.Push('(');
                            break;
                        case ')':
                            evalUntil("(");
                            opStack.Pop();
                            break;
                        default:
                            valStack.Push(long.Parse(ch.ToString()));
                            break;
                    }
                }

                evalUntil("(");

                sum += valStack.Single();
            }

            return sum;
        }
    }
}