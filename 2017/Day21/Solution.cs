using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2017.Day21 {

    class Solution : Solver {

        public string GetName() => "Fractal Art";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Iterate(input, 5);

        int PartTwo(string input) => Iterate(input, 18);

        int Iterate(string input, int iterations) {
            var mtx = Mtx.FromString(".#./..#/###");
            var ruleset = new RuleSet(input);
            for (var i = 0; i < iterations; i++) {
                mtx = ruleset.Apply(mtx);
            }
            return mtx.Count();
        }
    }

    class RuleSet {
        private Dictionary<int, Mtx> rules2;
        private Dictionary<int, Mtx> rules3;

        public RuleSet(string input) {
            rules2 = new Dictionary<int, Mtx>();
            rules3 = new Dictionary<int, Mtx>();

            foreach (var line in input.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line))) {
                var parts = Regex.Split(line, " => ");
                var left = parts[0];
                var right = parts[1];
                var rules =
                    left.Length == 5 ? rules2 :
                    left.Length == 11 ? rules3 :
                    throw new Exception();
                foreach (var mtx in Variations(Mtx.FromString(left))) {
                    rules[mtx.CodeNumber] = Mtx.FromString(right);
                }
            }
        }

        public Mtx Apply(Mtx mtx) {
            return Mtx.Join((
                from child in mtx.Split()
                select
                    child.Size == 2 ? rules2[child.CodeNumber] :
                    child.Size == 3 ? rules3[child.CodeNumber] :
                    null
            ).ToArray());
        }

        IEnumerable<Mtx> Variations(Mtx mtx) {
            for (int j = 0; j < 2; j++) {
                for (int i = 0; i < 4; i++) {
                    yield return mtx;
                    mtx = mtx.Rotate();
                }
                mtx = mtx.Flip();
            }
        }
    }

    class Mtx {
        private bool[] flags;

        public int Size {
            get;
            private set;
        }

        public int CodeNumber {
            get {
                if (Size != 2 && Size != 3) {
                    throw new ArgumentException();
                }
                var i = 0;
                for (int irow = 0; irow < Size; irow++) {
                    for (int icol = 0; icol < Size; icol++) {
                        if (this[irow, icol]) {
                            i |= (1 << (irow * Size + icol));
                        }
                    }
                }
                return i;
            }
        }

        public Mtx(int size) {
            this.flags = new bool[size * size];
            this.Size = size;
        }

        public static Mtx FromString(string st) {
            st = st.Replace("/", "");
            var size = (int)Math.Sqrt(st.Length);
            var res = new Mtx(size);
            for (int i = 0; i < st.Length; i++) {
                res[i / size, i % size] = st[i] == '#';
            }
            return res;
        }

        public static Mtx Join(Mtx[] rgmtx) {
            var mtxPerRow = (int)Math.Sqrt(rgmtx.Length);
            var res = new Mtx(mtxPerRow * rgmtx[0].Size);
            for (int imtx = 0; imtx < rgmtx.Length; imtx++) {
                var mtx = rgmtx[imtx];
                for (int irow = 0; irow < mtx.Size; irow++) {
                    for (int icol = 0; icol < mtx.Size; icol++) {
                        var irowRes = (imtx / mtxPerRow) * mtx.Size + irow;
                        var icolRes = (imtx % mtxPerRow) * mtx.Size + icol;
                        res[irowRes, icolRes] = mtx[irow, icol];
                    }
                }
            }

            return res;
        }

        public IEnumerable<Mtx> Split() {

            var blockSize =
                Size % 2 == 0 ? 2 :
                Size % 3 == 0 ? 3 :
                throw new Exception();

            for (int irow = 0; irow < Size; irow += blockSize) {
                for (int icol = 0; icol < Size; icol += blockSize) {
                    var mtx = new Mtx(blockSize);
                    for (int drow = 0; drow < blockSize; drow++) {
                        for (int dcol = 0; dcol < blockSize; dcol++) {
                            mtx[drow, dcol] = this[irow + drow, icol + dcol];
                        }
                    }
                    yield return mtx;
                }
            }
        }
        
        public Mtx Flip() {
            var res = new Mtx(this.Size);
            for (int irow = 0; irow < Size; irow++) {
                for (int icol = 0; icol < Size; icol++) {
                    res[irow, Size - icol - 1] = this[irow, icol];
                }
            }
            return res;
        }

        public Mtx Rotate() {
            var res = new Mtx(this.Size);
            for (int i = 0; i < Size; i++) {
                for (int j = 0; j < Size; j++) {
                    res[i, j] = this[j, Size - i - 1];
                }
            }
            return res;
        }

        public int Count() {
            var count = 0;
            for (int irow = 0; irow < Size; irow++) {
                for (int icol = 0; icol < Size; icol++) {
                    if (this[irow, icol]) {
                        count++;
                    }
                }
            }
            return count;
        }

        public override string ToString() {
            var sb = new StringBuilder();
            for (int irow = 0; irow < Size; irow++) {
                for (int icol = 0; icol < Size; icol++) {
                    sb.Append(this[irow, icol] ? "#" : ".");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private bool this[int irow, int icol] {
            get {
                return flags[(Size * irow) + icol];
            }
            set {
                flags[(Size * irow) + icol] = value;
            }
        }
    }
}