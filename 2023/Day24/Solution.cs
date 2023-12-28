namespace AdventOfCode.Y2023.Day24;

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;

record Vec2(decimal x0, decimal x1);
record Vec3(decimal x0, decimal x1, decimal x2);
record Particle2(Vec2 pos, Vec2 vel);
record Particle3(Vec3 pos, Vec3 vel);

[ProblemName("Never Tell Me The Odds")]
class Solution : Solver {

    public object PartOne(string input) {
        var particles = Project(ParseParticles(input), v => (v.x0, v.x1));

        var inRange = (decimal d) =>
             200000000000000 <= d && d <= 400000000000000;

        var future = (Particle2 p, Vec2 pos) =>
            Math.Sign(pos.x0 - p.pos.x0) == Math.Sign(p.vel.x0);

        var res = 0;
        for (var i = 0; i < particles.Length; i++) {
            for (var j = i + 1; j < particles.Length; j++) {
                var pos = Intersection(particles[i], particles[j]);
                if (pos != null && inRange(pos.x0) && inRange(pos.x1) &&
                    future(particles[i], pos) && future(particles[j], pos)
                ) {
                    res++;
                }
            }
        }
        return res;
    }

    public object PartTwo(string input) {
        var particles = ParseParticles(input);
        var stoneXY = Solve2D(Project(particles, vec => (vec.x0, vec.x1)));
        var stoneXZ = Solve2D(Project(particles, vec => (vec.x0, vec.x2)));
        return Math.Round(stoneXY.x0 + stoneXY.x1 + stoneXZ.x1);
    }

    Vec2 Solve2D(Particle2[] particles) {
        // We will bruteforce the velocity of the stone in the two dimensions
        // we are working right now.
        // 
        // We can transform the particles to the reference frame that moves
        // together with the stone, just subtract the stone's velocity 
        // from the particle's velocity.
        // 
        // If we knew the position of the stone as well, we could use a
        // reference frame where the stone rested at (0,0) but we dont know it yet.
        // But in that was the case each particle had to go through (0,0) to get 
        // hit by the stone.
        // 
        // Now the twist is: our stone still has some fixed coordinates in this
        // reference frame we selected, and it's still true that all particles
        // has to go through that point.
        // 
        // So if we have the right velocity, we can find a point that is crossed
        // by every particles. And this has to be the position of the stone.

        var translateV = (Particle2 p, Vec2 vel) =>
            new Particle2(p.pos, new Vec2(p.vel.x0 - vel.x0, p.vel.x1 - vel.x1));

        var s = 500; //arbitrary limits for the brute force that worked for me.
        for (var v1 = -s; v1 < s; v1++) {
            for (var v2 = -s; v2 < s; v2++) {
                var vel = new Vec2(v1, v2);

                // p0 and p1 are linearly independent (for me) => stone != null
                var stone = Intersection(
                    translateV(particles[0], vel),
                    translateV(particles[1], vel)
                );

                if (particles.All(p => Hits(translateV(p, vel), stone))) {
                    return stone;
                }
            }
        }
        throw new Exception();
    }

    // returns if a particle's line hits (goes very close to) pos
    bool Hits(Particle2 p, Vec2 pos) {
        var d = (pos.x0 - p.pos.x0) * p.vel.x1 - (pos.x1 - p.pos.x1) * p.vel.x0;
        return Math.Abs(d) < (decimal)0.0001;
    }

    // returns the position where the path of the particles meet
    Vec2 Intersection(Particle2 p1, Particle2 p2) {
        // we are solving ax=b here with matrix inversion, which
        // would look way better if I had a matrix library at my disposal.
        var (a11, a12, a21, a22) = (
            p1.vel.x1, -p1.vel.x0,
            p2.vel.x1, -p2.vel.x0
        );

        var b = new Vec2(
            p1.vel.x1 * p1.pos.x0 - p1.vel.x0 * p1.pos.x1,
            p2.vel.x1 * p2.pos.x0 - p2.vel.x0 * p2.pos.x1
        );

        var determinant = a11 * a22 - a12 * a21;
        if (determinant == 0) {
            return null; //particles don't meet
        }

        var (i11, i12, i21, i22) = (
            a22 / determinant, -a12 / determinant,
            -a21 / determinant, a11 / determinant
        );

        return new Vec2(
             i11 * b.x0 + i12 * b.x1,
             i21 * b.x0 + i22 * b.x1
         );
    }

    Particle3[] ParseParticles(string input) => [..
        from line in input.Split('\n')
        let v = ParseNum(line)
        select
            new Particle3(
                new Vec3(v[0], v[1], v[2]),
                new Vec3(v[3], v[4], v[5])
            )
    ];

    decimal[] ParseNum(string l) =>
        [.. from m in Regex.Matches(l, @"-?\d+") select decimal.Parse(m.Value)];

    // Project the particle to a 2D plane:
    Particle2[] Project(Particle3[] ps, Func<Vec3, (decimal, decimal)> proj) => [..
        from p in ps select new Particle2(
            new Vec2(proj(p.pos).Item1, proj(p.pos).Item2),
            new Vec2(proj(p.vel).Item1, proj(p.vel).Item2)
        )
    ];
}
