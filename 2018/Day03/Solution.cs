using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2018.Day03 {

    [ProblemName("No Matter How You Slice It")]
    class Solution : Solver {

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Decorate(input).overlapArea;

        int PartTwo(string input) => Decorate(input).intactId;

        (int overlapArea, int intactId) Decorate(string input) {
            // #1 @ 55,885: 22x10
            var rx = new Regex(@"(?<id>\d+) @ (?<x>\d+),(?<y>\d+): (?<width>\d+)x(?<height>\d+)");
            var mtx = new int[1000, 1000];

            var overlapArea = 0;

            var ids = new HashSet<int>();
            foreach (var line in input.Split("\n")) {
                var parts = rx.Match(line);
                var id = int.Parse(parts.Groups["id"].Value);
                var x = int.Parse(parts.Groups["x"].Value);
                var y = int.Parse(parts.Groups["y"].Value);
                var width = int.Parse(parts.Groups["width"].Value);
                var height = int.Parse(parts.Groups["height"].Value);

                ids.Add(id);

                for (var i = 0; i < width; i++) {
                    for (var j = 0; j < height; j++) {
                        if (mtx[x + i, y + j] == 0) {
                            mtx[x + i, y + j] = id;
                        } else if (mtx[x + i, y + j] == -1) {
                            ids.Remove(id);
                        } else {
                            ids.Remove(mtx[x + i, y + j]);
                            ids.Remove(id);
                            overlapArea++;

                            mtx[x + i, y + j] = -1;
                        }
                    }
                }
            }

            return (overlapArea, ids.Single());
        }
    }
}