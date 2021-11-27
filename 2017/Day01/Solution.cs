using System.Linq;

namespace AdventOfCode.Y2017.Day01;

[ProblemName("Inverse Captcha")]
class Solution : Solver {

    public object PartOne(string input) => InverseCaptcha(input, 1);

    public object PartTwo(string input) => InverseCaptcha(input, input.Length / 2);

    int InverseCaptcha(string input, int skip) {
        return (
            from i in Enumerable.Range(0, input.Length)
            where input[i] == input[(i + skip) % input.Length]
            select int.Parse(input[i].ToString())
        ).Sum();
    }
}
