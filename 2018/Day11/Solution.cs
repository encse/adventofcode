using System.Collections.Generic;

namespace AdventOfCode.Y2018.Day11 {

    [ProblemName("Chronal Charge")]
    class Solution : Solver {

        public object PartOne(string input) {
            var res = Solver(int.Parse(input), 3);
            return $"{res.xMax},{res.yMax}";
        }

        public object PartTwo(string input) {
            var res = Solver(int.Parse(input), 300);
            return $"{res.xMax},{res.yMax},{res.dMax}";
        }

        (int xMax, int yMax, int dMax) Solver(int gridSerialNumber, int D) {
            var gridOriginal = new int[300, 300];
            for (var irow = 0; irow < 300; irow++) {
                for (var icol = 0; icol < 300; icol++) {
                    var x = icol + 1;
                    var y = irow + 1;
                    //  Find the fuel cell's *rack ID*, which is its *X coordinate plus 10*.
                    var rackId = x + 10;
                    // - Begin with a power level of the *rack ID* times the *Y coordinate*.
                    var powerLevel = rackId * y;
                    // - Increase the power level by the value of the *grid serial number* (your puzzle input).
                    powerLevel += gridSerialNumber;
                    // - Set the power level to itself multiplied by the *rack ID*.
                    powerLevel *= rackId;
                    // - Keep only the *hundreds digit* of the power level (so `12*3*45` becomes `3`; numbers with no hundreds digit become `0`).
                    powerLevel = (powerLevel % 1000) / 100;
                    // - *Subtract 5* from the power level.
                    powerLevel -= 5;

                    gridOriginal[irow, icol] = powerLevel;
                }
            }

            var maxTotalPower = int.MinValue;
            var yMax = int.MinValue;
            var xMax = int.MinValue;
            var dMax = int.MinValue;

            var grid = new int[300, 300];
            for (var d = 1; d <= D; d++) {
                for (var irow = 0; irow < 300 - d; irow++) {
                    for (var icol = 0; icol < 300; icol++) {
                        grid[irow, icol] += gridOriginal[irow + d - 1, icol];
                    }
                }

                for (var irow = 0; irow < 300 - d; irow++) {
                    for (var icol = 0; icol < 300 - d; icol++) {
                        var totalPower = 0;

                        for (var i = 0; i < d; i++) {
                            totalPower += grid[irow, icol + i];
                        }

                        if (totalPower > maxTotalPower) {
                            maxTotalPower = totalPower;
                            yMax = irow + 1;
                            xMax = icol + 1;
                            dMax = d;
                        }
                    }
                }
            }
            return (xMax, yMax, dMax);
        }

    }
}