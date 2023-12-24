namespace AdventOfCode.Y2023.Day24;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

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


    // BigInteger FlipTime(Particle3 p1, Particle3 p2) {
    //     if (p1.pos.x == p2.pos.x) {
    //         if (p1.vel.x == p2.vel.x) {
    //             return int.MaxValue;
    //         }
    //         return 1;
    //     }

    //     // p1.x + v1.x*t = p2.x + v2.x*t
    //     if (p1.vel.x == p2.vel.x) {
    //         return int.MaxValue;
    //     }

    //     var t = (p1.pos.x - p2.pos.x) / (p2.vel.x - p1.vel.x);
    //     return Math.Floor(t) + 1;
    // }

    BigInteger Min(BigInteger a, BigInteger b) {
        return a < b ? a : b;
    }
    public object PartTwo(string input) {
        var particles = ParseParticles3(input);

        Dictionary<Particle3, BigInteger> flip = new Dictionary<Particle3, BigInteger>();
        // p1x + v1x*t = p0x + v0x*t
        // p1x - p0x = t * (v0x - v1x)
        // p1x  =  p0x + t * (v0x - v1x)

        for (var v0x = -100000; v0x < 100000; v0x++) {
            // (BigInteger mod, BigInteger a)
            var selector = particles.Where(p1 => IsPrime((v0x - p1.vel.x)));
            var items0 = selector.Select(p1 => (mod: (v0x - p1.vel.x), p1)).ToArray();
            var items = new List<(BigInteger mod, Particle3 p)>();
            foreach(var item in items0) {
                if (items.All(i => i.mod != item.mod)){
                    items.Add(item);
                }
            }
            if (items.Count > 1) {
                var p0X = ChineseRemainderTheorem(items.Select(i => (i.mod, i.p.pos.x)).ToArray());

                if (p0X != 0) {
                    // Console.WriteLine((items.Length, p0X, v0x));
                    var ok = true;
                    // foreach (var p1 in items.Select(i => i.p)) {
                    foreach (var p1 in particles) {
                        if (v0x == p1.vel.x) {
                            Console.Write("x");
                            continue;
                        }
                        var dv = (v0x - p1.vel.x);
                        var p = IsPrime(dv);
                        var dx = p1.pos.x > p0X ? p1.pos.x - p0X : p0X - p1.pos.x;
                        var m =dx % dv;
                        if (m != 0) {
                            ok = false;
                        }
                    }
                    if (ok) {
                        Console.WriteLine(("ok", items.Count, p0X, v0x));
                    }
                    // break;
                }
            }

        }
        return 0;
    }

    public static bool IsPrime(BigInteger number) {
        if (number <= 2) return false;
        if (number % 2 == 0) return false;

        for (int i = 3; i*i <= number; i += 2)
            if (number % i == 0)
                return false;

        return true;
    }

    // Vec3 Intersection(Line p1, Line p2) {
    //     var particle1xy = new Particle(new Vec2(p1.p.x, p1.p.y), new Vec2(p1.v.x, p1.v.y));
    //     var particle2xy = new Particle(new Vec2(p2.p.x, p2.p.y), new Vec2(p2.v.x, p2.v.y));

    //     var particle1xz = new Particle(new Vec2(p1.p.x, p1.p.z), new Vec2(p1.v.x, p1.v.z));
    //     var particle2xz = new Particle(new Vec2(p2.p.x, p2.p.z), new Vec2(p2.v.x, p2.v.z));

    //     var m_xy = MeetPoint(particle1xy, particle2xy);
    //     var m_xz = MeetPoint(particle1xz, particle2xz);

    //     if (m_xy == null || m_xz == null){
    //         return null;
    //     }

    //     if (!AlmostEq(m_xy.x,  m_xz.x)) {
    //         return null;
    //     }

    //     return new Vec3(m_xy.x, m_xy.y, m_xz.y);
    // }

    // bool AlmostEq(BigInteger d1, BigInteger d2) {
    //     return Math.Abs(d1 - d2) < (BigInteger)0.001;
    // }

    static BigInteger SquereRoot(BigInteger square) {
        if (square < 0) return 0;

        BigInteger root = square / 3;
        int i;
        for (i = 0; i < 32; i++)
            root = (root + square / root) / 2;
        return root;
    }

    Particle[] ParseParticles(string input) => (
        from line in input.Split('\n')
        let v = Regex.Matches(line, @"-?\d+").Select(m => BigInteger.Parse(m.Value)).ToArray()
        select new Particle(new Vec2(v[0], v[1]), new Vec2(v[3], v[4]))
    ).ToArray();


    Particle3[] ParseParticles3(string input) => (
        from line in input.Split('\n')
        let v = Regex.Matches(line, @"-?\d+").Select(m => BigInteger.Parse(m.Value)).ToArray()
        select new Particle3(new Vec3(v[2], v[1], v[2]), new Vec3(v[5], v[4], v[5]))
    ).ToArray();

    Line MakeLine(Particle3 p) {
        return new Line(p.vel, p.pos);
    }

    // Plane MakePlane(Line l1, Line l2) {
    //     if (Intersection(l1, l2) == null) {
    //         return null;
    //     }
    //     var n = l1.v.CrossProduct(l2.v);
    //     if (n.Norm2() == 0) {
    //         return null;
    //     }
    //     var d = l1.p.Dot(n);
    //     return new Plane(n, d);
    // }

    // bool Middle(Particle3[] particles, Particle3 p) {
    //     var left = particles.Any(pT => pT.pos.x < p.pos.x && pT.vel.x < 0);
    //     var right = particles.Any(pT => pT.pos.x > p.pos.x && pT.vel.x > 0);
    //     return left && right;
    // }

    // Line CommonLine(Plane p1, Plane p2) {
    //     // find the common point of the planes at z = 0;
    //     var m = new Mat2(
    //         p1.n.x, p1.n.y,
    //         p2.n.x, p2.n.y
    //     );

    //     if (m.Det == 0) {
    //         return null;
    //     }

    //     var pxy = m.Inv() * new Vec2(p1.d, p2.d);
    //     var p = new Vec3(pxy.x, pxy.y, 0);

    //     var v = p1.n.CrossProduct(p2.n);
    //     return new Line(v, p);
    // }

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

record Plane(Vec3 n, BigInteger d);
record Line(Vec3 v, Vec3 p);

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
    public BigInteger Dot(Vec3 b) {
        return x * b.x + y * b.y + z * b.z;
    }
    public Vec3 CrossProduct(Vec3 b) {
        return new Vec3(
            y * b.z - z * b.y,
            z * b.x - x * b.z,
            x * b.y - y * b.x
        );
    }
    public BigInteger Norm2() {
        return Dot(this);
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