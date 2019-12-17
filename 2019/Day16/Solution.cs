using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
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
            for(var i =0;i<100;i++){
                digits = Fft(digits);
            }

            return string.Join("", digits.Take(8));
        }

        string PartTwo(string input) {

           var digits = input.Select(ch => int.Parse(ch.ToString())).ToArray();

            var t = int.Parse(input.Substring(0, 7));

            var res = "";
           
            for (var digit = 0; digit < 8; digit++) {

                var c = new BigInteger(1);
                var k = 99;
                var s = 0;
                for (var i = 1; i + t <= input.Length * 10000; i++){
                    var mc = (int)(c % 10);
                    s += digits[(i+t-1) % input.Length] * mc;
                    c = c * (i+k)/(i);
                }
                res += (s%10).ToString();
                t++;
                
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