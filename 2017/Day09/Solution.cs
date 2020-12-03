using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2017.Day09 {

    [ProblemName("Stream Processing")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }
        
        int PartOne(string input) => BlockScores(input).Sum();
        int PartTwo(string input) => Classify(input).Where((x) => x.garbage).Count();

        IEnumerable<int> BlockScores(string input) {
            var score = 0;
            foreach (var ch in Classify(input).Where((x) => !x.garbage).Select(x => x.ch)) {
                if (ch == '}') {
                    score--;
                } else if (ch == '{') {
                    score++;
                    yield return score;
                }
            }
        }

        IEnumerable<(char ch, bool garbage)> Classify(string input) {
            var skip = false;
            var garbage = false;
            foreach (var ch in input) {
                if (garbage) {
                    if (skip) {
                        skip = false;
                    } else {
                        if (ch == '>') {
                            garbage = false;
                        } else if (ch == '!') {
                            skip = true;
                        } else {
                            yield return (ch, garbage);
                        }
                    }
                } else {
                    if (ch == '<') {
                        garbage = true;
                    } else {
                        yield return (ch, garbage);
                    }
                }
            }
        }
    }
}
