namespace AdventOfCode.Y2023.Day24;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Numerics;
using System.Data;

record Range(BigInteger begin, BigInteger end);
record Particle(Vec2 pos, Vec2 vel);
record Particle3(Vec3 pos, Vec3 vel);

// WIP, dont look at this :D

[ProblemName("Never Tell Me The Odds")]
class Solution : Solver {

    public object PartOne(string input) {
        var particles = ParseParticles(input);
        var testArea = new Range(200000000000000, 400000000000000);
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

        return (decimal)(v.x - p.pos.x) / (decimal)p.vel.x < 0;
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

    public object PartTwo(string input) {
        var particles = ParseParticles3(input);
        return Solve(v => v.x, particles) + Solve(v => v.y, particles) + Solve(v => v.z, particles);
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
        let v = Regex.Matches(line, @"-?\d+").Select(m => BigInteger.Parse(m.Value)).ToArray()
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

record Vec2(BigInteger x, BigInteger y) {
    public static BigInteger operator *(Vec2 v1, Vec2 v2) {
        return v1.x * v2.x + v1.y * v2.y;
    }
}

record Vec3(BigInteger x, BigInteger y, BigInteger z) {
    public static Vec3 operator +(Vec3 v1, Vec3 v2) {
        return new Vec3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }
    public static Vec3 operator -(Vec3 v1, Vec3 v2) {
        return new Vec3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }
    public static Vec3 operator *(BigInteger d, Vec3 v1) {
        return new Vec3(d * v1.x, d * v1.y, d * v1.z);
    }
}

record Mat2(BigInteger a, BigInteger b, BigInteger c, BigInteger d) {
    public BigInteger Det => a * d - b * c;
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