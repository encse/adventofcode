using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace AdventOfCode.Y2018.Day13 {

    class Solution : Solver {

        public string GetName() => "Mine Cart Madness";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        string PartOne(string input) {
            var (mat, carts) = Parse(input);
            while (true) {
                var loc = Step(mat, carts);
                if (loc != null) {
                    return $"{loc.Value.icol},{loc.Value.irow}";
                }
            }
        }

        (int irow, int icol)? Step(string[] mat, List<Cart> carts) {
            carts.Sort((cartA, cartB) => cartA.pos.CompareTo(cartB.pos));
            (int irow, int icol)? res = null;
            foreach (var cart in carts.ToArray()) {
                cart.pos = (irow: cart.pos.irow + cart.drow, icol: cart.pos.icol + cart.dcol);
                
                foreach (var cart2 in carts.ToArray()) {
                    if (cart != cart2 && cart.pos.irow == cart2.pos.irow && cart.pos.icol == cart2.pos.icol) {
                        if(res == null){
                            res = cart.pos;
                        }
                        carts.Remove(cart);
                        carts.Remove(cart2);
                    }
                }
                switch (mat[cart.pos.irow][cart.pos.icol]) {
                    case '\\':
                        if (cart.dcol == 1 || cart.dcol == -1) {
                            cart.Rotate(Dir.Right);
                        } else if (cart.drow == -1 || cart.drow == 1) {
                            cart.Rotate(Dir.Left);
                        } else {
                            throw new Exception();
                        }
                        break;
                    case '/':
                        if (cart.dcol == 1 || cart.dcol == -1) {
                            cart.Rotate(Dir.Left);
                        } else if (cart.drow == 1 || cart.drow == -1) {
                            cart.Rotate(Dir.Right);
                        }
                        break;
                    case '+':
                        cart.Turn();
                        break;
                }
            }
            return res;
        }
        string PartTwo(string input) {
            var (mat, carts) = Parse(input);
            while (carts.Count > 1) {
                Step(mat, carts);
            }
            return $"{carts[0].pos.icol},{carts[0].pos.irow}"; 
        }

        (string[] mat, List<Cart> carts) Parse(string input){
            var mat = input.Split("\n");
            var crow = mat.Length;
            var ccol = mat[0].Length;

            var carts = new List<Cart>();
            for (var irow = 0; irow < crow; irow++) {
                for (var icol = 0; icol < ccol; icol++) {
                    var ch = mat[irow][icol];
                    switch (ch) {
                        case '^':
                            carts.Add(new Cart { pos = (irow: irow, icol: icol), dcol = 0, drow = -1 });
                            break;
                        case 'v':
                            carts.Add(new Cart { pos = (irow: irow, icol: icol), dcol = 0, drow = 1 });
                            break;
                        case '<':
                            carts.Add(new Cart { pos = (irow: irow, icol: icol), dcol = -1, drow = 0 });
                            break;
                        case '>':
                            carts.Add(new Cart { pos = (irow: irow, icol: icol), dcol = 1, drow = 0 });
                            break;
                    }
                }
            }
            return (mat, carts);
        }
    }

    enum Dir { Left, Forward, Right }
    class Cart {
        public (int irow, int icol) pos;
        public int drow;
        public int dcol;
        private Dir nextTurn = Dir.Left;

        public void Rotate(Dir dir) {
            switch (dir) {
                case Dir.Left:
                    (drow, dcol) = (-dcol, drow);
                    break;
                case Dir.Right:
                    (drow, dcol) = (dcol, -drow);
                    break;
                default:
                    break;
            }
        }

        public void Turn() {
            Rotate(nextTurn);
            nextTurn = (Dir)(((int)nextTurn + 1) % 3);
        }
    }
}