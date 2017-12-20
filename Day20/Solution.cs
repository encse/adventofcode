using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2017.Day20 {

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
        public override string ToString() {
            return $"({x}, {y}, {z})";
        }
    }

    class Particle {
        public int i;
        public Point p;
        public Point v;
        public Point a;

        public bool destroyed = false;

        public Particle(int i, Point p, Point v, Point a) {
            this.i = i;
            this.p = p;
            this.v = v;
            this.a = a;
        }

        public void Step() {
            (v.x, v.y, v.z) = (v.x + a.x, v.y + a.y, v.z + a.z);
            (p.x, p.y, p.z) = (p.x + v.x, p.y + v.y, p.z + v.z);
        }

        public override string ToString() {
            return $"({i}, p: {p}, v: {v}, a: {a})";
        }
    }

    class Solution : Solver {

        public string GetName() => "Particle Swarm";

        public IEnumerable<object> Solve(string input) {
            yield return PartOne(input);
            yield return PartTwo(input);
        }

        int PartOne(string input) => FinalOrder(Parse(input)).First().i;

        int PartTwo(string input) {
            var particles = Parse(input);
            var r = 0;
            while (true) {
                r++;
                var particlesByPos = (from particle in particles orderby particle.p.x, particle.p.y, particle.p.z select particle).ToArray();

                var particlePrev = particlesByPos[0];
                for (int i = 1; i < particlesByPos.Length; i++) {
                    var particle = particlesByPos[i];
                    if (particlePrev.p.x == particle.p.x && particlePrev.p.y == particle.p.y && particlePrev.p.z == particle.p.z) {
                        particlePrev.destroyed = true;
                        particle.destroyed = true;
                    }
                    particlePrev = particle;
                }

                if (particles.Any(p => p.destroyed)) {
                    particles = particles.Where(particle => !particle.destroyed).ToList();
                    Console.WriteLine(particles.Count());
                }

                foreach (var particle in particles) {
                    particle.Step();
                }

            }
        }

        List<Particle> Parse(string input) {
            var lines = input.Split('\n').Where(st => !string.IsNullOrWhiteSpace(st));
            return (
                 from q in Enumerable.Zip(lines, Enumerable.Range(0, int.MaxValue), (line, i) => (i: i, line: line))
                 let nums = Regex.Matches(q.line, "-?[0-9]+").Select(m => int.Parse(m.Value)).ToArray()
                 let p = new Point(nums[0], nums[1], nums[2])
                 let v = new Point(nums[3], nums[4], nums[5])
                 let a = new Point(nums[6], nums[7], nums[8])
                 select new Particle(q.i, p, v, a))
             .ToList();
        }

        List<Particle> FinalOrder(List<Particle> particles) {
            return (
                from particle in particles
                orderby particle.a.Len(), particle.v.Len(), particle.p.Len()
                select particle
            ).ToList();
        }

        List<Particle> DistOrder(List<Particle> particles) {
            return (
                from particle in particles
                orderby particle.p.Len()
                select particle
            ).ToList();
        }

        bool SameOrder(List<Particle> particlesA, List<Particle> particlesB) {
            return Enumerable.Range(0, particlesA.Count).All(i => particlesA[i].i == particlesB[i].i);
        }
    }
}