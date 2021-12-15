using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Y2021.Day11;

[ProblemName("Dumbo Octopus")]
class Solution : Solver {

    public object PartOne(string input) => Simulate(input).Take(100).Sum();
    public object PartTwo(string input) => Simulate(input).TakeWhile(flash => flash != 100).Count() + 1;

    // run the simulation in an endless loop, yield flash counts in each step
    IEnumerable<int> Simulate(string input) {

        var map = GetMap(input);

        while (true) {

            var queue = new Queue<Pos>();
            var flashed = new HashSet<Pos>();

            // increase the energy level of each octopus:
            foreach (var key in map.Keys) {
                map[key]++;
                if (map[key] == 10) {
                    queue.Enqueue(key);
                }
            }

            // those that reach level 10 should flash, use a queue so that flashing can trigger others
            while (queue.Any()) {
                var pos = queue.Dequeue();
                flashed.Add(pos);
                foreach (var n in Neighbours(pos).Where(x => map.ContainsKey(x))) {
                    map[n]++;
                    if (map[n] == 10) {
                        queue.Enqueue(n);
                    }
                }
            }

            // reset energy level of flashed octopuses
            foreach (var pos in flashed) {
                map[pos] = 0;
            }

            yield return flashed.Count;
        }
    }

    // store the points in a dictionary so that we can iterate over them and 
    // to easily deal with points outside the area using ContainsKey
    Dictionary<Pos, int> GetMap(string input) {
        var map = input.Split("\n");
        return new Dictionary<Pos, int>(
            from y in Enumerable.Range(0, map.Length)
            from x in Enumerable.Range(0, map[0].Length)
            select new KeyValuePair<Pos, int>(new Pos(x, y), map[y][x] - '0')
        );
    }

    IEnumerable<Pos> Neighbours(Pos pos) =>
        from dx in new int[] { -1, 0, 1 }
        from dy in new int[] { -1, 0, 1 }
        where dx != 0 || dy != 0
        select new Pos(pos.x + dx, pos.y + dy);

}
record Pos(int x, int y);