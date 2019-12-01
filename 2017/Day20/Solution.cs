using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2017.Day20 {

    class Solution : Solver {

        public string GetName() => "Particle Swarm";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) {
            var particles = Parse(input);
            return (
                from particle in particles
                orderby particle.acc.Len(), particle.vel.Len(), particle.pos.Len()
                select particle
            ).First().i;
        }

        int PartTwo(string input) {
            var particles = Parse(input);
            var collisionTimes = (
                from p1 in particles
                from p2 in particles
                where p1.i != p2.i
                from collisionTime in p1.CollisionTime(p2)
                select collisionTime
            ).ToArray();
            var T = collisionTimes.Max();

            var t = 0;
            while (t <= T) {
                var particlesByPos = (from particle in particles orderby particle.pos.x, particle.pos.y, particle.pos.z select particle).ToArray();
                
                var particlePrev = particlesByPos[0];

                for (int i = 1; i < particlesByPos.Length; i++) {
                    var particle = particlesByPos[i];
                    if (particlePrev.pos.x == particle.pos.x && particlePrev.pos.y == particle.pos.y && particlePrev.pos.z == particle.pos.z) {
                        particlePrev.destroyed = true;
                        particle.destroyed = true;
                    }
                    particlePrev = particle;
                }

                if (particles.Any(p => p.destroyed)) {
                    particles = particles.Where(particle => !particle.destroyed).ToList();
                }

                foreach (var particle in particles) {
                    particle.Step();
                }

                t++;
            }
            return particles.Count;
        }


        List<Particle> Parse(string input) {
            var lines = input.Split('\n');
            return (
                 from q in Enumerable.Zip(lines, Enumerable.Range(0, int.MaxValue), (line, i) => (i: i, line: line))
                 let nums = Regex.Matches(q.line, "-?[0-9]+").Select(m => int.Parse(m.Value)).ToArray()
                 let p = new Point(nums[0], nums[1], nums[2])
                 let v = new Point(nums[3], nums[4], nums[5])
                 let a = new Point(nums[6], nums[7], nums[8])
                 select new Particle(q.i, p, v, a))
             .ToList();
        }
    }

    class Point {
        public int x;
        public int y;
        public int z;

        public int Len() => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

        public Point(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    class Particle {
        public int i;
        public Point pos;
        public Point vel;
        public Point acc;

        public bool destroyed = false;

        public Particle(int i, Point pos, Point vel, Point acc) {
            this.i = i;
            this.pos = pos;
            this.vel = vel;
            this.acc = acc;
        }

        public void Step() {
            (vel.x, vel.y, vel.z) = (vel.x + acc.x, vel.y + acc.y, vel.z + acc.z);
            (pos.x, pos.y, pos.z) = (pos.x + vel.x, pos.y + vel.y, pos.z + vel.z);
        }

        public IEnumerable<int> CollisionTime(Particle particle) {
            return
                from tx in CollisionTimeOnAxis(particle.acc.x - acc.x, particle.vel.x - vel.x, particle.pos.x - pos.x)
                from ty in CollisionTimeOnAxis(particle.acc.y - acc.y, particle.vel.y - vel.y, particle.pos.y - pos.y)
                from tz in CollisionTimeOnAxis(particle.acc.z - acc.x, particle.vel.z - vel.z, particle.pos.z - pos.z)
                where tx == ty && ty == tz
                select (tx);
        }

        private IEnumerable<int> CollisionTimeOnAxis(int da, int dv, int dp) =>
            SolveIntEq(da / 2, dv, dp);

        private IEnumerable<int> SolveIntEq(int a, int b, int c) {
            if (a == 0) {
                if (b == 0) {
                    if (c == 0) {
                        yield return 0;
                    }
                } else {
                    yield return -c / b;
                }
            } else {
                var d = b * b - 4 * a * c;
                if (d == 0) {
                    yield return -b / (2 * a);
                } else if (d > 0) {
                    var ds = Math.Sqrt(d);
                    if (ds * ds == d) {
                        yield return (int)((-b + ds) / (2 * a));
                        yield return (int)((-b - ds) / (2 * a));
                    }

                }
            }
        }
    }
}