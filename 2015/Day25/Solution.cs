using System.Text.RegularExpressions;

namespace AdventOfCode.Y2015.Day25 {

    [ProblemName("Let It Snow")]
    class Solution : Solver {

        public object PartOne(string input) {
            var m = 20151125L;
            var (irow, icol) = (1, 1);
            var (irowDst, icolDst) = Parse(input);
            while (irow != irowDst || icol != icolDst) {
                irow--;
                icol++;
                if (irow == 0) {
                    irow = icol;
                    icol = 1;
                }
                m = (m * 252533L) % 33554393L;
            }
            return m;
        }

        (int irowDst, int icolDst) Parse(string  input){
            var m = Regex.Match(input, @"To continue, please consult the code grid in the manual.  Enter the code at row (\d+), column (\d+).");
            return (int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
        }
    }
}