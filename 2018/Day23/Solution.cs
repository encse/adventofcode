using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2018.Day23;

[ProblemName("Experimental Emergency Teleportation")]
class Solution : Solver {

    int Dist((int x, int y, int z) a, (int x, int y, int z) b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);

    public object PartOne(string input) {
        var drones = Parse(input);
        var maxRange = drones.Select(drone => drone.r).Max();
        var maxDrone = drones.Single(drone => drone.r == maxRange);
        return drones.Count(drone => Dist(drone.pos, maxDrone.pos) <= maxRange);
    }

    IEnumerable<(int x, int y, int z)> Corners(Drone[] drones) => (
        from drone in drones
        from dx in new[] { -1, 0, 1 }
        from dy in new[] { -1, 0, 1 }
        from dz in new[] { -1, 0, 1 }
        where dx * dx + dy * dy + dz * dz == 1
        select (drone.pos.x + dx * drone.r, drone.pos.y + dy * drone.r, drone.pos.z + dz * drone.r)
    ).ToArray();

    Drone[] Parse(string input) => (
        from line in input.Split("\n")
        let parts = Regex.Matches(line, @"-?\d+").Select(x => int.Parse(x.Value)).ToArray()
        select new Drone((parts[0], parts[1], parts[2]), parts[3])
    ).ToArray();

    public object PartTwo(string input) {
        var drones = Parse(input);
        var minX = drones.Select(drone => drone.pos.x).Min();
        var minY = drones.Select(drone => drone.pos.y).Min();
        var minZ = drones.Select(drone => drone.pos.z).Min();

        var maxX = drones.Select(drone => drone.pos.x).Max();
        var maxY = drones.Select(drone => drone.pos.y).Max();
        var maxZ = drones.Select(drone => drone.pos.z).Max();

        return Solve(new Box((minX, minY, minZ), (maxX - minX + 1, maxY - minY + 1, maxZ - minZ + 1)), drones).pt;
    }

    (int drones, int pt) Solve(Box box, Drone[] drones) {

        var q = new PQueue<(int, int), (Box box, Drone[] drones)>();
        q.Enqueue((0, 0), (box, drones));

        while (q.Any()) {
            (box, drones) = q.Dequeue();

            if (box.Size() == 1) {
                return (drones.Count(drone => drone.Contains(box)), box.Dist());
            } else {
                foreach (var subBox in box.Divide()) {
                    var intersectingDrones = drones.Where(drone => drone.Intersects(subBox)).ToArray();
                    q.Enqueue((-intersectingDrones.Count(), subBox.Dist()), (subBox, intersectingDrones));
                }
            }
        }
        throw new Exception();
    }

}

class Box {
    public readonly (int x, int y, int z) min;
    public readonly (int x, int y, int z) max;
    private readonly (int sx, int sy, int sz) size;
    public Box((int x, int y, int z) min, (int sx, int sy, int sz) size) {
        this.min = min;
        this.max = (min.x + size.sx - 1, min.y + size.sy - 1, min.z + size.sz - 1);
        this.size = size;
    }

    public IEnumerable<(int x, int y, int z)> Corners() {
        yield return (min.x, min.y, min.z);
        yield return (max.x, min.y, min.z);
        yield return (min.x, max.y, min.z);
        yield return (max.x, max.y, min.z);

        yield return (min.x, min.y, max.z);
        yield return (max.x, min.y, max.z);
        yield return (min.x, max.y, max.z);
        yield return (max.x, max.y, max.z);
    }
    
    public IEnumerable<Box> Divide() {
        var sx = size.sx / 2;
        var tx = size.sx - sx; 
        var sy = size.sy / 2;
        var ty = size.sy - sy;
        var sz = size.sz / 2;
        var tz = size.sz - sz;

        return new[]{
            new Box((min.x,      min.y,       min.z     ), (sx, sy, sz)),
            new Box((min.x + sx, min.y,       min.z     ), (tx, sy, sz)),
            new Box((min.x,      min.y + sy,  min.z     ), (sx, ty, sz)),
            new Box((min.x + sx, min.y + sy,  min.z     ), (tx, ty, sz)),

            new Box((min.x,      min.y,       min.z + sz), (sx, sy, tz)),
            new Box((min.x + sx, min.y,       min.z + sz), (tx, sy, tz)),
            new Box((min.x,      min.y + sy,  min.z + sz), (sx, ty, tz)),
            new Box((min.x + sx, min.y + sy,  min.z + sz), (tx, ty, tz)),

        }.Where(box => box.size.sx > 0 && box.size.sy > 0 && box.size.sz > 0);
    }

    public int Dist() {
        return Corners().Select(pt => Math.Abs(pt.x) + Math.Abs(pt.y) + Math.Abs(pt.z)).Min();
    }

    public BigInteger Size() {
        return (BigInteger)size.sx * (BigInteger)size.sy * (BigInteger)size.sz;
    }
}

class Drone {
    public readonly (int x, int y, int z) pos;
    public readonly int r;
    public readonly Box box;
    public Drone((int x, int y, int z) pos, int r) {
        this.pos = pos;
        this.r = r;
        box = new Box((pos.x - r, pos.y - r, pos.z - r), (2 * r + 1, 2 * r + 1, 2 * r + 1));
    }

    public bool Intersects(Box box) {
        var dx = Math.Max(0, Math.Max(box.min.x - pos.x, pos.x - box.max.x));
        var dy = Math.Max(0, Math.Max(box.min.y - pos.y, pos.y - box.max.y));
        var dz = Math.Max(0, Math.Max(box.min.z - pos.z, pos.z - box.max.z));

        return Math.Abs(dx) + Math.Abs(dy) + Math.Abs(dz) <= r;
    }

    public bool Contains(Box box) {
        return box
            .Corners()
            .All(pt => Math.Abs(pt.x - pos.x) + Math.Abs(pt.y - pos.y) + Math.Abs(pt.z - pos.z) <= r);
    }
}

class PQueue<K, T> where K : IComparable {
    SortedDictionary<K, Queue<T>> d = new SortedDictionary<K, Queue<T>>();
    int c = 0;
    public bool Any() {
        return d.Any();
    }

    public void Enqueue(K p, T t) {
        if (!d.ContainsKey(p)) {
            d[p] = new Queue<T>();
        }
        d[p].Enqueue(t);
        c++;
    }

    public T Dequeue() {
        c--;
        var p = d.Keys.First();
        var items = d[p];
        var t = items.Dequeue();
        if (!items.Any()) {
            d.Remove(p);
        }
        return t;
    }

    public int Count() {
        return c;
    }
}
