using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2016.Day03;

[ProblemName("Squares With Three Sides")]
class Solution : Solver {

    public object PartOne(string input) => ValidTriangles(Parse(input));

    public object PartTwo(string input) {
        var tripplets = new List<IEnumerable<int>>();

        foreach (var lineT in Transpose(Parse(input))) {
            IEnumerable<int> line = lineT;
            while (line.Any()) {
                tripplets.Add(line.Take(3));
                line = line.Skip(3);
            }
        }

        return ValidTriangles(tripplets);
    }

    int[][] Parse(string input) => (
            from line in input.Split('\n')
            select Regex.Matches(line, @"\d+").Select(m => int.Parse(m.Value)).ToArray()
        ).ToArray();

    int ValidTriangles(IEnumerable<IEnumerable<int>> tripplets) =>
       tripplets.Count(tripplet => {
           var nums = tripplet.OrderBy(x => x).ToArray();
           return nums[0] + nums[1] > nums[2];
       });

    int[][] Transpose(int[][] src) {
        var crowDst = src[0].Length;
        var ccolDst = src.Length;
        int[][] dst = new int[crowDst][];
        for (int irowDst = 0; irowDst < crowDst; irowDst++) {
            dst[irowDst] = new int[ccolDst];
            for (int icolDst = 0; icolDst < ccolDst; icolDst++) {
                dst[irowDst][icolDst] = src[icolDst][irowDst];
            }
        }
        return dst;
    }
}
