namespace AdventOfCode.Y2015.Day20;

[ProblemName("Infinite Elves and Infinite Houses")]
class Solution : Solver {

    public object PartOne(string input) {
        var l = int.Parse(input);
        return PresentsByHouse(1000000, 10, l);
    }

    public object PartTwo(string input) {
        var l = int.Parse(input);
        return PresentsByHouse(50, 11, l);
    }

    int PresentsByHouse(int steps, int mul, int l) {
        var presents = new int[1000000];
        for (var i = 1; i < presents.Length; i++) {
            var j = i;
            var step = 0;
            while (j < presents.Length && step < steps) {
                presents[j] += mul * i;
                j += i;
                step++;
            }
        }

        for (var i = 0; i < presents.Length; i++) {
            if (presents[i] >= l) {
                return i;
            }
        }
        return -1;

    }
}
