namespace AdventOfCode.Y2023.Day24;

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;

// WIP, dont look at this :D
// part 1
record Vec2(decimal x, decimal y);
record Mat2(decimal a, decimal b, decimal c, decimal d);
record Particle(Vec2 pos, Vec2 vel);

// part 2
record Vec3(decimal x, decimal y, decimal z);
record Particle3(Vec3 pos, Vec3 vel);

[ProblemName("Never Tell Me The Odds")]
class Solution : Solver {

    public object PartOne(string input) {
        var particles = Proj(ParseParticles(input), v => v.x, v => v.y);

        var begin = 200000000000000;
        var end = 400000000000000;

        return (
            from i in Enumerable.Range(0, particles.Length)
            from j in Enumerable.Range(0, particles.Length)
            where i < j
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

    Particle[] Proj(
        Particle3[] ps,
        Func<Vec3, decimal> dim1,
        Func<Vec3, decimal> dim2
    ) {
        return ps.Select(p =>
            new Particle(
                new Vec2(dim1(p.pos), dim2(p.pos)),
                new Vec2(dim1(p.vel), dim2(p.vel))
            )
        ).ToArray();
    }

    public object PartTwo(string input) {
        var particles = ParseParticles(input);
        var xy = Solve2D(Proj(particles, p => p.x, p => p.y));
        var xz = Solve2D(Proj(particles, p => p.x, p => p.z));
        return Math.Round(xy.p.x + xy.p.y + xz.p.y);
    }

    (Vec2 p, Vec2 v) Solve2D(
        Particle[] particles
    ) {
        var s = 500;
        for (var v1 = -s; v1 < s; v1++) {
            for (var v2 = -s; v2 < s; v2++) {
                var p1 = new Particle(particles[0].pos,
                    new Vec2(particles[0].vel.x - v1, particles[0].vel.y - v2)
                );
                var p2 = new Particle(particles[1].pos,
                 new Vec2(particles[1].vel.x - v1, particles[1].vel.y - v2)
                 );

                var (mp, _, __) = Meet(p1, p2);
                if (mp == null) {
                    continue;
                }
                var ok = true;
                for (var i = 0; i < particles.Length && ok; i++) {
                    var pi = particles[i].pos;
                    var vi = new Vec2(particles[i].vel.x - v1, particles[i].vel.y - v2);

                    if (vi.x != 0 && vi.y != 0) {
                        var tx = (mp.x - pi.x) / vi.x;
                        var ty = (mp.y - pi.y) / vi.y;
                        if (Math.Abs(tx - ty) > (decimal)0.00001) {
                            ok = false;
                        }
                    }
                }
                if (ok) {
                    return (mp, new Vec2(v1, v2));
                }
            }
        }
        throw new Exception();
    }

    // the position where the path of the particles cross and the time(s) when 
    // they are at the meeting point. 
    (Vec2 pos, decimal t1, decimal t2) Meet(Particle part1, Particle part2) {
        // Solving m * x = b provides the meeting point
        Mat2 m = new Mat2(
            part1.vel.y, -part1.vel.x,
            part2.vel.y, -part2.vel.x
        );
        Vec2 b = new Vec2(
            part1.vel.y * part1.pos.x - part1.vel.x * part1.pos.y,
            part2.vel.y * part2.pos.x - part2.vel.x * part2.pos.y
        );

        // I don't have a matrix library at my disposal, but we just need
        // to compute the determinant, the inverse of m, and one matrix 
        // multiplication, so I'll inline it here. I can't make it better.
        var determinant = m.a * m.d - m.b * m.c;
        if (determinant == 0 || part1.vel.x == 0 || part2.vel.x == 0) {
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

        // times come from solving pi.pos + pi.vel * t = pos for x (or y):
        var t1 = (pos.x - part1.pos.x) / part1.vel.x;
        var t2 = (pos.x - part2.pos.x) / part2.vel.x;
        return (pos, t1, t2);
    }


    Particle3[] ParseParticles(string input) => (
        from line in input.Split('\n')
        let v = Regex.Matches(line, @"-?\d+").Select(m => decimal.Parse(m.Value)).ToArray()
        select new Particle3(new Vec3(v[0], v[1], v[2]), new Vec3(v[3], v[4], v[5]))
    ).ToArray();
}

