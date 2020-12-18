using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.Day18 {

    record Node;
    record Add(Node left, Node right) : Node;
    record Mul(Node left, Node right) : Node;
    record Num(long value) : Node;

    [ProblemName("Operation Order")]
    class Solution : Solver {

        public object PartOne(string input) => Solve(input, Parse1);
        public object PartTwo(string input) => Solve(input, Parse2);
        
        long Solve(string input, Func<string, Node> parse) => 
            (from line in input.Split("\n") select Eval(parse(line))).Sum();

        long Eval(Node node) {
            return node switch {
                Add add => Eval(add.left) + Eval(add.right),
                Mul mul => Eval(mul.left) * Eval(mul.right),
                Num num => num.value,
                _ => throw new Exception()
            };
        }

        Node Parse1(string input) {
            bool accept(string st) {
                if (input.StartsWith(st)) {
                    input = input.Substring(st.Length);
                    return true;
                }
                return false;
            }

            Node primary() {
                if (accept("(")) {
                    var res = expr();
                    accept(")");
                    return res;
                } else {
                    var m = Regex.Match(input, "^[0-9]+").Value;
                    input = input.Substring(m.Length);
                    return new Num(long.Parse(m));
                }
            }

            Node expr() {
                var res = primary();
                while (true) {
                    if (accept("*")) {
                        res = new Mul(res, primary());
                    } else if (accept("+")) {
                        res = new Add(res, primary());
                    } else {
                        break;
                    }
                }
                return res;
            }

            input = input.Replace(" ", "");
            var res = expr();
            return res;
        }

        Node Parse2(string input) {
            bool accept(string st) {
                if (input.StartsWith(st)) {
                    input = input.Substring(st.Length);
                    return true;
                }
                return false;
            }

            Node add() {
                var res = primary();
                while (accept("+")) {
                    res = new Add(res, add());
                }
                return res;
            }

            Node primary() {
                if (accept("(")) {
                    var res = expr();
                    accept(")");
                    return res;
                } else {
                    var m = Regex.Match(input, "^[0-9]+").Value;
                    input = input.Substring(m.Length);
                    return new Num(long.Parse(m));
                }
            }

            Node expr() {
                var res = add();
                while (accept("*")) {
                    res = new Mul(res, add());
                }
                return res;
            }

            input = input.Replace(" ", "");
            var res = expr();
            return res;
        }
    }
}