using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode.Y2020.Day18 {

    [ProblemName("Operation Order")]
    class Solution : Solver {

        public object PartOne(string input) => Solve(input, (line) => Eval(line, false));
        public object PartTwo(string input) => Solve(input, (line) => Eval(line, true));

        long Solve(string input, Func<string, long> eval) =>
            (from line in input.Replace(" ", "").Split("\n") select eval(line)).Sum();

        long Eval(string line, bool p2) {
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
                    case '*':
                        evalUntil("(");
                        opStack.Push('*');
                        break;
                    case '+':
                        evalUntil(p2 ? "(*" : "(");
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

            return valStack.Single();
        }
    }
}