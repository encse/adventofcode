using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Y2022.Day19;

[ProblemName("Not Enough Minerals")]
class Solution : Solver {

    public object PartOne(string input) {
        var res = 0;
        foreach (var blueprint in Parse(input).Where(bp => bp.id < 100)) {
            var m = MaxGeodes(blueprint, 24);
            res += blueprint.id * m;
        }
        return res;
    }

    public object PartTwo(string input) {
        var res = 1;
        foreach (var blueprint in Parse(input).Where(bp => bp.id <= 3)) {
            var m = MaxGeodes(blueprint, 32);
            res *= m;
        }
        return res;
    }

    // Priority queue based maximum search with LOTS OF PRUNING
    private int MaxGeodes(Blueprint blueprint, int timeLimit) {
        var q = new PriorityQueue<State, int>();
        var seen = new HashSet<State>();

        enqueue(new State(
            remainingTime: timeLimit, 
            available: Nothing, 
            producing: Ore, 
            dontBuild: 0
        ));

        var max = 0;
        while (q.Count > 0) {
            var state = q.Dequeue();

            // Queue is ordered by potentialGeodeCount, there is
            // no point in investigating the remaining items.
            if (potentialGeodeCount(state) < max) {
                break;
            }

            if (!seen.Contains(state)) {
                seen.Add(state);

                if (state.remainingTime == 0) {
                    // time is off, just update max
                    max = Math.Max(max, state.available.geode);
                } else {
                    // What robots can be created from the available materials?
                    var buildableRobots = blueprint.robots
                        .Where(robot => state.available >= robot.cost)
                        .ToArray();

                    // 1) build one of them right away
                    foreach (var robot in buildableRobots) {
                        if (worthBuilding(state, robot)) {
                            enqueue(state with {
                                remainingTime = state.remainingTime - 1,
                                available = state.available + state.producing - robot.cost,
                                producing = state.producing + robot.producing,
                                dontBuild = 0
                            });
                        }
                    }

                    // 2) or wait until next round for more robot types. Don't postpone
                    //    building of robots which are already available. This is a very
                    //    very important prunning step. It's about 25 times faster if we 
                    //    do it this way.
                    enqueue(
                        state with {
                            remainingTime = state.remainingTime - 1,
                            available = state.available + state.producing,
                            dontBuild = buildableRobots.Select(robot => robot.id).Sum(),
                        }
                    );
                }
            }
        }

        return max;

        // ------- 

        // Upper limit for the maximum geodes we reach when starting from this state. 
        // Let's be optimistic and suppose that in each step we will be able to build 
        // a new geode robot...
        int potentialGeodeCount(State state) {
            // sum of [state.producing.geode .. state.producing.geode + state.remainingTime - 1]
            var future = 
                (2 * state.producing.geode + state.remainingTime - 1) * state.remainingTime / 2;
            return state.available.geode + future;
        }

        bool worthBuilding(State state, Robot robot) {
            // We can explicitly ignore building some robots. 
            // Robot ids are powers of 2 used as flags in the dontBuild integer.
            if ((state.dontBuild & robot.id) != 0) {
                return false;
            }

            // Our factory can build just a single robot in a round. This gives as 
            // a prunning condition. Producing more material in a round that we can 
            // spend on building a new robot is worthless.
            return state.producing + robot.producing <= blueprint.maxCost;
        }

        // Just add an item to the search queue, use -potentialGeodeCount as priority 
        void enqueue(State state) {
            q.Enqueue(state, -potentialGeodeCount(state));
        }
    }

    IEnumerable<Blueprint> Parse(string input) {
        foreach (var line in input.Split("\n")) {
            var numbers = Regex.Matches(line, @"(\d+)").Select(x => int.Parse(x.Value)).ToArray();
            yield return new Blueprint(
                id: numbers[0],
                new Robot(id: 1, producing: Ore, cost: numbers[1] * Ore),
                new Robot(id: 2, producing: Clay, cost: numbers[2] * Ore),
                new Robot(id: 4, producing: Obsidian, cost: numbers[3] * Ore + numbers[4] * Clay),
                new Robot(id: 8, producing: Geode, cost: numbers[5] * Ore + numbers[6] * Obsidian)
            );
        }
    }

    static Material Nothing = new Material(0, 0, 0, 0);
    static Material Ore = new Material(1, 0, 0, 0);
    static Material Clay = new Material(0, 1, 0, 0);
    static Material Obsidian = new Material(0, 0, 1, 0);
    static Material Geode = new Material(0, 0, 0, 1);

    record Material(int ore, int clay, int obsidian, int geode) {
        public static Material operator *(int m, Material a) {
            return new Material(m * a.ore, m * a.clay, m * a.obsidian, m * a.geode);
        }
        public static Material operator +(Material a, Material b) {
            return new Material(
                a.ore + b.ore,
                a.clay + b.clay,
                a.obsidian + b.obsidian,
                a.geode + b.geode
            );
        }

        public static Material operator -(Material a, Material b) {
            return new Material(
                a.ore - b.ore,
                a.clay - b.clay,
                a.obsidian - b.obsidian,
                a.geode - b.geode
            );
        }

        public static bool operator <=(Material a, Material b) {
            return
                a.ore <= b.ore &&
                a.clay <= b.clay &&
                a.obsidian <= b.obsidian &&
                a.geode <= b.geode;
        }

        public static bool operator >=(Material a, Material b) {
            return
                a.ore >= b.ore &&
                a.clay >= b.clay &&
                a.obsidian >= b.obsidian &&
                a.geode >= b.geode;
        }
    }

    record Robot(int id, Material cost, Material producing);
    record State(int remainingTime, Material available, Material producing, int dontBuild);
    record Blueprint(int id, params Robot[] robots) {
        public Material maxCost = new Material(
            ore: robots.Select(robot => robot.cost.ore).Max(),
            clay: robots.Select(robot => robot.cost.clay).Max(),
            obsidian: robots.Select(robot => robot.cost.obsidian).Max(),
            geode: int.MaxValue
        );
    }
}
