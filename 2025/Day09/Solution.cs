namespace AdventOfCode.Y2025.Day09;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

record Rectangle(long top, long left, long bottom, long right);

[ProblemName("Movie Theater")]
class Solution : Solver {

    public object PartOne(string input) {
        var points = Parse(input);
        return (
             from r in RectanglesOrderedByArea(points)
             select Area(r)
         ).First();
    }

    public object PartTwo(string input) {
        var points = Parse(input);
        var segments = Boundary(points).ToArray();
        // AabbCollision enables rectangles inside or outside the
        // shape, but the input is set up in a way that big rectangles
        // are all inside, so this loop will find the correct one for actual
        // problems.
        return (
             from r in RectanglesOrderedByArea(points)
             where segments.All(s => !AabbCollision(r, s))
             select Area(r)
         ).First();
    }

    IEnumerable<Rectangle> RectanglesOrderedByArea(Complex[] points) =>
        from p1 in points
        from p2 in points 
        let r = RectangleFromPoints(p1, p2)
        orderby Area(r) descending
        select r;

    IEnumerable<Rectangle> Boundary(Complex[] corners) =>
        from pair in corners.Zip(corners.Prepend(corners.Last()))
        select RectangleFromPoints(pair.First, pair.Second);

    Rectangle RectangleFromPoints(Complex p1, Complex p2) {
        var top = Math.Min(p1.Imaginary, p2.Imaginary);
        var bottom = Math.Max(p1.Imaginary, p2.Imaginary);
        var left = Math.Min(p1.Real, p2.Real);
        var right = Math.Max(p1.Real, p2.Real);
        return new Rectangle((long)top, (long)left, (long)bottom, (long)right);
    }

    Complex[] Parse(string input) => (
        from line in input.Split("\n")
        let parts = line.Split(",").Select(int.Parse).ToArray()
        select parts[0] + Complex.ImaginaryOne * parts[1]
    ).ToArray();

    long Area(Rectangle r) => (r.bottom - r.top + 1) * (r.right - r.left + 1);

    // see https://kishimotostudios.com/articles/aabb_collision/
    bool AabbCollision(Rectangle a, Rectangle b) {
        var aIsToTheLeft = a.right <= b.left;
        var aIsToTheRight = a.left >= b.right;
        var aIsAbove = a.bottom <= b.top;
        var aIsBelow = a.top >= b.bottom;
        return !(aIsToTheRight || aIsToTheLeft || aIsAbove || aIsBelow);
    }
}