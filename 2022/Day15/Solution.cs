using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day15;

[ProblemName("Beacon Exclusion Zone")]
class Solution : Solver {

    public object PartOne(string input) {
        var pairing = Parse(input).ToArray();

        var rects = pairing.Select(pair => pair.ToRect()).ToArray();
        var left = rects.Select(r => r.Left).Min();
        var right = rects.Select(r => r.Right).Max();

        var y = 2000000;
        var res = 0;
        for (var x = left; x <= right; x++) {
            var pos = new Pos(x, y);
            if (pairing.Any(pair => pair.beacon != pos && pair.InRange(pos))) {
                res++;
            }
        }
        return res;
    }

    public object PartTwo(string input) {
        var pairing = Parse(input).ToArray();
        var area = GetUncoveredAreas(pairing, new Rect(0, 0, 4000001, 4000001)).First();
        return area.X * 4000000L + area.Y;
    }


    // Parse the 4 numbers with regex from each line and return the list of pairings
    IEnumerable<Pair> Parse(string input) {
        foreach (var line in input.Split("\n")) {
            var numbers = Regex.Matches(line, "-?[0-9]+").Select(m => int.Parse(m.Value)).ToArray();
            yield return new Pair(
                sensor: new Pos(numbers[0], numbers[1]),
                beacon: new Pos(numbers[2], numbers[3])
            );
        }
    }

    // Do a quadtree style recursive check for uncovered areas with early exit
    // when there is proof that the rectangle is fully covered / uncovered
    IEnumerable<Rect> GetUncoveredAreas(Pair[] pairing, Rect rect) {
        // empty rectangle -> doesn't have uncovered areas 👍
        if (rect.Width == 0 || rect.Height == 0) {
            yield break;
        }

        // if all 4 corners of the rectangle are in range of one of the sensors -> it's covered 👍
        foreach (var pair in pairing) {
            if (rect.Corners.All(corner => pair.InRange(corner))) {
                yield break;
            }
        }

        // if the rectangle is 1x1 -> we just proved that it's uncovered 👍
        if (rect.Width == 1 && rect.Height == 1) {
            yield return rect;
            yield break;
        }

        // otherwise split the rectangle into smaller parts and recurse
        foreach (var rectT in rect.Split()) {
            foreach (var area in GetUncoveredAreas(pairing, rectT)) {
                yield return area;
            }
        }
    }

    // ---------

    record struct Pos(int X, int Y);

    // I don't have a better name for a sensor-bacon pair
    record struct Pair(Pos sensor, Pos beacon) {
        public int Radius = Manhattan(sensor, beacon);

        public bool InRange(Pos pos) => Manhattan(pos, sensor) <= Radius;

        // The smallest rectangle that covers the whole range of the pairing:
        // ............................
        // ..........====#====.........
        // ..........===B##===.........
        // ..........==#####==.........
        // ..........=#######=.........
        // ..........####S####.........
        // ..........=#######=.........
        // ..........==#####==.........
        // ..........===###===.........
        // ..........====#====.........
        // ............................
        public Rect ToRect() =>
             new Rect(sensor.X - Radius, sensor.Y - Radius, 2 * Radius + 1, 2 * Radius + 1);

        static int Manhattan(Pos p1, Pos p2) =>
             Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
    }

    record struct Rect(int X, int Y, int Width, int Height) {
        public int Left => X;
        public int Right => X + Width - 1;
        public int Top => Y;
        public int Bottom => Y + Height - 1;

        public IEnumerable<Pos> Corners {
            get {
                yield return new Pos(Left, Top);
                yield return new Pos(Right, Top);
                yield return new Pos(Right, Bottom);
                yield return new Pos(Left, Bottom);
            }
        }

        // Creates 4 smaller rectangles, might return empty ones with width or height == 0
        public IEnumerable<Rect> Split() {
            var w0 = Width / 2;
            var w1 = Width - w0;
            var h0 = Height / 2;
            var h1 = Height - h0;
            yield return new Rect(Left, Top, w0, h0);
            yield return new Rect(Left + w0, Top, w1, h0);
            yield return new Rect(Left, Top + h0, w0, h1);
            yield return new Rect(Left + w0, Top + h0, w1, h1);
        }
    }
}
