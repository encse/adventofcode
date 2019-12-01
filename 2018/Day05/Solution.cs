using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2018.Day05 {

    class Solution : Solver {

        public string GetName() => "Alchemical Reduction";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => React(input);

        int PartTwo(string input) => (from ch in "abcdefghijklmnopqrstuvwxyz" select React(input, ch)).Min();

        char ToLower(char ch) => ch <= 'Z' ? (char)(ch - 'A' + 'a') : ch;

        int React(string input, char? skip = null) {
            var stack = new Stack<char>("⊥");
            
            foreach (var ch in input) {
                var top = stack.Peek();
                if (ToLower(ch) == skip) {
                    continue;
                } else if (top != ch && ToLower(ch) == ToLower(top)) {
                    stack.Pop();
                } else {
                    stack.Push(ch);
                }
            }
            return stack.Count() - 1;
        }
    }
}