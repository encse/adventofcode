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

        public IEnumerable<Mtx> Split() {
            if (lines.Length != 4) {
                throw new ArgumentException();
            }
            for (int irow = 0; irow < lines.Length; irow += 2) {
                for (int icol = 0; icol < lines.Length; icol += 2) {
                    var linesChild = new string[2];
                    for (int i = 0; i < 2; i++) {
                        linesChild[i] = lines[irow + i].Substring(icol, 2);
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

        int PartOne(string input) {

            var gridRoot = new Grid(new Mtx(".#.\n..#\n###"));
            var ruleset = Parse(input);
            Console.WriteLine(gridRoot);
              Console.WriteLine(gridRoot.Size);
                Console.WriteLine();
            for (var i = 0; i < 5; i++) {
                gridRoot.ApplyRuleset(ruleset);
                Console.WriteLine(gridRoot);
                Console.WriteLine(gridRoot.Size);
                Console.WriteLine();
            }
            var count = 0;
            var q = new Queue<Grid>();
            q.Enqueue(gridRoot);
            while (q.Any()) {
                var grid = q.Dequeue();
                if (grid.mtx != null) {
                    count += grid.mtx.Count();
                } else {
                    foreach (var gridChild in grid.Children) {
                        q.Enqueue(gridChild);
                    }
                }
            }
            return count;
        }

        string PartTwo(string input) {
            return "";
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
        public Grid[] Children;
        public Mtx mtx;

        public Grid(Mtx mtx) {
            this.mtx = mtx;
        }

        public int Size {
           get{
                return mtx?.Size ?? 2 * Children[0].Size;
            }
        }

        public void ApplyRuleset(Rule[] ruleset) {
            if (this.mtx != null) {
                Mtx mtxNew = null;
                foreach (var rule in ruleset) {
                    var mtxNewT = ApplyRule(rule);
                    if (mtxNewT != null) {
                        Console.WriteLine(this.mtx);
                        Console.WriteLine("<-->");
                        Console.WriteLine(rule.Left);
                        Console.WriteLine("    ");
                        if (mtxNew != null) throw new Exception("coki");
                        mtxNew = mtxNewT;
                    }
                }
                if (mtxNew == null) {
                    throw new Exception("no match found");
                }

                if (mtxNew.Size == 3) {
                    if (this.mtx.Size != 2) throw new Exception("coki");
                    this.mtx = mtxNew;
                } else if (mtxNew.Size == 4) {
                    if (this.mtx.Size != 3) throw new Exception("coki");
                    this.mtx = null;
                    Children = (from mtxChild in mtxNew.Split() select new Grid(mtxChild)).ToArray();
                } else {
                    throw new Exception("coki");
                }

            } else {
                foreach (var child in Children) {
                    child.ApplyRuleset(ruleset);
                }
            }
        }

        Mtx ApplyRule(Rule rule) {
            foreach (var mtxT in Variations(mtx)) {
                if (rule.Match(mtxT)) {
                    return rule.Right;
                }
            }
            return null;
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

        public override string ToString() {
            if (this.mtx != null) {
                return mtx.ToString();
            }

            var top = Enumerable.Zip(this.Children[0].ToString().Split('\n'), this.Children[1].ToString().Split('\n'), (c1, c2) => c1 + "|" + c2);
            var bottom = string.Join("\n", Enumerable.Zip(this.Children[2].ToString().Split('\n'), this.Children[3].ToString().Split('\n'), (c1, c2) => c1 + "|" + c2));
            return
                string.Join("\n", top) 
                + "\n" + new string('-', top.First().Length) +"\n" +
                string.Join("\n", bottom)
                ;
        }
    }
}