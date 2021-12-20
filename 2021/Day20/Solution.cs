using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace AdventOfCode.Y2021.Day20;

[ProblemName("Trench Map")]
class Solution : Solver {

    public object PartOne(string input) => EnhanceN(input, 2).Count(x => x.Value == 1);
    public object PartTwo(string input) => EnhanceN(input, 50).Count(x => x.Value == 1);

    // return the N times enhanced image
    Dictionary<Point, int> EnhanceN(string input, int n) {
        var blocks = input.Split("\n\n");
        var (algo, image) = (blocks[0], GetImage(blocks[1]));

        System.Diagnostics.Debug.Assert(algo[0] == '#'); // the image changes parity in each rounds

        var (minX, minY, maxX, maxY) = (0, 0, image.Keys.MaxBy(p => p.x).x, image.Keys.MaxBy(p => p.y).y);

        for (var i = 0; i < n; i++) {
            var tmp = new Dictionary<Point, int>();

            for (var y = minY - 1; y <= maxY + 1; y++) {
                for (var x = minX - 1; x <= maxX + 1; x++) {

                    var point = new Point(x, y);
                    
                    var index = 0;

                    // it's supposed that neighbours are enumarated in the right order
                    foreach (var neighbour in Neighbours(point)) {

                        // the trick is in the i % 2 part,
                        // for even values of i, the infinite part of the image is all zero
                        // for odd ones, it contains 1-s due to the way the 'algo' is set up.
                        index = index * 2 + image.GetValueOrDefault(neighbour, i % 2);
                    }

                    tmp[point] = algo[index] == '#' ? 1 : 0;
                }
            }

            // update bounds & image
            (minX, minY, maxX, maxY) = (minX - 1, minY - 1, maxX + 1, maxY + 1);
            image = tmp;
        }

        return image;
    }

    // store the points in a dictionary so that we can iterate over them and 
    // to easily deal with points outside the area
    Dictionary<Point, int> GetImage(string input) {
        var map = input.Split("\n");
        return new Dictionary<Point, int>(
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Point, int>(new Point(x, y), map[y][x] == '#' ? 1 : 0)
        );
    }

    IEnumerable<Point> Neighbours(Point pos) =>
        from y in Enumerable.Range(-1, 3)
        from x in Enumerable.Range(-1, 3)
        select new Point(pos.x + x, pos.y + y);
}

record Point(int x, int y);
