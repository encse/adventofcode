using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2022.Day21;

[ProblemName("Monkey Math")]
class Solution : Solver {

    public object PartOne(string input) {
        return Parse(input, "root", false).Simplify();
    }

    public object PartTwo(string input) {
        var expr = Parse(input, "root", true) as Eq;

        while (!(expr.left is Var)) {
            expr = Solve(expr);
        }
        return expr.right;
    }

    // one step towards making the equation to look like <variable> = <constant>
    // it is supposed that there is only one variable occurrence in the whole expression tree.
    Eq Solve(Eq eq) =>
        eq.left switch {
            Op(Const l, "+", Expr r) => new Eq(r, new Op(eq.right, "-", l).Simplify()),
            Op(Const l, "*", Expr r) => new Eq(r, new Op(eq.right, "/", l).Simplify()),
            Op(Expr  l, "+", Expr r) => new Eq(l, new Op(eq.right, "-", r).Simplify()),
            Op(Expr  l, "-", Expr r) => new Eq(l, new Op(eq.right, "+", r).Simplify()),
            Op(Expr  l, "*", Expr r) => new Eq(l, new Op(eq.right, "/", r).Simplify()),
            Op(Expr  l, "/", Expr r) => new Eq(l, new Op(eq.right, "*", r).Simplify()),
            Const                    => new Eq(eq.right, eq.left),
            _ => eq
        };

    // parses the input including the special rules for part2 
    // and returns the expression with the specified name
    Expr Parse(string input, string name, bool part2) {

        var context = new Dictionary<string, string[]>();
        foreach (var line in input.Split("\n")) {
            var parts = line.Split(" ");
            context[parts[0].TrimEnd(':')] = parts.Skip(1).ToArray();
        }

        Expr buildExpr(string name) {
            var parts = context[name];
            if (part2) {
                if (name == "humn") {
                    return new Var("humn");
                } else if (name == "root") {
                    return new Eq(buildExpr(parts[0]), buildExpr(parts[2]));
                }
            }
            if (parts.Length == 1) {
                return new Const(long.Parse(parts[0]));
            } else {
                return new Op(buildExpr(parts[0]), parts[1], buildExpr(parts[2]));
            }
        }

        return buildExpr(name);
    }

    // standard expression tree representation
    interface Expr {
        Expr Simplify();
    }

    record Const(long Value) : Expr {
        public override string ToString() => Value.ToString();
        public Expr Simplify() => this;
    }

    record Var(string name) : Expr {
        public override string ToString() => name;
        public Expr Simplify() => this;
    }

    record Eq(Expr left, Expr right) : Expr {
        public override string ToString() => $"{left} == {right}";
        public Expr Simplify() => new Eq(left.Simplify(), right.Simplify());
    }

    record Op(Expr left, string op, Expr right) : Expr {
        public override string ToString() => $"({left}) {op} ({right})";
        public Expr Simplify() {
            return (left.Simplify(), op, right.Simplify()) switch {
                (Const l, "+", Const r) => new Const(l.Value + r.Value),
                (Const l, "-", Const r) => new Const(l.Value - r.Value),
                (Const l, "*", Const r) => new Const(l.Value * r.Value),
                (Const l, "/", Const r) => new Const(l.Value / r.Value),
                (Expr l, _, Expr r) => new Op(l, op, r),
            };
        }
    }
}
