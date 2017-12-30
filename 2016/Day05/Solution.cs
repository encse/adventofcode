using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

namespace AdventOfCode.Y2016.Day05 {

    class Solution : Solver {

        public string GetName() => "How About a Nice Game of Chess?";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        string PartOne(string input) {
            var st = "";
            foreach(var hash in Hashes(input)){
                if (hash[0] == 0 && hash[1] == 0 && hash[2] < 16) {
                    st += hash[2].ToString("x");
                    if (st.Length == 8) {
                        break;
                    }
                }
            }
            return st;
        }

        string PartTwo(string input) {
            var chars = Enumerable.Range(0, 8).Select(_ => (char)255).ToArray();
            var found = 0;
            foreach (var hash in Hashes(input)) {
                if (hash[0] == 0 && hash[1] == 0 && hash[2] < 8) {
                    var i = hash[2];
                    if (chars[i] == 255) {
                        chars[i] = hash[3].ToString("x2")[0];
                        found++;
                        if (found == 8) {
                            break;
                        }
                    }

                }
            }
            return string.Join("", chars);
        }

        public IEnumerable<byte[]> Hashes(string input) {
            var md5 = MD5.Create();
            for (var i = 0; ;i++)
               yield return md5.ComputeHash(Encoding.ASCII.GetBytes(input + i));
        }
    }
}