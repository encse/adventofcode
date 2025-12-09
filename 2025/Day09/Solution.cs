namespace AdventOfCode.Y2025.Day09;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.IO;

record Segment(Complex a, Complex b);


// rossz: 1525957356 <5328; 67412> <94891; 50375>
//        1310082477
//        1525812304
        // 1525991432 <94891; 50375> <5328; 67412>  too low
        // 1525991432
   //        1525991432
   //        1310082477
[ProblemName("Movie Theater")]
class Solution : Solver {

    public object PartOne(string input) {
        var points = Parse(input);
        return (
            from p1 in points
            from p2 in points
            select Math.Abs((p1.Real - p2.Real + 1) * (p1.Imaginary - p2.Imaginary + 1))
        ).Max();
    }


    public object PartTwo(string input) {

        // 94891; 50375
        // 94891; 48378

        var points = Parse(input);
        var shape = Draw(points);

        var res = 0L;
        // var p1 = 9 + 5 * Complex.ImaginaryOne;
        // var p2 = 2 + 3 * Complex.ImaginaryOne;

        var reactanglesByArea = (
            // from p1 in points
            // from p1 in new []{94891 + 48378 * Complex.ImaginaryOne, 94891 + 50375 * Complex.ImaginaryOne}
            // from p1 in new []{94891 + 50375 * Complex.ImaginaryOne}
            from p1 in new []{94891 + 48378 * Complex.ImaginaryOne}
            from p2 in points
            orderby Area(p1, p2) descending
            select (p1, p2)
        ).ToArray();

        // <94891; 48378> <3933; 33976>

        var centerY = (points.Select(x=>x.Imaginary).Max() + points.Select(x=>x.Imaginary).Min())/2;

        var i =0;
        foreach (var (p1, p2) in reactanglesByArea) {
            i++;
            if (i%1000 == 0) {
                Console.WriteLine(i);
            }
            if (Math.Sign(p1.Imaginary-centerY) != Math.Sign(p2.Imaginary - centerY)) {
                continue;
            }

            // <9; 5> <2; 3>
            var top = Math.Min(p1.Imaginary, p2.Imaginary);
            var left = Math.Min(p1.Real, p2.Real);
            var bottom = Math.Max(p1.Imaginary, p2.Imaginary);
            var right = Math.Max(p1.Real, p2.Real);

            var corners = new Complex[]{
                left + Complex.ImaginaryOne * top,
                right + Complex.ImaginaryOne * top,
                right + Complex.ImaginaryOne * bottom,
                left + Complex.ImaginaryOne * bottom,
            };

            // var shape2 = Draw(corners);

            // foreach (var p in shape2) {
            //     Console.WriteLine(p + " " + Inside(p, shape));
            // }


            if (corners.All(c => Inside(c, shape))) {

                var qqq = points.Where(p => InRect(p, top, left, bottom, right)).ToArray();
                if (qqq.Any()) {
                    continue;
                }
                var area = Area(p1, p2);
                Console.WriteLine("found " + p1 + " " + p2 + " " + area);
                File.WriteAllText("2025/Day09/coords.txt", 
                $"""
                    {p1.Real},{p1.Imaginary}
                    {p2.Real},{p2.Imaginary}
                """
                );
                res = Area(p1, p2);
                break;
            }
            //  else {
            //      res = Area(p1, p2);
            //      Console.WriteLine(p1 + " " + p2 + " " + res + " x");
            // }
        }

        return res;
        // return (
        //     from p1 in pointsInShape
        //     from p2 in pointsInShape
        //     where points.All(p => !InsideRectangle(p1, p2, p))
        //     select Math.Abs((p1.Real - p2.Real + 1) * (p1.Imaginary - p2.Imaginary + 1))
        // ).Max();
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
        var lines = points.Zip(points.Prepend(points.Last())).ToArray();
        foreach (var line in lines) {
            var a = line.First;
            var b = line.Second;
            var d = Math.Sign(b.Real - a.Real) + Complex.ImaginaryOne * Math.Sign(b.Imaginary - a.Imaginary);
            for (var p = a; p != b; p += d) {
                res.Add(p);
            }
            res.Add(b);
        }
        return res;
    }

    bool InRect(Complex position, double top, double left, double bottom, double right) {
        return 
            left < position.Real && position.Real < right && 
            top < position.Imaginary && position.Imaginary < bottom
            ;
    }
    // Check if position is inside the loop using ray casting algorithm
    bool Inside(Complex position, HashSet<Complex> shape) {
        if (shape.Contains(position)) {
            return true;
        }

        var inside = false;
        position -= 1;
        while (position.Real > 0) {
            if (
                shape.Contains(position) && shape.Contains(position + Complex.ImaginaryOne)
            ) {
                inside = !inside;
            }

            position -= 1;
        }
        return inside;
    }

    
}