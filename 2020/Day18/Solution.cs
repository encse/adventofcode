using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.Day18 {

    [ProblemName("Operation Order")]
    class Solution : Solver {

        public object PartOne(string input) => Solve(input, Eval1);
        public object PartTwo(string input) => Solve(input, Eval2);

        long Solve(string input, Func<string, long> eval) =>
            (from line in input.Replace(" ", "").Split("\n") select eval(line)).Sum();

        long Eval1(string line) {
            bool accept(string st) {
                if (line.StartsWith(st)) {
                    line = line.Substring(st.Length);
                    return true;
                }
                return false;
            }

            long primary() {
                if (accept("(")) {
                    var res = expr();
                    accept(")");
                    return res;
                } else {
                    var m = Regex.Match(line, "^[0-9]+").Value;
                    line = line.Substring(m.Length);
                    return long.Parse(m);
                }
            }

            long expr() {
                var res = primary();
                while (true) {
                    if (accept("*")) {
                        res = res * primary();
                    } else if (accept("+")) {
                        res = res + primary();
                    } else {
                        break;
                    }
                }
                return res;
            }

            return expr();
        }

        long Eval2(string line) {
            bool accept(string st) {
                if (line.StartsWith(st)) {
                    line = line.Substring(st.Length);
                    return true;
                }
                return false;
            }

            long primary() {
                if (accept("(")) {
                    var res = expr();
                    accept(")");
                    return res;
                } else {
                    var m = Regex.Match(line, "^[0-9]+").Value;
                    line = line.Substring(m.Length);
                    return long.Parse(m);
                }
            }

            long add() {
                var res = primary();
                while (accept("+")) {
                    res = res + primary();
                }
                return res;
            }

            long expr() {
                var res = add();
                while (true) {
                    if (accept("*")) {
                        res = res * add();
                    } else { 
                        break; 
                    }
                }
                return res;
            }

            return expr();
        }
    }
}