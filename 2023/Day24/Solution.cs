namespace AdventOfCode.Y2023.Day24;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

record Range(Decimal begin, Decimal end);
record Particle(Vec2 pos, Vec2 vel);

[ProblemName("Never Tell Me The Odds")]
class Solution : Solver {

    public object PartOne(string input) {
        var particles = ParseParticles(input);
        var testArea = new Range(200000000000000, 400000000000000);
        //var testArea = new Range(7, 27);
        // var mx = particles.Select(p => Intersects(p.pos.x, p.velocity.x, testArea)).ToArray();
        // var my = particles.Select(p => Intersects(p.pos.y, p.velocity.y, testArea)).ToArray();



        var res = 0;
        for (var i = 0; i < particles.Length; i++) {
            for (var j = i + 1; j < particles.Length; j++) {
                var mp = MeetPoint(particles[i], particles[j]);
                if (mp == null) {
                    continue;
                }
                if (!(testArea.begin <= mp.x && mp.x <= testArea.end)) {
                    continue;
                }

                if (!(testArea.begin <= mp.y && mp.y <= testArea.end)) {
                    continue;
                }
                if (Past(particles[i], mp)) {
                    continue;
                }
                if (Past(particles[j], mp)) {
                    continue;
                }
                res++;
            }
        }
        return res;
    }

    bool Past(Particle p, Vec2 v) {
        // p.pos.x + t * p.vel.x = v.x
        if (p.pos.x == v.x) {
            return false;
        }

        if (p.vel.x == 0) {
            return true;
        }

        return (v.x - p.pos.x) / p.vel.x < 0;
    }

    Vec2 MeetPoint(Particle p1, Particle p2) {
        Mat2 m1 = new Mat2(
            p1.vel.y, -p1.vel.x,
            p2.vel.y, -p2.vel.x
        );

        var det = m1.Det;
        if (det == 0) {
            return null;
        }

        Vec2 v = new Vec2(
            p1.vel.y * p1.pos.x - p1.vel.x * p1.pos.y,
            p2.vel.y * p2.pos.x - p2.vel.x * p2.pos.y
        );

        return m1.Inv() * v;
    }

    // (Vec, decimal) Meet(Particle p1, Particle p2) {
    //     Console.WriteLine(p1);
    //     Console.WriteLine(p2);
    //     // p1.x + p1.v.x * t = p2.x + p2.v.x * t 
    //     var dvx = p2.vel.x - p1.vel.x;
    //     var dvy = p2.vel.y - p1.vel.y;
    //     if (dvx == 0) {
    //         if (p1.pos.x != p2.pos.x) {
    //             return (new Vec(0, 0, 0), -1);
    //         }
    //     }

    //     if (dvy == 0) {
    //         if (p1.pos.y != p2.pos.y) {
    //             return (new Vec(0, 0, 0), -1);
    //         }
    //     }

    //     if (dvx == 0 && dvy == 0) {
    //         return (p1.pos, 0);
    //     }

    //     if (dvx != 0) {
    //         var t = (p1.pos.x - p2.pos.x) / dvx;

    //         Console.WriteLine(p1.pos.x + p1.vel.x * t);
    //         Console.WriteLine(p2.pos.x + p2.vel.x * t);
    //         Console.WriteLine(p1.pos.y + p1.vel.y * t);
    //         Console.WriteLine(p2.pos.y + p2.vel.y * t);
    //         var p1tx = p1.pos.x + t * p1.pos.x;
    //         var p1tz = p1.pos.z + t * p1.pos.z;
    //         var p1ty = p1.pos.y + t * p1.pos.y;
    //         var p2ty = p2.pos.y + t * p2.pos.y;
    //         // var t1 = DivUp(p1.pos.x - p2.pos.x, dvx);
    //         if (p1ty - p2ty <= 1) {
    //             return (new Vec(p1tx, p1ty, p1tz), t);
    //         } else {
    //             return (new Vec(0, 0, 0), -1);
    //         }
    //     }

    //     if (dvy != 0) {
    //         var t = (p1.pos.y - p2.pos.y) / dvy;
    //         var p1tx = p1.pos.x + t * p1.pos.x;
    //         var p1tz = p1.pos.z + t * p1.pos.z;
    //         var p1ty = p1.pos.y + t * p1.pos.y;
    //         var p2ty = p2.pos.y + t * p2.pos.y;
    //         // var t1 = DivUp(p1.pos.x - p2.pos.x, dvx);
    //         if (p1ty - p2ty <= 1) {
    //             return (new Vec(p1tx, p1ty, p1tz), t);
    //         } else {
    //             return (new Vec(0, 0, 0), -1);
    //         }
    //     }

    //     return (new Vec(0, 0, 0), -1);
    // }

    // Range Ok(Range r1, Range r2) {
    //     if (r1.end < r2.begin) {
    //         return new Range(0, -1);
    //     }
    //     if (r2.end < r1.begin) {
    //         return new Range(0, -1);
    //     }
    //     var l =  Max(r1.begin, r2.begin);
    //     var r = Min(r1.end, r2.end);

    //     if (r < 0) {
    //         return new Range(0, -1);
    //     }

    //     if (l < 0) {
    //         l = 0;
    //     }



    //     return new Range(l, r);
    // }

    // see https://stackoverflow.com/a/3269471
    bool Intersects(Range r1, Range r2) {
        return r1.begin <= r2.end && r2.begin <= r1.end;
    }

    // BigInteger DivUp(BigInteger a, BigInteger b) {
    //     var r = a / b;
    //     return a % b != 0 ? r + 1 : r;
    // }
    // BigInteger DivDown(BigInteger a, BigInteger b) {
    //     var r = a / b;
    //     return a % b != 0 ? r - 1 : r;
    // }

    // BigInteger Max(BigInteger a, BigInteger b) {
    //     return a > b ? a : b;
    // }


    // BigInteger Min(BigInteger a, BigInteger b) {
    //     return a < b ? a : b;
    // }

    // Range Intersects(BigInteger c, BigInteger v, Range r) {
    //     // p = c + v * t
    //     var start1 = DivUp(r.begin - c, v);
    //     var start2 = DivUp(r.end - c, v);

    //     var end1 = DivDown(r.begin - c, v);
    //     var end2 = DivDown(r.end - c, v);
    //     return new Range(Min(start1, start2), Max(end1, end2));

    // }
    public object PartTwo(string input) {
        return 0;
    }

    Particle[] ParseParticles(string input) => (
        from line in input.Split('\n')
        let v = Regex.Matches(line, @"-?\d+").Select(m => Decimal.Parse(m.Value)).ToArray()
        select new Particle(new Vec2(v[0], v[1]), new Vec2(v[3], v[4]))
    ).ToArray();
}

record Vec2(Decimal x, Decimal y) {
    public static decimal operator *(Vec2 v1, Vec2 v2) {
        return v1.x * v2.x + v1.y * v2.y;
    }
}

record Mat2(decimal a, decimal b, decimal c, decimal d) {
    public decimal Det => a * d - b * c;
    public Mat2 Inv() {
        var det = Det;
        return new Mat2(d / det, -b / det, -c / det, a / det);
    }
    public static Mat2 operator *(Mat2 m1, Mat2 m2) {
        return new Mat2(
            m1.a * m2.a + m1.b * m2.c,
            m1.a * m2.b + m1.b * m2.d,
            m1.b * m2.a + m1.d * m2.c,
            m1.b * m2.b + m1.d * m2.d
        );
    }

    public static Vec2 operator *(Mat2 m1, Vec2 v) {
        return new Vec2(
            m1.a * v.x + m1.b * v.y,
            m1.c * v.x + m1.d * v.y
        );
    }
}