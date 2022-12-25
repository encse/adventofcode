using System.Linq;

namespace AdventOfCode.Y2022.Day25;

[ProblemName("Full of Hot Air")]
class Solution : Solver {

    public object PartOne(string input) =>
        LongToSnafu(
            input
                .Split("\n")
                .Select(SnafuToLong)
                .Sum()
        );
    
    long SnafuToLong(string snafu) {
        // This is just string to number conversion in base 5
        // with the two special digits that worth -2 and -1.
        long res = 0L;
        foreach (var digit in snafu) {
            res = res * 5;
            switch (digit) {
                case '=': res += -2; break;
                case '-': res += -1; break;
                case '0': res += 0; break;
                case '1': res += 1; break;
                case '2': res += 2; break;
            }
        }
        return res;
    }

    string LongToSnafu(long d) {
        // Almost standard base conversion, but when dealing with digits 3 and 4
        // we need to increment the higher decimal place and subtract 2 or 1.
        var res = "";
        while (d > 0) {
            var digit = d % 5;
            d /= 5;
            switch (digit) {
                case 0: res = '0' + res; break;
                case 1: res = '1' + res; break;
                case 2: res = '2' + res; break;
                case 3: res = '=' + res; d++; break; 
                case 4: res = '-' + res; d++; break;
            }
        }
        return res;
    }
}
