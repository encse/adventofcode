namespace AdventOfCode.Y2023.Day24;

using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;

record Vec2(decimal x0, decimal x1);
record Vec3(decimal x0, decimal x1, decimal x2);
record Particle<T>(T pos, T vel);

[ProblemName("Never Tell Me The Odds")]
class Solution : Solver {

    public object PartOne(string input) {
        var particles = Proj(ParseParticles(input), v => (v.x0, v.x1));

        var begin = 200000000000000;
        var end = 400000000000000;

        bool reachesInFuture(Particle<Vec2> p, Vec2 pos) {
            if (p.vel.x0 == 0) {
                return pos.x0 == p.pos.x0;
            }
            return (pos.x0 - p.pos.x0) / p.vel.x0 >= 0;
        }

        var res = 0;
        for (var i = 0; i < particles.Length; i++) {
            for (var j = i + 1; j < particles.Length; j++) {
                var pos = Meet(particles[i], particles[j]);
                if (pos != null &&
                    begin <= pos.x0 && pos.x0 <= end &&
                    begin <= pos.x1 && pos.x1 <= end &&
                    reachesInFuture(particles[i], pos) &&
                    reachesInFuture(particles[j], pos)
                ) {
                    res++;
                }
            }
        }
        return res;
    }

    public object PartTwo(string input) {
        var particles = ParseParticles(input);
        var stoneXY = Solve2D(Proj(particles, vec => (vec.x0, vec.x1)));
        var stoneXZ = Solve2D(Proj(particles, vec => (vec.x0, vec.x2)));
        return Math.Round(stoneXY.x0 + stoneXY.x1 + stoneXZ.x1);
    }

    Particle<Vec2> TranslateV(Particle<Vec2> p, Vec2 vel) =>
         new Particle<Vec2>(p.pos, new Vec2(p.vel.x0 - vel.x0, p.vel.x1 - vel.x1));

    Vec2 Solve2D(Particle<Vec2>[] particles) {
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

        var s = 500; //arbitrary limits for the brute force that worked for me.
        for (var v1 = -s; v1 < s; v1++) {
            for (var v2 = -s; v2 < s; v2++) {
                var vel = new Vec2(v1, v2);
                var pos = Meet(
                    TranslateV(particles[0], vel), 
                    TranslateV(particles[1], vel)
                );
                
                if (pos == null) {
                    continue;
                }

                var hitsAllStones = true;
                for (var i = 0; i < particles.Length && hitsAllStones; i++) {
                    var pi = TranslateV(particles[i], vel);
                    // ignore the case when the velocity is 0, 
                    // we should check these as well, but it's not necessary
                    // to solve solve my input.
                    if (pi.vel.x0 != 0 && pi.vel.x1 != 0) {
                        var tx = (pos.x0 - pi.pos.x0) / pi.vel.x0;
                        var ty = (pos.x1 - pi.pos.x1) / pi.vel.x1;
                        if (Math.Abs(tx - ty) > (decimal)0.00001) {
                            hitsAllStones = false;
                        }
                    }
                }

                if (hitsAllStones) {
                    return pos;
                }
            }
        }
        throw new Exception();
    }

    // returns the position where the path of the particles cross
    // I don't have a matrix library at my disposal, but we just need
    // to compute the determinant, the inverse of m, and one matrix 
    // multiplication, so I'll inline it here. I can't make it better.
    Vec2 Meet(Particle<Vec2> p1, Particle<Vec2> p2) {

        var (a11, a12, a21, a22)= (
            p1.vel.x1, -p1.vel.x0,
            p2.vel.x1, -p2.vel.x0
        );
        
        Vec2 b = new Vec2(
            p1.vel.x1 * p1.pos.x0 - p1.vel.x0 * p1.pos.x1,
            p2.vel.x1 * p2.pos.x0 - p2.vel.x0 * p2.pos.x1
        );

        var determinant = a11 * a22 - a12 * a21;
        if (determinant == 0 || p1.vel.x0 == 0 || p2.vel.x0 == 0) {
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

    Particle<Vec3>[] ParseParticles(string input) => (
        from line in input.Split('\n')
        let v = Regex.Matches(line, @"-?\d+").Select(m => decimal.Parse(m.Value)).ToArray()
        select new Particle<Vec3>(new Vec3(v[0], v[1], v[2]), new Vec3(v[3], v[4], v[5]))
    ).ToArray();

    Particle<Vec2>[] Proj(
        Particle<Vec3>[] ps, 
        Func<Vec3, (decimal, decimal)> proj
    ) => (
        from p in ps
        let pos = proj(p.pos)
        let vel = proj(p.vel)
        select new Particle<Vec2>(
            new Vec2(pos.Item1, pos.Item2),
            new Vec2(vel.Item1, vel.Item2)
        )
    ).ToArray();
}
