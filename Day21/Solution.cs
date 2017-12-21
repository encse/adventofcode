using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Day21 {

    class Mtx {
        private string[] lines;

        public Mtx(string st) : this(st.Split('\n')) {
        }

        public Mtx(string[] lines) {
            if (lines.Length < 2 || lines.Length > 4) {
                throw new Exception("invalid size");
            }
            if (lines.Any(line => lines.Length != lines.Length)) {
                throw new Exception("invalid size");
            }
            this.lines = lines;
        }


        public Mtx(Mtx[] children) {
            var block = (int)Math.Sqrt(children.Length);
            if (block * block != children.Length) {
                throw new ArgumentException("non square");
            }

            var crow = children[0].lines.Length;

            this.lines = new string[block * crow];
            var iline = 0;

            for (int offset = 0; offset < children.Length; offset += block) {
                for (int irow = 0; irow < crow; irow++) {
                    var line = "";
                    for (int icol = 0; icol < block; icol++) {
                        line += children[offset + icol].lines[irow];
                    }
                    lines[iline] = line;
                    iline++;
                }
            }
        }

        public IEnumerable<Mtx> Split() {
            var d =
                Size % 2 == 0 ? 2 :
                Size % 3 == 0 ? 3 :
                throw new ArgumentException();

            for (int irow = 0; irow < lines.Length; irow += d) {
                for (int icol = 0; icol < lines.Length; icol += d) {
                    var linesChild = new string[d];
                    for (int i = 0; i < d; i++) {
                        linesChild[i] = lines[irow + i].Substring(icol, d);
                    }
                    yield return new Mtx(linesChild);
                }
            }
        }

        public int Size {
            get { return this.lines.Length; }
        }

        public Mtx Flip() {
            var flippedLines = from line in lines select string.Join("", line.Reverse());
            return new Mtx(flippedLines.ToArray());
        }

        public Mtx Rotate() {
            var resMtx = (from line in lines select line.ToArray()).ToArray();
            for (int i = 0; i < lines.Length; i++) {
                for (int j = 0; j < lines.Length; j++) {
                    resMtx[i][j] = lines[j][lines.Length - i - 1];
                }
            }
            var resLines = from line in resMtx select string.Join("", line);
            return new Mtx(resLines.ToArray());
        }

        public override string ToString() {
            return string.Join("\n", lines);
        }

        public int Count() {
            return (from line in lines select line.Count(ch => ch == '#')).Sum();
        }

        public override bool Equals(object obj) {
            if (obj == null || !(obj is Mtx)) {
                return false;
            }
            if (ReferenceEquals(this, obj))
                return true;

            var that = obj as Mtx;
            return this.Size == that.Size && this.ToString() == that.ToString();
        }

        public override int GetHashCode() {
            return this.ToString().GetHashCode();
        }
    }

    class Solution : Solver {

        public string GetName() => "Fractal Art";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Iterate(input, 5);
        int PartTwo(string input) => Iterate(input, 18);

        int Iterate(string input, int count) {
            var gridRoot = new Grid(new Mtx(".#.\n..#\n###"));
            var ruleset = Parse(input);
            for (var i = 0; i < count; i++) {
                gridRoot.ApplyRuleset(ruleset);
            }
            return gridRoot.mtx.Count();
        }

        Rule[] Parse(string input) => (
            from line in input.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line))
            let parts = Regex.Split(line, " => ")
            select new Rule(new Mtx(parts[0].Split('/')), new Mtx(parts[1].Split('/')))
        ).ToArray();
    }

    class Rule {
        public Mtx Left;
        public Mtx Right;
        public Rule(Mtx left, Mtx right) {
            this.Left = left;
            this.Right = right;

        }
        public bool Match(Mtx mtx) {
            return mtx.Equals(Left);
        }
    }

    class Grid {
        public Mtx mtx;

        public Grid(Mtx mtx) {
            this.mtx = mtx;
        }

        public int Size {
            get {
                return mtx.Size;
            }
        }

        public void ApplyRuleset(Rule[] ruleset) {
            this.mtx = new Mtx(
                this.mtx.Split().Select(child => ApplyRuleset(child, ruleset)).ToArray()
            );
        }

        public Mtx ApplyRuleset(Mtx mtxChild, Rule[] ruleset) {
            foreach (var rule in ruleset) {
                foreach (var mtx in Variations(mtxChild)) {
                    if (rule.Match(mtx)) {
                        return rule.Right;
                    }
                }
            }
            throw new Exception();
        }

        IEnumerable<Mtx> Variations(Mtx mtx) {
            for (int i = 0; i < 4; i++) {
                yield return mtx;
                mtx = mtx.Rotate();
            }
            mtx = mtx.Flip();
            for (int i = 0; i < 4; i++) {
                yield return mtx;
                mtx = mtx.Rotate();
            }
        }

    }
}