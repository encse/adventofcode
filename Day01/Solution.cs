using System;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode2017.Day01 {

    class Solution : Solver {

        public string GetName() => "Inverse Captcha"; 
        
        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => InverseCaptcha(input, 1);

        int PartTwo(string input) => InverseCaptcha(input, input.Length / 2);

        int InverseCaptcha(string input, int skip) {
            return (
                from i in Enumerable.Range(0, input.Length)
                where input[i] == input[(i + skip) % input.Length]
                select int.Parse(input[i].ToString())
            ).Sum();
        }
    }
}
