using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Y2019.Day16 {

    class Solution : Solver {

        public string GetName() => "Flawed Frequency Transmission";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        string PartOne(string input) {
            var digits = input.Select(ch => int.Parse(ch.ToString())).ToArray();

            var cache = new Dictionary<(int idigit, int depth), int>();
            for (var i = 0; i < 100; i++) {
                digits = Fft(digits);
            }

            return string.Join("", digits.Take(8));
        }

        string PartTwo(string input) {
            /* 
                Let's introduce the following matrix:
                        FFT = [
                            1,  0, -1,  0,  1,  0, -1,  0, ...
                            0,  1,  1,  0,  0, -1, -1,  0, ...
                            0,  0,  1,  1,  1,  0,  0,  0, ...
                            0,  0,  0,  1,  1,  1,  1,  0, ...
                            0,  0,  0,  0,  1,  1,  1,  1, ...
                            0,  0,  0,  0,  0,  1,  1,  1, ...
                            0,  0,  0,  0,  0,  0,  1,  1, ...
                            0,  0,  0,  0,  0,  0,  0,  1, ...
                            ...
                        ]

                A single FFT step of the data stored in vector x is just a matrix multiplication FFT . x
                We get repeated FFT steps with multiplying with the proper power of FFT: FFT^2, FFT^3, ... FFT^100.

                Looking at the FFT matrix, we notice that the bottom right corner is always an upper triangular filled with 1s:
                        A = [
                            1, 1, 1, 1, ...
                            0, 1, 1, 1, ...
                            0, 0, 1, 1, ...
                            0, 0, 0, 1, ...
                            ....
                        ]
                The problem asks for output components that correspond to multiplication with rows in this area.

                Examining A's powers reveal that the the first row can be:
                    the numbers from 1-n, 
                        A^2 = [
                            1, 2, 3, 4, ...
                            0, 1, 2, 3, ...
                            0, 0, 1, 3, ...
                            0, 0, 0, 1, ...
                            ....
                        ]
                    the sum of numbers from 1-n
                        A^3 = [
                            1, 3, 6, 10, ...
                            0, 1, 3, 6, ...
                            0, 0, 1, 3, ...
                            0, 0, 0, 1, ...
                            ....
                        ]
                    the sum of the sum of numbers from 1-n
                        A^4 = [
                            1, 4, 10, 20, ...
                            0, 1,  4, 10, ...
                            0, 0,  1,  4, ...
                            0, 0,  0,  1, ...
                            ....
                        ]
                    etc.
                And we get the second, third... rows with shifting the previous one.

                Using the properties of binomial coefficients we get that the items of the first row of A^k are
                    (A^k)_1_j = choose(j - 1 + k - 1, k - 1)

                    see https://math.stackexchange.com/questions/234304/sum-of-the-sum-of-the-sum-of-the-first-n-natural-numbers

                and we can compute the items from left to right with
                    choose(m + 1, n) = choose(m, n) * (m + 1) / (m + 1 - n)
                
                specifically
                     (A^k)_1_(j + 1) = 
                        choose(j + k - 1, k - 1) = 
                        choose(j - 1 + k - 1, k - 1) * (j + k - 1) / j =
                        (A^k)_1_j * (j + k - 1) / j

                let B = A^100 and so k - 1 = 99.
                    B_1_(j + 1) = B_1_j * (j + 99) / j
                and 
                    B_i_j = B_1_(j - i + 1)

                we need to compute [B]_{1..7} * xs % 10, where xs is the digits of input repeated 10000 times shifted with t
             */

            var xs = input.Select(ch => int.Parse(ch.ToString())).ToArray();
            var res = "";

            var t = int.Parse(input.Substring(0, 7));
            var crow = 8;
            var ccol = input.Length * 10000 - t;

            for (var i = 1; i <= crow; i++) {
                var s = 0;
                var bij = new BigInteger(1);
                for (var j = i; j <= ccol; j++) {
                    var x = xs[(t + j - 1) % input.Length];
                    var bijMod = (int)(bij % 10);
                    s += x * bijMod;
                    bij = bij * (j - i + 100) / (j - i + 1);
                }
                res += (s % 10).ToString();
            }

            return res;
        }

        int[] Fft(int[] digits) {
            var res = new int[digits.Length];
            for (var i = 0; i < digits.Length; i++) {
                res[i] = Math.Abs(digits.Zip(Pattern(i)).Select(p => p.First * p.Second).Sum()) % 10;
            }
            return res;
        }

        IEnumerable<int> Pattern(int digit) => RepeatItems(Loop(new[] { 0, 1, 0, -1 }), digit + 1).Skip(1);

        IEnumerable<int> RepeatItems(IEnumerable<int> items, int count) {
            while (true) {
                foreach (var item in items) {
                    for (var i = 0; i < count; i++) {
                        yield return item;
                    }
                }
            }
        }
        IEnumerable<int> Loop(IEnumerable<int> items) {
            while (true) {
                foreach (var item in items) {
                    yield return item;
                }
            }
        }

    }
}