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

        var inRange = (decimal d) => 2e14m <= d && d <= 4e14m;

        var inFuture = (Particle2 p, Vec2 pos) => 
            Math.Sign(pos.x0 - p.pos.x0) == Math.Sign(p.vel.x0);

        var res = 0;
        for (var i = 0; i < particles.Length; i++) {
            for (var j = i + 1; j < particles.Length; j++) {
                var pos = Intersection(particles[i], particles[j]);
                if (pos != null && 
                    inRange(pos.x0) && 
                    inRange(pos.x1) &&
                    inFuture(particles[i], pos) && 
                    inFuture(particles[j], pos)
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
        // We try to guess the speed of our stone (a for loop), then supposing 
        // that it is the right velocity we create a new reference frame that 
        // moves with that speed. The stone doesn't move in this frame, it has 
        // some fixed unknown coordinates. Now transform each particle into 
        // this reference frame as well. Since the stone is not moving, if we 
        // properly guessed the speed, we find that each particle meets at the 
        // same point. This must be the stone's location.

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

    // returns if p hits (goes very close to) pos
    bool Hits(Particle2 p, Vec2 pos) {
        var d = (pos.x0 - p.pos.x0) * p.vel.x1 - (pos.x1 - p.pos.x1) * p.vel.x0;
        return Math.Abs(d) < (decimal)0.0001;
    }

    // returns the pos hit by both p1 and p2
    Vec2 Intersection(Particle2 p1, Particle2 p2) {
        // this would look way better if I had a matrix library at my disposal.
        var determinant = p1.vel.x0 * p2.vel.x1 - p1.vel.x1 * p2.vel.x0;
        if (determinant == 0) {
            return null; //particles don't meet
        }
        
        var b0 = p1.vel.x0 * p1.pos.x1 - p1.vel.x1 * p1.pos.x0;
        var b1 = p2.vel.x0 * p2.pos.x1 - p2.vel.x1 * p2.pos.x0;
       
        return new (
             (p2.vel.x0 * b0 - p1.vel.x0 * b1) / determinant,
             (p2.vel.x1 * b0 - p1.vel.x1 * b1) / determinant
         );
    }

    Particle3[] ParseParticles(string input) => [..
        from line in input.Split('\n')
        let v = ParseNum(line)
        select new Particle3(new (v[0], v[1], v[2]), new (v[3], v[4], v[5]))
    ];

    decimal[] ParseNum(string l) => [.. 
        from m in Regex.Matches(l, @"-?\d+") select decimal.Parse(m.Value)
    ];

    // Project the particle to a 2D plane:
    Particle2[] Project(Particle3[] ps, Func<Vec3, (decimal, decimal)> proj) => [..
        from p in ps select new Particle2(
            new Vec2(proj(p.pos).Item1, proj(p.pos).Item2),
            new Vec2(proj(p.vel).Item1, proj(p.vel).Item2)
        )
    ];
}
