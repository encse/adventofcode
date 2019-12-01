using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2015.Day14 {

    class Solution : Solver {

        public string GetName() => "Reindeer Olympics";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => Race(Parse(input)).Skip(2502).First().Max();
        int PartTwo(string input) => Race2(Parse(input)).Skip(2502).First().Max();

        IEnumerable<int>[] Parse(string input) => input.Split('\n').Select(Reindeer).ToArray();

        IEnumerable<int[]> Race(IEnumerable<int>[] reindeers) {
            var res = new int[reindeers.Length];
            var enumarators = reindeers.Select(r => r.GetEnumerator()).ToArray();
            while (true) {
                yield return (from en in enumarators
                              let _ = en.MoveNext()
                              select en.Current).ToArray();
            }
        }

        IEnumerable<int[]> Race2(IEnumerable<int>[] reindeers) {
            var points = new int[reindeers.Length];
            foreach (var step in Race(reindeers)) {
                var m = step.Max();
                for (var i = 0; i < step.Length; i++) {
                    if (step[i] == m) {
                        points[i]++;
                    }
                }
                yield return points;
            }
        }
        
        IEnumerable<int> Reindeer(string line) {
            var m = Regex.Match(line, @"(.*) can fly (.*) km/s for (.*) seconds, but then must rest for (.*) seconds.");
            var speed = int.Parse(m.Groups[2].Value);
            var flightTime = int.Parse(m.Groups[3].Value);
            var restTime = int.Parse(m.Groups[4].Value);
            var t = 0;
            var dist = 0;
            var flying = true;
            while (true) {
                if (flying) {
                    dist += speed;
                }
                t++;
                if ((flying && t == flightTime) || (!flying && t == restTime)) {
                    t = 0;
                    flying = !flying;
                }
                yield return dist;
            }
        }
    }
}