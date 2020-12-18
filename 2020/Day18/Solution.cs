using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2020.Day18 {

    record Expr;
    record Add(Expr left, Expr right) : Expr;
    record Mul(Expr left, Expr right) : Expr;
    record Num(long value) : Expr;

    [ProblemName("Operation Order")]
    class Solution : Solver {

        public object PartOne(string input) => Solve(input, Parse1);
        public object PartTwo(string input) => Solve(input, Parse2);
        
        long Solve(string input, Func<string, Expr> parse) => 
            (from line in input.Replace(" ", "").Split("\n") select Eval(parse(line))).Sum();

        long Eval(Expr expr) =>
            expr switch {
                Add add => Eval(add.left) + Eval(add.right),
                Mul mul => Eval(mul.left) * Eval(mul.right),
                Num num => num.value,
                _ => throw new Exception()
            };

        Expr Parse1(string line) {
            bool accept(string st) {
                if (line.StartsWith(st)) {
                    line = line.Substring(st.Length);
                    return true;
                }
                return false;
            }

            Expr primary() {
                if (accept("(")) {
                    var res = expr();
                    accept(")");
                    return res;
                } else {
                    var m = Regex.Match(line, "^[0-9]+").Value;
                    line = line.Substring(m.Length);
                    return new Num(long.Parse(m));
                }
            }

            Expr expr() {
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

            return expr();
        }

        Expr Parse2(string line) {
            bool accept(string st) {
                if (line.StartsWith(st)) {
                    line = line.Substring(st.Length);
                    return true;
                }
                return false;
            }

            Expr add() {
                var res = primary();
                while (accept("+")) {
                    res = new Add(res, add());
                }
                return res;
            }

            Expr primary() {
                if (accept("(")) {
                    var res = expr();
                    accept(")");
                    return res;
                } else {
                    var m = Regex.Match(line, "^[0-9]+").Value;
                    line = line.Substring(m.Length);
                    return new Num(long.Parse(m));
                }
            }

            Expr expr() {
                var res = add();
                while (accept("*")) {
                    res = new Mul(res, add());
                }
                return res;
            }
            
            return expr();
        }
    }
}