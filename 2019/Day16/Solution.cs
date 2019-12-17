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
            var res = "";
            for (var i = 0; i < 8; i++) {
                res += Fft2(digits, i, 100, cache);
            }

            return res;
        }

        string PartTwo(string input) {

            Expect(Foo("03036732577212944063491565474664"), "84462026");
            Expect(Foo("02935109699940807407585447034323"), "78725270");
            Expect(Foo("03081770884921959731165446850517"), "53553731");
            return Foo(input);
        }

        void Expect(string a, string b){
            if(a != b){
                throw new Exception();
            }
        }

        string Foo(string input){
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

        int Fft2(int[] input, int idigit, int depth, Dictionary<(int idigit, int depth), int> cache) {
            if (depth == 0) {
                return input[idigit];
            }

            var key = (idigit, depth);
            if (cache.ContainsKey(key)) {
                return cache[key];
            }
           
            var s = 0;
            var skip = idigit;
            for (var i = skip; i < input.Length; i++) {
                var p = Pattern(idigit, i);
                if (p == 0) {
                    i += skip;
                } else {
                   var prevDigit = Fft2(input, i, depth - 1, cache);
                   s += prevDigit * p;
                }
            }
            var res = Math.Abs(s) % 10;
            cache[key] = res;
            return res;
        }


        int[] Fft(int[] digits) {
            var res = new int[digits.Length];
            for (var i = 0; i < digits.Length; i++) {
                res[i] = Math.Abs(digits.Zip(Pattern(i)).Select(p => p.First * p.Second).Sum()) % 10;
            }
            return res;
        }

        int Pattern(int idigit, int i){
            i++;
            idigit ++;

            var m = 4 * idigit;
            var q = i % m;
            //000 111 000 -1-1-1
            //      q

            var v = q / idigit;
            if(v == 1){
                return 1;
            } else if (v == 3) {
                return -1;
            } else {
                return 0;
            }

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