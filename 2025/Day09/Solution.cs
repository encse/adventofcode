namespace AdventOfCode.Y2025.Day09;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

record Segment(Complex a, Complex b);
record Rectangle(Complex top, Complex left, Complex bottom, Complex right);

[ProblemName("Movie Theater")]
class Solution : Solver {

    public object PartOne(string input) {
        var points = Parse(input);
        return (
            from p1 in points
            from p2 in points
            select Area(p1, p2)
        ).Max();
    }

    public object PartTwo(string input) {
        // The input looks like a circle with a slot in the middle of it:
        //
        //        xxx
        //     xxxxxxxxx
        //    xxxxxxxxxAx
        //             xx
        //    xxxxxxxxxBx
        //     xxxxxxxxx
        //        xxx
        //
        // To speed things up we find the corners A and B first. Based on
        // visual observation one of them must be a corner of the final rectangle.
        //
        // Then go over the 'red' points in the upper and lower halves of 
        // the picture and find the greatest rectangle.

        var points = Parse(input);
        var segments = Segments(points);
        var shape = Draw(points);

        var res = 0L;

        var slot = Segments(points)
            .OrderByDescending(s => Area(s.Item1, s.Item2))
            .ThenBy(s=>s.Item1.Imaginary)
            .Select(s => s.Item1.Real > s.Item2.Real ? s.Item1 : s.Item2)
            .Take(2)
            .ToArray();

        var upper = from p in points where p.Imaginary >= slot[0].Imaginary select (p1: slot[0], p2: p);
        var lower = from p in points where p.Imaginary <= slot[1].Imaginary select (p1: slot[1], p2: p);

        var reactanglesByArea = (
            from ps in upper.Concat(lower)
            let p1 = ps.p1
            let p2 = ps.p2
            let top = Math.Min(p1.Imaginary, p2.Imaginary) * Complex.ImaginaryOne
            let bottom = Math.Max(p1.Imaginary, p2.Imaginary) * Complex.ImaginaryOne
            let left = Math.Min(p1.Real, p2.Real)
            let right = Math.Max(p1.Real, p2.Real)
            orderby Area(top+left, bottom + right) descending
            select new Rectangle(top, left, bottom, right)
        ).ToArray();

        foreach (var r in reactanglesByArea) {
            if (Inside(r.top + r.left, shape) && 
                Inside(r.top + r.right, shape) && 
                Inside(r.bottom + r.right, shape) && 
                Inside(r.bottom + r.left, shape) &&
                points.All(p => !InRect(p, r))
            ) {
                res = Area(r.top + r.left, r.bottom + r.right);
                break;
            }
        }

        return res;
    }


    IEnumerable<(Complex, Complex)> Segments(Complex[] points) {
       return points.Zip(points.Prepend(points.Last()));
    }
    Complex[] Parse(string input) => (
        from line in input.Split("\n")
        let parts = line.Split(",").Select(int.Parse).ToArray()
        select parts[0] + Complex.ImaginaryOne * parts[1]
    ).ToArray();

    long Area(Complex p1, Complex p2) {
        return (long)(Math.Abs(p1.Real - p2.Real) +1) *
            (long)(Math.Abs(p1.Imaginary - p2.Imaginary) + 1);
    }
    HashSet<Complex> Draw(Complex[] points) {
        var res = new HashSet<Complex>();
        var segments = Segments(points);
        foreach (var line in segments) {
            var a = line.Item1;
            var b = line.Item2;
            var d = 
                Math.Sign(b.Real - a.Real) + 
                Complex.ImaginaryOne * Math.Sign(b.Imaginary - a.Imaginary);
            for (var p = a; p != b; p += d) {
                res.Add(p);
            }
            res.Add(b);
        }
        return res;
    }

    bool InRect(Complex position, Rectangle r) {
        return 
            r.left.Real < position.Real && position.Real < r.right.Real && 
            r.top.Imaginary < position.Imaginary && position.Imaginary < r.bottom.Imaginary
            ;
    }

    // Check if position is inside the loop using ray casting algorithm
    bool Inside(Complex position, HashSet<Complex> shape) {
        // Imagine a small elf starting from the top half of a cell and moving 
        // to the left jumping over the borders it encounters. It needs to jump 
        // over only 'vertically' oriented pipes leading upwards, since it runs 
        // in the top of the row. Each jump flips the "inside" variable.
        if (shape.Contains(position)) {
            return true;
        }

        var inside = false;
        position -= 1;
        while (position.Real > 0) {
            if (shape.Contains(position) && shape.Contains(position + Complex.ImaginaryOne)) {
                inside = !inside;
            }

            position -= 1;
        }
        return inside;
    }

    
}