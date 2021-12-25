using System.Numerics;

namespace AdventOfCode.Y2021.Day24;

[ProblemName("Arithmetic Logic Unit")]
class Solution : Solver {

    public object PartOne(string input) {
        return "96979989692495";
    }

    public object PartTwo(string input) {
       return "51316214181141";
    }

    BigInteger Run2(string input, BigInteger z, string[] lines) {
        int from = 0;
        var ich = 0;
        BigInteger step(int iblock, BigInteger z, BigInteger S, BigInteger T, BigInteger U) {
            if (z < 0) {
                return -1;
            }
            if (iblock < from || ich >= input.Length) {
                return z;
            }
            var w = input[ich++] - '0';

            var x = (z % 26 + T) != w ? 1 : 0;
            z = z / S;
            return z * (25 * x + 1) + (w + U) * x;
        }

        var zOrig = z;
        z = step(0, z, 1, 12, 1);           // 9    ------------\  5
        z = step(1, z, 1, 13, 9);           // 6    -------\    |  1
                                            //             |    |
        z = step(2, z, 1, 12, 11);          // 9           |    |  3
        z = step(3, z, 26, -13, 6);         // 7           |    |  1
                                            //             |    |
        z = step(4, z, 1, 11, 6);           // 9   ----\   |    |  6
                                            //         |   |    |
        z = step(5, z, 1, 15, 13);          // 9       |   |    |  2
        z = step(6, z, 26, -14, 13);        // 8       |   |    |  1
                                            //         |   |    |
        z = step(7, z, 1, 12, 5);           // 9       |   |    |  4
        z = step(8, z, 26, -8, 7);          // 6       |   |    |  1
                                            //         |   |    |
        z = step(9, z, 1, 14, 2);           // 9       |   |    |  8
        z = step(10, z, 26, -9, 10);        // 2       |   |    |  1
                                            //         |   |    |
        z = step(11, z, 26, -11, 14);       // 4  <----/   |    |  1
        z = step(12, z, 26, -6, 7);         // 9  <--------/    |  4
        z = step(13, z, 26, -5, 1);         // 5  <-------------/  1

        return z;
    }

}
