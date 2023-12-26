namespace AdventOfCode.Y2023.Day24;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Numerics;
using System.Data;

// WIP, dont look at this :D

record Vec2(decimal x, decimal y) { }
record Mat2(decimal a, decimal b, decimal c, decimal d) {}
record Vec3(BigInteger x, BigInteger y, BigInteger z);
record Particle(Vec2 pos, Vec2 vel);
record Particle3(Vec3 pos, Vec3 vel);

[ProblemName("Never Tell Me The Odds")]
class Solution : Solver {

    public object PartOne(string input) {
        var particles = ParseParticles(input);
        var begin = 200000000000000;
        var end = 400000000000000;

        return (
            from i in Enumerable.Range(0, particles.Length)
            from j in Enumerable.Range(0, particles.Length)
            where i< j
            let intersection = Meet(particles[i], particles[j])
            where (
                intersection.pos != null &&
                begin <= intersection.pos.x && intersection.pos.x <= end &&
                begin <= intersection.pos.y && intersection.pos.y <= end &&
                intersection.t1 >= 0 && intersection.t2 >= 0
            )
            select 1
        ).Sum(); 
    }

    public object PartTwo(string input) {
        var particles = ParseParticles3(input);
        return Solve(v => v.x, particles) + Solve(v => v.y, particles) + Solve(v => v.z, particles);
    }

    // the position where the path of the particles cross and the time(s) when 
    // they are at the meeting point. 
    (Vec2 pos, decimal t1, decimal t2) Meet(Particle p1, Particle p2) {
        // Solving m * x = b provides the meeting point
        Mat2 m = new Mat2(
            p1.vel.y, -p1.vel.x, 
            p2.vel.y, -p2.vel.x
        );
        Vec2 b = new Vec2(
            p1.vel.y * p1.pos.x - p1.vel.x * p1.pos.y,
            p2.vel.y * p2.pos.x - p2.vel.x * p2.pos.y
        );

        // I don't have a matrix library at my disposal, but we just need
        // to compute the determinant, the inverse of m, and one matrix 
        // multiplication, so I'll inline it here. It's as ugly as it is.
        var determinant = m.a * m.d - m.b * m.c;
        if (determinant == 0) {
            return (null, -1, -1); //particles don't meet
        }

        var inverse = new Mat2(
            m.d / determinant, -m.b / determinant, 
            -m.c / determinant, m.a / determinant
        );

        var pos = new Vec2(
            inverse.a * b.x + inverse.b * b.y,
            inverse.c * b.x + inverse.d * b.y
        );

        // times come from solving a linear equation for one coordinate (x):
        var t1 = (pos.x - p1.pos.x) / p1.vel.x;
        var t2 = (pos.x - p2.pos.x) / p2.vel.x;
        return (pos, t1, t2);
    }

    BigInteger Solve(Func<Vec3, BigInteger> dim, Particle3[] particles) {
        for (var v0 = -10000; v0 < 10000; v0++) {
            var items = new List<(BigInteger dv, BigInteger x)>();
            foreach (var p in particles) {
                var dv = v0 - dim(p.vel);
                if (IsPrime(dv) && items.All(i => i.dv != dv)) {
                    items.Add((dv: dv, x: dim(p.pos)));
                }
            }
            if (items.Count > 1) {
                var p0 = ChineseRemainderTheorem(items.ToArray());
                var ok = true;
                foreach (var p in particles) {
                    var dv = v0 - dim(p.vel);
                    var dx = dim(p.pos) > p0 ? dim(p.pos) - p0 : p0 - dim(p.pos);
                    if (dv == 0) {
                        if (dx != 0) {
                            ok = false;
                        }
                    } else {
                        if (dx % dv != 0) {
                            ok = false;
                        }
                    }
                }
                if (ok) {
                    return p0;
                }
            }
        }
        throw new Exception();

    }

    public static bool IsPrime(BigInteger number) {
        if (number <= 2) return false;
        if (number % 2 == 0) return false;

        for (int i = 3; i * i <= number; i += 2)
            if (number % i == 0)
                return false;

        return true;
    }

    Particle[] ParseParticles(string input) => (
        from line in input.Split('\n')
        let v = Regex.Matches(line, @"-?\d+").Select(m => decimal.Parse(m.Value)).ToArray()
        select new Particle(new Vec2(v[0], v[1]), new Vec2(v[3], v[4]))
    ).ToArray();


    Particle3[] ParseParticles3(string input) => (
        from line in input.Split('\n')
        let v = Regex.Matches(line, @"-?\d+").Select(m => BigInteger.Parse(m.Value)).ToArray()
        select new Particle3(new Vec3(v[0], v[1], v[2]), new Vec3(v[3], v[4], v[5]))
    ).ToArray();

    // https://rosettacode.org/wiki/Chinese_remainder_theorem#C.23
    BigInteger ChineseRemainderTheorem((BigInteger mod, BigInteger a)[] items) {
        var prod = items.Aggregate(BigInteger.One, (acc, item) => acc * item.mod);
        var sum = items.Select((item, i) => {
            var p = prod / item.mod;
            return item.a * ModInv(p, item.mod) * p;
        });

        var s = BigInteger.Zero;
        foreach (var item in sum) {
            s += item;
        }

        return s % prod;
    }
    BigInteger ModInv(BigInteger a, BigInteger m) => BigInteger.ModPow(a, m - 2, m);
}

